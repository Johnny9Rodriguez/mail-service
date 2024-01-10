namespace MailService;

public class ClientOptions
{
    readonly IConfiguration configuration;

    public string? SenderName { get; }
    public string? SenderAddress { get; }
    public string? SmtpServer { get; }
    public int SmtpPort { get; }
    public string? Username { get; }
    public string? Password { get; }

    public ClientOptions(IConfiguration configuration)
    {
        this.configuration = configuration;

        SenderName = GetString("SENDER_NAME");
        SenderAddress = GetString("SENDER_ADDRESS");
        SmtpServer = GetString("SMTP_SERVER");
        SmtpPort = GetInt("SMTP_PORT");
        Username = GetString("USERNAME");
        Password = GetString("PASSWORD");
    }

    string GetString(string key)
    {
        string value = configuration[key] ?? string.Empty;

        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Please enter an environment variable for '" + key + "'");
        }

        return value;
    }

    int GetInt(string key)
    {
        if (!int.TryParse(configuration[key], out int value))
        {
            throw new ArgumentException("Please enter a valid integer value for '" + key + "'");
        }
        return value;
    }
}