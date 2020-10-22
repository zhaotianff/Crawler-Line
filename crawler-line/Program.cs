using crawler_line.Model;
using crawler_line.Util;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static System.Console;

namespace crawler_line
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument<string>(
                    "url","web site url"),
                new Option<bool>(new string[]{ "--get-html" ,"-html"},"Get html source"),
                new Option<bool>(new string[]{ "--get-image" ,"-image"},"Get images"),
                new Option<bool>(new string[]{ "--get-video","-video"},"Get videos"),
                new Option<bool>(new string[]{ "--loop-image", "-loopi"},"Loop get images"),
                new Option<bool>(new string[]{ "--regex-option" ,"-regex"},"Use regex"),
                new Option<bool>(new string[]{ "--htmlagilitypack-option", "-agpack"},"Use HtmlAgilityPack"),
                new Option<bool>(new string[]{ "--anglesharp-option", "-agsharp"},"Use AngleSharp"),
                new Option<string>(new string[]{ "--download-path" ,"-path"},getDefaultValue:()=>"D:\\download","Designate download path"),              
            };

            rootCommand.Description = ".Net Core command-line crawler.";
            rootCommand.TreatUnmatchedTokensAsErrors = true;
            var githubCommand = new Command("github", "fork me on github");
            githubCommand.Handler = CommandHandler.Create(() => { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd", $"/c start https://github.com/zhaotianff/Crawler-Line")); });
            rootCommand.AddCommand(githubCommand);
            rootCommand.Handler = CommandHandler.Create<CrawlerOption>(Crawler);
            return rootCommand.InvokeAsync(args).Result;
        }

        static void Crawler(CrawlerOption crawlerOption)
        {
            if (string.IsNullOrEmpty(crawlerOption.Url))
            {
                WriteLine("url is required");
                return;
            }

            if(RegexUtil.MatchUrl(crawlerOption.Url) == false)
            {
                WriteLine("illegal url");
                return;
            }

            if(crawlerOption.GetHtml == true)
            {
                GetHtmlAsync(crawlerOption.Url);
            }

            if(crawlerOption.GetImage == true)
            {
                GetImageAsync(crawlerOption.Url);
            }

            if(crawlerOption.GetVideo == true)
            {
                GetVideoAsync(crawlerOption.Url);
            }
        }


        static void GetHtmlAsync(string url)
        {
            var html =  WebUtil.GetHtmlSourceAsync(url);
            WriteLine(html);
        }

        static void GetImageAsync(string url)
        {
            var imgList = WebUtil.GetImageAsync(url);
            foreach (var item in imgList)
            {
                WriteLine(item);
                try
                {
                    WebUtil.DownloadFileAsync(item);
                }
                catch
                {
                    WriteErrorInfo($"Down {item} failed.");
                    continue;
                }
            }
        }

        static void GetVideoAsync(string url)
        {
            var videoList = WebUtil.GetVideoAsync(url);

            foreach (var item in videoList)
            {
                WriteLine(item);
                try
                {
                    WebUtil.DownloadFileAsync(item);
                }
                catch
                {
                    WriteErrorInfo($"Down {item} failed.");
                }
            }
        }

        static void WriteErrorInfo(string info)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(info);
            ForegroundColor = ConsoleColor.White;
        }

    }
}
