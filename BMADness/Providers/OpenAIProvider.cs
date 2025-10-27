using System.ClientModel;
using System.ClientModel.Primitives;
using BMADness.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;

namespace BMADness.Providers;

public class OpenAIProvider: IProvider
{
    private readonly OpenAIClient _client;
    public string Model { get; set; }

    public OpenAIProvider(AllSettings settings)
    {
        _client = new OpenAIClient(new ApiKeyCredential(settings.Providers.OpenAI.ApiKey), new OpenAIClientOptions
        {
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