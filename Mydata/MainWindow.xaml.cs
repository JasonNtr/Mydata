using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mydata.ViewModels;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AutoMapper;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IInvoiceService invoiceService;
        private readonly IMyDataCancelInvoiceRepo myDataCancelInvoiceRepo;
        private readonly MainWindowVM mainVm;
        private readonly DispatcherTimer timer;
        private readonly IOptions<AppSettings> appsettings;
        private readonly IMapper mapper;
        private double _autoHeight;
        private GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();

        public MainWindow(IServiceProvider ServiceProvider, IInvoiceService invoiceService, IInvoiceRepo invoiceRepo, IMyDataCancelInvoiceRepo myDataCancelInvoiceRepo, IOptions<AppSettings> appsettings, IMapper mapper)
        {
            InitializeComponent();
           
            this.ServiceProvider = ServiceProvider;
            mainVm = ServiceProvider.GetService<MainWindowVM>(); //new MainWindowVM();
            this.DataContext = mainVm;
            this.ShowInTaskbar = true;

            this.errorGrid.QueryRowHeight += dataGrid_QueryRowHeight;
            this.invoiceService = invoiceService;
            this.myDataCancelInvoiceRepo = myDataCancelInvoiceRepo;
            this.invoiceRepo = invoiceRepo;
            this.appsettings = appsettings;
            this.mapper = mapper;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();

            SfDatePicker1.ValueChanged += SfDatePicker_OnValueChanged;
            SfDatePicker2.ValueChanged += SfDatePicker_OnValueChanged;
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

        private bool is_search_finish = true;

        private async void timer_Tick(object sender, EventArgs e)
        {
            if (is_search_finish)
            {
                await LoadFiles();//.ContinueWith(async task => await CancellationFinish());
            }
        }

        private async Task LoadFiles()
        {
            is_search_finish = false;
            var filedirectory = appsettings.Value.folderPath + @"\\Invoice";
            var files = Directory
                .GetFiles(filedirectory, "*", SearchOption.AllDirectories)
                .Select(f => Path.GetFileName(f));

            var list = mainVm.newFiles;
            var listtoberemove = list.Where(x => !files.Contains(x)).ToList();
            foreach (var item in listtoberemove)
            {
                list.Remove(item);
            }
            var listtobeadded = files.Where(x => !list.Contains(x)).ToList();
            foreach (var item in listtobeadded)
            {
                list.Add(item);
            }

            if (appsettings.Value.Auto && list.Count > 0)
            {
                var filename = list.FirstOrDefault();
                await InvoiceChecked(filename);
            }
            is_search_finish = true;
        }

        private async Task InvoiceChecked(string filename)
        {
            var path = appsettings.Value.folderPath + @"\\Invoice\\" + filename;
            var destinationpath = appsettings.Value.folderPath + @"\\Stored\\Invoice\\" + filename;
            var result = await invoiceService.PostAction(path);
            File.Copy(path, destinationpath, true);
            File.Delete(path);
            mainVm?.LoadInvoices();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            this.Activate();
            this.Opacity = 1;
        }

        private void HideSelected(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Hide();
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
                viewmodel.mydataErrorDTOs.Add((MyGenericErrorsDTO) mydataerrordto);
            }

            //var viewmodel = (MainWindowVM)errorGrid.DataContext;
            //viewmodel.mydataErrorDTOs.Clear();
            //viewmodel.mydataErrorDTOs = errors.ToObservableCollection();
            Popupnew.IsPopupOpen = true;
            this.MainGrid.Opacity = 0.2;
        }

        private void MinimizeSelected(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CheckBox2_ItemChecked(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selected = this.OldFileList.SelectedItem as string;
            var list = mainVm.oldFiles;
            list.Remove(selected);
            OldFileList.SelectedItems.Clear();
        }

        private async void CheckBox1_ItemChecked(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (appsettings.Value.Auto)
            {
                NewFileList.SelectedItems.Clear();
            }

            var selecteds = this.NewFileList.SelectedItems;
            if (selecteds.Count < 1)
                return;
            var selectedstring = selecteds[0].ToString();
            var list = mainVm.oldFiles;
            list.Add(selectedstring);
            NewFileList.SelectedItems.Clear();
            await InvoiceChecked(selectedstring);
        }

        private void Popupnew_Closed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
        }

        private void SfDatePicker_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (mainVm != null && mainVm.finishLoading)
                mainVm?.LoadInvoices();
        }


    }
}