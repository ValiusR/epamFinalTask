using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using CoreLayer.Configuration;
using BusinessLayer.Models;
using TestLayer.Data;
using System.Collections.Generic;

namespace TestLayer.Tests;
/// <summary>
/// Tests for favorite products functionality - UC-2
/// </summary>
public class UC2_FavoriteProductsTests : TestBase
{
    public UC2_FavoriteProductsTests(ITestOutputHelper output) : base() { }

    // Helper to combine  existing ValidCredentialsData with Browser Types
    public static IEnumerable<object[]> GetValidCredentialsWithBrowser()
    {
        foreach (var data in TestDataProvider.ValidCredentialsData)
        {
            yield return new[] { data[0], (object)BrowserType.Chrome };
            yield return new[] { data[0], (object)BrowserType.Edge };
        }
    }
    /// <summary>
    /// Test to verify that when a user marks products as favorites, 
    /// those products are displayed in the Favorites page
    /// </summary>
    /// <param name="browserType"></param>
    [Theory(DisplayName = "UC-2: Verify selected items appear in Favorites page")]
    [InlineData(BrowserType.Chrome)]
    [InlineData(BrowserType.Edge)]
    [Trait("Category", "UC-2")]
    [Trait("Priority", "High")]
    public void UC2_VerifySelectedItemsInFavoritesPage(BrowserType browserType)
    {
        Logger.Info($"Starting UC-2: Verify selected items appear in Favorites page on {browserType}");
        Initialize(browserType);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        Logger.Info("Performing login with valid credentials");
        PerformLogin(credentials);

        Logger.Info("Marking products as favorites");
        var favoriteProductNames = ProductsPage.MarkProductsAsFavoriteAndGetNames(3);
        favoriteProductNames.Should().HaveCount(3, "Should mark exactly 3 products as favorites");

        Logger.Info("Navigating to Favorites page");
        ProductsPage.NavigateToFavorites();

        Logger.Info("Verifying favorite products are displayed");
        var favoriteProductsOnPage = FavoritesPage.GetFavoriteProductNames();

        favoriteProductsOnPage.Should().NotBeEmpty("Favorites page should contain products");
        favoriteProductsOnPage.Should().HaveCountGreaterOrEqualTo(3, "At least 3 products should be in favorites");

        foreach (var productName in favoriteProductNames)
        {
            FavoritesPage.ContainsProduct(productName).Should().BeTrue(
                $"Product '{productName}' should be displayed in Favorites page");
        }

        Logger.Info($"UC-2 test completed successfully on {browserType}");
    }
    /// <summary>
    /// Test to verify that multiple users can mark products
    /// as favorites and see them in the Favorites page
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="browserType"></param>
    [Theory(DisplayName = "UC-2: Data-driven test with multiple valid credentials")]
    [MemberData(nameof(GetValidCredentialsWithBrowser))]
    [Trait("Category", "UC-2")]
    [Trait("Priority", "Medium")]
    public void UC2_DataDriven_VerifyFavoritesWithMultipleUsers(TestCredentials credentials, BrowserType browserType)
    {
        Logger.Info($"Starting UC-2 data-driven test with user: {credentials.Description ?? credentials.Email} on {browserType}");
        Initialize(browserType);

        PerformLogin(credentials);

        var favoriteProducts = ProductsPage.MarkProductsAsFavoriteAndGetNames(2);
        ProductsPage.NavigateToFavorites();

        var favoritesOnPage = FavoritesPage.GetFavoriteProductNames();
        favoritesOnPage.Should().HaveCountGreaterOrEqualTo(2,
            $"User {credentials.Description} should have at least 2 favorite products on {browserType}");
    }
    /// <summary>
    /// Test to verify that favorites persist after
    /// navigating away and back to the Favorites page
    /// </summary>
    /// <param name="browserType"></param>
    [Theory(DisplayName = "UC-2: Verify favorites persist after page navigation")]
    [InlineData(BrowserType.Chrome)]
    [InlineData(BrowserType.Edge)]
    [Trait("Category", "UC-2")]
    [Trait("Priority", "Low")]
    public void UC2_VerifyFavoritesPersistAfterNavigation(BrowserType browserType)
    {
        Logger.Info($"Starting UC-2: Verify favorites persist after navigation on {browserType}");
        Initialize(browserType);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.MarkProductsAsFavoriteAndGetNames(2);

        LoginPage.NavigateTo("https://practice.qabrains.com/ecommerce");
        ProductsPage.NavigateToFavorites();

        FavoritesPage.HasFavoriteProducts().Should().BeTrue(
            "Favorites should persist after navigation");

        Logger.Info($"UC-2 persistence test completed successfully on {browserType}");
    }
}