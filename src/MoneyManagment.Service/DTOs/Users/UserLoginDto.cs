using System.ComponentModel.DataAnnotations;

namespace MoneyManagment.Service.DTOs.Users;

public class UserLoginDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
