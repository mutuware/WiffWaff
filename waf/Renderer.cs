using System;
using System.Collections.ObjectModel;
using System.Text;
using WiffWaff.Pages;

namespace WiffWaff
{
    public class Renderer
    {
        public string Do(Page page, Type type, ReadOnlyDictionary<string, Type> routes)
        {
            var masterPage =
                "<HTML><HEAD><link rel=\"stylesheet\" type=\"text/css\" href=\"/style.css\"><TITLE>{{title}}</TITLE></HEAD><BODY><div class=\"navbar\">{{navbar}}</div><div class=\"container\">{{content}}</div></BODY></HTML>";
            var html = masterPage
                .Replace("{{title}}", type.FullName)
                .Replace("{{navbar}}", Navbar(routes))
                .Replace("{{content}}", page.GetContents());

            return html;
        }

        private string Navbar(ReadOnlyDictionary<string, Type> routes)
        {
            var sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var route in routes)
            {
                sb.Append($"<li><a href=\"{route.Key}\">{route.Value}</a></li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}
