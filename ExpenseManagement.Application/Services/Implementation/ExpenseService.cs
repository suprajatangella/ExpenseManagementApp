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
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateExpense(Expense expense)
        {
            _unitOfWork.Expense.Add(expense);
            _unitOfWork.Save();
        }

        public bool DeleteExpense(int id)
        {
            var expense = _unitOfWork.Expense.Get(n => n.Id == id);

            if (expense != null)
            {
                _unitOfWork.Expense.Remove(expense);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<Expense> GetAllExpenses(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all expenses
                return _unitOfWork.Expense.GetAll()
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own expenses
                return _unitOfWork.Expense.GetAll()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

        public Expense GetExpenseById(int id)
        {
            return _unitOfWork.Expense.Get(n=> n.Id == id);
        }

        public void UpdateExpense(Expense expense)
        {
            _unitOfWork.Expense.Update(expense);
            _unitOfWork.Save();
        }
    }
}
