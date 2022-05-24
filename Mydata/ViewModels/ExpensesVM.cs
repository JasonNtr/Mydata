using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Domain.DTO;
using Infrastructure.Interfaces.Services;

namespace Mydata.ViewModels
{
    public class ExpensesVM : INotifyPropertyChanged
    {
        private readonly IExpenseRepo _expenseRepo;

        private ObservableCollection<MyDataInvoiceDTO> _myDataInvoiceDtos = new ObservableCollection<MyDataInvoiceDTO>();
        public ObservableCollection<MyDataInvoiceDTO> mydataInvoiceDTOs
        {
            get
            {
                return _myDataInvoiceDtos;
            }
            set
            {
                _myDataInvoiceDtos = value;
                OnPropertyChanged("mydataInvoiceDTOs");
            }
        }

        private ObservableCollection<MyGenericErrorsDTO> _mydataErrorDTOs;
        public ObservableCollection<MyGenericErrorsDTO> mydataErrorDTOs
        {
            get
            {
                return _mydataErrorDTOs;
            }
            set
            {
                _mydataErrorDTOs = value;
                OnPropertyChanged("mydataErrorDTOs");
            }
        }


        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get
            {
                return _DateFrom;
            }
            set
            {
                _DateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }

        private DateTime _DateTo;
        public DateTime DateTo
        {
            get
            {
                return _DateTo;
            }
            set
            {
                _DateTo = value;
                OnPropertyChanged("DateTo");
            }
        }


        private ObservableCollection<string> _newFiles;

        public ObservableCollection<string> newFiles
        {
            get { return _newFiles; }
            set
            {
                _newFiles = value;
                OnPropertyChanged("newFiles");
            }
        }

        private ObservableCollection<string> _oldFiles;

        public ObservableCollection<string> oldFiles
        {
            get { return _oldFiles; }
            set { _oldFiles = value; OnPropertyChanged("oldFiles"); }
        }

        public ExpensesVM(IExpenseRepo expenseRepo)
        {
            this._expenseRepo = expenseRepo;
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
            mydataInvoiceDTOs.Clear();
            var listOfInvoice = await _expenseRepo.GetList(DateFrom, DateTo);
            foreach (var myDataInvoiceDto in listOfInvoice)
            {
                //mydataInvoiceDTOs.Add(myDataInvoiceDto);
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
