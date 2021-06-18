using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Domain.DTO;
using Infrastructure.Interfaces.Services;

namespace Mydata.ViewModels
{
    public class IncomesVM : INotifyPropertyChanged
    {
        private readonly IIncomeRepo _incomeRepo;

        private ObservableCollection<MyDataIncomeDTO> _myDataIncomeDTOs = new ObservableCollection<MyDataIncomeDTO>();
        public ObservableCollection<MyDataIncomeDTO> MyDataIncomeDTOs
        {
            get => _myDataIncomeDTOs;
            set
            {
                _myDataIncomeDTOs = value;
                OnPropertyChanged("MyDataIncomeDTOs");
            }
        }

        private ObservableCollection<MyGenericErrorsDTO> _mydataErrorDTOs;
        public ObservableCollection<MyGenericErrorsDTO> mydataErrorDTOs
        {
            get => _mydataErrorDTOs;
            set
            {
                _mydataErrorDTOs = value;
                OnPropertyChanged("mydataErrorDTOs");
            }
        }


        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get => _DateFrom;
            set
            {
                _DateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }

        private DateTime _DateTo;
        public DateTime DateTo
        {
            get => _DateTo;
            set
            {
                _DateTo = value;
                OnPropertyChanged("DateTo");
            }
        }


        private ObservableCollection<string> _newFiles;

        public ObservableCollection<string> newFiles
        {
            get => _newFiles;
            set
            {
                _newFiles = value;
                OnPropertyChanged("newFiles");
            }
        }

        private ObservableCollection<string> _oldFiles;

        public ObservableCollection<string> oldFiles
        {
            get => _oldFiles;
            set { _oldFiles = value; OnPropertyChanged("oldFiles"); }
        }

        public IncomesVM(IIncomeRepo incomeRepo)
        {

            this._incomeRepo = incomeRepo;
            // Submit = new AsyncCommand(ExecuteSubmitAsync, CanExecuteSubmit);
            //_myDataInvoiceDtos = new ObservableCollection<MyDataInvoiceDTO>();
            _mydataErrorDTOs = new ObservableCollection<MyGenericErrorsDTO>();
            _mydataErrorDTOs.Add(new MyGenericErrorsDTO()
            {
                Code = 1,
                Message = "No error"
            });
            _newFiles = new ObservableCollection<string>();
            _oldFiles = new ObservableCollection<string>();

            _DateFrom = DateTime.Now.AddDays(-1);
            _DateTo = DateTime.Now;
            LoadInvoices();
        }


        public bool finishLoading = true;
        public async void LoadInvoices()
        {
            finishLoading = false;
            MyDataIncomeDTOs.Clear();
            var listOfIncome = await _incomeRepo.GetList(DateFrom, DateTo);
            foreach (var myDataIncomeDto in listOfIncome)
            {
                MyDataIncomeDTOs.Add(myDataIncomeDto);
            }
            finishLoading = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public void ShowFilesToCheckBoxList(List<string> currentFiles)
        {
            newFiles = new ObservableCollection<string>(currentFiles);
        }
    }
}
