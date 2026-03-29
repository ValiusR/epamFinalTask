using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BusinessLayer.Driver;
using BusinessLayer.Models;
using NLog;

namespace BusinessLayer.PageObjects;

/// <summary>
/// Bridge Pattern - Concrete Abstraction
/// Products page object - uses IDriverOperations abstraction
/// </summary>
public class ProductsPage : BasePage
{
    // XPath locators for Products page 


    private static readonly By UserMenu = By.XPath("//button[.//span[contains(@class,'user-name')]]");

    // Product cards
    private static readonly By ProductCards = By.XPath("//div[contains(@class,'flex flex-col gap-3 relative group')]");

    // Product prices
    private static readonly By ProductPrices = By.XPath("//div[contains(@class,'flex items-center justify-between font-oswald')]//span[contains(@class,'text-lg font-bold text-black')]");

    // Sort dropdown
    private static readonly By SortDropdown = By.XPath("//button[@role='combobox']");

    // Favorites navigation
    private static readonly By FavoritesMenuItem = By.XPath("//div[@role='menuitem' and contains(text(),'Favorites')]");

    public ProductsPage(IDriverOperations driverOperations, IWebDriver driver) : base(driverOperations, driver)
    {
    }

    /// <summary>
    /// Opens user menu/dropdown for navigation using Actions class
    /// </summary>
    public void OpenUserMenu()
    {
        Logger.Info("Opening user menu");

        // Find the user menu button
        var userMenuBtn = Driver.FindElement(UserMenu);

        // Scroll into view first
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", userMenuBtn);
        System.Threading.Thread.Sleep(300);

        var actions = new OpenQA.Selenium.Interactions.Actions(Driver);
        actions.MoveToElement(userMenuBtn).Click().Perform();

        Logger.Info("User menu clicked");
        System.Threading.Thread.Sleep(500); // Wait for dropdown animation
    }

    /// <summary>
    /// Navigates to Favorites page via user menu or direct URL
    /// </summary>
    public void NavigateToFavorites()
    {
        Logger.Info("Navigating to Favorites page");

        try
        {
            // click user menu and select favorites
            OpenUserMenu();

            // Try to find and click favourites menu item
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
                var favoritesItem = wait.Until(d => d.FindElement(FavoritesMenuItem));

                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", favoritesItem);
                Logger.Info("Favorites menu item clicked successfully");
                System.Threading.Thread.Sleep(500);
                return;
            }
            catch (WebDriverTimeoutException ex)
            {
                Logger.Warn("Favorites menu item not visible, trying direct navigation", ex);
            }

        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to navigate to favorites: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Clicks favorite button for a product by name
    /// Product name is in the img alt attribute
    /// </summary>
    public void ClickFavoriteButtonByProductName(string productName)
    {
        var products = FindElements(ProductCards);
        if (products == null || products.Count == 0)
        {
            Logger.Warn("No products found on page");
            return;
        }

        foreach (var product in products)
        {
            var imgElement = product.FindElement(By.XPath(".//img"));
            var altText = imgElement.GetAttribute("alt");

            if (!string.IsNullOrEmpty(altText) && altText.Contains(productName))
            {
                // Find the favorite button
                var favoriteBtn = product.FindElement(By.XPath(".//following-sibling::span//button"));
                Logger.Info($"Clicking favorite button for product: {productName}");
                favoriteBtn.Click();
                System.Threading.Thread.Sleep(200);
                return;
            }
        }
        Logger.Warn($"Product '{productName}' not found");
    }

    /// <summary>
    /// Gets product names from img alt attributes (helper method - does NOT click favorites)
    /// </summary>
    public List<string> GetProductNames(int count = 0)
    {
        var productNames = new List<string>();
        var productCards = FindElements(ProductCards);

        if (productCards == null || productCards.Count == 0)
        {
            Logger.Warn("No product cards found on page");
            return productNames;
        }

        var limit = count > 0 ? Math.Min(count, productCards.Count) : productCards.Count;

        for (int i = 0; i < limit; i++)
        {
            try
            {
                // Get product name from img alt attribute
                var imgElement = productCards[i].FindElement(By.XPath(".//img"));
                var altText = imgElement.GetAttribute("alt");

                if (!string.IsNullOrWhiteSpace(altText) && altText.Length > 2)
                {
                    productNames.Add(altText);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"Could not get product name for card {i}: {ex.Message}", ex);
            }
        }

        Logger.Debug($"Retrieved {productNames.Count} product names");
        return productNames;
    }

    /// <summary>
    /// Marks specified number of products as favorites and returns their names
    /// </summary>
    public List<string> MarkProductsAsFavoriteAndGetNames(int count)
    {
        var favoriteProducts = new List<string>();
        var productCards = FindElements(ProductCards);

        if (productCards == null || productCards.Count == 0)
        {
            Logger.Warn("No products found on page");
            return favoriteProducts;
        }

        var js = (IJavaScriptExecutor)Driver;

        // Find all favorite buttons within product cards
        for (int i = 0; i < productCards.Count && favoriteProducts.Count < count; i++)
        {
            string? productName = null;

            try
            {
                // Get product name from img alt attribute
                var imgElement = productCards[i].FindElement(By.XPath(".//img"));
                productName = imgElement.GetAttribute("alt");

                if (!string.IsNullOrEmpty(productName) && productName.Length > 2)
                {
                    favoriteProducts.Add(productName);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"Could not get product name for card {i}: {ex.Message}", ex);
            }

            try
            {
                // Find the favorite button within product card
                var favoriteBtn = productCards[i].FindElement(By.XPath(".//button[contains(@class,'cursor-pointer')]"));

                // Scroll into view and click using JavaScript
                js.ExecuteScript("arguments[0].scrollIntoView(true);", favoriteBtn);
                System.Threading.Thread.Sleep(100);
                js.ExecuteScript("arguments[0].click();", favoriteBtn);

                Logger.Info($"Marked product as favorite: {productName ?? "unknown"}");
                System.Threading.Thread.Sleep(300); 
            }
            catch (Exception ex)
            {
                Logger.Warn($"Failed to click favorite button for product {productName}: {ex.Message}", ex);
            }
        }

        return favoriteProducts;
    }

    /// <summary>
    /// Selects a sort option from the combobox dropdown
    /// </summary>
    public void SelectSortOption(string sortOption)
    {
        Logger.Info($"Selecting sort option: {sortOption}");

        try
        {
            // click on the combobox
            Click(SortDropdown);
            System.Threading.Thread.Sleep(300); // dropdown animation

            var optionText = sortOption switch
            {
                "Price: Low to High" => "Low to High (Price)",
                "Price: High to Low" => "High to Low (Price)",
                "Name: A to Z" => "A to Z (Ascending)",
                "Name: Z to A" => "Z to A (Descending)",
                "Newest" => "Newest",
                _ => sortOption
            };

            var optionLocator = By.XPath($"//div[@role='option' and contains(text(),'{optionText}')]");
            var optionElement = Wait.Until(d => Driver.FindElement(optionLocator));
            optionElement.Click();

            Logger.Info($"Sort option '{sortOption}' selected successfully");
            System.Threading.Thread.Sleep(500); // Wait for sorting to apply
        }
        catch (Exception ex)
        {
            Logger.Error($"Sort dropdown not found or not available: {ex.Message}", ex);
            // Sort may not exist on this page - this is expected in some scenarios
        }
    }

    /// <summary>
    /// Gets all product prices using the price span locator
    /// </summary>
    public List<decimal> GetProductPrices()
    {
        var prices = new List<decimal>();

        // refresh
        System.Threading.Thread.Sleep(1500);

        var priceElements = FindElements(ProductPrices);

        if (priceElements == null || priceElements.Count == 0)
        {
            Logger.Warn("No price elements found on page");
            return prices;
        }

        foreach (var priceElement in priceElements)
        {
            var priceText = priceElement.Text.Trim();
            if (string.IsNullOrWhiteSpace(priceText)) continue;

            // extract price
            var cleanPrice = priceText
                .Replace("$", "")
                .Replace(" ", "")
                .Replace(" ", "")
                .Replace(",", "");

            if (decimal.TryParse(cleanPrice, out var price))
            {
                prices.Add(price);
            }
        }

        Logger.Debug($"Found {prices.Count} product prices");
        return prices;
    }

    /// <summary>
    /// Gets all products as Product objects using product cards and price spans
    /// </summary>
    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();
        var productCards = FindElements(ProductCards);
        var priceElements = FindElements(ProductPrices);

        if (productCards == null || productCards.Count == 0)
        {
            Logger.Warn("No products found on page");
            return products;
        }

        // Get product names from cards
        for (int i = 0; i < productCards.Count; i++)
        {
            var product = new Product
            {
                Name = productCards[i].Text.Split('\n').FirstOrDefault()?.Trim() ?? "",
                RawText = productCards[i].Text
            };

            // get price if available
            if (i < priceElements.Count)
            {
                var priceText = priceElements[i].Text.Trim()
                    .Replace("$", "").Replace(",", "");
                if (decimal.TryParse(priceText, out var price))
                {
                    product.Price = price;
                }
            }

            products.Add(product);
        }

        Logger.Debug($"Retrieved {products.Count} products");
        return products;
    }

    /// <summary>
    /// Gets the count of products on the page
    /// </summary>
    public int GetProductCount()
    {
        var cards = FindElements(ProductCards);
        return cards?.Count ?? 0;
    }


/*    public bool IsProductsPageDisplayed()
    {
        return GetProductCount() > 0;
    }

    public bool IsSortDropdownAvailable()
    {
        return IsElementVisible(SortDropdown);
    }*/
}