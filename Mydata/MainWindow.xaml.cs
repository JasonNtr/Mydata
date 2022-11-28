using Domain.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Windows;
using System.Windows.Input;

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

            var appSettings = serviceProvider.GetService<IOptions<AppSettings>>();
            var conenctionString = serviceProvider.GetService<IOptions<ConnectionStrings>>();

            this.ShowInTaskbar = true;

            var invoiceTab = new InvoicesUserControl(appSettings, conenctionString.Value.Default);
            var expensesTab = new ExpensesUserControl(appSettings, conenctionString.Value.Default);

            this.InvoiceTab.Content = invoiceTab;
            this.ExpensesTab.Content = expensesTab;
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