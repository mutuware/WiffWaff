using System;
using WiffWaff.Pages;

namespace WiffWaff
{
    public class Renderer
    {
        public string Do(Page page, Type type)
        {
                var masterPage = "<HTML><HEAD><link rel=\"stylesheet\" type=\"text/css\" href=\"/style.css\"><TITLE>{{title}}</TITLE></HEAD><BODY><div class=\"container\">{{content}}</div></BODY></HTML>";
                var html = masterPage
                    .Replace("{{title}}", type.FullName)
                    .Replace("{{content}}", page.GetContents());

            return html;
        }
    }
}
