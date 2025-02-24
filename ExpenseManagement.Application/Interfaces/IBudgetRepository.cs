using ExpenseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagement.Application.Interfaces
{
    public interface IBudgetRepository : IRepository<Budget>
    {
        void Update(Budget entity);
    }
}
