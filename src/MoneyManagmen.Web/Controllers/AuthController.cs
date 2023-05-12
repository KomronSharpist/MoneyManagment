using Microsoft.AspNetCore.Mvc;
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
        var user = await this.authService.AuthenticateAsync(dto);
        if (user == null)
            return RedirectToAction("Index","Home");

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(user);
        var userId = token.Claims.First(c => c.Type == "Id").Value;
        var role = token.Claims.First(c => c.Type == "role").Value;

        Response.Cookies.Append("userId", JsonConvert.SerializeObject(userId));
        
        if(role == Convert.ToString(Roles.Admin))
            return RedirectToAction("Index","Admin");
        
        return RedirectToAction("Index","User");
    }

    public async Task<IActionResult> Register(UserCreationDto dto, IFormFile image)
    {
        var user = await this.userService.AddAsync(dto);
        if (!user)
            return RedirectToAction("Index", "Home", user);

        var newDto = new UserLoginDto()
        {
            Email = dto.Email,
            Password = dto.Password,
        };
        return RedirectToAction("Index", "Auth", newDto);
    }
}
