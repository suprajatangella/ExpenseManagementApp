using ExpenseManagement.Application.Services.Implementation;
using ExpenseManagement.Application.Services.Interface;
using ExpenseManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpenseManagement.Web.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly UserManager<User> _userManager;
        public BudgetController(IBudgetService budgetService, UserManager<User> userManager)
        {
            _budgetService = budgetService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var budgets = _budgetService.GetAllBudgets(_userManager.GetUserId(User), role[0].ToString());
            return View(budgets);
        }

        public IActionResult AddBudget()
        {
            ViewBag.Users = GetUsers();
            return View();
        }

        [HttpPost]
        public IActionResult AddBudget(Budget budget)
        {
            if(budget == null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (_budgetService.CheckIfBudgetExists(budget.UserId, budget.Month.Month, budget.Month.Year))
            {
                ModelState.AddModelError("", "A budget for this month and year already exists for this user.");
                TempData["error"] = "A budget for this month and year already exists for this user.";
                return View(budget);
            }
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed to add the budget.";
                return View(budget);
            }
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                budget.CreatedBy = budget.UserId;
            }
            else
            {
                budget.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            budget.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            _budgetService.CreateBudget(budget);
            TempData["success"] = "The Budget has been added Successfully.";
            return View(budget);
        }
        public IActionResult EditBudget(int id)
        {
            var budget = _budgetService.GetBudgetById(id);

            if (budget == null)
            {
                return RedirectToAction("Error", "Home");
            }
            ViewBag.Users = GetUsers();
            return View(budget);
        }

        [HttpPost]
        public async Task<IActionResult> EditBudget(Budget budget)
        {
            if (budget.SpentAmount > budget.MonthlyLimit)
            {
                TempData["Warning"] = "Warning: Spent amount exceeds the monthly budget limit!";
            }
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Manager"))
                {
                    budget.UpdatedBy = budget.UserId;
                }
                else
                {
                    budget.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                budget.UpdatedDate = DateTime.Now;
                _budgetService.UpdateBudget(budget);
                TempData["success"] = "The Budget has been updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to update the budget.";
            }
            return View(budget);
        }

        public IActionResult Delete(int id)
        {
            var budget = _budgetService.GetBudgetById(id);
            if (budget == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(budget);
        }
        [HttpPost]
        public IActionResult Delete(Budget budget)
        {
            if (budget != null)
            {
                
                _budgetService.GetBudgetById(budget.Id);
                TempData["success"] = "The Budget has been deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the budget.";
            }
            return View(budget);
        }

        public IEnumerable<SelectListItem> GetUsers()
        {
            return _userManager.Users.Select(user => new SelectListItem
            {
                Value = user.Id, // Set the Value property to the user's ID
                Text = user.FullName // Set the Text property to the user's Name
            });
        }
    }
}

