using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Services.Interface
{
    public interface IBudgetService
    {
        IEnumerable<Budget> GetAllBudgets(string userId, string userRole);
        Budget GetBudgetById(int id);
        void CreateBudget(Budget budget);
        void UpdateBudget(Budget budget);
        bool DeleteBudget(int id);
        bool CheckIfBudgetExists(string userId, int Month, int Year);
    }
}
