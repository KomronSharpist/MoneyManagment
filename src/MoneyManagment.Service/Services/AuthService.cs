using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Service.DTOs.Users;
using MoneyManagment.Service.Exceptions;
using MoneyManagment.Service.Helpers;
using MoneyManagment.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyManagment.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserService userService;
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;

    public AuthService(IUserService userService, IConfiguration configuration, IMapper mapper)
    {
        this.userService = userService;
        this.configuration = configuration;
        this.mapper = mapper;
    }

    public async Task<string> AuthenticateAsync(UserLoginDto dto)
    {
        var user = await this.userService.RetrieveByEmailAsync(dto.Email);
        if (user == null || !PasswordHelper.Verify(dto.Password, user.Salt, user.Password))
            throw new MoneyException(400, "Email or password is incorrect");


        return GenerateToken(this.mapper.Map<User>(user));
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
             new Claim("Id", user.Id.ToString()),
             new Claim(ClaimTypes.Role, user.Role.ToString()),
             new Claim(ClaimTypes.Name, user.FirstName)
            }),
            Audience = configuration["JWT:Audience"],
            Issuer = configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["JWT:Expire"])),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
