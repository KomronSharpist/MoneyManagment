using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.Users;

namespace MoneyManagment.Service.Interfaces;

public interface IUserService
{
    ValueTask<bool> AddAsync(UserCreationDto dto);
    ValueTask<UserResultDto> UpdateAsync(UserCreationDto dto);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<UserResultDto> RetrieveMeAsync();
    ValueTask<UserResultDto> RetrieveByIdAsync(long id);
    ValueTask<UserResultDto> RetrieveByEmailAsync(string email);
    ValueTask<List<UserResultDto>> RetrieveAllAsync(PaginationParams @params);
    ValueTask<bool> ChangePasswordAsync(UserChangePasswordDto dto);
}
