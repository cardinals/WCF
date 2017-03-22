using System.Data;
using System;

namespace SWSoft.Reflector
{
    public partial class Code
    {
        private static CodeFile Create(CodeFile codefile, DataTable table)
        {
            return AddModel(codefile, table);
        }


        /// <summary>
        /// 生成实体对象类代码文件
        /// </summary>
        /// <param name="codefile">代码文件</param>
        /// <param name="table">内存中的一个表</param>
        public static CodeFile AddModel(CodeFile codefile, DataTable table)
        {
            codefile.NewLine(0, "using System;");
            codefile.Null();
            codefile.NewLine(0, "//此代码由工具AutoCode自动生成");
            codefile.NewLine(0, "//开发者：http://www.fenglaijun.com/");
            codefile.NewLine(0, "namespace {0}", codefile.NameSpace + ".Model");
            codefile.NewLine(0, "{");
            if (codefile.Remark)
            {
                codefile.NewLine(1, "/// <summary>");
                codefile.NewLine(1, "/// {0}", table.ExtendedProperties["Description"]);
                codefile.NewLine(1, "/// </summary>");
            }
            codefile.NewLine(1, "public class {0}", table.TableName);
            codefile.NewLine(1, "{");
            foreach (DataColumn column in table.Columns)
            {
                if (codefile.Remark)
                {
                    codefile.NewLine(2, "/// <summary>");
                    codefile.NewLine(2, "/// {0}", column.ExtendedProperties["Description"]);
                    codefile.NewLine(2, "/// </summary>");
                }
                codefile.NewLine(2, "public {0} {1} {{ get; set; }}", column.DataType.Name, column.ColumnName);
            }
            codefile.NewLine(1, "}");
            codefile.NewLine(0, "}");
            return codefile;
        }

        /// <summary>
        /// 生成业务访问类代码文件
        /// </summary>
        /// <param name="codefile">代码文件</param>
        /// <param name="table">数据库表对象</param>
        public static void LogicClass(CodeFile codefile, DataTable table)
        {
            codefile.NewLine(0, "using System.Collections.Generic;");
            switch (codefile.DBModel)
            {
                case "mysql": codefile.NewLine(0, "using MySql.Data.MySqlClient;"); break;
                case "access": codefile.NewLine(0, "using System.Data.OleDb;"); break;
                default: codefile.NewLine(0, "using System.Data.SqlClient;"); break;
            }
            codefile.NewLine(0, "using SWSoft.MVC.Model;");
            //codefile.NewLine(0, "using {0}.Model;", codefile.NameSpace);
            codefile.Null();
            codefile.NewLine(0, "namespace {0}.Dal", codefile.NameSpace);
            codefile.NewLine(0, "{");
            //codefile.NewLine(1, "public class Dal{0} : {1}Visitor<{2}>", table.TableName, codefile.DBModel, table.TableName);
            switch (codefile.DBModel)
            {
                case "mysql": codefile.NewLine(1, "DalOption : DBVisitor<{0}, MySqlConnection, MySqlDataAdapter>", table.TableName); break;
                case "access": codefile.NewLine(1, "DalOption : DBVisitor<{0}, OleDbConnection, OleDbDataAdapter>", table.TableName); break;
                default: codefile.NewLine(1, "DalOption : DBVisitor<{0}, SqlConnection, SqlDataAdapter>", table.TableName); break;
            }
            codefile.NewLine(1, "{");
            codefile.NewLine(1, "}");
            codefile.NewLine(0, "}");
        }
    }
}
