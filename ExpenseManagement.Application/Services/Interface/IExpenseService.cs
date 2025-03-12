using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Services.Interface
{
    public interface IExpenseService
    {
        IEnumerable<Expense> GetAllExpenses(string userId, string userRole);
        Expense GetExpenseById(int id);
        void CreateExpense(Expense expense);
        void UpdateExpense(Expense expense);
        bool DeleteExpense(int id);
    }
}
