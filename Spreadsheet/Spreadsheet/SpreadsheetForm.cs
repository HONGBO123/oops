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
            public const int formHeight = demoButtonVerticalStart + 70;
            public const string demoButtonText = "Do spreadsheet modification demo where changes in engine trigger UI updates.";
            public const int demoButtonWidth = gridWidth;
            public const int demoButtonVerticalStart = gridHeight + 10;
        }

        /// <summary>
        /// Fields
        ///     _spreadsheet : Backend spreadsheet object
        /// </summary>
        private SpreadsheetEngine.Spreadsheet _spreadsheet;

        /// <summary>
        /// Constructor
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// What occurs when the form loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ObjectInitialization();
            DelegateEventHandlers();
            SetUpFormView();
            SetUpDataGridView();
        }

        /// <summary>
        /// Initialize any internal objects.
        /// </summary>
        private void ObjectInitialization()
        {
            _spreadsheet = new SpreadsheetEngine.Spreadsheet(Constants.numberOfRows, Constants.numberOfColumns);
        }

        /// <summary>
        /// Assign event handlers to specific events.
        /// </summary>
        private void DelegateEventHandlers()
        {
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellBeginEdit);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            _spreadsheet.PropertyChanged += new PropertyChangedEventHandler(OnCellPropertyChanged);
            button1.Click += new EventHandler(button1_Click);
        }

        /// <summary>
        /// Set up the form properties.
        /// </summary>
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

            // Button1 Properties Set-Up
            button1.Text = Constants.demoButtonText;
            button1.Width = Constants.demoButtonWidth;
            button1.Location = new Point(0, Constants.demoButtonVerticalStart);
        }

        /// <summary>
        /// Demo: randomly assign 50 cells to a test string; set column B cells to "This is cell B#";
        /// and set column A cells to "=B#".
        /// </summary>
        private void Demo()
        {
            // First, randomly assign 50 cells to "Testing" string.
            Random rnd = new Random();     // construct random instance
            for (int i = 0; i < 50; i++)
            {
                SpreadsheetEngine.AbstractCell cell = _spreadsheet.GetCell(rnd.Next(0,Constants.numberOfRows), rnd.Next(0,Constants.numberOfColumns));   // get reference to random cell
                cell.Text = "Testing";       // set cell's text to the test message
            }

            // Second, assign every row in column B to "This is cell B#." Where # is actual row number.
            for (int i = 0; i < Constants.numberOfRows; ++i)
            {
                SpreadsheetEngine.AbstractCell cell = _spreadsheet.GetCell(i, 1);    // 1 == column B
                cell.Text = "This is cell B" + (i + 1).ToString();        // i + 1 because 0-indexing to 1-indexing
            }

            // Lastly, set every row in column B to "=B#", where # is actual row number.
            for (int i = 0; i < Constants.numberOfRows; ++i)
            {
                SpreadsheetEngine.AbstractCell cell = _spreadsheet.GetCell(i, 0);    // 0 == column A
                cell.Text = "=B" + (i + 1).ToString();     // i + 1 because 0-indexing to 1-indexing
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
        /// Event handler for "cell end edit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                SpreadsheetEngine.AbstractCell editedCell = _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);    // grab reference to backend cell being edited
                editedCell.Text = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();                   // update reference's text
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editedCell.Value;                             // display the value rather than the text.
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(
                    ex.Message,
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
                case "Value":
                    dataGridView1.Rows[backendCell.RowIndex].Cells[backendCell.ColumnIndex].Value = backendCell.Value;     // display value
                    if (_spreadsheet.Error)
                    {
                        MessageBox.Show(
                            _spreadsheet.ErrorMessage,
                            "Invalid Formula",
                            MessageBoxButtons.OK
                        );
                        _spreadsheet.Error = false;       // have to do this workaround error propagation because mixing exceptions and event handlers is highly frowned upon
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler for demo_button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Demo();
        }
    }
}
