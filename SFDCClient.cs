using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Runtime.Serialization.Json;

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

        private FormUrlEncodedContent CreateHttpContent()
        {
            return new FormUrlEncodedContent(
                new Dictionary<string, string> {
                    {"grant_type", "password"},
                    {"client_id", this.ClientId},
                    {"client_secret", this.ClientSecret},
                    {"username", this.Username},
                    {"password", this.Password}
                }
            );
        }

        public static AccessTokenResponseBody CreateAccessTokenResponseBody(string json)
        {
            AccessTokenResponseBody body = new AccessTokenResponseBody();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(body.GetType());
            body = serializer.ReadObject(stream) as AccessTokenResponseBody;
            stream.Close();
            return body;
        }

        public async Task RequestAccessToken()
        {

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage res = await client.PostAsync(this.LoginEndpoint, CreateHttpContent());
                    string resString = await res.Content.ReadAsStringAsync();
                    AccessTokenResponseBody resObj = CreateAccessTokenResponseBody(resString);
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
        }

    }
}