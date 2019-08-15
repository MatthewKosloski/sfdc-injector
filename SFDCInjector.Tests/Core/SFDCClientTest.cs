using NUnit.Framework;
using SFDCInjector.Core;
using SFDCInjector.Exceptions;
using SFDCInjector.PlatformEvents.Test;
using System.Net;
using System.Net.Http;
using System;
using RichardSzalay.MockHttp;

namespace SFDCInjector.Tests.Core
{
    /// <summary>
    /// Tests for SFDCInjector's SFDCClient class.
    /// </summary>
    [TestFixture]
    public class SFDCClientTest
    {
        /// <summary>
        /// When requesting for a token, test to make sure that the 
        /// AccessToken property on SFDCClient is set from the response.
        /// </summary>
        [Test]
        public void RequestAccessToken_WhenOKStatusCode_ShouldSetAccessToken()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 45.0;

            string endpoint = "/services/oauth2/token";
            string fakeAccessTokenResponse = "{\"access_token\":\"fake_access_token\"," + 
                "\"instance_url\":\"https://www.fake_instance_url.com\"," + 
                "\"id\":\"fake_id\"," + 
                "\"token_type\":\"fake_token_type\"," + 
                "\"issued_at\":\"fake_issued_at\"," + 
                "\"signature\":\"fake_signature\"}";

            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeAccessTokenResponse);
            
            client.RequestAccessToken().Wait();

            string expected = "fake_access_token";
            string actual = client.AccessToken;

            // ensure client.AccessToken has been set to the expected value
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// When requesting for a token, test to make sure that the 
        /// InstanceUrl property on SFDCClient is set from the response.
        /// </summary>
        [Test]
        public void RequestAccessToken_WhenOKStatusCode_ShouldSetInstanceUrl()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 45.0;

            string endpoint = "/services/oauth2/token";
            string fakeAccessTokenResponse = "{\"access_token\":\"fake_access_token\"," + 
                "\"instance_url\":\"https://www.fake_instance_url.com\"," + 
                "\"id\":\"fake_id\"," + 
                "\"token_type\":\"fake_token_type\"," + 
                "\"issued_at\":\"fake_issued_at\"," + 
                "\"signature\":\"fake_signature\"}";

            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeAccessTokenResponse);
            
            client.RequestAccessToken().Wait();

            string expected = "https://www.fake_instance_url.com";
            string actual = client.InstanceUrl;

            // ensure client.InstanceUrl has been set to the expected value
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// When requesting for a token, test to make sure that an
        /// exception is thrown if the ClientId property is null.
        /// </summary>
        [Test]
        public void RequestAccessToken_NullClientId_ShouldThrowInsufficientAccessTokenRequestException()
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            // should set client.ClientId to avoid exception.
            client.ClientSecret = "client_secret";
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 45.0;

            Assert.That(() => {
                client.RequestAccessToken().Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InsufficientAccessTokenRequestException>());
        }

        /// <summary>
        /// When requesting for a token, test to make sure that an
        /// exception is thrown if the ClientSecret property is null.
        /// </summary>
        [Test]
        public void RequestAccessToken_NullClientSecret_ShouldThrowInsufficientAccessTokenRequestException()
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            client.ClientId = "cliend_id";
            // should set client.ClientSecret to avoid exception.
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 45.0;

            Assert.That(() => {
                client.RequestAccessToken().Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InsufficientAccessTokenRequestException>());
        }

        /// <summary>
        /// When requesting for a token, test to make sure that an
        /// exception is thrown if the Username property is null.
        /// </summary>
        [Test]
        public void RequestAccessToken_NullUsername_ShouldThrowInsufficientAccessTokenRequestException()
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            // should set client.Username to avoid exception.
            client.Password = "password";
            client.ApiVersion = 45.0;

            Assert.That(() => {
                client.RequestAccessToken().Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InsufficientAccessTokenRequestException>());
        }

        /// <summary>
        /// When requesting for a token, test to make sure that an
        /// exception is thrown if the Password property is null.
        /// </summary>
        [Test]
        public void RequestAccessToken_NullPassword_ShouldThrowInsufficientAccessTokenRequestException()
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "username";
            // should set client.Password to avoid exception
            client.ApiVersion = 45.0;

            Assert.That(() => {
                client.RequestAccessToken().Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InsufficientAccessTokenRequestException>());
        }

        /// <summary>
        /// Tests to make sure an exception is thrown if there is an attempt 
        /// to inject an event without first attempting to acquire an access token.
        /// </summary>
        [Test]
        public void InjectEvent_NoPriorCallToRequestAccessToken_ShouldThrowInsufficientEventInjectionException()
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            client.ApiVersion = 43.0;

            TestEvent evt = new TestEvent {
                ApiName = "Test_Event_Api_Name__e",
                Fields = new TestEventFields {
                    StringTestField = "TestFieldValue"
                }
            };

            Assert.That(() => {
                /// should call client.RequestAccessToken().Wait() to avoid exception.
                client.InjectEvent(evt).Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InsufficientEventInjectionException>());

            // props are null because they are set by RequestAccessToken
            Assert.That(client.AccessToken, Is.Null);
            Assert.That(client.InstanceUrl, Is.Null);
        }

        /// <summary>
        /// Tests to make sure an exception is thrown if 
        /// ApiVersion is not set.
        /// </summary>
        [Test]
        public void InjectEvent_NullApiVersion_ShouldThrowInsufficientEventInjectionException()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "client_username";
            client.Password = "password";
            // should set client.ApiVersion to avoid exception

            string endpoint = "/services/oauth2/token";
            TestEvent evt = new TestEvent {
                ApiName = "Test_Event_Api_Name__e",
                Fields = new TestEventFields {
                    StringTestField = "TestFieldValue"
                }
            };

            // send empty response (not relevant to test)
            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", "{}");

            Assert.That(() => {
                client.RequestAccessToken().Wait();
                client.InjectEvent(evt).Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InsufficientEventInjectionException>());
        }

        /// <summary>
        /// Tests that an exception is thrown if the event 
        /// supplied to InjectEvent has no ApiName.
        /// </summary>
        [Test]
        public void InjectEvent_NullEventApiName_ShouldThrowInvalidPlatformEventException()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 10;

            string endpoint = "/services/oauth2/token";
            string fakeAccessTokenResponse = "{\"access_token\":\"fake_access_token\"," + 
                "\"instance_url\":\"https://www.fake_instance_url.com\"," + 
                "\"id\":\"fake_id\"," + 
                "\"token_type\":\"fake_token_type\"," + 
                "\"issued_at\":\"fake_issued_at\"," + 
                "\"signature\":\"fake_signature\"}";
            TestEvent evt = new TestEvent {
                // should set ApiName to avoid exception
                Fields = new TestEventFields {
                    StringTestField = "TestFieldValue"
                }
            };

            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeAccessTokenResponse);

            Assert.That(() => {
                client.RequestAccessToken().Wait();
                client.InjectEvent(evt).Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InvalidPlatformEventException>());
        }

        /// <summary>
        /// Tests that the ApiEndpoint is set when the ApiVersion is set.
        /// </summary>
        [Test]
        public void ApiEndpoint_ApiVersionIsSet_ShouldBeSet()
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            
            client.ApiVersion = 42.3;
            Assert.That("/services/data/v42.3/", Is.EqualTo(client.ApiEndpoint));

            client.ApiVersion = 15.0;
            Assert.That("/services/data/v15.0/", Is.EqualTo(client.ApiEndpoint));
        }

        /// <summary>
        /// Tests that an exception is thrown if the event 
        /// supplied to InjectEvent has no Fields.
        /// </summary>
        [Test]
        public void InjectEvent_NullEventFields_ShouldThrowInvalidPlatformEventException()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 10;

            string endpoint = "/services/oauth2/token";
            string fakeAccessTokenResponse = "{\"access_token\":\"fake_access_token\"," + 
                "\"instance_url\":\"https://www.fake_instance_url.com\"," + 
                "\"id\":\"fake_id\"," + 
                "\"token_type\":\"fake_token_type\"," + 
                "\"issued_at\":\"fake_issued_at\"," + 
                "\"signature\":\"fake_signature\"}";
            TestEvent evt = new TestEvent {
                ApiName = "Test_Event_Api_Name__e"
                // should set Fields to avoid exception
            };

            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeAccessTokenResponse);

            Assert.That(() => {
                client.RequestAccessToken().Wait();
                client.InjectEvent(evt).Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<InvalidPlatformEventException>());
        }

        /// <summary>
        /// Tests that an exception is thrown if Salesforce
        /// returns a response indicating that the event
        /// was not injected.
        /// </summary>
        [Test]
        public void InjectEvent_UnsuccessfulInjection_ShouldThrowEventInjectionUnsuccessfulException()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));
            client.ClientId = "cliend_id";
            client.ClientSecret = "client_secret";
            client.Username = "username";
            client.Password = "password";
            client.ApiVersion = 10;

            string accessTokenEndpoint = "/services/oauth2/token";
            string eventInjectionEndpoint = "/services/data/v10.0/sobjects/Test_Event_Api_Name__e";
            string fakeAccessTokenResponse = "{\"access_token\":\"fake_access_token\"," + 
                "\"instance_url\":\"https://www.fake_instance_url.com\"," + 
                "\"id\":\"fake_id\"," + 
                "\"token_type\":\"fake_token_type\"," + 
                "\"issued_at\":\"fake_issued_at\"," + 
                "\"signature\":\"fake_signature\"}";
            string fakeEventInjectionResponse = "{\"id\":\"fake_id\"," + 
                "\"success\":false," +  // indicates an unsuccessful injection
                "\"errors\":[]}";
            TestEvent evt = new TestEvent {
                ApiName = "Test_Event_Api_Name__e",
                Fields = new TestEventFields {
                    StringTestField = "TestFieldValue"
                }
            };

            mockHandler
                .When(accessTokenEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeAccessTokenResponse);
            
            mockHandler
                .When(eventInjectionEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeEventInjectionResponse);

            Assert.That(() => {
                client.RequestAccessToken().Wait();
                client.InjectEvent(evt).Wait();
            }, Throws
                .TypeOf<AggregateException>()
                .With.InnerException
                .TypeOf<EventInjectionUnsuccessfulException>());
        }

    }
}