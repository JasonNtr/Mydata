using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class StrLengths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyDataCancelInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Uid = table.Column<long>(nullable: true),
                    invoiceMark = table.Column<long>(nullable: true),
                    invoiceProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataCancelInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyDataCancelledInvoicesDocs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    invoiceMark = table.Column<long>(nullable: true),
                    cancellationMark = table.Column<long>(nullable: true),
                    cancellationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataCancelledInvoicesDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyDataExpenseTypes",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ShortTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    sign = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataExpenseTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MyDataIncomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Uid = table.Column<long>(nullable: true),
                    IncomeDate = table.Column<DateTime>(nullable: true),
                    IncomeNumber = table.Column<long>(nullable: true),
                    VAT = table.Column<string>(nullable: true),
                    IncomeTypeCode = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    StoredXml = table.Column<string>(nullable: true),
                    CancellationMark = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataIncomes", x => x.Id);
                });

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
                name: "MyDataTransmittedDocInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Uid = table.Column<string>(maxLength: 100, nullable: true),
                    authenticationCode = table.Column<string>(maxLength: 100, nullable: true),
                    mark = table.Column<long>(nullable: true),
                    cancelledByMark = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataTransmittedDocInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyDataIncomeResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataIncomeId = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: true),
                    StatusCode = table.Column<string>(nullable: true),
                    IncomeUid = table.Column<string>(nullable: true),
                    IncomeMark = table.Column<long>(nullable: true),
                    AuthenticationCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataIncomeResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataIncomeResponses_MyDataIncomes_MyDataIncomeId",
                        column: x => x.MyDataIncomeId,
                        principalTable: "MyDataIncomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    StoredXml = table.Column<string>(nullable: true),
                    CancellationMark = table.Column<long>(nullable: true)
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
                name: "MyDataExpensesClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: true),
                    optionalId = table.Column<int>(nullable: true),
                    classificationType = table.Column<string>(nullable: true),
                    classificationCategory = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataExpensesClassifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataExpensesClassifications_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MyDataInvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: false),
                    lineNumber = table.Column<int>(nullable: false),
                    netValue = table.Column<double>(nullable: false),
                    vatCategory = table.Column<string>(nullable: true),
                    vatAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataInvoiceDetails_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataInvoiceHeaderTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: false),
                    series = table.Column<string>(maxLength: 50, nullable: true),
                    aa = table.Column<string>(maxLength: 50, nullable: true),
                    issueDate = table.Column<DateTime>(nullable: false),
                    invoiceType = table.Column<string>(maxLength: 20, nullable: true),
                    vatPaymentSuspension = table.Column<bool>(nullable: true),
                    currency = table.Column<string>(maxLength: 20, nullable: true),
                    exchangeRate = table.Column<double>(nullable: true),
                    correlatedInvoices = table.Column<long>(nullable: true),
                    selfPricing = table.Column<bool>(nullable: true),
                    dispatchDate = table.Column<DateTime>(nullable: true),
                    dispatchTime = table.Column<string>(maxLength: 50, nullable: true),
                    vehicleNumber = table.Column<string>(maxLength: 50, nullable: true),
                    movePurpose = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoiceHeaderTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataInvoiceHeaderTypes_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataInvoiceRowType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: true),
                    lineNumber = table.Column<int>(nullable: false),
                    netValue = table.Column<double>(nullable: false),
                    vatCategory = table.Column<string>(maxLength: 20, nullable: true),
                    vatAmount = table.Column<double>(nullable: false),
                    quantity = table.Column<double>(nullable: true),
                    measurementUnit = table.Column<int>(nullable: true),
                    invoiceDetailType = table.Column<int>(nullable: true),
                    vatExemptionCategory = table.Column<int>(nullable: true),
                    discountOption = table.Column<bool>(nullable: true),
                    withheldAmount = table.Column<double>(nullable: true),
                    withheldPercentCategory = table.Column<int>(nullable: true),
                    stampDutyAmount = table.Column<double>(nullable: true),
                    stampDutyPercentCategory = table.Column<int>(nullable: true),
                    feesAmount = table.Column<double>(nullable: true),
                    feesPercentCategory = table.Column<int>(nullable: true),
                    otherTaxesPercentCategory = table.Column<int>(nullable: true),
                    otherTaxesAmount = table.Column<double>(nullable: true),
                    deductionsAmount = table.Column<double>(nullable: true),
                    lineComments = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoiceRowType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataInvoiceRowType_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataInvoiceSummary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: false),
                    totalNetValue = table.Column<double>(nullable: false),
                    totalVatAmount = table.Column<double>(nullable: false),
                    totalWithheldAmounr = table.Column<double>(nullable: false),
                    totalFeesAmount = table.Column<double>(nullable: false),
                    totalStumpDutyAmount = table.Column<double>(nullable: false),
                    totalOtherTaxesAmount = table.Column<double>(nullable: false),
                    totalDeductionsAmount = table.Column<double>(nullable: false),
                    totalGrossValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoiceSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataInvoiceSummary_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataPartyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocIssuerInvoiceId = table.Column<Guid>(nullable: true),
                    MyDataDocEncounterInvoiceId = table.Column<Guid>(nullable: true),
                    vatNumber = table.Column<string>(maxLength: 50, nullable: true),
                    country = table.Column<string>(maxLength: 50, nullable: true),
                    branch = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataPartyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataPartyTypes_MyDataTransmittedDocInvoices_MyDataDocEncounterInvoiceId",
                        column: x => x.MyDataDocEncounterInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MyDataPartyTypes_MyDataTransmittedDocInvoices_MyDataDocIssuerInvoiceId",
                        column: x => x.MyDataDocIssuerInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MyDataPaymentMethodDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    amount = table.Column<double>(nullable: false),
                    paymentMethodInfo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataPaymentMethodDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataPaymentMethodDetails_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataDocInvoiceId = table.Column<Guid>(nullable: true),
                    taxType = table.Column<int>(nullable: true),
                    taxCategory = table.Column<int>(nullable: true),
                    taxunderlyingValueType = table.Column<double>(nullable: true),
                    taxAmount = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataTaxes_MyDataTransmittedDocInvoices_MyDataDocInvoiceId",
                        column: x => x.MyDataDocInvoiceId,
                        principalTable: "MyDataTransmittedDocInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataIncomeErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataIncomeResponseId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataIncomeErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataIncomeErrors_MyDataIncomeResponses_MyDataIncomeResponseId",
                        column: x => x.MyDataIncomeResponseId,
                        principalTable: "MyDataIncomeResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataCancellationResponses",
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
                    table.PrimaryKey("PK_MyDataCancellationResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataCancellationResponses_MyDataInvoices_MyDataInvoiceId",
                        column: x => x.MyDataInvoiceId,
                        principalTable: "MyDataInvoices",
                        principalColumn: "Id",
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
                name: "MyDataInvoiceExpensesClassificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    invoiceMark = table.Column<long>(nullable: false),
                    classificationMark = table.Column<long>(nullable: true),
                    transactionMode = table.Column<int>(nullable: true),
                    lineNumber = table.Column<int>(nullable: true),
                    expensesClassificationDetailDataId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataInvoiceExpensesClassificationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataInvoiceExpensesClassificationTypes_MyDataExpensesClassifications_expensesClassificationDetailDataId",
                        column: x => x.expensesClassificationDetailDataId,
                        principalTable: "MyDataExpensesClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MyDataIncomeClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataInvoiceDetailsId = table.Column<Guid>(nullable: true),
                    optionalId = table.Column<int>(nullable: true),
                    classificationType = table.Column<string>(maxLength: 50, nullable: true),
                    classificationCategory = table.Column<string>(maxLength: 50, nullable: true),
                    amount = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataIncomeClassifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataIncomeClassifications_MyDataInvoiceRowType_MyDataInvoiceDetailsId",
                        column: x => x.MyDataInvoiceDetailsId,
                        principalTable: "MyDataInvoiceRowType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataAddressType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    MyDataPartyTypeId = table.Column<Guid>(nullable: false),
                    postalCode = table.Column<string>(maxLength: 50, nullable: true),
                    city = table.Column<string>(maxLength: 50, nullable: true),
                    street = table.Column<string>(maxLength: 50, nullable: true),
                    number = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDataAddressType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataAddressType_MyDataPartyTypes_MyDataPartyTypeId",
                        column: x => x.MyDataPartyTypeId,
                        principalTable: "MyDataPartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDataCancellationErrors",
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
                    table.PrimaryKey("PK_MyDataCancellationErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDataCancellationErrors_MyDataCancellationResponses_MyDataCancelationResponseId",
                        column: x => x.MyDataCancelationResponseId,
                        principalTable: "MyDataCancellationResponses",
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
                table: "MyDataExpenseTypes",
                columns: new[] { "Code", "Description", "ShortTitle", "Title", "sign" },
                values: new object[,]
                {
                    { 270, "12 παρ. 5", "ΤΙΜ.ΑΓΟΡ", "ΤΙΜΟΛΟΓΙΟ ΑΓΟΡΑΣ", "-" },
                    { 272, "12 παρ. 5, 11 παρ. 1", "ΤΙΜ.ΑΓ.−Δ.Α.", "ΤΙΜΟΛΟΓΙΟ ΑΓΟΡΑΣ − ΔΕΛΤΙΟ ΑΠΟΣΤΟΛΗΣ", "-" },
                    { 279, "ΠΟΛ 1151/06−06−2001 (ελαιοτριβεία)", "Α.Π.Υ−Δ.Π.Π−Δ.Α", "ΑΠΟΔ. ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ − ΔΕΛ. ΠΟΣΟΤ. ΠAΡΑΛΑΒΗΣ − ΔΕΛ. ΑΠΟΣΤΟΛΗΣ", "+" },
                    { 295, null, "ΔΠΠ−ΤΙΜ ΑΓΟΡ", "ΔΕΛΤΙΟ ΠΟΣΟΤΙΚΗΣ ΠΑΡΑΛΑΒΗΣ − ΤΙΜΟΛΟΓΙΟ ΑΓΟΡΑΣ", "-" },
                    { 329, null, "ΤΠ−Τ(ΑΓ.ΑΓΡ)−ΔΑ", "ΤΙΜΟΛΟΓΙΟ (Παροχ. Υπηρ.) − ΤΙΜ. (Αγοράς Αγρ. Προϊόντων) − ΔΕΛ. ΑΠΟΣΤ", "+" }
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
                name: "IX_MyDataAddressType_MyDataPartyTypeId",
                table: "MyDataAddressType",
                column: "MyDataPartyTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyDataCancellationErrors_MyDataCancelationResponseId",
                table: "MyDataCancellationErrors",
                column: "MyDataCancelationResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataCancellationResponses_MyDataInvoiceId",
                table: "MyDataCancellationResponses",
                column: "MyDataInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataErrors_MyDataResponseId",
                table: "MyDataErrors",
                column: "MyDataResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataExpensesClassifications_MyDataDocInvoiceId",
                table: "MyDataExpensesClassifications",
                column: "MyDataDocInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataIncomeClassifications_MyDataInvoiceDetailsId",
                table: "MyDataIncomeClassifications",
                column: "MyDataInvoiceDetailsId",
                unique: true,
                filter: "[MyDataInvoiceDetailsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataIncomeErrors_MyDataIncomeResponseId",
                table: "MyDataIncomeErrors",
                column: "MyDataIncomeResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataIncomeResponses_MyDataIncomeId",
                table: "MyDataIncomeResponses",
                column: "MyDataIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoiceDetails_MyDataDocInvoiceId",
                table: "MyDataInvoiceDetails",
                column: "MyDataDocInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoiceExpensesClassificationTypes_expensesClassificationDetailDataId",
                table: "MyDataInvoiceExpensesClassificationTypes",
                column: "expensesClassificationDetailDataId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoiceHeaderTypes_MyDataDocInvoiceId",
                table: "MyDataInvoiceHeaderTypes",
                column: "MyDataDocInvoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoiceRowType_MyDataDocInvoiceId",
                table: "MyDataInvoiceRowType",
                column: "MyDataDocInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoices_InvoiceTypeCode",
                table: "MyDataInvoices",
                column: "InvoiceTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataInvoiceSummary_MyDataDocInvoiceId",
                table: "MyDataInvoiceSummary",
                column: "MyDataDocInvoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyDataPartyTypes_MyDataDocEncounterInvoiceId",
                table: "MyDataPartyTypes",
                column: "MyDataDocEncounterInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataPartyTypes_MyDataDocIssuerInvoiceId",
                table: "MyDataPartyTypes",
                column: "MyDataDocIssuerInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataPaymentMethodDetails_MyDataDocInvoiceId",
                table: "MyDataPaymentMethodDetails",
                column: "MyDataDocInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataResponses_MyDataInvoiceId",
                table: "MyDataResponses",
                column: "MyDataInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyDataTaxes_MyDataDocInvoiceId",
                table: "MyDataTaxes",
                column: "MyDataDocInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyDataAddressType");

            migrationBuilder.DropTable(
                name: "MyDataCancelInvoices");

            migrationBuilder.DropTable(
                name: "MyDataCancellationErrors");

            migrationBuilder.DropTable(
                name: "MyDataCancelledInvoicesDocs");

            migrationBuilder.DropTable(
                name: "MyDataErrors");

            migrationBuilder.DropTable(
                name: "MyDataExpenseTypes");

            migrationBuilder.DropTable(
                name: "MyDataIncomeClassifications");

            migrationBuilder.DropTable(
                name: "MyDataIncomeErrors");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceDetails");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceExpensesClassificationTypes");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceHeaderTypes");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceSummary");

            migrationBuilder.DropTable(
                name: "MyDataPaymentMethodDetails");

            migrationBuilder.DropTable(
                name: "MyDataTaxes");

            migrationBuilder.DropTable(
                name: "MyDataPartyTypes");

            migrationBuilder.DropTable(
                name: "MyDataCancellationResponses");

            migrationBuilder.DropTable(
                name: "MyDataResponses");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceRowType");

            migrationBuilder.DropTable(
                name: "MyDataIncomeResponses");

            migrationBuilder.DropTable(
                name: "MyDataExpensesClassifications");

            migrationBuilder.DropTable(
                name: "MyDataInvoices");

            migrationBuilder.DropTable(
                name: "MyDataIncomes");

            migrationBuilder.DropTable(
                name: "MyDataTransmittedDocInvoices");

            migrationBuilder.DropTable(
                name: "MyDataInvoiceTypes");
        }
    }
}
