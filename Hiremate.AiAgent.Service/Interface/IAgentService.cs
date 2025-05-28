using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireMate.Common;

namespace Hiremate.AiAgent.Service.Interface
{
    public interface IAgentService
    {
        Task<bool> IsCandidateEligable(string jobDescription, string candidateInfo);
        Task<List<int>> GetSuitableInterviewers(string jobDescription, List<EmployeeSkill> employeeSkills);
        Task<JobPostAssistantDto> JobPostAssistant(JobPostAssistantDto jobPostAssistantDto);
    }
}
