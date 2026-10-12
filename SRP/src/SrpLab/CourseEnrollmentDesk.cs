namespace SrpLab;

/// <summary>
/// Course enrollment: capacity, waitlist math, welcome-packet markdown, and invoice lines.
/// </summary>
public sealed class CourseEnrollmentDesk
{
    private readonly HashSet<string> _seated = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _waitlist = new();

    public HashSet<string> Seated => _seated;
    public int Capacity { get; }
    public decimal Tuition { get; }
    public string CourseCode { get; }

    public CourseEnrollmentDesk(string courseCode, int capacity, decimal tuition)
    {
        CourseCode = courseCode;
        Capacity = capacity;
        Tuition = tuition;
    }

    public string Register(string studentEmail)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email");
        var email = studentEmail.Trim();

        if (_seated.Contains(email) || _waitlist.Contains(email))
            return "ALREADY_REGISTERED";

        if (_seated.Count < Capacity)
        {
            _seated.Add(email);
            return "SEATED";
        }

        _waitlist.Add(email);
        return $"WAITLIST:{_waitlist.Count}";
    }

    public int WaitlistPosition(string studentEmail)
    {
        var idx = _waitlist.FindIndex(x => x.Equals(studentEmail, StringComparison.OrdinalIgnoreCase));
        return idx < 0 ? -1 : idx + 1;
    }

    public void PromoteFromWaitlist(int seats)
    {
        // Operational promotion policy mixed with messaging responsibilities above.
        while (seats > 0 && _waitlist.Count > 0 && _seated.Count < Capacity)
        {
            var next = _waitlist[0];
            _waitlist.RemoveAt(0);
            _seated.Add(next);
            seats--;
        }
    }
}


public class CalculateCourseTuition
{


    private readonly CourseEnrollmentDesk _courseEnrollmentDesk;
    public CalculateCourseTuition(CourseEnrollmentDesk courseEnrollment)
    {
        _courseEnrollmentDesk = courseEnrollment;
    }




    public string TuitionInvoiceLine(string studentEmail)
    {
        // Finance formatting / tax later — separate from enrollment capacity.
        if (!_courseEnrollmentDesk.Seated.Contains(studentEmail)) return $"{_courseEnrollmentDesk.CourseCode},WAITLIST,0.00";
        var vat = Math.Round(_courseEnrollmentDesk.Tuition * 0.14m, 2);
        return $"{_courseEnrollmentDesk.CourseCode},TUITION,{_courseEnrollmentDesk.Tuition:0.00},VAT,{vat:0.00},TOTAL,{(_courseEnrollmentDesk.Tuition + vat):0.00}";
    }


}





public class WelcomingStudent
{

    private readonly CourseEnrollmentDesk _courseEnrollmentDesk;
    public WelcomingStudent(CourseEnrollmentDesk courseEnrollment)
    {
        _courseEnrollmentDesk = courseEnrollment;
    }


    public string WelcomePacketMarkdown(string studentEmail, string studentName)
    {
        // Content design changes with academy marketing — not with seat algorithms.
        var status = _courseEnrollmentDesk.Seated.Contains(studentEmail) ? "confirmed seat" : $"waitlist #{_courseEnrollmentDesk.WaitlistPosition(studentEmail)}";
        return $"# Welcome to {_courseEnrollmentDesk.CourseCode}\nHi {studentName},\nYour status: **{status}**.\n" +
               $"Bring a laptop. Discord onboarding link: https://example.invalid/{_courseEnrollmentDesk.CourseCode.ToLowerInvariant()}\n";
    }
}
