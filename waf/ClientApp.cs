using System;
using System.Linq;
using System.Reflection;
using System.Web;
using WiffWaff.Pages;

namespace WiffWaff
{
    public class ClientApp
    {
        // instantates the type and invokes the client method code based on the httpVerb.
        public Page InvokeApp(Type type, string httpVerb, string body)
        {
            var instance = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(httpVerb, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
                throw new Exception($"Cannot find method on {type} that matches HTTP verb {httpVerb}");

            // if POST then add body to app method call
            object[] paramsArray = null;
            if (httpVerb.ToLower() == "post")
            {
                var bodyObj = ParseBody(body, method);

                paramsArray = new object[1] { bodyObj };
            }

            // handle simple string signature
            if (method.ReturnType == typeof(string))
            {
                var page = new TextPage
                {
                    Text = (string)method.Invoke(instance, paramsArray),
                };
                return page;
            }
            else if (method.ReturnType.IsAssignableFrom(typeof(WebPage)))
            {
                return (Page)method.Invoke(instance, paramsArray);
            }
            // if return type is T create a Detail page.
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
            else if(method.ReturnType== typeof(void))
            {
                method.Invoke(instance, paramsArray); // Does POST then calls equivalent GET method
                return InvokeApp(type, "GET", "");
            }
            else
            {
                throw new Exception($"Unknown method return type:{method.ReturnType}");
            }
        }

        private object ParseBody(string body, MethodInfo method)
        {
            // parse to key/value
            var pairs = body.Split('&');
            var dict = pairs.ToDictionary(x => x.Split('=')[0], x => (object)HttpUtility.HtmlDecode(x.Split('=')[1]));

            var paramType = method.GetParameters()[0].ParameterType;
            var instance = Activator.CreateInstance(paramType);

            foreach (var pi in paramType.GetProperties())
            {
                if(!dict.TryGetValue(pi.Name, out var dictValue))
                {
                    pi.SetValue(instance, default); // POST doesn't contain checkboxes which are empty
                }
                else
                {
                    var convertedValue = Convert.ChangeType(dictValue, pi.PropertyType);
                    pi.SetValue(instance, convertedValue);
                }
            }

            return instance;
        }
    }
}
