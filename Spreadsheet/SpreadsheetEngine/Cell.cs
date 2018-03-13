using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SpreadsheetEngine
{
    public abstract class SpreadsheetCell : INotifyPropertyChanged
    {
        /// <summary>
        /// Fields:
        ///     _text
        ///     _value
        /// </summary>
        protected string _text = "";
        protected string _value = "";
        private int _row_index;
        private int _col_index;

        /// <summary>
        /// Constructor for abstract SpreadsheetCell. Can NOT be instantiated.
        /// </summary>
        /// <param name="r_index"></param>
        /// <param name="c_index"></param>
        public SpreadsheetCell(int r_index, int c_index)
        {
            _row_index = r_index; _col_index = c_index;
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
                    // FIRE PROPERTY CHANGED EVENT
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
            protected set     // only classes within this DLL can set; even classes that inherit from SpreadsheetCell outside of this DLL cannot
            {

            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        protected void OnPropertyChanged()
        {

        }
    }

    public class Spreadsheet
    {
        public Spreadsheet()
        {
            
        }
    }
}
