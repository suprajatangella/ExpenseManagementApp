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
        private readonly IEmailService _emailService;
        public ReportsController(UserManager<User> userManager, IExpenseService expenseService, IEmailService emailService)
        {
            _userManager = userManager;
            _expenseService = expenseService;
            _emailService = emailService;
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
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, Font.UNDERLINE, BaseColor.BLUE ); 

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
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

                // Add expenses in a table format
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell dateHeader = new PdfPCell(new Phrase("Date", headerFont))
                {
                    BackgroundColor = BaseColor.BLUE,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidth = 2f
                };
                PdfPCell descriptionHeader = new PdfPCell(new Phrase("Description", headerFont))
                {
                    BackgroundColor = BaseColor.BLUE,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidth = 2f
                };
                PdfPCell amountHeader = new PdfPCell(new Phrase("Amount", headerFont))
                {
                    BackgroundColor = BaseColor.BLUE,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidth = 2f
                };

                table.AddCell(dateHeader);
                table.AddCell(descriptionHeader);
                table.AddCell(amountHeader);
                decimal totalAmount = 0;

                foreach (var expense in model.Expenses)
                {
                    PdfPCell dateCell = new PdfPCell(new Phrase(expense.Date.ToShortDateString()))
                    {
                        BorderWidth = 2f
                    };
                    PdfPCell descriptionCell = new PdfPCell(new Phrase(expense.Notes))
                    {
                        BorderWidth = 2f
                    };
                    PdfPCell amountCell = new PdfPCell(new Phrase(expense.Amount.ToString("C")))
                    {
                        BorderWidth = 2f
                    };

                    table.AddCell(dateCell);
                    table.AddCell(descriptionCell);
                    table.AddCell(amountCell);
                    //table.AddCell(expense.Date.ToShortDateString());
                    //table.AddCell(expense.Notes);
                    //table.AddCell(expense.Amount.ToString("C"));
                    totalAmount += expense.Amount;
                }

                PdfPCell totalCell = new PdfPCell(new Phrase("Total", headerFont))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = BaseColor.BLUE,
                    BorderWidth = 2f

                };

                table.AddCell(totalCell);
                table.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("C"), headerFont))
                    {
                        BackgroundColor = BaseColor.BLUE,
                        BorderWidth = 2f
                });


                document.Add(table);
                document.Close();
                TempData["Success"] = "Expense report generated successfully!";
                // Send the PDF as an attachment via email
                _emailService.SendEmailWithAttachment(
                     ms.ToArray(),
                    "ExpenseReport - " + DateTime.UtcNow + ".pdf"
                );

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
