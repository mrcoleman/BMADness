using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using BMADness.Agents;
using BMADness.Providers;
using BMADness.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BMADness.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    private readonly IProvider _provider;
    private readonly AnalystAgent _agent;
    public ObservableCollection<ChatItem> ChatItems { get; set; } = [];
    public MainWindowViewModel(IProvider provider)
    {
        _provider = provider;
        _provider.Model = "google/gemma-3n-e4b";
        _agent = new AnalystAgent(provider);
    }
    
    
    public string Message { get; set; }
    private bool _sending = false;
    
    [RelayCommand]
    public async void OnSendClick()
    {
        if (_sending)
            return;
        
        var chatItem = new ChatItem
        {
            Message = Message,
            Name = "You"
        };
        ChatItems.Add(chatItem);
        var aiChatItem = new ChatItem
        {
            Name = "Them",
            Message = ""
        };
        ChatItems.Add(aiChatItem);
        var item = ChatItems.Last();
        var savedMessage = Message;
        Message=string.Empty;
        _sending = true;
        await foreach (var response in _agent.SendMessage(savedMessage,CancellationToken.None))
        {
            item.Message += response.Text;
            ChatItems[ChatItems.Count() - 1] = item;
        }
        _sending = false;
    }
}