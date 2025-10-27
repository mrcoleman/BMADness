using BMADness.Providers;
using BMADness.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace BMADness.Agents;

public interface IAgent
{
    public IAsyncEnumerable<AgentRunResponseUpdate> SendMessage(string message, CancellationToken token);
}

public class AnalystAgent : IAgent
{
    private readonly ChatClientAgent _agent;

    public AnalystAgent(IProvider provider)
    {
        _agent = provider.GetChatClient().CreateAIAgent(new ChatClientAgentOptions
        {
            Instructions = File.ReadAllText("./.bmad-core/agents/analyst.md"),
            Name = "Analyst",
            ChatOptions = new ChatOptions()
            {
                Tools = FileTools.GetAllTools()
            }
        });
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> SendMessage(string message, CancellationToken token) =>
        _agent.RunStreamingAsync(message, cancellationToken: token);
}