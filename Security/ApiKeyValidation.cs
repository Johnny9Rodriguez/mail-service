using Microsoft.AspNetCore.Mvc;

namespace MailService;

public class ApiKeyValidation : IApiKeyValidation
{
    private readonly IConfiguration configuration;

    public ApiKeyValidation(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IResult? ValidateApiKey(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("x-api-key", out var apiKeyValues))
        {
            return Results.BadRequest("API key header not found");
        }

        var apiKey = apiKeyValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Results.BadRequest("API key is empty");
        }

        bool isValid = IsValidApiKey(apiKey);
        if (!isValid)
        {
            return Results.Unauthorized();
        }

        return null;
    }

    bool IsValidApiKey(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
            return false;

        string? apiKey = configuration.GetValue<string>("ApiKey");

        if (apiKey == null || apiKey != userApiKey)
            return false;

        return true;
    }
}