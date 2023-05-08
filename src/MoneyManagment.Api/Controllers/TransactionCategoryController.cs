using Microsoft.AspNetCore.Authorization;
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

    [HttpPost]
    [Authorize("admin")]
    public async Task<IActionResult> Post(TransactionCategoryCreationDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.AddAsync(dto)
        });

    [HttpPut]
    [Authorize("admin")]
    public async Task<IActionResult> Put([FromBody] TransactionCategoryCreationDto dto, long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.UpdateAsync(dto, id)
        });

    [HttpDelete("{id:long}")]
    [Authorize("admin")]
    public async Task<IActionResult> Delete(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.DeleteAsync(id)
        });

    [HttpGet("by-id/{id:long}")]
    [Authorize("allow")]
    public async Task<IActionResult> GetById(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionCategoryService.RetrieveByIdAsync(id)
        });

    [HttpGet("list")]
    [Authorize("allow")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
       => Ok(new
       {
           Code = 200,
           Error = "Success",
           Data = await this.transactionCategoryService.RetrieveAllAsync(@params)
       });
}
