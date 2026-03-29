using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using CoreLayer.Configuration;
using BusinessLayer.Models;
using TestLayer.Data;

namespace TestLayer.Tests;

public class UC2_FavoriteProductsTests : TestBase
{
    public UC2_FavoriteProductsTests(ITestOutputHelper output) : base() { }

    [Fact(DisplayName = "UC-2: Verify selected items appear in Favorites page")]
    [Trait("Category", "UC-2")]
    [Trait("Priority", "High")]
    public void UC2_VerifySelectedItemsInFavoritesPage()
    {
        Logger.Info("Starting UC-2: Verify selected items appear in Favorites page");
        Initialize(BrowserType.Chrome);
        
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
        
        Logger.Info("UC-2 test completed successfully");
    }

    [Theory(DisplayName = "UC-2: Data-driven test with multiple valid credentials")]
    [MemberData("ValidCredentialsData", MemberType = typeof(TestDataProvider))]
    [Trait("Category", "UC-2")]
    [Trait("Priority", "Medium")]
    public void UC2_DataDriven_VerifyFavoritesWithMultipleUsers(TestCredentials credentials)
    {
        Logger.Info($"Starting UC-2 data-driven test with user: {credentials.Description ?? credentials.Email}");
        Initialize(BrowserType.Chrome);

        PerformLogin(credentials);
        
        var favoriteProducts = ProductsPage.MarkProductsAsFavoriteAndGetNames(2);
        ProductsPage.NavigateToFavorites();

        var favoritesOnPage = FavoritesPage.GetFavoriteProductNames();
        favoritesOnPage.Should().HaveCountGreaterOrEqualTo(2,
            $"User {credentials.Description} should have at least 2 favorite products");
    }

    [Fact(DisplayName = "UC-2: Verify favorites functionality on Edge browser")]
    [Trait("Category", "UC-2")]
    [Trait("Browser", "Edge")]
    [Trait("Priority", "High")]
    public void UC2_VerifyFavoritesOnEdge()
    {
        Logger.Info("Starting UC-2: Verify favorites on Edge browser");
        Initialize(BrowserType.Edge);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.MarkProductsAsFavoriteAndGetNames(2);
        ProductsPage.NavigateToFavorites();

        FavoritesPage.HasFavoriteProducts().Should().BeTrue(
            "Edge browser: Favorites page should contain products");
        
        Logger.Info("UC-2 Edge browser test completed successfully");
    }

    [Fact(DisplayName = "UC-2: Verify favorites persist after page navigation")]
    [Trait("Category", "UC-2")]
    [Trait("Priority", "Low")]
    public void UC2_VerifyFavoritesPersistAfterNavigation()
    {
        Logger.Info("Starting UC-2: Verify favorites persist after navigation");
        Initialize(BrowserType.Chrome);

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
        
        Logger.Info("UC-2 persistence test completed successfully");
    }
}
