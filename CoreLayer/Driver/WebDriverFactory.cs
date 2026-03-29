using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using CoreLayer.Configuration;
using NLog;

namespace CoreLayer.Driver;

/// <summary>
/// Factory for creating WebDriver instances - supports Chrome and Edge
/// </summary>
public class WebDriverFactory : IWebDriverFactory
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    // path to Drivers folder at solution root
    private static readonly string DriversFolder = @"C:\Users\Valius\source\repos\epamFinalTask\BusinessLayer\Driver\";

    public IWebDriver CreateDriver(BrowserType browserType)
    {
        Logger.Info($"Creating WebDriver for browser: {browserType}");

        return browserType switch
        {
            BrowserType.Chrome => CreateChromeDriver(),
            BrowserType.Edge => CreateEdgeDriver(),
            _ => throw new ArgumentException($"Unsupported browser: {browserType}")
        };
    }

    /// <summary>
    /// Create Chrome WebDriver with fallback options
    /// </summary>
    private static IWebDriver CreateChromeDriver()
    {
        Logger.Info("Setting up Chrome WebDriver");
        
        // first check Drivers folder
        var chromeDriverPath = FindDriver("chromedriver", DriversFolder);
        
        if (!string.IsNullOrEmpty(chromeDriverPath))
        {
            Logger.Info($"Using local ChromeDriver from: {chromeDriverPath}");
            return CreateChromeDriverWithPath(chromeDriverPath);
        }
        
        // check common locations
        var commonPaths = new[]
        {
            @"C:\WebDriver\chromedriver.exe",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "chromedriver.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "chromedriver.exe")
        };
        
        foreach (var path in commonPaths)
        {
            if (File.Exists(path))
            {
                Logger.Info($"Found ChromeDriver at: {path}");
                return CreateChromeDriverWithPath(Path.GetDirectoryName(path) ?? "");
            }
        }
        
        Logger.Warn("ChromeDriver not found locally, will try default");
        return CreateChromeDriverWithPath("");
    }

    /// <summary>
    /// Create Chrome WebDriver with given path
    /// </summary>
    private static IWebDriver CreateChromeDriverWithPath(string driverPath)
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        
        ChromeDriver driver;
        if (!string.IsNullOrEmpty(driverPath) && Directory.Exists(driverPath))
        {
            driver = new ChromeDriver(driverPath, options);
        }
        else
        {
            driver = new ChromeDriver(options);
        }
        
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        
        Logger.Info("Chrome WebDriver ready");
        return driver;
    }

    /// <summary>
    /// Create Edge WebDriver with fallback options
    /// </summary>
    private static IWebDriver CreateEdgeDriver()
    {
        Logger.Info("Setting up Edge WebDriver");
        
        var edgeDriverPath = FindDriver("msedgedriver", DriversFolder);
        
        if (!string.IsNullOrEmpty(edgeDriverPath))
        {
            Logger.Info($"Using local EdgeDriver from: {edgeDriverPath}");
            return CreateEdgeDriverWithPath(edgeDriverPath);
        }
        
        var commonPaths = new[]
        {
            @"C:\WebDriver\msedgedriver.exe",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "msedgedriver.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "msedgedriver.exe")
        };
        
        foreach (var path in commonPaths)
        {
            if (File.Exists(path))
            {
                Logger.Info($"Found EdgeDriver at: {path}");
                return CreateEdgeDriverWithPath(Path.GetDirectoryName(path) ?? "");
            }
        }
        
        Logger.Warn("EdgeDriver not found locally, will try default");
        return CreateEdgeDriverWithPath("");
    }

    /// <summary>
    /// Create Edge WebDriver with given path
    /// </summary>
    private static IWebDriver CreateEdgeDriverWithPath(string driverPath)
    {
        var options = new EdgeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--no-sandbox");
        
        EdgeDriver driver;
        if (!string.IsNullOrEmpty(driverPath) && Directory.Exists(driverPath))
        {
            driver = new EdgeDriver(driverPath, options);
        }
        else
        {
            driver = new EdgeDriver(options);
        }
        
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        
        Logger.Info("Edge WebDriver ready");
        return driver;
    }

    /// <summary>
    /// Search for driver in given directory
    /// </summary>
    private static string? FindDriver(string driverName, string searchPath)
    {
        try
        {
            if (!Directory.Exists(searchPath))
            {
                Logger.Debug($"Drivers folder not found: {searchPath}");
                return null;
            }

            var driverFiles = Directory.GetFiles(searchPath, $"{driverName}*.exe", SearchOption.AllDirectories);
            if (driverFiles.Length > 0)
            {
                return Path.GetDirectoryName(driverFiles[0]);
            }
        }
        catch (Exception ex)
        {
            Logger.Warn(ex, $"Error searching for driver: {ex.Message}");
        }
        
        return null;
    }
}