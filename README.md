# WiffWaff

Turns this class

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

into this page:

[https://raw.githubusercontent.com/mutuware/WiffWaff/master/docs/Product-Detail.png]
