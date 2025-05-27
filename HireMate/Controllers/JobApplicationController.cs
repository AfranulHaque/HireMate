using Microsoft.AspNetCore.Mvc;

namespace HireMate.Controllers
{
    public class JobApplicationController : Controller
    {
        public IActionResult Apply()
        {
            return View();
        }
    }
}
