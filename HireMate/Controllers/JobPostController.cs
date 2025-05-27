using HireMate.Model.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HireMate.Controllers
{
    public class JobPostController : Controller
    {
        // GET: /JobPost
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            JobPost jobPost = new();
            return View();
        }

        // GET: /JobPost/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            JobPost jobPost = new();
            return View(jobPost);
        }

        // POST: /JobPost/Create
        //[HttpPost]
        //public async Task<IActionResult> Create()
        //{
        //    if (ModelState.IsValid)
        //    {
                
        //    }
        //    return View();
        //}
    }
}
