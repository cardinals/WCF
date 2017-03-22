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
    public class ZD_YAOPINXX : IMessage<ZD_YAOPINXX_IN, ZD_YAOPINXX_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new ZD_YAOPINXX_OUT();

            string xiangmuGl = InObject.XIANGMUGL;
            string shurumaLx = InObject.SHURUMLX;
            string shuruMa = InObject.SHURUM;
            string MenZhenZYQY = InObject.MENZHENZYQY;
            string fenYuanDM = InObject.BASEINFO.FENYUANDM;//分院代码
            if (string.IsNullOrEmpty(xiangmuGl)) {
                xiangmuGl = "0";
            }

            if (string.IsNullOrEmpty(MenZhenZYQY))
            {
                MenZhenZYQY = "0";
            }

            if (string.IsNullOrEmpty(fenYuanDM) || fenYuanDM == "0")
            {
                fenYuanDM = "1";
            }

            StringBuilder sbSql = new StringBuilder("select * from  v_yaopinxx_cx where 1=1 ");
            if (xiangmuGl != "0")
                sbSql.Append(" and yaopinlx='" + xiangmuGl + "' ");
            if (!string.IsNullOrEmpty(shuruMa))
                sbSql.Append(" and shuruma1 like '" + shuruMa + "%' ");
            if (MenZhenZYQY == "1") {
                sbSql.Append(" and menzhenqybz = 1 ");   
            }
            if (MenZhenZYQY == "2")
            {
                sbSql.Append(" and zhuyuanqybz = 1 ");
            }

            sbSql.Append("  order by XIANGMUGL,XIANGMUMC ");

            DataTable dtYaoPinXX = DBVisitor.ExecuteTable(sbSql.ToString());
            
            if (dtYaoPinXX.Rows.Count > 0) {
                for (int i = 0; i < dtYaoPinXX.Rows.Count; i++)
                {
                    YAOPINXX ypxx = new YAOPINXX();
                    ypxx.XIANGMUGL = dtYaoPinXX.Rows[i]["XIANGMUGL"].ToString();//项目归类
                    ypxx.XIANGMUXH = dtYaoPinXX.Rows[i]["XIANGMUXH"].ToString();//项目序号
                    ypxx.XIANGMUCDDM = dtYaoPinXX.Rows[i]["XIANGMUCDDM"].ToString();//项目产地代码
                    ypxx.XIANGMUMC = dtYaoPinXX.Rows[i]["XIANGMUMC"].ToString();//项目名称
                    ypxx.XIANGMUGLMC = dtYaoPinXX.Rows[i]["XIANGMUGLMC"].ToString();//项目归类吗名称
                    ypxx.XIANGMUGG = dtYaoPinXX.Rows[i]["XIANGMUGG"].ToString();//项目规格
                    ypxx.XIANGMUJX = dtYaoPinXX.Rows[i]["XIANGMUJX"].ToString();//项目剂型
                    ypxx.XIANGMUDW = dtYaoPinXX.Rows[i]["XIANGMUDW"].ToString();//项目单位
                    ypxx.XIANGMUCDMC = dtYaoPinXX.Rows[i]["XIANGMUCDMC"].ToString();//项目产地名称
                    ypxx.DANJIA = Convert.ToDecimal( dtYaoPinXX.Rows[i]["danjia"+fenYuanDM].ToString()).ToString("0.00");//单价
                    ypxx.YIBAODJ = dtYaoPinXX.Rows[i]["yibaodj"].ToString();//医保等级
                    ypxx.MENZHENQY = dtYaoPinXX.Rows[i]["menzhenqybz"].ToString();//门诊启用
                    ypxx.ZHUYUANQY = dtYaoPinXX.Rows[i]["zhuyuanqybz"].ToString();//住院启用
                    OutObject.YAOPINMX.Add(ypxx);
                }
            }
            else{
                throw new Exception("未找到相关的药品信息");
            }

        }

    }
}
