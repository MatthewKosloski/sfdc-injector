using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace SFDCInjector.Parsing
{
    public class Options 
    {
        [Option('a', "authenticate", Separator = ':', Required = true, HelpText = "Authenticate with SFDC.")]
        public IEnumerable<string> AuthArgs { get; set; }

        [Option('i', "inject", Separator = ':', Required = true, HelpText = "Inject event into SFDC.")]
        public IEnumerable<string> InjectArgs { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Authenticate with Salesforce via Username-Password Flow", new Options { 
                        AuthArgs = new List<string> {
                            "CliendId",
                            "ClientSecret",
                            "Username",
                            "Password",
                            "ApiVersion"
                        }
                    })
                };
            }
        }
    }
}