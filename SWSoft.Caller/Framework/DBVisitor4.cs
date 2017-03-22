using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using System.Diagnostics;
using log4net;
using SWSoft.Framework;

namespace SWSoft.Caller.Framework
{
    public class DBVisitor4
    {

        static ILog log = log4net.LogManager.GetLogger("SqlInfo");
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString4"];
        /// <summary>
        /// 执行IDbCommand所需要的参数集合
        /// </summary>
        public static ParameterCollection Params { get; set; }
        /// <summary>
        /// 数据库连接工厂
        /// </summary>
        public static DbProviderFactory DataFactory { get; set; }
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public static DbConnection Connection
        {
            get
            {
                if (ConnectionString == null)
                {
                    throw new Exception("未配置数据库连接AppSettings.ConnectionString3");
                }
                var Conn = DataFactory.CreateConnection();
                Conn.ConnectionString = ConnectionString.ConnectionString;
                Conn.Open();
                return Conn;
            }
        }

        public static DbConnection GetConnection(string connect)
        {
            var csts = ConfigurationManager.ConnectionStrings[connect];
            var conn = DbProviderFactories.GetFactory(csts.ProviderName).CreateConnection();
            conn.ConnectionString = csts.ConnectionString;
            return conn;
        }

        /// <summary>
        /// Orcale连接对象获取
        /// </summary>
        /// <param name="connectstring"></param>
        /// <param name="DbType"></param>
        /// <returns></returns>
        public static OracleConnection GetConnection(string connectstring, string DbType)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectstring];
            if (cs == null) throw new Exception(string.Format("配置文件web.config中节点{0}配置未取到", connectstring));
            OracleConnection oc = new OracleConnection(cs.ToString());
            return oc;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        static DBVisitor4()
        {
            Params = new ParameterCollection();
            DataFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
        }

        #region 基础执行对象

        public static int ExecuteNonQuery(string cmdText, DbTransaction tran = null, string connect = "")
        {
            try
            {
                log.InfoFormat("{0}", cmdText + "\r\n");
                var command = Command(cmdText, tran, connect);
                var outobj = command.ExecuteNonQuery();
                if (tran == null)
                {
                    command.Connection.Close();
                }
                return outobj;
            }
            catch (Exception ex)
            {
                throw new Exception(cmdText, ex);
            }
        }

        public static bool ExecuteBool(string cmdText, DbTransaction tran = null, string connect = "")
        {
            return ExecuteNonQuery(cmdText, tran, connect) > 0;
        }

        public static object ExecuteScalar(string cmdText, DbTransaction tran, string connect = "")
        {
            var command = Command(cmdText, tran, connect);
            var outobj = command.ExecuteScalar();
            if (tran == null)
            {
                command.Connection.Close();
            }
            return outobj;
        }

        public static IDataReader ExecuteReader(string cmdText, string connect = "")
        {
            return Command(cmdText, null, connect).ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static IDataReader ExecuteReader(string cmdText, DbTransaction tran, string connect = "")
        {
            return Command(cmdText, tran, connect).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        /// <summary>
        /// 数据库执行对象
        /// </summary>
        /// <param name="cmdText">数据源运行的文本命令</param>
        ///<param name="transaction">是否开启事务</param>
        ///<param name="connect">要连接的数据库配置名</param>
        static DbCommand Command(string cmdText, DbTransaction transaction = null, string connect = "")
        {
            DbCommand DbCommand;
            if (transaction == null)
            {
                DbCommand = connect == string.Empty ? Connection.CreateCommand() : GetConnection(connect).CreateCommand();
            }
            else
            {
                DbCommand = transaction.Connection.CreateCommand();
            }
            Debug.WriteLine(cmdText.Trim());
            Debug.WriteLine("");
            DbCommand.CommandText = cmdText;
            foreach (var item in Params)
            {
                DbParameter param = DbCommand.CreateParameter();
                param.ParameterName = item.Name;
                param.Value = item.Value;
                param.Direction = item.Direction;
                DbCommand.Parameters.Add(param);
            }
            if (transaction != null)
            {
                DbCommand.Transaction = transaction;
            }
            Params.Items.Clear();
            if (DbCommand.Connection.State != ConnectionState.Open)
            {
                DbCommand.Connection.Open();
            }
            return DbCommand;
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">执行 SQL 语句</param>
        /// <returns>第一行第一列</returns>
        public static object ExecuteScalar(string cmdText, string connect = "")
        {
            var cmd = Command(cmdText, null, connect);
            var obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return obj;
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">执行 SQL 语句</param>
        /// <returns>内存中数据的一个表</returns>
        public static DataTable ExecuteTable(string cmdText, string connect = "")
        {
            return ExecuteDataSet(cmdText, connect).Tables[0];
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">执行 SQL 语句</param>
        /// <returns>内存中数据的一个表</returns>
        public static DataSet ExecuteDataSet(string cmdText, string connect = "")
        {
            var adapter = DataFactory.CreateDataAdapter();
            adapter.SelectCommand = Command(cmdText, null, connect);
            var dataset = new DataSet();
            adapter.Fill(dataset);
            adapter.SelectCommand.Connection.Close();
            return dataset;
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <returns>用户对象集合</returns>
        public static Entry ExecuteModel(string cmdText)
        {
            var list = ExecuteModels<Entry>(cmdText);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 执行 SQL 语句或存储过程名称
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <returns>用户对象集合</returns>
        public static List<Entry> ExecuteModels(string cmdText, string connect = "")
        {
            return ExecuteModels<Entry>(cmdText, connect);
        }
        public static List<Entry> ExecuteModels_tran(string cmdText, DbTransaction tran = null, string connect = "")
        {
            return ExecuteModels_tran<Entry>(cmdText,tran,connect);
        }
        /// <summary>
        /// 执行 SQL 语句或存储过程名称
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <returns>对象集合JSON方式</returns>
        public static string ExecuteJson(string cmdText, DbTransaction tran = null, string connect = "")
        {
            return Json.ToString(list: ExecuteModels(cmdText));
        }

        #region Reflection


        public static List<T> ExecuteModels<T>(string cmdText, string connect = "")
        {
            ///////////////////////
            log.InfoFormat("{0}", cmdText + "\r\n");
            var reader = ExecuteTable(cmdText, connect);
            var list = new List<T>();
            var propertys = new Dictionary<string, PropertyInfo>();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!propertys.ContainsKey(item.Name.ToUpper()))
                {
                    propertys.Add(item.Name.ToUpper(), item);
                }
            }
            foreach (DataRow item in reader.Rows)
            {
                T entry = Activator.CreateInstance<T>();

                foreach (DataColumn column in reader.Columns)
                {
                    PropertyInfo property;
                    if (propertys.TryGetValue(column.ColumnName, out property))
                    {
                        property.SetValue(entry, IsNull(property, item[property.Name]), null);
                    }
                    else
                    {
                        //对象包含此属性时添加到基类集合中
                        propertys["ITEM"].SetValue(entry, item[column.ColumnName], new object[] { column.ColumnName });
                    }
                }
                list.Add(entry);
            }
            return list;
        }

        public static List<T> ExecuteModels_tran<T>(string cmdText, DbTransaction transaction = null, string connect = "")
        {
            DbCommand DbCommand;
            if (transaction == null)
            {
                DbCommand = connect == string.Empty ? Connection.CreateCommand() : GetConnection(connect).CreateCommand();
            }
            else
            {
                DbCommand = transaction.Connection.CreateCommand();
            }
            ///////////////////////
            log.InfoFormat("{0}", cmdText + "\r\n");
            var reader = ExecuteTable(cmdText, connect);
            var list = new List<T>();
            var propertys = new Dictionary<string, PropertyInfo>();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!propertys.ContainsKey(item.Name.ToUpper()))
                {
                    propertys.Add(item.Name.ToUpper(), item);
                }
            }
            foreach (DataRow item in reader.Rows)
            {
                T entry = Activator.CreateInstance<T>();

                foreach (DataColumn column in reader.Columns)
                {
                    PropertyInfo property;
                    if (propertys.TryGetValue(column.ColumnName, out property))
                    {
                        property.SetValue(entry, IsNull(property, item[property.Name]), null);
                    }
                    else
                    {
                        //对象包含此属性时添加到基类集合中
                        propertys["ITEM"].SetValue(entry, item[column.ColumnName], new object[] { column.ColumnName });
                    }
                }
                list.Add(entry);
            }
            return list;
        }
        /// <summary>
        /// 空值验证
        /// </summary>
        /// <param name="property">属性对象</param>
        /// <param name="value">属性的值</param>
        public static Object IsNull(PropertyInfo property, object value)
        {
            value = Convert.ChangeType(value, property.PropertyType);
            switch (property.PropertyType.Name)
            {
                case "Boolean":
                    return value == DBNull.Value ? false : value;
                case "DateTime":
                    return value == DBNull.Value ? DateTime.MinValue : value;
                case "Byte[]":
                    return value == DBNull.Value ? new Byte[] { } : value;
                case "String":
                    return value == DBNull.Value ? String.Empty : value;
                default:
                    return value;
            }
        }

        /// <summary>
        /// 获取对象属性的值
        /// </summary>
        /// <param name="obj">要获取值的对象</param>
        /// <param name="name">属性的名称</param>
        /// <returns>属性的值</returns>
        public static object GetValue(object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj, null);
        }

        #endregion

        public static T1 ExecuteModel<T1>(string cmdText, string connect = "")
        {
            var list = ExecuteModels<T1>(cmdText, connect);
            return list.Count > 0 ? list[0] : default(T1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">SqlLoadKey</param>
        /// <param name="dict"></param>
        /// <param name="tran"></param>
        /// <param name="connect"></param>
        public static void ExecuteProcedure(ProcedureItem pitem, DbTransaction tran = null, string connect = "")
        {
            var commd = Command(pitem.ProcedureName, tran, connect);
            commd.CommandType = CommandType.StoredProcedure;
            string paramstr = "(";
            foreach (var item in pitem.ParametersKeys)
            {
                var p = pitem.Parameters[item];
                var param = commd.CreateParameter();
                param.ParameterName = p.ParameterName;
                param.Direction = p.Direction;
                param.Value = p.Value;
                param.DbType = p.DataType;
                commd.Parameters.Add(param);
                paramstr += p.Value + ",";
            }
            //Debug.Write(paramstr.TrimEnd(',') + ")");
            commd.ExecuteNonQuery();
            if (tran == null)
            {
                commd.Connection.Close();
            }
            foreach (DbParameter item in commd.Parameters)
            {
                if (item.Direction != ParameterDirection.Input)
                {
                    pitem[item.ParameterName] = item.Value;
                }
            }
        }
    }
}
