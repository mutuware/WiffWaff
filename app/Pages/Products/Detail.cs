using App.Models;

namespace App.Pages.Products
{
    public class Detail
    {
        private static Product _product = new Product
        {
            Name = "Widget",
            Description = "One size fits all widget!",
            Category = "Mechanical",
            Price = 99.99m,
            InStock = true
        };

        public Product Get() => _product;

        public void Post(Product body) => _product = body;

}
}
