using MailService;

var builder = WebApplication.CreateBuilder(args);
string apiPort = builder.Configuration["Port"] ?? "5000";
builder.WebHost.UseUrls($"http://localhost:{apiPort}");
builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection("Client"));
builder.Services.AddTransient<MailSender>();
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
builder.Services.AddHttpLogging(o => { });

var app = builder.Build();
app.UseHttpLogging();
app.UseCors("AllowLocalhost");

MailSender mailSender = app.Services.GetRequiredService<MailSender>();
string templatePath = app.Configuration["TemplatePath"] ?? "./Templates";
RouteConfig.ConfigureRoutes(app, mailSender, templatePath);

app.Run();