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
            public const int numberOfColumns = 26;         // currently accepts (1 to inf)
            public const int numberOfRows = 50;            // currently accepts (1 to inf)
            public const int gridWidth = 800;
            public const int gridHeight = 500;
            public const int formWidth = gridWidth + 30;
            public const int formHeight = gridHeight + 50;
        }

        /// <summary>
        /// Fields
        ///     _spreadsheet : Backend spreadsheet object
        /// </summary>
        private SpreadsheetEngine.Spreadsheet _spreadsheet;

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
            _spreadsheet.PropertyChanged += new PropertyChangedEventHandler(OnCellPropertyChanged);
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
            foreach (string colHeader in SpreadsheetEngine.Spreadsheet.ColumnHeaders(Constants.numberOfColumns))
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
        /// In "edit" mode, we need to display the cell's TEXT rather than its value, since the 
        /// text is what is actually parsed and evaluated to obtain a value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SpreadsheetEngine.AbstractCell editedCell = _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);
                DataGridView dgv = sender as DataGridView;
                editedCell.Text = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(
                    "You attempted to access a cell that's out of bounds. Here is a warning. Don't do it again please.",
                    "Index Out Of Range Error",
                    MessageBoxButtons.OK
                    );
            }
        }

        /// <summary>
        /// Responsible for handling the event 'PropertyChanged', which is fired
        /// by the AbstractCell class in the backend. Respond by updating the view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetEngine.AbstractCell backendCell = sender as SpreadsheetEngine.AbstractCell;
            switch (e.PropertyName)
            {
                case "Text":
                    try
                    {
                        dataGridView1.Rows[backendCell.RowIndex].Cells[backendCell.ColumnIndex].Value = backendCell.Value;     // display value
                    }
                    catch (ArgumentException ae)
                    {
                        MessageBox.Show(
                            ae.Message,
                            "Invalid Formula",
                            MessageBoxButtons.OK
                    );
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
