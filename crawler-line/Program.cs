using crawler_line.Model;
using crawler_line.Util;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;

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
                new Option<bool>(new string[]{ "--gethtml" ,"-html"},"Get html source"),
                new Option<bool>(new string[]{ "--getimage" ,"-image"},"Get images"),
                new Option<bool>(new string[]{ "--regex-option" ,"-regex"},"Use regex"),
                new Option<bool>(new string[]{ "--htmlagilitypack-option", "-agpack"},"Use HtmlAgilityPack"),
                new Option<bool>(new string[]{ "--anglesharp-option", "-agsharp"},"Use AngleSharp"),
                new Option<string>(new string[]{ "--download-path" ,"-path"},getDefaultValue:()=>"D:\\download","Designate download path"),
            };

            //也可以以这种方式来初始化RootCommand
            //var rootCommand = new RootCommand();
            //添加 Argument
            //rootCommand.AddArgument(new Argument<string>("url","web site url"));
            //添加 Option
            //rootCommand.AddOption(new Option<string>(new string[] {"--download-path","-path" },"download path"));

            rootCommand.Description = ".Net Core command-line crawler.";
            rootCommand.TreatUnmatchedTokensAsErrors = true;

            //添加 Command
            var githubCommand = new Command("github", "fork me on github");
            //添加 Command的处理函数
            githubCommand.Handler = CommandHandler.Create(() => { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd", $"/c start https://github.com/zhaotianff/Crawler-Line")); });
           
            var subcommand = new Command("sub", "subcommand");
            subcommand.Handler = CommandHandler.Create(()=> { });
            githubCommand.AddCommand(subcommand);

            //将Command添加 到RootCommand
            rootCommand.AddCommand(githubCommand);

            //rootCommand.Handler = CommandHandler.Create<CrawlerOption>((crawlerOption) =>
            //{

            //});

            rootCommand.Handler = CommandHandler.Create<string, bool, bool, bool, bool, bool, string>((string url, bool html, bool image, bool regex, bool agpack, bool agsharp, string path) => {
                Console.WriteLine(path);
                Console.WriteLine(url);
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        static async void GetHtml()
        {
            var htmlSource = await WebUtil.GetHtmlSourceAsync();
            Console.WriteLine(htmlSource);
        }
    }
}
