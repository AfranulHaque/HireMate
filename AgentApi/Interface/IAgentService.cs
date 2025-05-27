using AgentApi.DTO;

namespace AgentApi.Interface
{
    public interface IAgentService
    {
        Task<bool> IsCandidateEligible(CandidateCheckRequest request);
    }
}
