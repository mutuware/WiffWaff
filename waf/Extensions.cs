﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WiffWaff
{
    public static class Extensions
    {
        public static async Task WriteStringAsync(this HttpListenerResponse httpListenerResponse, string contents, int statusCode = 200)
        {
            var buffer = Encoding.UTF8.GetBytes(contents);
            // Get a response stream and write the response to it.
            httpListenerResponse.StatusCode = statusCode;
            httpListenerResponse.ContentLength64 = buffer.Length;
            await httpListenerResponse.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            httpListenerResponse.OutputStream.Close();
        }

        public static Dictionary<string, object> GetPropertyNamesAndValues(this object obj)
        {
            var dict = new Dictionary<string, object>();

            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                dict[pi.Name] = pi.GetValue(obj, null);
            }

            return dict;
        }

		public async static Task<string> GetBodyContentAsStringAsync(this Stream stream)
        {
            string content = string.Empty;

            using (var readStream = new StreamReader(stream, Encoding.UTF8))
            {
                content = await readStream.ReadToEndAsync();
            }

            return content;
        }
	}
}
