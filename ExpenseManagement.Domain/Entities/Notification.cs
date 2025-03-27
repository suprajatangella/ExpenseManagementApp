using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } // "Your budget limit exceeded!"
        public bool IsRead { get; set; } = false; // Mark notifications as read

        public int NotificationTypeId { get; set; }

        [NotMapped]
        public string? NotificationTypeText { get; set; } // "Expense Approval"

        [NotMapped]
        public IEnumerable<SelectListItem>? NotificationTypes { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Expense Approval" },
            new SelectListItem { Value = "2", Text = "Expense Rejected" },
            new SelectListItem { Value = "3", Text = "Expense Approved" },
            new SelectListItem { Value = "4", Text = "Reimbursement Processed" },
            new SelectListItem { Value = "5", Text = "New Policy Update" },
            new SelectListItem { Value = "6", Text = "Expense Submission Reminder" },
            new SelectListItem { Value = "7", Text = "Receipt Upload Pending" },
            new SelectListItem { Value = "8", Text = "Expense Report Overdue" },
            new SelectListItem { Value = "9", Text = "Manager Approval Needed" }
        };

        // Foreign Key - User
        public string UserId { get; set; }
        public User? User { get; set; }
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }

    }
}
