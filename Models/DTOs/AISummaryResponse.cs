namespace AIKnowledgeAssistant.Models.DTOs;

public class AISummaryResponse
{
    public int OriginalWordCount { get; set; }
    public int SummaryWordCount { get; set; }
    public string Summary { get; set; } = string.Empty;
}
