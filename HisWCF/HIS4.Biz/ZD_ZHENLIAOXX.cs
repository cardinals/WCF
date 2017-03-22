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
    public class ZD_ZHENLIAOXX : IMessage<ZD_ZHENLIAOXX_IN, ZD_ZHENLIAOXX_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new ZD_ZHENLIAOXX_OUT();

            string xiangMuGl = InObject.XIANGMUGL;//项目归类
            string shuRuMLX = InObject.SHURUMLX;//输入码类型
            string shuRuM = InObject.SHURUM;//输入码
            string fenYuanDM = InObject.BASEINFO.FENYUANDM;//分院代码
            string MenZhenZYQY = InObject.MENZHENZYQY;

            #region 基础入参判断
            if (string.IsNullOrEmpty(xiangMuGl)) {
                xiangMuGl = "0";
            }

            if (string.IsNullOrEmpty(shuRuM)) {
                shuRuM = "*";
            }

            if (string.IsNullOrEmpty(fenYuanDM) || fenYuanDM == "0") {
                fenYuanDM = "1";
            }

            if (string.IsNullOrEmpty(MenZhenZYQY))
            {
                MenZhenZYQY = "0";
            }
            if (MenZhenZYQY != "0" && MenZhenZYQY != "1" && MenZhenZYQY != "2") {
                throw new Exception("门诊住院启用标识获取不正确，请确保其值为 0：全院 1：门诊 2：住院");
            }
            #endregion

            string zhenLiaoXXSQL = @"select * from v_zhenliaoxx_cx where (XIANGMUGL = {0} or 0 = {0}) and (shuruma1 like '{1}%' or '*' ='{1}') 
                        and ('0' = '{2}' or ('1' = '{2}' and Menzhensy = '1') or ('2' = '{2}' and Zhuyuansy = '1')) order by XIANGMUGL , XIANGMUMC ";



            DataTable dtZLXX = DBVisitor.ExecuteTable(string.Format(zhenLiaoXXSQL, xiangMuGl, shuRuM, MenZhenZYQY));


            if (dtZLXX.Rows.Count > 0)
            {
                for (int i = 0; i < dtZLXX.Rows.Count; i++)
                {

                    ZHENLIAOXX zlxx = new ZHENLIAOXX();

                    zlxx.XIANGMUGL = dtZLXX.Rows[i]["XIANGMUGL"].ToString();//项目归类
                    zlxx.XIANGMUXH = dtZLXX.Rows[i]["XIANGMUXH"].ToString();//项目序号
                    zlxx.XIANGMUCDDM = dtZLXX.Rows[i]["XIANGMUCDDM"].ToString();//项目产品代码
                    zlxx.XIANGMUMC = dtZLXX.Rows[i]["XIANGMUMC"].ToString();//项目名称
                    zlxx.XIANGMUGLMC = dtZLXX.Rows[i]["XIANGMUGLMC"].ToString();//项目归类名称
                    zlxx.XIANGMUGG = dtZLXX.Rows[i]["XIANGMUGG"].ToString();//项目规格
                    zlxx.XIANGMUDW = dtZLXX.Rows[i]["XIANGMUDW"].ToString();//项目单位
                    zlxx.DANJIA = Convert.ToDecimal( dtZLXX.Rows[i]["danjia"+fenYuanDM].ToString()).ToString("0.00");//单价
                    zlxx.YIBAODJ = dtZLXX.Rows[i]["YIBAODJ"].ToString();//医保等级
                    zlxx.MENZHENQY = dtZLXX.Rows[i]["Menzhensy"].ToString();//门诊启用
                    zlxx.ZHUYUANQY = dtZLXX.Rows[i]["Zhuyuansy"].ToString();//住院启用

                    OutObject.ZHENLIAOMX.Add(zlxx);
                }

            }
            else {
                throw new Exception("未找到相关诊疗信息!");
            }

        }
    }
}
