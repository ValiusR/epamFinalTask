using OpenQA.Selenium;

namespace BusinessLayer.Driver;

public interface IDriverOperations
{
    void NavigateTo(string url);
    void Click(By locator);
    void TypeText(By locator, string text);
    string GetText(By locator);
    bool IsElementVisible(By locator);
    IWebElement FindElement(By locator);
    IReadOnlyCollection<IWebElement> FindElements(By locator);
    void ClearCookies();
    string CurrentUrl { get; }
    string Title { get; }
}
