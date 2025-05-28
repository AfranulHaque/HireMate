using HireMate.Common;
using HireMate.Service;
using Microsoft.AspNetCore.Mvc;

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
    }
}
