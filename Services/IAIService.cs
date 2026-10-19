using AIKnowledgeAssistant.Models.DTOs;

namespace AIKnowledgeAssistant.Services
{
    public interface IAIService
    {
        Task<string> GetAnswerAsync(string question);
        Task<AISummaryResponse> GetSummaryAsync(AISummaryRequest request);
    }
}
