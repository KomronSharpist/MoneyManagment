using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.Transactions;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Api.Controllers;

public class TransactionController : BaseController
{
    private readonly ITransactionService transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    [HttpPost("create")]
    [Authorize("allow")]
    public async Task<IActionResult> Post(TransactionCreationDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.AddAsync(dto)
        });

    [HttpPut("update")]
    [Authorize("allow")]
    public async Task<IActionResult> Put(TransactionUpdateDto dto)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.UpdateAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    [Authorize("allow")]
    public async Task<IActionResult> Delete(long id)
       => Ok(new
       {
           Code = 200,
           Error = "Success",
           Data = await this.transactionService.DeleteAsync(id)
       });

    [HttpGet("get-by-id/{id:long}")]
    [Authorize("admin")]
    public async Task<IActionResult> GetById(long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-list")]
    [Authorize("admin")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.RetrieveAllAsync(@params)
        });

    [HttpGet("get-list-by-id")]
    [Authorize("admin")]
    public async Task<IActionResult> GetAllById([FromQuery] PaginationParams @params, long id)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.RetrieveAllByUserIdAsync(@params, id)
        });

    [HttpGet("get-list-by-me")]
    [Authorize("allow")]
    public async Task<IActionResult> GetAllByMe([FromQuery] PaginationParams @params)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.RetrieveAllByMeAsync(@params)
        });

    [HttpGet("get-mothly-by-me")]
    [Authorize("allow")]
    public async Task<IActionResult> GetMothlyByMe([FromQuery] PaginationParams @params)
        => Ok(new
        {
            Code = 200,
            Error = "Success",
            Data = await this.transactionService.RetrieveMothlyByMeAsync(@params)
        });

    [HttpGet("get-mothly-by-id")]
    [Authorize("admin")]
    public async Task<IActionResult> GetMothlyById([FromQuery] PaginationParams @params, long id)
       => Ok(new
       {
           Code = 200,
           Error = "Success",
           Data = await this.transactionService.RetrieveMothlyByUserIdAsync(@params, id)
       });
}
