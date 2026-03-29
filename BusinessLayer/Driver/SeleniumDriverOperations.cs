using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BusinessLayer.Driver;

public class SeleniumDriverOperations : IDriverOperations
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public SeleniumDriverOperations(IWebDriver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    /// <summary>
    /// Get current URL and page title for verification purposes
    /// </summary>
    public string CurrentUrl => _driver.Url;
    /// <summary>
    /// Get page title for verification purposes
    /// </summary>
    public string Title => _driver.Title;

    /// <summary>
    ///     Navigate to a specified URL using the WebDriver
    /// </summary>
    /// <param name="url"></param>
    public void NavigateTo(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }
    /// <summary>
    /// Click an element specified by a locator, waiting until it's clickable to ensure stability
    /// </summary>
    /// <param name="locator"></param>
    public void Click(By locator)
    {
        var element = _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        element.Click();
    }
    /// <summary>
    ///     Type text into an input field specified by a locator, 
    ///     waiting until it's visible and clearing it first for stability
    /// </summary>
    /// <param name="locator"></param>
    /// <param name="text"></param>
    public void TypeText(By locator, string text)
    {
        var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        element.Clear();
        element.SendKeys(text);
    }
    /// <summary>
    /// Get text from an element specified by a locator, waiting until it's visible to ensure stability
    /// </summary>
    /// <param name="locator"></param>
    /// <returns></returns>
    public string GetText(By locator)
    {
        var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        return element.Text;
    }
    /// <summary>
    /// Check if an element specified by a locator is visible on the page, 
    /// returning false if it's not found to avoid exceptions
    /// </summary>
    /// <param name="locator"></param>
    /// <returns></returns>
    public bool IsElementVisible(By locator)
    {
        try
        {
            var element = _driver.FindElement(locator);
            return element.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
    /// <summary>
    /// Find a single element specified by a locator, waiting until it's visible to ensure stability
    /// </summary>
    /// <param name="locator"></param>
    /// <returns></returns>
    public IWebElement FindElement(By locator)
    {
        return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }
    /// <summary>
    /// Find multiple elements specified by a locator, returning an empty collection if none are found to avoid exceptions
    /// </summary>
    /// <param name="locator"></param>
    /// <returns></returns>
    public IReadOnlyCollection<IWebElement> FindElements(By locator)
    {
        return _driver.FindElements(locator);
    }

    /// <summary>
    /// Clear all cookies from the browser session to ensure a clean state for tests,
    /// </summary>
    public void ClearCookies()
    {
        _driver.Manage().Cookies.DeleteAllCookies();
    }
}
