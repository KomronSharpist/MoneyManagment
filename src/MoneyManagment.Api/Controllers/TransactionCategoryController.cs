using Microsoft.AspNetCore.Mvc;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.TransactionCategory;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Api.Controllers;

public class TransactionCategoryController : BaseController
{
    private readonly ITransactionCategoryService transactionCategoryService;

    public TransactionCategoryController(ITransactionCategoryService transactionCategoryService)
    {
        this.transactionCategoryService = transactionCategoryService;
    }
    // Create, Update, Delete, RetrieveById , Retrieve ALl

    [HttpPost("create")]
    public async Task<IActionResult> Post(TransactionCategoryCreationDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> Put(TransactionCategoryCreationDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.UpdateAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.DeleteAsync(id)
        });

    [HttpGet("get-by-id/{id:long}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-list")]
    public async Task<IActionResult> GetAll([FromBody] PaginationParams @params)
       => Ok(new
       {
           Code = 200,
           Error = "Success",
           Data = await this.transactionCategoryService.RetrieveAllAsync(@params)
       });
}
