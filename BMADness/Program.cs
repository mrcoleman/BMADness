using Microsoft.Extensions.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI;
using System.ClientModel.Primitives;
using System.ClientModel;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using BMADness.Agents;
using BMADness.Configuration;
using BMADness.Providers;
using OpenAI;

var settings = new AllSettings();
var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.dev.json", optional: true)
    .Build();
configBuilder.Bind(settings);

IProvider provider;
switch (settings.DefaultProvider)
{
    case PROVIDERS.LMSTUDIO:
    provider= new LMStudioProvider(settings);
        break;
    case PROVIDERS.CLAUDE:
        provider = new ClaudeProvider(settings);
        break;
    case PROVIDERS.GOOGLEAI:
        provider = new GoogleAIProvider(settings);
        break;
    case PROVIDERS.OLLAMA:
        provider = new OllamaProvider(settings);
        break;
    case PROVIDERS.OPENAI:
        provider = new OpenAIProvider(settings);
        break;
    case PROVIDERS.OPENROUTER:
        provider = new OpenrouterProvider(settings);
        break;
    case PROVIDERS.ZAI:
        provider = new ZaiProvider(settings);
        break;
    default:
        throw new ArgumentOutOfRangeException($"{settings.DefaultProvider} not supported");
}

Console.WriteLine("Which model do you want to use?");
foreach(var model in provider.GetModels())
    Console.WriteLine(model);

provider.Model = "z-ai/glm-4.5-air:free";
var agent =new ArchitectAgent(provider);
if (File.Exists("history.json"))
{
    var history = File.ReadAllText("history.json");
    agent.SetHistoryFromString(JsonDocument.Parse(history).RootElement);
}


Console.WriteLine("Welcome to BMADness!");
var nextMesssage = "*help";
var cancelToken = CancellationToken.None;
while (nextMesssage != "exit")
{
    try
    {
        await foreach (var response in agent.SendMessage(nextMesssage, cancelToken))
        {
            Console.Write(response.Text);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"**Finished with error {e.Message}");
    }

    Console.Write("\nYou: ");
    nextMesssage = Console.ReadLine() ?? "";
    if (nextMesssage == "exit") break;
}
var thread = agent.GetHistory();
File.WriteAllText("history.json", thread.GetRawText());