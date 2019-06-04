using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace SFDCInjector.Parsing
{
    public class Options 
    {
        [Option('a', "authenticate", Min = 5, Separator = ':', 
        Required = true, HelpText = "Authenticate with SFDC.")]
        public IEnumerable<string> AuthArgs { get; set; }

        [Option('i', "inject", Separator = ':', Required = true, HelpText = "Inject event into SFDC.")]
        public IEnumerable<string> InjectArgs { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("To override all settings in App.config, supply all arguments", new Options { 
                        AuthArgs = new List<string> {
                            "CliendId",
                            "ClientSecret",
                            "Username",
                            "Password",
                            "ApiVersion"
                        }
                    }),
                    new Example("To override some App.config settings (e.g., username and password) and keep the rest", new Options { 
                        AuthArgs = new List<string> {"", "", "Username", "Password", ""}
                    }),
                    new Example("To just use the App.config settings", new Options { 
                        AuthArgs = new List<string> {"", "", "", "", ""}
                    })
                };
            }
        }
    }
}