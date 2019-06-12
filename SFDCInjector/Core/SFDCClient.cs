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

namespace SFDCInjector.Core
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

        public string AccessToken { get => _AccessToken; }

        public string InstanceUrl { get => _InstanceUrl; }

        public string ApiEndpoint { get => _ApiEndpoint; }

        public double ApiVersion {
            get => _ApiVersion;
            set {
                _ApiVersion = value;
                _ApiEndpoint = $"/services/data/v{value.ToString("0.0")}/";
            }
        }

        private static readonly string _LoginEndpoint;

        private static readonly string _GrantType;

        private readonly HttpClient _Client;

        private string _ApiEndpoint;

        private double _ApiVersion;

        private string _AccessToken;

        private string _InstanceUrl;

        static SFDCClient()
        {
            _LoginEndpoint = "https://login.salesforce.com/services/oauth2/token";
            _GrantType = "password";
        }

        public SFDCClient(HttpClient client)
        {
            _Client = client;
        }

        /// <summary>
        /// The query parameters used in the request for an access token.
        /// </summary>
        private Dictionary<string, string> GetAccessTokenQueryParams()
        {
            return new Dictionary<string, string> {
                {"grant_type", _GrantType},
                {"client_id", this.ClientId},
                {"client_secret", this.ClientSecret},
                {"username", this.Username},
                {"password", this.Password}
            };
        }

        /// <summary>
        /// Returns a boolean indicating if `_LoginEndpoint`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoLoginEndpoint()
        {
            return String.IsNullOrWhiteSpace(_LoginEndpoint);
        }

        /// <summary>
        /// Returns a boolean indicating if `_GrantType`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoGrantType()
        {
            return String.IsNullOrWhiteSpace(_GrantType);
        }

        /// <summary>
        /// Returns a boolean indicating if `CliendId`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoCliendId()
        {
            return String.IsNullOrWhiteSpace(this.ClientId);
        }

        /// <summary>
        /// Returns a boolean indicating if `ClientSecret`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoClientSecret()
        {
            return String.IsNullOrWhiteSpace(this.ClientSecret);
        }

        /// <summary>
        /// Returns a boolean indicating if `Username`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoUsername()
        {
            return String.IsNullOrWhiteSpace(this.Username);
        }

        /// <summary>
        /// Returns a boolean indicating if `Password`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoPassword()
        {
            return String.IsNullOrWhiteSpace(this.Password);
        }

        /// <summary>
        /// Returns a boolean indicating if `ApiVersion`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoApiVersion()
        {
            return this.ApiVersion == 0;
        }

        /// <summary>
        /// Returns a boolean indicating if `_ApiEndpoint`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoApiEndpoint()
        {
            return String.IsNullOrWhiteSpace(_ApiEndpoint);
        }

        /// <summary>
        /// Returns a boolean indicating if `InstanceUrl`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoInstanceUrl()
        {
            return String.IsNullOrWhiteSpace(this.InstanceUrl);
        }

        /// <summary>
        /// Returns a boolean indicating if `AccessToken`
        /// is null, empty, or whitespace.
        /// </summary>
        private bool HasNoAccessToken()
        {
            return String.IsNullOrWhiteSpace(this.AccessToken);
        }

        /// <summary>
        /// Returns a boolean indicating if there is not enough
        /// information to make a request for an access token 
        /// (e.g. missing query parameter or invalid uri).
        /// </summary>
        private bool IsInsufficientAccessTokenRequest()
        {
            return HasNoLoginEndpoint() || HasNoGrantType() || HasNoCliendId() || 
            HasNoClientSecret() || HasNoUsername() || HasNoPassword();
        }

        /// <summary>
        /// Returns a boolean indicating if there is not enough
        /// information to inject an event into Salesforce.
        /// (e.g. missing access token).
        /// </summary>
        private bool IsInsufficientEventInjection()
        {
            return HasNoInstanceUrl() || HasNoApiVersion() 
            || HasNoApiEndpoint() || HasNoAccessToken();
        }

        /// <summary>
        /// Produces a string of the query parameters used in the
        /// request for an access token.
        /// </summary>
        private string GetAccessTokenQueryParamsString()
        {
            string str = "";
            foreach(KeyValuePair<string, string> kvp in GetAccessTokenQueryParams())
                str += $"{kvp.Key}: {kvp.Value}\n";
            return str.Trim();
        }

        /// <summary>
        /// Uses a username and password to request an access token and instance url
        /// by making a POST request to the Salesforce token request endpoint. 
        /// <seealso cref="SFDCInjector.InjectEvent()"/>
        /// <exception cref="SFDCInjector.Exceptions.InsufficientAccessTokenRequestException"></exception>
        /// </summary>
        public async Task RequestAccessToken()
        {
            if(IsInsufficientAccessTokenRequest())
            {
                throw new InsufficientAccessTokenRequestException("Unable to request an access " + 
                "token. Make sure the query parameters and the Uri are valid and correct.");
            }

            try
            {
                FormUrlEncodedContent httpContent = new FormUrlEncodedContent(GetAccessTokenQueryParams());
                HttpResponseMessage res = await _Client.PostAsync(_LoginEndpoint, httpContent);
                string resString = await res.Content.ReadAsStringAsync();
                AccessTokenResponseBody resObj = SerializerDeserializer
                .DeserializeJsonToType<AccessTokenResponseBody>(resString);

                this._AccessToken = resObj.AccessToken;
                this._InstanceUrl = resObj.InstanceUrl;

                res.EnsureSuccessStatusCode();
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine("The Url is null.");
                Console.WriteLine($"{e.GetType()}: {e.Message}");
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("Failed to get an access token from the Salesforce request endpoint.");
                Console.WriteLine("Provided query parameters:");
                Console.WriteLine(GetAccessTokenQueryParamsString());
                Console.WriteLine("Request endpoint:");
                Console.WriteLine(_LoginEndpoint);
                Console.WriteLine($"{e.GetType()}: {e.Message}");
                //Console.WriteLine(new StackTrace(true).ToString());
            }
        }

        /// <summary>
        /// Injects an event into Salesforce at the 
        /// `/services/data/vXX.0/sobjects/Event_Api_Name__e/` endpoint, 
        /// where `XX.0` is `ApiVersion` and `Event_Api_Name__e` is the event's
        /// `ApiName` property.
        /// <exception cref="SFDCInjector.Exceptions.EventInjectionUnsuccessfulException"></exception>
        /// <exception cref="SFDCInjector.Exceptions.InsufficientEventInjectionException"></exception>
        /// <exception cref="SFDCInjector.Exceptions.InvalidPlatformEventException"></exception>
        /// <param name="evt">The event to be injected.</param>
        /// <typeparam name="TEventFields">The data type of the event's fields.</typeparam>
        /// </summary>
        public async Task InjectEvent<TEventFields>(IPlatformEvent<TEventFields> evt) 
        where TEventFields : IPlatformEventFields
        {

            bool eventHasNoApiName = String.IsNullOrWhiteSpace(evt.ApiName);
            bool eventHasNoFields = evt.Fields == null;
            bool isInvalidPlatformEvent = eventHasNoApiName || eventHasNoFields;

            if(IsInsufficientEventInjection())
            {
                throw new InsufficientEventInjectionException("Unable to inject an event into Salesforce. " + 
                "Make sure to first call SFDCClient.RequestAccessToken to retrieve an access token and " + 
                "instance url. Also make sure that an ApiVersion has been specified.");
            }

            if(isInvalidPlatformEvent)
            {
                throw new InvalidPlatformEventException("Unable to inject an event into Salesforce. " + 
                "The supplied platform event is incomplete.  Make sure the platform event " + 
                "has fields and an api name.");
            }

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