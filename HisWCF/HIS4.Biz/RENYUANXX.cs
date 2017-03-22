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
    public class RENYUANXX : IMessage<RENYUANXX_IN,RENYUANXX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new RENYUANXX_OUT();
            int JIUZHENKCD = Convert.ToInt32(ConfigurationManager.AppSettings["JIUZHENKCD"]);//就诊卡默认长度
            char JIUZHENKTCZF = string.IsNullOrEmpty(ConfigurationManager.AppSettings["JIUZHENKTCZF"]) ? '0' :
                ConfigurationManager.AppSettings["JIUZHENKTCZF"].ToCharArray()[0];//就诊卡填充字符
            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string bingrenLb = InObject.BINGRENLB;//病人类别
            string yibaokLx = InObject.YIBAOKLX;//医保卡类型
            string yibaokMm = InObject.YIBAOKMM;//医保卡密码
            string yibaokXx = InObject.YIBAOKXX;//医保卡信息
            string yiliaoLb = InObject.YILIAOLB;//医疗类别
            string jiesuanLb = InObject.JIESUANLB;//结算类别
            string jiuzhenRq = InObject.JIUZHENRQ;//就诊日期
            string qianfeiKz = InObject.QIANFEIKZ;//欠费控制
            string yinhangKh = InObject.YINHANGKH;//银行卡号
            string caozuoyDm = InObject.BASEINFO.CAOZUOYDM;//操作员工号
            string zhengJianHm = InObject.ZHENGJIANHM;//证件号码
            string bingRenId = InObject.BINGRENID;//病人ID

            #region 基本入参判断
            //就诊卡号
            if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(zhengJianHm) && string.IsNullOrEmpty(bingRenId))
            {
                throw new Exception("就诊卡号和证件号不能同时为空");
            }

            if (string.IsNullOrEmpty(jiuzhenKh)) {
                jiuzhenKh = string.Empty;
            }

            //病人类别
            //if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(bingrenLb))
            //{
            //    throw new Exception("病人类别获取失败");
            //}

            //医疗类别
            //if (string.IsNullOrEmpty(yiliaoLb))
            //{
            //    throw new Exception("医疗类别获取失败");
            //}

            //结算类别
            //if (string.IsNullOrEmpty(jiesuanLb))
            //{
            //    throw new Exception("结算类别获取失败");
            //}
            #endregion

            #region 获取病人信息
            StringBuilder sbSql = new StringBuilder("select a.bingrenid,a.jiuzhenkh,a.feiyonglb bingrenlb,a.feiyongxz bingrenxz,a.yibaokh, ");
            sbSql.Append("a.gerenbh,a.binganhao binglibh,a.xingming,a.xingbie,a.minzu,to_char(a.chushengrq,'yyyy-mm-dd') chushengrq, ");
            sbSql.Append("'1' zhengjianlx,a.shenfenzh zhengjianhm,'' danweilx,'' danweibh,a.gongzuodw danweimc, ");
            sbSql.Append("a.jiatingdz jiatingzz,'' renyuanlb,nvl(b.qimoje,0) dangnianzhye,nvl(b.qimoje,0) linianzhye,a.teshubz teshubzbz, ");
            sbSql.Append("'' teshubzspbh,a.yibaobrxx,'' tishixx,a.youhuilb daiyulb,'' canbaoxzdm,'' teshudylb,");
            sbSql.Append("'' qianfeije,'' hisbrxx, lianxidh,'' qianyuebz,nvl(a.zijinzhqybz,0) kaihuzt,");
            sbSql.Append("''xunizh,'' guahaohmd,nvl(b.qimoje,0) zhanghuye,b.jiaoyimm jiaoyimm ,empiid ");
            sbSql.Append("from gy_v_bingrenxx a ,gy_zijinzh b where a.bingrenid = b.bingrenid(+) ");

            #region 就诊卡自动长度补全
            if (JIUZHENKCD > 0)
            {
                if (jiuzhenKh.Length < JIUZHENKCD)
                {
                    jiuzhenKh = jiuzhenKh.PadLeft(JIUZHENKCD, JIUZHENKTCZF);
                }
            }
            #endregion

            sbSql.Append(" and (a.jiuzhenkh='" + jiuzhenKh + "' or a.yibaokh='" + jiuzhenKh.TrimStart('0') + "' or nvl(a.shenfenzh,'*') = '" + zhengJianHm + "'  or a.bingrenid = '" + bingRenId + "' ) ");


            if (!string.IsNullOrEmpty(yinhangKh) && yinhangKh != "-1")
                sbSql.Append(" and a.YILIANMA='" + yinhangKh + "' ");//银行卡号过滤
            sbSql.Append(" order by a.yibaokh asc, a.xiugaisj desc ");
            #endregion
            #region 错误执行及错误判断
            DataTable dt = new DataTable();
            try
            {
                dt = DBVisitor.ExecuteTable(sbSql.ToString());
                if (dt.Rows.Count <= 0)//返回数据空
                {
                    throw new Exception("该卡未建档,请前往人工服务台建档!");
                }
                else
                {
                    OutObject.BINGRENID = dt.Rows[0]["BINGRENID"].ToString();//病人ID
                    OutObject.JIUZHENKH = dt.Rows[0]["JIUZHENKH"].ToString();//就诊卡号
                    OutObject.BINGRENLB = dt.Rows[0]["BINGRENLB"].ToString();//病人类别
                    OutObject.BINGRENXZ = dt.Rows[0]["BINGRENXZ"].ToString();//病人性质
                    OutObject.YIBAOKH = dt.Rows[0]["YIBAOKH"].ToString();//医保卡号
                    OutObject.GERENBH = dt.Rows[0]["GERENBH"].ToString();//个人编号
                    OutObject.BINGLIBH = dt.Rows[0]["BINGLIBH"].ToString();//病历本号
                    OutObject.XINGMING = dt.Rows[0]["XINGMING"].ToString();//姓名
                    OutObject.XINGBIE = dt.Rows[0]["XINGBIE"].ToString();//性别
                    OutObject.MINZU = dt.Rows[0]["MINZU"].ToString();//民族
                    OutObject.CHUSHENGRQ = dt.Rows[0]["CHUSHENGRQ"].ToString();//出生日期
                    OutObject.ZHENGJIANLX = dt.Rows[0]["ZHENGJIANLX"].ToString();//证件类别
                    OutObject.ZHENGJIANHM = dt.Rows[0]["ZHENGJIANHM"].ToString();//证件号码
                    OutObject.DANWEILX = dt.Rows[0]["DANWEILX"].ToString();//单位类型
                    OutObject.DANWEIBH = dt.Rows[0]["DANWEIBH"].ToString();//单位编号
                    OutObject.DANWEIMC = dt.Rows[0]["DANWEIMC"].ToString().Replace("<", "(").Replace(">", ")");//单位名称
                    
                    OutObject.JIATINGZZ = dt.Rows[0]["JIATINGZZ"].ToString().Replace("<", "(").Replace(">", ")");//家庭住址
                    OutObject.RENYUANLB = dt.Rows[0]["RENYUANLB"].ToString();//人员类别
                    OutObject.DANGNIANZHYE = dt.Rows[0]["DANGNIANZHYE"].ToString();//当年账户余额
                    OutObject.LINIANZHYE = dt.Rows[0]["LINIANZHYE"].ToString();//历年账户余额
                    OutObject.TESHUBZBZ = dt.Rows[0]["TESHUBZBZ"].ToString();//特殊病种标识
                    OutObject.TESHUBZSPBH = dt.Rows[0]["TESHUBZSPBH"].ToString();//特殊病种审批编号
                    OutObject.YIBAOBRXX = dt.Rows[0]["YIBAOBRXX"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//医保病人信息
                    OutObject.TISHIXX = dt.Rows[0]["TISHIXX"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//提示信息
                    OutObject.DAIYULB = dt.Rows[0]["DAIYULB"].ToString();//待遇类别
                    OutObject.CANBAOXZDM = dt.Rows[0]["CANBAOXZDM"].ToString();//参保性质代码
                    OutObject.TESHUDYLB = dt.Rows[0]["TESHUDYLB"].ToString();//特殊待遇类别
                    OutObject.HISBRXX = dt.Rows[0]["HISBRXX"].ToString().Replace("<", "(").Replace(">", ")").Replace("&", "|");//his病人信息
                    OutObject.LIANXIDH = dt.Rows[0]["LIANXIDH"].ToString();//联系电话
                    OutObject.QIANYUEBZ = dt.Rows[0]["QIANYUEBZ"].ToString();//签约标识
                    OutObject.KAIHUZT = dt.Rows[0]["KAIHUZT"].ToString();//开户状态
                    OutObject.XUNIZH = dt.Rows[0]["XUNIZH"].ToString();//虚拟账户
                    OutObject.GUAHAOHMD = dt.Rows[0]["GUAHAOHMD"].ToString();//挂号黑名单
                    OutObject.ZHANGHUYE = dt.Rows[0]["ZHANGHUYE"].ToString();//挂号黑名单
                    OutObject.JIAOYIMM = dt.Rows[0]["JIAOYIMM"].ToString();//院内账户交易密码
                    OutObject.EMPIID = dt.Rows[0]["EMPIID"].ToString();//实名制认证
                    //医保类型名称
                    OutObject.YIBAOBXLX = DBVisitor.ExecuteScalar(string.Format("select a.yewujh from yb_baoxianlb a,gy_feiyonglb b where a.yibaoid(+)=b.yibaoid and b.leibieid = {0} ", OutObject.BINGRENLB)).ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            #endregion

        }
    }
}
