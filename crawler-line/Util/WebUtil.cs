using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace crawler_line.Util
{
    public class WebUtil
    {
        public static async void GetHtmlSource(string url)
        {
            HttpClient httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            Console.WriteLine(html);
        }
    }
}
