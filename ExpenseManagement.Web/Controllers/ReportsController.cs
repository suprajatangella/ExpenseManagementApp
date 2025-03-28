using ExpenseManagement.Application.Services.Interface;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ExpenseManagement.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly UserManager<User> _userManager;
        public ReportsController(UserManager<User> userManager, IExpenseService expenseService)
        {
            _userManager = userManager;
            _expenseService = expenseService;
        }
        public IActionResult Index()
        {
            return View(new ReportVM());
        }
        [HttpPost]
        public IActionResult GenerateExpenseReport(ReportVM model)
        {
            // Logic to fetch and prepare report data based on the selected month and year
            model.Expenses = GetExpenseReportData(model.Month, model.Year);

            if(model.Expenses == null || model.Expenses.Count() == 0)
            {
                ModelState.AddModelError(string.Empty, "No expenses found for the selected month and year.");
                TempData["Error"] = "No expenses found for the selected month and year.";
                return View("Index", model);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Add title
                // Create a font for the title
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, Font.UNDERLINE);

                // Add title
                Paragraph title = new Paragraph("Expense Report - " + DateTime.Now.ToString("MMMM yyyy"), titleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(title);

                // Add user name
                Paragraph userName = new Paragraph("User Name: " + _userManager.GetUserName(User))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(userName);

                document.Add(new Paragraph("\n"));

                // Create a font for the table header
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                // Add expenses in a table format
                PdfPTable table = new PdfPTable(3);
                table.AddCell(new PdfPCell(new Phrase("Date", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Description", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Amount", headerFont)));

                foreach (var expense in model.Expenses)
                {
                    table.AddCell(expense.Date.ToShortDateString());
                    table.AddCell(expense.Notes);
                    table.AddCell(expense.Amount.ToString("C"));
                }

                document.Add(table);
                document.Close();
                TempData["Success"] = "Expense report generated successfully!";
                return File(ms.ToArray(), "application/pdf", "ExpenseReport - "+ DateTime.UtcNow + ".pdf");
                //return View("Index", model);
            }
        }
        private IEnumerable<Expense> GetExpenseReportData(int month, int year)
        {
            // Replace with actual data fetching logic
            return _expenseService.GetExpensesReportData(year, month, _userManager.GetUserId(User));
        }


    }
}
