using NUnit.Framework;
using SFDCInjector.Core;
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
        /// When requesting for a token, tests to make sure that the 
        /// AccessToken property on SFDCClient is set from the response.
        /// </summary>
        [Test]
        public void RequestAccessToken_WhenCalled_ShouldSetAccessToken()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));

            string endpoint = "/services/oauth2/token";
            string fakeJson = "{\"access_token\":\"fake_access_token\"}";

            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeJson);
            
            client.RequestAccessToken().Wait();

            string expected = "fake_access_token";
            string actual = client.AccessToken;

            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// When requesting for a token, tests to make sure that the 
        /// InstanceUrl property on SFDCClient is set from the response.
        /// </summary>
        [Test]
        public void RequestAccessToken_WhenCalled_ShouldSetInstanceUrl()
        {
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            SFDCClient client = new SFDCClient(new HttpClient(mockHandler));

            string endpoint = "/services/oauth2/token";
            string fakeJson = "{\"instance_url\":\"fake_instance_url\"}";

            mockHandler
                .When(endpoint)
                .Respond(HttpStatusCode.OK, "application/json", fakeJson);
            
            client.RequestAccessToken().Wait();

            string expected = "fake_instance_url";
            string actual = client.InstanceUrl;

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}