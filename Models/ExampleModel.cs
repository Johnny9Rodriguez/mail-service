namespace MailService;

public class ExampleModel : IMailModel
{
    public string Key { get; } = "example";
    public required string RecipientName { get; set; }
    public required string RecipientAddress { get; set; }
    public required string Subject { get; set; }

    // Custom properties.
    public required string Message { get; set; }
}