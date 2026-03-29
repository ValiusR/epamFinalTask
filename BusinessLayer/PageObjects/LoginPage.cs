using OpenQA.Selenium;
using BusinessLayer.Driver;

namespace BusinessLayer.PageObjects;

/// <summary>
/// Login page object - handles login form interactions
/// </summary>
public class LoginPage : BasePage
{
    // locators based on site HTML
    private static readonly By EmailInput = By.XPath("//input[@name='email']");
    private static readonly By PasswordInput = By.XPath("//input[@name='password']");
    private static readonly By LoginButton = By.XPath("//button[@type='submit']");
    
    // error message locators
    private static readonly By UsernameError = By.XPath("//p[contains(text(),'Username is incorrect')]");
    private static readonly By PasswordError = By.XPath("//p[contains(text(),'Password is incorrect')]");
    private static readonly By EmailValidationError = By.XPath("//p[contains(text(),'Email must be a valid email')]");
    private static readonly By EmailRequiredError = By.XPath("//p[contains(text(),'Email is a required field')]");
    private static readonly By PasswordRequiredError = By.XPath("//p[contains(text(),'Password is a required field')]");
    
    // user account after login
    private static readonly By UserAccountLink = By.XPath("//span[contains(@class,'user-name')]");

    public LoginPage(IDriverOperations driverOperations, IWebDriver driver) : base(driverOperations, driver)
    {
    }

    /// <summary>
    /// Enter email into email field
    /// </summary>
    public void EnterEmail(string email)
    {
        TypeText(EmailInput, email);
    }

    /// <summary>
    /// Enter password into password field
    /// </summary>
    public void EnterPassword(string password)
    {
        TypeText(PasswordInput, password);
    }

    /// <summary>
    /// Click the login button
    /// </summary>
    public void ClickLoginButton()
    {
        Click(LoginButton);
    }

    /// <summary>
    /// Check if username error is displayed
    /// </summary>
    public bool IsUsernameErrorDisplayed()
    {
        return IsElementVisible(UsernameError);
    }

    /// <summary>
    /// Check if password error is displayed
    /// </summary>
    public bool IsPasswordErrorDisplayed()
    {
        return IsElementVisible(PasswordError);
    }

    /// <summary>
    /// Check if email validation error is displayed
    /// </summary>
    public bool IsEmailValidationErrorDisplayed()
    {
        return IsElementVisible(EmailValidationError);
    }

    /// <summary>
    /// Check if email required error is displayed
    /// </summary>
    public bool IsEmailRequiredErrorDisplayed()
    {
        return IsElementVisible(EmailRequiredError);
    }

    /// <summary>
    /// Check if password required error is displayed
    /// </summary>
    public bool IsPasswordRequiredErrorDisplayed()
    {
        return IsElementVisible(PasswordRequiredError);
    }

    /// <summary>
    /// Check if user account is displayed after login
    /// </summary>
    public bool IsUserAccountDisplayed()
    {
        return IsElementVisible(UserAccountLink);
    }

    /// <summary>
    /// Perform complete login with email and password
    /// </summary>
    public void Login(string email, string password)
    {
        Logger.Info($"Logging in with: {email}");
        EnterEmail(email);
        EnterPassword(password);
        ClickLoginButton();
    }
}
