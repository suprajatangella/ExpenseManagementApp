using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Services.Interface
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNotifications(string userId, string userRole);
        Notification GetNotificationById(int id);
        void CreateNotification(Notification notification);
        void UpdateNotification(Notification notification);
        bool DeleteNotification(int id);
        void MarkAsRead(int id);
    }
}
