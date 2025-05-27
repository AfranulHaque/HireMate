using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiremate.AiAgent.Service.Interface
{
    public interface IAgentService
    {
        Task<bool> IsCandidateEligable(string jobDescription, string candidateInfo);
    }
}
