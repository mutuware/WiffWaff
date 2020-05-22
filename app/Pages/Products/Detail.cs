using App.Models;
using System;

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

        public string Post() => "Post has been delivered!";

    }
}
