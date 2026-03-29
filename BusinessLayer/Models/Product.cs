namespace BusinessLayer.Models;

public class Product
{
    /// <summary>
    /// Product name, used for identification and comparison in tests
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public string? RawText { get; set; }

    public override string ToString() => $"Product: {Name}, Price: {Price}";
    /// <summary>
    /// Override Equals to compare products by name 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is Product other)
        {
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
    /// <summary>
    /// Override GetHashCode to be consistent with Equals
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Name.ToLowerInvariant().GetHashCode();
}
