using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManagment.Api.Models;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Service.DTOs.Transactions;
using MoneyManagment.Service.Interfaces;

namespace MoneyManagment.Api.Controllers;

public class TransactionController : BaseController
{
    //private readonly ITransactionService transactionService;

    //public TransactionController(ITransactionService transactionService)
    //{
    //    this.transactionService = transactionService;
    //}

    //[HttpPost]
    //[Authorize("allow")]
    //public async Task<IActionResult> Post(TransactionCreationDto dto)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.AddAsync(dto)
    //    });

    //[HttpPut]
    //[Authorize("allow")]
    //public async Task<IActionResult> Put(TransactionUpdateDto dto)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.UpdateAsync(dto)
    //    });

    //[HttpDelete("{id:long}")]
    //[Authorize("allow")]
    //public async Task<IActionResult> Delete(long id)
    //   => Ok(new Response
    //   {
    //       Code = 200,
    //       Error = "Success",
    //       Data = await this.transactionService.DeleteAsync(id)
    //   });

    //[HttpGet("by-id/{id:long}")]
    //[Authorize("admin")]
    //public async Task<IActionResult> GetById(long id)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.RetrieveByIdAsync(id)
    //    });

    //[HttpGet("list")]
    //[Authorize("admin")]
    //public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.RetrieveAllAsync(@params)
    //    });

    //[HttpGet("list-by-id")]
    //[Authorize("admin")]
    //public async Task<IActionResult> GetAllById([FromQuery] PaginationParams @params, long id)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.RetrieveAllByUserIdAsync(@params, id)
    //    });

    //[HttpGet("list-by-me")]
    //[Authorize("allow")]
    //public async Task<IActionResult> GetAllByMe([FromQuery] PaginationParams @params)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.RetrieveAllByMeAsync(@params)
    //    });

    //[HttpGet("monthly-by-me")]
    //[Authorize("allow")]
    //public async Task<IActionResult> GetMothlyByMe([FromQuery] PaginationParams @params)
    //    => Ok(new Response
    //    {
    //        Code = 200,
    //        Error = "Success",
    //        Data = await this.transactionService.RetrieveMothlyByMeAsync(@params)
    //    });

    //[HttpGet("monthly-by-id")]
    //[Authorize("admin")]
    //public async Task<IActionResult> GetMothlyById([FromQuery] PaginationParams @params, long id)
    //   => Ok(new Response
    //   {
    //       Code = 200,
    //       Error = "Success",
    //       Data = await this.transactionService.RetrieveMothlyByUserIdAsync(@params, id)
    //   });
}
