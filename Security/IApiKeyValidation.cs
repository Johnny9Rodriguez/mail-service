using Microsoft.AspNetCore.Mvc;

namespace MailService;

public interface IApiKeyValidation
{
    IResult? ValidateApiKey(HttpContext httpContext);
}