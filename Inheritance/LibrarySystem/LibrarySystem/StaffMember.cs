

namespace LibrarySystem;

public abstract class StaffMember : Person
{

    public DateTime HireDate { get; }
    public decimal Salary { get; private set; }

    protected StaffMember(string personId, string name, string phone, DateTime hireDate, decimal salary) : base(personId, name, phone)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(hireDate), "Hire date can't be empty");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(salary, "Hire date can't be empty");
        HireDate = hireDate;
        Salary = salary;

    }

    public void GiveRaise(decimal percentage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(percentage, "Hire date can't be empty");
        Salary = Salary + (Salary * (percentage / 100));
    }

    public abstract decimal GetMonthlyPay();
}