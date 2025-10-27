using System.ClientModel;
using System.ClientModel.Primitives;
using BMADness.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;

namespace BMADness.Providers;

public class GoogleAIProvider : IProvider
{
    private readonly OpenAIClient _client;
    public string Model { get; set; }

    public GoogleAIProvider(AllSettings settings)
    {
        _client = new OpenAIClient(new ApiKeyCredential(settings.Providers.GoogleAI.ApiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(settings.Providers.GoogleAI.Url),
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