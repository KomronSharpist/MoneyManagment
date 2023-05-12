using Microsoft.AspNetCore.Mvc;

namespace MoneyManagmen.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
