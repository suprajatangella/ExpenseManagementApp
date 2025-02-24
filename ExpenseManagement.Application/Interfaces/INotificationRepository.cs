using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        void Update(Notification entity);
    }
}
