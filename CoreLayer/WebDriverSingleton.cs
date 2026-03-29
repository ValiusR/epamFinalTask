using OpenQA.Selenium;
using CoreLayer.Configuration;
using NLog;

namespace CoreLayer.Driver;

/// <summary>
/// Thread-safe singleton for managing WebDriver instances
/// Each thread gets its own WebDriver via ThreadLocal storage
/// </summary>
public sealed class WebDriverSingleton
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static readonly Lazy<WebDriverSingleton> _instance = new(() => new WebDriverSingleton());
    
    // each thread gets its own driver
    private readonly ThreadLocal<IWebDriver> _threadDriver = new();
    private readonly IWebDriverFactory _factory;
    private readonly ThreadLocal<BrowserType?> _currentBrowser = new();

    private WebDriverSingleton()
    {
        _factory = new WebDriverFactory();
        Logger.Info("WebDriverSingleton initialized");
    }

    public static WebDriverSingleton Instance => _instance.Value;

    /// <summary>
    /// Get or create WebDriver for specified browser
    /// Thread-safe: each thread gets its own instance
    /// </summary>
    public IWebDriver GetInstance(BrowserType browserType)
    {
        Logger.Debug($"GetInstance called for: {browserType} on thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        
        if (_threadDriver.Value == null || _currentBrowser.Value != browserType)
        {
            Logger.Info($"Creating new WebDriver for: {browserType} on thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            
            if (_threadDriver.Value != null)
            {
                try
                {
                    _threadDriver.Value.Quit();
                    _threadDriver.Value.Dispose();
                    Logger.Debug($"Disposed old driver on thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, $"Error disposing driver on thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }
            }
            
            var driver = _factory.CreateDriver(browserType);
            _threadDriver.Value = driver;
            _currentBrowser.Value = browserType;
            Logger.Info($"WebDriver created for: {browserType}");
        }
        else
        {
            Logger.Debug($"Reusing existing driver for: {browserType}");
        }
        
        return _threadDriver.Value;
    }

    /// <summary>
    /// Get current WebDriver for this thread
    /// </summary>
    public IWebDriver? GetCurrentDriver()
    {
        return _threadDriver.Value;
    }

    /// <summary>
    /// Quit WebDriver for current thread only
    /// </summary>
    public void QuitCurrent()
    {
        var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        Logger.Info($"Quitting WebDriver for thread: {threadId}");
        
        if (_threadDriver.Value != null)
        {
            try
            {
                _threadDriver.Value.Quit();
                _threadDriver.Value.Dispose();
                Logger.Debug($"WebDriver disposed for thread: {threadId}");
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, $"Error quitting WebDriver for thread: {threadId}");
            }
            finally
            {
                _threadDriver.Value = null;
                _currentBrowser.Value = null;
            }
        }
    }

    /// <summary>
    /// Cleanup all drivers
    /// </summary>
    public void QuitAll()
    {
        Logger.Info("QuitAll - cleaning up drivers");
        QuitCurrent();
    }

    /// <summary>
    /// Dispose all resources
    /// </summary>
    public void Dispose()
    {
        Logger.Info("Disposing WebDriverSingleton");
        QuitCurrent();
        _threadDriver.Dispose();
        _currentBrowser.Dispose();
    }
}
