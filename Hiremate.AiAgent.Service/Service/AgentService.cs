using System.Net.Http.Headers;
using System.Text;
using Hiremate.AiAgent.Service.Interface;
using HireMate.Common;
using Microsoft.EntityFrameworkCore.Metadata;
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
        - Emphasize on the comparison between employees' skillSetDetails and jobDescription for selecting suitable interviewers (employees).

        You will take prompt from the user. The user will provide the job description and a list of employee. 
        
        Your output will be a string where only comma seperated employeeId will be there.
        The output must be comma seperated suitable employeeId.";

        private const string JobPostJson = @"
        {
          ""jobPostId"": 0,
          ""title"": """",
          ""description"": """",
          ""location"": """",
          ""requirements"": """",
          ""benefits"": """",
          ""companyName"": """",
          ""contactEmail"": """",
          ""postedDate"": ""2025-05-27T00:00:00"",
          ""expiryDate"": ""2025-05-27T00:00:00"",
          ""isActive"": false,
          ""processInfo"": ""Filling up the jobpost form. Please wait...""
        }";

        private const string JobPostBasePrompt = @"You are an ai assistant designed to provide ready-to-post job descriptions based on user propmt.
        Please keep going until the user’s query is completely resolved, before ending your turn and yielding back to the user. Only terminate your turn when you are sure that the Job Description is fully prepared.
        
        Your goal is to:
        - Prepare job description based on the title, experience, skillset requirements and other information provided by the user.
        - Ensure that the job description is comprehensive and ready to be posted.
        - update job description based on user feedback.

        The job description should include the following sections:
        - Title: The title of the job position.
        - Description: A detailed description of the job responsibilities and expectations.
        - Location: The location of the job.
        - Requirements: The skills and qualifications required for the job.
        - Benefits: The benefits offered by the company for this position.
        - Company Name: The name of the company offering the job.
        - Contact Email: The email address for job applications or inquiries.

        At the end of each interaction, check if the user's message indicates satisfaction with your response.
        The user is considered satisfied if their message contains any of the following phrases (case-insensitive) or similar phrases:
        - ""job post is ready""
        - ""job post is complete""
        - ""job post is good""
        - ""looks good""
        - ""Excellent""
        - ""Good""
        - ""fill up the form""
        - ""form fill up""
        - ""please fill up the form""
        - ""fill the text boxes""

        When the user is satisfied, you should provide the response in specific JSON format provided here.
        Here is the format:
        {
          ""jobPostId"": 0,
          ""title"": """",
          ""description"": """",
          ""location"": """",
          ""requirements"": """",
          ""benefits"": """",
          ""companyName"": """",
          ""contactEmail"": """",
          ""postedDate"": ""2025-05-27T00:00:00"",
          ""expiryDate"": ""2025-05-27T00:00:00"",
          ""isActive"": false,
          ""processInfo"": ""Filling up the jobpost form. Please wait...""
        }
            
        The JSON should not contain any additional text or any 'json' prefix.
        In the JSON, processInfo and jobPostId property default value is be fixed.
        Only output this JSON when the user is satisfied. At all other times, do not include or mention JSON.";

        private readonly string _apiKey = "";

        public AgentService(IOptions<AppSettings> options, IOptions<AiModel> aiModel)
        {
            _appSettings = options.Value;
            _aiModel = aiModel.Value;
            _apiKey = Environment.GetEnvironmentVariable("ApiKey");
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
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
                if (!string.IsNullOrWhiteSpace(assistantMessage))
                {
                    var ids = assistantMessage.Split(',');
                    foreach (var id in ids)
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

        public async Task<JobPostAssistantDto> JobPostAssistant(JobPostAssistantDto jobPostAssistantDto)
        {
            
            var messageList = new List<Message>();
            messageList.Add(new Message { role = "system", content = JobPostBasePrompt });
            //messageList.Add(new Message { role = "system", content = $"Json structure: {JobPostJson}" });
            foreach(var chat in jobPostAssistantDto.Conversation)
            {
                messageList.Add(new Message { role = chat.Role == 1?"assistant":"user", content = chat.Message });
            }
            
            
            //messageList.Add(new Message { role = "user", content = $"employees: {employees}" });

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost.com"); // Replace with your site or localhost
            client.DefaultRequestHeaders.Add("X-Title", "HireMate");

            var requestBody = new
            {
                model = _aiModel.Gpt,
                messages = messageList
            };
            var responsDto = new JobPostAssistantDto();
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(_appSettings.ApiBaseUrl, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseBody);
                string assistantMessage = responseData?.choices[0]?.message?.content;

                //JobPost job = JsonConvert.DeserializeObject<JobPost>(assistantMessage);

                jobPostAssistantDto.Conversation.Add(new Chat
                {
                    Role = 1,
                    Message = assistantMessage
                });

                return jobPostAssistantDto;
            }
            catch (Exception ex)
            {
                return jobPostAssistantDto;
            }

        }
        private async Task<float> CallLLM(string model, List<Message> messages)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
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
