using Microsoft.AspNetCore.Mvc;
using MoneyManagmen.Web.Models;
using MoneyManagment.Service.DTOs.Users;
using System.Diagnostics;

namespace MoneyManagmen.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new LoginAndCreateUserViewModel
            {
                LoginDto = new UserLoginDto(),
                CreateUserDto = new UserCreationDto()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}