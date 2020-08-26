using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WiffWaff
{
    public class Router
    {
        public ReadOnlyDictionary<string, Type> Routes;
        private const string PAGES = ".Pages";

        public Router()
        {
            var routes = Assembly.GetEntryAssembly().GetTypes()
                .Where(x => x.Namespace.Contains(PAGES))
                .ToDictionary(k => ToUrl(k.FullName));

            Routes = new ReadOnlyDictionary<string, Type>(routes);

            foreach (var route in Routes)
            {
                Console.WriteLine($"Route {route.Key} => {route.Value.Name}");
            }
        }

        public Type GetRoute(string url)
        {
            if (!Routes.TryGetValue(url.ToLower(), out Type type))
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