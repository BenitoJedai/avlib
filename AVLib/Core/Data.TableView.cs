using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVLib.Data
{
    public class TableViewColumn : ObjectProperty
    {
        public TableViewColumn()
        {
        }

        public TableViewColumn(string name, string propertyPath): base(propertyPath)
        {
            m_Name = name;
        }

        private string m_Name = "";
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
    }

    public interface ITableView
    {
        TableViewColumn AddColumn(string name, string propertyPath);
    }

    public class TableView<T> : ITableView
    {
        private IList<T> m_DataSet = null;
        public IList<T> DataSet
        {
            get { return m_DataSet; }
            set { m_DataSet = value; }
        }

        private List<TableViewColumn> m_Columns = new List<TableViewColumn>();
        private Dictionary<string, TableViewColumn> m_ColumnsSorted = new Dictionary<string, TableViewColumn>(StringComparer.OrdinalIgnoreCase);

        public TableViewColumn AddColumn(string name, string propertyPath)
        {
            if (name == null) throw new ApplicationException("Column name cannot be empty");
            if (m_ColumnsSorted.ContainsKey(name)) throw new ApplicationException("Column with name '" + name + "' already exist");
            var col = new TableViewColumn(name, propertyPath);
            m_Columns.Add(col);
            m_ColumnsSorted.Add(col.Name, col);
            col.InitProperty(typeof (T));
            return col;
        }
    }
}
