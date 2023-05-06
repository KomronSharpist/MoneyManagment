using MoneyManagment.Service.DTOs.Users;

namespace MoneyManagment.Service.Interfaces;

public interface IAuthService
{
    Task<string> AuthenticateAsync(UserLoginDto dto);
}
