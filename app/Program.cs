using System.Threading.Tasks;
using waf;

namespace app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.Run(
                new HostOptions
                {
                    AppAssembly = typeof(Program).Assembly
                }
            );
        }
    }
}
