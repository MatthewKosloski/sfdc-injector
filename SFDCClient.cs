using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Runtime.Serialization.Json;
using Flurl;

namespace SFDCInjector {
    public class SFDCClient {
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

        // Deserializes a JSON string into an instance of T.
        private static T DeserializeJsonToType<T>(string json) where T : class, new()
        {
            T obj = new T();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            obj = serializer.ReadObject(stream) as T;
            stream.Close();
            return obj;
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
                AccessTokenResponseBody resObj = DeserializeJsonToType<AccessTokenResponseBody>(resString);
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

        public async Task InjectEvent()
        {
            try
            {
                Url reqUri = Url.Combine(this.InstanceUrl, this.ApiEndpoint, "sobjects", "Data_Center_Status__e");
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, new Uri(reqUri));
                req.Headers.Add("Authorization", $"Bearer {this.AccessToken}");
                req.Headers.Add("Accept", "application/json");
                string json = "{\"Status_Code__c\":\"995\",\"Data_Center_Id__c\":\"a032E00000xzevTQAQ\"}";
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await _client.SendAsync(req);
                string resString = await res.Content.ReadAsStringAsync();
                InjectEventResponseBody resObj = DeserializeJsonToType<InjectEventResponseBody>(resString);
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