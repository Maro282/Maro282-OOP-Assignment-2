
namespace LibrarySystem;

public class Book : Item
{
    public override int LoanPeriodDays => 21;

    public Book(string catalogNumber, string title, decimal baseLateFee) : base(catalogNumber, title, baseLateFee)
    {

    }

    public override decimal GetDailyLateFee() => BaseLateFee;





}
