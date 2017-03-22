using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace SWSoft.Forms
{
    public class ListViewBuffer : ListView
    {
        /// <summary>
        /// ��ȡ�����ô� SWSoft.Forms.ListViewBuffer ������Դ
        /// </summary>
        public object DataSource
        {
            get { return this.Tag; }
            set
            {
                if (value != null)
                {
                    Clear();
                    if (value.GetType() == typeof(DataTable))
                    {
                        var table = value as DataTable;
                        foreach (DataColumn item in table.Columns)
                        {
                            Columns.Add(item.ColumnName);
                        }
                        foreach (var item in table.Rows)
                        {

                        }
                    }
                }
            }
        }
        public ListViewBuffer()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
