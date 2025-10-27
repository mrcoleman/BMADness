using Microsoft.Extensions.AI;

namespace BMADness.Providers;

public interface IProvider
{
    string Model { get; set; }
    IChatClient GetChatClient();
    IEnumerable<string> GetModels();
}