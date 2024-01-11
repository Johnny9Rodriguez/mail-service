namespace MailService;

public static class RouteConfig
{
    static string templatePath;

    static RouteConfig()
    {
        DotNetEnv.Env.Load();
        templatePath = Environment.GetEnvironmentVariable("TEMPLATE_PATH") ?? "./Templates";
    }

    public static void ConfigureRoutes(WebApplication app, MailSender mailSender)
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