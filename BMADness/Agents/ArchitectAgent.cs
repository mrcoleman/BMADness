using System.Collections;
using System.Text.Json;
using BMADness.Providers;
using BMADness.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace BMADness.Agents;

public class ArchitectAgent : IAgent
{
    private readonly ChatClientAgent _agent;
    private AgentThread _thread;

    public ArchitectAgent(IProvider provider)
    {
        
        _agent = provider.GetChatClient().CreateAIAgent(new ChatClientAgentOptions
        {
            Instructions = File.ReadAllText("./.bmad-core/agents/architect.md"),
            Name = "Architect",
            ChatOptions = new ChatOptions()
            {
                Tools = FileTools.GetAllTools()
                
            }
        });
        _thread = _agent.GetNewThread();
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> SendMessage(string message, CancellationToken token)
    {
        try
        {
            return _agent.RunStreamingAsync(message, _thread, cancellationToken: token);
        }
        catch (Exception e)
        {
            Console.WriteLine( $"{e.Message}\n{e.StackTrace}");
        }
        return null;
    }

    public JsonElement GetHistory()
    {
        return _thread.Serialize(null);
    }

    public void SetHistoryFromString(JsonElement history)
    {
        _thread = _agent.DeserializeThread(history);
    }
}