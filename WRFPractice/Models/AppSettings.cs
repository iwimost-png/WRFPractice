using System;
using System.IO;
using System.Text.Json;

namespace WRFPractice.Models;

public class AppSettings
{
    public AiProvider Provider { get; set; } = AiProvider.Mock;
    
    // OpenAI Settings
    public string OpenAiApiKey { get; set; } = string.Empty;
    public string OpenAiModel { get; set; } = "gpt-4o";
    public string OpenAiEndpoint { get; set; } = "https://api.openai.com/v1/chat/completions";

    // Gemini Settings
    public string GeminiApiKey { get; set; } = string.Empty;
    public string GeminiModel { get; set; } = "gemini-1.5-flash";

    // Ollama / Custom Settings
    public string CustomEndpoint { get; set; } = "http://localhost:11434/v1/chat/completions";
    public string CustomModel { get; set; } = "llama3";

    // Common
    public string SystemPrompt { get; set; } = "あなたは親切で有能なAIアシスタントです。ユーザーの質問にわかりやすく丁寧に応答してください。";
    public double Temperature { get; set; } = 0.7;

    private static readonly string SettingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WRFPractice",
        "settings.json"
    );

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsFilePath))
            {
                var json = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);
                if (settings != null) return settings;
            }
        }
        catch
        {
            // Fall back to default
        }
        return new AppSettings();
    }

    public void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
        catch
        {
            // Ignored in desktop local storage failure
        }
    }
}
