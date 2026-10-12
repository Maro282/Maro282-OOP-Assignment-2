namespace PatternsLab.Problems.Builder;

public sealed class CourseRegistration
{
    public string StudentEmail { get; }
    public string CourseCode { get; }
    public string AccessMode { get; }
    public string? GroupCode { get; }
    public string? DiscountCode { get; }
    public bool SendWhatsApp { get; }
    public bool SendEmailWelcome { get; }
    public string? MentorNote { get; }
    public DateOnly? PreferredStart { get; }

    public CourseRegistration(
        string studentEmail,
        string courseCode,
        string accessMode,
        string? groupCode,
        string? discountCode,
        bool sendWhatsApp,
        bool sendEmailWelcome,
        string? mentorNote,
        DateOnly? preferredStart)
    {
        //if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email required");
        //if (string.IsNullOrWhiteSpace(courseCode)) throw new ArgumentException("course required");

        //if (accessMode == "LiveGroup" && string.IsNullOrWhiteSpace(groupCode))
        //    throw new InvalidOperationException("LiveGroup requires GroupCode");
        //if (accessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(groupCode))
        //    throw new InvalidOperationException("VideosOnly cannot have GroupCode");

        StudentEmail = studentEmail;
        CourseCode = courseCode;
        AccessMode = accessMode;
        GroupCode = groupCode;
        DiscountCode = discountCode;
        SendWhatsApp = sendWhatsApp;
        SendEmailWelcome = sendEmailWelcome;
        MentorNote = mentorNote;
        PreferredStart = preferredStart;
    }

    public override string ToString()
        => $"{StudentEmail} → {CourseCode} [{AccessMode}] group={GroupCode ?? "-"} discount={DiscountCode ?? "-"} wa={SendWhatsApp} mail={SendEmailWelcome}";
}

public static class RegistrationCallSites
{
    public static CourseRegistration CreateLiveStudentUgly()
    {
        return new CourseRegistration(
            "sara@mail.com",
            "SEF-101",
            "LiveGroup",
            "G1",
            "EARLY10",
            true,
            true,
            "Needs evening slot",
            new DateOnly(2026, 10, 1));
    }

    public static CourseRegistration CreateVideosOnlyUgly()
    {
        return new CourseRegistration(
            "ali@mail.com",
            "SEF-101",
            "VideosOnly",
            null,
            null,
            false,
            true,
            null,
            null);
    }
}


public class CourseRegistrationBuilder
{
    //Mandatory properties
    private string StudentEmail { get; }
    private string CourseCode { get; }
    private string AccessMode { get; }



    //optional properties

    private string? GroupCode { get; set; }
    private string? DiscountCode { get; set; }
    private bool SendWhatsApp { get; set; }
    private bool SendEmailWelcome { get; set; }
    private string? MentorNote { get; set; }
    private DateOnly? PreferredStart { get; set; }

    public CourseRegistrationBuilder(string studentEmail, string courseCode, string accessMode)
    {

        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email required");
        if (string.IsNullOrWhiteSpace(courseCode)) throw new ArgumentException("course required");

        StudentEmail = studentEmail;
        CourseCode = courseCode;
        AccessMode = accessMode;

    }

    public CourseRegistrationBuilder WithGroupCode(string groupCode)
    {
        GroupCode = groupCode;
        return this;
    }

    public CourseRegistrationBuilder WithDiscountCode(string discountCode)
    {
        DiscountCode = discountCode;
        return this;
    }
    public CourseRegistrationBuilder WithWhatsAppMsg(bool x)
    {
        SendWhatsApp = x;
        return this;
    }
    public CourseRegistrationBuilder WithSendEmailWelcome(bool x)
    {
        SendEmailWelcome = x;
        return this;
    }


    public CourseRegistrationBuilder WithMentorNote(string note)
    {
        MentorNote = note;
        return this;
    }
    public CourseRegistrationBuilder WithPreferedStartDate(DateOnly date)
    {
        PreferredStart = date;
        return this;
    }

    public CourseRegistration Build()
    {
        if (AccessMode == "LiveGroup" && string.IsNullOrWhiteSpace(GroupCode))
            throw new InvalidOperationException("LiveGroup requires GroupCode");
        if (AccessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(GroupCode))
            throw new InvalidOperationException("VideosOnly cannot have GroupCode");

        return new CourseRegistration(StudentEmail,
                                      CourseCode,
                                      AccessMode, GroupCode,
                                      DiscountCode, SendWhatsApp,
                                      SendEmailWelcome, MentorNote,
                                      PreferredStart);
    }





}