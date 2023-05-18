using Microsoft.AspNetCore.Mvc;
using MoneyManagmen.Web.Models;
using MoneyManagment.Domain.Configurations;
using MoneyManagment.Domain.Entities;
using MoneyManagment.Domain.Enums;
using MoneyManagment.Service.DTOs.TransactionCategory;
using MoneyManagment.Service.DTOs.Users;
using MoneyManagment.Service.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MoneyManagmen.Web.Controllers;

public class AdminController : Controller
{
    private readonly IUserService userService;
    private readonly ITransactionService transactionService;
    private readonly ITransactionCategoryService transactionCategoryService;

    public AdminController(IUserService userService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService)
    {
        this.userService = userService;
        this.transactionService = transactionService;
        this.transactionCategoryService = transactionCategoryService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var id = Request.Cookies["userId"];
            long userId = JsonConvert.DeserializeObject<long>(id);

            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var result = await this.userService.RetrieveByIdAsync(userId);
            if(result.Role == Roles.Admin)
            {
                var user = new UserResultDto();
                var pagination = new PaginationParams()
                {
                    PageIndex = 1,
                    PageSize = 1000
                };
                var users = await this.userService.RetrieveAllAsync(pagination);
                var newModel = new UserResAndCreate()
                {
                    Results = users,
                    Result = user
                };
                return View(newModel);
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
            if (user.Role == Roles.Admin)
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
    public async Task<IActionResult> Create(UserCreationDto dto)
    {
        try
        {
            var id = Request.Cookies["userId"];
            var userId = JsonConvert.DeserializeObject<long>(id);
            var user = await this.userService.RetrieveByIdAsync(userId);
            if (user.Role == Roles.Admin)
            {
                var result = await this.userService.AddAsync(dto);
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            return RedirectToAction("Index", "Home");
        }
    }
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var uid = Request.Cookies["userId"];
            var userId = JsonConvert.DeserializeObject<long>(uid);
            var user = await this.userService.RetrieveByIdAsync(userId);
            if (user.Role == Roles.Admin)
            {
                var result = await this.userService.DeleteAsync(id);
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            return RedirectToAction("Index", "Home");
        }
    }
    public async Task<IActionResult> CategoryDelete(long id)
    {
        try
        {
            var uid = Request.Cookies["userId"];
            var userId = JsonConvert.DeserializeObject<long>(uid);
            var user = await this.userService.RetrieveByIdAsync(userId);
            if (user.Role == Roles.Admin)
            {
                var result = await this.transactionCategoryService.DeleteAsync(id);
                return RedirectToAction("TransactionCategories", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            return RedirectToAction("Index", "Admin");
        }
    }
    public async Task<IActionResult> Transactions()
    {
        var json = Request.Cookies["role"];
        var role = JsonConvert.DeserializeObject<string>(json);
        if (role == Convert.ToString(Roles.Admin))
        {
            var pagination = new PaginationParams()
            {
                PageIndex = 1,
                PageSize = 10
            };
            var transactions = await this.transactionService.RetrieveAllAsync(pagination);
            return View(transactions);
        }

        return RedirectToAction("Index", "Admin");
    }
    public async Task<IActionResult> UserUpdate(UserCreateAndResForUser dto)
    {
        try
        {
            var json = Request.Cookies["userId"];
            var id = JsonConvert.DeserializeObject<long>(json);
            var user = await this.userService.RetrieveByIdAsync(id);
            if (user.Role == Roles.Admin)
            {
                var result = await this.userService.UpdateAsync(dto.Creation, dto.Result.Id);
                return RedirectToAction("Settings", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            return View(ex.Message);
        }
    }
    public async Task<IActionResult> UsersUpdate(int id, string firstName, string lastName, string email)
    {
        try
        {
            var json = Request.Cookies["userId"];
            var userId = JsonConvert.DeserializeObject<long>(json);
            var user = await this.userService.RetrieveByIdAsync(userId);
            if (user.Role == Roles.Admin)
            {
                var userForUpdate = new UserCreationDto()
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                };
                var result = await this.userService.UpdateAsync(userForUpdate, id, userId);
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            return View(ex.Message);
        }
    }
    public async Task<IActionResult> CategoryCreate(TransactionCreateAndResult dto)
    {
        var json = Request.Cookies["role"];
        var role = JsonConvert.DeserializeObject<string>(json);
        if (role == Convert.ToString(Roles.Admin))
        {
            var transactions = await this.transactionCategoryService.AddAsync(dto.CreationDto);
            return RedirectToAction("TransactionCategories", "Admin");
        }

        return RedirectToAction("Index", "Admin");
    }
    public async Task<IActionResult> CategoryUpdate(TransactionCreateAndResult dto, string oldName)
    {
        var json = Request.Cookies["role"];
        var role = JsonConvert.DeserializeObject<string>(json);
        if (role == Convert.ToString(Roles.Admin))
        {
            var category = await this.transactionCategoryService.RetrieveByNameAsync(oldName);
            var transactions = await this.transactionCategoryService.UpdateAsync(dto.CreationDto,category.Id);
            return RedirectToAction("TransactionCategories", "Admin");
        }

        return RedirectToAction("Index", "Admin");
    }
    public async Task<IActionResult> UserSearch(string value)
    {

        if (value is null)
            return RedirectToAction("Index", "Admin");

        var json = Request.Cookies["role"];
        var role = JsonConvert.DeserializeObject<string>(json);
        if (role == Convert.ToString(Roles.Admin))
        {
            var pagination = new PaginationParams()
            {
                PageIndex = 1,
                PageSize = 100
            };
            var users = await this.userService.RetrieveAllAsync(pagination);
            var result = new List<UserResultDto>();
            foreach (var user in users)
            {

                if (value is null || user.FirstName.ToLower().Contains(value.ToLower()) ||
                   user.LastName.ToLower().Contains(value.ToLower()) ||
                   Convert.ToString(user.IsVerify).ToLower().Contains(value.ToLower()) ||
                   user.Email.ToLower().Contains(value.ToLower()) ||
                   Convert.ToString(user.Role).ToLower().Contains(value.ToLower()))
                {
                    result.Add(user);
                }
            }
            return View(result);
        }

        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> TransactionCategories()
    {
        var json = Request.Cookies["role"];
        var role = JsonConvert.DeserializeObject<string>(json);
        if (role == Convert.ToString(Roles.Admin))
        {
            var pagination = new PaginationParams()
            {
                PageIndex = 1,
                PageSize = 10
            };
            var transactionCategories = await this.transactionCategoryService.RetrieveAllAsync(pagination);
            var newDto = new TransactionCreateAndResult()
            {
                TransactionCategories = transactionCategories
            };
            return View(newDto);
        }

        return RedirectToAction("Index", "Home");
    }
}
