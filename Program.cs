using MailService;

DotNetEnv.Env.Load();
string apiPort = Environment.GetEnvironmentVariable("API_PORT") ?? "5000";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://localhost:{apiPort}");
builder.Configuration.AddEnvironmentVariables(prefix: "Client_");
var app = builder.Build();

ClientOptions clientOptions = new (app.Configuration);
MailSender mailSender = new(clientOptions);

RouteConfig.ConfigureRoutes(app, mailSender);

app.Run();