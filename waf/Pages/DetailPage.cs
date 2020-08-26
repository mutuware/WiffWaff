using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WiffWaff.Pages
{
    public class DetailPage : Page
    {
        public Dictionary<string, object> Fields { get; set; }

        public override string GetContents()
        {
            var sb = new StringBuilder();
            sb.Append("<form method=\"post\" id='detail'>");
            foreach (var field in Fields)
            {
                var inputType = GetInputType(field.Value);
                var stringValue = field.Value is string ? HttpUtility.UrlDecode(field.Value.ToString()) : field.Value.ToString();

                sb.Append($"<p><label>{field.Key}</label><input type=\"{inputType}\" name=\"{field.Key}\" value=\"{stringValue}\"/></p>");
            }
            sb.Append("<input type=\"submit\" value=\"Submit\"/>");
            sb.Append("</form>");


            return sb.ToString();
        }

        private string GetInputType(object value)
        {
            var typeName = value.GetType().Name.ToLower();
            var length = value.ToString().Length;

            switch (typeName)
            {
                case "string":
                    return length < 15 ? "text" : "textarea";
                case "boolean":
                    return "checkbox";
                default:
                    return "text";
            }
        }
    }
}
