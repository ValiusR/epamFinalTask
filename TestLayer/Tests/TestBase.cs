using OpenQA.Selenium;
using CoreLayer.Configuration;
using CoreLayer.Driver;
using BusinessLayer.Driver;
using BusinessLayer.PageObjects;
using NLog;

namespace TestLayer.Tests;

/// <summary>
/// Base class for all test classes - provides WebDriver and page object initialization
/// </summary>
public abstract class TestBase : IDisposable
{
    protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    protected IWebDriver Driver { get; private set; } = null!;
    protected IDriverOperations DriverOperations { get; private set; } = null!;
    protected LoginPage LoginPage { get; private set; } = null!;
    protected ProductsPage ProductsPage { get; private set; } = null!;
    protected FavoritesPage FavoritesPage { get; private set; } = null!;

    protected TestBase()
    {
        Logger.Info($"Starting test: {GetType().Name} on thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
    }

    /// <summary>
    /// Initialize WebDriver and page objects
    /// </summary>
    protected void Initialize(BrowserType browserType = BrowserType.Chrome)
    {
        Logger.Info($"Setting up WebDriver: {browserType} on thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        
        Driver = WebDriverSingleton.Instance.GetInstance(browserType);
        DriverOperations = new SeleniumDriverOperations(Driver);
        
        LoginPage = new LoginPage(DriverOperations, Driver);
        ProductsPage = new ProductsPage(DriverOperations, Driver);
        FavoritesPage = new FavoritesPage(DriverOperations, Driver);
        
        LoginPage.NavigateTo("https://practice.qabrains.com/ecommerce");
        
        Logger.Info("WebDriver and page objects ready");
    }

    /// <summary>
    /// Perform login with email and password
    /// </summary>
    protected void PerformLogin(string email, string password)
    {
        Logger.Info($"Logging in with: {email}");
        LoginPage.Login(email, password);
        System.Threading.Thread.Sleep(500);
    }

    /// <summary>
    /// Perform login with TestCredentials object
    /// </summary>
    protected void PerformLogin(BusinessLayer.Models.TestCredentials credentials)
    {
        PerformLogin(credentials.Email, credentials.Password);
    }

    /// <summary>
    /// Cleanup after test - dispose WebDriver
    /// </summary>
    public virtual void Dispose()
    {
        Logger.Info($"Cleaning up test: {GetType().Name}");
        
        try { Driver?.Manage()?.Cookies?.DeleteAllCookies(); }
        catch (Exception ex) { Logger.Warn(ex, "Error during cookie cleanup"); }
        
        try
        {
            WebDriverSingleton.Instance.QuitCurrent();
            Logger.Debug($"WebDriver quit for thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        }
        catch (Exception ex) { Logger.Warn(ex, "Error quitting WebDriver"); }
    }
}
