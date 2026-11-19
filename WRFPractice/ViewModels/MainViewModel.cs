using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WRFPractice.Models;
using WRFPractice.Services;

namespace WRFPractice.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly AppSettings _settings;
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _statusText = "準備完了";

    [ObservableProperty]
    private string _providerDisplayName = "モックAI (オフラインテスト)";

    public ObservableCollection<ChatMessageViewModel> Messages { get; } = new();

    public event Action? RequestScrollToBottom;
    public event Action? RequestOpenSettings;

    public MainViewModel()
    {
        _settings = AppSettings.Load();
        UpdateProviderDisplay();

        // 初期ウェルカムメッセージ
        Messages.Add(new ChatMessageViewModel
        {
            Role = MessageRole.Assistant,
            Content = "こんにちは！AIチャットアシスタントです。🤖\n" +
                      "何か質問やお手伝いできることはありますか？\n\n" +
                      "※ 右上の「⚙ 設定」から OpenAI や Gemini の API キーを設定することで、最新のAIモデルと接続できます。",
            Timestamp = DateTime.Now
        });
    }

    public void RefreshSettings()
    {
        UpdateProviderDisplay();
    }

    private void UpdateProviderDisplay()
    {
        ProviderDisplayName = _settings.Provider switch
        {
            AiProvider.Mock => "モックAI (オフライン)",
            AiProvider.OpenAi => $"OpenAI ({_settings.OpenAiModel})",
            AiProvider.Gemini => $"Google Gemini ({_settings.GeminiModel})",
            AiProvider.CustomOllama => $"ローカル/Ollama ({_settings.CustomModel})",
            _ => "AIアシスタント"
        };
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        var text = InputText.Trim();
        if (string.IsNullOrWhiteSpace(text) || IsLoading) return;

        // 入力クリア
        InputText = string.Empty;

        // ユーザーメッセージ追加
        var userMsg = new ChatMessageViewModel
        {
            Role = MessageRole.User,
            Content = text,
            Timestamp = DateTime.Now
        };
        Messages.Add(userMsg);
        RequestScrollToBottom?.Invoke();

        IsLoading = true;
        StatusText = "AIが回答を生成中...";

        _cts = new CancellationTokenSource();

        try
        {
            var service = AiServiceFactory.GetService(_settings.Provider);
            var history = Messages.Select(m => m.ToModel()).ToList();

            var responseText = await service.SendMessageAsync(history, _settings, _cts.Token);

            var aiMsg = new ChatMessageViewModel
            {
                Role = MessageRole.Assistant,
                Content = responseText,
                Timestamp = DateTime.Now
            };
            Messages.Add(aiMsg);
            StatusText = "応答を受信しました";
        }
        catch (OperationCanceledException)
        {
            StatusText = "送信をキャンセルしました";
        }
        catch (Exception ex)
        {
            var errorMsg = new ChatMessageViewModel
            {
                Role = MessageRole.Assistant,
                Content = $"エラーが発生しました:\n{ex.Message}",
                Timestamp = DateTime.Now,
                IsError = true,
                ErrorMessage = ex.Message
            };
            Messages.Add(errorMsg);
            StatusText = "エラーが発生しました";
        }
        finally
        {
            IsLoading = false;
            _cts?.Dispose();
            _cts = null;
            RequestScrollToBottom?.Invoke();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        if (IsLoading && _cts != null)
        {
            _cts.Cancel();
            StatusText = "キャンセル中...";
        }
    }

    [RelayCommand]
    private void ClearHistory()
    {
        Messages.Clear();
        Messages.Add(new ChatMessageViewModel
        {
            Role = MessageRole.Assistant,
            Content = "会話履歴をクリアしました。新しい話題をどうぞ！",
            Timestamp = DateTime.Now
        });
        StatusText = "会話をリセットしました";
        RequestScrollToBottom?.Invoke();
    }

    [RelayCommand]
    private void OpenSettings()
    {
        RequestOpenSettings?.Invoke();
        UpdateProviderDisplay();
    }
}
