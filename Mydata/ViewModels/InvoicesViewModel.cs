using Business.ApiServices;
using Business.Brokers;
using Business.Services;
using Domain.AADE;
using Domain.DTO;
using Microsoft.Extensions.Options;
using Mydata.Helpers;
using Mydata.UiModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Mydata.ViewModels
{
    public class InvoicesViewModel : ViewModelBase
    {
        private readonly Timer _timer;
        private ParticleBroker _broker;
        private readonly List<ParticleRetries> _retries;
        private List<string> _postsPerUnit;
        private List<ParticleDTO> _listOfIParticles;
        private List<ParticleDTO> _errorParticles;
        private readonly string _connectionString;
        private readonly IOptions<AppSettings> _appSettings;
        private bool _reloadNeeded;

        public event EventHandler<bool> TypesAndCategoriesLoaded;

        public InvoicesViewModel(IOptions<AppSettings> appSettings, string connectionString)
        {
            _appSettings = appSettings;
            _connectionString = connectionString;
            _postsPerUnit = new List<string>();
            _retries = new List<ParticleRetries>();
            _listOfIParticles = new List<ParticleDTO>();
            Init();

            _timer = new Timer();
            _timer.Elapsed += DoAutoProcedure;
            _timer.Interval = 10000;// _appSettings.Value.timerMsSeconds;
        }

        private async Task Init()
        {
            await FillTypesAndCategories();
            await LoadInvoices();
            StartEmptyBroker();
        }

        private async Task FillTypesAndCategories()
        {
            var itemRepo = new ItemRepo(_connectionString);
            var categories = await itemRepo.GetCategories();
            Categories = new ObservableCollection<string>(categories);

            TypesAndCategoriesLoaded?.Invoke(this, true);
        }

        private void StartBroker(List<ParticleDTO> particleDTOs)
        {
            _broker = new ParticleBroker(_connectionString, DateFrom, DateTo, particleDTOs);
            _broker.Start();
            _broker.ProductionChangeHappened += ParticleChange;
            _reloadNeeded = false;
        }

        private void StartEmptyBroker()
        {
            _broker = new ParticleBroker(_connectionString, DateFrom, DateTo);
            _broker.Start();
            _broker.ProductionChangeHappened += ParticleChange;
            _reloadNeeded = false;
        }

        private async void Reload()
        {
            IsBusy = true;
            GridEnabled = false;
            await LoadInvoices();
            var particleRepo = new ParticleRepo(_connectionString);
            var particles = await particleRepo.GetParticlesBetweenDates(DateFrom, DateTo);
            _listOfIParticles = particles;
            if (_listOfIParticles.Count > 0) CreateDataGridParticles();
            if (_reloadNeeded)
            {
                _broker.Stop();
                StartBroker(particles);
            }
            IsBusy = false;
            GridEnabled = true;
        }

        private void ParticleChange(object sender, bool e)
        {
            IsBusy = true;
            GridEnabled = false;
            _listOfIParticles = _broker.Particles;
            if (_listOfIParticles.Count > 0) CreateDataGridParticles();
            IsBusy = false;
            GridEnabled = true;
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

        private void StartAutoProcedure()
        {
            AutoProcedureExecute();
            _timer.Start();
            SendEnabled = false;
        }

        private void StopAutoProcedure()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void DoAutoProcedure(object sender, ElapsedEventArgs e)
        {
            var needs = CheckIfCalendarNeedsChange();
            if (needs)
            {
                DateTo = DateTime.UtcNow.ToLocalTime();
                return;
            }
            AutoProcedureExecute();
        }

        private bool CheckIfCalendarNeedsChange()
        {
            var timespan = DateTime.UtcNow.Subtract(DateTo);
            var dayDifference = timespan.Days;
            return dayDifference > 0;
        }

        private void AutoProcedureExecute()
        {
            var invoices = new List<ParticleDTO>();
            foreach (var particle in _listOfIParticles)
            {
                var exist = _retries.Any(x => x.ParticleRec0 == particle.Rec0);
                if (!exist) invoices.Add(particle);
                else
                {
                    var retry = _retries.FirstOrDefault(x => x.ParticleRec0 == particle.Rec0);
                    if (retry != null)
                    {
                        var timespan = DateTime.UtcNow.Subtract(retry.LastTryDate);
                        var dayDifference = timespan.Days;
                        if (retry.Retries < 3 || dayDifference > 0)
                            invoices.Add(particle);
                    }
                }
            }

            foreach (var invoice in invoices)
            {
                var exist = _retries.Any(x => x.ParticleRec0 == invoice.Rec0);
                if (exist)
                {
                    var retry = _retries.FirstOrDefault(x => x.ParticleRec0 == invoice.Rec0);
                    if (retry != null)
                    {
                        retry.Retries++;
                        retry.LastTryDate = DateTime.UtcNow;
                    }
                }
                else
                {
                    var retry = new ParticleRetries
                    {
                        Retries = 1,
                        ParticleRec0 = invoice.Rec0,
                        LastTryDate = DateTime.UtcNow
                    };
                    _retries.Add(retry);
                }
            }
            DoTransferConversion(invoices);
        }

        private void CreateDataGridParticles()
        {
            Particles = new ObservableCollection<DataGridParticle>();
            foreach (var invoice in _listOfIParticles)
            {
                var particle = new DataGridParticle();
                var guid = Guid.NewGuid();
                invoice.DataGridId = guid;

                particle.Rec0 = invoice.Rec0;
                particle.DataGridId = guid;
                particle.Date = invoice.Date.ToShortDateString();
                particle.Branch = invoice.Branch + " " + invoice.BranchDTO.CityName;
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
            _postsPerUnit = new List<string>();
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
                if (type is < 11 or > 12)
                {
                    var counterPart = new InvoicesDocInvoiceCounterpart
                    {
                        vatNumber = particleDTO.Client?.VatNumber?.Trim(),
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
                if (paymentMethod.type == 0) paymentMethod.type = 5;
                var paymentMethods = new List<InvoicesDocInvoicePaymentMethodDetails>
                {
                    paymentMethod
                };
                invoice.paymentMethods = paymentMethods.ToArray();

                var header = new InvoicesDocInvoiceInvoiceHeader
                {
                    series = particleDTO.Series,
                    aa = particleDTO.Number.ToString(CultureInfo.InvariantCulture),
                    issueDate = particleDTO.Date,
                    invoiceType = type,
                    currency = "EUR"
                };

                if (header.series.Equals(".") || header.series.Trim().Length == 0) header.series = "0";
                invoice.invoiceHeader = header;

                decimal totalWithheldAmount = 0;
                decimal totalStampDutyAmount = 0;
                var incomeClassificationSummary = new List<InvoicesDocInvoiceInvoiceSummaryIncomeClassification>();
                var details = new List<InvoicesDocInvoiceInvoiceDetails>();
                var i = 1;
                if (particleDTO.Pmoves.Count == 0) continue;
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
                        if (sum != null) sum.amount += incomeClassification.amount;
                    }
                    incomeClassifications.Add(incomeClassification);
                    var detail = new InvoicesDocInvoiceInvoiceDetails
                    {
                        lineNumber = (uint)i,
                        netValue = item.PMS_AMAFTDISC,
                        vatCategory = (int)item.Item.FPA.FpaCategory,
                        vatAmount = item.PMS_VATAM,
                        stampDutyAmount = item.POSO_XARTOSH,
                        incomeClassification = incomeClassifications.ToArray()
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
                 
                invoice.invoiceSummary = invoicesDocInvoiceInvoiceSummary;
                list.Add(invoice);
                AddToPostXmlPerUnit(invoice);
            }

            doc.invoice = list.ToArray();
 
            using var stringWriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(doc.GetType());
            serializer.Serialize(stringWriter, doc);
            var xml = stringWriter.ToString();

            return xml;
        }

        private void AddToPostXmlPerUnit(InvoicesDocInvoice invoice)
        {
            var doc = new InvoicesDoc();
            var list = new List<InvoicesDocInvoice> { invoice };
            doc.invoice = list.ToArray();
            using var stringWriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(doc.GetType());
            serializer.Serialize(stringWriter, doc);
            var xml = stringWriter.ToString();
            _postsPerUnit.Add(xml);
        }

        private void SendParticles()
        {
            var particleIds = SelectedParticles.Select(x => x.DataGridId).ToList();

            var invoices = _listOfIParticles.Where(x => particleIds.Contains(x.DataGridId)).ToList();

            DoTransferConversion(invoices);
        }

        private async void DoTransferConversion(IReadOnlyCollection<ParticleDTO> invoices)
        {
            IsBusy = true;
            GridEnabled = false;

            var postInvoices = invoices.Where(x => x.Ptyppar.IsForCancellation is null or "0").ToList();
            var cancelInvoices = invoices.Where(x => x.Ptyppar.IsForCancellation is "1").ToList();
            var taxInvoiceRepo = new TaxInvoiceRepo(_connectionString);
            var particleRepo = new ParticleRepo(_connectionString);
            var postTransferModel = new MyDataInvoiceTransferModel();
            var cancelTransferModel = new MyDataInvoiceTransferModel();

            var validInvoices = RemoveInvalidParticles(postInvoices);

            foreach (var item in validInvoices)
            {
                var typeCode = await taxInvoiceRepo.GetTaxCode(item.Ptyppar?.Code);
                if (typeCode != null)
                {
                    var myDataInvoice = new MyDataInvoiceDTO
                    {
                        Uid = (long?)item.Rec0,
                        InvoiceNumber = (long?)item.Number,
                        InvoiceDate = item.Date,
                        VAT = item.Client?.VatNumber?.Trim(),
                        InvoiceTypeCode = (int)typeCode,
                        Particle = item
                    };
                    postTransferModel.MyDataInvoices.Add(myDataInvoice);
                }
            }
            var postXml = CreateInvoiceDocXml(validInvoices);

            postTransferModel.Xml = postXml;
            postTransferModel.XmlPerInvoice = _postsPerUnit;

            var invoiceService = new InvoiceService(_appSettings, _connectionString);
            await InsertErrorInvoices();

            if (postTransferModel.MyDataInvoices.Count > 0)
                await invoiceService.PostActionNew(postTransferModel);

            foreach (var item in cancelInvoices)
            {
                var particleToBeCancelled = await particleRepo.GetCancel(item.PARTL_RECR);
                if (particleToBeCancelled.Mark == null) break;
                var myDataInvoice = new MyDataCancelInvoiceDTO
                {
                    Uid = (long?)item.Rec0,
                    ParticleToBeCancelledMark = long.Parse(particleToBeCancelled.Mark),
                    Particle = item
                };
                cancelTransferModel.MyCancelDataInvoices.Add(myDataInvoice);
            }

            if (cancelTransferModel.MyCancelDataInvoices.Count > 0)
                await invoiceService.CancelActionNew(cancelTransferModel);
            Reload();
            IsBusy = false;
            GridEnabled = true;
        }

        private async Task InsertErrorInvoices()
        {
            if (_errorParticles.Count == 0) return;
 
            var taxInvoiceRepo = new TaxInvoiceRepo(_connectionString);
            var invoices = new List<MyDataInvoiceDTO>();

            foreach (var item in _errorParticles)
            {
                var responses = new List<MyDataResponseDTO>();
                var errors = new List<MyDataErrorDTO>();

                var typeCode = await taxInvoiceRepo.GetTaxCode(item.Ptyppar?.Code);
                var myDataInvoice = new MyDataInvoiceDTO
                {
                    Uid = (long?)item.Rec0,
                    InvoiceNumber = (long?)item.Number,
                    InvoiceDate = item.Date,
                    VAT = item.Client?.VatNumber?.Trim()
                };
                if (typeCode != null) myDataInvoice.InvoiceTypeCode = (int)typeCode;
                myDataInvoice.Particle = item;

                var response = new MyDataResponseDTO
                {
                    index = 1,
                    statusCode = "ValidationError",
                    MyDataInvoiceId = myDataInvoice.Id
                };

                var error = new MyDataErrorDTO
                {
                    MyDataResponseId = response.Id,
                    code = 420
                };

                var message = string.Empty;
                var isValid = Business.Helpers.VatValidator.Validate(item.Client?.VatNumber?.Trim());
                if (!isValid)
                    message = "Invalid Greek VAT number: " + item.Client?.VatNumber?.Trim();

              
                foreach (var pMove in item.Pmoves)
                {
                    var hasCategory = pMove.Item.KATHG_XARAKTHR != null;
                    if (!hasCategory)
                        message = "Enter correct classification Category for: " + pMove.Item.ITEM_DESCR + " Code: " + pMove.Item.ITEM_CODE;
                }
                error.message = message;

                errors.Add(error);

                response.errors = errors;
                responses.Add(response);
                myDataInvoice.MyDataResponses = responses.ToArray();
                invoices.Add(myDataInvoice);
            }
            var invoiceRepo = new InvoiceRepo(_connectionString);
            var result = await invoiceRepo.InsertOrUpdateRangeForPost(invoices);
        }

        private List<ParticleDTO> RemoveInvalidParticles(List<ParticleDTO> postInvoices)
        {
            _errorParticles = new List<ParticleDTO>();
            var list = new List<ParticleDTO>();
            foreach (var particleDTO in postInvoices)
            {
                var isValid = Business.Helpers.VatValidator.Validate(particleDTO.Client?.VatNumber?.Trim());
                var hasCategory = true;
                foreach (var item in particleDTO.Pmoves)
                {
                    hasCategory = item.Item.KATHG_XARAKTHR != null;
                }
                if (isValid && hasCategory)
                    list.Add(particleDTO);
                else
                    _errorParticles.Add(particleDTO);
            }

            return list;
        }

        private async void ResentInvoice()
        {
            if (IncomeClassificationsForEdit.Count == 0) return;
            if (SelectedInvoice == null) return;

            var invoiceService = new InvoiceService(_appSettings, _connectionString);
            var incomeClassificationsDoc = new IncomeClassificationsDoc();
            var list = new List<IncomeClassificationsDocIncomeInvoiceClassification>();
            var invoiceIncomeClassificationsDocIncomeInvoiceClassification = new IncomeClassificationsDocIncomeInvoiceClassification
                {
                    invoiceMark = long.Parse(SelectedInvoice.invoiceMark),
                    entityVatNumber = "157395112" //particleDTO.BranchDTO?.VatNumber,
                };
             

            var detailList = new List<IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetails>();

            var i = 1;
            foreach (var classification in IncomeClassificationsForEdit)
            {
                var incomeClassificationDetails = new IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetails
                    {
                        lineNumber = (short)i
                    };

                var incomeClassificationDetailsData = new IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetailsIncomeClassificationDetailData
                    {
                        classificationCategory = classification.CharacterizationCategory,
                        amount = classification.Amount
                    };

                incomeClassificationDetails.incomeClassificationDetailData = incomeClassificationDetailsData;
                detailList.Add(incomeClassificationDetails);
                i++;
            }

            invoiceIncomeClassificationsDocIncomeInvoiceClassification.invoicesIncomeClassificationDetails = detailList.ToArray();

            list.Add(invoiceIncomeClassificationsDocIncomeInvoiceClassification);
            incomeClassificationsDoc.incomeInvoiceClassification = list.ToArray();

            string xml;
            await using (var stringWriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(incomeClassificationsDoc.GetType());
                serializer.Serialize(stringWriter, incomeClassificationsDoc);
                xml = stringWriter.ToString();
            }

            await invoiceService.PostIncomeClassification(xml);
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

        private ICommand _resentCommand;

        public ICommand ResentCommand
        {
            get
            {
                return _resentCommand ?? (_resentCommand = new CommandHandler(() => ResentInvoice(), () => true));
            }
        }

        #endregion Commands

        #region Properties

        private bool _autoProcedure ;

        public bool AutoProcedure
        {
            get
            {
                return _autoProcedure;
            }
            set
            {
                _autoProcedure = value;
                if (_autoProcedure) StartAutoProcedure();
                else StopAutoProcedure();
                OnPropertyChanged();
            }
        }

        private bool _gridEnabled = true;

        public bool GridEnabled
        {
            get => _gridEnabled;
            set
            {
                _gridEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DataGridParticle> _particles = new ObservableCollection<DataGridParticle>();

        public ObservableCollection<DataGridParticle> Particles
        {
            get => _particles;
            set
            {
                _particles = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DataGridParticle> _selectedParticles;

        public ObservableCollection<DataGridParticle> SelectedParticles
        {
            get => _selectedParticles;
            set
            {
                _selectedParticles = value;
                if (_selectedParticles.Count > 0 && !AutoProcedure)
                    SendEnabled = true;
                else
                    SendEnabled = false;
                OnPropertyChanged();
            }
        }

        private MyDataInvoiceDTO _selectedInvoice;

        public MyDataInvoiceDTO SelectedInvoice
        {
            get => _selectedInvoice;
            set
            {
                _selectedInvoice = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<MyDataInvoiceDTO> _myDataInvoiceDtos = new ObservableCollection<MyDataInvoiceDTO>();

        public ObservableCollection<MyDataInvoiceDTO> MyDataInvoiceDTOs
        {
            get => _myDataInvoiceDtos;
            set
            {
                _myDataInvoiceDtos = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MyGenericErrorsDTO> _mydataErrorDTOs = new ObservableCollection<MyGenericErrorsDTO>();

        public ObservableCollection<MyGenericErrorsDTO> MyDataErrorDTOs
        {
            get => _mydataErrorDTOs;
            set
            {
                _mydataErrorDTOs = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IncomeClassificationForEditInvoice> _incomeClassificationsForEdit = new ObservableCollection<IncomeClassificationForEditInvoice>();

        public ObservableCollection<IncomeClassificationForEditInvoice> IncomeClassificationsForEdit
        {
            get => _incomeClassificationsForEdit;
            set
            {
                _incomeClassificationsForEdit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<String> _categories = new ObservableCollection<String>();

        public ObservableCollection<String> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateFrom = DateTime.UtcNow.ToLocalTime();

        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value;
                _reloadNeeded = true;
                OnPropertyChanged();
            }
        }

        private DateTime _dateTo = DateTime.UtcNow.ToLocalTime();

        public DateTime DateTo
        {
            get => _dateTo;
            set
            {
                _dateTo = value;
                _reloadNeeded = true;
                OnPropertyChanged();
            }
        }

        private bool _sendEnabled;

        public bool SendEnabled
        {
            get => _sendEnabled;
            set
            {
                _sendEnabled = value;

                OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}