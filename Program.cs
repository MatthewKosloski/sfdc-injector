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

            DataCenterStatusEvent evt1 = new DataCenterStatusEvent {
                Fields = new DataCenterStatusEventFields {
                    StatusCode = 500,
                    DataCenterId = "a032E00000xzevTQAQ"
                }
            };

            client.InjectEvent(evt1).Wait();
        }
    }
}