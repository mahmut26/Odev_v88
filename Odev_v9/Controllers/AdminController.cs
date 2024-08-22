using Microsoft.AspNetCore.Mvc;

namespace Odev_v9.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
