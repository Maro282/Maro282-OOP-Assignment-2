
namespace LibrarySystem;

public abstract class Member : Person
{


    private readonly List<Loan> _loanHistory = new();
    public IReadOnlyList<Loan> LoanHistory { get; }
    public abstract int MaxLoans { get; }
    public abstract decimal DiscountPercentage { get; }


    public Member(string personId, string name, string phone) : base(personId, name, phone)
    {


    }

    // Need implementation
    public Loan Borrow(Item item)
    {

        if (item.IsWithdrawn)
            throw new InvalidOperationException("Item is Withdrawn");
        if (item.IsOnLoan)
            throw new InvalidOperationException("Item already borrowed");

        if (GetActiveLoanCount() >= this.MaxLoans)
            throw new InvalidOperationException("YOU HAVE EXCEEDE MAX LOANS NUMBER");

        item.MarkedAsBorrowed(); // on loan will be true
        var loan = new Loan(Guid.NewGuid().ToString(), DateTime.Now, this, item);
        _loanHistory.Add(loan);
        return loan;
    }

    public int GetActiveLoanCount()
    {
        int count = 0;
        foreach (var loan in _loanHistory)
        {
            if (loan.Status == LoanStatus.Borrowed)
                count++;
        }
        return count;
    }

    internal virtual void OnLoanReturn() { }



}