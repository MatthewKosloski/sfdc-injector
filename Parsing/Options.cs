using CommandLine;
using System.Collections.Generic;

namespace SFDCInjector.Parsing
{
    public class Options 
    {
        [Option('a', "authenticate", Separator=':')]
        public IEnumerable<string> AuthArgs { get; set; }

        [Option('i', "inject", Separator=':')]
        public IEnumerable<string> InjectArgs { get; set; }
    }
}