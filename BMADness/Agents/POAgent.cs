using BMADness.Providers;
using BMADness.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace BMADness.Agents;

public class POAgent : IAgent
{
    private readonly ChatClientAgent _agent;

    public POAgent(IProvider provider)
    {
        _agent = provider.GetChatClient().CreateAIAgent(new ChatClientAgentOptions
        {
            Instructions = File.ReadAllText("./.bmad-core/agents/po.md"),
            Name = "PO",
            ChatOptions = new ChatOptions()
            {
                Tools = FileTools.GetAllTools()
                
            }
        });
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> SendMessage(string message, CancellationToken token) =>
        _agent.RunStreamingAsync(message, cancellationToken: token);
}