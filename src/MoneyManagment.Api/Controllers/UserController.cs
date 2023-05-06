using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.Users;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Api.Controllers;

public class UserController : BaseController
{
    private readonly IUserService userService;
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPut("update")]
    public async Task<IActionResult> Put(UserCreationDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.UpdateAsync(dto)
        });

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(UserChangePasswordDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.ChangePasswordAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.DeleteAsync(id)
        });

    [HttpGet("get-by-id/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-by-email/{email}")]
    public async Task<IActionResult> GetByEmailAsync(string email)
       => Ok(new
       {
           Code = 200,
           Error = "Success",
           Data = await this.userService.RetrieveByEmailAsync(email)
       });

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.RetrieveMeAsync()
        });

    [HttpGet("get-list/{params}")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.RetrieveAllAsync(@params)
        });
}
