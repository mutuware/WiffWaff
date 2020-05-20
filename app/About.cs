using waf;

namespace app
{
    public class About
    {
        [Route("/about", "GET")]
        public static string Get()
        {
            return "About page";
        }
    }
}
