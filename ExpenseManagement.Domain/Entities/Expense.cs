namespace ExpenseManagement.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; } // E.g., "Dinner at restaurant"
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } // "Cash", "Credit Card", "Bank Transfer"
        public string Notes { get; set; } // Optional description

        // Foreign Key - Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Foreign Key - User (Who added the expense)
        public int UserId { get; set; }
        public User User { get; set; }

        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }

    }
}
