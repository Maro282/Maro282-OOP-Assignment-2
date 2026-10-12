namespace SrpLab;

/// <summary>
/// Loan desk: eligibility arithmetic, required-document lists, and rejection letter prose.
/// </summary>
public sealed class LoanDesk
{
    public decimal RequestedAmount { get; }
    public int CreditScore { get; }
    public int EmploymentMonths { get; }
    public bool HasCollateral { get; }

    public LoanDesk(decimal requestedAmount, int creditScore, int employmentMonths, bool hasCollateral)
    {
        RequestedAmount = requestedAmount;
        CreditScore = creditScore;
        EmploymentMonths = employmentMonths;
        HasCollateral = hasCollateral;
    }


}


public class LoanManagement
{
    private readonly LoanDesk _loanDesk;
    public LoanManagement(LoanDesk loanDesk)
    {
        _loanDesk = loanDesk;
    }


    public decimal RiskScore()
    {
        // Risk model will change with risk committee — not with letter templates.
        decimal score = 100m;
        score -= Math.Max(0, 700 - _loanDesk.CreditScore) * 0.15m;
        if (_loanDesk.EmploymentMonths < 6) score -= 20m;
        if (_loanDesk.RequestedAmount > 50_000m && !_loanDesk.HasCollateral) score -= 25m;
        if (_loanDesk.RequestedAmount > 150_000m) score -= 10m;
        return Math.Clamp(score, 0m, 100m);
    }

    public bool IsEligible() => RiskScore() >= 55m && _loanDesk.CreditScore >= 580;



}


public class RequiredDocs
{

    private readonly LoanDesk _loanDesk;
    private readonly LoanManagement _loanManagement;
    public RequiredDocs(LoanDesk loanDesk, LoanManagement loanManagement)
    {
        _loanDesk = loanDesk;
        _loanManagement = loanManagement;
    }


    public IReadOnlyList<string> RequiredDocuments()
    {
        // Compliance checklist changes with regulation, independently of risk formula.
        var docs = new List<string> { "National ID", "Proof of income (3 months)" };
        if (_loanDesk.RequestedAmount > 40_000m) docs.Add("Bank statements (6 months)");
        if (_loanDesk.HasCollateral) docs.Add("Collateral ownership deed");
        if (_loanDesk.EmploymentMonths < 12) docs.Add("Employer letter");
        if (!_loanManagement.IsEligible()) docs.Add("Manual underwriter referral form");
        return docs;
    }
}


public class CreateDecisionLetter
{

    private readonly LoanDesk _loanDesk;
    private readonly LoanManagement _loanManagement;
    private readonly RequiredDocs _docs;
    public CreateDecisionLetter(LoanDesk loanDesk, LoanManagement loanManagement, RequiredDocs docs)
    {
        _loanDesk = loanDesk;
        _loanManagement = loanManagement;
        _docs = docs;
    }


    public string DecisionLetter(string applicantName)
    {
        // Legal/comms wording ≠ underwriting math.
        if (_loanManagement.IsEligible())
        {
            return $"Dear {applicantName},\nYour request for {_loanDesk.RequestedAmount:C} is pre-approved (risk {_loanManagement.RiskScore():0}).\n" +
                   $"Please upload: {string.Join("; ", _docs.RequiredDocuments())}.\n";
        }

        return $"Dear {applicantName},\nWe are unable to approve {_loanDesk.RequestedAmount:C} at this time.\n" +
               $"Reference risk={_loanManagement.RiskScore():0}. You may reapply after improving documentation.\n";
    }
}


public class ExportingCSV
{

    private readonly LoanDesk _loanDesk;
    private readonly LoanManagement _loanManagement;
    public ExportingCSV(LoanDesk loanDesk, LoanManagement loanManagement)
    {
        _loanDesk = loanDesk;
        _loanManagement = loanManagement;
    }

    public string UnderwriterCsvRow(string applicationId)
    {
        // Analytics export schema is yet another reason to change.
        return $"{applicationId},{_loanDesk.CreditScore},{_loanDesk.EmploymentMonths},{(_loanDesk.HasCollateral ? 1 : 0)},{_loanManagement.RiskScore():0.00},{(_loanManagement.IsEligible() ? "Y" : "N")}";
    }
}