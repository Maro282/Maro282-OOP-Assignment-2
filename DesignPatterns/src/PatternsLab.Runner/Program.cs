using System.Diagnostics;
using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;

//Console.WriteLine("=== SINGLETON: BEFORE ===\n");

//var db = new DatabaseService();
//var ui = new UiService();

//Console.WriteLine();

//db.Config.Theme = "Dark";
//Console.WriteLine("Admin changed theme to Dark.");

//ui.Render();

//Console.WriteLine($"\nSame config object? {ReferenceEquals(db.Config, ui.Config)}");
//Console.WriteLine($"Times config was loaded from disk: {AppConfig.LoadCount}");



//Console.WriteLine();
//Console.WriteLine("=== SINGLETON: AFTER ===\n");

//var db2 = new ModifiedDatabaseService();
//var ui2 = new ModifiedUiService();

//Console.WriteLine();

//db2.Config.Theme = "Dark";
//Console.WriteLine("Admin changed theme to Dark.");

//ui2.Render();

//Console.WriteLine($"\nSame config object? {ReferenceEquals(db2.Config, ui2.Config)}");
//Console.WriteLine($"Times config was loaded from disk: {ModifiedAppConfig.LoadCount}");



Console.WriteLine("\n=== PROTOTYPE: BEFORE ===\n");

var sw = Stopwatch.StartNew();
var army = new List<Enemy>();
for (int i = 1; i <= 5; i++)
{
    var orc = new Orc();
    orc.Name = $"Orc-{i}";
    army.Add(orc);
}
Console.WriteLine($"Created 5 orcs in {sw.ElapsedMilliseconds} ms\n");

Enemy original = new Orc();
original.Name = "Boss Orc";
Enemy copy = EnemyCopyHelper.CopyEnemy(original);

Console.WriteLine($"\nOriginal model id: {original.ModelId}");
Console.WriteLine($"Copy model id:     {copy.ModelId}");

Console.WriteLine("\n=== PROTOTYPE: After ===\n");

Enemy originalAfter = new Orc();
originalAfter.Name = "Boss Orc";
Enemy copyAfter = originalAfter.Clone();

Console.WriteLine($"\nOriginal model id: {originalAfter.ModelId}");
Console.WriteLine($"Copy model id:       {copyAfter.ModelId}");

copyAfter.Weapon.Damage = 999;
Console.WriteLine($"\nWe changed the COPY's weapon damage to 999.");
Console.WriteLine($"Original's weapon damage is now: {originalAfter.Weapon.Damage}");

copy.Abilities.Add("Fire Breath");
Console.WriteLine($"Original abilities: {string.Join(", ", originalAfter.Abilities)}");




//Console.WriteLine("\n=== BUILDER: BEFORE ===\n");
//Console.WriteLine(RegistrationCallSites.CreateLiveStudentUgly());
//Console.WriteLine(RegistrationCallSites.CreateVideosOnlyUgly());


//Console.WriteLine("\n=== BUILDER: After ===\n");

//CourseRegistration registration1 = new CourseRegistrationBuilder("sara@mail.com", "SEF-101", "LiveGroup")
//                                      .WithGroupCode("G1")
//                                      .WithSendEmailWelcome(true)
//                                      .WithWhatsAppMsg(true)
//                                      .WithDiscountCode("EARLY10")
//                                      .WithMentorNote("Needs evening slot")
//                                      .WithPreferedStartDate(new DateOnly(2026, 10, 1))
//                                      .Build();
//CourseRegistration registration2 = new CourseRegistrationBuilder("ali@mail.com", "SEF-101", "VideosOnly")
//                                      .WithSendEmailWelcome(true)
//                                      .WithWhatsAppMsg(false)
//                                      //.WithGroupCode("s1") // This line to check Validation is working or not
//                                      .Build();

//Console.WriteLine(registration1);
//Console.WriteLine(registration2);
