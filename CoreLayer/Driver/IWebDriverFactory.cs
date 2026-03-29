using OpenQA.Selenium;
using CoreLayer.Configuration;

namespace CoreLayer.Driver;

public interface IWebDriverFactory
{
    IWebDriver CreateDriver(BrowserType browserType);
}
