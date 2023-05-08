using Microsoft.AspNetCore.Http;
using MoneyManagment.Service.Commons.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoneyManagment.Service.DTOs.Users;

public class UserCreationDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Email]
    public string Email { get; set; }
    public IFormFile Image { get; set; }

    [StrongPassword]
    public string Password { get; set; }
}
