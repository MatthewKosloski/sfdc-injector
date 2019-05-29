/*

public interface IChild
{
}

public interface IParent<TChild> where TChild : IChild
{
    List<TChild> a { get; set; } 
}

public class ChildA : IChild {  }   

public class ChildB : IChild {  }   

public class ParentA : IParent<ChildA>
{
    public List<ChildA> a { get; set; }
}

public class ParentB : IParent<ChildB>
{
    public List<ChildB> a { get; set; }
}
 */
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

            IPlatformEvent<IPlatformEventFields> evt = (IPlatformEvent<IPlatformEventFields>) new DataCenterStatusEvent<DataCenterStatusEventFields> {
                Fields = new DataCenterStatusEventFields {
                    StatusCode = 500,
                    DataCenterId = "a032E00000xzevTQAQ"
                }
            };

            client.InjectEvent(evt).Wait();
        }
    }
}