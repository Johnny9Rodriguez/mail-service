# Mail-Service

The Mail-Service is a lightweight tool designed for effortlessly sending HTML emails. Deployable on your server, it provides a simple and efficient solution for sending HTML emails with support for razor templating.

I built this app to simplify sending HTML emails for job applications and to use it in my web apps for the contact field.

### Key Features

-   **Easy Deployment:** The Mail-Service is designed to be easily deployed on any server. It has minimal dependencies and a straightforward setup process.
-   **Razor Templating Support:** This feature allows you to create dynamic HTML emails with Razor syntax.

## Requirements

This project requires the following to be installed on your system:

-   [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0): The project is built using .NET 8.0. You can download it from the official .NET website.

Please ensure you have the correct version installed before running the project.

## Installation

1. Clone the repository:

    ```sh
    git clone git@github.com:Johnny9Rodriguez/mail-service.git
    ```

2. Navigate to the project directory:

    ```sh
    cd mail-service
    ```

3. Set up your environment variables. Create a `appsettings.json` file in the root directory of the project and add your environment variables:

    ```json
    {
        "Logging": {
            "LogLevel": {
                "Default": "Information",
                "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "Port": "8010",
        "TemplatePath": "./Templates",
        "Client": {
            "SenderName": "yourName",
            "SenderAddress": "yourEmailAddress",
            "SmtpServer": "yourSmtpServer",
            "SmtpPort": "yourSmtpPort",
            "Username": "yourSmtpUsername",
            "Password": "yourSmtpPassword"
        }
    }
    ```

4. Restore the .NET packages:

    ```sh
    dotnet restore MailService.csproj
    ```

5. Run the application:

    ```sh
    dotnet run
    ```

The application will start and listen for HTTP requests. You can test the API communication by sending a GET request to the root `/` endpoint.

#### Send an Email

To send an email using the `/example` endpoint, make a POST request with an `ExampleModel` object. The `ExampleModel` object should match the structure defined in [`ExampleModel.cs`](/Models/ExampleModel.cs).

```json
{
    "RecipientName": "John Doe",
    "RecipientAddress": "john@doe.com",
    "Subject": "Example",
    "Message": "This is an example."
}
```

The email template used for the ''/example' endpoint is located in [`Example.cshtml`](/Templates/Example.cshtml). You can modify this template as needed.

## Adding a New Route, Model, and Template

### 1. Create a new Model

Create a new C# class file in the `Models` directory. For example, `NewModel.cs`. This file will define the data structure for your new endpoint. The new model class should implement the `IMailModel` interface.

The `Key` property is required for the caching of compiled templates. The `RecipientName`, `RecipientAddress`, and `Subject` properties are used by the `MailBuilder` to set the corresponding fields. These are not primarily meant for adding to the HTML template.

```csharp
public class NewModel : IMailModel
{
    public string Key { get; } = "newModel";

    public string RecipientName { get; set; }
    public string RecipientAddress { get; set; }
    public string Subject { get; set; }

    // CUSTOM PROPERTIES
    public string Message { get; set; }
    // Add more properties as needed.
}
```

### 2. Create a new Template

Create a new `.cshtml` file in the `Templates` (or in the specified path in the `.env`) directory. For example, `NewTemplate.cshtml`.

In the template, you can access the properties of your model using `@Model.PropertyName`. For example, `@Model.Message` will insert the value of the `Message` property from your model into the HTML.

```html
@using MailService @model NewModel
<html>
    <body>
        <h1>New Model</h1>
        <p>@Model.Message</p>
    </body>
</html>
```

#### HTML Injection

If you need to inject HTML content into your template, you can use the `@RazorExtensions.Raw()` method from [`RazorExtensions.cs`](/Services/RazorExtensions.cs). This method takes a string of HTML and inserts it into the template as raw HTML, without encoding special characters. This is particularly useful when you want to add styling or links to your email body.

```html
@using MailService @model NewModel
<html>
    <body>
        <h1>New Model</h1>
        <p>@RazorExtensions.Raw(Model.Message)</p>
    </body>
</html>
```

### 3. Create a new Route

In `RouteConfig.cs`, add a new route for your endpoint.

```csharp
app.MapPost("/new", async (NewModel model) =>
{
    string filePath = Path.Combine(templatePath, "NewTemplate.cshtml");
    string template = await File.ReadAllTextAsync(filePath);
    return await mailSender.Send(model, template);
});
```

This code creates a new POST route at `/new` that accepts a `NewModel` as the request body. It reads the `NewTemplate.cshtml` file from the `Templates` directory and sends an email using the `NewModel` and the template.

## API Key Usage

This application allows the use of an API key in the request header `x-api-key`for authentication. To protect a route with an API key, add the API key to your `appsettings.json`:

```json
{
	"ApiKey": "yourApiKey"
}
```

To protect a route with the API key, you need to use the [`IApiKeyValidation`](/Security/IApiKeyValidation.cs) service in your route configuration. Here's an example of how to do this in a `MapPost` method:

```csharp
app.MapPost("/new", async (HttpContext httpContext, NewModel model) =>
{
    var validationResponse = apiKeyValidation.ValidateApiKey(httpContext);
    if (validationResponse != null)
    {
        return validationResponse;
    }

    // Your existing code here...
});
```

In this code, `apiKeyValidation.ValidateApiKey(httpContext)` checks if the request contains a valid API key. If it doesn't, it returns an error response, and the rest of the code is not executed.

### Sending a Request with an API Key

To send a request to a protected route, you need to add the API key to the request header. The key should be in the format `x-api-key: yourApiKey`. 

## Deployment

### Publish the Project

Use the `dotnet publish` command to compile the application, its dependencies, and its .NET runtime into a folder for deployment to a hosting system. Run the following command in your terminal:

```sh
dotnet publish MailService.csproj -o ./publish
```

This command will create a `publish` directory at the root of your project which contains the compiled application and its dependencies.

#### Update the Template Path

By default, the templates located in the `Templates` directory are copied to the publish directory when you run the `dotnet publish` command. This behavior is defined in the [`MailService.csproj`](MailService.csproj) file:

```xml
<ItemGroup>
  <None Include="Templates\**" CopyToPublishDirectory="PreserveNewest" />
</ItemGroup>
```

You can change this behavior by removing or commenting out the line above.

### Run the Application

After publishing and setting up the environment variables, you can run the application using the `dotnet` command:

```sh
cd publish
dotnet MailService.dll
```

This command will start the application using the settings and dependencies in the `publish` directory.

Remember to ensure that the server where you're deploying the application has the necessary runtime installed. For this project, you need the .NET 8.0 runtime.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

If you have any questions or feedback, contact me at contact@joepytlik.de.
