
namespace LibrarySystem;

public class Magazine : Item
{

    public override int LoanPeriodDays => 3;

    public Magazine(string catalogNumber, string title, decimal baseLateFee) : base(catalogNumber, title, baseLateFee) { }

    public override decimal GetDailyLateFee() => BaseLateFee * 0.5m;
}
