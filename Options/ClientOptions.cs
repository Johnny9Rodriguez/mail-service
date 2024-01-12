namespace MailService;

public class ClientOptions
{
    public string? SenderName { get; set; }
    public string? SenderAddress { get; set; }
    public string? SmtpServer { get; set; }
    public string? SmtpPort { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}