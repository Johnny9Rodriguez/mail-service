using MailService;

var builder = WebApplication.CreateBuilder(args);
string apiPort = builder.Configuration["Port"] ?? "5000";
builder.WebHost.UseUrls($"http://localhost:{apiPort}");
builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection("Client"));
builder.Services.AddTransient<MailSender>();
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddTransient<RouteConfig>();
builder.Services.AddHttpLogging(o => { });

var app = builder.Build();
app.UseHttpLogging();

MailSender mailSender = app.Services.GetRequiredService<MailSender>();
RouteConfig routeConfig = app.Services.GetRequiredService<RouteConfig>();
routeConfig.ConfigureRoutes(app, mailSender);

app.Run();