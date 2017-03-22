using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;

namespace FSDYY.Biz
{
    public class SHEBEIYYQR : IMessage<SHEBEIYYQR_IN, SHEBEIYYQR_OUT>
    {
        public override void ProcessMessage()
        {
            //取得预约信息
            OutObject = new SHEBEIYYQR_OUT();
            if (InObject.YUYUESQDBH == null || InObject.YUYUESQDBH == "")
            {
                throw new Exception(string.Format("预约申请单编号为空！"));
            }
            var listyyxx = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.FSD00004, InObject.YUYUESQDBH.ToString()));
            if (listyyxx == null)
            {
                OutObject.OUTMSG.ERRNO = "-2";
                OutObject.OUTMSG.ERRMSG = string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString());
                return;
                //throw new Exception(string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString()));
            }
            if (listyyxx.Items.Count == 0)
            {
                OutObject.OUTMSG.ERRNO = "-2";
                OutObject.OUTMSG.ERRMSG = string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString());
                return;
                //throw new Exception(string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString()));
            }
            else
            {
                var tran = DBVisitor.Connection.BeginTransaction();
                try
                {
                    if (InObject.YUYUEQRLX == "1")
                    {
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00005, InObject.YUYUESQDBH.ToString(), 1), tran);
                    }
                    else if (InObject.YUYUEQRLX == "2")
                    {
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00015, InObject.YUYUESQDBH.ToString(), 1), tran);
                    }
                    else if (InObject.YUYUEQRLX == "3")
                    {
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.FSD00016, InObject.YUYUESQDBH.ToString(), 1), tran);
                    }
                    else
                    {
                        OutObject.OUTMSG.ERRNO = "-2";
                        OutObject.OUTMSG.ERRMSG = string.Format("找不到预约信息:申请单编号[{0}]", InObject.YUYUESQDBH.ToString());
                        return;
                    }
                    
                    tran.Commit();
                    OutObject = new SHEBEIYYQR_OUT();
                    OutObject.YUYUEH = listyyxx.Items["YYH"].ToString();
                    OutObject.YUYUERQ = listyyxx.Items["JCRQ"].ToString();
                    OutObject.YUYUESJ = listyyxx.Items["JCSJ"].ToString();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
    }
}
