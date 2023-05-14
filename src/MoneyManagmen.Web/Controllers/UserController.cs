using Microsoft.AspNetCore.Mvc;
using MoneyManagmen.Web.Models;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Enums;
using MoneyManagment.Service.DTOs.Transactions;
using MoneyManagment.Service.Interfaces;
using Newtonsoft.Json;

namespace MoneyManagmen.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService userService;
    private readonly ITransactionService transactionService;

    public UserController(IUserService userService, ITransactionService transactionService)
    {
        this.userService = userService;
        this.transactionService = transactionService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var json = Request.Cookies["userId"];
            long userId = JsonConvert.DeserializeObject<long>(json);

            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var pagination = new PaginationParams()
            {
                PageIndex = 1,
                PageSize = 100
            };
            var result = await this.transactionService.RetrieveAllByUserIdAsync(pagination,userId);
            var secondResult = await this.transactionService.RetrieveAllAsync(pagination);
            var newList = new List<TransactionResultDto>();
            foreach (var item in secondResult)
                if(item.UserId == userId)
                    newList.Add(item);
            var user = await this.userService.RetrieveByIdAsync(userId);
            var res = new TotalAndTransaction()
            {
                TotalResultDto = result,
                TransactionResults = newList,
            };
            if (user.Role == Roles.User)
                return View(res);

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            return RedirectToAction("Index", "Home");
        }
    }
    public async Task<IActionResult> Settings()
    {
        try
        {
            var id = Request.Cookies["userId"];
            var userId = JsonConvert.DeserializeObject<long>(id);
            var user = await this.userService.RetrieveByIdAsync(userId);
            if (user.Role == Roles.User)
                return View(user);

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            return RedirectToAction("Index", "Home");
        }
    }
    public async Task<IActionResult> History()
    {
        var json = Request.Cookies["userId"];
        var id = JsonConvert.DeserializeObject<long>(json);
        var user = await this.userService.RetrieveByIdAsync(id);
        var result = new List<TransactionResultDto>();
        if (user.Role == Roles.User)
        {
            var pagination = new PaginationParams()
            {
                PageIndex = 1,
                PageSize = 100
            };
            var transactions = await this.transactionService.RetrieveAllAsync(pagination);
            foreach(var transaction in transactions)
                if(transaction.UserId == user.Id)
                    result.Add(transaction);
            return View(result);
        }

        return RedirectToAction("Index", "Home");
    }
}
