using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace WiffWaff
{
    public static class Host
    {
        private static HttpListener _listener;
        private static Assembly _appAssembly;
        private static Dictionary<string, Type> _routes;

        public static async Task Run()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8888/");
            Console.WriteLine($"Listening at {_listener.Prefixes.First()}");

            _appAssembly = Assembly.GetEntryAssembly();
            _routes = BuildRoutes(_appAssembly);
            foreach(var route in _routes)
            {
                Console.WriteLine($"Route {route.Key} => {route.Value.Name}");
            }

            _listener.Start();

            while (true)
            {
                var context = await _listener.GetContextAsync();
                Console.WriteLine($"{DateTime.Now} {context.Request.RawUrl} {context.Request.HttpMethod}");
                Process(context);
            }
        }

        private static Dictionary<string, Type> BuildRoutes(Assembly appAssembly)
        {
            return appAssembly.GetTypes()
                .Where(x => x.Namespace.Contains(".Pages"))
                .ToDictionary(k => "/" + k.Name.ToLower());
        }

        private static void Process(HttpListenerContext context)
        {
            try
            {
                var url = context.Request.Url.AbsolutePath.ToLower();

                if (!_routes.TryGetValue(url, out Type type))
                {
                    throw new KeyNotFoundException("Invalid route!");
                }

                var httpVerb = context.Request.HttpMethod; // e.g. GET, POST

                // instantiate matched type transiently
                var instance = Activator.CreateInstance(type);
                MethodInfo method = type.GetMethod(httpVerb, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                var routeResponse = (string)method.Invoke(instance, null);

                var outerHtml = "<HTML><BODY>{{content}}</BODY></HTML>";
                var html = outerHtml.Replace("{{content}}", routeResponse);

                var buffer = System.Text.Encoding.UTF8.GetBytes(html);
                // Get a response stream and write the response to it.
                context.Response.ContentLength64 = buffer.Length;
                var output = context.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Response.StatusCode = 500;
                var buffer = System.Text.Encoding.UTF8.GetBytes("Oh noes!");
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
            }
        }
    }
}
