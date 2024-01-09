using MailService;

DotNetEnv.Env.Load();
string apiPort = Environment.GetEnvironmentVariable("API_PORT") ?? "5000";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://localhost:{apiPort}");
var app = builder.Build();

RouteConfig.ConfigureRoutes(app);

app.Run();