using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AIKnowledgeAssistant.Services;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;

    public AIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAnswerAsync(string question)
    {
        var request = new OllamaRequest
        {
            Model = "qwen2.5:3b",
            Prompt = question,
            Stream = false
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/generate", request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<OllamaResponse>();

        return result?.Response
            ?? throw new InvalidOperationException(
                "Ollama returned an empty response.");
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