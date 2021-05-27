using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Mydata.ViewModels;
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
    public partial class MainWindow
    {


        public MainWindow(IInvoiceService invoiceService, IInvoiceRepo invoiceRepo, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            InitializeComponent();
            var mainVm = new MainWindowVM(invoiceRepo);
            this.DataContext = mainVm;


            this.ShowInTaskbar = true;


            var invoiceTab = new InvoicesUserControl(invoiceRepo, mapper, appSettings, invoiceService);
            this.InvoiceTab.Content = invoiceTab;
        }

        #region Invoices Tab
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            this.Activate();
            this.Opacity = 1;
        }

        private void HideSelected(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Hide();
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