using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using CoreLayer.Configuration;
using BusinessLayer.Models;
using TestLayer.Data;

namespace TestLayer.Tests;

public class UC1_LoginWithWrongCredentialsTests : TestBase
{
    public UC1_LoginWithWrongCredentialsTests(ITestOutputHelper output) : base() { }

    [Fact(DisplayName = "UC-1: Verify error messages for invalid credentials")]
    [Trait("Category", "UC-1")]
    [Trait("Priority", "High")]
    public void UC1_VerifyErrorMessagesForWrongCredentials()
    {
        Logger.Info("Starting UC-1: Verify error messages for wrong credentials");
        Initialize(BrowserType.Chrome);
        
        var invalidCredentials = TestCredentialsBuilder.Create()
            .WithEmail("wrong@email.com")
            .WithPassword("wrongpassword")
            .Build();

        Logger.Info($"Entering invalid credentials: {invalidCredentials.Email}");
        LoginPage.EnterEmail(invalidCredentials.Email);
        LoginPage.EnterPassword(invalidCredentials.Password);
        LoginPage.ClickLoginButton();

        System.Threading.Thread.Sleep(1000);

        Logger.Info("Verifying error messages are displayed");
        LoginPage.IsUsernameErrorDisplayed().Should().BeTrue(
            "Username error message should be displayed for invalid email");
        LoginPage.IsPasswordErrorDisplayed().Should().BeTrue(
            "Password error message should be displayed for invalid password");
        
        Logger.Info("UC-1 test completed successfully");
    }

    [Theory(DisplayName = "UC-1: Data-driven test with multiple invalid credentials")]
    [MemberData("InvalidCredentialsData", MemberType = typeof(TestDataProvider))]
    [Trait("Category", "UC-1")]
    [Trait("Priority", "Medium")]
    public void UC1_DataDriven_VerifyErrorMessages(TestCredentials credentials)
    {
        Logger.Info($"Starting UC-1 data-driven test with credentials: {credentials.Email}");
        Initialize(BrowserType.Chrome);

        LoginPage.EnterEmail(credentials.Email);
        LoginPage.EnterPassword(credentials.Password);
        LoginPage.ClickLoginButton();

        System.Threading.Thread.Sleep(1000);

        Logger.Info($"Verifying error messages for: {credentials.Description ?? credentials.Email}");
        
        if (credentials.IsEmailEmpty)
        {
            LoginPage.IsEmailRequiredErrorDisplayed().Should().BeTrue(
                $"Email required error should be displayed for {credentials.Description ?? credentials.Email}");
        }
        else
        {
            LoginPage.IsUsernameErrorDisplayed().Should().BeTrue(
                $"Username error should be displayed for {credentials.Description ?? credentials.Email}");
        }
        
        if (credentials.IsPasswordEmpty)
        {
            LoginPage.IsPasswordRequiredErrorDisplayed().Should().BeTrue(
                $"Password required error should be displayed for {credentials.Description ?? credentials.Email}");
        }
        else
        {
            LoginPage.IsPasswordErrorDisplayed().Should().BeTrue(
                $"Password error should be displayed for {credentials.Description ?? credentials.Email}");
        }
    }

    [Fact(DisplayName = "UC-1: Verify error messages on Edge browser")]
    [Trait("Category", "UC-1")]
    [Trait("Browser", "Edge")]
    [Trait("Priority", "High")]
    public void UC1_VerifyErrorMessagesOnEdge()
    {
        Logger.Info("Starting UC-1: Verify error messages on Edge browser");
        Initialize(BrowserType.Edge);

        var invalidCredentials = TestCredentialsBuilder.Create()
            .WithEmail("invalid@test.com")
            .WithPassword("invalidpass")
            .Build();

        LoginPage.Login(invalidCredentials.Email, invalidCredentials.Password);
        System.Threading.Thread.Sleep(1000);

        LoginPage.IsUsernameErrorDisplayed().Should().BeTrue(
            "Username error message should be displayed on Edge browser");
        LoginPage.IsPasswordErrorDisplayed().Should().BeTrue(
            "Password error message should be displayed on Edge browser");
        
        Logger.Info("UC-1 Edge browser test completed successfully");
    }
}
