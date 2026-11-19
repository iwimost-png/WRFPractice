using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using WRFPractice.Models;

namespace WRFPractice.Services;

public class OpenAiCompatibleService : IAiService
{
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(60) };

    public async Task<string> SendMessageAsync(
        IReadOnlyList<ChatMessage> history,
        AppSettings settings,
        CancellationToken cancellationToken = default)
    {
        var endpoint = settings.Provider == AiProvider.CustomOllama
            ? settings.CustomEndpoint
            : settings.OpenAiEndpoint;

        var model = settings.Provider == AiProvider.CustomOllama
            ? settings.CustomModel
            : settings.OpenAiModel;

        var apiKey = settings.Provider == AiProvider.CustomOllama
            ? "ollama" // Ollama does not require key
            : settings.OpenAiApiKey;

        if (settings.Provider == AiProvider.OpenAi && string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("OpenAI APIキーが設定されていません。右上の「設定」から入力してください。");
        }

        var messages = new List<object>();

        if (!string.IsNullOrWhiteSpace(settings.SystemPrompt))
        {
            messages.Add(new { role = "system", content = settings.SystemPrompt });
        }

        foreach (var msg in history.Where(m => !m.IsError))
        {
            var roleStr = msg.Role switch
            {
                MessageRole.User => "user",
                MessageRole.Assistant => "assistant",
                MessageRole.System => "system",
                _ => "user"
            };
            messages.Add(new { role = roleStr, content = msg.Content });
        }

        var requestBody = new
        {
            model = model,
            messages = messages,
            temperature = settings.Temperature
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        using var response = await HttpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"APIリクエストに失敗しました (Status: {response.StatusCode}):\n{responseContent}");
        }

        using var doc = JsonDocument.Parse(responseContent);
        var root = doc.RootElement;

        if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var firstChoice = choices[0];
            if (firstChoice.TryGetProperty("message", out var messageObj) &&
                messageObj.TryGetProperty("content", out var contentElem))
            {
                return contentElem.GetString() ?? string.Empty;
            }
        }

        return "応答メッセージのパースに失敗しました。";
    }
}
