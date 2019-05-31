using System;
using System.Configuration; 
using SFDCInjector.PlatformEvents.DataCenter;
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
            // Parser.Default.ParseArguments<Options>(args)
            // .WithParsed<Options>(o =>
            // {
            //     if (o.Verbose)
            //     {
            //         Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
            //         Console.WriteLine("Quick Start Example! App is in Verbose mode!");
            //     }
            //     if (!String.IsNullOrEmpty(o.ConfigFile))
            //     {
            //         Console.WriteLine($"Set config file to {o.ConfigFile}");
            //     }
            //     else
            //     {
            //         Console.WriteLine($"Current Arguments: -v {o.Verbose}");
            //         Console.WriteLine("Quick Start Example!");
            //     }
            // });

            SFDCClient client = CreateSFDCClient();
            client.RequestAccessToken().Wait();

            DataCenterStatusEvent statusChangeEvent = new DataCenterStatusEvent {
                Fields = new DataCenterStatusEventFields {
                    StatusCode = 201,
                    DataCenterId = "a032E00000xzevTQAQ"
                }
            };

            DataCenterNameEvent nameChangeEvent = new DataCenterNameEvent {
                Fields = new DataCenterNameEventFields {
                    Name = "Applied-Denver",
                    DataCenterId = "a032E00000xzevTQAQ"
                }
            };

            client.InjectEvent(nameChangeEvent).Wait();
            client.InjectEvent(statusChangeEvent).Wait();

        }
    }

    // public class Options
    // {
    //     [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    //     public bool Verbose { get; set; }

    //     [Option('c', "config", Required = false, HelpText = "Sets the configuration file.")]
    //     public string ConfigFile { get; set; }

    // }
}