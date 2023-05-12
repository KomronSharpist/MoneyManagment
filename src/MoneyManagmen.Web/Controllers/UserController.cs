using Microsoft.AspNetCore.Mvc;

namespace MoneyManagmen.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
