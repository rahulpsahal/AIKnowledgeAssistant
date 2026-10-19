using AIKnowledgeAssistant.Models.DTOs;
using AIKnowledgeAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIKnowledgeAssistant.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly IAIService _aiService;

    public AIController(IAIService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<AIResponse>> Ask([FromBody] AIRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Question))
        {
            return BadRequest("Question cannot be empty.");
        }

        var answer = await _aiService.GetAnswerAsync(request.Question);

        return Ok(new AIResponse { Answer = answer });
    }

    [HttpPost("summarize")]
    public async Task<ActionResult<AISummaryResponse>> Summarize([FromBody] AISummaryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Text))
        {
            return BadRequest("Text to summarize cannot be empty.");
        }

        if (request.TargetWordCount <= 0)
        {
            return BadRequest("Target word count must be greater than zero.");
        }

        var summary = await _aiService.GetSummaryAsync(request);
        return Ok(summary);
    }
}
