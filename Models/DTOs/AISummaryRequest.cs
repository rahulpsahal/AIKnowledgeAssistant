using System.ComponentModel.DataAnnotations;

namespace AIKnowledgeAssistant.Models.DTOs;

public class AISummaryRequest
{
    [Required]
    public string Text { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "TargetWordCount must be at least 1.")]
    public int TargetWordCount { get; set; }
}
