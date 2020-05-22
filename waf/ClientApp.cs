using System;
using System.Reflection;
using System.Windows.Markup;
using WiffWaff.Pages;

namespace WiffWaff
{
    public class ClientApp
    {
        // instantates the type and invokes the client method code based on the httpVerb.
        public Page InvokeApp(Type type, string httpVerb)
        {
            var instance = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(httpVerb, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
                throw new Exception($"Cannot find method on {type} that matches HTTP verb {httpVerb}");

            if (method.ReturnType == typeof(string))
            {
                var page = new TextPage
                {
                    Text = (string)method.Invoke(instance, null),
                };
                return page;
            }
            else if (method.ReturnType.IsAssignableFrom(typeof(WebPage)))
            {
                return (Page)method.Invoke(instance, null);
            }
            else if (method.ReturnType.IsClass)
            {
                var obj = method.Invoke(instance, null);
                var values = obj.GetPropertyNamesAndValues();
                var page = new DetailPage
                {
                    Fields = values
                };
                return page;
            }
            else
            {
                throw new Exception($"Unknown method return type:{method.ReturnType}");
            }
        }
    }
}
