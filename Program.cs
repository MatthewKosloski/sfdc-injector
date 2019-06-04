using CommandLine;
using SFDCInjector.Parsing;
// using System.Configuration;
// using SFDCInjector.PlatformEvents.DataCenter;

namespace SFDCInjector
{
    class Program
    {

        // private static SFDCClient CreateSFDCClient() 
        // {
        //     var appSettings = ConfigurationManager.AppSettings;
        //     return new SFDCClient 
        //     {
        //         ClientId = appSettings["ClientId"],
        //         ClientSecret = appSettings["ClientSecret"],
        //         Username = appSettings["Username"],
        //         Password = appSettings["Password"],
        //         ApiVersion = SFDCInjector.Utils.Conversions.StringToDouble(appSettings["ApiVersion"])
        //     };
        // }

        // public static void Main(string[] args)
        // {
        //     SFDCClient client = CreateSFDCClient();
        //     client.RequestAccessToken().Wait();

        //     DataCenterStatusEvent statusChangeEvent = new DataCenterStatusEvent {
        //         Fields = new DataCenterStatusEventFields {
        //             StatusCode = 404,
        //             DataCenterId = "a032E00000xzevTQAQ"
        //         }
        //     };

        //     client.InjectEvent(statusChangeEvent).Wait();
        // }


        static int Main(string[] args) {
            return Parser.Default.ParseArguments<InjectOptions, AboutOptions>(args).MapResult(
                (InjectOptions opts) => InjectController.Init(opts),
                (AboutOptions opts) => AboutController.Init(opts),
                errs => 1);
        }
    }
}