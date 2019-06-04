using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace SFDCInjector.Parsing
{
    [Verb("inject", HelpText = "Inject an event into SFDC (Salesforce).")] 
    public class InjectOptions 
    {
        [Option('a', "authenticate", Min = 5, Max = 5, Separator = ':', 
        Required = true, HelpText = "Authenticate with SFDC Username-Password Flow.")]
        public IEnumerable<string> AuthArgs { get; set; }

        [Option('e', "event", Min = 2, Separator = ':', Required = true, 
        HelpText = "The event to inject into Salesforce.")]
        public IEnumerable<string> EventArgs { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("To override all settings in App.config, supply all arguments", new InjectOptions { 
                        AuthArgs = new List<string> {
                            "CliendId",
                            "ClientSecret",
                            "Username",
                            "Password",
                            "ApiVersion"
                        }
                    }),
                    new Example("To override some App.config settings (e.g., username and password) and keep the rest", new InjectOptions { 
                        AuthArgs = new List<string> {"", "", "Username", "Password", ""}
                    }),
                    new Example("To just use the App.config settings", new InjectOptions { 
                        AuthArgs = new List<string> {"", "", "", "", ""}
                    })
                };
            }
        }
    }
}