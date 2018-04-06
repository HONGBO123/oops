/*
 * Assignment #5: Spreadsheet Prototype
 * Author: Kyler Little
 * ID: 11472421
 * Last Modified: 3/15/2018 2:20 PM
 * 
 * Notes: Currently doesn't support "cascading changes." Would like to wait on adding this functionality until
 * we cover how to handle expressions. Also, need a break from coding to clear the head.
 */





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
            public const int gridHeightOffset = 35;
            public const int formWidth = gridWidth + 30;
            public const int formHeight = demoButtonVerticalStart + 70;
            public const string demoButtonText = "Do spreadsheet modification demo where changes in engine trigger UI updates.";
            public const int demoButtonWidth = gridWidth;
            public const int demoButtonVerticalStart = gridHeightOffset + gridHeight + 10;
            public const int editBoxHeight = 30;
            public const int editBoxOffset = 7;
        }

        /// <summary>
        /// Fields
        ///     _spreadsheet : Backend spreadsheet object
        /// </summary>
        private SpreadsheetEngine.Spreadsheet _spreadsheet;
        private int _selected_cell_row = 0;
        private int _selected_cell_col = 0;

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
            button1.DoubleClick += new EventHandler(button1_Click);
            dataGridView1.CellEnter += new DataGridViewCellEventHandler(dataGridView1_CellEnter);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            textBox1.Leave += new EventHandler(textBox1_Leave);
            textBox1.EnabledChanged += new EventHandler(textBox1_EnabledChanged);
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
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Size = new System.Drawing.Size(Constants.gridWidth, Constants.gridHeight);
            dataGridView1.Location = new System.Drawing.Point(0, Constants.gridHeightOffset);
                // Column Headers
            foreach (string colHeader in SpreadsheetEngine.Spreadsheet.ColumnHeaders)
            {
                this.dataGridView1.Columns.Add(colHeader, colHeader);       // add as column header
            }

            // Row Properties Set-Up
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
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

            // EditingBox Properties Set-Up
            textBox1.Size = new System.Drawing.Size(dataGridView1.Width - dataGridView1.RowHeadersWidth, Constants.editBoxHeight);
            textBox1.Location = new System.Drawing.Point(dataGridView1.RowHeadersWidth, Constants.editBoxOffset);
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
                cell.Text = (i + 1).ToString();        // i + 1 because 0-indexing to 1-indexing
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
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null) editedCell.Text = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();  // update reference's text
                else editedCell.Text = "";       // if user removes all text... need to set text to nothing
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
                    if (_spreadsheet.Error)      // if error occurred
                    {
                        MessageBox.Show(
                            _spreadsheet.ErrorMessage,
                            "Invalid Formula",
                            MessageBoxButtons.OK
                        );
                        _spreadsheet.Error = false;
                        _spreadsheet.ErrorMessage = "";
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

        /// <summary>
        /// Event handler to handle when the user presses a key. We check if the key pressed
        /// is "Return" or "Enter". If it is, the user is done editing the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Return == e.KeyCode)    // user pressed enter, so done editing
            {
                // Leaving the textbox is the generalized case that the user is done editing, so simply fire that event!
                textBox1_Leave(sender, new EventArgs());
            }
        }

        /// <summary>
        /// When any datagrid view cell becomes in focus, display its text value in textBox1!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = _spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
            _selected_cell_row = e.RowIndex;
            _selected_cell_col = e.ColumnIndex;
        }

        /// <summary>
        /// When textbox1 becomes "disabled", we immediately enable it again. This is solely
        /// for the purpose of updating the view of the textbox with its new text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_EnabledChanged(object sender, EventArgs e)
        {
            if (!textBox1.Enabled) textBox1.Enabled = true;
        }

        /// <summary>
        /// The event handler for when the user exits the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_Leave(object sender, EventArgs e)
        {
            // Set the datagridview's cell's VALUE to whatever the textbox's value is.
            dataGridView1.Rows[_selected_cell_row].Cells[_selected_cell_col].Value = textBox1.Text;

            // Next, simply fire the event for when a datagridview's cell is done editing!
            dataGridView1_CellEndEdit(dataGridView1, new DataGridViewCellEventArgs(_selected_cell_col, _selected_cell_row));

            // Lastly, we set "Enabled" to false, so control returns to the cell in question (which is good)
            textBox1.Enabled = false;
        }
    }
}
