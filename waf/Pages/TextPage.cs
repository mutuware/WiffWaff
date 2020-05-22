namespace WiffWaff.Pages
{
    public class TextPage : Page
    {
        public string Text { get; set; }

        public override string GetContents()
        {
            return Text;
        }
    }
}
