namespace SrpLab;

/// <summary>
/// Warehouse pick list: naive path order, stock allocation, and picker instruction prose.
/// </summary>
public sealed class WarehousePickList
{
    private readonly List<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines = new();
    public List<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> Lines => _lines;

    // Determine needed product with it's details
    public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand)
    {
        _lines.Add((sku, aisle, bin, qtyNeeded, qtyOnHand));
    }


}

// return list of products with actuall quantity can be picked

public class ReAllocation
{
    private readonly WarehousePickList _wareHouseList;
    public ReAllocation(WarehousePickList wareHouseList)
    {

        _wareHouseList = wareHouseList;
    }


    public IReadOnlyList<(string Sku, int Allocated)> Allocate()
    {
        // Allocation/backorder policy ≠ walking path ≠ human instructions.
        var result = new List<(string, int)>();
        foreach (var line in _wareHouseList.Lines)
        {
            var alloc = Math.Min(line.QtyNeeded, line.QtyOnHand);
            result.Add((line.Sku, alloc));
        }
        return result;
    }
}



// Determine where the worker should walk and in what order
public class WhereToGo
{

    private readonly WarehousePickList _wareHouseList;
    public WhereToGo(WarehousePickList wareHouseList)
    {

        _wareHouseList = wareHouseList;
    }

    public IReadOnlyList<(string Aisle, int Bin, string Sku, int Qty)> WalkingOrder()
    {
        // Path heuristic will change with warehouse layout tech.
        return _wareHouseList.Lines
            .OrderBy(l => l.Aisle)
            .ThenBy(l => l.Bin)
            .Select(l => (l.Aisle, l.Bin, l.Sku, Math.Min(l.QtyNeeded, l.QtyOnHand)))
            .Where(x => x.Item4 > 0)
            .ToList();
    }
}


// Creating a script for worker to read
public class Scripting
{

    private readonly WarehousePickList _wareHouseList;
    private readonly WhereToGo _whereToGo;
    private readonly ReAllocation _allocator;

    public Scripting(WarehousePickList wareHouseList, WhereToGo whereToGo, ReAllocation allocator)
    {

        _wareHouseList = wareHouseList;
        _whereToGo = whereToGo;
        _allocator = allocator;
    }



    public string PickerScript()
    {
        // UX wording for handheld devices — separate owners.
        var steps = _whereToGo.WalkingOrder()
            .Select((s, i) => $"{i + 1}. Go aisle {s.Aisle} bin {s.Bin}: pick {s.Qty} × {s.Sku}");
        var shortfalls = _allocator.Allocate().Where(a =>
        {
            var need = _wareHouseList.Lines.First(l => l.Sku == a.Sku).QtyNeeded;
            return a.Allocated < need;
        });
        var warn = shortfalls.Any()
            ? "SHORTAGES: " + string.Join(", ", shortfalls.Select(s => s.Sku))
            : "SHORTAGES: none";
        return string.Join('\n', steps) + "\n" + warn;
    }
}


// Create XML batch

public class CreateXML
{
    private readonly WarehousePickList _wareHouseList;
    private readonly ReAllocation _allocator;
    public CreateXML(WarehousePickList wareHouseList, ReAllocation allocator)
    {

        _wareHouseList = wareHouseList;
        _allocator = allocator;
    }


    public string WmsXmlBatch(string batchId)
    {
        // Integration contract with WMS — third reason to change.
        var parts = _allocator.Allocate().Select(a => $"<line sku=\"{a.Sku}\" qty=\"{a.Allocated}\" />");
        return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
    }
}