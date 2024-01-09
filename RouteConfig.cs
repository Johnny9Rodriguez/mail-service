namespace MailService
{
    public static class RouteConfig
    {
        public static void ConfigureRoutes(WebApplication app)
        {
            // Default route for testing API
            app.Map("/", () => "Ok");
        }
    }
}