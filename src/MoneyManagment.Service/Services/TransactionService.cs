using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManagment.DAL.IRepositories;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Domain.Enums;
using MoneyManagment.Service.DTOs.Transactions;
using MoneyManagment.Service.Exceptions;
using MoneyManagment.Service.Extensions;
using MoneyManagment.Service.Helpers;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async ValueTask<bool> AddAsync(TransactionCreationDto dto, long userId)
    {
        if (dto.TransactionType != Domain.Enums.TransactionType.income && dto.TransactionType != Domain.Enums.TransactionType.outgo)
            throw new MoneyException(400, "Type is wrong: income = 0, outgo = 1");
        var mappedDto = this.mapper.Map<Transaction>(dto);
        mappedDto.UserId = userId;
        mappedDto.TransactionCategoryId = dto.CategoryId;
        await this.unitOfWork.Transactions.InsertAsync(mappedDto);
        await this.unitOfWork.SaveChangesAsync();

        return true;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var transaction = await this.unitOfWork.Transactions.SelectAsync(t => t.Id.Equals(id));
        if (transaction is null || transaction.IsDeleted)
            throw new MoneyException(404, "Transaction is not found");

        if (HttpContextHelper.UserRole == Convert.ToString(Roles.Admin))
            await this.unitOfWork.Transactions.DeleteAsync(t => t.Id.Equals(id));

        else if (HttpContextHelper.UserRole == Convert.ToString(Roles.User) && transaction.UserId == HttpContextHelper.UserId)
            await this.unitOfWork.Transactions.DeleteAsync(t => t.Id.Equals(id));
        else
            throw new MoneyException(403, "You don't have authorize for this");

        await this.unitOfWork.SaveChangesAsync();

        return true;
    }

    public async ValueTask<List<TransactionResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var transactions = await this.unitOfWork.Transactions.SelectAll()
            .Where(t => t.IsDeleted == false)
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<List<TransactionResultDto>>(transactions);
    }

    public async ValueTask<TransactionTotalResultDto> RetrieveAllByMeAsync(PaginationParams @params, long userId)
    {
        var transaction = await this.unitOfWork.Transactions.SelectAll()
            .Where(t => t.IsDeleted == false && t.UserId == userId)
            .ToPagedList(@params)
            .ToListAsync();

        var newDto = new TransactionTotalResultDto();
        foreach (var item in transaction)
        {
            if (item.TransactionType == Domain.Enums.TransactionType.income)
            {
                newDto.Income += item.Sum;
                newDto.Total += item.Sum;
            }

            else
            {
                newDto.Outgo += item.Sum;
                newDto.Total -= item.Sum;
            }
        }

        return newDto;
    }

    public async ValueTask<TransactionTotalResultDto> RetrieveAllByUserIdAsync(PaginationParams @params, long id)
    {
        var transaction = await this.unitOfWork.Transactions.SelectAll()
            .Where(t => t.IsDeleted == false && t.UserId == id)
            .ToPagedList(@params)
            .ToListAsync();

        var newDto = new TransactionTotalResultDto();
        foreach (var item in transaction)
        {
            if (item.TransactionType == Domain.Enums.TransactionType.income)
            {
                newDto.Income += item.Sum;
                newDto.Total += item.Sum;
            }

            else
            {
                newDto.Outgo += item.Sum;
                newDto.Total -= item.Sum;
            }
        }

        return newDto;
    }

    public async ValueTask<TransactionResultDto> RetrieveByIdAsync(long id)
    {
        var category = await this.unitOfWork.Transactions.SelectAsync(t => t.Id.Equals(id));
        if (category is null || category.IsDeleted)
            throw new MoneyException(404, "Categroy is not found");

        return this.mapper.Map<TransactionResultDto>(category);
    }


    public async ValueTask<TransactionTotalResultDto> RetrieveMothlyByMeAsync(PaginationParams @params, long userId)
    {
        var transaction = await this.unitOfWork.Transactions.SelectAll()
           .Where(t => t.IsDeleted == false &&
           t.UserId == userId &&
           t.CreatedAt.Year == DateTime.UtcNow.Year && t.CreatedAt.Month == DateTime.UtcNow.Month)
           .ToPagedList(@params)
           .ToListAsync();

        var newDto = new TransactionTotalResultDto();
        foreach (var item in transaction)
        {
            if (item.TransactionType == Domain.Enums.TransactionType.income)
            {
                newDto.Income += item.Sum;
                newDto.Total += item.Sum;
            }

            else
            {
                newDto.Outgo += item.Sum;
                newDto.Total -= item.Sum;
            }
        }

        return newDto;
    }

    public async ValueTask<TransactionTotalResultDto> RetrieveMothlyByUserIdAsync(PaginationParams @params, long id)
    {
        var transaction = await this.unitOfWork.Transactions.SelectAll()
           .Where(t => t.IsDeleted == false &&
           t.UserId == id &&
           t.CreatedAt.Year == DateTime.UtcNow.Year && t.CreatedAt.Month == DateTime.UtcNow.Month)
           .ToPagedList(@params)
           .ToListAsync();

        var newDto = new TransactionTotalResultDto();
        foreach (var item in transaction)
        {
            if (item.TransactionType == Domain.Enums.TransactionType.income)
            {
                newDto.Income += item.Sum;
                newDto.Total += item.Sum;
            }

            else
            {
                newDto.Outgo += item.Sum;
                newDto.Total -= item.Sum;
            }
        }

        return newDto;
    }

    public async ValueTask<bool> UpdateAsync(TransactionUpdateDto dto)
    {
        var transaction = await this.unitOfWork.Transactions.SelectAsync(t => t.Id.Equals(dto.Id));
        if (transaction is null || transaction.IsDeleted)
            throw new MoneyException(404, "Transaction is not found");

        if (HttpContextHelper.UserRole == Convert.ToString(Roles.Admin))
        {
            var mappedDto = this.mapper.Map(dto, transaction);
            mappedDto.UpdatedAt = DateTime.UtcNow;
        }
        else if (HttpContextHelper.UserRole == Convert.ToString(Roles.User) && HttpContextHelper.UserId == transaction.UserId)
        {
            var mappedDto = this.mapper.Map(dto, transaction);
            mappedDto.UpdatedAt = DateTime.UtcNow;
        }
        else
            throw new MoneyException(403, "You don't have authorize for this");
        await this.unitOfWork.SaveChangesAsync();

        return true;
    }
}
