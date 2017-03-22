using System.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace SWSoft.Reflector
{
    /// <summary>
    /// 数据库的相关信息
    /// </summary>
    public partial class DBHelp
    {
        /// <summary>
        /// 数据库列表
        /// </summary>
        public List<string> Datas { get { return BaseList(); } }
        /// <summary>
        /// 数据库在内存中的缓存
        /// </summary>
        public DataSet DataSet { get { return GetSet(); } }
        public List<string> TableList { get { return GetTables(); } }

        public List<string> GetTables()
        {
            string sql = Sql2000 ? "SELECT name FROM sysobjects WHERE xtype='u'" : "SELECT name FROM sys.objects WHERE type='u'";
            List<string> list = new List<string>();
            foreach (DataRow item in ExecuteDataTable(sql).Rows)
            {
                list.Add(item["name"].ToString());
            }
            return list;
        }
        public bool Sql2000 { get { return Connection.ServerVersion.StartsWith("08"); } }

        /// <summary>
        /// 数据库列表
        /// </summary>
        /// <returns></returns>
        private List<string> BaseList()
        {
            string sql = Sql2000 ? "select [name] from sysdatabases where dbid>4" : "select [name] from sys.databases where database_id>4";
            List<string> list = new List<string>();
            foreach (DataRow item in ExecuteDataTable(sql).Rows)
            {
                list.Add(item[0].ToString());
            }
            return list;
        }

        /// <summary>
        /// 数据库对象
        /// </summary>
        private DataSet GetSet()
        {
            string sql = Sql2000 ? "select [name] from sysobjects where type='u'" : "select [Name] from sys.objects where type='u'";
            Debug.WriteLine(sql);
            DataSet dataset = ExecuteDataSet(sql);

            //为表添加扩展属性说明字段
            foreach (DataRow item in TableExtend().Rows)
            {
                if (dataset.Tables.Contains(item["tablename"].ToString()))
                {
                    dataset.Tables[item["tablename"].ToString()].ExtendedProperties.Add("Description", item["Description"]);
                }
            }
            //为列添加扩展属性说明字段
            foreach (DataRow item in ColumnExtend().Rows)
            {
                DataTable table = dataset.Tables[item["tablename"].ToString()];
                if (table.Columns.Contains(item["tablename"].ToString()))
                {
                    table.Columns[item["columnname"].ToString()].ExtendedProperties.Add("Description", item["value"]);
                }
            }
            DataTable table1 = Connection.GetSchema("columns");
            foreach (DataRow item in table1.Rows)
            {
                foreach (var item1 in item.ItemArray)
                {
                    Debug.Write("[" + item1.ToString() + "]\t");
                }
                Debug.WriteLine("");
            }
            return dataset;
        }

        /// <summary>
        /// 表的扩展属性说明字段
        /// </summary>
        private DataTable TableExtend()
        {
            string sql = Sql2000 ? "select [tablename]=(select [name] from sysobjects where type='u' and xtype='u' and sysobjects.id=prop.id),Description=[value] from sysproperties prop where smallid=0" : "select [tablename]=(select [name] from sys.tables where [object_id]=extend.major_id),Description=[value] from sys.extended_properties extend where minor_id=0";
            return ExecuteDataTable(sql);
        }

        /// <summary>
        /// 列的扩展属性说名字字段
        /// </summary>
        private DataTable ColumnExtend()
        {
            string sql = Sql2000 ? "select tablename=(select name from sysobjects where id=col.id),name,value=(select value from sysproperties where smallid=colid and id=col.id) from syscolumns col where id in (select id from sysobjects where type='u' and xtype='u')" : "select [tablename]=(select [name] from sys.tables where [object_id]=col.[object_id]),[columnname]=[name],Description=(select [value] from sys.extended_properties where minor_id=col.column_id and major_id=col.[object_id]) from sys.columns col where col.object_id in (select major_id from sys.extended_properties extend where minor_id!=0)";
            return ExecuteDataTable(sql);
        }
    }
}