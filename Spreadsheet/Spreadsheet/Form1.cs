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
            public const int numberOfColumns = 26;         // currently accepts (1 - inf)
            public const int numberOfRows = 50;            // currently accepts (1 = inf)
            public const int gridWidth = 800;
            public const int gridHeight = 500;
            public const int formWidth = gridWidth + 30;
            public const int formHeight = gridHeight + 50;
        }

        /// <summary>
        /// This function automatically determines the column headers (based on Constants.numberOfColumns) and 
        /// returns the column headers as an array of strings.
        /// If you'd like to have a special character in the alphabet, add this to the alphabet string below (separated by a delimiter).
        /// </summary>
        /// <returns></returns>
        private string[] ColumnHeaders()
        {
            string[] alphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(new char[] { ',' });
            string[] columnHeaders = new string[Constants.numberOfColumns];
            for (int i = 0; i < Constants.numberOfColumns; ++i)
            {
                string header = "";
                // Loop sets current header; if numberOfColumns > 26, we begin doubling letters. If > 52, triple. And so on.
                for (int j = 0; j <= i / alphabet.Length; j++)        
                {
                    header += alphabet[i % alphabet.Length];        // add current letter in alphabet (i % alphabet.Length) to header.
                }
                columnHeaders.SetValue(header, i);       // Set Header
            }
            return columnHeaders;
        }

        /// <summary>
        /// Fields
        ///     _spreadsheet : Backend spreadsheet object
        ///     _spreadsheet_binding: BindingSource
        ///             - serves to respond to notifications from _spreadsheet; binds data in _spreadsheet to dataGridView1
        /// </summary>
        private SpreadsheetEngine.Spreadsheet _spreadsheet;
        private BindingSource _spreadsheet_binding = new BindingSource();   // use to bind data in _spreadsheet to

        /// <summary>
        /// 
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            _spreadsheet = new SpreadsheetEngine.Spreadsheet(Constants.numberOfRows, Constants.numberOfColumns);
            DelegateEventHandlers();
            SetUpBinding();
            SetUpFormView();
            SetUpDataGridView();
        }

        /// <summary>
        /// Assign event handlers to specific events.
        /// </summary>
        private void DelegateEventHandlers()
        {
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellBeginEdit);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        /// <summary>
        /// Sets up the "Reverse Event Handler" Mechanism. 
        /// Because of this binding, the dataGridView will automatically respond to
        /// events fired in _spreadsheet (due to changes).
        /// </summary>
        private void SetUpBinding()
        {
            // next up!!!!!!!!!!
            this.dataGridView1.DataSource = this._spreadsheet_binding;
        }

        private void SetUpFormView()
        {
            this.Size = new System.Drawing.Size(Constants.formWidth, Constants.formHeight);
        }

        /// <summary>
        /// Disallows user from adding/removing rows.
        /// Initialize columns with headers.
        /// </summary>
        private void SetUpDataGridView()
        {
            // Column Properties Set-Up
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(Constants.gridWidth, Constants.gridHeight);
                // Column Headers
            foreach (string colHeader in ColumnHeaders())
            {
                this.dataGridView1.Columns.Add(colHeader, colHeader);       // add as column header
            }

            // Row Properties Set-Up
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.RowHeadersVisible = true;
            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                // Row Headers
            for (int i = 1; i <= Constants.numberOfRows; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();      // i - 1 since Rows are 0-indexed; humans like 1-indexing though
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SpreadsheetEngine.Spreadsheet.CellEditArgs cea = new SpreadsheetEngine.Spreadsheet.CellEditArgs(
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), e.RowIndex, e.ColumnIndex);
            _spreadsheet.OnCellPropertyChanged(this, cea);    // pass in object that is raising the event & custom event args
        }
    }
}
