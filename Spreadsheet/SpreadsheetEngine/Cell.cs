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
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace SpreadsheetEngine
{
    public abstract class AbstractCell : INotifyPropertyChanged, IXmlSerializable
    {
        /// <summary>
        /// Fields:
        ///     _text
        ///     _value
        ///     _name
        ///     _row_index
        ///     _col_index
        ///     PropertyChanged
        /// </summary>
        protected string _text = "";
        protected string _value = "";
        private readonly string _name = "";
        private readonly int _row_index;
        private readonly int _col_index;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor for AbstractCell. Can NOT be instantiated.
        /// </summary>
        /// <param name="r_index"></param>
        /// <param name="c_index"></param>
        public AbstractCell(int r_index, int c_index, string name)
        {
            _row_index = r_index;
            _col_index = c_index;
            _name = name;
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
                if (value != _value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        /// <summary>
        /// Name property-- ex: "A5"
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Makes an abstract cell hashable
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        /// <summary>
        /// If PropertyChanged isn't null, then we invoke (fire) the event. This additional check makes
        /// sure that anyone dealing with these AbstractCells have a PropertyChangedEventHandler to handle 
        /// the event appropriately.
        /// </summary>
        /// <param name="prop"></param>
        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Return Cell's XmlScheme-- UNIMPLEMENTED
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Given an XmlReader, load data into cell. This is unimplemented because I have some
        /// readonly attributes in the xml tags that cannot be set here
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Xml-ify a cell.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("cell");
            writer.WriteElementString("cellname", this.Name);
            writer.WriteElementString("celltext", this.Text);
            writer.WriteEndElement();
        }
    }

    /// <summary>
    /// Instanciable Cell
    /// </summary>
    public class Cell : AbstractCell
    {
        public Cell(int r_index, int c_index, string name) : base(r_index, c_index, name) { }
    }
}
