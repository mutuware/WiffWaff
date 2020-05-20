using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace WiffWaff
{
    public class Processor
    {
        private Router _router;

        public Processor(Router router)
        {
            _router = router;
        }

        public async Task DoAsync(HttpListenerContext context)
        {
            Console.WriteLine($"{DateTime.Now} {context.Request.RawUrl} {context.Request.HttpMethod}");
            var url = context.Request.Url.AbsolutePath.ToLower();

            if (!_router.Routes.TryGetValue(url, out Type type))
            {
                throw new KeyNotFoundException("Invalid route!");
            }

            var httpVerb = context.Request.HttpMethod; // e.g. GET, POST

            // instantiate matched type transiently and execute http verb as method. e.g. Index.Get()
            var instance = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(httpVerb, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
                throw new Exception($"Cannot find method on {type} that matches HTTP verb {httpVerb}");

            string routeResponse = "";
            if (method.ReturnType == typeof(string))
            {
                routeResponse = (string)method.Invoke(instance, null);
            }
            else
            {
                // do something else
                routeResponse = method.Invoke(instance, null).ToString();
            }

            // masterpage
            var outerHtml = "<HTML><BODY>{{content}}</BODY></HTML>";
            var html = outerHtml.Replace("{{content}}", routeResponse);

            await context.Response.WriteStringAsync(html);
        }
    }
}
