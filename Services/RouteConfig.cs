namespace MailService;

public static class RouteConfig
{
    public static void ConfigureRoutes(WebApplication app, MailSender mailSender)
    {
        // Default route for testing API communication.
        app.MapGet("/", () => "Ok");

        app.MapPost("/example", async (ExampleModel model) =>
        {
            string template = await File.ReadAllTextAsync("./Templates/Example.cshtml");
            mailSender.Send(model, template);
        });
    }
}