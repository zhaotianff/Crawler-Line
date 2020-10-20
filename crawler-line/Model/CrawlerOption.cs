using System;
using System.Collections.Generic;
using System.Text;

namespace crawler_line.Model
{
    public class CrawlerOption
    {
        public string Url { get; set; }
        public bool GetHtml { get; set; }
        public bool GetImage { get; set; }
        public bool RegexOption { get; set; }
        public bool HtmlagilitypackOption { get; set; }
        public bool AnglesharpOption { get; set; }
        public string DownloadPath { get; set; }
    }
}
