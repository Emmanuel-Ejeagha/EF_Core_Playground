using System;

namespace A_Routing;

public class ProductService
{
    private static readonly IDictionary<string, Product> _allProducts
        = new Dictionary<string, Product>
        {
            {"big-widget", new Product("Big Widget", 123)} ,
            {"super-fancy-widget", new Product("Super fancy widget", 456)}
        };
    public Product GetProduct(string productName)
    {
        if (_allProducts.TryGetValue(productName, out var product))
        {
            return product;
        }
        return null;
    }

    public List<Product> Search(string term, StringComparison comparisonType)
    {
        return _allProducts
            .Where(x => x.Value.Name.Contains(term, comparisonType))
            .Select(x => x.Value)
            .ToList();
    }
}
