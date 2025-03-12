using Microsoft.AspNetCore.Identity;

namespace ExpenseManagement.Domain.Entities
{
    /// <summary>
    /// Represents a user in the expense management system.
    /// </summary>
    public class User : IdentityUser
    {
        
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }
       
        /// <summary>
        /// Gets or sets the date and time when the user was created.
        /// </summary>
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }
       
    }
}
