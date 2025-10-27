namespace BMADness.Configuration;

public sealed class ProvdersSettings
{
    public UrlApiKeySettings? OpenAI { get; set; }
    public UrlApiKeySettings? Ollama { get; set; } = new() { Url = "http://localhost:11434" };
    public UrlApiKeySettings? LMStudio { get; set; } = new() { Url = "http://localhost:1234" };
    public UrlApiKeySettings? GoogleAI { get; set; }
    public UrlApiKeySettings? Claude { get; set; }
    public UrlApiKeySettings? Openrouter { get; set; }
    public UrlApiKeySettings? Zai { get; set; }
}