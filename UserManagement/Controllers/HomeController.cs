using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        //Authorize attribute'ü ile giriş yapılmadan bu sayfaya erişimi engelledik.
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
