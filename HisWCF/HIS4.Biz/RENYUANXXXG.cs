using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;
using HIS4.Schemas;

namespace HIS4.Biz
{
    public class RENYUANXXXG : IMessage<RENYUANXXXG_IN,RENYUANXXXG_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new RENYUANXXXG_OUT();

            string bingRenID = InObject.BINGRENID;
            string lianXiDH = InObject.LIANXIDH;

            if (string.IsNullOrEmpty(bingRenID)) {
                throw new Exception("病人ID不能为空！");
            }

            if (string.IsNullOrEmpty(lianXiDH) || (lianXiDH.Length != 11 && lianXiDH.Length != 8)) {
                throw new Exception("请输入正确的电话号码！");
            }

            DataTable dt = DBVisitor.ExecuteTable(string.Format("select * from gy_v_bingrenxx where bingrenid ='{0}' ", bingRenID));
            if (dt.Rows.Count <= 0) {
                throw new Exception("未找到相应的病人信息！");
            }

            if (!string.IsNullOrEmpty(lianXiDH)) {
                
                string sqlUpdateLXDH = "update gy_bingrenxx set jiatingdh = '{0}' where bingrenid = '{1}'";
                DBVisitor.ExecuteNonQuery(string.Format(sqlUpdateLXDH, lianXiDH, bingRenID));
            }

            OutObject.OUTMSG.ERRMSG = "数据更新成功";
        }
    }
}
