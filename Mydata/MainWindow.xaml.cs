using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
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
using Mydata.UserControls;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {


        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var invoiceService = serviceProvider.GetService<IInvoiceService>();
            var invoiceRepo = serviceProvider.GetService<IInvoiceRepo>();
            var expenseRepo = serviceProvider.GetService<IExpenseRepo>();
            var expenseService = serviceProvider.GetService<IExpenseService>();
            var incomeRepo = serviceProvider.GetService<IIncomeRepo>();
            var incomeService = serviceProvider.GetService<IIncomeService>();

            var appSettings = serviceProvider.GetService<IOptions<AppSettings>>();
            var mapper = serviceProvider.GetService<IMapper>();

            var mainVm = new MainWindowVM(invoiceRepo);
            this.DataContext = mainVm;


            this.ShowInTaskbar = true;

            

            var invoiceTab = new InvoicesUserControl(invoiceRepo, mapper, appSettings, invoiceService);
            //var expenseTab = new ExpensesUserControl(expenseRepo, mapper, appSettings, invoiceService);
            var incomeTab = new IncomesUserControl(incomeRepo,incomeService,mapper,appSettings);
            this.InvoiceTab.Content = invoiceTab;
            this.IncomesTab.Content = incomeTab;
            //this.ExpensesTab.Content = expenseTab;
        }

        
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
    }
}