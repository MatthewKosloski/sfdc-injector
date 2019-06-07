using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Diagnostics;
using Flurl;
using SFDCInjector.Exceptions;
using SFDCInjector.PlatformEvents;
using SFDCInjector.ResponseBodies;
using SFDCInjector.Utils;

namespace SFDCInjector 
{

    /// <summary>
    /// Handles communication with Salesforce's REST API.
    /// </summary>
    public class SFDCClient 
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AccessToken { get; set; }

        public string InstanceUrl { get; set; }

        public string ApiEndpoint { get => _ApiEndpoint; }

        public double ApiVersion {
            get => _ApiVersion;
            set {
                _ApiVersion = value;
                _ApiEndpoint = $"/services/data/v{value.ToString("0.0")}/";
            }
        }

        private static readonly string _LoginEndpoint;

        private static readonly HttpClient _Client;

        private string _ApiEndpoint;

        private double _ApiVersion;

        static SFDCClient()
        {
            _Client = new HttpClient();
            _LoginEndpoint = "https://login.salesforce.com/services/oauth2/token";
        }

        /// <summary>
        /// Uses a username and password to request an access token and instance url
        /// by making a POST request to the Salesforce token request endpoint. 
        /// <seealso cref="SFDCInjector.InjectEvent()"/>
        /// </summary>
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
                HttpResponseMessage res = await _Client.PostAsync(_LoginEndpoint, httpContent);
                string resString = await res.Content.ReadAsStringAsync();
                AccessTokenResponseBody resObj = SerializerDeserializer
                .DeserializeJsonToType<AccessTokenResponseBody>(resString);
                this.AccessToken = resObj.AccessToken;
                this.InstanceUrl = resObj.InstanceUrl;
                res.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine($"{e.GetType()}: {e.Message}");
                Console.WriteLine(new StackTrace(true).ToString());
            }
        }

        /// <summary>
        /// Injects an event into Salesforce at the 
        /// `/services/data/vXX.0/sobjects/Event_Api_Name__e/` endpoint, 
        /// where `XX.0` is `ApiVersion` and `Event_Api_Name__e` is the event's
        /// `ApiName` property.
        /// <exception cref="SFDCInjector.Exceptions.EventInjectionUnsuccessfulException">
        /// Thrown when the Success property on the response body is false.
        /// </exception>
        /// <param name="evt">The event to be injected.</param>
        /// <typeparam name="TEventFields">The data type of the event's fields.</typeparam>
        /// </summary>
        public async Task InjectEvent<TEventFields>(IPlatformEvent<TEventFields> evt) 
        where TEventFields : IPlatformEventFields
        {
            try
            {
                Url reqUri = Url.Combine(this.InstanceUrl, this.ApiEndpoint, "sobjects", evt.ApiName);
                string json = SerializerDeserializer.SerializeTypeToJson<TEventFields>(evt.Fields);
                
                HttpResponseMessage res = await _Client.SendAsync(new HttpRequestMessage {
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
                InjectEventResponseBody resObj = SerializerDeserializer
                .DeserializeJsonToType<InjectEventResponseBody>(resString);

                bool eventInjectionDidFail = !resObj.Success;
                if(eventInjectionDidFail)
                {
                    throw new EventInjectionUnsuccessfulException(
                        $"Failed to inject event {resObj.Id} into Salesforce.");
                } 
                else 
                {
                    Console.WriteLine($"Successfully injected a(n) {evt.ApiName} event.");
                    Console.WriteLine($"Returned Id From Salesforce: {resObj.Id}");
                }
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine($"{e.GetType()}: {e.Message}");
                Console.WriteLine(new StackTrace(true).ToString());
            }
            catch(EventInjectionUnsuccessfulException e)
            {
                Console.WriteLine($"{e.GetType()}: {e.Message}");
                Console.WriteLine(new StackTrace(true).ToString());
            }
        }

    }
}