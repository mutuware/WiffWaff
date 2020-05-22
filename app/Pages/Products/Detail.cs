using App.Models;

namespace App.Pages.Products
{
    public class Detail
    {
        public Product Get() => new Product
        {
            Name = "Widget",
            Description = "One size fits all widget!",
            Category = "Mechanical",
            Price = 99.99m,
            InStock = true
        };

        public string Post(Product body) => $"Post has been delivered!:{body}";

    }
}
