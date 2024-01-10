namespace MailService;

public interface IMailModel
{
    string Key { get; }
    string RecipientName { get; set; }
    string RecipientAddress { get; set; }
    string Subject { get; set; }
}