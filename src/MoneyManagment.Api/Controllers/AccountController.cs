
using Microsoft.AspNetCore.Mvc;
using MoneyManagment.Api.Models;
using MoneyManagment.Service.DTOs.Users;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Api.Controllers;

[Route("api/account")]
public class AccountController : BaseController
{
    private readonly IAuthService authService;
    private readonly IUserService userService;
    public AccountController(IAuthService authService, IUserService userService)
    {
        this.authService = authService;
        this.userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
         => Ok(new Response
         {
             Code = 200,
             Error = "Success",
             Data = await this.authService.AuthenticateAsync(dto)
         });

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserCreationDto dto)
    => Ok(new Response
    {
        Code = 200,
        Error = "Success",
        Data = await this.userService.AddAsync(dto)
    });
}