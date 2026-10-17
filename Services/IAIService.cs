namespace AIKnowledgeAssistant.Services
{
    public interface IAIService
    {
        Task<string> GetAnswerAsync(string question);
    }
}
