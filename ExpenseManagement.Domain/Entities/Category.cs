namespace ExpenseManagement.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // E.g., "Food", "Transport", "Entertainment"
        public string Description { get; set; } // Optional

        // One category can have multiple expenses
        public List<Expense> Expenses { get; set; }

        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; } = null;
    }
}
