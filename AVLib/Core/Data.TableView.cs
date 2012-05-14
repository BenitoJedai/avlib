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

    public class DataRow
    {
        private DataRows m_owner = null;
        private object m_value = null;
        private int m_DataIndex = -1;
        private bool m_Expanded = false;
        private DataRow m_Parent = null;
        private DataRows m_childs = null;

        public DataRows Owner
        {
            get { return m_owner; }
            internal set { m_owner = value; }
        }
        public DataRows Childs
        {
            get { return m_childs; }
            internal set { m_childs = value; }
        }
        public object Value
        {
            get { return m_value; }
            internal set { m_value = value; }
        }
        public int DataIndex
        {
            get { return m_DataIndex; }
            internal set { m_DataIndex = value; }
        }
        public bool Expanded
        {
            get { return m_Expanded; }
            set { m_Expanded = value; }
        }
        public DataRow Parent
        {
            get { return m_Parent; }
            internal set { m_Parent = value; }
        }
        public int Count
        {
            get { return m_childs == null ? 0 : m_childs.Count; }
        }
        public int ExpandedCount
        {
            get { return (m_Expanded && m_childs != null) ? m_childs.Count : 0; }
        }
        public DataRow this[int index]
        {
            get { return m_childs[index]; }
        }
        public int Level
        {
            get
            {
                var rez = 0;
                var p = m_Parent;
                while (p != null)
                {
                    p = p.Parent;
                    rez++;
                }
                return rez;
            }
        }
    }

    public interface IHidenAccessor
    {
        object this[int col, int row] { get; set; }
        object this[string col, int row] { get; set; }
    }

    public class DataRows : IEnumerable<DataRow>, IHidenAccessor
    {
        private DataView m_owner = null;
        private DataViewColumns m_columns = null;
        private List<DataRow> m_visible = new List<DataRow>();
        private List<DataRow> m_hiden = new List<DataRow>();

        public DataView Owner
        {
            get { return m_owner; }
            internal set { m_owner = value; }
        }

        internal DataViewColumns Columns
        {
            get { return m_columns; }
            set { m_columns = value; }
        }

        public IEnumerator<DataRow> GetEnumerator()
        {
            return m_visible.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_visible.GetEnumerator();
        }

        public int Count
        {
            get { return m_visible.Count; }
        }

        public DataRow this[int index]
        {
            get { return m_visible[index]; }
        }

        public List<DataRow> VisibleRows
        {
            get { return m_visible; }
        }

        public List<DataRow> HidenRows
        {
            get { return m_hiden; }
        }

        public IHidenAccessor Hiden
        {
            get { return this; }
        }

        public void Clear()
        {
            m_visible.Clear();
            m_hiden.Clear();
        }

        public void Add(DataRow row)
        {
            row.Owner = this;
            m_visible.Add(row);
        }

        public void Hide(int index)
        {
            m_hiden.Add(m_visible[index]);
            m_visible.RemoveAt(index);
        }

        public void UnHide(int hidenIndex)
        {
            m_visible.Add(m_hiden[hidenIndex]);
            m_hiden.RemoveAt(hidenIndex);
        }

        public void Delete(int index, bool setDeleted)
        {
            if (setDeleted && m_owner != null)
                m_owner.SetDeleted(m_visible[index]);
            m_visible.RemoveAt(index);
        }

        public void Delete(int index)
        {
            Delete(index, false);
        }

        public object this[int col, int row]
        {
            get
            {
                if (m_columns == null) return null;
                return m_columns[col][m_visible[row].Value];
            }
            set
            {
                if (m_columns == null) return;
                m_columns[col][m_visible[row].Value] = value;
            }
        }

        public object this[string col, int row]
        {
            get
            {
                if (m_columns == null) return null;
                return m_columns[col][m_visible[row].Value];
            }
            set
            {
                if (m_columns == null) return;
                m_columns[col][m_visible[row].Value] = value;
            }
        }

        object IHidenAccessor.this[int col, int row]
        {
            get
            {
                if (m_columns == null) return null;
                return m_columns[col][m_hiden[row].Value];
            }
            set
            {
                if (m_columns == null) return;
                m_columns[col][m_hiden[row].Value] = value;
            }
        }

        object IHidenAccessor.this[string col, int row]
        {
            get
            {
                if (m_columns == null) return null;
                return m_columns[col][m_hiden[row].Value];
            }
            set
            {
                if (m_columns == null) return;
                m_columns[col][m_hiden[row].Value] = value;
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
            set
            {
                m_DataSet = value;
                ReadDataSet();
            }
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

        private bool m_IsTree = false;
        DataRows m_rows = new DataRows();
        DataRows m_tree = new DataRows();
        DataRows m_deleted = new DataRows();

        public DataRows VisibleRows
        {
            get { return m_rows; }
        }

        public DataRows Rows
        {
            get { return m_IsTree ? m_tree : m_rows; }
        }

        public DataRows Deleted
        {
            get { return m_deleted; }
        }

        private void ClearDataRows()
        {
            m_rows.Clear();
            m_tree.Clear();
            m_deleted.Clear();
        }

        public void Clear()
        {
            ClearDataRows();
            m_DataSet.Clear();
        }

        private void ReadDataSet()
        {
            ClearDataRows();
            if (m_DataSet == null) return;
            for (int i = 0; i < m_DataSet.Count; i++)
            {
                var r = new DataRow();
                r.Value = m_DataSet[i];
                r.DataIndex = i;
                m_rows.Add(r);
            }
        }

        internal void SetDeleted(DataRow row)
        {
            if (row.Childs != null)
            {
                for (int i = 0; i < row.Childs.VisibleRows.Count - 1; i++)
                    SetDeleted(row.Childs.VisibleRows[i]);
                for (int i = 0; i < row.Childs.HidenRows.Count - 1; i++)
                    SetDeleted(row.Childs.HidenRows[i]);
                row.Childs = null;
            }
            if (row.DataIndex >= 0) m_deleted.Add(row);
        }
    }
}
