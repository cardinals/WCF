using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class ZD_YIYUANXX : IMessage<ZD_YIYUANXX_IN, ZD_YIYUANXX_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new ZD_YIYUANXX_OUT();

            string YiYuanXXSql = "select YILIAOJGMC,YILIAOJGJS,YILIAOJGDZ from web_yiliaojgxx ";

            DataTable dtYiYuanXX = DBVisitor.ExecuteTable(YiYuanXXSql);

            if (dtYiYuanXX.Rows.Count > 0)
            {
                OutObject.YIYUANMC = dtYiYuanXX.Rows[0]["YILIAOJGMC"].ToString();
                OutObject.YIYUANJS = dtYiYuanXX.Rows[0]["YILIAOJGJS"].ToString();
                OutObject.YIYUANDZ = dtYiYuanXX.Rows[0]["YILIAOJGDZ"].ToString();
            }
            else {
                throw new Exception("系统建设中……");
            }

        }

    }
}
