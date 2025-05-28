using System;
using Azure;
using HireMate.Common;
using HireMate.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Google.Apis.Requests.BatchRequest;

namespace HireMate.Controllers
{
    public class JobPostController : Controller
    {
        private readonly IJobService _jobService;

        public JobPostController(IJobService jobService)
        {
            _jobService = jobService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var jobPosts = await _jobService.GetAllActiveJobPostAsync();
            return View(jobPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpContext.Session.Clear();
            var sessionId = Guid.NewGuid().ToString();
            var conversation = new JobPostAssistantDto
            {
                Conversation = new List<Chat>()
            };
            HttpContext.Session.SetString(sessionId, JsonConvert.SerializeObject(conversation));
            ViewBag.SessionId = sessionId;
            JobPost jobPost = new();
            jobPost.PostedDate = DateTime.Now;
            jobPost.ExpiryDate = DateTime.Now.AddDays(7);             
            
            return View(jobPost);
        }


        [HttpPost]
        public async Task<IActionResult> Create(JobPost model)
        {
            if (ModelState.IsValid)
            {
                model.IsActive = model.PostedDate <= DateTime.Now;

                await _jobService.AddJobPostAsync(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChatRequest(string message, string sessionId)
        {
            var sessionObj = HttpContext.Session.GetString(sessionId);
            if (sessionObj != null)
            {
                var conversation = JsonConvert.DeserializeObject<JobPostAssistantDto>(sessionObj);
                conversation.Conversation.Add(new Chat { Role = 2, Message = message });
                var response = await _jobService.JobPostConversation(conversation);
                var chatList = new JobPostAssistantDto
                {
                    Conversation = new List<Chat>()
                };
                try
                {
                    var chatCount = response.Conversation.Count;
                    var jobPost = JsonConvert.DeserializeObject<JobPost>(response.Conversation[chatCount - 1].Message);
                    HttpContext.Session.SetString(sessionId + sessionId, JsonConvert.SerializeObject(jobPost));
                }
                catch (Exception)
                {


                }
                var adjustedChatList = adjustChatList(response);
                HttpContext.Session.SetString(sessionId, JsonConvert.SerializeObject(response));
                return Json(JsonConvert.SerializeObject(adjustedChatList));
            }


            return Json("");
        }

        [HttpGet]
        public async Task<IActionResult> GetJobPost(string sessionId)
        {
            var sessionObj = HttpContext.Session.GetString(sessionId+ sessionId);
            if (!string.IsNullOrWhiteSpace( sessionObj))
            {
                var jobPost = JsonConvert.DeserializeObject<JobPost>(sessionObj);
                HttpContext.Session.SetString(sessionId + sessionId, "");
                return Json(JsonConvert.SerializeObject(jobPost));
            }


            return Json("");
        }

        private async Task<JobPostAssistantDto> adjustChatList(JobPostAssistantDto jobPostAssistantDto)
        {
            var chatList = new JobPostAssistantDto
            {
                Conversation = new List<Chat>()
            };
            foreach (var chat in jobPostAssistantDto.Conversation)
            {
                try
                {
                    var jobPost = JsonConvert.DeserializeObject<JobPost>(chat.Message);
                    chatList.Conversation.Add(new Chat
                    {
                        Role = chat.Role,
                        Message = jobPost.ProcessInfo
                    });
                }
                catch (Exception)
                {

                    chatList.Conversation.Add(new Chat
                    {
                        Role = chat.Role,
                        Message = chat.Message
                    });
                }
            }
            return chatList;

        }
    }
}
