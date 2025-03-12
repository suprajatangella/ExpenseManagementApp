using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Application.Interfaces;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Infrastructure.Repository;
using ExpenseManagement.InfraStructure.Data;

namespace ExpenseManagement.InfraStructure.Repository
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _db;
        public ExpenseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Expense expense)
        {
            _db.Expenses.Update(expense);
        }
    }
}
