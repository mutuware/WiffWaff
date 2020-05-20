using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WiffWaff
{
    public class Router
    {
        public Dictionary<string, Type> Routes { get; }
        private const string PAGES = ".Pages";

        public Router()
        {
            Routes = Assembly.GetEntryAssembly().GetTypes()
                .Where(x => x.Namespace.Contains(PAGES))
                .ToDictionary(k => ToUrl(k.FullName));

            foreach (var route in Routes)
            {
                Console.WriteLine($"Route {route.Key} => {route.Value.Name}");
            }
        }

        // Go from fullname App.Pages.Products.Index to url /Products/Index
        private string ToUrl(string typeName)
        {
            var remainder = typeName.ToLower().Substring(typeName.IndexOf(PAGES) + PAGES.Length);
            return remainder.Replace('.', '/');
        }
    }
}