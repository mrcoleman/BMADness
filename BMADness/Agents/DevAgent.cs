using BMADness.Providers;
using BMADness.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace BMADness.Agents;

public class DevAgent : IAgent
{
    private readonly ChatClientAgent _agent;

    public DevAgent(IProvider provider)
    {
        _agent = provider.GetChatClient().CreateAIAgent(new ChatClientAgentOptions
        {
            Instructions = File.ReadAllText("./.bmad-core/agents/dev.md"),
            Name = "PM",
            ChatOptions = new ChatOptions()
            {
                Tools = FileTools.GetAllTools()
                
            }
        });
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> SendMessage(string message, CancellationToken token) =>
        _agent.RunStreamingAsync(message, cancellationToken: token);
}