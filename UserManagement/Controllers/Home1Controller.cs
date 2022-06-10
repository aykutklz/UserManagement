using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    public class Home1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
