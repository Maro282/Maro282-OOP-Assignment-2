

namespace LibrarySystem;

public abstract class Person
{
    public string PersonId { get; }
    public string Name { get; }
    public string Phone { get; }


    public Person(string personId, string name, string phone)
    {

        PersonId = personId;
        Name = name;
        Phone = phone;
    }

}
