using crawler_line.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace crawler_line.Util
{
    public class WebUtil
    {
        private static string Url = "";

        public static bool SetUrl(string url)
        {
            Url = url;
            return RegexUtil.MatchUrl(url);
        }        

        public static async Task<string> GetHtmlSourceAsync()
        {
            if (string.IsNullOrEmpty(Url))
                return "";

            HttpClient httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(Url);
            return html;
        }

        public static async Task<List<string>> GetImageAsync()
        {
            var htmlSource = await GetHtmlSourceAsync();

            if (string.IsNullOrEmpty(htmlSource))
                return new List<string>();

            return RegexUtil.MatchImg(htmlSource);
        }
    }
}
