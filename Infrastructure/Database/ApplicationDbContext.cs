using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<MyDataInvoice> MyDataInvoices { get; set; }
        public DbSet<MyDataResponse> MyDataResponses { get; set; }
        public DbSet<MyDataError> MyDataErrors { get; set; }
        public DbSet<MyDataInvoiceType> MyDataInvoiceTypes { get; set; }
        public DbSet<MyDataCancelationError> MyDataCancellationErrors { get; set; }
        public DbSet<MyDataCancelationResponse> MyDataCancellationResponses { get; set; }
        public DbSet<MyDataCancelInvoice> MyDataCancelInvoices { get; set; }

        public DbSet<MyDataIncome> MyDataIncomes { get; set; }
        public DbSet<MyDataIncomeResponse> MyDataIncomeResponses { get; set; }
        public DbSet<MyDataIncomeError> MyDataIncomeErrors { get; set; }



        public DbSet<MyDataTransmittedDocInvoice> MyDataTransmittedDocInvoices { get; set; }
        public DbSet<MyDataInvoiceHeaderType> MyDataInvoiceHeaderTypes { get; set; }
        public DbSet<MyDataPartyType> MyDataPartyTypes { get; set; }
        public DbSet<MyDataPaymentMethodDetail> MyDataPaymentMethodDetails { get; set; }
        public DbSet<MyDataInvoiceDetails> MyDataInvoiceDetails { get; set; }
        public DbSet<MyDataInvoiceSummary> MyDataInvoiceSummary { get; set; }
        public DbSet<MyDataAddressType> MyDataAddressType { get; set; }
        public DbSet<MyDataInvoiceRowType> MyDataInvoiceRowType { get; set; }
        public DbSet<MyDataTaxes> MyDataTaxes { get; set; }
        public DbSet<MyDataIncomeClassification> MyDataIncomeClassifications { get; set; }
        public DbSet<MyDataExpensesClassification> MyDataExpensesClassifications { get; set; }
        public DbSet<MyDataInvoiceExpensesClassificationType> MyDataInvoiceExpensesClassificationTypes { get; set; }
        public DbSet<MyDataCancelledInvoicesDoc> MyDataCancelledInvoicesDocs { get; set; }
        public DbSet<MyDataExpenseType> MyDataExpenseTypes { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetSystemEntityModels(builder);
            SeedData(builder);
        }
        private void SetSystemEntityModels(ModelBuilder builder)
        {
            builder.Entity<MyDataInvoice>()
                .HasMany(p => p.MyDataResponses)
                .WithOne(p => p.MyDataInvoice)
                .HasForeignKey(p => p.MyDataInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MyDataInvoice>()
                .HasMany(p => p.MyDataCancelationResponses)
                .WithOne(p => p.MyDataInvoice)
                .HasForeignKey(p => p.MyDataInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MyDataInvoice>()
                .HasOne(p => p.InvoiceType)
                .WithMany()
                .HasForeignKey(p => p.InvoiceTypeCode)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MyDataResponse>()
                .HasMany(p => p.Errors)
                .WithOne(p => p.MyDataResponse)
                .HasForeignKey(p => p.MyDataResponseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MyDataCancelationResponse>()
                .HasMany(p => p.Errors)
                .WithOne(p => p.MyDataCancelationResponse)
                .HasForeignKey(p => p.MyDataCancelationResponseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MyDataCancelInvoice>().Property(x => x.Id).HasDefaultValueSql("NEWID()");


            builder.Entity<MyDataIncome>()
                .HasMany(p => p.MyDataIncomeResponses)
                .WithOne(p => p.MyDataIncome)
                .HasForeignKey(p => p.MyDataIncomeId)
                .OnDelete(DeleteBehavior.Cascade); 
            builder.Entity<MyDataIncomeResponse>()
                .HasMany(p => p.Errors)
                .WithOne(p => p.MyIncomeDataResponse)
                .HasForeignKey(p => p.MyDataIncomeResponseId)
                .OnDelete(DeleteBehavior.Cascade);


            //builder.Entity<MyDataTransmittedDocInvoice>()
            //    .HasMany(p => p.issuer)
            //    .WithOne(p => p.MyDataDocIssuerInvoice)
            //    .HasForeignKey(p => p.MyDataDocIssuerInvoiceId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<MyDataTransmittedDocInvoice>()
            //   .HasMany(p => p.counterpart)
            //   .WithOne(p => p.MyDataDocEncounterInvoice)
            //   .HasForeignKey(p => p.MyDataDocEncounterInvoiceId)
            //   .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<MyDataPartyType>()
                .HasOne(p => p.MyDataDocIssuerInvoice)
                .WithMany(p => p.issuer)
                .HasForeignKey(p => p.MyDataDocIssuerInvoiceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MyDataPartyType>()
               .HasOne(p => p.MyDataDocEncounterInvoice)
                .WithMany(p => p.counterpart)
                .HasForeignKey(p => p.MyDataDocEncounterInvoiceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MyDataTransmittedDocInvoice>()
                .HasOne(p => p.invoiceHeaderType)
                .WithOne(p => p.MyDataDocInvoice)
                .HasForeignKey<MyDataInvoiceHeaderType>(p => p.MyDataDocInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<MyDataTransmittedDocInvoice>()
                .HasMany(p => p.paymentMethodDetailType)
                .WithOne(p => p.MyDataDocInvoice)
                .HasForeignKey(p => p.MyDataDocInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyDataTransmittedDocInvoice>()
                .HasMany(p => p.invoiceDetails)
                .WithOne(p => p.MyDataDocInvoice)
                .HasForeignKey(p => p.MyDataDocInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyDataTransmittedDocInvoice>()
                .HasMany(p => p.taxesTotals)
                .WithOne(p => p.MyDataDocInvoice)
                .HasForeignKey(p => p.MyDataDocInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyDataTransmittedDocInvoice>()
                .HasOne(p => p.invoiceSummary)
                .WithOne(p => p.MyDataDocInvoice)
                .HasForeignKey<MyDataInvoiceSummary>(p => p.MyDataDocInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyDataPartyType>()
                .HasOne(p => p.address)
                .WithOne(p => p.MyDataPartyType)
                .HasForeignKey<MyDataAddressType>(p => p.MyDataPartyTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyDataInvoiceRowType>()
                .HasOne(p => p.incomeClassification)
                .WithOne(p => p.MyDataInvoiceDocRowType)
                .HasForeignKey<MyDataIncomeClassification>(p => p.MyDataInvoiceDetailsId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<MyDataInvoiceType>(b =>
            {
                b.HasData(
                    new MyDataInvoiceType
                    {
                        Code = 40,
                        Title = "ΔΕΛΤΙΟ ΠΟΣΟΤΙΚΗΣ ΠΑΡΑΛΑΒΗΣ",
                        ShortTitle = "ΔΕΛΤ.ΠΟΣ.ΠΑΡ.",
                        Description = "10 παρ. 1 Π.Δ. 186/92 και 2 παρ. 11 Ν. 3052/2002 από 1−1−2003",
                        sign = '0'
                    }, new MyDataInvoiceType
                    {
                        Code = 54,
                        Title = "ΑΠΟΔΕΙΞΗ ΑΣΦΑΛΙΣΤΡΩΝ",
                        ShortTitle = "ΑΠΟΔ.ΑΣΦ.",
                        Description = "ΠΟΛ. 176/77",
                        sign = '+'
                    }, new MyDataInvoiceType
                    {
                        Code = 158,
                        Title = "ΔΕΛΤΙΟ ΑΠΟΣΤΟΛΗΣ",
                        ShortTitle = "Δ.Α ",
                        Description = "11 παρ. 1",
                        sign = '0'
                    }, new MyDataInvoiceType
                    {
                        Code = 162,
                        Title = "ΤΙΜΟΛΟΓΙΟ (Παροχή Υπηρεσιών)",
                        ShortTitle = "Τ.Π.Υ",
                        Description = "12 παρ. 1, 2",
                        sign = '+'
                    }, new MyDataInvoiceType
                    {
                        Code = 165,
                        Title = "ΤΙΜΟΛΟΓΙΟ",
                        ShortTitle = "ΤΙΜ.",
                        Description = "2 μικτή χρήση",
                        sign = '+'
                    }, new MyDataInvoiceType
                    {
                        Code = 169,
                        Title = "ΠΙΣΤΩΤΙΚΟ ΤΙΜΟΛΟΓΙΟ",
                        ShortTitle = "Π.Τ",
                        Description = "12 παρ. 13",
                        sign = '-'
                    }, new MyDataInvoiceType
                    {
                        Code = 173,
                        Title = "ΑΠΟΔΕΙΞΗ ΛΙΑΝΙΚΗΣ ΠΩΛΗΣΗΣ",
                        ShortTitle = "Α.Λ.Π",
                        Description = "13 παρ. 1−3",
                        sign = '+'
                    }, new MyDataInvoiceType
                    {
                        Code = 174,
                        Title = "ΑΠΟΔΕΙΞΗ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ",
                        ShortTitle = "Α.Π.Υ",
                        Description = "13 παρ. 1−3 ",
                        sign = '+'
                    }, new MyDataInvoiceType
                    {
                        Code = 175,
                        Title = "ΑΠΟΔΕΙΞΗ ΕΠΙΣΤΡΟΦΗΣ",
                        ShortTitle = "Α.ΕΠΙΣΤΡ",
                        Description = "13 παρ. 1",
                        sign = '-'
                    }, new MyDataInvoiceType
                    {
                        Code = 215,
                        Title = "ΕΙΔΙΚΟ ΑΚΥΡΩΤΙΚΟ ΣΤΟΙΧΕΙΟ",
                        ShortTitle = "ΕΙΔ.ΑΚ.ΣΤ.",
                        Description = "Άρθρο 23 παρ. 5",
                        sign = ' '
                    }

                );
            });
        }
    }
}
