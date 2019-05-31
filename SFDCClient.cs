using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Flurl;
using SFDCInjector.Exceptions;
using SFDCInjector.PlatformEvents;
using SFDCInjector.ResponseBodies;
using SFDCInjector.Utils;

namespace SFDCInjector 
{
    public class SFDCClient 
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AccessToken { get; set; }

        public string InstanceUrl { get; set; }

        public string ApiEndpoint {
            get => _apiEndpoint;
        }

        public double ApiVersion {
            get => _apiVersion;
            set {
                _apiVersion = value;
                _apiEndpoint = $"/services/data/v{value.ToString("0.0")}/";
            }
        }

        public static readonly string loginEndpoint;

        private static readonly HttpClient _client;

        private string _apiEndpoint;

        private double _apiVersion;

        static SFDCClient()
        {
            _client = new HttpClient();
            loginEndpoint = "https://login.salesforce.com/services/oauth2/token";
        }

        public async Task RequestAccessToken()
        {
            try
            {
                FormUrlEncodedContent httpContent = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        {"grant_type", "password"},
                        {"client_id", this.ClientId},
                        {"client_secret", this.ClientSecret},
                        {"username", this.Username},
                        {"password", this.Password}
                    }
                );
                HttpResponseMessage res = await _client.PostAsync(loginEndpoint, httpContent);
                string resString = await res.Content.ReadAsStringAsync();
                AccessTokenResponseBody resObj = SerializerDeserializer.DeserializeJsonToType<AccessTokenResponseBody>(resString);
                this.AccessToken = resObj.AccessToken;
                this.InstanceUrl = resObj.InstanceUrl;
                res.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException thrown!");
                Console.WriteLine(e.Message);
            }
        }

        public async Task InjectEvent<TEventFields>(IPlatformEvent<TEventFields> evt) where TEventFields : IPlatformEventFields
        {
            try
            {
                Url reqUri = Url.Combine(this.InstanceUrl, this.ApiEndpoint, "sobjects", evt.API_NAME);
                string json = SerializerDeserializer.SerializeTypeToJson<TEventFields>(evt.Fields);
                
                HttpResponseMessage res = await _client.SendAsync(new HttpRequestMessage {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(reqUri),
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    Headers = {
                        {"Authorization", $"Bearer {this.AccessToken}"},
                        {"Accept", "application/json"}
                    }
                });

                res.EnsureSuccessStatusCode();

                string resString = await res.Content.ReadAsStringAsync();
                InjectEventResponseBody resObj = SerializerDeserializer.DeserializeJsonToType<InjectEventResponseBody>(resString);

                bool eventInjectionDidFail = !resObj.Success;
                if(eventInjectionDidFail)
                {
                    throw new EventInjectionUnsuccessfulException(
                        $"Failed to inject event {resObj.Id} into Salesforce.");
                } 
                else 
                {
                    Console.WriteLine($"Successfully injected a(n) {evt.API_NAME} event.");
                    Console.WriteLine($"Returned Id From Salesforce: {resObj.Id}");
                }
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(EventInjectionUnsuccessfulException e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}