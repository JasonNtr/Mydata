using Business.Services;
using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Mydata.Renderers;
using Mydata.UiModels;
using Mydata.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.ScrollAxis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Domain.Enums.Enums;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for InvoicesUserControl.xaml
    /// </summary>
    public partial class InvoicesUserControl : UserControl
    {
        
        private readonly string _connectionString;
       
        private InvoicesViewModel _viewmodel;

        private MyDataInvoiceDTO _selectedInvoice;




        private int _rowHeight = 30;

        public InvoicesUserControl(IOptions<AppSettings> appSettings, string conenctionString)
        {
            InitializeComponent();
            _connectionString = conenctionString;
            _viewmodel = new InvoicesViewModel(appSettings, conenctionString);
            this.DataContext = _viewmodel;

            this.editGrid.CellRenderers.Remove("ComboBox");
            //Customized combobox cell renderer is added.
            this.editGrid.CellRenderers.Add("ComboBox", new ComboBoxRenderer());

            FillSources();
        }

        private void FillSources()
        {
            GridComboBoxColumn2.ItemsSource = Enum.GetValues(typeof(Domain.Enums.Enums.IncomeClassificationCategoryType)).Cast<IncomeClassificationCategoryType>().ToList();
        }

        private void ViewClicked(object sender, RoutedEventArgs e)
        {
            var rowDataContent = sfGrid.SelectedItem as MyDataInvoiceDTO;

            if (rowDataContent.invoiceMark.Length != 0 && rowDataContent.MyDataCancellationResponses.Count == 0)
            {
                ShowInvoiceLinesToEdit(rowDataContent);
                return;
            }

            List<MyGenericErrorsDTO> errors;
            var myDataResponseRepo = new MyDataResponseRepo(_connectionString);

            if (rowDataContent.MyDataCancellationResponses.Count == 0)
            {
                errors = myDataResponseRepo.MapToGenericErrorDTO(rowDataContent.MyDataResponses.OrderBy(x => x.Created).Last().errors);
            }
            else
            {
                errors = myDataResponseRepo.MapToGenericErrorDTOCancellation(rowDataContent.MyDataCancellationResponses.OrderBy(x => x.Created).Last().Errors);
            }

            if (errors.Count > 0)
            {
                _viewmodel.MyDataErrorDTOs.Clear();
                foreach (var myDataErrorDTO in errors)
                {
                    _viewmodel.MyDataErrorDTOs.Add(myDataErrorDTO);
                }

                var maxLength = errors.Max(x => x.Message.Length);
                var lines = maxLength / 10;
                _rowHeight = 5 * lines;

                if (_rowHeight < 25) _rowHeight = 30;
            }

            Popupnew.IsPopupOpen = true;
            MainGrid.Opacity = 0.2;
        }

        private async void ShowInvoiceLinesToEdit(MyDataInvoiceDTO invoice)
        {
            var particleRepo = new ParticleRepo(_connectionString);
            var particleDTO = await particleRepo.GetParticleByRec0(invoice.Uid);
            _viewmodel.SelectedInvoice = invoice;

            if (particleDTO.Pmoves.Count == 0) return;
            _viewmodel.IncomeClassificationsForEdit.Clear();

            foreach (var item in particleDTO.Pmoves)
            {
                var type = (IncomeClassificationValueType)System.Enum.Parse(typeof(IncomeClassificationValueType), particleDTO.Ptyppar.TYPOS_XARAKTHR);
                var category = (IncomeClassificationCategoryType)System.Enum.Parse(typeof(IncomeClassificationCategoryType), item.ItemDTO.Category);
                var incomeClassificationForEdit = new IncomeClassificationForEditInvoice
                {
                    ItemDescription = item.ItemDTO.ITEM_DESCR,
                    CharacterizationType = type,
                    CharacterizationCategory = category,
                    Amount = (decimal)item.PMS_AMAFTDISC
                };
                _viewmodel.IncomeClassificationsForEdit.Add(incomeClassificationForEdit);
            }

            PopupEdit.IsPopupOpen = true;
            MainGrid.Opacity = 0.2;
        }

        private void PopupNewClosed(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Opacity = 1;
        }

        private void ErrorGrid_OnQueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex == 0) return;
            e.Height = _rowHeight;
            e.Handled = true;
        }

        private void SfGrid2_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var items = sfGrid2.SelectedItems;
            var list = new ObservableCollection<DataGridParticle>();
            foreach (var item in items)
            {
                var myItem = item as DataGridParticle;
                list.Add(myItem);
            }
            _viewmodel.SelectedParticles = new ObservableCollection<DataGridParticle>(list);
        }

        private void SfGrid_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var item = (MyDataInvoiceDTO)sfGrid.SelectedItem;
            _selectedInvoice = item;

            var particles = (ObservableCollection<DataGridParticle>)sfGrid2.ItemsSource;
            var particle = particles.FirstOrDefault(x => x.Rec0 == item.Uid);
            var rowindex = this.sfGrid2.ResolveToRowIndex(particle);
            if (particle == null) return;
            var columnindex = this.sfGrid2.ResolveToStartColumnIndex();

            if (rowindex == particles.Count) rowindex--;
            this.sfGrid2.ScrollInView(new RowColumnIndex(rowindex, columnindex));

            this.sfGrid2.View.MoveCurrentTo(particle);
            sfGrid2.SelectedItems.Clear();
            if (rowindex == 0) rowindex++;
            sfGrid2.SelectedIndex = rowindex - 1;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PopupEdit.IsPopupOpen = false;
            MainGrid.Opacity = 1;
        }

        private void EditGrid_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var item = (MyDataInvoiceDTO)sfGrid.SelectedItem;
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            _viewmodel.ExportJson();
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            _viewmodel.ExportXml();
        }

        private void ExportXmlInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInvoice is not null && !string.IsNullOrEmpty(_selectedInvoice.StoredXml))
            {
                _viewmodel.ExportXml(_selectedInvoice.StoredXml);
            }
        }
    }
}