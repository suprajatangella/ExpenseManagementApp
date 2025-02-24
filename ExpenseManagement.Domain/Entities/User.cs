using Microsoft.AspNetCore.Identity;

namespace ExpenseManagement.Domain.Entities
{
    /// <summary>
    /// Represents a user in the expense management system.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the role of the user. Possible values are "User" and "Admin".
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user was created.
        /// </summary>
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the list of expenses associated with the user.
        /// </summary>
        public List<Expense> Expenses { get; set; }
    }
}
