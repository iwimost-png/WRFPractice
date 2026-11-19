using System;
using WRFPractice.Models;

namespace WRFPractice.Services;

public static class AiServiceFactory
{
    private static readonly MockAiService MockService = new();
    private static readonly OpenAiCompatibleService OpenAiService = new();
    private static readonly GeminiAiService GeminiService = new();

    public static IAiService GetService(AiProvider provider)
    {
        return provider switch
        {
            AiProvider.Mock => MockService,
            AiProvider.OpenAi => OpenAiService,
            AiProvider.Gemini => GeminiService,
            AiProvider.CustomOllama => OpenAiService,
            _ => MockService
        };
    }
}
