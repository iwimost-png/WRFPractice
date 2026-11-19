using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WRFPractice.Models;

namespace WRFPractice.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly AppSettings _settings;

    [ObservableProperty]
    private AiProvider _selectedProvider;

    [ObservableProperty]
    private string _openAiApiKey = string.Empty;

    [ObservableProperty]
    private string _openAiModel = "gpt-4o";

    [ObservableProperty]
    private string _openAiEndpoint = "https://api.openai.com/v1/chat/completions";

    [ObservableProperty]
    private string _geminiApiKey = string.Empty;

    [ObservableProperty]
    private string _geminiModel = "gemini-1.5-flash";

    [ObservableProperty]
    private string _customEndpoint = "http://localhost:11434/v1/chat/completions";

    [ObservableProperty]
    private string _customModel = "llama3";

    [ObservableProperty]
    private string _systemPrompt = string.Empty;

    [ObservableProperty]
    private double _temperature = 0.7;

    public ObservableCollection<AiProvider> AvailableProviders { get; } = new([
        AiProvider.Mock,
        AiProvider.OpenAi,
        AiProvider.Gemini,
        AiProvider.CustomOllama
    ]);

    public event Action? RequestClose;

    public SettingsViewModel(AppSettings settings)
    {
        _settings = settings;
        LoadFromSettings();
    }

    private void LoadFromSettings()
    {
        SelectedProvider = _settings.Provider;
        OpenAiApiKey = _settings.OpenAiApiKey;
        OpenAiModel = _settings.OpenAiModel;
        OpenAiEndpoint = _settings.OpenAiEndpoint;
        GeminiApiKey = _settings.GeminiApiKey;
        GeminiModel = _settings.GeminiModel;
        CustomEndpoint = _settings.CustomEndpoint;
        CustomModel = _settings.CustomModel;
        SystemPrompt = _settings.SystemPrompt;
        Temperature = _settings.Temperature;
    }

    [RelayCommand]
    private void Save()
    {
        _settings.Provider = SelectedProvider;
        _settings.OpenAiApiKey = OpenAiApiKey.Trim();
        _settings.OpenAiModel = OpenAiModel.Trim();
        _settings.OpenAiEndpoint = OpenAiEndpoint.Trim();
        _settings.GeminiApiKey = GeminiApiKey.Trim();
        _settings.GeminiModel = GeminiModel.Trim();
        _settings.CustomEndpoint = CustomEndpoint.Trim();
        _settings.CustomModel = CustomModel.Trim();
        _settings.SystemPrompt = SystemPrompt.Trim();
        _settings.Temperature = Temperature;

        _settings.Save();
        RequestClose?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke();
    }
}
