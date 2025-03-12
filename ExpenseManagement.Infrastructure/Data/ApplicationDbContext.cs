using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpenseManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.InfraStructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey }); // Composite key

        //    modelBuilder.Entity<ExpenseCategory>()
        //.HasOne(e => e.Expense)
        //.WithMany()
        //.HasForeignKey(e => e.ExpenseId)
        //.OnDelete(DeleteBehavior.NoAction); // Or use DeleteBehavior.NoAction

        }

    }
}
