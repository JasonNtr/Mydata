using Business.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDataCancel2021Invoices
{
    public partial class InvoiceCancellationForm : Form
    {
        private string _projectDirectory;
        public InvoiceCancellationForm()
        {
            InitializeComponent();
        }

        private void InvoiceCancellationForm_Load(object sender, EventArgs e)
        {
            InitializeFields();
        }

        private void InitializeFields()
        {
            labelLogFile.Select();
            _projectDirectory = Directory.GetParent(AppContext.BaseDirectory).FullName;//Path.GetDirectoryName(projectExePath);
            textBoxLogFilePath.Text = _projectDirectory;
            textBoxLogFilePath.BackColor = System.Drawing.SystemColors.Window;
        }

        private void buttonChooseLogFilePath_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = _projectDirectory;
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    textBoxLogFilePath.Text = folderBrowserDialog.SelectedPath;
                    //string[] files = Directory.GetFiles(fbd.SelectedPath);
                    //MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }
        }

        private void buttonCancelInvoices_ClickAsync(object sender, EventArgs e)
        {

        }

        private void CancelInvoices()
        {
            //var successfullInvoices = await InvoiceRepo.GetInvoicesWithSuccessStatusCodeFor2021();
            //foreach (var invoice in successfullInvoices)
            //{
            //    _ = await _invoiceService.CancelInvoiceBatchProcess(invoice);//invoice.Uid.ToString());         
            //}
            //_invoiceService.CreateLogFileForBatchProcess();
        }
    }
}
