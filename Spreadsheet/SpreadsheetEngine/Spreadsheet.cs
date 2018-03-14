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
        public delegate void CellEditEventHandler(object sender, CellEditArgs cea);
        public event CellEditEventHandler CellPropertyChanged;

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
                    _spreadsheet.SetValue(new Cell(i, j), new int[] { i, j });     // initialize 2-D array with Cell(i,j) at location [i,j]
                }
            }
            this.CellPropertyChanged += new CellEditEventHandler(OnCellPropertyChanged);
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
                return _spreadsheet[_row_dim, _col_dim];
            }
            else
            {
                return null;
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
        /// 
        /// </summary>
        public class CellEditArgs : EventArgs
        {
            private string _text;
            private int _row_index;
            private int _col_index;
            public CellEditArgs(string text, int row_index, int col_index)
            {
                _text = text;
                _row_index = row_index;
                _col_index = col_index;
            }
            public int RowIndex
            {
                get { return _row_index; }
            }

            public int ColumnIndex
            {
                get { return _col_index; }
            }

            public string Text
            {
                get { return _text; }
            }
        }

        public void OnCellPropertyChanged(object sender, CellEditArgs cea)
        {
            if (CellPropertyChanged != null)
            {
                Console.WriteLine(cea.Text);
                Console.WriteLine("col " + cea.ColumnIndex.ToString() + " row: " + cea.RowIndex.ToString());
            }
        }
    }
}
