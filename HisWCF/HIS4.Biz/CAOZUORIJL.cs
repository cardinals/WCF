using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using HIS4.Schemas; 
namespace HIS4.Biz
{
    /// <summary>
    /// 记录操作日志,方便在平台显示查询 add by Hu.Q 2017年3月9日 20:08:12
    /// </summary>
    public class CAOZUORIJL : IMessage<CAOZUORIJL_IN, CAOZUORIJL_OUT>
    {
        public override void ProcessMessage()
        {
            if (string.IsNullOrEmpty(InObject.LEIXING))
            {
                throw new Exception("日志类型不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.CONTEXT))
            {
                throw new Exception("日志内容不能为空!");
            }
            string RiZId = "";
            var maxid = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.HIS00026, " seq_GY_RIZHI_ZZJ.nextval "));
            if (maxid == null)
            {
                throw new Exception(string.Format("获取日志ID失败！"));
            }
            else
            {
                RiZId = maxid.Items["MAXID"].ToString();
            }
            string StrTct = InObject.CONTEXT.Replace("'", "").Trim();
            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00027, RiZId, InObject.JIUZHENKH, StrTct, InObject.BASEINFO.CAOZUOYDM,
                         InObject.IP, InObject.LEIXING,InObject.ERRBZ));
            OutObject = new CAOZUORIJL_OUT();


        }
    }
}
