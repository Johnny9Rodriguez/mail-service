# Mail-Service

The Mail-Service is a lightweight tool designed for effortlessly sending HTML emails. Deployable on your server, it provides a simple and efficient solution for sending HTML emails with support for razor templating.

I built this app to simplify sending HTML emails for job applications and to use it in my web apps for the contact field.

### Key Features

-   **Easy Deployment:** The Mail-Service is designed to be easily deployed on any server. It has minimal dependencies and a straightforward setup process.
-   **Razor Templating Support:** This feature allows you to create dynamic HTML emails with Razor syntax.

## Requirements

This project requires the following to be installed on your system:

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0): The project is built using .NET 8.0. You can download it from the official .NET website.

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

3. Set up your environment variables. Create a `.env` file in the root directory of the project and add your environment variables:

   ```sh
   touch .env
   ```

   Open the `.env` file and add your variables:

   ```plaintext
   API_PORT=8010 #default
   TEMPLATE_PATH="./Templates" #default 
   
   Client_SENDER_NAME=your_name
   Client_SENDER_ADDRESS=your_email_address
   
   Client_SMTP_SERVER=your_smtp_server
   Client_SMTP_PORT=your_smtp_port
   
   Client_USERNAME=your_smtp_username
   Client_PASSWORD=your_smtp_password
   ```

4. Restore the .NET packages:

   ```sh
   dotnet restore
   ```

5. Run the application:

   ```sh
   dotnet run
   ```

The application will start and listen for HTTP requests. You can test the API communication by sending a GET request to the root `/` endpoint.

To send an email using the `/example` endpoint, make a POST request with an `ExampleModel` object. The `ExampleModel` object should match the structure defined in [`ExampleModel.cs`](/Models/ExampleModel.cs).

The email template used for the ''/example' endpoint is located in [`Example.cshtml`](/Templates/Example.cshtml). You can modify this template as needed.

## Adding a New Route, Model, and Template

### 1. Create a new model

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

### 2. Create a new template

Create a new `.cshtml` file in the `Templates` (or in the specified path in the `.env`) directory. For example, `NewTemplate.cshtml`.

In the template, you can access the properties of your model using `@Model.PropertyName`. For example, `@Model.Message` will insert the value of the `Message` property from your model into the HTML.

```html
@using MailService
@model NewModel
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
@using MailService
@model NewModel
<html>
    
<body>
    <h1>New Model</h1>
    <p>@RazorExtensions.Raw(Model.Message)</p>
</body>
    
</html>
```

### 3. Create a new route

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

## Deployment

### 1. Publish the project

Use the `dotnet publish` command to compile the application, its dependencies, and its .NET runtime into a folder for deployment to a hosting system. Run the following command in your terminal:

```sh
dotnet publish MailService.csproj -o ./publish
```

This command will create a `publish` directory at the root of your project which contains the compiled application and its dependencies.

### 2. Update the .env file

By default, the `.env` file is copied to the output directory during the build process. This is specified in the [`MailService.csproj`](MailService.csproj) file:

```xml
<Content Include=".env">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</Content>
```

If you don't want to copy the `.env` file to the output directory by default, you can remove this entry from the `.csproj` file. However, you will need to manually copy the `.env` file to the `publish` directory after running the `dotnet publish` command.

### 3. Update the template file directory

 In the production environment, you may need to update the `TEMPLATE_PATH` variable in the `.env` file to point to the correct directory where your template files are located.

### 4. Run the application

After publishing and setting up the environment variables, you can run the application using the `dotnet` command:

```sh
dotnet ./publish/MailService.dll
```

This command will start the application using the settings and dependencies in the `publish` directory.

Remember to ensure that the server where you're deploying the application has the necessary runtime installed. For this project, you need the .NET 8.0 runtime.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

If you have any questions or feedback, contact me at contact@joepytlik.de.
