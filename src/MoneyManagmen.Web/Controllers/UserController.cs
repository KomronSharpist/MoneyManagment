using Microsoft.AspNetCore.Mvc;
using MoneyManagmen.Web.Models;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Domain.Enums;
using MoneyManagment.Service.DTOs.Transactions;
using MoneyManagment.Service.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace MoneyManagmen.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService userService;
    private readonly ITransactionService transactionService;
    private readonly ITransactionCategoryService transactionCategoryService;

    public UserController(IUserService userService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService)
    {
        this.userService = userService;
        this.transactionService = transactionService;
        this.transactionCategoryService = transactionCategoryService;
    }

    public async Task<IActionResult> Index()
    {
        if (TempData.TryGetValue("Data", out var serializedData) && serializedData is string dataString)
        {
            var totalAndTransaction = JsonConvert.DeserializeObject<TotalAndTransaction>(dataString);
            return View(totalAndTransaction);
        }
        else
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
                var thirdResult = await this.transactionCategoryService.RetrieveAllAsync(pagination);
                var newList = new List<TransactionResultDto>();
                foreach (var item in secondResult)
                    if(item.UserId == userId)
                        newList.Add(item);
                var fourResult = new TransactionCreationDto();
                var user = await this.userService.RetrieveByIdAsync(userId);
                var res = new TotalAndTransaction()
                {
                    TotalResultDto = result,
                    TransactionResults = newList.OrderByDescending(item => item.CreatedAt).ToList(),
                    TransactionCategoryResultDto = thirdResult,
                    TransactionCreationDto = fourResult,
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
    }
    public async Task<IActionResult> TransactionChose(int selectedOption)
    {
        try
        {
            var json = Request.Cookies["userId"];
            long userId = JsonConvert.DeserializeObject<long>(json);

            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if(selectedOption == 0)
                return RedirectToAction("Index", "User");
            
            var pagination = new PaginationParams()
            {
                PageIndex = 1,
                PageSize = 100
            };
            var result = await this.transactionService.RetrieveMothlyByUserIdAsync(pagination, userId);
            var secondResult = await this.transactionService.RetrieveAllAsync(pagination);
            var thirdResult = await this.transactionCategoryService.RetrieveAllAsync(pagination);
            var newList = new List<TransactionResultDto>();
            foreach (var item in secondResult)
                if (item.UserId == userId && item.CreatedAt.Month == DateTime.Now.Month && item.CreatedAt.Year == DateTime.Now.Year)
                    newList.Add(item);
            var fourResult = new TransactionCreationDto();
            var user = await this.userService.RetrieveByIdAsync(userId);
            var data = new TotalAndTransaction()
            {
                TotalResultDto = result,
                TransactionResults = newList.OrderByDescending(item => item.CreatedAt).ToList(),
                TransactionCategoryResultDto = thirdResult,
                TransactionCreationDto = fourResult,
            };
            if (user.Role == Roles.User)
            {
                string serializedData = JsonConvert.SerializeObject(data);
                TempData["Data"] = serializedData;
                return RedirectToAction("Index", "User");
            }

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
            {
                var result = new UserCreateAndResForUser()
                {
                    Result = user
                };
                return View(result);
            }

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
    public async Task<IActionResult> UserUpdate(UserCreateAndResForUser dto)
    {
        try
        {
            var json = Request.Cookies["userId"];
            var id = JsonConvert.DeserializeObject<long>(json);
            var user = await this.userService.RetrieveByIdAsync(id);
            if (user.Role == Roles.User)
            {
                var result = await this.userService.UpdateAsync(dto.Creation, dto.Result.Id);
                return RedirectToAction("Settings", "User");
            }

            return RedirectToAction("Index", "Home");
        }
        catch(Exception ex)
        {
            return View(ex.Message);
        }
    }
    public async Task<IActionResult> TransactionDelete(long id)
    {
        var json = Request.Cookies["userId"];
        var userId = JsonConvert.DeserializeObject<long>(json);
        var user = await this.userService.RetrieveByIdAsync(userId);
        var result = new List<TransactionResultDto>();
        if (user.Role == Roles.User)
        {
            var transactions = await this.transactionService.DeleteAsync(id);
            return RedirectToAction("Index", "User");
        }

        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> TransactionCreate(TransactionCreationDto dto, long categoryId)
    {
        try
        {
            var initialDate = DateTime.ParseExact(Convert.ToString(dto.CreatedAt), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var updatedDateTime = new DateTime(initialDate.Year, initialDate.Month, initialDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            dto.CreatedAt = updatedDateTime.ToUniversalTime();
            var json = Request.Cookies["userId"];
            var id = JsonConvert.DeserializeObject<long>(json);
            var user = await this.userService.RetrieveByIdAsync(id);
            if (user.Role == Roles.User)
            {
                dto.CategoryId = categoryId;
                var result = await this.transactionService.AddAsync(dto, id);
                return RedirectToAction("Index", "User");
            }

            return RedirectToAction("Index", "Home");
        }
        catch(Exception ex)
        {
            return View(ex.Message);
        }
    }
}