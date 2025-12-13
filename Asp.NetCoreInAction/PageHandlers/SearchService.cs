using System;

namespace PageHandlers;

public class SearchService
{
    private static readonly List<Product> _item = new List<Product>
    {
        new Product{Name="iPad"},
        new Product{Name="iPod"},
        new Product{Name="iMac"},
        new Product{Name="Mac Pro"},
        new Product{Name="Mac mini"},
    };

    public List<Product> SearchProducts(string term)
    {
        // filter by the provided category
        return _item.Where(x => x.Name.Contains(term)).ToList();
    }
}
