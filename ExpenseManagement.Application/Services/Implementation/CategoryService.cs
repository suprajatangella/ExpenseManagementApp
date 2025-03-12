using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Application.Interfaces;
using ExpenseManagement.Application.Services.Interface;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateCategory(Category category)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();
        }

        public bool DeleteCategory(int id)
        {
            var category = _unitOfWork.Category.Get(n => n.Id == id);

            if (category != null)
            {
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<Category> GetAllCategories(string userId)
        {
                // Admins can view all budgets
                return _unitOfWork.Category.GetAll()
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.Category.Get(n=> n.Id == id);
        }

        public void UpdateCategory(Category category)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
        }
    }
}
