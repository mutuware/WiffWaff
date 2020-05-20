using waf;

namespace app
{
    public class Index
    {
        [Route("/index", "GET")]
        public static string Get()
        {
            return "Hello World";
        }
    }
}
