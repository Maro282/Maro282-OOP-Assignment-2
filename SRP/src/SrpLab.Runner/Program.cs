using SrpLab;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("SrpLab — 10 intentional SRP violations (refactor me)");
Console.WriteLine("=================================================");



// ==================== WardBoard Modifications ================

var ward = new WardBoard();
var acuity = new ScoringAcuity();
var bedPatientManagement = new BedPatientManagement(ward, acuity);
bedPatientManagement.AssignBed(1, "p-88", heartRate: 130, spo2: 89);
var NurseNotes = new NurseNotesCreation(ward);
Console.WriteLine(NurseNotes.BuildHandoffNote(1));
Console.WriteLine(string.Join(" | ", bedPatientManagement.DrainPagerLog()));

// ==================== End WardBoard Modifications ================





// ==================== CheckoutBasket Modifications ================

var basket = new CheckoutBasket();
basket.AddLine("SKU-1", 40m, 2);
basket.ApplyCouponText("SAVE10");
basket.EnableGiftWrap();
var basketCalculation = new BasketCalculation(basket);
var paymentAuthorization = new PaymentAuthorization(basket, basketCalculation);
Console.WriteLine($"\nbasket total={basketCalculation.GrandTotal()} auth={paymentAuthorization.AuthorizePaymentStub("4242")}\n");

// ==================== End CheckoutBasket Modifications ================





// ==================== Ticket support Modifications ================

var ticket = new SupportTicket("T-1", "cannot login", "prod is down for me", DateTimeOffset.UtcNow);
var slaCalc = new SLACalculations(ticket);
var publicReply = new PublicReply(ticket, slaCalc);
Console.WriteLine(publicReply.DraftPublicReply("Nora"));
// ==================== End ticket support  ================






//===================== LoanDesk Modification ======================

var loan = new LoanDesk(60_000m, 640, 4, hasCollateral: false);
var loanManagement = new LoanManagement(loan);
var requiredDocs = new RequiredDocs(loan, loanManagement);
CreateDecisionLetter DecisionLetter = new(loan, loanManagement, requiredDocs);

Console.WriteLine(DecisionLetter.DecisionLetter("Omar"));

// =============================== End LoanDesk Ticket ====================





//===================== Kitchen CourseEnrollment Modification ======================

var course = new CourseEnrollmentDesk("SEF-101", capacity: 1, tuition: 3000m);
Console.WriteLine(course.Register("a@mail.com"));
Console.WriteLine(course.Register("b@mail.com"));
var welcomeMarkdown = new WelcomingStudent(course);
Console.WriteLine(welcomeMarkdown.WelcomePacketMarkdown("b@mail.com", "Bea"));

// =============================== End CourseEnrollment Ticket ====================




//===================== Kitchen Ticket Modification ======================

var kitchen = new KitchenTicket();
kitchen.AddItem("Pasta", new[] { "wheat", "milk" }, 12);
kitchen.AddItem("Piza", new[] { "chicken", "milk" }, 12);
var allergensDetector = new AllergensDetector(kitchen);
var orderServices = new OrderServices(kitchen, allergensDetector);
var renderThermal = new ThermalTicketRender(kitchen, orderServices, allergensDetector);
Console.WriteLine(renderThermal.RenderThermalTicket(42));


// =============================== End Kitchen Ticket ====================




//=====================  Subscribtion Billing Modification ======================

var sub = new SubscriptionBilling("c-9", 99m, new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 1));
sub.RegisterFailedPayment();
var subReqPay = new SubscribtionRequiredPay(sub);
var nextInvoice = new NextInvoice(sub);
var dunningEmail = new SendingEmail(sub, subReqPay, nextInvoice);
Console.WriteLine(dunningEmail.DunningEmail("Sara", new DateOnly(2026, 9, 20)));

// =============================== End Subscribtion Billing ====================



// ======================== WarehousePickList Modifications ======================

var pick = new WarehousePickList();
pick.AddNeed("BOLT", "A", 3, 10, 7);
pick.AddNeed("NUT", "B", 1, 5, 5);
var path = new WhereToGo(pick);
var allocator = new ReAllocation(pick);

var humanScript = new Scripting(pick, path, allocator);
Console.WriteLine(humanScript.PickerScript());

// =============================== End WarehousePickList Billing ====================




// ======================== GradeBook Modifications ======================

var grades = new GradeBook();
grades.Record("s1", 92);
grades.Record("s1", 88);
var transcriptPlain = new PlainTranscript(grades);
Console.WriteLine($"\n{transcriptPlain.TranscriptPlain("s1", "Marwan")}\n");

// ======================== End GradeBook Modifications ======================





//  ========   Appointment Class  Modified =======
var appt = new AppointmentDesk(new TimeOnly(9, 0), new TimeOnly(17, 0), 30); // clinic working hours
var slot = appt.FindNextSlot(DateTimeOffset.Parse("2026-09-21T08:00:00Z"), 48);
if (slot is null) throw new InvalidOperationException("no slot");
appt.TryBook(slot.Value);
var smsReminder = new AppointmentSmsReminder();
Console.WriteLine(smsReminder.SmsReminder(slot.Value, "0100")); //updated
var saveToCalenderObj = new SaveToCalender(appt); //created to use save To calender feature
Console.WriteLine(saveToCalenderObj.ToIcs((DateTimeOffset)slot, "Marwan", "hamdy"));
Console.WriteLine("Done. Now split responsibilities — without breaking behavior.");
//  ========  End Appointment Class  Modified =======
