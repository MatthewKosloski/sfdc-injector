using System.Configuration; 
using System;

namespace SFDCInjector.Parsing
{
    public class Controller
    {

        private static SFDCClient _client;

        public static void Init(Options o)
        {
            foreach(string arg in o.AuthArgs)
            {
                Console.WriteLine($"Auth arg {arg}");
            }

            foreach(string arg in o.InjectArgs)
            {
                Console.WriteLine($"Inject arg {arg}");
            }
        }
    }
}