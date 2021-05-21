using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fixes_toInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "MyDataInvoices");

            migrationBuilder.AddColumn<long>(
                name: "CancellationMark",
                table: "MyDataInvoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationMark",
                table: "MyDataInvoices");

            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "MyDataInvoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
