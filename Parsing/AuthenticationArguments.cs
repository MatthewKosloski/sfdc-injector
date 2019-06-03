using System.Configuration; 
using System.Diagnostics;
using SFDCInjector.Utils;
using SFDCInjector.Exceptions;
using System;

namespace SFDCInjector.Parsing
{
    public class AuthenticationArguments
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double ApiVersion { get; set; }
        public bool HasAppConfig { get; }
        public bool HasMissingSettings { get; }
        public bool HasValidAppConfig { get => HasAppConfig && !HasMissingSettings; }
        public bool HasInvalidAppConfig { get => HasAppConfig && HasMissingSettings; }
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
                
                HasAppConfig = appSettings.Count != 0;

                if(HasAppConfig)
                {
                    HasMissingSettings = 
                        Helpers.IsTrimmedStringEmpty(appSettings[_ClientIdConfigKey]) ||
                        Helpers.IsTrimmedStringEmpty(appSettings[_ClientSecretConfigKey]) ||
                        Helpers.IsTrimmedStringEmpty(appSettings[_UsernameConfigKey]) ||
                        Helpers.IsTrimmedStringEmpty(appSettings[_PasswordConfigKey]) ||
                        Helpers.IsTrimmedStringEmpty(appSettings[_ApiConfigKey]);

                    if(HasValidAppConfig)
                    {
                        // Set default args to values found in App.config
                        ClientId = appSettings[_ClientIdConfigKey];
                        ClientSecret = appSettings[_ClientSecretConfigKey];
                        Username = appSettings[_UsernameConfigKey];
                        Password = appSettings[_PasswordConfigKey];
                        ApiVersion = Convertions.StringToDouble(appSettings[_ApiConfigKey],
                        "Could not parse ApiVersion. Please check App.config to make sure the value" + 
                        " of ApiVersion conforms to the requirements of a double.");
                    }
                    else if(HasInvalidAppConfig)
                    {
                        throw new InvalidAppConfigException("One or more authentication settings " + 
                        "are missing from App.config.");
                    }
                }
            }
            catch(ConfigurationErrorsException)
            {
                Console.WriteLine("Encountered an error while reading App.config.");
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