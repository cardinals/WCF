using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SWSoft.Reflector
{
    public partial class DBHelp
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public SqlConnection Connection { get; set; }

        protected string GetJavaType(string sqlDbType)
        {
            switch (sqlDbType)
            {
                case "bigint": return "Long";
                case "bit": return "Boolean";
                case "char": return "String";
                case "datetime": return "Timestamp";
                case "decimal": return "BigDecimal";
                case "float": return "Double";
                case "int": return "Integer";
                case "money": return "BigDecimal";
                case "nchar": return "String";
                case "ntext": return "String";
                case "numeric": return "BigDecimal";
                case "nvarchar": return "String";
                case "real": return "Float";
                case "smalldatetime": return "Timestamp";
                case "smallint": return "Short";
                case "smallmoney": return "BigDecimal";
                case "text": return "String";
                case "tinyint": return "Short";
                case "uniqueidentifier": return "String";
                case "varchar": return "String";
                case "xml": return "String";
                default: return "String";
            }
        }

        #region ADO.NET

        protected DataSet ExecuteDataSet(string sql)
        {
            DataSet dataset = new DataSet(Connection.Database);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            foreach (DataRow item in table.Rows)
            {
                adapter = new SqlDataAdapter(string.Format("select top 1 * from [{0}]", item[0]), Connection);
                adapter.Fill(dataset, item[0].ToString());
            }
            return dataset;
        }

        protected DataTable ExecuteDataTable(string sql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            adapter.Fill(table);
            return table;
        }

        #endregion
    }
}