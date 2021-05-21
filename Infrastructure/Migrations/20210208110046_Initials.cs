using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyDataInvoiceTypes",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    ShortTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    sign = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoiceTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MyDataInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Uid = table.Column<long>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    InvoiceNumber = table.Column<long>(nullable: true),
                    VAT = table.Column<string>(nullable: true),
                    InvoiceTypeCode = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    StoredXml = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataInvoices_MyDataInvoiceTypes_InvoiceTypeCode",
                        column: x => x.InvoiceTypeCode,
                        principalTable: "MyDataInvoiceTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataInvoiceId = table.Column<Guid>(nullable: false),
                    index = table.Column<int>(nullable: true),
                    statusCode = table.Column<string>(nullable: true),
                    invoiceUid = table.Column<string>(nullable: true),
                    invoiceMark = table.Column<long>(nullable: true),
                    authenticationCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataResponses_MyDataInvoices_MyDataInvoiceId",
                        column: x => x.MyDataInvoiceId,
                        principalTable: "MyDataInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataResponseId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataErrors_MyDataResponses_MyDataResponseId",
                        column: x => x.MyDataResponseId,
                        principalTable: "MyDataResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MyDataInvoiceTypes",
                columns: new[] { "Code", "Description", "ShortTitle", "Title", "sign" },
                values: new object[,]
                {
                    { 40, "10 παρ. 1 Π.Δ. 186/92 και 2 παρ. 11 Ν. 3052/2002 από 1−1−2003", "ΔΕΛΤ.ΠΟΣ.ΠΑΡ.", "ΔΕΛΤΙΟ ΠΟΣΟΤΙΚΗΣ ΠΑΡΑΛΑΒΗΣ", "0" },
                    { 54, "ΠΟΛ. 176/77", "ΑΠΟΔ.ΑΣΦ.", "ΑΠΟΔΕΙΞΗ ΑΣΦΑΛΙΣΤΡΩΝ", "+" },
                    { 158, "11 παρ. 1", "Δ.Α ", "ΔΕΛΤΙΟ ΑΠΟΣΤΟΛΗΣ", "0" },
                    { 162, "12 παρ. 1, 2", "Τ.Π.Υ", "ΤΙΜΟΛΟΓΙΟ (Παροχή Υπηρεσιών)", "+" },
                    { 165, "2 μικτή χρήση", "ΤΙΜ.", "ΤΙΜΟΛΟΓΙΟ", "+" },
                    { 169, "12 παρ. 13", "Π.Τ", "ΠΙΣΤΩΤΙΚΟ ΤΙΜΟΛΟΓΙΟ", "-" },
                    { 173, "13 παρ. 1−3", "Α.Λ.Π", "ΑΠΟΔΕΙΞΗ ΛΙΑΝΙΚΗΣ ΠΩΛΗΣΗΣ", "+" },
                    { 174, "13 παρ. 1−3 ", "Α.Π.Υ", "ΑΠΟΔΕΙΞΗ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ", "+" },
                    { 175, "13 παρ. 1", "Α.ΕΠΙΣΤΡ", "ΑΠΟΔΕΙΞΗ ΕΠΙΣΤΡΟΦΗΣ", "-" },
                    { 215, "Άρθρο 23 παρ. 5", "ΕΙΔ.ΑΚ.ΣΤ.", "ΕΙΔΙΚΟ ΑΚΥΡΩΤΙΚΟ ΣΤΟΙΧΕΙΟ", " " }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyDataErrors_MyDataResponseId",
                table: "MyDataErrors",
                column: "MyDataResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoices_InvoiceTypeCode",
                table: "MyDataInvoices",
                column: "InvoiceTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyDataResponses_MyDataInvoiceId",
                table: "MyDataResponses",
                column: "MyDataInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyDataErrors");

            migrationBuilder.DropTable(
                name: "MyDataResponses");

            migrationBuilder.DropTable(
                name: "MyDataInvoices");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceTypes");
        }
    }
}
