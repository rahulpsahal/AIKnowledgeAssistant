using AIKnowledgeAssistant.Models;
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
    public async Task<ActionResult<AIResponse>> Ask(
        [FromBody] AIRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.question))
        {
            return BadRequest("Question cannot be empty.");
        }

        var answer = await _aiService.GetAnswerAsync(
            request.question);

        return Ok(new AIResponse { answer = answer });
    }
    [HttpPost("summarize")]
    public async Task<ActionResult<>>
}