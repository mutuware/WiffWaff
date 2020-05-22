using System.Text;

namespace WiffWaff.Pages
{
    public class WebPage : Page
    {
        public string Header { get; set; }
        public string Text { get; set; }

        public override string GetContents()
        {
            var sb = new StringBuilder();
            sb.Append("<H1>");
            sb.Append(Header);
            sb.Append("</H1>");

            sb.Append("<P>");
            sb.Append(Text);
            sb.Append("</P>");

            return sb.ToString();
        }
    }
}
