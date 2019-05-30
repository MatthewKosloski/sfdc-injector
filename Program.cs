using System;
using System.Configuration; 
using SFDCInjector.PlatformEvents;
using CommandLine;
using System.Collections;

namespace SFDCInjector
{
    class Program
    {

        private static SFDCClient CreateSFDCClient() 
        {
            var appSettings = ConfigurationManager.AppSettings;
            return new SFDCClient 
            {
                LoginEndpoint = appSettings["LoginEndpoint"],
                ApiEndpoint = appSettings["ApiEndpoint"],
                ClientId = appSettings["ClientId"],
                ClientSecret = appSettings["ClientSecret"],
                Username = appSettings["Username"],
                Password = appSettings["Password"]
            };
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (o.Verbose)
                {
                    Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                    Console.WriteLine("Quick Start Example! App is in Verbose mode!");
                }
                else
                {
                    Console.WriteLine($"Current Arguments: -v {o.Verbose}");
                    Console.WriteLine("Quick Start Example!");
                }
            });
        }
    }

    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}