using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using System.Configuration;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class MENZHENJSJL : IMessage<MENZHENJSJL_IN, MENZHENJSJL_OUT>
    {

        public override void ProcessMessage()
        {
            this.OutObject = new MENZHENJSJL_OUT();

            string bingRenID = InObject.BINGRENID;//病人ID
            string kaiShiRQ = InObject.KAISHIRQ;//开始日期
            string jieShuRQ = InObject.JIESHURQ;//结束日期
            string yuanQuID = InObject.BASEINFO.FENYUANDM;//分院代码
            int menZhenSJFW = -1 * Convert.ToInt32(ConfigurationManager.AppSettings["FeiYongSJ"]);//门诊费用检索默认时间范围
            string menZhenFeiYongJLXS = ConfigurationManager.AppSettings["MZFYJLTFXX"];//门诊费用记录是否显示退费作废记录
            #region 基础入参判断
            if (string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("病人信息获取失败!");
            }

            if (string.IsNullOrEmpty(jieShuRQ)) {
                jieShuRQ = DateTime.Now.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrEmpty(kaiShiRQ)) {
                kaiShiRQ = DateTime.Now.AddDays(menZhenSJFW).Date.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrEmpty(menZhenFeiYongJLXS))
            {
                menZhenFeiYongJLXS = "0";
            }
            else {
                menZhenFeiYongJLXS = "1";
            }
            #endregion

            StringBuilder shouFeiJLSQL = new StringBuilder();
            shouFeiJLSQL.Append(" select a.shoufeiid,a.feiyonglb,(select leibiemc from gy_feiyonglb where leibieid = a.feiyonglb ) as feiyonglbmc,a.feiyonghj,a.shifuje,a.jiaoyilx,a.jizhenbz,a.shoufeilb,a.shoufeilx,to_char(a.shoufeirq,'yyyy-mm-dd hh24:mi') as shoufeirq,a.jiluly,decode(a.shoufeilb,1,'挂号',2,'收费','收费') as SHOUFEILBMC,decode(a.shoufeilx,10,'门诊挂号',11,'急诊挂号',20,'门诊收费',21,'急诊收费',30,'规定病种',31,'单病种',41,'其他收费') as SHOUFEILXMC,decode(a.jiluly,0,'正常',1,'退费',2,'作废')  as JILULYMC from mz_shoufei1 a where a.shoufeirq between to_date( '{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date( '{2} 23:59:59','yyyy-mm-dd hh24:mi:ss') and a.bingrenid = '{0}' ");

            if (menZhenFeiYongJLXS == "1")
            {
                shouFeiJLSQL.Append(" and chongxiaobz = 0 and jiaoyilx = 1 ");
            }
            shouFeiJLSQL.Append("  order by a.shoufeirq desc ");
            //string shouFeiJLSQL = "select a.shoufeiid,a.feiyonglb,(select leibiemc from gy_feiyonglb where leibieid = a.feiyonglb ) as feiyonglbmc,a.feiyonghj,a.shifuje,a.jiaoyilx,a.jizhenbz,a.shoufeilb,a.shoufeilx,to_char(a.shoufeirq,'yyyy-mm-dd hh24:mi') as shoufeirq,a.jiluly,decode(a.shoufeilb,1,'挂号',2,'收费','收费') as SHOUFEILBMC,decode(a.shoufeilx,10,'门诊挂号',11,'急诊挂号',20,'门诊收费',21,'急诊收费',30,'规定病种',31,'单病种',41,'其他收费') as SHOUFEILXMC,decode(a.jiluly,0,'正常',1,'退费',2,'作废')  as JILULYMC from mz_shoufei1 a where a.shoufeirq between to_date( '{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date( '{2} 23:59:59','yyyy-mm-dd hh24:mi:ss') and a.bingrenid = '{0}' order by a.shoufeirq desc";
            DataTable dtShouFeiJL = DBVisitor.ExecuteTable(string.Format(shouFeiJLSQL.ToString(),bingRenID,kaiShiRQ,jieShuRQ));

            if (dtShouFeiJL.Rows.Count > 0) {
                OutObject.JIESUANMXTS = dtShouFeiJL.Rows.Count.ToString();
                for (int i=0; i < dtShouFeiJL.Rows.Count; i++)
                {
                    MENZHENJSXX mzjsxx = new MENZHENJSXX();
                    mzjsxx.SHOUFEIID = dtShouFeiJL.Rows[i]["shoufeiid"].ToString();//收费ID
                    mzjsxx.FEIYONGLB = dtShouFeiJL.Rows[i]["feiyonglb"].ToString();//费用类别
                    mzjsxx.FEIYONGLBMC = dtShouFeiJL.Rows[i]["feiyonglbmc"].ToString();//费用类别名称
                    mzjsxx.FEIYONGHJ = dtShouFeiJL.Rows[i]["feiyonghj"].ToString();//费用合计
                    mzjsxx.SHIFUJE = dtShouFeiJL.Rows[i]["shifuje"].ToString();//实付金额
                    mzjsxx.JIAOYILX = dtShouFeiJL.Rows[i]["jiaoyilx"].ToString();//交易类型
                    mzjsxx.JIZHENBZ = dtShouFeiJL.Rows[i]["jizhenbz"].ToString();//急诊标志
                    mzjsxx.SHOUFEILB = dtShouFeiJL.Rows[i]["shoufeilb"].ToString();//收费类别
                    mzjsxx.SHOUFEILBMC = dtShouFeiJL.Rows[i]["SHOUFEILBMC"].ToString();//收费类别名称
                    mzjsxx.SHOUFEILX = dtShouFeiJL.Rows[i]["shoufeilx"].ToString();//收费类型
                    mzjsxx.SHOUFEILXMC = dtShouFeiJL.Rows[i]["SHOUFEILXMC"].ToString();//收费类型名称
                    mzjsxx.SHOUFEIRQ = dtShouFeiJL.Rows[i]["shoufeirq"].ToString();//收费日期
                    mzjsxx.JILULY = dtShouFeiJL.Rows[i]["jiluly"].ToString();//记录来源
                    mzjsxx.JILULYMC = dtShouFeiJL.Rows[i]["JILULYMC"].ToString();//记录来源名称
                    OutObject.MENZHENJSMX.Add(mzjsxx);
                }
            }

        }
    }
}
