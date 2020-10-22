using crawler_line.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace crawler_line.Util
{
    public class WebUtil
    {
        private static readonly string DefaultDownLoadPath = Path.Combine(Environment.CurrentDirectory,"Download");

        public static string GetHtmlSourceAsync(string url,Encoding encoding = null,string userAgent = "",string accept = "")
        {
            if (string.IsNullOrEmpty(url))
                return "";

            HttpClient httpClient = new HttpClient();

            if(string.IsNullOrEmpty(userAgent))
            {
                userAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
            }

            if(string.IsNullOrEmpty(accept))
            {
                accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            }

            httpClient.DefaultRequestHeaders.Add("user-agent", userAgent);
            httpClient.DefaultRequestHeaders.Add("accept", accept);

            var task = httpClient.GetByteArrayAsync(url);
            task.Wait();
            var buffer = task.Result;

            //todo get correct encoding
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetString(buffer);
        }

        public static List<string> GetImageAsync(string url)
        {
            var htmlSource = GetHtmlSourceAsync(url);

            if (string.IsNullOrEmpty(htmlSource))
                return new List<string>();

            return RegexUtil.MatchImg(htmlSource);
        }

        public static List<string> GetVideoAsync(string url)
        {
            var htmlSource = GetHtmlSourceAsync(url);

            if (string.IsNullOrEmpty(htmlSource))
                return new List<string>();

            return RegexUtil.MatchVideo(htmlSource);
        }

        public static HttpHeader GetHeader(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return new HttpHeader()
                    {
                        CharSet = response.CharacterSet,
                        ContentEncoding = response.ContentEncoding,
                        ContentLength = response.ContentLength,
                        ContentType = response.ContentType,
                        LastModified = response.LastModified,
                        Server = response.Server,
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch
            {
                return new HttpHeader();
            }
        }

        public static void DownloadFileAsync(string url, string downloadPath = "",int timeout = 3000)
        {
            if (GetHeader(url).StatusCode != HttpStatusCode.OK)
                return;

            WebClient client = new WebClient();
            var fileName = "";

            if (string.IsNullOrEmpty(downloadPath))
            {
                downloadPath = DefaultDownLoadPath;
                fileName = Path.Combine(downloadPath, ExtractFileName(url));
            }

            if (System.IO.Directory.Exists(downloadPath) == false)
                System.IO.Directory.CreateDirectory(downloadPath);

            var task = client.DownloadFileTaskAsync(new Uri(url), fileName);
            task.Wait(timeout);         
        }

        /// <summary>
        /// https://dldir1.qq.com/qqfile/qq/QQ9.1.0/24712/QQ9.1.0.24712.exe
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ExtractFileName(string url)
        {
            return url.Substring(url.LastIndexOf("/") + 1);
        }
    }
}
