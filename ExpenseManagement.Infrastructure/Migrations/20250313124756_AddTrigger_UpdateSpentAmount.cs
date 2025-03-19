using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrigger_UpdateSpentAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Budgets_AspNetUsers_UserId1",
            //    table: "Budgets");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Expenses_AspNetUsers_UserId1",
            //    table: "Expenses");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notifications_AspNetUsers_UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropTable(
            //    name: "ExpenseCategories");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notifications_UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropIndex(
            //    name: "IX_Expenses_UserId1",
            //    table: "Expenses");

            //migrationBuilder.DropIndex(
            //    name: "IX_Budgets_UserId1",
            //    table: "Budgets");

            //migrationBuilder.DropColumn(
            //    name: "UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropColumn(
            //    name: "UserId1",
            //    table: "Expenses");

            //migrationBuilder.DropColumn(
            //    name: "UserId1",
            //    table: "Budgets");

            //migrationBuilder.DropColumn(
            //    name: "Role",
            //    table: "AspNetUsers");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserId",
            //    table: "Notifications",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserId",
            //    table: "Expenses",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserId",
            //    table: "Budgets",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<string>(
            //    name: "PasswordHash",
            //    table: "AspNetUsers",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Email",
            //    table: "AspNetUsers",
            //    type: "nvarchar(256)",
            //    maxLength: 256,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(256)",
            //    oldMaxLength: 256);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notifications_UserId",
            //    table: "Notifications",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Expenses_UserId",
            //    table: "Expenses",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Budgets_UserId",
            //    table: "Budgets",
            //    column: "UserId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Budgets_AspNetUsers_UserId",
            //    table: "Budgets",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Expenses_AspNetUsers_UserId",
            //    table: "Expenses",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notifications_AspNetUsers_UserId",
            //    table: "Notifications",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            string triggerSql = @"
               CREATE TRIGGER UpdateSpentAmount  
            ON Expenses  
            AFTER INSERT  
            AS  
            BEGIN  
                SET NOCOUNT ON;  

                -- Update Budgets by adding the new expense amount
                UPDATE B  
                SET SpentAmount = COALESCE(B.SpentAmount, 0) + COALESCE(I.Amount, 0)  
                FROM Budgets B  
                INNER JOIN INSERTED I  
                ON B.UserId = I.UserId  
                WHERE FORMAT(B.Month, 'yyyy-MM') = FORMAT(I.Date, 'yyyy-MM');  
            END;";

            migrationBuilder.Sql(triggerSql);

            migrationBuilder.Sql(@"
            CREATE TRIGGER UpdateSpentAmountOnUpdate
            ON Expenses
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                -- Update the budget's SpentAmount
                UPDATE B
                SET SpentAmount = COALESCE(B.SpentAmount, 0) 
                                - COALESCE(d.Amount, 0) 
                                + COALESCE(i.Amount, 0)
                FROM Budgets B
                INNER JOIN INSERTED i ON B.UserId = i.UserId
                INNER JOIN DELETED d ON i.Id = d.Id
                WHERE FORMAT(B.Month, 'yyyy-MM') = FORMAT(i.Date, 'yyyy-MM')
                  AND FORMAT(B.Month, 'yyyy-MM') = FORMAT(d.Date, 'yyyy-MM'); -- Ensure the correct month

                -- Check if SpentAmount exceeds MonthlyLimit
                DECLARE @UserId NVARCHAR(450), @Month DATE, @ExceededAmount DECIMAL(18,2);

                SELECT TOP 1 @UserId = B.UserId, @Month = B.Month, @ExceededAmount = B.SpentAmount
                FROM Budgets B
                INNER JOIN INSERTED I ON B.UserId = I.UserId
                WHERE FORMAT(B.Month, 'yyyy-MM') = FORMAT(I.Date, 'yyyy-MM')
                  AND B.SpentAmount > B.MonthlyLimit;

                -- Raise an alert if budget limit is exceeded
                IF @UserId IS NOT NULL
                BEGIN
                    RAISERROR ('Alert: Your spending has exceeded the monthly budget limit!', 16, 1);
                END
            END;
        ");

            migrationBuilder.Sql(@"CREATE TRIGGER DeleteExpenseUpdateBudgets
                ON expenses
                AFTER DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE budgets
                    SET SpentAmount = SpentAmount - d.Amount
                    FROM budgets b
                    INNER JOIN deleted d 
                        ON b.UserId = d.UserId 
                        AND FORMAT(b.Month, 'yyyy-MM') = FORMAT(d.Date, 'yyyy-MM');
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Budgets_AspNetUsers_UserId",
            //    table: "Budgets");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Expenses_AspNetUsers_UserId",
            //    table: "Expenses");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notifications_AspNetUsers_UserId",
            //    table: "Notifications");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notifications_UserId",
            //    table: "Notifications");

            //migrationBuilder.DropIndex(
            //    name: "IX_Expenses_UserId",
            //    table: "Expenses");

            //migrationBuilder.DropIndex(
            //    name: "IX_Budgets_UserId",
            //    table: "Budgets");

            //migrationBuilder.AlterColumn<int>(
            //    name: "UserId",
            //    table: "Notifications",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId1",
            //    table: "Notifications",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AlterColumn<int>(
            //    name: "UserId",
            //    table: "Expenses",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId1",
            //    table: "Expenses",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AlterColumn<int>(
            //    name: "UserId",
            //    table: "Budgets",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId1",
            //    table: "Budgets",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AlterColumn<string>(
            //    name: "PasswordHash",
            //    table: "AspNetUsers",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Email",
            //    table: "AspNetUsers",
            //    type: "nvarchar(256)",
            //    maxLength: 256,
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(256)",
            //    oldMaxLength: 256,
            //    oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Role",
            //    table: "AspNetUsers",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.CreateTable(
            //    name: "ExpenseCategories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryId = table.Column<int>(type: "int", nullable: false),
            //        ExpenseId = table.Column<int>(type: "int", nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
            //        UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ExpenseCategories_Categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ExpenseCategories_Expenses_ExpenseId",
            //            column: x => x.ExpenseId,
            //            principalTable: "Expenses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notifications_UserId1",
            //    table: "Notifications",
            //    column: "UserId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Expenses_UserId1",
            //    table: "Expenses",
            //    column: "UserId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Budgets_UserId1",
            //    table: "Budgets",
            //    column: "UserId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExpenseCategories_CategoryId",
            //    table: "ExpenseCategories",
            //    column: "CategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExpenseCategories_ExpenseId",
            //    table: "ExpenseCategories",
            //    column: "ExpenseId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Budgets_AspNetUsers_UserId1",
            //    table: "Budgets",
            //    column: "UserId1",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Expenses_AspNetUsers_UserId1",
            //    table: "Expenses",
            //    column: "UserId1",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notifications_AspNetUsers_UserId1",
            //    table: "Notifications",
            //    column: "UserId1",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS UpdateSpentAmount;");
            // Drop the trigger if the migration is rolled back
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS UpdateSpentAmountOnUpdate;");
        }
    }
}
