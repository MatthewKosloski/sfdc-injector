using System.Configuration; 
using System.Diagnostics;
using System.Collections.Generic;
using SFDCInjector.Utils;
using SFDCInjector.Exceptions;
using System;

namespace SFDCInjector.Parsing.Arguments
{
    /// <summary>
    /// Holds the values of the authentication arguments. 
    /// Upon instantiation, it attempts to read authentication
    /// arguments from App.config. If no App.config exists, 
    /// then the properties are Null.
    /// </summary>
    public class AuthenticationArguments
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double ApiVersion { get; set; }
        private static readonly string _ClientIdConfigKey = "ClientId";
        private static readonly string _ClientSecretConfigKey = "ClientSecret";
        private static readonly string _UsernameConfigKey = "Username";
        private static readonly string _PasswordConfigKey = "Password";
        private static readonly string _ApiConfigKey = "ApiVersion";

        public AuthenticationArguments()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                
                bool hasAppConfig = appSettings.Count != 0;

                if(hasAppConfig)
                {
                    bool hasMissingSettings = Helpers.HasNullOrWhiteSpace(new List<string> {
                        appSettings[_ClientIdConfigKey],
                        appSettings[_ClientSecretConfigKey],
                        appSettings[_UsernameConfigKey],
                        appSettings[_PasswordConfigKey],
                        appSettings[_ApiConfigKey]
                    });
                    
                    bool hasValidAppConfig = hasAppConfig && !hasMissingSettings;
                    bool hasInvalidAppConfig = hasAppConfig && hasMissingSettings;

                    if(hasValidAppConfig)
                    {
                        // Set default args to values found in App.config
                        ClientId = appSettings[_ClientIdConfigKey];
                        ClientSecret = appSettings[_ClientSecretConfigKey];
                        Username = appSettings[_UsernameConfigKey];
                        Password = appSettings[_PasswordConfigKey];
                        ApiVersion = Conversions.StringToDouble(appSettings[_ApiConfigKey],
                        "Could not parse ApiVersion. Please check App.config to make sure the value" + 
                        " of ApiVersion conforms to the requirements of a double.");
                    }
                    else if(hasInvalidAppConfig)
                    {
                        throw new InvalidAppConfigException("One or more authentication settings " + 
                        "are missing from App.config.");
                    }
                }
            }
            catch(ConfigurationErrorsException e)
            {
                Console.WriteLine($"{e.GetType()}: Encountered an error while reading App.config.");
                Console.WriteLine(new StackTrace(true).ToString());
            }
            catch(InvalidAppConfigException e)
            {
                Console.WriteLine($"{e.GetType()}: {e.Message}");
                Console.WriteLine(new StackTrace(true).ToString());
            }
        }
    }
}