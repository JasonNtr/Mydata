using System;
using System.Collections.Generic;
using System.Data;
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

namespace Mydata.UserControls
{
    /// <summary>
    /// Interaction logic for IncomesUserControl.xaml
    /// </summary>
    public partial class IncomesUserControl : UserControl
    {
        private readonly IIncomeRepo _incomeRepo;
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;
        private double _autoHeight;
        private readonly IncomesVM _incomesVm;
        private readonly GridRowSizingOptions _gridRowResizingOptions;
        private readonly IOptions<AppSettings> _appSettings;
        private List<string> _currentFiles;
        public IncomesUserControl(IIncomeRepo incomeRepo, IIncomeService invoiceService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            InitializeComponent();
            _incomesVm = new IncomesVM(incomeRepo);
            this.DataContext = _incomesVm;
            _mapper = mapper;
            this.errorGrid.QueryRowHeight += dataGrid_QueryRowHeight;
            _incomeRepo = incomeRepo;
            _appSettings = appSettings;
            _incomeService = invoiceService;
            _gridRowResizingOptions = new GridRowSizingOptions();
            _currentFiles = new List<string>();
            SfDatePicker1.ValueChanged += SfDatePicker_OnValueChanged;
            SfDatePicker2.ValueChanged += SfDatePicker_OnValueChanged;
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2500)
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
            var fileDirectory = _appSettings.Value.folderPath + @"\\Income";
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
            _incomesVm.ShowFilesToCheckBoxList(_currentFiles);

            if (_appSettings.Value.Auto && list.Count > 0)
            {
                var filename = list.FirstOrDefault();
                //await InvoiceChecked(filename);
            }
            _isSearchFinish = true;

        }

        private async Task InvoiceChecked(string filename)
        {
            var path = _appSettings.Value.folderPath + @"\\Income\\" + filename;
            var destinationPath = _appSettings.Value.folderPath + @"\\Stored\\Income\\" + filename;
            await _incomeService.PostIncome(path);
            File.Copy(path, destinationPath, true);
            File.Delete(path);
            _incomesVm?.LoadInvoices();
        }

        public IncomesUserControl()
        {
            InitializeComponent();
            _gridRowResizingOptions = new GridRowSizingOptions();
        }


        private void ViewClicked(object sender, RoutedEventArgs e)
        {
            var rowDataContent = sfGrid.SelectedItem as MyDataIncomeDTO;
            var latestResponse = rowDataContent.MyDataIncomeResponses;

            var errors = new List<MyGenericErrorsDTO>();

            if (latestResponse.Count != 0)
            {
                errors = _mapper.Map<List<MyGenericErrorsDTO>>(rowDataContent.MyDataIncomeResponses.Last()?.Errors);    
            }
            

            if (errorGrid.DataContext is IncomesVM viewmodel)
            {
                viewmodel.mydataErrorDTOs.Clear();
                foreach (var myDataErrorDTO in errors)
                {
                    viewmodel.mydataErrorDTOs.Add(myDataErrorDTO);
                }
            }

            Popupnew.IsPopupOpen = true;
            MainGrid.Opacity = 0.2;
        }

        private void CheckBox2_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            var selected = OldFileList.SelectedItem as string;
            var list = _incomesVm.oldFiles;
            list.Remove(selected);
            OldFileList.SelectedItems.Clear();
        }

        private async void CheckBox1_ItemChecked(object sender, SelectionChangedEventArgs e)
        {
            if (_appSettings.Value.Auto)
                NewFileList.SelectedItems.Clear();

            var files = this.NewFileList.SelectedItems;
            if (files.Count < 1)
            {
                var x = NewFileList.SelectedItems;
                return;
            }


            var selectedString = files[0].ToString();
            var list = _incomesVm.oldFiles;
            list.Add(selectedString);
            NewFileList.SelectedItems.Clear();
            await InvoiceChecked(selectedString);
        }

        private void PopupNewClosed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
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

        private void SfDatePicker_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_incomesVm is { finishLoading: true })
                _incomesVm?.LoadInvoices();
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

    }
}
