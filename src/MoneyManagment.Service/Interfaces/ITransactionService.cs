﻿using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.Transactions;

namespace MoneyManagment.Service.Interfaces;

public interface ITransactionService
{
    ValueTask<bool> AddAsync(TransactionCreationDto dto, long userId);
    ValueTask<bool> UpdateAsync(TransactionUpdateDto dto);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<TransactionResultDto> RetrieveByIdAsync(long id);
    ValueTask<TransactionTotalResultDto> RetrieveMothlyByUserIdAsync(PaginationParams @params, long id);
    ValueTask<TransactionTotalResultDto> RetrieveMothlyByMeAsync(PaginationParams @params, long userId);
    ValueTask<TransactionTotalResultDto> RetrieveAllByMeAsync(PaginationParams @params, long userId);
    ValueTask<TransactionTotalResultDto> RetrieveAllByUserIdAsync(PaginationParams @params, long id);
    ValueTask<List<TransactionResultDto>> RetrieveAllAsync(PaginationParams @params);
}
