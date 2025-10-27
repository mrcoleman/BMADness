using BMADness.Providers;
using BMADness.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace BMADness.Agents;

public class QAAgent : IAgent
{
    private readonly ChatClientAgent _agent;

    public QAAgent(IProvider provider)
    {
        _agent = provider.GetChatClient().CreateAIAgent(new ChatClientAgentOptions
        {
            Instructions = File.ReadAllText("./.bmad-core/agents/qa.md"),
            Name = "QA",
            ChatOptions = new ChatOptions()
            {
                Tools = FileTools.GetAllTools()
                
            }
        });
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> SendMessage(string message, CancellationToken token) =>
        _agent.RunStreamingAsync(message, cancellationToken: token);
}