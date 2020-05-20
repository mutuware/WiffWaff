using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace waf
{
    public static class Host
    {
        private static HttpListener _listener;
        private static Dictionary<string, MethodInfo> _routes;

        public static async Task Run(HostOptions hostOptions)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8888/");

            _routes = BuildRoutes(hostOptions.AppAssembly);
            Console.WriteLine($"Listening at {_listener.Prefixes.First()}");
            _listener.Start();

            while (true)
            {
                var context = await _listener.GetContextAsync();
                Console.WriteLine($"{DateTime.Now} {context.Request.RawUrl} {context.Request.HttpMethod}");
                Process(context);
            }
        }

        private static Dictionary<string, MethodInfo> BuildRoutes(Assembly appAssembly)
        {
            var attrs = appAssembly.GetTypes()
                .SelectMany(x => x.GetMethods())
                .Select(x => new { Attr = x.GetCustomAttribute<RouteAttribute>(false), MI = x })
                .Where(x => x.Attr != null)
                .ToList();

            var dict = new Dictionary<string, MethodInfo>();
            foreach (var attr in attrs)
            {
                dict.Add(attr.Attr.Url, attr.MI);
            }

            return dict;
            //return new Dictionary<string, Func<string>>
            //{
            //    {"/index", () => "hello world" },
            //    {"/about", () => "welcome to about" },
            //};
        }

        private static void Process(HttpListenerContext context)
        {
            try
            {
                var url = context.Request.Url.AbsolutePath;

                if (!_routes.TryGetValue(url, out MethodInfo funcResponse))
                {
                    throw new KeyNotFoundException("Invalid route!");
                }

                var routeResponse = (string)funcResponse.Invoke(null, null); // need instance for non-static methods

                var outerHtml = "<HTML><BODY>{{content}}</BODY></HTML>";
                var html = outerHtml.Replace("{{content}}", routeResponse);

                var buffer = System.Text.Encoding.UTF8.GetBytes(html);
                // Get a response stream and write the response to it.
                context.Response.ContentLength64 = buffer.Length;
                var output = context.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            catch(Exception ex)
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
