

namespace LibrarySystem;

public class PremiumMember : Member
{

    public override int MaxLoans => 10;
    public int ReadingPoints { get; private set; }
    public override decimal DiscountPercentage { get; }

    public PremiumMember(string personId, string fullName, string phone, decimal discount) : base(personId, fullName, phone)
    {

        if (discount < 0 || discount > 100)
            throw new ArgumentOutOfRangeException("Discount can't be negative or over 100");
        DiscountPercentage = discount;
    }

    internal override void OnLoanReturn()
    {

        ReadingPoints += 5;
    }

}