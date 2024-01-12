using Microsoft.AspNetCore.Mvc;

namespace MailService;

public class RouteConfig
{
    private readonly IApiKeyValidation apiKeyValidation;
    private readonly IConfiguration configuration;
    private readonly string templatePath;

    public RouteConfig(IApiKeyValidation apiKeyValidation, IConfiguration configuration)
    {
        this.apiKeyValidation = apiKeyValidation;
        this.configuration = configuration;
        templatePath = this.configuration["TemplatePath"] ?? "./Templates";
    }

    public void ConfigureRoutes(WebApplication app, MailSender mailSender)
    {
        // Default route for testing API communication.
        app.MapGet("/", () => "Ok");

        app.MapPost("/example", async (ExampleModel model) =>
        {
            string filePath = Path.Combine(templatePath, "Example.cshtml");
            string template = await File.ReadAllTextAsync(filePath);
            return await mailSender.Send(model, template);
        });

        app.MapPost("/api-key-example", async (HttpContext httpContext, ExampleModel model) =>
        {
            var validationResponse = apiKeyValidation.ValidateApiKey(httpContext);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            string filePath = Path.Combine(templatePath, "Example.cshtml");
            string template = await File.ReadAllTextAsync(filePath);
            return await mailSender.Send(model, template);
        });

        // Add custom routes here.
    }
}
