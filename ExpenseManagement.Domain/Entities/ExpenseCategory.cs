namespace ExpenseManagement.Domain.Entities
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public int ExpenseId { get; set; }
        public int CategoryId { get; set; }

        public virtual Expense Expense { get; set; }
        public virtual Category Category { get; set; }

        public decimal Amount { get; set; }
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }
    }
}
