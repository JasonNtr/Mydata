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
using System.Windows.Threading;
using AutoMapper;
using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Mydata.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Path = System.Windows.Shapes.Path;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for ExpensesUserControl.xaml
    /// </summary>
    public partial class ExpensesUserControl : UserControl
    {
        private readonly IExpenseRepo _expenseRepo;
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        private double _autoHeight;
        private readonly ExpensesVM _expensesVm;
        private readonly GridRowSizingOptions _gridRowResizingOptions;
        private readonly IOptions<AppSettings> _appSettings;
        private List<string> _currentFiles;
        public ExpensesUserControl(IExpenseRepo expenseRepo, IMapper mapper, IOptions<AppSettings> appSettings, IInvoiceService invoiceService)
        {
            InitializeComponent();
            _expensesVm = new ExpensesVM(expenseRepo);
            this.DataContext = _expensesVm;
            _mapper = mapper;
            this.errorGrid.QueryRowHeight += dataGrid_QueryRowHeight;
            _expenseRepo = expenseRepo;
            _appSettings = appSettings;
            _invoiceService = invoiceService;
            _gridRowResizingOptions = new GridRowSizingOptions();
            _currentFiles = new List<string>();
            SfDatePicker1.ValueChanged += SfDatePicker_OnValueChanged;
            SfDatePicker2.ValueChanged += SfDatePicker_OnValueChanged;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2000)
            };
            timer.Tick += timer_Tick;
            timer.Start();
        }


        private bool _isSearchFinish = true;

        private async void timer_Tick(object sender, EventArgs e)
        {
            if (_isSearchFinish)
            {
                await LoadFiles();//.ContinueWith(async task => await CancellationFinish());
            }
        }

        private async Task LoadFiles()
        {
            _isSearchFinish = false;
            var fileDirectory = _appSettings.Value.folderPath + @"\\Invoice";
            var files = Directory
                .GetFiles(fileDirectory, "*", SearchOption.AllDirectories)
                .Select(System.IO.Path.GetFileName);


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
            _expensesVm.ShowFilesToCheckBoxList(_currentFiles);

            if (_appSettings.Value.Auto && list.Count > 0)
            {
                var filename = list.FirstOrDefault();
                await InvoiceChecked(filename);
            }
            _isSearchFinish = true;

        }

        //private void ViewClicked(object sender, RoutedEventArgs e)
        //{
        //    var rowDataContent = sfGrid.SelectedItem as MyDataInvoiceDTO;
        //    List<MyGenericErrorsDTO> errors;
        //    if (rowDataContent.CancellationMark == null)
        //    {
        //        errors = _mapper.Map<List<MyGenericErrorsDTO>>(rowDataContent.MyDataResponses.Last().Errors);
        //    }
        //    else
        //    {
        //        errors = _mapper.Map<List<MyGenericErrorsDTO>>(rowDataContent.MyDataCancelationResponses.Last().Errors);
        //    }

        //    if (errorGrid.DataContext is InvoicesVM viewmodel)
        //    {
        //        viewmodel.mydataErrorDTOs.Clear();
        //        foreach (var myDataErrorDTO in errors)
        //        {
        //            viewmodel.mydataErrorDTOs.Add(myDataErrorDTO);
        //        }
        //    }

        //    Popupnew.IsPopupOpen = true;
        //    MainGrid.Opacity = 0.2;
        //}
        private void CheckBox2_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            var selected = OldFileList.SelectedItem as string;
            var list = _expensesVm.oldFiles;
            list.Remove(selected);
            OldFileList.SelectedItems.Clear();
        }
        private async void CheckBox1_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            if (_appSettings.Value.Auto)
                NewFileList.SelectedItems.Clear();


            var files = this.NewFileList.SelectedItems;
            if (files.Count < 1)
                return;

            var selectedString = files[0].ToString();
            var list = _expensesVm.oldFiles;
            list.Add(selectedString);
            NewFileList.SelectedItems.Clear();
            await InvoiceChecked(selectedString);
        }

        private async Task InvoiceChecked(string filename)
        {
            var path = _appSettings.Value.folderPath + @"\\Invoice\\" + filename;
            var destinationPath = _appSettings.Value.folderPath + @"\\Stored\\Invoice\\" + filename;
            await _invoiceService.PostAction(path);
            File.Copy(path, destinationPath, true);
            File.Delete(path);
            _expensesVm?.LoadInvoices();
        }

        private void PopupNewClosed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
        }

        private void SfDatePicker_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_expensesVm is { finishLoading: true })
                _expensesVm?.LoadInvoices();
        }

        private void dataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (errorGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, _gridRowResizingOptions, out _autoHeight))
            {
                if (_autoHeight > 24)
                {
                    e.Height = _autoHeight;
                    e.Handled = true;
                }
            }
        }
        private void NewFileList_ItemChecked(object sender, Syncfusion.Windows.Tools.Controls.ItemCheckedEventArgs e)
        {
            if (NewFileList.IsCheckOnFirstClick)
            {
                return;
            }
            else
            {
                return;
            }

        }
    }
}
