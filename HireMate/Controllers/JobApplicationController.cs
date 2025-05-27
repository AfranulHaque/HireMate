using HireMate.Common;
using HireMate.Service;
using Microsoft.AspNetCore.Mvc;

namespace HireMate.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly IJob _jobService;

        public JobApplicationController(IJob jobService)
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
                string resumePath = string.Empty;
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
                    resumePath = filePath;
                }

                var applicantEntity = new DataManagement.Entities.Applicant
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ResumeFilePath = resumePath,
                    ApplicationDate = DateTime.UtcNow,
                    IsShortlisted = false,
                    IsHired = false,
                    JobPostId = model.JobPostId
                };

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
