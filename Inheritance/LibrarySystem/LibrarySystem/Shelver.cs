
namespace LibrarySystem;

public class Shelver : StaffMember
{
    public string Section { get; private set; }

    public Shelver(string personId, string name, string phone, DateTime hireDate, decimal salary, string section)
    : base(personId, name, phone, hireDate, salary)
    {

        ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(section), "Section can't be null or empty");
        Section = section;
    }

    public override decimal GetMonthlyPay()
    {
        return Salary;
    }

    public void Reassign(string section)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(section);
        Section = section;
    }
}