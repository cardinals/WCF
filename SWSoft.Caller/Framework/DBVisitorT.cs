using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using System.Diagnostics;

namespace SWSoft.Framework
{

    public class DBVisitor<TModel> where TModel : Entry, new()
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected static ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"];
        /// <summary>
        /// 执行IDbCommand所需要的参数集合
        /// </summary>
        protected ParameterCollection Params { get; set; }
        /// <summary>
        /// 数据库连接工厂
        /// </summary>
        protected DbProviderFactory DataFactory { get; set; }
        /// <summary>
        /// 当前对象属性集合
        /// </summary>
        public Dictionary<string, PropertyInfo> Propertys { get; set; }
        private DbConnection connection;
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                var Conn = DataFactory.CreateConnection();
                Conn.ConnectionString = ConnectionString.ConnectionString;
                return Conn;
                /*if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;*/
            }
        }
        /// <summary>
        /// 数据库命令执行对象
        /// </summary>
        protected DbCommand DbCommand { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DBVisitor()
        {
            Params = new ParameterCollection();
            Propertys = new Dictionary<string, PropertyInfo>();
            foreach (var property in typeof(TModel).GetProperties())
            {
                if (!Propertys.ContainsKey(property.Name))
                {
                    Propertys.Add(property.Name, property);
                }
            }
            DataFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
            connection = DataFactory.CreateConnection();
            connection.ConnectionString = ConnectionString.ConnectionString;
        }

        /// <summary>
        /// 数据库执行对象
        /// </summary>
        /// <param name="cmdText">数据源运行的文本命令</param>
        ///<param name="transaction">是否开启事务</param>
        DbCommand Command(string cmdText, DbTransaction transaction = null)
        {
            DbCommand = Connection.CreateCommand();
            DbCommand.CommandText = cmdText;
            if (cmdText.StartsWith("["))
            {   //[开头的表示存储过程
                DbCommand.CommandType = CommandType.StoredProcedure;
            }
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
        /// <returns>结果集流</returns>
        protected IDataReader ExecuteReader(string cmdText)
        {
            return Command(cmdText).ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">执行 SQL 语句</param>
        /// <returns>第一行第一列</returns>
        protected object ExecuteScalar(string cmdText)
        {
            return Command(cmdText).ExecuteScalar();
        }

        protected bool ExecuteNonQuery(string cmdText)
        {
            return Command(cmdText).ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">执行 SQL 语句</param>
        /// <returns>内存中数据的一个表</returns>
        protected DataTable ExecuteTable(string cmdText)
        {
            return ExecuteDataSet(cmdText).Tables[0];
        }

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">执行 SQL 语句</param>
        /// <returns>内存中数据的一个表</returns>
        protected DataSet ExecuteDataSet(string cmdText)
        {
            var adapter = DataFactory.CreateDataAdapter();
            adapter.SelectCommand = Command(cmdText);
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
        protected TModel ExecuteModel(string cmdText)
        {
            return GetModel(ExecuteReader(cmdText));
        }

        /// <summary>
        /// 执行 SQL 语句或存储过程名称
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <returns>用户对象集合</returns>
        protected List<TModel> ExecuteModels(string cmdText)
        {
            return GetModels(ExecuteReader(cmdText));
        }

        /// <summary>
        /// 执行 SQL 语句或存储过程名称
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <returns>对象集合JSON方式</returns>
        protected string ExecuteJson(string cmdText)
        {
            return Json.ToString(list: GetModels(ExecuteReader(cmdText)));
        }

        #region Reflection

        /// <summary>
        /// 封装为实体对象
        /// </summary>
        /// <param name="reader">数据库读取器</param>
        /// <returns>实体对象的泛型集合</returns>
        protected TModel GetModel(IDataReader reader)
        {
            var entrys = GetModels(reader);
            return entrys.Count > 0 ? entrys[0] : default(TModel);
        }

        protected List<TModel> GetModels(DataTable table)
        {
            var list = new List<TModel>();
            var properties = typeof(TModel).GetProperties();
            foreach (DataRow item in table.Rows)
            {
                var model = new TModel();
                foreach (var prop in properties)
                {
                    prop.SetValue(model, item[prop.Name], null);
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 封装为实体对象
        /// </summary>
        /// <param name="reader">数据库读取器</param>
        /// <returns>实体对象的泛型集合</returns>
        protected List<TModel> GetModels(IDataReader reader)
        {
            var list = new List<TModel>();
            while (reader.Read())
            {
                TModel entry = new TModel();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    PropertyInfo property;
                    if (Propertys.TryGetValue(name, out property))
                    {
                        SetValue(entry, property.Name, IsNull(property, reader[property.Name]));
                    }
                    else
                    {
                        //对象包含此属性时添加到基类集合中
                        SetValue(entry, "Item", reader[name], new object[] { name });
                    }
                }
                list.Add(entry);
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 空值验证
        /// </summary>
        /// <param name="property">属性对象</param>
        /// <param name="value">属性的值</param>
        protected Object IsNull(PropertyInfo property, object value)
        {
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
        /// 设置对象属性的值
        /// </summary>
        /// <param name="obj">将设置属性值的对象</param>
        /// <param name="name">置属性的名称</param>
        /// <param name="value">属性的新值</param>
        /// <param name="index">索引</param>
        protected void SetValue(object obj, string name, object value, object[] index = null)
        {
            value = value == DBNull.Value ? null : value;
            Propertys[name].SetValue(obj, value, index);
        }

        /// <summary>
        /// 获取对象属性的值
        /// </summary>
        /// <param name="obj">要获取值的对象</param>
        /// <param name="name">属性的名称</param>
        /// <returns>属性的值</returns>
        protected object GetValue(object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj, null);
        }

        #endregion
    }
}