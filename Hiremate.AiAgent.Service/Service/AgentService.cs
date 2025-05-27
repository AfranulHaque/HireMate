using System.Net.Http.Headers;
using System.Text;
using Hiremate.AiAgent.Service.Interface;
using HireMate.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hiremate.AiAgent.Service.Service
{
    public class AgentService : IAgentService
    {
        private readonly AppSettings _appSettings;
        private readonly AiModel _aiModel;

        private const string CandidateEligableBasePrompt = @"You are an ai assistant designed to evaluate and sort CVs (resume) based on how well they match a given job description.
        Your goal is to:
        - Analyze the job description and understand the key requirements, including skills, experience, and qualification.
        - Review CV in details.
        - Score each CV from 1 to 10 based on how well it matches the job description.
        - Put highest weight on skills.

        You will take prompt from the user. The user will provide the job description and cv details. 
        
        Your output will be a number only which represents the score of the candidate. The output can be fractional number also. But it must be between 1 to 10.";


        private const string InterviewerEligableBasePrompt = @"You are an ai assistant designed to evaluate and sort employees based on how well their skill match a given job description.
        Your goal is to:
        - Analyze the job description and understand the key requirements, including skills, experience, and qualification.
        - Review skillSetDetails of employees.
        - To find the only suitable interviewer (employees) analyze jobDescription with employees skillSetDetails.

        You will take prompt from the user. The user will provide the job description and a list of employee. 
        
        Your output will be a string where only comma seperated employeeId will be there.
        The output must be comma seperated suitable employeeId.";
        //private const string CandidateEligableEndingPrompt = "";

        public AgentService(IOptions<AppSettings> options, IOptions<AiModel> aiModel)
        {
            _appSettings = options.Value;
            _aiModel = aiModel.Value;
        }

        public async Task<bool> IsCandidateEligable(string jobDescription, string candidateInfo)
        {
            var messageList = new List<Message>();
            messageList.Add(new Message { role = "system", content = CandidateEligableBasePrompt });
            messageList.Add(new Message { role = "user", content = $"job description: {jobDescription}" });
            messageList.Add(new Message { role = "user", content = $"candidate information: {candidateInfo}" });
            var gptScore = await CallLLM(_aiModel.Gpt, messageList);
            var claudeScore = await CallLLM(_aiModel.Claude, messageList);
            var geminiScore = await CallLLM(_aiModel.Gemini, messageList);
            int successLLMResponseCount = 0;
            float totalLLMScore = 0;
            if (gptScore > 0)
            {
                totalLLMScore += gptScore;
                successLLMResponseCount++;
            }
            if (claudeScore > 0)
            {
                totalLLMScore += claudeScore;
                successLLMResponseCount++;
            }
            if (geminiScore > 0)
            {
                totalLLMScore += geminiScore;
                successLLMResponseCount++;
            }
            return successLLMResponseCount == 0 ? false : (totalLLMScore / successLLMResponseCount) > 6;
        }

        public async Task<List<int>> GetSuitableInterviewers(string jobDescription, List<EmployeeSkill> employeeSkills)
        {
            var messageList = new List<Message>();
            messageList.Add(new Message { role = "system", content = InterviewerEligableBasePrompt });
            messageList.Add(new Message { role = "user", content = $"job description: {jobDescription}" });
            var employees = JsonConvert.SerializeObject(employeeSkills);
            messageList.Add(new Message { role = "user", content = $"employees: {employees}" });

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.ApiKey);
            client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost.com"); // Replace with your site or localhost
            client.DefaultRequestHeaders.Add("X-Title", "HireMate");

            var requestBody = new
            {
                model = _aiModel.Gemini,
                messages = messageList
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(_appSettings.ApiBaseUrl, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseBody);
                string assistantMessage = responseData?.choices[0]?.message?.content;
                var employeeIds = new List<int>();
                if(!string.IsNullOrWhiteSpace(assistantMessage))
                {
                    var ids = assistantMessage.Split(',');
                    foreach(var id in ids)
                    {
                        try
                        {
                            int empId = int.Parse(id);
                            employeeIds.Add(empId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
                return employeeIds;

            }
            catch (Exception ex)
            {
                return new List<int>();
            }

        }
        private async Task<float> CallLLM(string model, List<Message> messages)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.ApiKey);
            client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost.com"); // Replace with your site or localhost
            client.DefaultRequestHeaders.Add("X-Title", "HireMate");

            var requestBody = new
            {
                model,
                messages
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(_appSettings.ApiBaseUrl, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseBody);
                string assistantMessage = responseData?.choices[0]?.message?.content;
                string score = assistantMessage;
                try
                {
                    float result = float.Parse(score);
                    return result;
                }
                catch (FormatException)
                {
                    return 0;
                }
                catch (OverflowException)
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
