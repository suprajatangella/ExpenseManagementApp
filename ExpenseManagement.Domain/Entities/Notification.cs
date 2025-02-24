namespace ExpenseManagement.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } // "Your budget limit exceeded!"
        public bool IsRead { get; set; } = false; // Mark notifications as read

        // Foreign Key - User
        public int UserId { get; set; }
        public User User { get; set; }
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }

    }
}
