using OpenQA.Selenium;
using BusinessLayer.Driver;
using BusinessLayer.Models;

namespace BusinessLayer.PageObjects;

/// <summary>
/// Favorites page object - handles favorites page interactions
/// </summary>
public class FavoritesPage : BasePage
{
    // locators based on actual site HTML
    private static readonly By FavoriteProducts = By.XPath("//div[contains(@class,'flex flex-col gap-3 relative group')]");
    private static readonly By EmptyFavoritesMessage = By.XPath("//p[contains(text(),'No favorites') or contains(text(),'empty') or contains(text(),'No items')]");
    private static readonly By PageTitle = By.XPath("//h1[contains(text(),'Favorites')] | //h2[contains(text(),'Favorites')]");

    public FavoritesPage(IDriverOperations driverOperations, IWebDriver driver) : base(driverOperations, driver)
    {
    }

    /// <summary>
    /// Get all favorite products as Product objects
    /// </summary>
    public List<Product> GetFavoriteProducts()
    {
        var products = new List<Product>();
        var productElements = FindElements(FavoriteProducts);
        
        foreach (var element in productElements)
        {
            var nameElement = element.FindElement(By.XPath(".//a[contains(@class,'font-oswald')]"));
            var name = nameElement.Text.Trim();
            
            var priceSpan = element.FindElement(By.XPath(".//span[contains(@class,'text-lg font-bold text-black')]"));
            var priceText = priceSpan.Text.Trim().Replace("$", "").Replace(",", "");
            decimal.TryParse(priceText, out var price);
            
            products.Add(new Product { Name = name, Price = price, RawText = element.Text });
        }
        
        Logger.Info($"Found {products.Count} favorite products");
        return products;
    }

    /// <summary>
    /// Get list of favorite product names
    /// </summary>
    public List<string> GetFavoriteProductNames()
    {
        var productNames = new List<string>();
        var productElements = FindElements(FavoriteProducts);
        
        foreach (var element in productElements)
        {
            try
            {
                var nameElement = element.FindElement(By.XPath(".//a[contains(@class,'font-oswald')]"));
                var name = nameElement.Text.Trim();
                if (!string.IsNullOrEmpty(name) && name.Length > 2)
                {
                    productNames.Add(name);
                }
            }
            catch { }
        }
        
        return productNames;
    }

    /// <summary>
    /// Get count of favorite products
    /// </summary>
    public int GetFavoriteProductCount() => FindElements(FavoriteProducts).Count;

    /// <summary>
    /// Check if there are any favorite products
    /// </summary>
    public bool HasFavoriteProducts() => GetFavoriteProductCount() > 0;

    /// <summary>
    /// Check if empty favorites message is displayed
    /// </summary>
    public bool IsEmptyFavoritesMessageDisplayed() => IsElementVisible(EmptyFavoritesMessage);

    /// <summary>
    /// Check if favorites page is displayed
    /// </summary>
    public bool IsFavoritesPageDisplayed() => IsElementVisible(PageTitle) || IsElementVisible(FavoriteProducts);

    /// <summary>
    /// Check if specific product is in favorites
    /// </summary>
    public bool ContainsProduct(string productName)
    {
        var productNames = GetFavoriteProductNames();
        var contains = productNames.Any(n => n.Contains(productName, StringComparison.OrdinalIgnoreCase));
        Logger.Debug($"Checking '{productName}' in favorites: {contains}");
        return contains;
    }
}
