using MoneyManagment.Domain.Commons;
using MoneyManagment.Domain.Enums;

namespace MoneyManagment.Domain.Entities;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ImagePath { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public Roles Role { get; set; }
    public bool IsVerify { get; set; } = false;
    public ICollection<Transaction> Transactions { get; set; }
}