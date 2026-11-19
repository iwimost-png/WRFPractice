using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WRFPractice.Models;

namespace WRFPractice.Services;

public class MockAiService : IAiService
{
    public async Task<string> SendMessageAsync(
        IReadOnlyList<ChatMessage> history,
        AppSettings settings,
        CancellationToken cancellationToken = default)
    {
        // リアルなAI応答待ち時間をシミュレート（1.2秒）
        await Task.Delay(1200, cancellationToken);

        var lastMessage = history.LastOrDefault(m => m.Role == MessageRole.User)?.Content ?? "";
        var lower = lastMessage.ToLowerInvariant();

        if (lower.Contains("こんにちは") || lower.Contains("hello"))
        {
            return "こんにちは！私はWPFチャットアプリのデモAIアシスタントです。😊\n何か質問や、お手伝いできることはありますか？";
        }

        if (lower.Contains("c#") || lower.Contains("wpf") || lower.Contains("コード") || lower.Contains("mvvm"))
        {
            return "WPFとC#に関するご質問ですね！\n\n" +
                   "WPFでは**MVVM（Model-View-ViewModel）パターン**を使うことで、UIとビジネスロジックを綺麗に分離できます。\n\n" +
                   "```csharp\n" +
                   "// CommunityToolkit.Mvvm を使ったプロパティ定義の例\n" +
                   "[ObservableProperty]\n" +
                   "private string _messageText = string.Empty;\n" +
                   "\n" +
                   "[RelayCommand]\n" +
                   "private async Task SendMessageAsync()\n" +
                   "{\n" +
                   "    // メッセージ送信ロジック\n" +
                   "}\n" +
                   "```\n\n" +
                   "画面右上の「⚙ 設定」から OpenAI や Gemini の API キーを登録すると、本物のAIモデルとリアルタイムで対話できます！";
        }

        if (lower.Contains("ありがとう") || lower.Contains("thanks"))
        {
            return "どういたしまして！お役に立てて嬉しいです。他にも気になることがあれば、いつでも気軽に聞いてくださいね。✨";
        }

        return $"「{lastMessage}」についてですね！\n\n" +
               "（※ 現在はモックモードで動作しています）\n\n" +
               "以下の設定を行うことで、本物の大規模言語モデルと接続できます：\n" +
               "1. 右上の「⚙ 設定」ボタンをクリック\n" +
               "2. プロバイダー（OpenAI / Gemini / Ollama等）を選択\n" +
               "3. お手持ちのAPIキーを入力して「保存」\n\n" +
               "設定完了後、このチャット画面から直接AIと高度なやりとりができるようになります！";
    }
}
