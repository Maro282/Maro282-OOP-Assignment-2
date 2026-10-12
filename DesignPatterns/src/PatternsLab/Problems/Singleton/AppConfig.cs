namespace PatternsLab.Problems.Singleton;

public class AppConfig
{
    public static int LoadCount;

    public string DbConnection { get; set; }
    public string Theme { get; set; }

    public AppConfig()
    {
        LoadCount++;
        Console.WriteLine($"[AppConfig] Loading settings from disk... (load #{LoadCount})");
        Thread.Sleep(300);
        DbConnection = "Server=localhost;Db=School";
        Theme = "Light";
    }
}

public class DatabaseService
{
    public AppConfig Config { get; } = new AppConfig();

    public void Connect() => Console.WriteLine($"Connecting to {Config.DbConnection}");
}

public class UiService
{
    public AppConfig Config { get; } = new AppConfig();

    public void Render() => Console.WriteLine($"UI is using theme: {Config.Theme}");
}


// ======================= After Applying Singlton Pattern ===================
public class ModifiedAppConfig
{
    //Lazy initializing
    private static readonly Lazy<ModifiedAppConfig> _appConfig = new(() => new ModifiedAppConfig());
    // Public Property to access _appConfig instace
    public static ModifiedAppConfig AppConfig => _appConfig.Value;
    public static int LoadCount;
    public string DbConnection { get; }
    public string Theme { get; set; }

    private ModifiedAppConfig()
    {
        LoadCount++;
        Console.WriteLine($"[AppConfig] Loading settings from disk... (load #{LoadCount})");
        DbConnection = "Server=localhost;Db=School";
        Theme = "Light";
    }
}

public class ModifiedDatabaseService
{
    public ModifiedAppConfig Config { get; } = ModifiedAppConfig.AppConfig;
    public void Connect() => Console.WriteLine($"Connecting to {Config.DbConnection}");
}

public class ModifiedUiService
{
    public ModifiedAppConfig Config { get; } = ModifiedAppConfig.AppConfig;

    public void Render() => Console.WriteLine($"UI is using theme: {Config.Theme}");
}