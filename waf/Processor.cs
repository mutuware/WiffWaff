using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WiffWaff.Pages;

namespace WiffWaff
{
    public class Processor
    {
        private readonly Router _router;
        private readonly Renderer _renderer;
        private readonly ClientApp _clientApp;

        public Processor(Router router, Renderer renderer, ClientApp clientApp)
        {
            _router = router;
            _renderer = renderer;
            _clientApp = clientApp;
        }

        public async Task DoAsync(HttpListenerContext context)
        {
            var url = context.Request.Url.AbsolutePath;
            var httpMethod = context.Request.HttpMethod;
            var body = await context.Request.InputStream.GetBodyContentAsStringAsync();

            Console.WriteLine($"{DateTime.Now} {url} {httpMethod}");

            if (await StaticFile(context, url))
                return;

            // match app class to url
            var type = _router.GetRoute(context.Request.Url.AbsolutePath);

            // get app response
            Page page = _clientApp.InvokeApp(type, httpMethod, body);

            // render page as appropriate
            var html = _renderer.Do(page, type, _router.Routes);

            await context.Response.WriteStringAsync(html);
        }

        // e.g. styles.css, favicon.ico
        private async Task<bool> StaticFile(HttpListenerContext context, string url)
        {
            var file = $"static{url}";
            if (!File.Exists(file))
                return false;
            
            var data = await File.ReadAllTextAsync(file);
            await context.Response.WriteStringAsync(data);
            return true;
        }
    }
}