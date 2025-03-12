using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Services.Interface
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories(string userId);
        Category GetCategoryById(int id);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        bool DeleteCategory(int id);
    }
}
