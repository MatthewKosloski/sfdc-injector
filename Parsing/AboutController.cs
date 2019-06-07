using System;
using SFDCInjector.Parsing;

namespace SFDCInjector.Parsing
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