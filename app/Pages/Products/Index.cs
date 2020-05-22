using WiffWaff.Pages;

namespace App.Pages.Products
{
    public class Index
    {
        public WebPage Get() => new WebPage
        {
            Text = "This is the product page"
        };
    }
}
