using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WRFPractice.Models;

namespace WRFPractice.Services;

public class GeminiAiService : IAiService
{
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(60) };

    public async Task<string> SendMessageAsync(
        IReadOnlyList<ChatMessage> history,
        AppSettings settings,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(settings.GeminiApiKey))
        {
            throw new InvalidOperationException("Gemini APIキーが設定されていません。右上の「設定」から入力してください。");
        }

        var model = string.IsNullOrWhiteSpace(settings.GeminiModel) ? "gemini-1.5-flash" : settings.GeminiModel;
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={settings.GeminiApiKey}";

        var contents = new List<object>();

        foreach (var msg in history.Where(m => !m.IsError))
        {
            var roleStr = msg.Role == MessageRole.Assistant ? "model" : "user";
            contents.Add(new
            {
                role = roleStr,
                parts = new object[]
                {
                    new { text = msg.Content }
                }
            });
        }

        object requestBody;
        if (!string.IsNullOrWhiteSpace(settings.SystemPrompt))
        {
            requestBody = new
            {
                systemInstruction = new
                {
                    parts = new object[] { new { text = settings.SystemPrompt } }
                },
                contents = contents,
                generationConfig = new
                {
                    temperature = settings.Temperature
                }
            };
        }
        else
        {
            requestBody = new
            {
                contents = contents,
                generationConfig = new
                {
                    temperature = settings.Temperature
                }
            };
        }

        var json = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await HttpClient.PostAsync(url, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Gemini APIリクエストに失敗しました (Status: {response.StatusCode}):\n{responseContent}");
        }

        using var doc = JsonDocument.Parse(responseContent);
        var root = doc.RootElement;

        if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
        {
            var firstCandidate = candidates[0];
            if (firstCandidate.TryGetProperty("content", out var contentObj) &&
                contentObj.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
            {
                var textParts = new List<string>();
                foreach (var part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("text", out var textElem))
                    {
                        var t = textElem.GetString();
                        if (!string.IsNullOrEmpty(t)) textParts.Add(t);
                    }
                }
                return string.Join("\n", textParts);
            }
        }

        return "Geminiからの応答テキストが見つかりませんでした。";
    }
}
