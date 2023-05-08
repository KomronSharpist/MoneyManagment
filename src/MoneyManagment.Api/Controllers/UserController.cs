using Microsoft.AspNetCore.Authorization;
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

    [HttpPut]
    [Authorize("allow")]
    public async Task<IActionResult> Put([FromForm] UserCreationDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.UpdateAsync(dto)
        });

    [HttpPut("change-password")]
    [Authorize("allow")]
    public async Task<IActionResult> ChangePassword(UserChangePasswordDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.ChangePasswordAsync(dto)
        });

    [HttpDelete("{id:long}")]
    [Authorize("allow")]
    public async Task<IActionResult> Delete(long id = 0)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.DeleteAsync(id)
        });

    [HttpGet("by-id/{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.RetrieveByIdAsync(id)
        });

    [HttpGet("by-email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByEmailAsync(string email)
       => Ok(new
       {
           Code = 200,
           Error = "Success",
           Data = await this.userService.RetrieveByEmailAsync(email)
       });

    [HttpGet("me")]
    [Authorize("allow")]
    public async Task<IActionResult> GetMe()
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.RetrieveMeAsync()
        });

    [HttpGet("list/")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.userService.RetrieveAllAsync(param)
        });
}
