using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using BMADness.Configuration;
using BMADness.Providers;
using BMADness.UI.ViewModels;
using BMADness.UI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMADness.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        var settings = new AllSettings();
        var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.dev.json", optional: true)
            .Build();
        configBuilder.Bind(settings);
        var collection = new ServiceCollection();
        

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

        collection.AddSingleton<IProvider>(provider);
        collection.AddScoped<MainWindowViewModel>();
        var services = collection.BuildServiceProvider();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext =services.GetService<MainWindowViewModel>(),
            };
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}