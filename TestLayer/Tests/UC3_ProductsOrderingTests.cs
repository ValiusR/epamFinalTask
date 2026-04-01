using Xunit;
using FluentAssertions;
using CoreLayer.Configuration;
using BusinessLayer.Models;
using TestLayer.Data;
using System.Linq;
using System.Collections.Generic;

namespace TestLayer.Tests;

/// <summary>
/// Tests for product sorting functionality - UC-3
/// </summary>
public class UC3_ProductsOrderingTests : TestBase
{
    public UC3_ProductsOrderingTests() : base() { }

    // Helper to combine  existing SortOptionsData with Browser Types
    public static IEnumerable<object[]> GetSortOptionsWithBrowser()
    {
        foreach (var data in TestDataProvider.SortOptionsData)
        {
            yield return new[] { data[0], (object)BrowserType.Chrome };
            yield return new[] { data[0], (object)BrowserType.Edge };
        }
    }

    /// <summary>
    /// Verify products are sorted by price from low to high
    /// </summary>
    [Theory(DisplayName = "UC-3: Verify products ordered by price (Low to High)")]
    [InlineData(BrowserType.Chrome)]
    [InlineData(BrowserType.Edge)]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "High")]
    public void UC3_VerifyProductsOrderedByPriceLowToHigh(BrowserType browserType)
    {
        Logger.Info($"Starting UC-3: Verify products ordered by price (Low to High) on {browserType}");
        Initialize(browserType);

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

        var pricesAfterSorting = ProductsPage.GetProductPrices();

        Logger.Info("Verifying products are sorted correctly");
        pricesAfterSorting.Should().NotBeEmpty("Should have product prices after sorting");
        pricesAfterSorting.Should().HaveCount(productCountBefore, "Product count should remain the same after sorting");

        var sortedPrices = pricesAfterSorting.OrderBy(p => p).ToList();
        pricesAfterSorting.Should().BeInAscendingOrder("Products should be ordered from lowest to highest price");

        Logger.Info($"Lowest price: {sortedPrices.FirstOrDefault()}, Highest price: {sortedPrices.LastOrDefault()}");
        Logger.Info($"UC-3 test completed successfully on {browserType}");
    }

    /// <summary>
    /// Verify products are sorted by price from high to low
    /// </summary>
    [Theory(DisplayName = "UC-3: Verify products ordered by price (High to Low)")]
    [InlineData(BrowserType.Chrome)]
    [InlineData(BrowserType.Edge)]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "Medium")]
    public void UC3_VerifyProductsOrderedByPriceHighToLow(BrowserType browserType)
    {
        Logger.Info($"Starting UC-3: Verify products ordered by price (High to Low) on {browserType}");
        Initialize(browserType);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.SelectSortOption("Price: High to Low");

        var pricesAfterSorting = ProductsPage.GetProductPrices();

        pricesAfterSorting.Should().BeInDescendingOrder("Products should be ordered from highest to lowest price");

        Logger.Info($"UC-3 High to Low test completed successfully on {browserType}");
    }

    /// <summary>
    /// Verify all products are still displayed after sorting
    /// </summary>
    [Theory(DisplayName = "UC-3: Verify all products displayed after sorting")]
    [InlineData(BrowserType.Chrome)]
    [InlineData(BrowserType.Edge)]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "High")]
    public void UC3_VerifyAllProductsDisplayedAfterSorting(BrowserType browserType)
    {
        Logger.Info($"Starting UC-3: Verify all products displayed after sorting on {browserType}");
        Initialize(browserType);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);

        var productsBefore = ProductsPage.GetAllProducts();
        var countBefore = productsBefore.Count;

        ProductsPage.SelectSortOption("Price: Low to High");

        var productsAfter = ProductsPage.GetAllProducts();
        var countAfter = productsAfter.Count;

        countBefore.Should().Be(countAfter, "All products should be displayed after sorting");
        countAfter.Should().BeGreaterThan(0, "Products should exist after sorting");

        Logger.Info($"Product count before: {countBefore}, after: {countAfter}");
        Logger.Info($"UC-3 product count verification completed successfully on {browserType}");
    }

    /// <summary>
    /// Data-driven test for all sort options
    /// </summary>
    [Theory(DisplayName = "UC-3: Data-driven test for all sort options")]
    [MemberData(nameof(GetSortOptionsWithBrowser))]
    [Trait("Category", "UC-3")]
    [Trait("Priority", "Low")]
    public void UC3_DataDriven_VerifyAllSortOptions(string sortOption, BrowserType browserType)
    {
        Logger.Info($"Starting UC-3 data-driven test with sort option: {sortOption} on {browserType}");
        Initialize(browserType);

        var credentials = TestCredentialsBuilder.Create()
            .WithEmail("test@qabrains.com")
            .WithPassword("Password123")
            .Build();

        PerformLogin(credentials);
        ProductsPage.SelectSortOption(sortOption);

        var products = ProductsPage.GetAllProducts();

        products.Should().NotBeEmpty($"Products should be displayed when sorted by '{sortOption}' on {browserType}");

        Logger.Info($"Sort option '{sortOption}' verified successfully on {browserType}");
    }
}