using HireMate.Common;
using HireMate.Service;
using Microsoft.AspNetCore.Mvc;

namespace HireMate.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly IJobService _jobService;

        public JobApplicationController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(Applicant model, IFormFile resumeFile)
        {
            if (ModelState.IsValid)
            {
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    var uploadsFolder = "C:\\Uploads";
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = model.FirstName + "_" + Guid.NewGuid()
                        + Path.GetExtension(resumeFile.FileName);

                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resumeFile.CopyToAsync(stream);
                    }

                    model.ResumeFilePath = filePath;
                } 

                await _jobService.ApplyForJobAsync(model);

                return RedirectToAction("Index", "JobPost");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ApplyConfirmation()
        {
            return View();
        }
    }
}
