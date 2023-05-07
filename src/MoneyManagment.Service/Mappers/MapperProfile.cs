using AutoMapper;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Service.DTOs.TransactionCategory;
using MoneyManagment.Service.DTOs.Transactions;
using MoneyManagment.Service.DTOs.Users;

namespace MoneyManagment.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserResultDto>().ReverseMap();
        CreateMap<UserCreationDto, UserResultDto>().ReverseMap();
        CreateMap<UserCreationDto, User>().ReverseMap();

        CreateMap<Transaction, TransactionResultDto>().ReverseMap();
        CreateMap<Transaction, TransactionCreationDto>().ReverseMap();
        CreateMap<Transaction, TransactionUpdateDto>().ReverseMap();
        CreateMap<TransactionResultDto, TransactionUpdateDto>().ReverseMap();
        CreateMap<TransactionResultDto, TransactionCreationDto>().ReverseMap();

        CreateMap<TransactionCategory, TransactionCategoryResultDto>().ReverseMap();
        CreateMap<TransactionCategory, TransactionCategoryCreationDto>().ReverseMap();
        CreateMap<TransactionCategoryResultDto, TransactionCategoryCreationDto>().ReverseMap();
    }
}
