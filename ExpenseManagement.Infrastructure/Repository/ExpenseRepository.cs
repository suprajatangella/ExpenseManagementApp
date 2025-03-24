using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagement.Application.Interfaces;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Infrastructure.Repository;
using ExpenseManagement.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.InfraStructure.Repository
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _db;
        public ExpenseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task AddExpense(Expense expense)
        {
            try
            {
                await _db.Database.ExecuteSqlRawAsync(
            "INSERT INTO Expenses (Amount, Date, PaymentMethod, UserId, Title, Notes, CategoryId, CreatedDate, CreatedBy) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)",
            expense.Amount, expense.Date, expense.PaymentMethod, expense.UserId, expense.Title, expense.Notes, expense.CategoryId, expense.CreatedDate, expense.CreatedBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //_db.Entry(expense).State = EntityState.Added; // Explicitly mark as Added
            
        }

        public async Task UpdateExpense(Expense expense)
        {
            try
            {
                using (var connection = _db.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    await _db.Database.ExecuteSqlRawAsync(
                "UPDATE Expenses SET Amount = @p0, Date = @p1, PaymentMethod = @p2, UserId = @p4, Title = @p5, Notes = @p6, CategoryId = @p7, UpdatedDate = @p8, UpdatedBy = @p9 WHERE Id = @p3",
                expense.Amount, expense.Date, expense.PaymentMethod, expense.Id, expense.UserId, expense.Title, expense.Notes, expense.CategoryId, expense.UpdatedDate, expense.UpdatedBy);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task DeleteExpense(int Id)
        {
            try
            {
                using (var connection = _db.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    await using var transaction = await _db.Database.BeginTransactionAsync();
                    await _db.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Expenses WHERE Id = @p0", Id);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
