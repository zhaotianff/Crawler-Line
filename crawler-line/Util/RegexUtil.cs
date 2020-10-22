using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace crawler_line.Util
{
    public class RegexUtil
    {
        private static readonly string MatchUrlPattern = "http|https://\\S+\\.\\S+";
        private static readonly string MatchImagePattern = "(ftp|http|https)://(\\S*/)+\\S*.(png|jpg|gif|jiff|jpeg|bmp)";
        public const string MatchTagImgPattern = "<img\\s+src=\"(?<image>\\S+)\"(.*)/>";
        public const string MatchVideoPattern = "(ftp|http|https)://(\\S*/)+\\S*.(mp4|avi|flv|mpg)";
        public const string MatchTagVideoPattern = "<video(.*)src=\"(?<video>\\S+)\"(.*)/>";

        public static bool MatchUrl(string url)
        {
            return Regex.IsMatch(url, MatchUrlPattern);
        }

        public static List<string> MatchImg(string input,int timeoutSeconds = 2)
        {
            var list = new List<string>();
            var matches1 = Regex.Matches(input, MatchImagePattern, RegexOptions.None, TimeSpan.FromSeconds(timeoutSeconds));
            var matches2 = Regex.Matches(input, MatchTagImgPattern, RegexOptions.None, TimeSpan.FromSeconds(timeoutSeconds));

            if (matches1 != null && matches1.Count > 0)
                list.AddRange(matches1.Select(x => x.Value));

            if (matches2 != null && matches2.Count > 0)
                list.AddRange(matches2.Select(x => x.Groups["image"].Value));

            return list;
        }

        public static List<string> MatchVideo(string input,int timeoutSecondes = 2)
        {
            var list = new List<string>();
            var matches1 = Regex.Matches(input, MatchVideoPattern, RegexOptions.None, TimeSpan.FromSeconds(timeoutSecondes));
            var matches2 = Regex.Matches(input, MatchTagVideoPattern, RegexOptions.None, TimeSpan.FromSeconds(timeoutSecondes));

            if (matches1 != null && matches1.Count > 0)
                list.AddRange(matches1.Select(x => x.Value));

            if (matches2 != null && matches2.Count > 0)
                list.AddRange(matches2.Select(x => x.Groups["video"].Value));

            return list;
        }
    }
}
