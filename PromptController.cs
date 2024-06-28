using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PromptEngineeringDemo
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromptController : ControllerBase
    {
        private readonly IPromptService _promptService;

        public PromptController(IPromptService promptService)
        {
            _promptService = promptService; 
        }

        [HttpGet("TriggerOpenAI")]
        public async Task<IActionResult> TriggerOpenAI([FromQuery] string prompt)
        {
            var res = await _promptService.TriggerOpenAI(prompt);
            return Ok(res);
        }
    }
}
