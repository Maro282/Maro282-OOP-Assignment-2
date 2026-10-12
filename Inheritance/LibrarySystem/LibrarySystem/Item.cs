

namespace LibrarySystem;

public abstract class Item
{

    public string CatalogNumber { get; }
    public string Title { get; }
    private decimal _baseLateFee;
    private bool _isWithdrawn;
    private bool _isOnloan;
    public bool IsWithdrawn => _isWithdrawn;
    public bool IsOnLoan => _isOnloan;
    public abstract int LoanPeriodDays { get; }
    public decimal BaseLateFee => _baseLateFee;

    protected Item(string catalogNumber, string title, decimal baseLateFee)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(catalogNumber);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(title);
        if (baseLateFee < 0)
            throw new ArgumentOutOfRangeException(nameof(baseLateFee), "BaseLateFee can't be negative");

        _baseLateFee = baseLateFee;
        Title = title;
        CatalogNumber = catalogNumber;
    }


    public abstract decimal GetDailyLateFee();


    //will be called when use it in the HeadLiberarian Class in the setLateFee method
    public void SetBaseLateFee(decimal newBaseLateFee)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(newBaseLateFee, "Late fee can't be negative");
        _baseLateFee = newBaseLateFee;
    }


    // If the manager make the item withdrawn
    public void Withdraw() => _isWithdrawn = true;
    public void Restore() => _isWithdrawn = false;


    // These function will be called in the Borrow method
    internal void MarkedAsBorrowed() => _isOnloan = true;
    internal void MarkedAsReturned() => _isOnloan = false;

}