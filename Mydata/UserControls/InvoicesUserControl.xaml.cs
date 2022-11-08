using Business.Services;
using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Mydata.UiModels;
using Mydata.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for InvoicesUserControl.xaml
    /// </summary>
    public partial class InvoicesUserControl : UserControl
    {
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IInvoiceService _invoiceService;

        private double _autoHeight;
        private InvoicesViewModel _viewmodel;

        private readonly GridRowSizingOptions _gridRowResizingOptions;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly string _conenctionString;

        private readonly IRequestTransmittedDocsService _requestTransmittedDataService;
        private int _rowHeight = 30;

        public InvoicesUserControl(IOptions<AppSettings> appSettings, string conenctionString)
        {
            InitializeComponent();
            _viewmodel = new InvoicesViewModel(appSettings, conenctionString);
            this.DataContext = _viewmodel;
        }

        private void ViewClicked(object sender, RoutedEventArgs e)
        {
            var rowDataContent = sfGrid.SelectedItem as MyDataInvoiceDTO;

            if (rowDataContent.invoiceMark.Length != 0 && rowDataContent.MyDataCancellationResponses.Count == 0) return;

            List<MyGenericErrorsDTO> errors;
            var myDataResponseRepo = new MyDataResponseRepo(_conenctionString);

            if (rowDataContent.MyDataCancellationResponses.Count == 0)
            {
                errors = myDataResponseRepo.MapToGenericErrorDTO(rowDataContent.MyDataResponses.OrderBy(x => x.Created).Last().errors);
            }
            else
            {
                errors = myDataResponseRepo.MapToGenericErrorDTOCancellation(rowDataContent.MyDataCancellationResponses.OrderBy(x => x.Created).Last().Errors);
            }

            if (errors.Count > 0)
            {
                _viewmodel.MyDataErrorDTOs.Clear();
                foreach (var myDataErrorDTO in errors)
                {
                    _viewmodel.MyDataErrorDTOs.Add(myDataErrorDTO);
                }

                var maxLength = errors.Max(x => x.Message.Length);
                var lines = maxLength / 10;
                _rowHeight = 5 * lines;
            }

            Popupnew.IsPopupOpen = true;
            MainGrid.Opacity = 0.2;
        }

        private void PopupNewClosed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
        }

        private async void ButtonCancel2021_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Θέλετε σίγουρα να προχωρήσετε σε ακύρωση των παραστατικών του 2021?", "Επιβεβαίωση", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;

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
                Mouse.OverrideCursor = null;
                MessageBox.Show("Υπήρξε πρόβλημα κατά την διαδικασία ακύρωσης. Δεν ήταν δυνατή η δημιουργία Log File", "Η Διαδικασία απέτυχε", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Mouse.OverrideCursor = null;

            var logFileResult = _invoiceService.CreateLogFileForBatchProcess();

            if (logFileResult)
            {
                MessageBox.Show("Το αρχείο καταγραφής, δημιουργήθηκε στο path : \n" +
                    _appSettings.Value.folderPath + "\\LogFiles", "Η Διαδικασία Ολοκληρώθηκε", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Υπήρξε πρόβλημα κατά την δημιουργία αρχείου log.", "Η Διαδικασία Ολοκληρώθηκε", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ErrorGrid_OnQueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex == 0) return;
            e.Height = _rowHeight;
            e.Handled = true;
        }

        private void SfGrid2_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var items = sfGrid2.SelectedItems;
            var list = new ObservableCollection<DataGridParticle>();
            foreach (var item in items)
            {
                var myItem = item as DataGridParticle;
                list.Add(myItem);
            }
            _viewmodel.SelectedParticles = new ObservableCollection<DataGridParticle>(list);
        }
    }
}