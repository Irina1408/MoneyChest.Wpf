using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Utils.FileUtils
{
    public class CsvTableReader : IDisposable
    {
        #region Private fields

        private System.IO.StreamReader sr;
        private List<string> columnNames;
        private char separator;
        private CsvRowValues currentRowValues;

        #endregion

        #region Initialization

        public CsvTableReader(string fileName, char separator = ';')
        {
            sr = new System.IO.StreamReader(fileName);
            columnNames = new List<string>();
            this.separator = separator;

            string colNames = sr.ReadLine();
            if (colNames != null)
            {
                columnNames.AddRange(colNames.Split(separator));
            }
        }

        #endregion

        #region Public properties

        public CsvRowValues CurrentRowValues { get { return currentRowValues; } }

        #endregion

        #region Public methods

        public bool ReadLine()
        {
            string values = sr.ReadLine();
            if (values != null)
            {
                var v = values.Split(separator);
                var vals = new Dictionary<string, string>();
                for (int i = 0; i < (v.Length < columnNames.Count ? v.Length : columnNames.Count); i++)
                {
                    vals.Add(columnNames[i].ToUpper(), v[i]);
                }
                currentRowValues = new CsvRowValues(vals);
                return true;
            }

            return false;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            sr.Close();
            sr.Dispose();
            sr = null;
        }

        #endregion
    }

    public class CsvRowValues
    {
        private Dictionary<string, string> values;

        public CsvRowValues(Dictionary<string, string> values)
        {
            this.values = values;
        }

        public string this[string columnName]
        {
            get { return values.ContainsKey(columnName.ToUpper()) ? values[columnName.ToUpper()] : string.Empty; }
        }
    }
}
