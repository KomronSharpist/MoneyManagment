using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManagment.DAL.IRepositories;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Service.DTOs.Users;
using MoneyManagment.Service.Exceptions;
using MoneyManagment.Service.Extensions;
using MoneyManagment.Service.Helpers;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }



    public async ValueTask<bool> AddAsync(UserCreationDto dto)
    {
        var exist = await this.unitOfWork.Users.SelectAsync(u => u.Email.Equals(dto.Email));
        if (exist is not null)
            throw new MoneyException(405, "User is already exist");

        if (exist is not null && exist.IsDeleted && PasswordHelper.Verify(dto.Password, exist.Salt, exist.Password))
        {
            var mappedDto = this.mapper.Map(dto, exist);
            var hash = PasswordHelper.Hash(exist.Password);
            exist.IsDeleted = false;
            exist.UpdatedAt = DateTime.UtcNow;
            exist.UpdatedBy = HttpContextHelper.UserId;
            exist.Password = hash.passwordHash;
            exist.Salt = hash.salt;
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }

        var newDto = this.mapper.Map<User>(dto);
        var newHash = PasswordHelper.Hash(dto.Password);
        newDto.Password = newHash.passwordHash;
        newDto.Salt = newHash.salt;
        await this.unitOfWork.Users.InsertAsync(newDto);
        await this.unitOfWork.SaveChangesAsync();

        return true;
    }

    public async ValueTask<bool> ChangePasswordAsync(UserChangePasswordDto dto)
    {
        var exist = await this.unitOfWork.Users.SelectAsync(u => u.Id.Equals(HttpContextHelper.UserId));
        if (exist is null || exist.IsDeleted)
            throw new MoneyException(404, "Password or email is wrong");

        if (dto.NewPassword != dto.VerifiedPassword)
            throw new MoneyException(400, "New Password is not equal");

        if (!PasswordHelper.Verify(dto.OldPassword, exist.Salt, exist.Password))
            throw new MoneyException(400, "Password or email is wrong");

        var newPassword = PasswordHelper.Hash(dto.NewPassword);
        exist.Password = newPassword.passwordHash;
        exist.Salt = newPassword.salt;
        exist.UpdatedAt = DateTime.UtcNow;
        exist.UpdatedBy = HttpContextHelper.UserId;
        await this.unitOfWork.SaveChangesAsync();

        return true;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var exist = await this.unitOfWork.Users.SelectAsync(u => u.Id.Equals(id));
        if (exist is null || exist.IsDeleted)
            throw new MoneyException(400, "User is not found");

        exist.DeletedBy = HttpContextHelper.UserId;
        await this.unitOfWork.Users.DeleteAsync(u => u.Id.Equals(id));
        await this.unitOfWork.SaveChangesAsync();

        return true;
    }

    public async ValueTask<List<UserResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var user = await this.unitOfWork.Users.SelectAll()
            .Where(u => u.IsDeleted == false)
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<List<UserResultDto>>(user);
    }

    public async ValueTask<UserResultDto> RetrieveByEmailAsync(string email)
    {
        var user = await this.unitOfWork.Users.SelectAsync(u => u.Email.Equals(email));
        if (user is null || user.IsDeleted)
            throw new MoneyException(404, "User is not found");

        return this.mapper.Map<UserResultDto>(user);
    }

    public async ValueTask<UserResultDto> RetrieveByIdAsync(long id)
    {
        var user = await this.unitOfWork.Users.SelectAsync(u => u.Id.Equals(id));
        if (user is null || user.IsDeleted)
            throw new MoneyException(404, "User is not found");

        return this.mapper.Map<UserResultDto>(user);
    }

    public async ValueTask<UserResultDto> RetrieveMeAsync()
    {
        var user = await this.unitOfWork.Users.SelectAsync(u => u.Id.Equals(HttpContextHelper.UserId));
        if (user is null || user.IsDeleted)
            throw new MoneyException(404, "User is not found");

        return this.mapper.Map<UserResultDto>(user);
    }

    public async ValueTask<UserResultDto> UpdateAsync(UserCreationDto dto)
    {
        var exist = await this.unitOfWork.Users.SelectAsync(u => u.Email.Equals(dto.Email));
        if (exist is null || exist.IsDeleted)
            throw new MoneyException(404, "User is not found");

        exist.UpdatedAt = DateTime.UtcNow;
        exist.UpdatedBy = HttpContextHelper.UserId;
        this.mapper.Map(dto, exist);
        await this.unitOfWork.SaveChangesAsync();

        return this.mapper.Map<UserResultDto>(exist);
    }
}
