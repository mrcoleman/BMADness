using System.ClientModel;
using System.ClientModel.Primitives;
using BMADness.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;
using System.Linq;

namespace BMADness.Providers;

public class LMStudioProvider: IProvider
{
    private readonly OpenAIClient _client;
    private readonly OpenAIClientOptions _clientOptions;
    public string Model { get; set; }
    public List<string> Models { get; set; } = new ();
    public LMStudioProvider(AllSettings settings)
    {
        _clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(settings.Providers?.LMStudio?.Url!),
            ClientLoggingOptions = new ClientLoggingOptions
            {
                EnableLogging = true
            }
        };
        
        if(settings.Providers?.LMStudio?.ApiKey!=string.Empty)
            _client = new OpenAIClient(new ApiKeyCredential(settings.Providers?.LMStudio?.ApiKey),_clientOptions);
        else
            _client = new OpenAIClient(new ApiKeyCredential("BMADness"), _clientOptions);
    }

    public IChatClient GetChatClient() => _client.GetChatClient(Model).AsIChatClient();
    public IEnumerable<string> GetModels()
    {
        var models = _client.GetOpenAIModelClient().GetModels(CancellationToken.None);
        return models.Value.Select(m => m.Id).ToArray();
    }
}