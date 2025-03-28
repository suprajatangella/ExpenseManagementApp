using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Application.Interfaces;
using ExpenseManagement.Application.Services.Interface;
using ExpenseManagement.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace ExpenseManagement.Application.Services.Implementation
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string CreateExpense(Expense expense)
        {
            try
            {
                //_unitOfWork.Expense.Add(expense);
                _unitOfWork.Expense.AddExpense(expense);
                //_unitOfWork.Save();
                return "Expense added successfully!";
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    // Check for the specific error number thrown by the trigger
                    if (sqlEx.Number == 50001) // This is the error number from THROW in SQL
                    {
                        return "⚠️ Alert: Your spending has exceeded the monthly budget limit!";
                    }
                }

                return "Error while adding expense. Please try again.";
            }
        }

        public bool DeleteExpense(int id)
        {
            var expense = _unitOfWork.Expense.Get(n => n.Id == id, tracked: true);

            if (expense != null)
            {
                _unitOfWork.Expense.DeleteExpense(id);
                //_unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<Expense> GetAllExpenses(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all expenses
                return _unitOfWork.Expense.GetAll(includeProperties: "Category,User", tracked: true)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own expenses
                return _unitOfWork.Expense.GetAll(includeProperties: "Category,User", tracked: true) 
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

        public Expense GetExpenseById(int id)
        {
            return _unitOfWork.Expense.Get(n=> n.Id == id, includeProperties: "Category,User", tracked: true);
        }

        public IEnumerable<Expense> GetExpensesReportData(int year, int month, string userId)
        {
            return _unitOfWork.Expense.GetExpensesReportData(year, month, userId);
        }

        public string UpdateExpense(Expense expense)
        {
            try
            {
                _unitOfWork.Expense.UpdateExpense(expense);
                //_unitOfWork.Save();
                return "Expense updated successfully!";
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    // Check for the specific error number thrown by the trigger
                    if (sqlEx.Number == 50001) // This is the error number from THROW in SQL
                    {
                        throw new Exception("⚠️ Alert: Your spending has exceeded the monthly budget limit!");
                    }
                }
                throw new Exception("Error while updating expense. Please try again.");
            }

        }
    }
}
