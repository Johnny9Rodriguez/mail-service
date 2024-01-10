namespace MailService;

public static class RazorExtensions
{
    public static RazorLight.Text.RawString Raw(string content)
    {
        return new RazorLight.Text.RawString(content);
    }
}