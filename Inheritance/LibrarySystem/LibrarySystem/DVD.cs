

namespace LibrarySystem;

public class DVD : Item
{

    public override int LoanPeriodDays => 7;

    public DVD(string catalogNumber, string title, decimal baseLateFee) : base(catalogNumber, title, baseLateFee) { }

    public override decimal GetDailyLateFee() => BaseLateFee * 2;



}