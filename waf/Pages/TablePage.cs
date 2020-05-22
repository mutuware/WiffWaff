using System.Collections.Generic;
using System.Text;

namespace WiffWaff.Pages
{
    public class TablePage : Page
    {
        public Dictionary<string,string> Values { get; set; }

        public override string GetContents()
        {
            var sb = new StringBuilder();
            sb.Append("<table id='detail'><tbody>");
            foreach (var value in Values)
            {
                sb.Append($"<tr><td class='name'>{value.Key}</td><td class='value'>{value.Value}</td></tr>");
            }

            sb.Append("</tbody></table>");

            return sb.ToString();
        }
    }
}
