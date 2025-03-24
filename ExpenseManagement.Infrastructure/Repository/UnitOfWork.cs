using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Application.Interfaces;
using ExpenseManagement.Infrastructure.Repository;
using ExpenseManagement.InfraStructure.Data;

namespace ExpenseManagement.InfraStructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IBudgetRepository Budget { get; private set; }

        public ICategoryRepository Category { get; private set; }

        //public IExpenseCategoryRepository ExpenseCategory { get; private set; }

        public IExpenseRepository Expense { get; private set; }

        public INotificationRepository Notification { get; private set; }

        public IUserRepository User { get; private set; }

        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Expense = new ExpenseRepository(_db);
            Budget = new BudgetRepository(_db);
            Category = new CategoryRepository(_db);
            //ExpenseCategory = new ExpenseCategoryRepository(_db);
            Notification = new NotificationRepository(_db);
            User= new UserRepository(_db);
        }

        async Task IUnitOfWork.Save()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
             //await _db.SaveChangesAsync();
        }
    }
}
