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
        /// <summary>
        /// Constants that affect behavior of the program-- all in one place that is easy to manage.
        /// </summary>
        static class Constants
        {
            public const int numberOfRows = 50;
            public const string columnHeaders = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";    // can easily add more space-separated header names
            public const int gridWidth = 800;
            public const int gridHeight = 500;
            public const int formWidth = gridWidth;
            public const int formHeight = gridHeight + 50;
        }

        public SpreadsheetForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetUpFormView();
            SetUpDataGridView();
        }

        public void SetUpFormView()
        {
            this.Size = new System.Drawing.Size(Constants.formWidth, Constants.formHeight);
        }

        /// <summary>
        /// Disallows user from adding/removing rows; initializes columns with headers 'A' to 'Z'
        /// </summary>
        private void SetUpDataGridView()
        {
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Size = new System.Drawing.Size(Constants.gridWidth, Constants.gridHeight);
            foreach (string colHeader in Constants.columnHeaders.Split(new char[] { ','}))       // split columnHeaders into separate strings by delimiter ','
            {
                this.dataGridView1.Columns.Add(colHeader, colHeader);       // add as column header
            }
        }
    }
}
