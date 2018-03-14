using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SpreadsheetEngine
{
    public class Spreadsheet
    {
        /// <summary>
        /// Fields:
        ///     _spreadsheet
        ///     _row_dim
        ///     _col_dim
        ///     CellEditEventHandler (i.e. the handler and the event)
        /// </summary>
        private Cell[,] _spreadsheet;
        private readonly int _row_dim = 0;
        private readonly int _col_dim = 1;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num_rows"></param>
        /// <param name="num_cols"></param>
        public Spreadsheet(int num_rows, int num_cols)
        {
            _spreadsheet = new Cell[num_rows, num_cols];
            for (int i = 0; i < num_rows; ++i)
            {
                for (int j = 0; j < num_cols; ++j)
                {
                    _spreadsheet[i, j] = new Cell(i, j);    // initialize 2-D array with Cell(i,j) at location [i,j]
                    _spreadsheet[i, j].PropertyChanged += new PropertyChangedEventHandler(OnCellPropertyChanged);    // delegate event handler for every cell in _spreadsheet
                }
            }
        }

        /// <summary>
        /// Returns a reference to the cell at location [row,col]. If not in bounds, returns null.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public AbstractCell GetCell(int row, int col)
        {
            if (row >= _spreadsheet.GetLowerBound(_row_dim) && row <= _spreadsheet.GetUpperBound(_row_dim) &&
                col >= _spreadsheet.GetLowerBound(_col_dim) && col <= _spreadsheet.GetUpperBound(_col_dim))
            {
                return _spreadsheet[row, col];
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns the number of columns in internal spreadsheet (2D array of Cells)
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return _spreadsheet.GetLength(_col_dim);
            }
        }

        /// <summary>
        /// Returns the number of rows in internal spreadsheet (2D array of Cells)
        /// </summary>
        public int RowCount
        {
            get
            {
                return _spreadsheet.GetLength(_row_dim);
            }
        }

        /// <summary>
        /// Handles the event of text change fired by the AbstractCell Class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AbstractCell c = sender as AbstractCell;
            //Console.WriteLine(c.RowIndex.ToString());
            //Console.WriteLine(c.ColumnIndex.ToString());
            switch (e.PropertyName)
            {
                case "Text":
                    c.Value = "yo";
                    PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(e.PropertyName));    // Pass along event to whoever uses this class
                    break;
                default:
                    break;
            }
        }
    }
}
