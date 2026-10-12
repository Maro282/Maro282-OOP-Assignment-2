

namespace LibrarySystem;

public class HeadLibrarian : StaffMember
{

    private const decimal ResponsibilityAllownce = 400;

    public HeadLibrarian(string personId, string name, string phone, DateTime hireDate, decimal salary) : base(personId, name, phone, hireDate, salary)
    {


    }

    public void WithdrawItem(Item item)
    {
        if (item is null)
            throw new ArgumentNullException("item is null");
        item.Withdraw();
    }
    public void RestoreItem(Item item)
    {
        if (item is null)
            throw new ArgumentNullException("item is null");
        item.Restore();
    }

    public void SetLateFee(Item item, decimal newFee)
    {
        if (item is null)
            throw new ArgumentNullException("item is null");
        item.SetBaseLateFee(newFee);

    }

    public override decimal GetMonthlyPay()
    {
        return Salary + ResponsibilityAllownce;
    }
}

