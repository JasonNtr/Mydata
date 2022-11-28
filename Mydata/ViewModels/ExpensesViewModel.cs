using Business.ApiServices;
using Domain.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mydata.ViewModels
{
    public class ExpensesViewModel : ViewModelBase
    {
        private readonly string _connectionString;
        private readonly IOptions<AppSettings> _appSettings;
        public ExpensesViewModel(IOptions<AppSettings> appSettings, string connectionString)
        {
            _appSettings = appSettings;
            _connectionString = connectionString;
            Init();
        }

        private async Task Init()
        {
            var expenseService = new ExpenseService(_appSettings, _connectionString);
            await expenseService.GetExpenses(DateFrom, DateTo);
        }

        #region Properties
        private DateTime _dateFrom = DateTime.UtcNow.ToLocalTime();

        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value;
                
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
                
                OnPropertyChanged();
            }
        }
#endregion
    }
}
