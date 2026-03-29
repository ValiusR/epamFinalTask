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
    public string CurrentUrl => _driver.Url;
    public string Title => _driver.Title;

    public void NavigateTo(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }
    public void Click(By locator)
    {
        var element = _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        element.Click();
    }
    public void TypeText(By locator, string text)
    {
        var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        element.Clear();
        element.SendKeys(text);
    }
    public string GetText(By locator)
    {
        var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        return element.Text;
    }
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
    public IWebElement FindElement(By locator)
    {
        return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }
    public IReadOnlyCollection<IWebElement> FindElements(By locator)
    {
        return _driver.FindElements(locator);
    }

    public void ClearCookies()
    {
        _driver.Manage().Cookies.DeleteAllCookies();
    }
}
