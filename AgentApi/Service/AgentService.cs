using AgentApi.DTO;
using AgentApi.Interface;

namespace AgentApi.Service
{
    public class AgentService : IAgentService
    {
        private readonly string apiKey;
        private readonly string apiBaseUrl;

        public AgentService(IConfiguration configuration)
        {
            apiKey = configuration["Config:apiKey"];
            apiBaseUrl = configuration["Config:apiBaseUrl"];
        }

        public async Task<bool> IsCandidateEligible(CandidateCheckRequest request)
        {
            // LLM call will be here
            return true;
        }
    }
}
