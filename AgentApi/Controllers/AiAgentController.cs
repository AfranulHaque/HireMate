using AgentApi.DTO;
using AgentApi.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiAgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AiAgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [Route("CheckPotentialCandidate")]
        [HttpPost]
        public async Task<IActionResult> CheckPotentialCandidate([FromBody] CandidateCheckRequest request)
        {
            
            var result = await _agentService.IsCandidateEligible(request);
            var response = new ApiResponse<bool>
            {
                Message = "successfully executed",
                Success = true,
                data = result
            };

            string jsonResponse = JsonConvert.SerializeObject(response);

            return Content(jsonResponse, "application/json");
        }
    }
}
