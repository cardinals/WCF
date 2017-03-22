using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SWSoft.Reflector
{
    /// <summary>
    /// 代码源文件对象
    /// </summary>
    public class CodeFile
    {
        /// <summary>
        /// 文件存储路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 源代码字符串
        /// </summary>
        public List<string> Content { get; set; }
        /// <summary>
        /// 数据库模型
        /// </summary>
        public string DBModel { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// 是否有字段说明
        /// </summary>
        public bool Remark { get; set; }

        public CodeFile()
        {
            Content = new List<string>();
        }

        /// <summary>
        /// 添加代码行
        /// </summary>
        /// <param name="num">缩进</param>
        /// <param name="context">代码内容</param>
        /// <param name="args">参数</param>
        public void NewLine(int num, string context, params object[] args)
        {
            string line = string.Empty;
            for (int i = 0; i < num; i++)
            {
                line += "\t";
            }
            if (args.Length == 0)
            {
                line += context;
            }
            else
            {
                line += string.Format(context, args);
            }
            Content.Add(line);
        }

        /// <summary>
        /// 新增代码行
        /// </summary>
        /// <param name="context">代码内容</param>
        /// <param name="args">参数</param>
        public void NewLine(string context, params object[] args)
        {
            NewLine(0, context, args);
        }

        /// <summary>
        /// 添加一个空行
        /// </summary>
        public void Null()
        {
            Content.Add("");
        }

        /// <summary>
        /// 保存代码
        /// </summary>
        public void Save()
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            using (StreamWriter sw = new StreamWriter(Path + "\\" + FileName, false, Encoding.UTF8))
            {
                foreach (String line in Content)
                {
                    sw.WriteLine(line);
                }
            }
        }

        public void CSharpModel()
        {

        }
    }
}