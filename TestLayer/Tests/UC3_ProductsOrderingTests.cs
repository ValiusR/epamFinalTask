using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using CoreLayer.Configuration;
using BusinessLayer.Models;
using TestLayer.Data;

namespace TestLayer.Tests;

/// <summary>
/// Tests for product sorting functionality - UC-3
/// </summary>
public class UC3_ProductsOrderingTests : TestBase
{
    public UC3_ProductsOrderingTests(ITestOutputHelper output) : base() { }

    /// <summary>
    /// Verify products are sorted by price from low to high
    /// </summary>
    [Fact(DisplayName = "UC-3: Verify products ordered by price (Low to High)")]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "High")]
    public void UC3_VerifyProductsOrderedByPriceLowToHigh()
    {
        Logger.Info("Starting UC-3: Verify products ordered by price (Low to High)");
        Initialize(BrowserType.Chrome);
        
        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        Logger.Info("Performing login with valid credentials");
        PerformLogin(credentials);

        var productCountBefore = ProductsPage.GetProductCount();
        productCountBefore.Should().BeGreaterThan(0, "Products page should contain products");
        Logger.Info($"Products found: {productCountBefore}");

        Logger.Info("Applying sort: Price: Low to High");
        ProductsPage.SelectSortOption("Price: Low to High");

        System.Threading.Thread.Sleep(500);
        var pricesAfterSorting = ProductsPage.GetProductPrices();

        Logger.Info("Verifying products are sorted correctly");
        pricesAfterSorting.Should().NotBeEmpty("Should have product prices after sorting");
        pricesAfterSorting.Should().HaveCount(productCountBefore, "Product count should remain the same after sorting");

        var sortedPrices = pricesAfterSorting.OrderBy(p => p).ToList();
        pricesAfterSorting.Should().BeInAscendingOrder("Products should be ordered from lowest to highest price");
        
        Logger.Info($"Lowest price: {sortedPrices.FirstOrDefault()}, Highest price: {sortedPrices.LastOrDefault()}");
        Logger.Info("UC-3 test completed successfully");
    }

    /// <summary>
    /// Verify products are sorted by price from high to low
    /// </summary>
    [Fact(DisplayName = "UC-3: Verify products ordered by price (High to Low)")]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "Medium")]
    public void UC3_VerifyProductsOrderedByPriceHighToLow()
    {
        Logger.Info("Starting UC-3: Verify products ordered by price (High to Low)");
        Initialize(BrowserType.Chrome);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.SelectSortOption("Price: High to Low");
        
        System.Threading.Thread.Sleep(500);
        var pricesAfterSorting = ProductsPage.GetProductPrices();

        pricesAfterSorting.Should().BeInDescendingOrder("Products should be ordered from highest to lowest price");
        
        Logger.Info("UC-3 High to Low test completed successfully");
    }

    /// <summary>
    /// Verify all products are still displayed after sorting
    /// </summary>
    [Fact(DisplayName = "UC-3: Verify all products displayed after sorting")]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "High")]
    public void UC3_VerifyAllProductsDisplayedAfterSorting()
    {
        Logger.Info("Starting UC-3: Verify all products displayed after sorting");
        Initialize(BrowserType.Chrome);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        
        var productsBefore = ProductsPage.GetAllProducts();
        var countBefore = productsBefore.Count;

        ProductsPage.SelectSortOption("Price: Low to High");
        System.Threading.Thread.Sleep(500);
        
        var productsAfter = ProductsPage.GetAllProducts();
        var countAfter = productsAfter.Count;

        countBefore.Should().Be(countAfter, "All products should be displayed after sorting");
        countAfter.Should().BeGreaterThan(0, "Products should exist after sorting");
        
        Logger.Info($"Product count before: {countBefore}, after: {countAfter}");
        Logger.Info("UC-3 product count verification completed successfully");
    }

    /// <summary>
    /// Verify sorting works on Edge browser
    /// </summary>
    [Fact(DisplayName = "UC-3: Verify sorting on Edge browser")]
    [Trait("Category", "UC-3")]
    [Trait("Browser", "Edge")]
    [Trait("Priority", "High")]
    public void UC3_VerifySortingOnEdge()
    {
        Logger.Info("Starting UC-3: Verify sorting on Edge browser");
        Initialize(BrowserType.Edge);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.SelectSortOption("Price: Low to High");
        
        System.Threading.Thread.Sleep(500);
        var prices = ProductsPage.GetProductPrices();

        prices.Should().BeInAscendingOrder("Products should be sorted on Edge browser");
        prices.Should().HaveCountGreaterThan(0, "Edge browser should display products");
        
        Logger.Info("UC-3 Edge browser test completed successfully");
    }

    /// <summary>
    /// Data-driven test for all sort options
    /// </summary>
    [Theory(DisplayName = "UC-3: Data-driven test for all sort options")]
    [MemberData("SortOptionsData", MemberType = typeof(TestDataProvider))]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "Low")]
    public void UC3_DataDriven_VerifyAllSortOptions(string sortOption)
    {
        Logger.Info($"Starting UC-3 data-driven test with sort option: {sortOption}");
        Initialize(BrowserType.Chrome);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.SelectSortOption(sortOption);
        
        System.Threading.Thread.Sleep(500);
        var products = ProductsPage.GetAllProducts();

        products.Should().NotBeEmpty($"Products should be displayed when sorted by '{sortOption}'");
        
        Logger.Info($"Sort option '{sortOption}' verified successfully");
    }
}
