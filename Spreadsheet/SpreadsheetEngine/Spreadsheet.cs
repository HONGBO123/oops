/*
 * Name: Kyler Little
 * ID: 11472421
 */




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
        ///     _dependencies: key : cell name, value : cell names that the key is dependent on
        ///     _row_dim
        ///     _col_dim
        ///     _column_header_alphabet: should be in this DLL because determining a cell reference's indices is dependent upon col headers
        ///     PropertyChanged Event
        ///     _error_occurred
        ///     _error_message
        /// </summary>
        private Cell[,] _spreadsheet;
        private Dictionary<AbstractCell, HashSet<AbstractCell>> _dependencies;
        private readonly int _row_dim = 0;
        private readonly int _col_dim = 1;
        private static string _column_header_alphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _error_occurred = false;
        private string _error_message = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="num_rows"></param>
        /// <param name="num_cols"></param>
        public Spreadsheet(int num_rows, int num_cols)
        {
            _spreadsheet = new Cell[num_rows, num_cols];
            _dependencies = new Dictionary<AbstractCell, HashSet<AbstractCell>>();
            string[] colheaders = ColumnHeaders(num_cols);
            for (int i = 0; i < num_rows; ++i)
            {
                for (int j = 0; j < num_cols; ++j)
                {
                    _spreadsheet[i, j] = new Cell(i, j, colheaders[j] + (i+1).ToString());    // initialize 2-D array with Cell(i,j) at location [i,j] with name colheaders[j](i+1)
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
        /// Boolean Property. If error occurred in the event handler, we must do this workaround solution
        /// because exceptions and events do NOT mix well (it's highly recommended not to mix them).
        /// The user of this class must ensure Error is reset to "false" if the error was handled.
        /// </summary>
        public bool Error
        {
            get
            {
                return _error_occurred;
            }
            set
            {
                _error_occurred = value;
            }
        }

        /// <summary>
        /// Error Message Property. Allows user to view what the problem was.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _error_message;
            }
            set
            {
                _error_message = value;
            }
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

        private void RemoveDependencies(AbstractCell cell)
        {
            if (_dependencies.ContainsKey(cell))     // If the cell had any dependencies,
            {
                _dependencies[cell].Clear();         // then remove them
            }
            _dependencies.Remove(cell);        // Remove key
        }
        
        /// <summary>
        /// Returns true if there are no reference cycles. False otherwise.
        /// EX of Reference Cycle:
        ///     A5 = 5 + B7
        ///     B7 = C5
        ///     C5 = A5
        /// Note that since this is done each time the text is updated, there won't be any cycles
        /// that don't include 'cell.' If there were, we would have already thrown an error.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool NoReferenceCycles(AbstractCell root, AbstractCell cell)
        {
            // BASE CASE:
            if (!_dependencies.ContainsKey(cell)) return true;       // no dependencies

            // RECURSION: 
            bool result = true;
            foreach (AbstractCell ac in _dependencies[cell])
            {
                if (ReferenceEquals(ac, root)) return false;
                result = result && NoReferenceCycles(root, ac);      // AND result of recursive call for each of cell's dependencies-- if one or more cycles, returns false
            }
            return result;
        }

        private void CascadingEffect(AbstractCell cell)
        {
            if (NoReferenceCycles(cell, cell))
            {
                foreach (HashSet<AbstractCell> hs in _dependencies.Values)
                {
                    //_dependencies.
                    // IDEA: if C appears in any of the hashsets, update the key for that hashset
                        // EvaluateFormula(key);
                        //_dependencies.
                }
                foreach (AbstractCell key in _dependencies.Keys)
                {
                    foreach (AbstractCell hashed_val in _dependencies[key])     // Check if cell appears in any of the hashsets
                    {
                        if (ReferenceEquals(hashed_val, cell))       // If so, we need to re-evaluate the formula for the key that mapped to the hashset
                        {
                            EvaluateFormula(key);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Circular reference detected.");
            }
        }

        /// <summary>
        /// Handles the event of text change fired by the AbstractCell Class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //_error_occurred = false;           // in the beginning, no error has occurred
            AbstractCell c = sender as AbstractCell;
            switch (e.PropertyName)
            {
                case "Text":
                    try
                    {
                        RemoveDependencies(c);
                        DetermineCellValue(c);
                        PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));    // Pass along event to whoever uses this class
                        CascadingEffect(c);    // if this changed any other
                    }
                    catch (Exception ex)
                    {
                        _error_occurred = true;
                        _error_message = ex.Message;
                    }
                    // Lastly, if any cells are dependent on c, update these
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Sets the cell's "Value" Property based on its "Text" Property
        /// If "Text" is empty or doesn't begin with "=", then set "Value" to "Text."
        /// Otherwise, evaluate the formula.
        /// </summary>
        /// <param name="cell"></param>
        private void DetermineCellValue(AbstractCell cell)
        {
            if (cell.Text.Length == 0)
            {
                cell.Value = "";
            }
            else if (cell.Text.StartsWith("="))       // formula type
            {
                if (cell.Text.Length > 1)
                {
                    try
                    {
                        EvaluateFormula(cell);
                    }
                    catch
                    {
                        throw;      // propagate error upwards
                    }
                }
                else
                {
                    throw new ArgumentException(_error_message = @"'=' is not a valid formula.");
                }
            }
            else
            {
                Console.WriteLine("modified cell's text: " + cell.Text);
                cell.Value = cell.Text;
            }
        }

        /// <summary>
        /// The passed in cell object's Text field begins with an '=', indicating it is
        /// a formula. This function evaluats that formula.
        /// </summary>
        /// <param name="cell"></param>
        private void EvaluateFormula(AbstractCell cell)
        {
            // For now, assume that the "formula" is just the name of another cell.
            // idea: parse cell into "col header" and "number"
            // convert "col header" to 0-based index "col_index"
            // then cell.Value = _spreadsheet[number - 1, col_index].Value;

            // But in general, we first remove all references that cell has (assume user got rid of all)
            // Next, count references; if number of references > 0, add dependencies

            try
            {
                ExpTree expTree = new ExpTree(cell.Text.Substring(1));    // pass in full string BUT '=' at start to expression tree constructor
                List<string> cellRefs = expTree.GetVariablesInExpression();     // get the cell references that are present in expression
                foreach (string cellName in cellRefs)
                {
                    // note: the "RefToIndices" method should probs throw errors instead of catching them internally
                    int[] indices = ReferenceToIndices(cellName);
                    // throw error ^^^^^^^^^^

                    AbstractCell cellReliesOnThisGuy = GetCell(indices[0], indices[1]);     // throws error if out of bounds
                    // 'cell' DEPENDS on each cell that cellName refers to, so add it to dict
                    if (!_dependencies.ContainsKey(cell)) _dependencies.Add(cell, new HashSet<AbstractCell>());    // we first check if this is a new entry in the dict-- if so, add
                    _dependencies[cell].Add(cellReliesOnThisGuy);

                    // Now, only allow a reference if the referenced cell's value can be converted to a double!
                    bool success = Double.TryParse(cellReliesOnThisGuy.Value, out double result);
                    if (success) expTree.SetVar(cellName, result);                    // now that we have the cell, set its value in the expression tree
                    else throw new ArgumentException(String.Format("{0} is not a value that can be referenced in a formula.", cellReliesOnThisGuy.Value));
                }
                cell.Value = expTree.Eval().ToString();
                
            }
            catch
            {
                throw;       // propagate error up
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
