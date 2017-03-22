using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using System.Data;
using SWSoft.Framework;
using HIS4.Schemas;

namespace HIS4.Biz
{
    public class MENZHENYJS :IMessage<MENZHENYJS_IN,MENZHENYJS_OUT>
    {
        public override void ProcessMessage()
        {
            string jiuZhenKLX = InObject.JIUZHENKLX.ToString();//就诊卡类型
            string jiuZhenKH = InObject.JIUZHENKH.ToString();//就诊卡号
            string bingRenLB = InObject.BINGRENLB.ToString();//病人类别
            string bingRenXZ = InObject.BINGRENXZ.ToString();//病人性质
            string yiBaoKLX = InObject.YIBAOKLX.ToString();//医保卡类型
            string yiBaoKMM = InObject.YIBAOKMM.ToString();//医保卡密码
            string yiBaoKXX = InObject.YIBAOKXX.ToString();//医保卡信息
            string yiBaoBRXX = InObject.YIBAOBRXX.ToString();//医保病人信息
            string yiLiaoLB = InObject.YILIAOLB.ToString();//医疗类别
            string jieSuanLB = InObject.JIESUANLB.ToString();//结算类别
            string hisBRXX = InObject.HISBRXX.ToString();//his病人信息

            #region 基础入参判断
            //就诊卡号
            if (string.IsNullOrEmpty(jiuZhenKH))
            {
                throw new Exception("就诊卡号获取失败");
            }

            //就诊卡类型
            if (string.IsNullOrEmpty(jiuZhenKH))
            {
                throw new Exception("就诊卡类型获取失败");
            }

            //病人类别
            if (string.IsNullOrEmpty(bingRenLB))
            {
                throw new Exception("病人类别获取失败");
            }

            //病人性质
            if (string.IsNullOrEmpty(bingRenXZ))
            {
                throw new Exception("病人性质获取失败");
            }
            #endregion

            
            DataTable dt = DBVisitor.ExecuteTable("");
            if (dt.Rows.Count > 0)
            {
                OutObject.JIESUANID = dt.Rows[0][0].ToString();//结算id
            }

            if (bingRenXZ == "XJ01")
            {
                #region 自费
                decimal fyze = 0;//费用总额
                decimal xjzf = 0;//现金支付金额
                foreach (var item in InObject.FEIYONGMX)
                {
                    fyze += Convert.ToDecimal(item.JINE);
                    xjzf += Convert.ToDecimal(item.JINE) * Convert.ToDecimal(string.IsNullOrEmpty(item.ZIFUBL) ? "1" : item.ZIFUBL); //金额*自付比例
                }

                OutObject.JIESUANJG.FEIYONGZE = fyze.ToString();//费用总额
                OutObject.JIESUANJG.ZILIJE = fyze.ToString();//自理金额
                OutObject.JIESUANJG.ZIFEIJE = fyze.ToString();//自费金额
                OutObject.JIESUANJG.ZIFUJE = fyze.ToString();//自负金额
                OutObject.JIESUANJG.YIYUANCDJE = "0";//医院承担金额
                OutObject.JIESUANJG.BAOXIAOJE = "0";//报销金额
                OutObject.JIESUANJG.XIANJINZF = xjzf.ToString();//现金支付
                OutObject.JIESUANJG.DONGJIEJE = "0";//冻结金额
                OutObject.JIESUANJG.YOUHUIJE = (fyze - xjzf).ToString();//优惠金额

                foreach (var item in InObject.FEIYONGMX)
                {
                    var zfmx = new MENZHENFYZFXX();
                    zfmx.CHUFANGXH = item.CHUFANGXH;//	处方序号
                    zfmx.MINGXIXH = item.MINGXIXH;//	明细序号
                    zfmx.YIBAODM = item.YIBAODM;//	医保代码
                    zfmx.YIBAOZFBL = item.YIBAOZFBL;//	医保自付比例
                    zfmx.XIANGMUXJ = item.XIANGMUXJ;//	项目限价
                    zfmx.YIBAOXMGL = item.XIANGMUGL;//	医保项目归类
                    zfmx.YIBAODJ = item.YIBAODJ;//	医保等级
                    zfmx.ZIFEIJE = item.ZIFEIJE;//	自费金额
                    zfmx.ZILIJE = item.ZILIJE;//	自理金额
                    zfmx.BEIZHUXX = "";//	备注信息
                }
                #endregion
            }
            else
            {
                #region 医保

                #endregion
            }
        }
    }
}
