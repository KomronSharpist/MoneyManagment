using MoneyManagment.Service.Commons.Attributes;

namespace MoneyManagment.Service.DTOs.Users;

public class UserChangePasswordDto
{
    public string OldPassword { get; set; }
    [StrongPassword]
    public string NewPassword { get; set; }
    public string VerifiedPassword { get; set; }
}
