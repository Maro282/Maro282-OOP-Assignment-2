

namespace LibrarySystem;

public class Librarian : StaffMember
{

    public Librarian(string personId, string name, string phone, DateTime hireDate, decimal salary)
: base(personId, name, phone, hireDate, salary)
    {

    }

    public decimal ProcessReturn(Loan loan, DateTime returnDate)
    {
        if (loan is null) throw new ArgumentNullException("loan is null");
        loan.MarkReturned(returnDate);
        return loan.CalculateLatefee();


    }

    public void MarkAsLost(Loan loan)
    {
        if (loan is null) throw new ArgumentNullException("loan is null");
        loan.MarkLost();
    }



    public override decimal GetMonthlyPay()
    {
        return Salary;
    }
}