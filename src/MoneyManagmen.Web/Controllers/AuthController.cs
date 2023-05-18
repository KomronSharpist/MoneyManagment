using Microsoft.AspNetCore.Mvc;
using MoneyManagmen.Web.Models;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Domain.Enums;
using MoneyManagment.Service.DTOs.Users;
using MoneyManagment.Service.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace MoneyManagmen.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService authService;
    private readonly IUserService userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        this.authService = authService;
        this.userService = userService;
    }

    public async Task<IActionResult> Index(UserLoginDto dto)
    {
        try
        {
            var user = await this.authService.AuthenticateAsync(dto);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(user);
            var userId = token.Claims.First(c => c.Type == "Id").Value;
            var role = token.Claims.First(c => c.Type == "role").Value;

            Response.Cookies.Append("userId", JsonConvert.SerializeObject(userId));
            Response.Cookies.Append("role", JsonConvert.SerializeObject(role));

            if (role == Convert.ToString(Roles.Admin))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "User");
        }
        catch
        {
            var model = new LoginAndCreateUserViewModel
            {
                LoginDto = new UserLoginDto(),
                CreateUserDto = new UserCreationDto(),
                WrongMessage = "Email or password is wrong"
            };
            return RedirectToActionPermanent("Index", "Home", model);
        }
    }
    public async Task<IActionResult> LogOut()
    {
        Response.Cookies.Append("userId", JsonConvert.SerializeObject(0));
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Register(UserCreationDto dto)
    {
        try
        {
            var user = await this.userService.AddAsync(dto);
      
            var newDto = new UserLoginDto()
            {
                Email = dto.Email,
                Password = dto.Password,
            };
            return RedirectToAction("Index", "Auth", newDto);
        }
        catch
        {
            var model = new LoginAndCreateUserViewModel
            {
                LoginDto = new UserLoginDto(),
                CreateUserDto = new UserCreationDto(),
                WrongMessage = "User is already exist."
            };
            return RedirectToActionPermanent("Index", "Home", model);
        }
    }
}
