using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Domain.Entities
{ 
    public class Budget
    {
        public int Id { get; set; }
        public decimal MonthlyLimit { get; set; } // Budget limit
        public decimal SpentAmount { get; set; } // Auto-updated based on expenses
        public DateTime Month { get; set; } // Budget for a specific month

        // Foreign Key - User
        [Required]
        public string UserId { get; set; }
        public User? User { get; set; }

        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
