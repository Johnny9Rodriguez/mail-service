using MailKit.Net.Smtp;
using MimeKit;

namespace MailService;

public class MailSender(ClientOptions clientOptions)
{
    private readonly MailRenderer mailRenderer = new();
    private readonly MailBuilder mailBuilder = new(clientOptions);

    public async void Send<TModel>(TModel model, string template) where TModel : IMailModel
    {
        string htmlRender = await mailRenderer.Render(model.Key, model, template);
        MimeMessage message = mailBuilder.Build(model, htmlRender);

        var smtpClient = new SmtpClient();
        smtpClient.Connect(clientOptions.SmtpServer, clientOptions.SmtpPort, false);
        smtpClient.Authenticate(clientOptions.Username, clientOptions.Password);
        smtpClient.Send(message);
        smtpClient.Disconnect(true);
    }
}