using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SpreadsheetEngine
{
    public abstract class AbstractCell : INotifyPropertyChanged
    {
        /// <summary>
        /// Fields:
        ///     _text
        ///     _value
        ///     _row_index
        ///     _col_index
        /// </summary>
        protected string _text = "";
        protected string _value = "";
        private readonly int _row_index;
        private readonly int _col_index;

        /// <summary>
        /// Constructor for AbstractCell. Can NOT be instantiated.
        /// </summary>
        /// <param name="r_index"></param>
        /// <param name="c_index"></param>
        public AbstractCell(int r_index, int c_index)
        {
            _row_index = r_index;
            _col_index = c_index;
        }

        /// <summary>
        /// Return row index (not set ability)
        /// </summary>
        public int RowIndex
        {
            get
            {
                return _row_index;
            }
        }

        /// <summary>
        /// Return column index (no set ability)
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return _col_index;
            }
        }

        /// <summary>
        /// Text Property
        ///     Setter first checks if value is equal to _text. If not, sets and fires PropertyChanged event.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// Value property-- READ ONLY.
        ///     Represents the "evaluated" value of the cell.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            internal set     // only classes within this DLL can set; even classes that inherit from AbstractCell outside of this DLL cannot
            {
                _value = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;       

        /// <summary>
        /// If PropertyChanged isn't null, then we invoke (fire) the event. This additional check makes
        /// sure that anyone dealing with these AbstractCells have a PropertyChangedEventHandler to handle 
        /// the event appropriately.
        /// </summary>
        /// <param name="prop"></param>
        protected void OnPropertyChanged(string prop)
        {
            //Console.Write("text:" + _text);
            //Console.WriteLine("value:" + _value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    /// <summary>
    /// Instanciable Cell
    /// </summary>
    public class Cell : AbstractCell
    {
        public Cell(int r_index, int c_index) : base(r_index, c_index) { }
    }
}
