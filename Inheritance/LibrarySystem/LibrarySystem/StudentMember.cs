

namespace LibrarySystem;

public class StudentMember : Member
{
    public override int MaxLoans => 3;

    public override decimal DiscountPercentage => 0;

    public StudentMember(string personId, string fullName, string phone) : base(personId, fullName, phone)
    { }


}
