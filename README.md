# sfdc-injector

This is a CLI console application built with .Net core that publishes events to Salesforce.

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