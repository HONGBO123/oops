using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spreadsheet
{
    public partial class SpreadsheetAboutForm : Form
    {
        public SpreadsheetAboutForm()
        {
            InitializeComponent();
        }

        private void SpreadsheetAboutForm_Load(object sender, EventArgs e)
        {
            SetFormProperties();   
            SetAboutText();
            SetTextBoxProperties();
        }

        private void SetFormProperties()
        {
            this.Size = new Size(530, 300);
            richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(Link_Clicked);
        }

        private void SetTextBoxProperties()
        {
            // Set General TextBox Properties
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = this.Size;

            // Modify the text
            richTextBox1.Find(SpreadsheetForm.AboutInformation.applicationName);
            richTextBox1.SelectionFont = new Font("Verdana", 16, FontStyle.Bold);
            richTextBox1.Select(SpreadsheetForm.AboutInformation.applicationName.Length, richTextBox1.TextLength - SpreadsheetForm.AboutInformation.applicationName.Length);
            richTextBox1.SelectionFont = new Font("Verdana", 14, FontStyle.Regular);
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.DeselectAll();
            
            // stop the cursor from blinking...
            //richTextBox1.Text += richTextBox1.Rtf.
        }

        private void SetAboutText()
        {
            richTextBox1.Text = SpreadsheetForm.AboutInformation.applicationName + Environment.NewLine +
                SpreadsheetForm.AboutInformation.programVersion + Environment.NewLine +
                SpreadsheetForm.AboutInformation.author + Environment.NewLine +
                SpreadsheetForm.AboutInformation.email + Environment.NewLine +
                SpreadsheetForm.AboutInformation.copyright + Environment.NewLine +
                SpreadsheetForm.AboutInformation.license + Environment.NewLine +
                SpreadsheetForm.AboutInformation.licenseLink;
        }

        protected void Link_Clicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
