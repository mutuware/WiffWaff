using WiffWaff.Pages;

namespace App.Pages
{
    public class About
    {
        public Page Get()
        {
            return new WebPage { Text = "Welcome to the WiffWaff App" };
        }
    }
}
