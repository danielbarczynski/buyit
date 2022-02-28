using Microsoft.AspNetCore.Mvc;

namespace buyitWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
