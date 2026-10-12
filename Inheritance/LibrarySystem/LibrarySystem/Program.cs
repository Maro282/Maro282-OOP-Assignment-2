namespace LibrarySystem;

public class Program
{
    static void Main(string[] args)
    {
        int passed = 0, failed = 0;

        void Section(string title) => Console.WriteLine($"\n=== {title} ===");

        // The action must throw; otherwise the test fails
        void ExpectRejected(string name, Action action)
        {
            try
            {
                action();
                Console.WriteLine($"  [FAIL] {name} - was NOT rejected");
                failed++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [PASS] {name} - rejected: {ex.Message}");
                passed++;
            }
        }

        void Check(string name, bool condition)
        {
            Console.WriteLine($"  [{(condition ? "PASS" : "FAIL")}] {name}");
            if (condition) passed++; else failed++;
        }

        // ---------- shared test data ----------
        var hire = new DateTime(2020, 1, 1);
        var head = new HeadLibrarian("H1", "Hana", "0100000001", hire, 5000m);
        var librarian = new Librarian("L1", "Laila", "0100000002", hire, 3000m);
        var shelver = new Shelver("S1", "Sara", "0100000003", hire, 2500m, "Fiction");

        // =====================================================
        // 1) Things that must NOT compile (uncomment one line to check)
        // =====================================================
        Section("1) Must NOT compile (commented out)");
        var student = new StudentMember("M1", "Student Sam", "0100000010");
        Console.WriteLine("  Uncomment each line below and the build must fail:");

        // var p = new Person("P1", "Plain", "0100");                  // must NOT compile (Person is abstract)
        // var m = new Member("M9", "Plain Member", "0100");           // must NOT compile (Member is abstract)
        // var s = new StaffMember("S9", "Plain", "0100", hire, 1m);   // must NOT compile (StaffMember is abstract)
        // var i = new Item("X1", "Plain Item", 1m);                   // must NOT compile (Item is abstract)
        // student.Name = "New Name";                                  // must NOT compile (Name has no setter)
        // student.LoanHistory.Add(null);                              // must NOT compile (IReadOnlyList has no Add)
        // var someBook = new Book("B-0", "Some Book", 1m);
        // someBook.IsOnLoan = true;                                   // must NOT compile (IsOnLoan has no setter)

        // =====================================================
        // 2) Borrowing rules
        // =====================================================
        Section("2) Borrow rules");

        // a) withdrawn item
        var withdrawn = new Book("B-100", "Withdrawn Book", 2m);
        head.WithdrawItem(withdrawn);
        ExpectRejected("Borrow a withdrawn item", () => student.Borrow(withdrawn));

        // b) item already on loan
        var busy = new Book("B-101", "Busy Book", 2m);
        var student2 = new StudentMember("M2", "Student Mona", "0100000011");
        student2.Borrow(busy);
        ExpectRejected("Borrow an item that is already on loan", () => student.Borrow(busy));

        // c) student borrowing a 4th item (limit is 3)
        var student3 = new StudentMember("M3", "Student Omar", "0100000012");
        student3.Borrow(new Book("B-201", "Book 1", 2m));
        student3.Borrow(new DVD("D-201", "DVD 1", 2m));
        student3.Borrow(new Magazine("MG-201", "Magazine 1", 2m));
        Check("Student borrowed 3 items", student3.GetActiveLoanCount() == 3);
        ExpectRejected("Student borrows a 4th item", () => student3.Borrow(new Book("B-202", "Book 4", 2m)));

        // =====================================================
        // 3) Staff polymorphism
        // =====================================================
        Section("3) Monthly pay (List<StaffMember>)");
        List<StaffMember> staff = new() { librarian, head, shelver };
        foreach (var s in staff)
            Console.WriteLine($"  {s.GetType().Name,-14} {s.Name,-6} monthly pay = {s.GetMonthlyPay():0.00}");
        Check("Head Librarian gets salary + 400", head.GetMonthlyPay() == 5400m);

        // =====================================================
        // 4) Item polymorphism
        // =====================================================
        Section("4) Loan period and daily late fee (List<Item>)");
        List<Item> items = new()
{
    new Book("B-300", "Test Book", 2m),
    new DVD("D-300", "Test DVD", 2m),
    new Magazine("MG-300", "Test Magazine", 2m)
};
        foreach (var it in items)
            Console.WriteLine($"  {it.GetType().Name,-9} loan period = {it.LoanPeriodDays,2} days, daily late fee = {it.GetDailyLateFee():0.00}");

        // =====================================================
        // 5) Premium member returns a DVD 5 days late
        // =====================================================
        Section("5) Premium member, DVD returned 5 days late");
        var premium = new PremiumMember("M4", "Premium Pam", "0100000013", 20m);   // 20% discount
        var dvd = new DVD("D-400", "Late DVD", 10m);                                // daily fee = 10 x 2 = 20
        var loan = premium.Borrow(dvd);
        var returnDate = loan.DueDate.AddDays(5);
        decimal fee = librarian.ProcessReturn(loan, returnDate);
        Console.WriteLine($"  Due date       : {loan.DueDate:yyyy-MM-dd}");
        Console.WriteLine($"  Return date    : {returnDate:yyyy-MM-dd}");
        Console.WriteLine($"  Late fee       : {fee:0.00}   (5 days x 20 = 100, minus 20% = 80)");
        Console.WriteLine($"  Reading points : {premium.ReadingPoints}");
        Check("Late fee is 80", fee == 80m);
        Check("Premium member has 5 reading points", premium.ReadingPoints == 5);
        Check("Item is available again", !dvd.IsOnLoan);

        // =====================================================
        // 6) Invalid status changes
        // =====================================================
        Section("6) Return twice / lost after returned");
        var student4 = new StudentMember("M5", "Student Lina", "0100000014");

        var loanA = student4.Borrow(new Book("B-500", "Book A", 2m));
        librarian.ProcessReturn(loanA, DateTime.Now);
        ExpectRejected("Return the same loan twice", () => librarian.ProcessReturn(loanA, DateTime.Now));

        var loanB = student4.Borrow(new Book("B-501", "Book B", 2m));
        librarian.ProcessReturn(loanB, DateTime.Now);
        ExpectRejected("Mark a returned loan as lost", () => librarian.MarkAsLost(loanB));

        // ---------- summary ----------
        Console.WriteLine($"\nPassed: {passed}   Failed: {failed}");

    }
}
