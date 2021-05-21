using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MyDataInvoices_InvoiceTypeCode",
                table: "MyDataInvoices");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoices_InvoiceTypeCode",
                table: "MyDataInvoices",
                column: "InvoiceTypeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MyDataInvoices_InvoiceTypeCode",
                table: "MyDataInvoices");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoices_InvoiceTypeCode",
                table: "MyDataInvoices",
                column: "InvoiceTypeCode",
                unique: true);
        }
    }
}
