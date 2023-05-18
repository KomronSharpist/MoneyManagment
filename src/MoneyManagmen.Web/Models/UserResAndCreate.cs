using MoneyManagment.Service.DTOs.Users;

namespace MoneyManagmen.Web.Models
{
    public class UserResAndCreate
    {
        public UserResultDto Result { get; set; }
        public List<UserResultDto> Results { get; set; }
        public UserCreationDto Creation { get; set; }
    }
}
