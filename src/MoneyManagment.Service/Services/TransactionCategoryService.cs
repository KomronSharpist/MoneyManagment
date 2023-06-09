﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManagment.DAL.IRepositories;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Service.DTOs.TransactionCategory;
using MoneyManagment.Service.Exceptions;
using MoneyManagment.Service.Extensions;
using MoneyManagment.Service.Helpers;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Service.Services
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public TransactionCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async ValueTask<bool> AddAsync(TransactionCategoryCreationDto dto)
        {
            if (dto.TransactionType != Domain.Enums.TransactionType.income && dto.TransactionType != Domain.Enums.TransactionType.outgo)
                throw new MoneyException(400, "Type is wrong: income = 0, outgo = 1");
            var category = await this.unitOfWork.TransactionCategories.SelectAsync(c => c.Name.Equals(dto.Name));
            if(category is not null && category.IsDeleted)
            {
                category.IsDeleted = false;
                category.TransactionType = dto.TransactionType;
            }
            if (category is not null && !category.IsDeleted)
                throw new MoneyException(405, "Category is already exist");

            var mappedDto = this.mapper.Map<TransactionCategory>(dto);
            await this.unitOfWork.TransactionCategories.InsertAsync(mappedDto);
            await this.unitOfWork.SaveChangesAsync();

            return true;
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            var category = await this.unitOfWork.TransactionCategories.SelectAsync(c => c.Id.Equals(id));
            if (category is null || category.IsDeleted)
                throw new MoneyException(404, "Category is not found");

            var mappedDto = this.mapper.Map<TransactionCategory>(category);
            await this.unitOfWork.TransactionCategories.DeleteAsync(c => c.Id.Equals(mappedDto.Id));
            await this.unitOfWork.SaveChangesAsync();

            return true;
        }

        public async ValueTask<List<TransactionCategoryResultDto>> RetrieveAllAsync(PaginationParams @params)
        {
            var categories = await this.unitOfWork.TransactionCategories.SelectAll()
                .Where(u => u.IsDeleted == false)
                .ToPagedList(@params)
                .ToListAsync();

            return this.mapper.Map<List<TransactionCategoryResultDto>>(categories);
        }

        public async ValueTask<TransactionCategoryResultDto> RetrieveByIdAsync(long id)
        {
            var category = await this.unitOfWork.TransactionCategories.SelectAsync(c => c.Id.Equals(id));
            if (category is null || category.IsDeleted)
                throw new MoneyException(404, "Category is not found");

            return this.mapper.Map<TransactionCategoryResultDto>(category);
        }

        public async ValueTask<TransactionCategoryResultDto> RetrieveByNameAsync(string name)
        {
            var category = await this.unitOfWork.TransactionCategories.SelectAsync(c => c.Name.Equals(name));
            if (category is null || category.IsDeleted)
                throw new MoneyException(404, "Category is not found");

            return this.mapper.Map<TransactionCategoryResultDto>(category);
        }

        public async ValueTask<bool> UpdateAsync(TransactionCategoryCreationDto dto, long id)
        {
            var category = await this.unitOfWork.TransactionCategories.SelectAsync(c => c.Id.Equals(id));
            if (category is null || category.IsDeleted)
                throw new MoneyException(404, "Category is not found");

            var mappedDto = this.mapper.Map(dto, category);
            mappedDto.UpdatedAt = DateTime.UtcNow;

            await this.unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
