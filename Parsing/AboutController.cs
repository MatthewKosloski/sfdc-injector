using System;

namespace SFDCInjector.Parsing
{
    public static class AboutController
    {
        public static int Init(AboutOptions opts)
        {
            Console.WriteLine("This is a CLI console application built with" + 
            " .Net core that publishes events to Salesforce.");
            return 0;
        }
    }
}