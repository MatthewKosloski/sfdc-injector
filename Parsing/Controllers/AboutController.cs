using System;
using SFDCInjector.Parsing.Options;

namespace SFDCInjector.Parsing.Controllers
{
    public class AboutController
    {
        public int Init(AboutOptions opts)
        {
            Console.WriteLine("This is a CLI console application built with" + 
            " .Net core that publishes events to Salesforce.");
            return 0;
        }
    }
}