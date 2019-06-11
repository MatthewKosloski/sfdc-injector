using System.Configuration; 
using System.Collections.Generic;
using System.Net.Http;
using System;
using SFDCInjector.Utils;
using SFDCInjector.Exceptions;
using SFDCInjector.Parsing.Arguments;
using SFDCInjector.Parsing.Options;
using SFDCInjector.Core;

namespace SFDCInjector.Parsing.Controllers
{
    /// <summary>
    /// Responsible for carrying out initialization actions 
    /// using the CLI argument values supplied to the `inject` verb.
    /// </summary>
    public class InjectController
    {

        private AuthenticationArguments _AuthArgs { get; set; }
        private EventArguments _EventArgs { get; set; }
        private SFDCClient _Client { get; set; }

        /// <summary>
        /// Creates a new instance of `AuthenticationArguments`
        /// using the supplied authentication arguments and the 
        /// contents of `App.config`.
        /// <param name="cliAuthArgs">
        /// An enumerable containing the values supplied to the -a CLI flag.
        /// </param>
        /// <exception cref="SFDCInjector.Exceptions.EventInjectionUnsuccessfulException">
        /// Thrown when App.config is not found and there are missing CLI arguments.
        /// </exception>
        /// </summary>
        private AuthenticationArguments CreateAuthArgs(IEnumerable<string> cliAuthArgs)
        {
            AuthenticationArguments authArgs = new AuthenticationArguments();
            
            List<string> cliAuthArgsList = new List<string>();
            foreach(string arg in cliAuthArgs)
                cliAuthArgsList.Add(arg.Trim());

            bool hasNoAppConfig = ConfigurationManager.AppSettings.Count == 0;
            bool hasMissingCliArgs = Helpers.HasNullOrWhiteSpace(cliAuthArgsList);

            if(hasNoAppConfig && hasMissingCliArgs)
            {
                throw new MissingAppConfigException("There is no App.config and not all authentication " + 
                "arguments have been provided.  Please provide all authentication arguments.");
            } 
            else
            {
                authArgs.ClientId = Helpers.KeepOriginalIfEmptyReplacement(authArgs.ClientId, cliAuthArgsList[0]);
                authArgs.ClientSecret = Helpers.KeepOriginalIfEmptyReplacement(authArgs.ClientSecret, cliAuthArgsList[1]);
                authArgs.Username = Helpers.KeepOriginalIfEmptyReplacement(authArgs.Username, cliAuthArgsList[2]);
                authArgs.Password = Helpers.KeepOriginalIfEmptyReplacement(authArgs.Password, cliAuthArgsList[3]);
                authArgs.ApiVersion = Helpers.KeepOriginalIfEmptyReplacement(authArgs.ApiVersion, cliAuthArgsList[4]);
            }

            return authArgs;
        }

        /// <summary>
        /// Creates a new instance of `EventArguments`
        /// using the supplied event arguments.
        /// <param name="cliEvtArgs">An enumerable containing the values supplied to the -e CLI flag.</param>
        /// </summary>
        private EventArguments CreateEventArgs(IEnumerable<string> cliEvtArgs)
        {
            EventArguments evtArgs = new EventArguments();

            List<object> cliEvtArgsList = new List<object>();

            foreach(string arg in cliEvtArgs)
                cliEvtArgsList.Add(arg);
            
            evtArgs.EventClassName = (string) cliEvtArgsList[0];
            
            // don't include event name in field values
            cliEvtArgsList.RemoveAt(0);

            evtArgs.EventFieldsPropValues = cliEvtArgsList;

            return evtArgs;
        }

        /// <summary>
        /// Creates a new instance of `SFDCClient`
        /// using the authentication arguments in
        /// `_AuthArgs` to interact with Salesforce's
        /// REST API.
        /// </summary>
        private SFDCClient CreateClient() 
        {
            SFDCClient client = new SFDCClient(new HttpClient());
            client.ClientId = _AuthArgs.ClientId;
            client.ClientSecret = _AuthArgs.ClientSecret;
            client.Username = _AuthArgs.Username;
            client.Password = _AuthArgs.Password;
            client.ApiVersion = _AuthArgs.ApiVersion;

            return client;
        }

        /// <summary>
        /// Injects an event into Salesforce using
        /// the supplied CLI authentication and 
        /// event arguments.
        /// </summary>
        public int Init(InjectOptions o)
        {
            _AuthArgs = CreateAuthArgs(o.AuthArgs);
            _EventArgs = CreateEventArgs(o.EventArgs);
            _Client = CreateClient();

            dynamic evt = EventCreator.CreateEvent(_EventArgs.EventClassName, 
            _EventArgs.EventFieldsClassName, _EventArgs.EventFieldsPropValues);

            _Client.RequestAccessToken().Wait();
            _Client.InjectEvent(evt).Wait();

            return 0;
        }
    }
}