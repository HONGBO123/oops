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
        private static string _column_header_alphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
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
        /// This function automatically determines the column headers (based on Constants.numberOfColumns) and 
        /// returns the column headers as an array of strings.
        /// If you'd like to have a special character in the alphabet, add this to the alphabet string below (separated by a delimiter).
        /// </summary>
        /// <returns></returns>
        public static string[] ColumnHeaders(int numberOfColumns)
        {
            string[] alphabet = _column_header_alphabet.Split(new char[] { ',' });
            string[] columnHeaders = new string[numberOfColumns];
            for (int i = 0; i < numberOfColumns; ++i)
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
        /// Returns a reference to the cell at location [row,col]. If not in bounds, throws IndexOutOfRangeException
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
                    DetermineCellValue(ref c);
                    break;
                default:
                    break;
            }
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(e.PropertyName));    // Pass along event to whoever uses this class
        }

        /// <summary>
        /// Sets the cell's "Value" Property based on its "Text" Property
        /// If "Text" is empty or doesn't begin with "=", then set "Value" to "Text."
        /// Otherwise, evaluate the formula.
        /// </summary>
        /// <param name="cell"></param>
        private void DetermineCellValue(ref AbstractCell cell)
        {
            if (cell.Text.Length == 0)
            {
                cell.Value = "";
            }
            else if (cell.Text.StartsWith("="))       // formula type
            {
                if (cell.Text.Length > 1)
                {
                    EvaluateFormula(ref cell);
                }
                else
                {
                    throw new ArgumentException(@"'=' is not a valid formula.");
                }
            }
            else
            {
                cell.Value = cell.Text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        private void EvaluateFormula(ref AbstractCell cell)
        {
            // For now, assume that the "formula" is just the name of another cell.
            // idea: parse cell into "col header" and "number"
            // convert "col header" to 0-based index "col_index"
            // then cell.Value = _spreadsheet[number - 1, col_index].Value;
            try
            {
                int[] indices = ReferenceToIndices(cell.Text.Substring(1));      // pass in full string BUT '=' at start
                cell.Value = GetCell(indices[0], indices[1]).Value;
            }
            catch
            {
                throw;     // pass error further up to be handled by SpreadsheetForm
            }

        }

        /// <summary>
        /// Precondition: cell_ref length is > 1.
        /// The algorithm works as follows. The argument passed in is the cell reference as a string (ex: "A5").
        /// We walk through the string and count the number of consecutive capital letters of the same value.
        /// Currently, something like "AB5" wouldn't throw an error so I'd need to fix that.
        /// Anyways, once we arrive at a character that's different from the first character, we break the loop
        /// and assume that the rest of the string is the row number. If we get an error when parsing this, then
        /// that means the column header wasn't correct.
        /// </summary>
        /// <param name="cell_ref"></param>
        /// <returns></returns>
        private int[] ReferenceToIndices(string cell_ref)
        {
            int[] indices = new int[_spreadsheet.Rank];         // default values are 0s
            int i = 0, letterRepetitions = 0;      // keep track of index so we know when the start of the int (row number) begins
            char current_char = char.ToUpper(cell_ref[0]);       // first letter
            Console.WriteLine("cell ref: " + cell_ref);
            for (i = 0; i < cell_ref.Length; ++i)
            {
                if (current_char != cell_ref[i])     // cell_ref[i] is no longer the same column header letter
                {
                    break;
                }
                ++letterRepetitions;
            }
            try
            {
                indices[0] = int.Parse(cell_ref.Substring(i));
            }
            catch (FormatException)
            {
                throw new ArgumentException("A column header is consecutive capital letters. Please fix this.");
            }
            indices[0] -= 1;         // it's 1-indexed, but we need 0-indexed.
            string[] string_alphabet = _column_header_alphabet.Split(new char[] { ',' });
            string alphabet = string.Join("", string_alphabet, 0, string_alphabet.Length - 1);       // "ABCDE..."
            indices[1] = (alphabet.Length * --letterRepetitions) + alphabet.IndexOf(current_char);   // Column is simply (alphabet length * (letterRepetitions - 1)) + letter - 'A'...
            return indices;
        }
    }
}
