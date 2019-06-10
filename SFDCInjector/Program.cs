using CommandLine;
using SFDCInjector.Parsing.Controllers;
using SFDCInjector.Parsing.Options;

namespace SFDCInjector
{

    class Program
    {
        static int Main(string[] args) {
            InjectController injectController = new InjectController();
            AboutController aboutController = new AboutController();

            return Parser.Default.ParseArguments<InjectOptions, AboutOptions>(args).MapResult(
                (InjectOptions opts) => injectController.Init(opts),
                (AboutOptions opts) => aboutController.Init(opts),
                errs => 1);
        }

    }
}