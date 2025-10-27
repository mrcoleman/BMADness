using System.ClientModel;
using System.ClientModel.Primitives;
using BMADness.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;

namespace BMADness.Providers;

public class OpenrouterProvider : IProvider
{
    private readonly OpenAIClient _client;
    public string Model { get; set; }

    public OpenrouterProvider(AllSettings settings)
    {
        _client = new OpenAIClient(new ApiKeyCredential(settings.Providers.Openrouter.ApiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(settings.Providers.Openrouter.Url),
            ClientLoggingOptions = new ClientLoggingOptions
            {
                EnableLogging = true
            }
        });
    }
    
    public IChatClient GetChatClient() => _client.GetChatClient(Model)
        .AsIChatClient()
        .AsBuilder()
        .UseOpenTelemetry(sourceName: "BMADness", configure: (cfg) => cfg.EnableSensitiveData = true)    // Enable OpenTelemetry instrumentation with sensitive data
        .Build();
    public IEnumerable<string> GetModels()
    {
        var models = _client.GetOpenAIModelClient().GetModels(CancellationToken.None);
        return models.Value.Select(m => m.Id).ToArray();
    }
}