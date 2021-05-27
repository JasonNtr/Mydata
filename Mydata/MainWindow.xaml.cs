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
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IInvoiceService invoiceService;
        private readonly IMyDataCancelInvoiceRepo myDataCancelInvoiceRepo;
        private readonly MainWindowVM mainVm;
        private readonly DispatcherTimer timer;
        private readonly IOptions<AppSettings> appsettings;
        private readonly IMapper mapper;
        private double _autoHeight;
        private GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        private List<string> _currentFiles = new List<string>();
        private InvoicesVM invoicesVm;
        public MainWindow(IInvoiceService invoiceService, IInvoiceRepo invoiceRepo, IMyDataCancelInvoiceRepo myDataCancelInvoiceRepo, IOptions<AppSettings> appsettings, IMapper mapper)
        {
            InitializeComponent();

            mainVm = new MainWindowVM(invoiceRepo);
            this.DataContext = mainVm;


            this.ShowInTaskbar = true;
            this.appsettings = appsettings;
            invoicesVm = new InvoicesVM(invoiceRepo);
            var invoiceTab = new InvoicesUserControl(invoiceRepo, mapper, appsettings, invoiceService, invoicesVm);
            invoiceTab.DataContext = invoicesVm;
            this.InvoiceTab.Content = invoiceTab;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();

            
        }

        #region Invoices Tab

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
            var fileDirectory = appsettings.Value.folderPath + @"\\Invoice";
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
            invoicesVm.ShowFilesToCheckBoxList(_currentFiles);
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


        private void MinimizeSelected(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        #endregion

        #region Expenses

        

        #endregion
    }
}