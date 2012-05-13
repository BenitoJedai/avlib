using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVLib.Data
{
    public class DataViewColumn : ObjectProperty
    {
        private DataViewColumns m_columns = null;

        public DataViewColumn()
        {
            m_Name = NextName;
        }

        public DataViewColumn(DataViewColumns columns): this(columns, NextName, ""){}

        public DataViewColumn(DataViewColumns columns, string name, string propertyPath)
            : base(propertyPath)
        {
            m_columns = columns;
            m_Name = name;
        }

        private string m_Name = "";
        public string Name
        {
            get { return m_Name; }
            set
            {
                if (m_Name != value)
                {
                    if (m_columns != null)
                        m_columns.ChangeColumnName(m_Name, value);
                    else
                        m_Name = value;
                }
            }
        }

        public int Index
        {
            get { return m_columns != null ? m_columns.IndexOf(this) : -1; }
            set
            {
                if (m_columns != null)
                {
                    m_columns.Move(this, value);
                }
            }
        }

        private static int NameIndex = 0;
        private static string NextName
        {
            get { return "col" + ++NameIndex; }
        }
    }

    public class DataViewColumns 
    {
        private DataView m_owner;
        private List<DataViewColumn> m_Columns = new List<DataViewColumn>();
        private Dictionary<string, DataViewColumn> m_ColumnsSorted = new Dictionary<string, DataViewColumn>(StringComparer.OrdinalIgnoreCase);

        public DataViewColumns(DataView owner)
        {
            m_owner = owner;
        }

        public int Count
        {
            get { return m_Columns.Count; }
        }

        public DataViewColumn this[int index]
        {
            get
            {
                if (index >= 0 && index < m_Columns.Count)
                    return m_Columns[index];
                return null;
            }
        }

        public DataViewColumn this[string name]
        {
            get
            {
                DataViewColumn col;
                if (m_ColumnsSorted.TryGetValue(name, out col))
                    return col;
                return null;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return m_Columns.GetEnumerator();
        }

        public DataViewColumn Add(DataViewColumn column)
        {
            if (column.Name == null) throw new ApplicationException("Column name cannot be empty");
            if (m_ColumnsSorted.ContainsKey(column.Name)) throw new ApplicationException("Column with name '" + column.Name + "' already exist");
            m_Columns.Add(column);
            m_ColumnsSorted.Add(column.Name, column);
            column.InitProperty(m_owner.ItemType);
            return column;
        }

        public DataViewColumn Add(string name, string propertyPath)
        {
            var col = new DataViewColumn(this, name, propertyPath);
            return Add(col);
        }

        public int IndexOf(DataViewColumn column)
        {
            if (column == null) return -1;
            return m_Columns.IndexOf(column);
        }

        public int IndexOf(string columnName)
        {
            return IndexOf(this[columnName]);
        }

        public bool IndexExist(int columnIndex)
        {
            return (columnIndex >= 0 && columnIndex < m_Columns.Count);
        }

        public void Move(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return;
            if (!IndexExist(fromIndex) || !IndexExist(toIndex)) return;
            var item = m_Columns[fromIndex];
            m_Columns.RemoveAt(fromIndex);
            m_Columns.Insert(toIndex, item);
        }

        public void Move(DataViewColumn column, int toIndex)
        {
            Move(IndexOf(column), toIndex);
        }

        public bool Contains(DataViewColumn column)
        {
            return IndexOf(column) >= 0;
        }

        public bool Contains(string columnName)
        {
            return m_ColumnsSorted.ContainsKey(columnName);
        }

        protected internal void ChangeColumnName(string oldName, string newName)
        {
            if (oldName == newName) return;
            if (newName == "") throw new ApplicationException("Column name cannot be empty.");
            if (m_ColumnsSorted.ContainsKey(newName)) throw new ApplicationException("Column with same name '" + newName + "' already exist.");
            DataViewColumn col;
            if (m_ColumnsSorted.TryGetValue(oldName, out col))
            {
                m_ColumnsSorted.Remove(oldName);
                col.Name = newName;
                m_ColumnsSorted.Add(col.Name, col);
            }
        }
    }

    public class DataView 
    {
        public DataView(Type itemType)
        {
            if (itemType is ValueType) throw new ApplicationException("ItemType cannot be ValueType.");
            m_itemType = itemType;
            m_columns = new DataViewColumns(this);   
        }

        private IList m_DataSet = null;
        public IList DataSet
        {
            get { return m_DataSet; }
            set { m_DataSet = value; }
        }

        DataViewColumns m_columns = null;
        public DataViewColumns Columns
        {
            get { return m_columns; }
        }

        private Type m_itemType;
        public Type ItemType
        {
            get { return m_itemType; }
        }
    }
}
