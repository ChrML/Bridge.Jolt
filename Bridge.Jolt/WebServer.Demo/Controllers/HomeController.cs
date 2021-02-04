using Microsoft.AspNetCore.Mvc;

namespace Bridge.Jolt.Demo.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
