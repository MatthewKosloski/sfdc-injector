using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Flurl;

namespace SFDCInjector 
{
    public class SFDCClient 
    {
        public string LoginEndpoint { get; set; }

        public string ApiEndpoint { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AccessToken { get; set; }

        public string InstanceUrl { get; set; }

        private static readonly HttpClient _client;

        static SFDCClient()
        {
            _client = new HttpClient();
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
                HttpResponseMessage res = await _client.PostAsync(this.LoginEndpoint, httpContent);
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

                string resString = await res.Content.ReadAsStringAsync();
                InjectEventResponseBody resObj = SerializerDeserializer.DeserializeJsonToType<InjectEventResponseBody>(resString);
                Console.WriteLine($"{resObj.Id} {resObj.Success}");
                res.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException thrown!");
                Console.WriteLine(e.Message);
            }
        }

    }
}