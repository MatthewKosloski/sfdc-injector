using System;
using System.Configuration; 

namespace SFDCInjector
{
    class Program
    {

        private static SFDCClient CreateSFDCClient() 
        {
            var appSettings = ConfigurationManager.AppSettings;
            return new SFDCClient 
            {
                LoginEndpoint = appSettings["LoginEndpoint"],
                ApiEndpoint = appSettings["ApiEndpoint"],
                ClientId = appSettings["ClientId"],
                ClientSecret = appSettings["ClientSecret"],
                Username = appSettings["Username"],
                Password = appSettings["Password"]
            };
        }

        public static void Main(string[] args)
        {
            SFDCClient client = CreateSFDCClient();
            client.RequestAccessToken().Wait();
            Console.WriteLine(client.AccessToken);
            Console.WriteLine(client.InstanceUrl);
        }
    }
}