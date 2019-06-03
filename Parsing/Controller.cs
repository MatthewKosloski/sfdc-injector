using System.Configuration; 
using System.Collections.Generic;
using System.Diagnostics;
using System;
using SFDCInjector.Utils;
using SFDCInjector.Exceptions;

namespace SFDCInjector.Parsing
{
    public class Controller
    {
        private static AuthenticationArguments SetAuthArgs(IEnumerable<string> cliArgs)
        {
            AuthenticationArguments args = new AuthenticationArguments();
            
            List<string> cliArgsList = new List<string>();
            foreach(string arg in cliArgs)
                cliArgsList.Add(arg.Trim());

            bool hasCliArgs = cliArgsList.Count != 0;
            bool hasTooManyArgs = cliArgsList.Count > 5;

            if(hasCliArgs)
            {
                if(hasTooManyArgs)
                {
                    throw new Exception("Too many authentication arguments have been provided.");
                }
                else
                {
                    foreach(string arg in cliArgsList)
                        Console.WriteLine($"\"{arg}\"");
                }
            }

            return args;
        }

        public static void Init(Options o)
        {
            AuthenticationArguments authArgs = SetAuthArgs(o.AuthArgs);
        }
    }
}