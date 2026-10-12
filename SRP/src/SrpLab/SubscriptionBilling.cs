namespace SrpLab;

/// <summary>
/// Subscription billing: proration math, invoice number minting, and dunning email bodies.
/// </summary>
public sealed class SubscriptionBilling
{
    public string CustomerId { get; }
    public decimal MonthlyPrice { get; }
    public DateOnly PeriodStart { get; }
    public DateOnly PeriodEnd { get; }
    public int FailedPayments { get; private set; }

    public SubscriptionBilling(string customerId, decimal monthlyPrice, DateOnly periodStart, DateOnly periodEnd)
    {
        CustomerId = customerId;
        MonthlyPrice = monthlyPrice;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public void RegisterFailedPayment() => FailedPayments++;

}

public class SubscribtionRequiredPay
{

    private readonly SubscriptionBilling _subBilling;
    public SubscribtionRequiredPay(SubscriptionBilling subBilling)
    {
        _subBilling = subBilling;
    }


    // calculate how much user should actully pay from activation date 
    public decimal Prorate(DateOnly activeFrom)
    {
        // Finance calendar rules change independently of email copy.
        if (activeFrom <= _subBilling.PeriodStart) return _subBilling.MonthlyPrice;
        if (activeFrom >= _subBilling.PeriodEnd) return 0m;
        var totalDays = _subBilling.PeriodEnd.DayNumber - _subBilling.PeriodStart.DayNumber;
        if (totalDays <= 0) return _subBilling.MonthlyPrice;
        var used = _subBilling.PeriodEnd.DayNumber - activeFrom.DayNumber;
        return Math.Round(_subBilling.MonthlyPrice * used / totalDays, 2);
    }
}


public class NextInvoice
{

    private readonly SubscriptionBilling _subBilling;
    public NextInvoice(SubscriptionBilling subBilling)
    {
        _subBilling = subBilling;
    }

    private static int _invoiceSeq = 1000;
    public string NextInvoiceNumber()
    {
        // Numbering scheme / fiscal prefixes — ops concern, not pricing.
        var n = ++_invoiceSeq;
        return $"INV-{_subBilling.PeriodStart:yyyyMM}-{n:D5}";
    }
}


public class SendingEmail
{

    private readonly SubscriptionBilling _subBilling;
    private readonly SubscribtionRequiredPay _subReqPay;
    private readonly NextInvoice _nextInvoice;
    public SendingEmail(SubscriptionBilling subBilling, SubscribtionRequiredPay subReqPay, NextInvoice nextInvoice)
    {
        _subBilling = subBilling;
        _subReqPay = subReqPay;
        _nextInvoice = nextInvoice;
    }

    public string DunningEmail(string customerName, DateOnly asOf)
    {
        // Collections tone & legal boilerplate ≠ proration formula.
        var amount = _subReqPay.Prorate(_subBilling.PeriodStart);
        var invoice = _nextInvoice.NextInvoiceNumber(); // side-effect while composing mail — nasty on purpose
        var severity = _subBilling.FailedPayments switch
        {
            <= 1 => "friendly reminder",
            2 => "second notice",
            _ => "final notice before suspension"
        };
        return $"Subject: {severity} {invoice}\nHi {customerName},\nBalance {amount:C} as of {asOf:o} ({_subBilling.FailedPayments} failures).\n";
    }
}


public class LegerLine
{

    private readonly SubscriptionBilling _subBilling;
    private readonly SubscribtionRequiredPay _subReqPay;
    private readonly NextInvoice _nextInvoice;
    public LegerLine(SubscriptionBilling subBilling, SubscribtionRequiredPay subReqPay, NextInvoice nextInvoice)
    {
        _subBilling = subBilling;
        _subReqPay = subReqPay;
        _nextInvoice = nextInvoice;
    }

    public string LedgerJournalLine(DateOnly activeFrom)
    {
        // Accounting export format is another axis of change.
        return $"{_subBilling.CustomerId},{_nextInvoice.NextInvoiceNumber()},{_subReqPay.Prorate(activeFrom):0.00},AR-SUB";
    }
}