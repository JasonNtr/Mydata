﻿using Domain.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AutoMapper;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Mydata.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Path = System.IO.Path;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for InvoicesUserControl.xaml
    /// </summary>
    public partial class InvoicesUserControl : UserControl
    {
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        private double _autoHeight;
        private readonly InvoicesVM _invoicesVm;
        private readonly GridRowSizingOptions _gridRowResizingOptions;
        private readonly IOptions<AppSettings> _appSettings;
        private List<string> _currentFiles;
        private readonly IRequestTransmittedDocsService _requestTransmittedDataService;
        private bool _sendFilesFinished = true;
        private bool isFirstTimeDelay = true;

        public InvoicesUserControl(IInvoiceRepo invoiceRepo, IMapper mapper, IOptions<AppSettings> appSettings, IInvoiceService invoiceService)
        {
            InitializeComponent();
            _invoicesVm = new InvoicesVM(invoiceRepo);
            this.DataContext = _invoicesVm;
            _mapper = mapper;
            this.errorGrid.QueryRowHeight += dataGrid_QueryRowHeight;
            _invoiceRepo = invoiceRepo;
            _appSettings = appSettings;
            _invoiceService = invoiceService;
            _gridRowResizingOptions = new GridRowSizingOptions();
            _currentFiles = new List<string>();
            SfDatePicker1.ValueChanged += SfDatePicker_OnValueChanged;
            SfDatePicker2.ValueChanged += SfDatePicker_OnValueChanged;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(_appSettings.Value.timerSeconds)
            };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public InvoicesUserControl(IInvoiceRepo invoiceRepo, IMapper mapper, IOptions<AppSettings> appSettings, IInvoiceService invoiceService, IRequestTransmittedDocsService requestTransmittedDocsService)
        {
            InitializeComponent();
            _invoicesVm = new InvoicesVM(invoiceRepo);
            this.DataContext = _invoicesVm;
            _mapper = mapper;
            this.errorGrid.QueryRowHeight += dataGrid_QueryRowHeight;
            _invoiceRepo = invoiceRepo;
            _appSettings = appSettings;
            _invoiceService = invoiceService;
            _gridRowResizingOptions = new GridRowSizingOptions();
            _currentFiles = new List<string>();
            SfDatePicker1.ValueChanged += SfDatePicker_OnValueChanged;
            SfDatePicker2.ValueChanged += SfDatePicker_OnValueChanged;

            _requestTransmittedDataService = requestTransmittedDocsService;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(_appSettings.Value.timerSeconds)
            };
            timer.Tick += timer_Tick;
            timer.Start();
        }


        private bool _isSearchFinish = true;

        private async void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_isSearchFinish)
                {
                    await LoadFiles();//.ContinueWith(async task => await CancellationFinish());
                }
            }
            catch (Exception)
            {

            }           
        }

        private async Task LoadFiles()
        {
            _isSearchFinish = false;
            var fileDirectory = _appSettings.Value.folderPath + @"\\Invoice";
            var files = Directory
                .GetFiles(fileDirectory, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName);


            var list = _currentFiles;
            var listToBeRemove = list.Where(x => !files.Contains(x)).ToList();
            foreach (var item in listToBeRemove)
            {
                list.Remove(item);
            }
            var listToBeAdded = files.Where(x => !list.Contains(x)).ToList();
            foreach (var item in listToBeAdded)
            {
                list.Add(item);
            }

            _currentFiles = list;
            _invoicesVm.ShowFilesToCheckBoxList(_currentFiles);


            if (_appSettings.Value.Auto && list.Count > 0)
            {

                if (isFirstTimeDelay)
                {
                    isFirstTimeDelay = false;
                    await Task.Delay(_appSettings.Value.startupDelayMSeconds);
                }

                var filename = list.FirstOrDefault();
                await InvoiceChecked(filename);
            }
            _isSearchFinish = true;

        }

        private void ViewClicked(object sender, RoutedEventArgs e)
        {
            var rowDataContent = sfGrid.SelectedItem as MyDataInvoiceDTO;
            List<MyGenericErrorsDTO> errors;
            if (rowDataContent.CancellationMark == null)
            {
                errors = _mapper.Map<List<MyGenericErrorsDTO>>(rowDataContent.MyDataResponses.Last().Errors);
            }
            else
            {
                errors = _mapper.Map<List<MyGenericErrorsDTO>>(rowDataContent.MyDataCancelationResponses.Last().Errors);
            }

            if (errorGrid.DataContext is InvoicesVM viewmodel)
            {
                viewmodel.mydataErrorDTOs.Clear();
                foreach (var myDataErrorDTO in errors)
                {
                    viewmodel.mydataErrorDTOs.Add(myDataErrorDTO);
                }
            }

            Popupnew.IsPopupOpen = true;
            MainGrid.Opacity = 0.2;
        }
        private void CheckBox2_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            var selected = OldFileList.SelectedItem as string;
            var list = _invoicesVm.oldFiles;
            list.Remove(selected);
            OldFileList.SelectedItems.Clear();
        }
        private async void CheckBox1_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            if (_appSettings.Value.Auto)
                NewFileList.SelectedItems.Clear();
            
            var files = this.NewFileList.SelectedItems;
            if (files.Count < 1)
            {
                var x = NewFileList.SelectedItems;
                return;
            }
                

            var selectedString = files[0].ToString();
            var list = _invoicesVm.oldFiles;
            list.Add(selectedString);
            NewFileList.SelectedItems.Clear();
            await InvoiceChecked(selectedString);
        }

        private async Task InvoiceChecked(string filename)
        {
            try
            {
                var path = _appSettings.Value.folderPath + @"\\Invoice\\" + filename;
                var destinationPath = _appSettings.Value.folderPath + @"\\Stored\\Invoice\\" + filename;
                var result = await _invoiceService.PostAction(path);
                if (result == -1)
                {
                    destinationPath = _appSettings.Value.folderPath + @"\\InvoiceFailed\\" + filename;
                    File.Copy(path, destinationPath, true);
                }
                else
                {
                    File.Copy(path, destinationPath, true);
                }
                File.Delete(path);
                _invoicesVm?.LoadInvoices();
            }
            catch (Exception)
            {

            }            
        }

        private void PopupNewClosed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
        }

        private void SfDatePicker_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_invoicesVm is {finishLoading: true})
                _invoicesVm?.LoadInvoices();
        }

        private void dataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (errorGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, _gridRowResizingOptions, out _autoHeight))
            {
                if (_autoHeight > 24)
                {
                    e.Height = _autoHeight;
                    e.Handled = true;
                }
            }
        }

        private async void ButtonCancel2021_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Θέλετε σίγουρα να προχωρήσετε σε ακύρωση των παραστατικών του 2021?", "Επιβεβαίωση", MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            var successfullInvoices = await _invoiceRepo.GetInvoicesWithSuccessStatusCodeFor2021();
            var counter = 0;

            try
            {
                foreach (var invoice in successfullInvoices)
                {
                    counter++;
                    LabelCancelled.Content = "(" + counter + ") " + invoice.FileName;
                    var cancellationResult = await _invoiceService.CancelInvoiceBatchProcess(invoice);
                    if (cancellationResult == -1)
                    {
                        LabelCancelled.Content += " : FAILED";
                    }
                    else
                    {
                        LabelCancelled.Content += " : OK";
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Υπήρξε πρόβλημα κατά την διαδικασία ακύρωσης. Δεν ήταν δυνατή η δημιουργία Log File", "Η Διαδικασία απέτυχε", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
            var logFileResult = _invoiceService.CreateLogFileForBatchProcess();

            if (logFileResult)
            {
                MessageBox.Show("Το αρχείο καταγραφής, δημιουργήθηκε στο path : \n" +
                    _appSettings.Value.folderPath +"\\LogFiles", "Η Διαδικασία Ολοκληρώθηκε", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Υπήρξε πρόβλημα κατά την δημιουργία αρχείου log.", "Η Διαδικασία Ολοκληρώθηκε", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_Click_RequestDocs(object sender, RoutedEventArgs e)
        {
            //What others have sent with us as counterpart
            await _requestTransmittedDataService.RequestDocs("0");
        }

        private async void Button_Click_RequestTransmittedDocs(object sender, RoutedEventArgs e)
        {
            //Invoices we have sent as issuer to API and have been successfully marked
            await _requestTransmittedDataService.RequestTransmittedDocs("0");
        }
    }
}
