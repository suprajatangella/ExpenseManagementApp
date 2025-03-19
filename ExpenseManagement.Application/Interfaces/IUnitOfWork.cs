using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IBudgetRepository Budget { get; }
        ICategoryRepository Category { get; }
        //IExpenseCategoryRepository ExpenseCategory { get; }
        IExpenseRepository Expense { get; }
        INotificationRepository Notification { get; }
        IUserRepository User { get; }
        Task Save();
    }
}
