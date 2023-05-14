using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManagment.DAL.IRepositories;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Domain.Enums;
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
            var hash = PasswordHelper.Hash(exist.Password);
            exist.IsDeleted = false;
            exist.UpdatedAt = DateTime.UtcNow;
            exist.UpdatedBy = HttpContextHelper.UserId;
            exist.Password = hash.passwordHash;
            exist.Salt = hash.salt;
            var mappedDto = this.mapper.Map(dto, exist);
            if (dto.Image is null)
                mappedDto.ImagePath = exist.ImagePath;
            else
            {
                byte[] image = dto.Image.ToByteArray();
                var fileExtension = Path.GetExtension(dto.Image.FileName);
                var fileName = Guid.NewGuid().ToString("N") + fileExtension;
                var webRootPath = EnvironmentHelper.WebHostPath;
                var folder = Path.Combine("wwwroot", "uploads", "images");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fullPath = Path.Combine(folder, fileName);
                using var imageStream = new MemoryStream(image);

                using var imagePath = new FileStream(fullPath, FileMode.CreateNew);
                imageStream.WriteTo(imagePath);

                mappedDto.ImagePath = fullPath;
            }

            await this.unitOfWork.SaveChangesAsync();
            return true;
        }

        byte[] image2 = dto.Image.ToByteArray();
        var fileExtension2 = Path.GetExtension(dto.Image.FileName);
        var fileName2 = Guid.NewGuid().ToString("N") + fileExtension2;
        var webRootPath2 = EnvironmentHelper.WebHostPath;
        var folder2 = Path.Combine("wwwroot", "uploads", "images");

        if (!Directory.Exists(folder2))
            Directory.CreateDirectory(folder2);

        var fullPath2 = Path.Combine(folder2, fileName2);
        using var imageStream2 = new MemoryStream(image2);

        using var imagePath2 = new FileStream(fullPath2, FileMode.CreateNew);
        imageStream2.WriteTo(imagePath2);


        var newDto = this.mapper.Map<User>(dto);
        var newHash = PasswordHelper.Hash(dto.Password);
        newDto.Password = newHash.passwordHash;
        newDto.Salt = newHash.salt;
        newDto.ImagePath = fullPath2;
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
        if (id == 0)
        {
            await this.unitOfWork.Users.DeleteAsync(u => u.Id.Equals(HttpContextHelper.UserId));
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }
        else
        {
            var exist = await this.unitOfWork.Users.SelectAsync(u => u.Id.Equals(id));
            if (exist is null || exist.IsDeleted)
                throw new MoneyException(400, "User is not found");

            if (HttpContextHelper.UserRole == Convert.ToString(Roles.Admin) || exist.Id == HttpContextHelper.UserId)
            {
                exist.DeletedBy = HttpContextHelper.UserId;
                await this.unitOfWork.Users.DeleteAsync(u => u.Id.Equals(id));
                await this.unitOfWork.SaveChangesAsync();
            }
            else
                throw new MoneyException(403, "You can not delete, you don't have authorize");
        }

        return true;
    }

    public async ValueTask<List<UserResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var user = await this.unitOfWork.Users.SelectAll()
            .Where(u => u.IsDeleted == false)
            .ToPagedList(@params)
            .ToListAsync();

        var mappedDto = this.mapper.Map<List<UserResultDto>>(user);
        return mappedDto;
    }

    public async ValueTask<UserResultDto> RetrieveByEmailAsync(string email)
    {
        var user = await this.unitOfWork.Users.SelectAsync(u => u.Email.Equals(email));
        if (user is null || user.IsDeleted)
            throw new MoneyException(400, "Email or password is incorrect");

        var mappedDto = this.mapper.Map<UserResultDto>(user);
        return mappedDto;
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
            throw new MoneyException(401, "Unauthorized");

        return this.mapper.Map<UserResultDto>(user);
    }

    public async ValueTask<UserResultDto> UpdateAsync(UserCreationDto dto)
    {
        var exist = await this.unitOfWork.Users.SelectAsync(u => u.Email.Equals(dto.Email));
        if (exist is null || exist.IsDeleted)
            throw new MoneyException(404, "User is not found with this email");

        if (!PasswordHelper.Verify(dto.Password, exist.Salt, exist.Password))
            throw new MoneyException(401, "Your password is wrong for update your profile");

        var newDto = this.mapper.Map(dto, exist);
        var newPass = PasswordHelper.Hash(dto.Password);
        newDto.Password = newPass.passwordHash;
        newDto.Salt = newPass.salt;
        newDto.UpdatedAt = DateTime.UtcNow;
        newDto.UpdatedBy = HttpContextHelper.UserId;

        await this.unitOfWork.SaveChangesAsync();

        return this.mapper.Map<UserResultDto>(exist);
    }
}
