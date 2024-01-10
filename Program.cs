using MailService;

DotNetEnv.Env.Load();
string apiPort = Environment.GetEnvironmentVariable("API_PORT") ?? "5000";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://localhost:{apiPort}");
builder.Configuration.AddEnvironmentVariables(prefix: "Client_");

// Add localhost and 127.0.0.1 to allowed CORS origins.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed(origin =>
                   {
                       var host = new Uri(origin).Host;
                       return host == "localhost" || host == "127.0.0.1";
                   });
        });
});

var app = builder.Build();

app.UseCors("AllowLocalhost");

ClientOptions clientOptions = new(app.Configuration);
MailSender mailSender = new(clientOptions);

RouteConfig.ConfigureRoutes(app, mailSender);

app.Run();