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
    public partial class SpreadsheetForm : Form
    {
        public SpreadsheetForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetUpLayout();
            
        }

        /// <summary>
        /// Disallows user from adding/removing rows; initializes columns with headers 'A' to 'Z'
        /// </summary>
        private void SetUpLayout()
        {
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            for (int i = (int)'A'; i <= (int)'Z'; ++i)
            {
                this.dataGridView1.Columns.Add(Convert.ToChar(i).ToString(), Convert.ToChar(i).ToString());
            }
        }
    }
}
