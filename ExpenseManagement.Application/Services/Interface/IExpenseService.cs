﻿using System;
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
        string CreateExpense(Expense expense);
        string UpdateExpense(Expense expense);
        bool DeleteExpense(int id);

        IEnumerable<Expense> GetExpensesReportData(int year, int month, string userId);
    }
}
