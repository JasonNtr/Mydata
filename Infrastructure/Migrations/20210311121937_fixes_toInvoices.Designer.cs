﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210311121937_fixes_toInvoices")]
    partial class fixes_toInvoices
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Model.MyDataCancelInvoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<long?>("Uid")
                        .HasColumnType("bigint");

                    b.Property<long?>("invoiceMark")
                        .HasColumnType("bigint");

                    b.Property<bool>("invoiceProcessed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("MyDataCancelInvoices");
                });

            modelBuilder.Entity("Domain.Model.MyDataCancelationError", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MyDataCancelationResponseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MyDataCancelationResponseId");

                    b.ToTable("MyDataCancelationErrors");
                });

            modelBuilder.Entity("Domain.Model.MyDataCancelationResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MyDataInvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("cancellationMark")
                        .HasColumnType("bigint");

                    b.Property<string>("statusCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MyDataInvoiceId");

                    b.ToTable("MyDataCancelationResponses");
                });

            modelBuilder.Entity("Domain.Model.MyDataError", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MyDataResponseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MyDataResponseId");

                    b.ToTable("MyDataErrors");
                });

            modelBuilder.Entity("Domain.Model.MyDataInvoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("CancellationMark")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("InvoiceNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("InvoiceTypeCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("StoredXml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Uid")
                        .HasColumnType("bigint");

                    b.Property<string>("VAT")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceTypeCode");

                    b.ToTable("MyDataInvoices");
                });

            modelBuilder.Entity("Domain.Model.MyDataInvoiceType", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sign")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("Code");

                    b.ToTable("MyDataInvoiceTypes");

                    b.HasData(
                        new
                        {
                            Code = 40,
                            Description = "10 παρ. 1 Π.Δ. 186/92 και 2 παρ. 11 Ν. 3052/2002 από 1−1−2003",
                            ShortTitle = "ΔΕΛΤ.ΠΟΣ.ΠΑΡ.",
                            Title = "ΔΕΛΤΙΟ ΠΟΣΟΤΙΚΗΣ ΠΑΡΑΛΑΒΗΣ",
                            sign = "0"
                        },
                        new
                        {
                            Code = 54,
                            Description = "ΠΟΛ. 176/77",
                            ShortTitle = "ΑΠΟΔ.ΑΣΦ.",
                            Title = "ΑΠΟΔΕΙΞΗ ΑΣΦΑΛΙΣΤΡΩΝ",
                            sign = "+"
                        },
                        new
                        {
                            Code = 158,
                            Description = "11 παρ. 1",
                            ShortTitle = "Δ.Α ",
                            Title = "ΔΕΛΤΙΟ ΑΠΟΣΤΟΛΗΣ",
                            sign = "0"
                        },
                        new
                        {
                            Code = 162,
                            Description = "12 παρ. 1, 2",
                            ShortTitle = "Τ.Π.Υ",
                            Title = "ΤΙΜΟΛΟΓΙΟ (Παροχή Υπηρεσιών)",
                            sign = "+"
                        },
                        new
                        {
                            Code = 165,
                            Description = "2 μικτή χρήση",
                            ShortTitle = "ΤΙΜ.",
                            Title = "ΤΙΜΟΛΟΓΙΟ",
                            sign = "+"
                        },
                        new
                        {
                            Code = 169,
                            Description = "12 παρ. 13",
                            ShortTitle = "Π.Τ",
                            Title = "ΠΙΣΤΩΤΙΚΟ ΤΙΜΟΛΟΓΙΟ",
                            sign = "-"
                        },
                        new
                        {
                            Code = 173,
                            Description = "13 παρ. 1−3",
                            ShortTitle = "Α.Λ.Π",
                            Title = "ΑΠΟΔΕΙΞΗ ΛΙΑΝΙΚΗΣ ΠΩΛΗΣΗΣ",
                            sign = "+"
                        },
                        new
                        {
                            Code = 174,
                            Description = "13 παρ. 1−3 ",
                            ShortTitle = "Α.Π.Υ",
                            Title = "ΑΠΟΔΕΙΞΗ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ",
                            sign = "+"
                        },
                        new
                        {
                            Code = 175,
                            Description = "13 παρ. 1",
                            ShortTitle = "Α.ΕΠΙΣΤΡ",
                            Title = "ΑΠΟΔΕΙΞΗ ΕΠΙΣΤΡΟΦΗΣ",
                            sign = "-"
                        },
                        new
                        {
                            Code = 215,
                            Description = "Άρθρο 23 παρ. 5",
                            ShortTitle = "ΕΙΔ.ΑΚ.ΣΤ.",
                            Title = "ΕΙΔΙΚΟ ΑΚΥΡΩΤΙΚΟ ΣΤΟΙΧΕΙΟ",
                            sign = " "
                        });
                });

            modelBuilder.Entity("Domain.Model.MyDataResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MyDataInvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("authenticationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("index")
                        .HasColumnType("int");

                    b.Property<long?>("invoiceMark")
                        .HasColumnType("bigint");

                    b.Property<string>("invoiceUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("statusCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MyDataInvoiceId");

                    b.ToTable("MyDataResponses");
                });

            modelBuilder.Entity("Domain.Model.MyDataCancelationError", b =>
                {
                    b.HasOne("Domain.Model.MyDataCancelationResponse", "MyDataCancelationResponse")
                        .WithMany("Errors")
                        .HasForeignKey("MyDataCancelationResponseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Model.MyDataCancelationResponse", b =>
                {
                    b.HasOne("Domain.Model.MyDataInvoice", "MyDataInvoice")
                        .WithMany("MyDataCancelationResponses")
                        .HasForeignKey("MyDataInvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Model.MyDataError", b =>
                {
                    b.HasOne("Domain.Model.MyDataResponse", "MyDataResponse")
                        .WithMany("Errors")
                        .HasForeignKey("MyDataResponseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Model.MyDataInvoice", b =>
                {
                    b.HasOne("Domain.Model.MyDataInvoiceType", "InvoiceType")
                        .WithMany()
                        .HasForeignKey("InvoiceTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Model.MyDataResponse", b =>
                {
                    b.HasOne("Domain.Model.MyDataInvoice", "MyDataInvoice")
                        .WithMany("MyDataResponses")
                        .HasForeignKey("MyDataInvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
