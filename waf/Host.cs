using System;
using System.Net;
using System.Threading.Tasks;

namespace WiffWaff
{
    public static class Host
    {
        private static HttpListener _listener = new HttpListener();
        private static Processor _processor = new Processor(new Router(), new Renderer(), new ClientApp());
        private const string BASEURL = "http://localhost:8888/";

        public static async Task Run()
        {
            _listener.Prefixes.Add(BASEURL);
            _listener.Start();
            Console.WriteLine($"Listening at {BASEURL}");

            while (true)
            {
                var context = await _listener.GetContextAsync();

                try
                {
                    await _processor.DoAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await context.Response.WriteStringAsync("Oh noes! " + ex.Message, 500);
                }
            }
        }
    }
}
