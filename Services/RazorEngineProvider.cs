using RazorLight;

namespace MailService;

public static class RazorEngineProvider
{
    public static RazorLightEngine RazorEngine { get; }

    static RazorEngineProvider()
    {
        RazorEngine = new RazorLightEngineBuilder()
            .UseMemoryCachingProvider()
            .Build();
    }
}