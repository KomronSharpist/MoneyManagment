using MoneyManagment.Domain.Enums;

namespace MoneyManagment.Service.DTOs.Users;

public class UserResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ImagePath { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public Roles Role { get; set; }
    public bool IsVerify { get; set; } = false;
}
