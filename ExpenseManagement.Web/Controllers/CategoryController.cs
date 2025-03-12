using ExpenseManagement.Application.Services.Implementation;
using ExpenseManagement.Application.Services.Interface;
using ExpenseManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetAllCategories(_userManager.GetUserId(User));
            return View(categories);
        }

        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(category);
                TempData["success"] = "The category has been created Successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UpdatedDate = DateTime.Now;
                _categoryService.UpdateCategory(category);
                TempData["success"] = "The Category has been updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            if (category != null)
            {
                _categoryService.DeleteCategory(category.Id);
                TempData["success"] = "The Category has been deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the Category.";
            }
            return View(category);
        }
    }
}
