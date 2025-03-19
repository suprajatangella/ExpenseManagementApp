using ExpenseManagement.Application.Services.Implementation;
using ExpenseManagement.Application.Services.Interface;
using ExpenseManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ExpenseManagement.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;

        public ExpensesController(IExpenseService expenseService, UserManager<User> userManager, ICategoryService categoryService)
        {
            _expenseService = expenseService;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var user =  await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var expenses = _expenseService.GetAllExpenses(_userManager.GetUserId(User), role[0].ToString()) ;
            GetPaymentMethods();
            foreach (var expense in expenses)
            {
                foreach (var item in ViewBag.PaymentMethods as List<SelectListItem>)
                {
                    if (item.Value == expense.PaymentMethod)
                    {
                        expense.PaymentMethod = item.Text;
                    }
                }
            }
            return View(expenses);
        }

        public IActionResult Create()
        {
            Expense expense = new Expense
            {
                Date = DateTime.UtcNow // Set today's date by default
            };
            GetPaymentMethods();
            ViewBag.Users = GetUsers();
            ViewBag.Categories = GetCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(expense);
        }
        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            //ModelState.Remove("User");
            //ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Manager"))
                {
                    expense.CreatedBy = expense.UserId;
                }
                else
                {
                    expense.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                TempData["success"] = _expenseService.CreateExpense(expense); 
                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }

        public IActionResult Edit(int id)
        {
            var expense = _expenseService.GetExpenseById(id);

            if (expense == null)
            {
                return RedirectToAction("Error", "Home");
            }
            GetPaymentMethods();
            ViewBag.Users = GetUsers();

            return View(expense);
        }
        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            if (ModelState.IsValid)
            {
                expense.UpdatedDate = DateTime.Now;
                if (User.IsInRole("Admin") || User.IsInRole("Manager"))
                {
                    expense.UpdatedBy = expense.UserId;
                }
                else
                {
                    expense.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                }
                TempData["success"] = _expenseService.UpdateExpense(expense); 
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        public IActionResult Delete(int id)
        {
            var expense = _expenseService.GetExpenseById(id);
            if (expense == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(expense);
        }
        [HttpPost]
        public IActionResult Delete(Expense expense)
        {
            if (expense != null)
            {
                _expenseService.DeleteExpense(expense.Id);
                TempData["success"] = "The Expense has been deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the expense.";
            }
            return View(expense);
        }

        public IEnumerable<SelectListItem> GetUsers()
        {
            return _userManager.Users.Select(user => new SelectListItem
            {
                Value = user.Id, // Set the Value property to the user's ID
                Text = user.FullName // Set the Text property to the user's Name
            });
        }
        public IEnumerable<SelectListItem> GetCategories(string userId)
        {
            return _categoryService.GetAllCategories(userId).Select(cat => new SelectListItem
            {
                Value = cat.Id.ToString(), // Set the Value property to the user's ID
                Text = cat.Name // Set the Text property to the user's Name
            });
        }

        public void GetPaymentMethods()
        {
            ViewBag.PaymentMethods = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Cash" },
                new SelectListItem { Value = "2", Text = "Credit Card" },
                new SelectListItem { Value = "3", Text = "Debit Card" },
                new SelectListItem { Value = "4", Text = "Bank Transfer" }
            };
        }
    }
}
