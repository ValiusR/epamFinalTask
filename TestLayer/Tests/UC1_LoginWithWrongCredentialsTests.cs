using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using CoreLayer.Configuration;
using BusinessLayer.Models;
using TestLayer.Data;
using System.Collections.Generic;

namespace TestLayer.Tests;
/// <summary>
/// Tests for login functionality with wrong credentials - UC-1
/// </summary>
public class UC1_LoginWithWrongCredentialsTests : TestBase
{
    public UC1_LoginWithWrongCredentialsTests(ITestOutputHelper output) : base() { }
    /// <summary>
    /// Helper to combine existing
    /// InvalidCredentialsData with Browser Types for data-driven testing
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> GetInvalidCredentialsWithBrowser()
    {
        foreach (var data in TestDataProvider.InvalidCredentialsData)
        {
            yield return new[] { data[0], (object)BrowserType.Chrome };
            yield return new[] { data[0], (object)BrowserType.Edge };
        }
    }
    /// <summary>
    /// Test to verify error messages are displayed when logging 
    /// in with wrong credentials (invalid email and password)
    /// </summary>
    /// <param name="browserType"></param>
    [Theory(DisplayName = "UC-1: Verify error messages for invalid credentials")]
    [InlineData(BrowserType.Chrome)]
    [InlineData(BrowserType.Edge)]
    [Trait("Category", "UC-1")]
    [Trait("Priority", "High")]
    public void UC1_VerifyErrorMessagesForWrongCredentials(BrowserType browserType)
    {
        Logger.Info($"Starting UC-1: Verify error messages for wrong credentials on {browserType}");
        Initialize(browserType);

        var invalidCredentials = TestCredentialsBuilder.Create()
            .WithEmail("wrong@email.com")
            .WithPassword("wrongpassword")
            .Build();

        Logger.Info($"Entering invalid credentials: {invalidCredentials.Email}");
        LoginPage.EnterEmail(invalidCredentials.Email);
        LoginPage.EnterPassword(invalidCredentials.Password);
        LoginPage.ClickLoginButton();

        // lazy but no time for explicit waits sorry
        System.Threading.Thread.Sleep(1000);

        Logger.Info("Verifying error messages are displayed");
        LoginPage.IsUsernameErrorDisplayed().Should().BeTrue(
            "Username error message should be displayed for invalid email");
        LoginPage.IsPasswordErrorDisplayed().Should().BeTrue(
            "Password error message should be displayed for invalid password");

        Logger.Info($"UC-1 test completed successfully on {browserType}");
    }
    /// <summary>
    /// Test to verify error messages for multiple invalid 
    /// credential scenarios (empty email, empty password, wrong email, wrong password)
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="browserType"></param>
    [Theory(DisplayName = "UC-1: Data-driven test with multiple invalid credentials")]
    [MemberData(nameof(GetInvalidCredentialsWithBrowser))]
    [Trait("Category", "UC-1")]
    [Trait("Priority", "Medium")]
    public void UC1_DataDriven_VerifyErrorMessages(TestCredentials credentials, BrowserType browserType)
    {
        Logger.Info($"Starting UC-1 data-driven test with credentials: {credentials.Email} on {browserType}");
        Initialize(browserType);

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
}