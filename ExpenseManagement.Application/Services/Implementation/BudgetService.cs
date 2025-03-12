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
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudgetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateBudget(Budget budget)
        {
            _unitOfWork.Budget.Add(budget);
            _unitOfWork.Save();
        }

        public bool DeleteBudget(int id)
        {
            var budget = _unitOfWork.Budget.Get(n => n.Id == id);

            if (budget != null)
            {
                _unitOfWork.Budget.Remove(budget);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<Budget> GetAllBudgets(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all budgets
                return _unitOfWork.Budget.GetAll()
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own budgets
                return _unitOfWork.Budget.GetAll()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

        public Budget GetBudgetById(int id)
        {
            return _unitOfWork.Budget.Get(n=> n.Id == id);
        }

        public void UpdateBudget(Budget budget)
        {
            _unitOfWork.Budget.Update(budget);
            _unitOfWork.Save();
        }
    }
}
