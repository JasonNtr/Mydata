using Domain.DTO;
using Mydata.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Business.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Business.ApiServices;
using Domain.AADE;
using Mydata.UiModels;


namespace Mydata.ViewModels
{
    public class InvoicesViewModel : ViewModelBase
    {
        private List<string> _currentFiles;
        private bool _readyToLoad = true;
        private List<ParticleDTO> _listOfInvoice;
        private bool _isFirstTimeDelay = true;
        private readonly string _connectionString;
        
        private readonly IOptions<AppSettings> _appSettings;
        
        public InvoicesViewModel(IOptions<AppSettings> appSettings, string connectionString)
        {
            _appSettings=appSettings;
            _connectionString= connectionString;
           
            _currentFiles = new List<string>();
            Init();
        }

        private async Task Init()
        {
            await GetParticles();
            await LoadInvoices();
           
        }
        public async Task LoadInvoices()
        {
            MyDataInvoiceDTOs.Clear();
            var invoiceRepo = new InvoiceRepo(_connectionString);
            var listOfInvoice = await invoiceRepo.GetList(DateFrom, DateTo);
            foreach (var myDataInvoiceDto in listOfInvoice)
            {
                MyDataInvoiceDTOs.Add(myDataInvoiceDto);
            }
        }
        private async Task GetParticles()
        {
            Particles = new ObservableCollection<DataGridParticle>();
            var particleRepo = new ParticleRepo(_connectionString);
            _listOfInvoice = await particleRepo.GetParticlesBetweenDates(DateFrom,DateTo);
            if(_listOfInvoice.Count>0) CreateDataGridParticles();


            
        }

        private void CreateDataGridParticles()
        {
            foreach (var invoice in _listOfInvoice)
            {
                var particle = new DataGridParticle();
                var guid = Guid.NewGuid();
                invoice.DataGridId = guid;
                
                particle.DataGridId = guid;
                particle.Date = invoice.Date.ToShortDateString();
                particle.Branch =invoice.Branch + " " + invoice.BranchDTO.CityName;
                particle.Series = invoice.Series;
                particle.Client = invoice.Client.Name;
                particle.PtyParDescription = invoice.Ptyppar.Code + " - " + invoice.Ptyppar.Description;
                particle.Amount = invoice.Amount.ToString("c");

                Particles.Add(particle);
            }
        }

        private string CreateInvoiceDocXml(List<ParticleDTO> particlesDTO)
        {
            var doc = new InvoicesDoc();
            var list = new List<InvoicesDocInvoice>();

            foreach (var particleDTO in particlesDTO)
            {
                var invoice = new InvoicesDocInvoice();
                var type = decimal.Parse(particleDTO.Ptyppar.EID_PARAST, CultureInfo.InvariantCulture);
                var issuer = new InvoicesDocInvoiceIssuer
                {
                    vatNumber = "157395112", //particleDTO.BranchDTO?.VatNumber,  
                    country = "GR",
                    branch = 0
                };
                var issuerPartAddress = new InvoicesDocInvoiceIssuerAddress
                {
                    postalCode = particleDTO.BranchDTO?.ZipCode,
                    city = particleDTO.BranchDTO?.CityName
                };
                issuer.address = issuerPartAddress;
                invoice.issuer = issuer;
                if (type < 11 || type > 12)
                {
                    var counterPart = new InvoicesDocInvoiceCounterpart
                    {
                        vatNumber = particleDTO.Client?.VatNumber.Trim(),
                        country = "GR",
                        branch = 0
                    };
                    var counterPartAddress = new InvoicesDocInvoiceCounterpartAddress
                    {
                        postalCode = particleDTO.Client?.CLIENT_ZIPCODE ?? "",
                        city = particleDTO.Client?.City.Name
                    };
                  
                    counterPart.address = counterPartAddress;
                    invoice.counterpart = counterPart;
                }
                
                var paymentMethod = new InvoicesDocInvoicePaymentMethodDetails
                {
                    type = (int)particleDTO.Paymentmethod,
                    amount = particleDTO.Amount
                };
                var paymentMethods = new List<InvoicesDocInvoicePaymentMethodDetails>();
                paymentMethods.Add(paymentMethod);
                invoice.paymentMethods = paymentMethods.ToArray();

                var header = new InvoicesDocInvoiceInvoiceHeader
                {
                    series = particleDTO.Series,
                    aa = particleDTO.Number.ToString(CultureInfo.InvariantCulture),
                    issueDate = particleDTO.Date,
                    invoiceType = type,
                    currency = "EUR",
                    movePurpose = 1,

                };
                if (header.series.Equals(".")) header.series = "0";
                invoice.invoiceHeader = header;


                decimal totalWithheldAmount = 0;
                decimal totalStampDutyAmount = 0;
                var incomeClassificationSummary = new List<InvoicesDocInvoiceInvoiceSummaryIncomeClassification>();
                var details = new List<InvoicesDocInvoiceInvoiceDetails>();
                var i = 1;
                foreach (var item in particleDTO.Pmoves)
                {
                    var incomeClassifications = new List<InvoicesDocInvoiceInvoiceDetailsIncomeClassification>();
                    var incomeClassification = new InvoicesDocInvoiceInvoiceDetailsIncomeClassification
                    {
                        classificationType = particleDTO.Ptyppar.TYPOS_XARAKTHR,
                        classificationCategory = item.Item.KATHG_XARAKTHR,
                        amount = item.PMS_AMAFTDISC
                    };
                    if (!incomeClassificationSummary.Any(x =>
                            x.classificationType.Equals(incomeClassification.classificationType)
                            && x.classificationCategory.Equals(incomeClassification.classificationCategory)))
                    {
                        incomeClassificationSummary.Add(new InvoicesDocInvoiceInvoiceSummaryIncomeClassification
                        {
                            classificationType = incomeClassification.classificationType,
                            classificationCategory = incomeClassification.classificationCategory,
                            amount = incomeClassification.amount,
                        });
                    }
                    else
                    {
                        var sum = incomeClassificationSummary.FirstOrDefault((x =>
                            x.classificationType.Equals(incomeClassification.classificationType)
                            && x.classificationCategory.Equals(incomeClassification.classificationCategory)));
                        sum.amount += incomeClassification.amount;
                    }
                    incomeClassifications.Add(incomeClassification);
                    var detail = new InvoicesDocInvoiceInvoiceDetails
                    {
                        lineNumber = (uint)i,
                        netValue = item.PMS_AMAFTDISC,
                        vatCategory = (int)item.Item.FPA.FpaCategory,
                        vatAmount = item.PMS_VATAM,
                        stampDutyAmount = item.POSO_XARTOSH,
                        incomeClassification = incomeClassifications.ToArray(),
                        measurementUnit = 1
                    };

                    details.Add(detail);
                    totalWithheldAmount += item.POSO_PARAKRAT;
                    totalStampDutyAmount += item.POSO_XARTOSH;
                    i++;
                }

                invoice.invoiceDetails = details.ToArray();


                var invoicesDocInvoiceInvoiceSummary = new InvoicesDocInvoiceInvoiceSummary
                {
                    totalNetValue = particleDTO.TotalNetAmount,
                    totalVatAmount = particleDTO.TotalVatAmount,
                    totalWithheldAmount = totalWithheldAmount,
                    totalFeesAmount = (long)particleDTO.TotalOtherTaxesAmount,
                    totalStampDutyAmount = (long)totalStampDutyAmount,
                    totalOtherTaxesAmount = (long)particleDTO.TotalOtherTaxesAmount,
                    totalDeductionsAmount = (long)particleDTO.TotalDeductionsAmount,
                    totalGrossValue = particleDTO.TotalNetAmount + particleDTO.TotalVatAmount,
                    incomeClassification = incomeClassificationSummary.ToArray()

                };

                var x = (long)particleDTO.TotalVatAmount;
                var c = (long)particleDTO.TotalNetAmount;
                invoice.invoiceSummary = invoicesDocInvoiceInvoiceSummary;
                list.Add(invoice);
            }


            doc.invoice = list.ToArray();
            var xml = "";
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(doc.GetType());
                serializer.Serialize(stringwriter, doc);
                xml= stringwriter.ToString();
            }
           
            return xml;
        }

        private async void Reload()
        {
            IsBusy = true;
            GridEnabled = false;
            await LoadInvoices();
            await GetParticles();
            IsBusy = false;
            GridEnabled = true;
        }

        private  void SendParticles()
        {
            var particleIds = SelectedParticles.Select(x=>x.DataGridId).ToList();
            
            var invoices = _listOfInvoice.Where(x => particleIds.Contains(x.DataGridId)).ToList();
            DoTransferConversion(invoices);
            
        }

        private  void SendBatchParticles()
        {
            var particleIds = Particles.Select(x => x.DataGridId).ToList();

            var invoices = _listOfInvoice.Where(x => particleIds.Contains(x.DataGridId)).ToList();

            DoTransferConversion(invoices);
        }

        private async void DoTransferConversion(List<ParticleDTO> invoices)
        {
            IsBusy = true;
            GridEnabled = false;

            var postInvoices = invoices.Where(x => x.Ptyppar.IsForCancellation == null || x.Ptyppar.IsForCancellation.Equals("0")).ToList();
            var cancelInvoices = invoices.Where(x => x.Ptyppar.IsForCancellation != null && x.Ptyppar.IsForCancellation.Equals("1")).ToList();
            var taxInvoiceRepo = new TaxInvoiceRepo(_connectionString);
            var particleRepo = new ParticleRepo(_connectionString);
            var postTransferModel = new MyDataInvoiceTransferModel();
            var cancelTransferModel = new MyDataInvoiceTransferModel();
            foreach (var item in postInvoices)
            {
                var typeCode = await taxInvoiceRepo.GetTaxCode(item.Ptyppar?.Code);
                var myDataInvoice = new MyDataInvoiceDTO();
                myDataInvoice.Uid = (long?)item.Rec0;
                myDataInvoice.InvoiceNumber = (long?)item.Number;
                myDataInvoice.InvoiceDate =  item.Date;
                myDataInvoice.VAT = item.Client?.VatNumber.Trim();
                myDataInvoice.InvoiceTypeCode = (int)typeCode;
                myDataInvoice.Particle = item;
                postTransferModel.MyDataInvoices.Add(myDataInvoice);
            }

            

            var postXml = CreateInvoiceDocXml(postInvoices);
           
            postTransferModel.Xml = postXml;

            var invoiceService = new InvoiceService(_appSettings, _connectionString);
            if(postTransferModel.MyDataInvoices.Count>0)
                 await invoiceService.PostActionNew(postTransferModel);


            foreach (var item in cancelInvoices)
            {
                var particleToBecancelled = await particleRepo.GetCancel(item.PARTL_RECR);
                if (particleToBecancelled.Mark == null) break;
                var myDataInvoice = new MyDataCancelInvoiceDTO();
                myDataInvoice.Uid = (long?)item.Rec0;
                myDataInvoice.ParticleToBeCancelledMark = long.Parse(particleToBecancelled.Mark);
                myDataInvoice.Particle = item;
                cancelTransferModel.MyCancelDataInvoices.Add(myDataInvoice);
            }

            if (cancelTransferModel.MyCancelDataInvoices.Count > 0)
                await invoiceService.CancelActionNew(cancelTransferModel);
            Reload();
            IsBusy = false;
            GridEnabled = true;
        }


        #region Commands
        private ICommand _reloadCommand;
        public ICommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (_reloadCommand = new CommandHandler(() => Reload(), () => true));
            }
        }
        private ICommand _sendParticlesCommand;
        public ICommand SendParticlesCommand
        {
            get
            {
                return _sendParticlesCommand ?? (_sendParticlesCommand = new CommandHandler(() => SendParticles(), () => true));
            }
        }
        private ICommand _sendBatchParticlesCommand;
        public ICommand SendBatchParticlesCommand
        {
            get
            {
                return _sendBatchParticlesCommand ?? (_sendBatchParticlesCommand = new CommandHandler(() => SendBatchParticles(), () => true));
            }
        }

     
        #endregion


        #region Properties
        private bool _gridEnabled = true;
        public bool GridEnabled
        {
            get
            {
                return _gridEnabled;
            }
            set
            {
                _gridEnabled = value;
                OnPropertyChanged("GridEnabled");
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private ObservableCollection<DataGridParticle> _particles = new ObservableCollection<DataGridParticle>();
        public ObservableCollection<DataGridParticle> Particles
        {
            get
            {
                return _particles;
            }
            set
            {
                _particles = value;
                OnPropertyChanged("Particles");
            }
        }

        private ObservableCollection<DataGridParticle> _selectedParticles;
        public ObservableCollection<DataGridParticle> SelectedParticles
        {
            get
            {
                return _selectedParticles;
            }
            set
            {
                _selectedParticles = value;
                if (_selectedParticles.Count > 0)
                    SendEnabled = true;
                else 
                    SendEnabled = false;
                OnPropertyChanged("SelectedParticles");
            }
        }

        private ObservableCollection<MyDataInvoiceDTO> _myDataInvoiceDtos = new ObservableCollection<MyDataInvoiceDTO>();
        public ObservableCollection<MyDataInvoiceDTO> MyDataInvoiceDTOs
        {
            get
            {
                return _myDataInvoiceDtos;
            }
            set
            {
                _myDataInvoiceDtos = value;
                OnPropertyChanged("MyDataInvoiceDTOs");
            }
        }
        private ObservableCollection<MyGenericErrorsDTO> _mydataErrorDTOs = new ObservableCollection<MyGenericErrorsDTO>();
        public ObservableCollection<MyGenericErrorsDTO> MyDataErrorDTOs
        {
            get
            {
                return _mydataErrorDTOs;
            }
            set
            {
                _mydataErrorDTOs = value;
                OnPropertyChanged("MyDataErrorDTOs");
            }
        }


        private DateTime _dateFrom = DateTime.UtcNow.ToLocalTime();
        public DateTime DateFrom
        {
            get
            {
                return _dateFrom;
            }
            set
            {
                _dateFrom = value;
                
                OnPropertyChanged("DateFrom");
            }
        }

        private DateTime _dateTo= DateTime.UtcNow.ToLocalTime();
        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                _dateTo = value;
               
                OnPropertyChanged("DateTo");
            }
        }

        private bool _sendEnabled = false;
        public bool SendEnabled
        {
            get
            {
                return _sendEnabled;
            }
            set
            {
                _sendEnabled = value;
               
                OnPropertyChanged("SendEnabled");
            }
        }
        
        #endregion 
    }
}
