using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class MyDataCancelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "MyDataInvoices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MyDataCancelationResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataInvoiceId = table.Column<Guid>(nullable: false),
                    cancellationMark = table.Column<long>(nullable: true),
                    statusCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataCancelationResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataCancelationResponses_MyDataInvoices_MyDataInvoiceId",
                        column: x => x.MyDataInvoiceId,
                        principalTable: "MyDataInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataCancelInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Uid = table.Column<long>(nullable: true),
                    invoiceMark = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataCancelInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyDataCancelationErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataCancelationResponseId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataCancelationErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataCancelationErrors_MyDataCancelationResponses_MyDataCancelationResponseId",
                        column: x => x.MyDataCancelationResponseId,
                        principalTable: "MyDataCancelationResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyDataCancelationErrors_MyDataCancelationResponseId",
                table: "MyDataCancelationErrors",
                column: "MyDataCancelationResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataCancelationResponses_MyDataInvoiceId",
                table: "MyDataCancelationResponses",
                column: "MyDataInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyDataCancelationErrors");

            migrationBuilder.DropTable(
                name: "MyDataCancelInvoices");

            migrationBuilder.DropTable(
                name: "MyDataCancelationResponses");

            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "MyDataInvoices");
        }
    }
}
