using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using Common.WSEntity;
using System.Configuration;
using Common.WSCall;

namespace HIS4.Biz
{
    public class SHEBEIYYQX : IMessage<SHEBEIYYQX_IN, SHEBEIYYQX_OUT>
    {
        public override void ProcessMessage()
        {
            string yuyuesqdBh =InObject.YUYUESQDBH;
            string yewuLx = InObject.YEWULX;
            string caozuoyDm = InObject.BASEINFO.CAOZUOYDM;
            if (yuyuesqdBh == "" || yuyuesqdBh == "-1")
            {
                throw new Exception( "预约申请单编号为空");
            }
            DataTable listyyxx =  DBVisitor.ExecuteTable(string.Format("select * from sxzz_jianchasqd a where a.jianchasqdid = '{0}'", yuyuesqdBh));
            if (listyyxx.Rows.Count <= 0)
            {
                throw new Exception( "找不到预约信息:申请单编号[" + yuyuesqdBh + "]");
            }
            if (yewuLx == "1")
            {
                if (listyyxx.Rows[0]["JIANCHASQDZT"].ToString() == "9")
                {
                    throw new Exception( "预约申请单已取消！");
                }
                DataTable listyy = DBVisitor.ExecuteTable(string.Format("select * from sxzz_jianchasqd where jianchasqdid = '{0}'", yuyuesqdBh));
                if (listyy.Rows.Count <= 0)
                {
                    throw new Exception( "未找到预约项目信息！");
                }
                if (listyy.Rows[0]["SHOUFEIBZ"].ToString() == "1" && listyy.Rows[0]["JIESHOUBZ"].ToString() == "1")
                {
                    throw new Exception( "已登记不能取消！");
                }
                var resource = new HISYY_Cancel();
                resource.RequestNo = "";// listyyxx.Items["YYH"].ToString();
                resource.YYH = listyyxx.Rows[0]["YIJIYYH"].ToString();
                resource.JCH = listyyxx.Rows[0]["YIJISQDH"].ToString();

                //调用莱达WebService---------------------------------------------------------------
                string url = System.Configuration.ConfigurationManager.AppSettings["LaiDa_Url"];
                string xml = XMLHandle.EntitytoXML<HISYY_Cancel>(resource);
                string outxml = WSServer.Call<HISYY_Cancel>(url, xml).ToString();
                HISYY_Cancel_Result result = XMLHandle.XMLtoEntity<HISYY_Cancel_Result>(outxml);
                //---------------------------------------------------------------------------------
                if (result.Success == "False")
                {
                    throw new Exception( "取消预约失败,错误原因：" + result.Message);
                }
                var trans = DBVisitor.Connection.BeginTransaction();
                
                try
                {
                    DBVisitor.ExecuteNonQuery( string.Format("update sxzz_jianchasqd a set a.jianchasqdzt = {1},quxiaogh = '{2}',quxiaorq = sysdate where a.jianchasqdid = {0}",
                        yuyuesqdBh, 9, caozuoyDm),trans);
                    
                   DBVisitor.ExecuteNonQuery( string.Format("update sxzz_jianchasqd set JIESHOUBZ = '{1}',SHOUFEIBZ = '{2}' where jianchasqdid = '{0}'",
                        yuyuesqdBh, 0, 1), trans);

                   trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                   
                    throw new Exception( ex.Message);
                }
            }
            else//检验的暂时不考虑
            {
                throw new Exception("检验业务建设中，敬请期待……");
            }
        }
        
    }

    
}
