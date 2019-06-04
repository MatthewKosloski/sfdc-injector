using System.Configuration; 
using System.Collections.Generic;
using System.Diagnostics;
using System;
using SFDCInjector.Utils;
using SFDCInjector.Exceptions;

namespace SFDCInjector.Parsing
{
    public static class InjectController
    {
        private static AuthenticationArguments SetAuthArgs(IEnumerable<string> cliArgs)
        {
            AuthenticationArguments authArgs = new AuthenticationArguments();
            
            List<string> cliArgsList = new List<string>();
            foreach(string arg in cliArgs)
                cliArgsList.Add(arg.Trim());

            bool hasNoAppConfig = ConfigurationManager.AppSettings.Count == 0;
            bool hasMissingCliArgs = Helpers.HasEmptyTrimmedString(cliArgsList);

            if(hasNoAppConfig && hasMissingCliArgs)
            {
                throw new MissingAppConfigException("There is no App.config and not all authentication " + 
                "arguments have been provided.  Please provide all authentication arguments.");
            } 
            else
            {
                authArgs.ClientId = Helpers.KeepOriginalIfEmptyReplacement(authArgs.ClientId, cliArgsList[0]);
                authArgs.ClientSecret = Helpers.KeepOriginalIfEmptyReplacement(authArgs.ClientSecret, cliArgsList[1]);
                authArgs.Username = Helpers.KeepOriginalIfEmptyReplacement(authArgs.Username, cliArgsList[2]);
                authArgs.Password = Helpers.KeepOriginalIfEmptyReplacement(authArgs.Password, cliArgsList[3]);
                authArgs.ApiVersion = Helpers.KeepOriginalIfEmptyReplacement(authArgs.ApiVersion, cliArgsList[4]);
            }

            return authArgs;
        }

        public static int Init(InjectOptions o)
        {
            AuthenticationArguments authArgs = SetAuthArgs(o.AuthArgs);

            Console.WriteLine($"ClientId: {authArgs.ClientId}");
            Console.WriteLine($"ClientSecret: {authArgs.ClientSecret}");
            Console.WriteLine($"Username: {authArgs.Username}");
            Console.WriteLine($"Password: {authArgs.Password}");
            Console.WriteLine($"ApiVersion: {authArgs.ApiVersion}");

            return 0;
        }
    }
}