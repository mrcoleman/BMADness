using System.ClientModel;
using System.ClientModel.Primitives;
using BMADness.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;

namespace BMADness.Providers;

public class OllamaProvider:IProvider
{
    private readonly OpenAIClient _client;
    public string Model { get; set; }
    public List<string> Models { get; set; } = new ();
    
    public OllamaProvider(AllSettings settings)
    {
        _client = new OpenAIClient(new ApiKeyCredential(settings.Providers.Ollama.ApiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(settings.Providers.Ollama.Url),
            ClientLoggingOptions = new ClientLoggingOptions
            {
                EnableLogging = true
            }
        });
    }
    
    public IChatClient GetChatClient() => _client.GetChatClient(Model).AsIChatClient();
    public IEnumerable<string> GetModels()
    {
        var models = _client.GetOpenAIModelClient().GetModels(CancellationToken.None);
        return models.Value.Select(m => m.Id).ToArray();
    }
}