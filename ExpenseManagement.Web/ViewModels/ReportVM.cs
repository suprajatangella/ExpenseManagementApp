using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Web.ViewModels
{
    public class ReportVM
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public IEnumerable<Expense>? Expenses { get; set; }
    }
}
