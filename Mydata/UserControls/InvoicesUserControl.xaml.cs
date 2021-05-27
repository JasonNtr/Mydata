using Domain.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AutoMapper;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Mydata.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for InvoicesUserControl.xaml
    /// </summary>
    public partial class InvoicesUserControl : UserControl
    {
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IInvoiceService invoiceService;
        private readonly IMapper mapper;
        private double _autoHeight;
        private readonly InvoicesVM invoicesVm;
        private GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        private readonly IOptions<AppSettings> appsettings;

        public InvoicesUserControl(IInvoiceRepo invoiceRepo, IMapper mapper, IOptions<AppSettings> appSettings, IInvoiceService invoiceService)
        {
            InitializeComponent();
            invoicesVm = new InvoicesVM(invoiceRepo);
            this.DataContext = invoicesVm;
            this.mapper = mapper;
            this.errorGrid.QueryRowHeight += dataGrid_QueryRowHeight;
            this.invoiceRepo = invoiceRepo;
            this.mapper = mapper;
            appsettings = appSettings;
            this.invoiceService = invoiceService;

            SfDatePicker1.ValueChanged += SfDatePicker_OnValueChanged;
            SfDatePicker2.ValueChanged += SfDatePicker_OnValueChanged;
        }

        public InvoicesVM GetInvoicesVM()
        {
            return DataContext as InvoicesVM;
        }

        private void ViewClicked(object sender, RoutedEventArgs e)
        {
            var rowdataContent = this.sfGrid.SelectedItem as MyDataInvoiceDTO;
            var errors = new List<MyGenericErrorsDTO>();
            if (rowdataContent.CancellationMark == null)
            {
                errors = mapper.Map<List<MyGenericErrorsDTO>>(rowdataContent.MyDataResponses.Last().Errors);
            }
            else
            {
                errors = mapper.Map<List<MyGenericErrorsDTO>>(rowdataContent.MyDataCancelationResponses.Last().Errors);
            }

            //mainVm.mydataErrorDTOs = errors.ToObservableCollection();
            var viewmodel = (MainWindowVM)errorGrid.DataContext;
            viewmodel.mydataErrorDTOs.Clear();
            foreach (var mydataerrordto in errors)
            {
                viewmodel.mydataErrorDTOs.Add((MyGenericErrorsDTO)mydataerrordto);
            }

            //var viewmodel = (MainWindowVM)errorGrid.DataContext;
            //viewmodel.mydataErrorDTOs.Clear();
            //viewmodel.mydataErrorDTOs = errors.ToObservableCollection();
            Popupnew.IsPopupOpen = true;
            this.MainGrid.Opacity = 0.2;
        }

        private void dataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (this.errorGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out _autoHeight))
            {
                if (_autoHeight > 24)
                {
                    e.Height = _autoHeight;
                    e.Handled = true;
                }
            }
        }

        private void CheckBox2_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            var selected = this.OldFileList.SelectedItem as string;
            var list = invoicesVm.oldFiles;
            list.Remove(selected);
            OldFileList.SelectedItems.Clear();
        }

        private async void CheckBox1_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            if (appsettings.Value.Auto)
            {
                NewFileList.SelectedItems.Clear();
            }

            var selecteds = this.NewFileList.SelectedItems;
            if (selecteds.Count < 1)
                return;
            var selectedstring = selecteds[0].ToString();
            var list = invoicesVm.oldFiles;
            list.Add(selectedstring);
            NewFileList.SelectedItems.Clear();
            await InvoiceChecked(selectedstring);
        }

        private async Task InvoiceChecked(string filename)
        {
            var path = appsettings.Value.folderPath + @"\\Invoice\\" + filename;
            var destinationpath = appsettings.Value.folderPath + @"\\Stored\\Invoice\\" + filename;
            var result = await invoiceService.PostAction(path);
            File.Copy(path, destinationpath, true);
            File.Delete(path);
            invoicesVm?.LoadInvoices();
        }

        private void Popupnew_Closed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
        }

        private void SfDatePicker_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (invoicesVm != null && invoicesVm.finishLoading)
                invoicesVm?.LoadInvoices();
        }

    }
}
