using ExpenseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagement.Application.Interfaces
{
    public interface IExpenseRepository  : IRepository<Expense>
    {
        Task AddExpense(Expense expense);
        Task UpdateExpense(Expense expense);
        Task DeleteExpense(int Id);
        IEnumerable<Expense> GetExpensesReportData(int year, int month, string userId);
    }
}
