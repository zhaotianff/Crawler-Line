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

        public static bool MatchUrl(string url)
        {
            return Regex.IsMatch(url, MatchUrlPattern);
        }

        public static List<string> MatchImg(string input)
        {
            return Regex.Matches(input, MatchImagePattern)?.Select(x => x.Value).ToList();
        }
    }
}
