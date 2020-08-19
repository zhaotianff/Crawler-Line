using crawler_line.Util;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Reflection;

namespace crawler_line
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand();

            //添加一个打印版本的Command
            var versionCommand = new Command("--version");
            versionCommand.AddAlias("-v");
            //添加到RootCommand
            rootCommand.AddCommand(versionCommand);

            //添加一个获取网页源码Option
            var getHtmlSourceOption = new Option<string>(new string[] { "--url" ,"-u"},"website url");

            rootCommand.AddOption(getHtmlSourceOption);
            //getHtmlSourceOption执行的操作
            rootCommand.Handler = CommandHandler.Create<string>((url) =>
            {
                WebUtil.GetHtmlSource(url);
            });

            rootCommand.Description = ".net core command-line crawler";

            //RootCommand执行的操作
            rootCommand.Handler = CommandHandler.Create(() =>
            {
                PrintUsageInfo();
            });

            //打印版本Command执行的操作
            versionCommand.Handler = CommandHandler.Create(PrintVersion);

            // Parse the incoming args and invoke the handler
            rootCommand.InvokeAsync(args).Wait();
        }

        static void PrintVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var versionStr = assembly.GetName().Version.ToString();
            var appName = assembly.GetName().Name;
            Console.WriteLine($"{appName} - {versionStr}");
        }

        static void PrintUsageInfo()
        {
            Console.WriteLine(".net core command-line crawler.");
        }
    }
}
