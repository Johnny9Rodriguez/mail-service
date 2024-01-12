using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailService;

public class MailSender
{
    private readonly MailRenderer mailRenderer;
    private readonly MailBuilder mailBuilder;
    private readonly ClientOptions clientOptions;

    public MailSender(IOptions<ClientOptions> clientOptions)
    {
        this.clientOptions = clientOptions.Value;
        mailRenderer = new MailRenderer();
        mailBuilder = new MailBuilder(this.clientOptions);
    }

    /// <summary>
    /// Sends an email using the provided model and template.
    /// </summary>
    /// <typeparam name="TModel">The type of the model. Must implement IMailModel.</typeparam>
    /// <param name="model">The model to use for the email.</param>
    /// <param name="template">The template to use for the email.</param>
    /// <returns>
    /// A Result object that represents the outcome of the email sending operation.
    /// Possible status codes are:
    /// - 200 (Ok): The email was sent successfully.
    /// - 401 (Unauthorized): Authentication with the SMTP server failed.
    /// - 502 (Bad Gateway): Connection to the SMTP server failed.
    /// - 500 (Internal Server Error): An unexpected exception occurred.
    /// </returns>
    public async Task<IResult> Send<TModel>(TModel model, string template) where TModel : IMailModel
    {
        string htmlRender = await mailRenderer.Render(model.Key, model, template);
        MimeMessage message = mailBuilder.Build(model, htmlRender);

        try
        {
            var smtpClient = new SmtpClient();
            smtpClient.Connect(clientOptions.SmtpServer, int.Parse(clientOptions.SmtpPort ?? "587"), false);
            smtpClient.Authenticate(clientOptions.Username, clientOptions.Password);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
            return Results.Ok("Mail sent successfully.");
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            string errorMessage = $"Connection failed: {ex.Message}";
            Console.WriteLine(errorMessage);
            return Results.Problem(detail: errorMessage, statusCode: 502);
        }
        catch (MailKit.Security.AuthenticationException ex)
        {
            string errorMessage = $"Authentication failed: {ex.Message}";
            Console.WriteLine(errorMessage);
            return Results.Problem(detail: errorMessage, statusCode: 401);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Exception: {ex.Message}";
            Console.WriteLine(errorMessage);
            return Results.Problem(detail: errorMessage, statusCode: 500);
        }
    }
}