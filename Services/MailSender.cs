using MailKit.Net.Smtp;
using Microsoft.VisualBasic;
using MimeKit;
using Sprache;

namespace MailService;

public class MailSender(ClientOptions clientOptions)
{
    private readonly MailRenderer mailRenderer = new();
    private readonly MailBuilder mailBuilder = new(clientOptions);

    public async Task<IResult> Send<TModel>(TModel model, string template) where TModel : IMailModel
    {
        string htmlRender = await mailRenderer.Render(model.Key, model, template);
        MimeMessage message = mailBuilder.Build(model, htmlRender);

        try
        {
            var smtpClient = new SmtpClient();
            smtpClient.Connect(clientOptions.SmtpServer, clientOptions.SmtpPort, false);
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