using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BusinessLayer.Driver;
using NLog;

namespace BusinessLayer.PageObjects;

/// <summary>
/// Base class for page objects 
/// </summary>
public abstract class BasePage
{
    protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    protected readonly IDriverOperations DriverOperations;
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    
    private const int DefaultWaitTimeout = 10;

    protected BasePage(IDriverOperations driverOperations, IWebDriver driver)
    {
        DriverOperations = driverOperations ?? throw new ArgumentNullException(nameof(driverOperations));
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultWaitTimeout));
    }

    protected void Click(By locator)
    {
        Logger.Debug($"Clicking: {locator}");
        DriverOperations.Click(locator);
    }


    protected void TypeText(By locator, string text)
    {
        Logger.Debug($"Typing '{text}' into: {locator}");
        DriverOperations.TypeText(locator, text);
    }

    /// <summary>
    /// Get text from element
    /// </summary>
    protected string GetText(By locator)
    {
        return DriverOperations.GetText(locator);
    }

    /// <summary>
    /// Check if element is visible
    /// </summary>
    protected bool IsElementVisible(By locator)
    {
        return DriverOperations.IsElementVisible(locator);
    }

    /// <summary>
    /// Find single element
    /// </summary>
    protected IWebElement FindElement(By locator)
    {
        return DriverOperations.FindElement(locator);
    }

    /// <summary>
    /// Find multiple elements
    /// </summary>
    protected IList<IWebElement> FindElements(By locator)
    {
        return DriverOperations.FindElements(locator).ToList();
    }

    /// <summary>
    /// Navigate to URL
    /// </summary>
    public void NavigateTo(string url)
    {
        Logger.Info($"Navigating to: {url}");
        DriverOperations.NavigateTo(url);
    }

    public string CurrentUrl => DriverOperations.CurrentUrl;
    public string Title => DriverOperations.Title;

    /// <summary>
    /// Wait for element to be visible
    /// </summary>
    protected IWebElement WaitForElementVisible(By locator, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(locator);
                return element.Displayed ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });
    }

    /// <summary>
    /// Wait for element to be clickable
    /// </summary>
    protected IWebElement WaitForElementClickable(By locator, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(locator);
                return element.Enabled && element.Displayed ? element : null;
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException($"Element not found: {locator}");
            }
        });
    }

    /// <summary>
    /// Wait for a specific condition to be true
    /// </summary>
    protected void WaitForCondition(Func<IWebDriver, bool> condition, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(d => condition(d));
    }
}
