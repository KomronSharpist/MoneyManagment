using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.TransactionCategory;

namespace MoneyManagment.Service.Interfaces;

public interface ITransactionCategoryService
{
    ValueTask<bool> AddAsync(TransactionCategoryCreationDto dto);
    ValueTask<bool> UpdateAsync(TransactionCategoryCreationDto dto);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<TransactionCategoryResultDto> RetrieveByIdAsync(long id);
    ValueTask<List<TransactionCategoryResultDto>> RetrieveAllAsync(PaginationParams @params);
}
