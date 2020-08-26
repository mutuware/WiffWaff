using WiffWaff.Pages;

namespace App.Pages
{
    public class About
    {
        public Page Get()
        {
            return new WebPage { Header = "About Page", Text = "Welcome to the WiffWaff App" };
        }
    }
}
