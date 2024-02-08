using Business.ApiServices;
using Business.Brokers;
using Business.Services;
using Domain.AADE;
using Domain.DTO;
using Domain.Model;
using Microsoft.Extensions.Options;
using Mydata.Helpers;
using Mydata.UiModels;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Mydata.ViewModels
{
    public class InvoicesViewModel : ViewModelBase , IDisposable
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

            ParticleNo = "(0)";
        }

        private async Task Init()
        {
            await LoadInvoices();
            StartEmptyBroker();
        }

        private void StartBroker(List<ParticleDTO> particleDTOs)
        {
            _broker = new ParticleBroker(_connectionString, DateFrom, DateTo, particleDTOs);

            _broker.ProductionChangeHappened += ParticleChange;
            _reloadNeeded = false;
        }

        private void StartEmptyBroker()
        {
            _broker = new ParticleBroker(_connectionString, DateFrom, DateTo);

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
            else
            {
                Particles = new ObservableCollection<DataGridParticle>();
                ParticleNo = "(0)";
            }
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
            else
            {
                Particles = new ObservableCollection<DataGridParticle>();
                ParticleNo = "(0)";
            }
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
                //if(myDataInvoiceDto.Id == Guid.Parse("8FDF1A30-1B5B-46D9-99D9-2289AB64FBCF")){
                //    var x = 5;
                //    MyDataInvoiceDTOs.Add(myDataInvoiceDto);
                //}
                MyDataInvoiceDTOs.Add(myDataInvoiceDto);
            }
        }

        private void StartAutoProcedure()
        {
            AutoProcedureExecute();
            _timer.Start();
            _broker.Start();
            SendEnabled = false;
        }

        private void StopAutoProcedure()
        {
            _timer.Stop();
            //_timer.Dispose();
            _broker.Stop();
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
                var exist = _retries.Any(x => x.ParticleNo == particle.Code);
                if (!exist) invoices.Add(particle);
                else
                {
                    var retry = _retries.FirstOrDefault(x => x.ParticleNo == particle.Code);
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
                var exist = _retries.Any(x => x.ParticleNo == invoice.Code);
                if (exist)
                {
                    var retry = _retries.FirstOrDefault(x => x.ParticleNo == invoice.Code);
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
                        ParticleNo = invoice.Code,
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

                particle.Rec0 = invoice.Code;
                particle.DataGridId = guid;
                particle.Date = invoice.Date.ToShortDateString();
                particle.Branch = "Branch";
                particle.Series = invoice.Ptyppar.Series;
                particle.Client = invoice.Client.Ship.Name;
                particle.PtyParDescription = invoice.Ptyppar.Code + " - " + invoice.Ptyppar.Description;
                particle.Amount = ((decimal)invoice.Amount).ToString("c");
                particle.Number = invoice.Number;
                particle.Code = invoice.Code.ToString();

                Particles.Add(particle);
            }
            var count = Particles.Count;
            ParticleNo = "(" + count + ")";
        }

        private async Task<string> CreateInvoiceDocXml(List<ParticleDTO> particlesDTO,CompanyDTO company)
        {
            var doc = new InvoicesDoc();
            var list = new List<InvoicesDocInvoice>();
            _postsPerUnit = new List<string>();
            foreach (var particleDTO in particlesDTO)
            {

                var invoice = new InvoicesDocInvoice();
                var type = decimal.Parse(particleDTO.Ptyppar.EidParast, CultureInfo.InvariantCulture);
                var issuer = new InvoicesDocInvoiceIssuer
                {
                    vatNumber = company.Vat.Trim(),
                    country = "GR",
                    branch = 0
                };
                var issuerPartAddress = new InvoicesDocInvoiceIssuerAddress
                {
                    postalCode = company.ZipCode,
                    city = company.CIty
                };
                issuer.address = issuerPartAddress;
                invoice.issuer = issuer;
                if (type is < 11 or > 12)
                {
                    var counterPart = new InvoicesDocInvoiceCounterpart
                    {
                        vatNumber = particleDTO.Client?.Ship.Vat?.Trim(),
                        country = particleDTO.Client?.Ship.CountryCodeISO,
                        branch = 0
                    };


                    if (String.IsNullOrEmpty(counterPart.vatNumber)) counterPart.vatNumber = particleDTO.Client?.Vat?.Trim();
                    if (String.IsNullOrEmpty(counterPart.country)) counterPart.country = particleDTO.Client?.CountryCodeISO;


                    //particleDTO.Client.CountryCodeISO = null;
                    if (!(bool)particleDTO.Client?.CountryCodeISO.IsNullOrWhiteSpace() && (bool)!particleDTO.Client?.Ship.CountryCodeISO.Equals("GR"))
                    {
                        counterPart.name = particleDTO.Client?.Ship.Name;
                    };
                    var counterPartAddress = new InvoicesDocInvoiceCounterpartAddress
                    {
                        postalCode = particleDTO.Client?.Ship.ZipCode ?? "",
                        city = particleDTO.Client?.City
                    };

                    if (String.IsNullOrEmpty(counterPartAddress.postalCode)) counterPartAddress.postalCode = particleDTO.Client?.ZipCode ?? "";


                    counterPart.address = counterPartAddress;
                    invoice.counterpart = counterPart;
                }

                var paymentMethod = new InvoicesDocInvoicePaymentMethodDetails
                {
                    type = (int)particleDTO.Paymentmethod,
                    amount = (decimal)particleDTO.Amount
                };
                if (paymentMethod.type == 0) paymentMethod.type = 5;
                var paymentMethods = new List<InvoicesDocInvoicePaymentMethodDetails>
                {
                    paymentMethod
                };
                invoice.paymentMethods = paymentMethods.ToArray();

                var bExeiApallaghFPA = particleDTO.Ptyppar.VatExemption;

                var header = new InvoicesDocInvoiceInvoiceHeader
                {
                    series = particleDTO.Ptyppar.Series,
                    aa = particleDTO.Number.ToString(CultureInfo.InvariantCulture),
                    issueDate = particleDTO.Date,
                    invoiceType = type,
                    currency = "EUR"
                };

                if(particleDTO.Ptyppar.Pistotiko == 1 && (particleDTO.Ptyppar.EidParast == "5.1") )
                {
                    var particleRepo = new ParticleRepo(_connectionString);
                    long parsedNumber;
                    var cancelparticlee = await particleRepo.GetParticleByRec0((long?)particleDTO.CanceledParticle);
                    // Try to parse the string into a long
                    bool success = long.TryParse(cancelparticlee.Mark, out parsedNumber);

                    if (success)
                    {
                        long[] yourArray = new long[] { parsedNumber };
                        header.correlatedInvoices = yourArray;
                    }
                    
                }
                if(bExeiApallaghFPA ==1) header.vatPaymentSuspension = true;
                if (header.series.Equals(".") || header.series.Trim().Length == 0) header.series = "0";
                invoice.invoiceHeader = header;

                decimal totalWithheldAmount = 0;
                decimal totalStampDutyAmount = 0;
                var incomeClassificationSummary = new List<InvoicesDocInvoiceInvoiceSummaryIncomeClassification>();
                var details = new List<InvoicesDocInvoiceInvoiceDetails>();
                var i = 1;
                if (particleDTO.Pmoves.Count == 0) continue;

                decimal? netAmount = 0;
                decimal? vatAmount = 0;
                decimal? withheldAmount = 0;
                decimal? stampDutyAmount = 0;
                decimal? grossAmount = 0;

                decimal sumDeduction = 0;
                foreach (var item in particleDTO.Pmoves)
                {
                    if (item.PMS_AMAFTDISC is not null && item.PMS_AMAFTDISC < 0)
                    {
                        sumDeduction = (decimal)(sumDeduction + item.PMS_AMAFTDISC);
                        break;
                    }

                    var incomeClassifications = new List<InvoicesDocInvoiceInvoiceDetailsIncomeClassification>();
                    var incomeClassification = new InvoicesDocInvoiceInvoiceDetailsIncomeClassification
                    {
                        classificationType = particleDTO.Ptyppar.TYPOS_XARAKTHR,
                        classificationCategory = item.ItemDTO.Category,
                        amount = (decimal)item.PMS_AMAFTDISC
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
                    var rounded = Math.Round((decimal)item.Net2, 2);
                    var detail = new InvoicesDocInvoiceInvoiceDetails
                    {
                        lineNumber = (uint)i,
                        netValue = (decimal)rounded,
                        vatCategory = (int)item.ItemDTO.FPA.Category,
                        vatAmount = (decimal)item.PMS_VATAM,
                        stampDutyAmount = (decimal)item.POSO_XARTOSH,
                        incomeClassification = incomeClassifications.ToArray(),
                        withheldAmount = item.POSO_PARAKRAT,
                        deductionsAmount = 0
                    };
                    if (bExeiApallaghFPA == 1)
                    {
                        var category = particleDTO.Ptyppar.EXAIRFPA;
                         
                        detail.vatCategory = 7;
                        detail.vatAmount = 0;
                        detail.vatExemptionCategory = (byte)category;
                    }

                    netAmount += rounded;
                    vatAmount += item.PMS_VATAM;
                    withheldAmount += item.POSO_PARAKRAT;
                    stampDutyAmount += item.POSO_XARTOSH;
                    var grossRounded = Math.Round((decimal)item.CalculatedGross, 2, MidpointRounding.AwayFromZero);
                    grossAmount += grossRounded;

                    details.Add(detail);

                    i++;
                }

                invoice.invoiceDetails = details.ToArray();

                var invoicesDocInvoiceInvoiceSummary = new InvoicesDocInvoiceInvoiceSummary
                {
                    totalNetValue = (decimal)netAmount,
                    totalVatAmount = (decimal)vatAmount,
                    totalWithheldAmount = (decimal)withheldAmount,
                    totalFeesAmount = 0,
                    totalStampDutyAmount = (long)stampDutyAmount,
                    totalOtherTaxesAmount = 0,
                    totalDeductionsAmount = 0,
                    totalGrossValue = (decimal)grossAmount,
                    incomeClassification = incomeClassificationSummary.ToArray()
                };

                if (bExeiApallaghFPA == 1)
                {
                    invoicesDocInvoiceInvoiceSummary.totalVatAmount = 0;
                }

                if(sumDeduction < 0)
                {
                    invoicesDocInvoiceInvoiceSummary.totalNetValue = invoicesDocInvoiceInvoiceSummary.totalNetValue + sumDeduction;
                    invoicesDocInvoiceInvoiceSummary.totalGrossValue = invoicesDocInvoiceInvoiceSummary.totalGrossValue + sumDeduction;

                    var randomPmove = invoice.invoiceDetails.FirstOrDefault(x=>x.netValue > (-1 * sumDeduction));
                    var randomClassification = randomPmove.incomeClassification.FirstOrDefault();
                    var sumClassification = invoicesDocInvoiceInvoiceSummary.incomeClassification.FirstOrDefault();

                    randomPmove.netValue = randomPmove.netValue + sumDeduction;
                    randomClassification.amount = randomClassification.amount + sumDeduction;
                    sumClassification.amount = sumClassification.amount + sumDeduction;
                   
                }

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

        private async void SendParticles()
        {
            var particleIds = SelectedParticles.Select(x => x.DataGridId).ToList();

            var invoices = _listOfIParticles.Where(x => particleIds.Contains(x.DataGridId)).ToList();

            UpdateClients();
            DoTransferConversion(invoices);
        }

        private async void UpdateClients()
        {
            var clientRepo = new ClientRepo(_connectionString);
            await clientRepo.UpdateSpecific();
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
            
            var pistotika = new List<ParticleDTO>();

            foreach (var item in validInvoices)
            {
                var typeCode = await taxInvoiceRepo.GetTaxCode(item.Ptyppar?.Code.ToString());
                if (typeCode != null)
                {
                    var vat = item.Client?.Ship.Vat?.Trim();
                    if (String.IsNullOrEmpty(vat)) vat = item.Client?.Vat?.Trim();

                    var myDataInvoice = new MyDataInvoiceDTO
                    {
                        Uid = (long?)item.Code,
                        InvoiceNumber = (long?)item.Number,
                        InvoiceDate = item.Date,
                        VAT = vat,
                        InvoiceTypeCode = (int)typeCode,
                        Particle = item
                    };
                    postTransferModel.MyDataInvoices.Add(myDataInvoice);
                }
            }
           
            var companyrepo = new CompanyRepo(_connectionString);
            var company = await companyrepo.Get();
            var postXml = await CreateInvoiceDocXml(validInvoices, company);

            postTransferModel.Xml = postXml;
            postTransferModel.XmlPerInvoice = _postsPerUnit;

            var invoiceService = new InvoiceService(_appSettings, _connectionString);
            await InsertErrorInvoices();

            if (postTransferModel.MyDataInvoices.Count > 0)
                await invoiceService.PostActionNew(postTransferModel);

            foreach (var item in cancelInvoices)
            {
                var particleToBeCancelled = await particleRepo.GetCancel(item.CanceledParticle);
                if (particleToBeCancelled.Mark == null) break;
                var myDataInvoice = new MyDataCancelInvoiceDTO
                {
                    Uid = (long?)item.Code,
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

                var typeCode = await taxInvoiceRepo.GetTaxCode(item.Ptyppar?.Code.ToString());

                var vat = item.Client?.Ship.Vat?.Trim();
                if (String.IsNullOrEmpty(vat)) vat = item.Client?.Vat?.Trim();

                var myDataInvoice = new MyDataInvoiceDTO
                {
                    Uid = (long?)item.Code,
                    InvoiceNumber = (long?)item.Number,
                    InvoiceDate = item.Date,
                    VAT = vat
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
                var isValid = Business.Helpers.VatValidator.Validate(item.Client?.Vat?.Trim());
                if (!isValid)
                    message = "Invalid Greek VAT number: " + item.Client?.Vat?.Trim();

                foreach (var pMove in item.Pmoves)
                {
                    var hasCategory = pMove.ItemDTO.Category != null;
                    if (!hasCategory)
                        message = "Enter correct classification Category for: " + pMove.ItemDTO.ITEM_DESCR + " Code: " + pMove.ItemDTO.Code;
                }
                if (typeCode == null)
                {
                    message = "Fix parforol for Code:" + item.Ptyppar?.Code;
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
                var type = decimal.Parse(particleDTO.Ptyppar.EidParast, CultureInfo.InvariantCulture);
                var isValid = true;

                var vat = particleDTO.Client?.Ship.Vat?.Trim();
                if (String.IsNullOrEmpty(vat)) vat = particleDTO.Client?.Vat?.Trim();

                var country = particleDTO.Client?.Ship.CountryCodeISO;
                if (String.IsNullOrEmpty(country)) country = particleDTO.Client?.CountryCodeISO;


                //if (!(bool)country.IsNullOrWhiteSpace() && (bool)country.Equals("GR") && type is < 11 or > 12)
                //{
                //    isValid = Business.Helpers.VatValidator.Validate(vat);
                //}
                var hasCategory = true;
                foreach (var item in particleDTO.Pmoves)
                {
                    hasCategory = item.ItemDTO.Category != null;
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
            var list = new List<InvoiceIncomeClassificationType>();
            var invoiceIncomeClassificationType = new InvoiceIncomeClassificationType
            {
                invoiceMark = long.Parse(SelectedInvoice.invoiceMark),
                entityVatNumber = "157395112" //particleDTO.BranchDTO?.VatNumber,
            };

            var detailList = new List<InvoicesIncomeClassificationDetailType>();

            var i = 1;
            foreach (var classification in IncomeClassificationsForEdit)
            {
                var incomeClassificationList = new List<IncomeClassificationType>();
                var incomeClassificationDetails = new InvoicesIncomeClassificationDetailType
                {
                    lineNumber = (short)i
                };

                var incomeClassificationDetailsData = new IncomeClassificationType
                {
                    classificationCategory = classification.CharacterizationCategory,
                    classificationType = classification.CharacterizationType,
                    amount = classification.Amount,
                    classificationTypeSpecified = true
                };

                incomeClassificationList.Add(incomeClassificationDetailsData);
                incomeClassificationDetails.incomeClassificationDetailData = incomeClassificationList.ToArray();
                detailList.Add(incomeClassificationDetails);
                i++;
            }

            invoiceIncomeClassificationType.invoicesIncomeClassificationDetails = detailList.ToArray();

            list.Add(invoiceIncomeClassificationType);
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

        public void Dispose()
        {
            _timer.Dispose();
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

        private bool _autoProcedure;

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

        private string _particleNo;

        public string ParticleNo
        {
            get
            {
                return _particleNo;
            }
            set
            {
                _particleNo = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateFrom = DateTime.UtcNow.AddDays(-1).ToLocalTime();

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