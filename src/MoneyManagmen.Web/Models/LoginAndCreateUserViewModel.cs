using MoneyManagment.Service.DTOs.Users;

namespace MoneyManagmen.Web.Models;

public class LoginAndCreateUserViewModel
{
    public UserLoginDto LoginDto { get; set; }
    public UserCreationDto CreateUserDto { get; set; }
}
