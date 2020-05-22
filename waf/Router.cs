using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WiffWaff
{
    public class Router
    {
        private Dictionary<string, Type> _routes;
        private const string PAGES = ".Pages";

        public Router()
        {
            _routes = Assembly.GetEntryAssembly().GetTypes()
                .Where(x => x.Namespace.Contains(PAGES))
                .ToDictionary(k => ToUrl(k.FullName));

            foreach (var route in _routes)
            {
                Console.WriteLine($"Route {route.Key} => {route.Value.Name}");
            }
        }

        public Type GetRoute(string url)
        {
            if (!_routes.TryGetValue(url.ToLower(), out Type type))
            {
                throw new KeyNotFoundException("Invalid route!");
            }

            return type;
        }

        // Go from fullname App.Pages.Products.Index to url /Products/Index
        private string ToUrl(string typeName)
        {
            var remainder = typeName.ToLower().Substring(typeName.IndexOf(PAGES) + PAGES.Length);
            return remainder.Replace('.', '/');
        }
    }
}