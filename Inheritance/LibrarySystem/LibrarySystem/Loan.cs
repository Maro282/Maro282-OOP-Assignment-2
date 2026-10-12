

namespace LibrarySystem;

public class Loan
{

    public string LoanId { get; }
    public DateTime BorrowDate { get; }
    public DateTime? ReturnDate { get; private set; }
    public LoanStatus Status { get; private set; }
    public Item Item { get; }
    public Member Member { get; }

    public DateTime DueDate { get; }


    internal Loan(string loanId, DateTime borrowDate, Member member, Item item)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(loanId, "LoanId can't be empty");
        if (member is null)
            throw new ArgumentNullException("Member Can't be null");
        if (item is null)
            throw new ArgumentNullException("item Can't be null or empty");


        LoanId = loanId;
        BorrowDate = borrowDate;
        Member = member;
        Item = item;
        Status = LoanStatus.Borrowed;
        DueDate = BorrowDate.AddDays(Item.LoanPeriodDays);

    }



    //Used by liberarian to make it lost
    internal void MarkLost()
    {

        if (Status == LoanStatus.Lost || Status == LoanStatus.Returned)
            throw new InvalidOperationException("Loan can't marked as lost while it lost or returned");

        Status = LoanStatus.Lost;
    }
    internal void MarkReturned(DateTime returnDate)
    {


        if (Status == LoanStatus.Returned)
            throw new InvalidOperationException("Loan can't marked as returned while it is already returned");

        if (returnDate.Date < BorrowDate.Date)
            throw new ArgumentOutOfRangeException("Return date can't be before borrow date");



        Status = LoanStatus.Returned;
        ReturnDate = returnDate;
        Item.MarkedAsReturned();
        Member.OnLoanReturn();
    }

    public decimal CalculateLatefee()
    {

        if (ReturnDate is null)
            return 0m;

        int daysLate = (ReturnDate.Value.Date - DueDate.Date).Days;

        if (daysLate <= 0)
            return 0m;

        decimal fee = daysLate * Item.GetDailyLateFee();

        return fee - fee * Member.DiscountPercentage / 100m;
    }


}


public enum LoanStatus
{
    Borrowed = 1,
    Returned,
    Lost
}