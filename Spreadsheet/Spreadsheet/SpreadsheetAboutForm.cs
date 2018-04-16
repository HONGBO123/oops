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
            SetTextBoxProperties();
            SetAboutText();
        }

        private void SetTextBoxProperties()
        {
            richTextBox1.Size = this.Size;
            // stop the cursor from blinking...
        }

        private void SetAboutText()
        {
            richTextBox1.Text = "Testing";
        }
    }
}
