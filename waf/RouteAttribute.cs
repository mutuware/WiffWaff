using System;

namespace waf
{
    public class RouteAttribute : Attribute
    {
        public string Url { get; }
        public string Verb { get; }

        public RouteAttribute(string url, string verb)
        {
            Url = url;
            Verb = verb;
        }
    }
}
