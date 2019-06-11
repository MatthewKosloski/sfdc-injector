# sfdc-injector

This is a CLI console application built with .Net core that publishes events to Salesforce.

## Prerequisites

Before using this application, please complete the following software and Salesforce configuration prerequisites.

### Software Prerequisites

Please download the following software before using this application.

- [.Net Core](https://dotnet.microsoft.com/download)

### Salesforce Configuration Prerequisites

Before using this application, you must create a connected app in your Salesforce development org:

1. From `Setup`, search for `"app manager"` in the `Quick Find` search box.
2. Click on `Apps > App Manager`.
3. Click on `New Connected App`.
4. Give your app a `Connected App Name`, `API Name`, and `Contact Email`.
5. Make sure `Enable OAuth Settings` is checked.
6. Make sure `Enable for Device Flow` is checked.
7. Make sure the `Callback URL` is `https://login.salesforce.com/services/oauth2/success`.
8. Select the `"Access and manage your data (api)"` OAuth Scope is selected.
9. Make sure `Require Secret for Web Server Flow` is checked.
10. Take note of your `Consumer Key` and `Consumer Secret`.  Copy and paste these into `App.config`.

## Usage

Follow the below instructions to use this application on your computer.

1. Download this repo's .ZIP folder or clone the repo.
2. From the root directory, build the binary files using the subsequent command.  After running the command, you should see two new folders, `bin` and `obj`.
    ```
    dotnet build
    ```
3. From the root directory, create an `App.config` file to house your API credentials.  Paste the subsequent contents into the file, modifying it to suit your credentials.  The API version for `ApiEndpoint` may need to be modified.  **Note: This file is ignored by Git version control to avoid exposing your credentials.**
    ```
    <?xml version="1.0" encoding="utf-8" ?>  
    <configuration>  
        <startup>   
            <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />  
        </startup>  
        <appSettings>
            <add key="LoginEndpoint" value="https://login.salesforce.com/services/oauth2/token" />
            <add key="ApiEndpoint" value="/services/data/v43.0/" />
            <add key="ClientId" value="<YOUR_CLIENT_ID>" />  
            <add key="ClientSecret" value="<YOUR_CLIENT_SECRET>" />  
            <add key="Username" value="<YOUR_USERNAME>" />
            <add key="Password" value="<YOUR_PASSWORD_AND_SECURITY_TOKEN>" />
        </appSettings>  
    </configuration>
    ```
4. Run the application using the subsequent command:
    ```
    dotnet run
    ```
## Testing

To run the tests, change your directory to `SFDCInjector.Tests` and use this command:

```
dotnet test
```

To generate code coverage report files in a `SFDCInjector.Tests/coverage` directory, run:

```
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=\"json,lcov\" -p:CoverletOutput='./coverage/'
```

## Resources

### Salesforce
- [Consuming Force.com SOAP and REST Web Services from .NET Applications](https://developer.salesforce.com/page/Consuming_Force.com_SOAP_and_REST_Web_Services_from_.NET_Applications)
- [Platform Events Developer Guide](https://developer.salesforce.com/docs/atlas.en-us.platform_events.meta/platform_events/platform_events_intro.htm)
- [REST API Developer Guide](https://developer.salesforce.com/docs/atlas.en-us.api_rest.meta/api_rest/intro_what_is_rest_api.htm)
- [SOAP API Developer Guide](https://developer.salesforce.com/docs/atlas.en-us.218.0.api.meta/api/sforce_api_quickstart_intro.htm)
- [Integrate Salesforce APIs with .Net Core](https://www.forcetalks.com/blog/how-to-integrate-salesforce-streaming-api-with-net-core-application/)
- [Integrating .Net and Salesforce](https://blog.mkorman.uk/integrating-net-and-salesforce-part-1-rest-api/)
- [Username-Password OAuth Flow](https://developer.salesforce.com/docs/atlas.en-us.api_rest.meta/api_rest/intro_understanding_username_password_oauth_flow.htm)

### C# and .Net
- [REST Client](https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient)
- [HTTP Client class](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/index)
- [A Tour of C#](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/index)
- [ConfigurationManager Class](https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=netframework-4.8)
- [Serialize and Deserialize JSON data](https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data)
- [Documenting code with XML](https://docs.microsoft.com/en-us/dotnet/csharp/codedoc)
- [Flurl HTTP library](https://flurl.dev/)
- [Best Practices for Exceptions](https://docs.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)
- [Command Line Parser](https://github.com/commandlineparser/commandline)
- [C# Coding Standards and Naming Conventions](https://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Conventions.md)
- [Reflection](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/reflection)
- [Attributes](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/)

### Testing
- [Unit Testing Async Code](https://msdn.microsoft.com/en-us/magazine/dn818493.aspx)
- [Naming Standards for Unit Tests](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html)