namespace MailService;

public static class RouteConfig
{
    public static void ConfigureRoutes(WebApplication app, MailSender mailSender, string templatePath)
    {
        // Default route for testing API communication.
        app.MapGet("/", () => "Ok");

        app.MapPost("/example", async (ExampleModel model) =>
        {
            string filePath = Path.Combine(templatePath, "Example.cshtml");
            string template = await File.ReadAllTextAsync(filePath);
            return await mailSender.Send(model, template);
        });

        // Add custom routes here.
    }
}