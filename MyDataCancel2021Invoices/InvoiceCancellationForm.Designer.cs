
namespace MyDataCancel2021Invoices
{
    partial class InvoiceCancellationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancelInvoices = new System.Windows.Forms.Button();
            this.textBoxLogFilePath = new System.Windows.Forms.TextBox();
            this.buttonChooseLogFilePath = new System.Windows.Forms.Button();
            this.labelLogFile = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Controls.Add(this.buttonCancelInvoices, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLogFilePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonChooseLogFilePath, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLogFile, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(825, 207);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonCancelInvoices
            // 
            this.buttonCancelInvoices.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonCancelInvoices, 4);
            this.buttonCancelInvoices.Location = new System.Drawing.Point(357, 74);
            this.buttonCancelInvoices.Name = "buttonCancelInvoices";
            this.buttonCancelInvoices.Size = new System.Drawing.Size(110, 52);
            this.buttonCancelInvoices.TabIndex = 0;
            this.buttonCancelInvoices.Text = "ΑΚΥΡΩΣΗ";
            this.buttonCancelInvoices.UseVisualStyleBackColor = true;
            this.buttonCancelInvoices.Click += new System.EventHandler(this.buttonCancelInvoices_ClickAsync);
            // 
            // textBoxLogFilePath
            // 
            this.textBoxLogFilePath.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxLogFilePath, 2);
            this.textBoxLogFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogFilePath.Location = new System.Drawing.Point(123, 3);
            this.textBoxLogFilePath.Name = "textBoxLogFilePath";
            this.textBoxLogFilePath.ReadOnly = true;
            this.textBoxLogFilePath.Size = new System.Drawing.Size(618, 23);
            this.textBoxLogFilePath.TabIndex = 1;
            // 
            // buttonChooseLogFilePath
            // 
            this.buttonChooseLogFilePath.Location = new System.Drawing.Point(747, 3);
            this.buttonChooseLogFilePath.Name = "buttonChooseLogFilePath";
            this.buttonChooseLogFilePath.Size = new System.Drawing.Size(66, 23);
            this.buttonChooseLogFilePath.TabIndex = 2;
            this.buttonChooseLogFilePath.Text = "Επιλογή..";
            this.buttonChooseLogFilePath.UseVisualStyleBackColor = true;
            this.buttonChooseLogFilePath.Click += new System.EventHandler(this.buttonChooseLogFilePath_Click);
            // 
            // labelLogFile
            // 
            this.labelLogFile.AutoSize = true;
            this.labelLogFile.Location = new System.Drawing.Point(3, 7);
            this.labelLogFile.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.labelLogFile.Name = "labelLogFile";
            this.labelLogFile.Size = new System.Drawing.Size(114, 15);
            this.labelLogFile.TabIndex = 3;
            this.labelLogFile.Text = "Τοποθεσία Log File :";
            // 
            // InvoiceCancellationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 207);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "InvoiceCancellationForm";
            this.Text = "InvoiceCancellationForm";
            this.Load += new System.EventHandler(this.InvoiceCancellationForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonCancelInvoices;
        private System.Windows.Forms.TextBox textBoxLogFilePath;
        private System.Windows.Forms.Button buttonChooseLogFilePath;
        private System.Windows.Forms.Label labelLogFile;
    }
}