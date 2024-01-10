using RazorLight;

namespace MailService;

public class MailRenderer
{
    readonly RazorLightEngine engine = RazorEngineProvider.RazorEngine;

    public async Task<string> Render<TModel>(string templateKey, TModel model, string template)
    {
        string htmlRender = "";

        var cacheData = engine.Handler.Cache.RetrieveTemplate(templateKey);
        if (cacheData.Success)
        {
            var cacheTemplate = cacheData.Template.TemplatePageFactory();
            htmlRender = await engine.RenderTemplateAsync(cacheTemplate, model);
        }
        else
        {
            htmlRender = await engine.CompileRenderStringAsync(templateKey, template, model);
        }
        
        return htmlRender;
    }
}