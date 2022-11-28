using Domain.DTO;
using Microsoft.Extensions.Options;
using Mydata.ViewModels;
using System.Windows.Controls;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for ExpensesUserControl.xaml
    /// </summary>
    public partial class ExpensesUserControl : UserControl
    {
        private readonly string _connectionString;

        private ExpensesViewModel _viewmodel;

        public ExpensesUserControl(IOptions<AppSettings> appSettings, string conenctionString)
        {
            InitializeComponent();
            _connectionString = conenctionString;
            _viewmodel = new ExpensesViewModel(appSettings, conenctionString);
            this.DataContext = _viewmodel;
        }
    }
}