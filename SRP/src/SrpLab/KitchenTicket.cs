namespace SrpLab;

/// <summary>
/// Kitchen ticket: allergen scan from ingredients, cook ETA heuristics, and printed ticket layout.
/// </summary>
public sealed class KitchenTicket
{
    private readonly List<(string Item, List<string> Ingredients, int PrepMinutes)> _items = new();

    public List<(string Item, List<string> Ingredients, int PrepMinutes)> Items => _items;


    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        _items.Add((item, ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList(), prepMinutes));
    }



}


public class AllergensDetector
{

    private readonly KitchenTicket _kitchenTicket;

    public AllergensDetector(KitchenTicket kitchenTicket)
    {
        _kitchenTicket = kitchenTicket;
    }

    public IReadOnlyList<string> DetectAllergens()
    {
        // Regulatory allergen dictionary changes separately from ticket layout.
        var hits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (_, ingredients, _) in _kitchenTicket.Items)
        {
            foreach (var ing in ingredients)
            {
                if (ing.Contains("milk") || ing.Contains("cheese") || ing.Contains("butter")) hits.Add("dairy");
                if (ing.Contains("wheat") || ing.Contains("flour") || ing.Contains("bread")) hits.Add("gluten");
                if (ing.Contains("peanut") || ing.Contains("almond") || ing.Contains("cashew")) hits.Add("nuts");
                if (ing.Contains("shrimp") || ing.Contains("prawn") || ing.Contains("crab")) hits.Add("shellfish");
            }
        }
        return hits.OrderBy(x => x).ToList();
    }

}



public class OrderServices
{

    private readonly KitchenTicket? _kitchenTicket;
    private readonly AllergensDetector _allergensDetector;

    public OrderServices(KitchenTicket kitchen, AllergensDetector allergensDetector)
    {
        _allergensDetector = allergensDetector;
        _kitchenTicket = kitchen;
    }


    public int EstimatedReadyMinutes(int openStations)
    {
        // Kitchen ops model ≠ printing.
        if (openStations <= 0) openStations = 1;
        var sequential = _kitchenTicket.Items.Sum(i => i.PrepMinutes);
        var parallel = (int)Math.Ceiling(sequential / (double)openStations);
        if (_allergensDetector.DetectAllergens().Count > 0) parallel += 3; // allergy protocol delay mixed in
        var longest = _kitchenTicket.Items.Count == 0 ? 0 : _kitchenTicket.Items.Max(i => i.PrepMinutes);
        return Math.Max(parallel, longest);
    }


    public string ExpoLaneHint()
    {
        return _allergensDetector.DetectAllergens().Count > 0 ? "LANE-ALLERGY" : EstimatedReadyMinutes(2) > 20 ? "LANE-SLOW" : "LANE-FAST";
    }
}



// Printing 
public class ThermalTicketRender
{

    private readonly KitchenTicket _kitchenTicket;
    private readonly OrderServices _orderServices;
    private readonly AllergensDetector _allergensDetector;


    public ThermalTicketRender(KitchenTicket kitchenTicket, OrderServices orderServices, AllergensDetector allergensDetector)
    {
        _kitchenTicket = kitchenTicket;
        _orderServices = orderServices;
        _allergensDetector = allergensDetector;

    }


    public string RenderThermalTicket(int orderNumber)
    {
        // Hardware/formatting concerns — width, separators — change with printer vendor.
        var width = 32;
        var line = new string('=', width);
        var body = string.Join('\n', _kitchenTicket.Items.Select(i => $"* {i.Item.ToUpperInvariant()} ({i.PrepMinutes}m)"));
        var allergens = _allergensDetector.DetectAllergens();
        var allergyLine = allergens.Count == 0 ? "ALLERGENS: none" : "ALLERGENS: " + string.Join(",", allergens);
        return $"{line}\nORDER #{orderNumber}\nETA {_orderServices.EstimatedReadyMinutes(2)} MIN\n{body}\n{allergyLine}\n{line}\n";
    }
}
