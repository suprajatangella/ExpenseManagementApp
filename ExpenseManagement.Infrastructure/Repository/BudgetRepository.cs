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
    public class BudgetRepository : Repository<Budget>, IBudgetRepository
    {
        private readonly ApplicationDbContext _db;
        public BudgetRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Budget budget)
        {
            _db.Budgets.Update(budget);
        }
    }
}
