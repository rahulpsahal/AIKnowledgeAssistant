using System.Net.Http.Json;
using System.Text.Json.Serialization;
using AIKnowledgeAssistant.Models.DTOs;

namespace AIKnowledgeAssistant.Services;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _modelName;

    public AIService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _modelName = configuration["Ollama:ModelName"] ?? "qwen2.5:3b";
    }

    public async Task<string> GetAnswerAsync(string question)
    {
        var request = new OllamaRequest
        {
            Model = _modelName,
            Prompt = question,
            Stream = false
        };

        var response = await _httpClient.PostAsJsonAsync("/api/generate", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.Response
            ?? throw new InvalidOperationException("Ollama returned an empty response.");
    }

    public async Task<AISummaryResponse> GetSummaryAsync(AISummaryRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var prompt = $"Summarize the following text in approximately {request.TargetWordCount} words. " +
                     $"Provide only the summary text without any additional introductory or concluding remarks:\n\n{request.Text}";

        var ollamaRequest = new OllamaRequest
        {
            Model = _modelName,
            Prompt = prompt,
            Stream = false
        };

        var response = await _httpClient.PostAsJsonAsync("/api/generate", ollamaRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
        var summaryText = result?.Response?.Trim()
            ?? throw new InvalidOperationException("Ollama returned an empty response.");

        return new AISummaryResponse
        {
            OriginalWordCount = CountWords(request.Text),
            SummaryWordCount = CountWords(summaryText),
            Summary = summaryText
        };
    }

    private static int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        return text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    private sealed class OllamaRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }

    private sealed class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = string.Empty;
    }
}
