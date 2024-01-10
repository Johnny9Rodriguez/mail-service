using MimeKit;

namespace MailService;

public class MailBuilder(ClientOptions clientOptions)
{
    public MimeMessage Build<TModel>(TModel model, string htmlRender) where TModel : IMailModel
    {
        MimeMessage message = new();

        message.From.Add(new MailboxAddress(clientOptions.SenderName, clientOptions.SenderAddress));
        message.To.Add(new MailboxAddress(model.RecipientName, model.RecipientAddress));
        message.Subject = model.Subject;

        var bodyBuilder = new BodyBuilder
        {
            // TextBody = @"Placeholder",
            HtmlBody = htmlRender
        };

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
}