using CommandLine;
using SFDCInjector.Parsing;

namespace SFDCInjector
{
    class Program
    {
        public static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o => Controller.Init(o));
        }
    }
}