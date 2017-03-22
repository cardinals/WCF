using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data.OracleClient;
using System.Data.Common;
using HIS4.Schemas;
using System.Configuration;

namespace HIS4.Biz
{
    public class RENYUANZC : IMessage<RENYUANZC_IN,RENYUANZC_OUT>
    {
        public override void ProcessMessage()
        {
            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string bingrenLb = InObject.BINGRENLB;//病人类别
            string yibaokLx = InObject.YIBAOKLX;//医保卡类型
            string yibaokXx = InObject.YIBAOKXX;//医保卡信息
            string yibaokh = InObject.YIBAOKH;//医保卡号
            string gerenBh = InObject.GERENBH;//个人编号
            string bingliBh = InObject.BINGLIBH;//病历本号
            string xingMing = InObject.XINGMING;//姓名
            string xingBie = InObject.XINGBIE;//性别
            string minZu = InObject.MINZU;//民族
            string chushengRq = InObject.CHUSHENGRQ;//出生日期 YYYT-MM-DD
            string zhengjianLx = InObject.ZHENGJIANLX;//证件类型
            string zhengjianHm = InObject.ZHENGJIANHM;//证件号码
            string danweiLx = InObject.DANWEILX;//单位类型
            string danweiBh = InObject.DANWEIBH;//单位编号
            string danweiMc = InObject.DANWEIMC;//单位名称
            string jiatingZz = InObject.JIATINGZZ;//家庭住址
            string renyuanLb = InObject.RENYUANLB;//人员类别
            string lianxiDh = InObject.LIANXIDH;//联系电话
            string yinhangKh = InObject.YINHANGKH;//银行卡号
            string qianyueBz = InObject.QIANYUEBZ;//签约标识
            string yiliaoLb = InObject.YILIAOLB;//医疗类别
            string jiesuanLb = InObject.JIESUANLB;//结算类别
            string yibaokMm = InObject.YIBAOKMM;//医保卡密码
            string photo = InObject.PHOTO;//照片 二进制码流
            string shifouYk = InObject.SHIFOUYK;//是否有卡
            string bangdingYhk = InObject.BANGDINGYHK;//绑定银行卡
            string zhongduansbXx = InObject.ZHONGDUANSBXX;//终端识别信息
            string bingrenXz = InObject.BINGRENXZ;//病人性质
            string chongzhiJe = InObject.CHONGZHIJE;//充值金额
            string zhifuFs = InObject.ZHIFUFS;//支付方式
            string jiandangRen = InObject.BASEINFO.CAOZUOYDM;//建档人工号
            string caozuoRq = InObject.BASEINFO.CAOZUORQ;//操作日期
            string empiId = InObject.EMPIID;//实名制认证标识

            OutObject = new RENYUANZC_OUT();

            int JIUZHENKCD = Convert.ToInt32(ConfigurationManager.AppSettings["JIUZHENKCD"]);//就诊卡默认长度
            char JIUZHENKTCZF = string.IsNullOrEmpty(ConfigurationManager.AppSettings["JIUZHENKTCZF"]) ? '0' :
                ConfigurationManager.AppSettings["JIUZHENKTCZF"].ToCharArray()[0];//就诊卡填充字符

            #region 基本入参判断
            if (string.IsNullOrEmpty(jiuzhenKh)&&string.IsNullOrEmpty(yibaokh))
            {
                throw new Exception("就诊卡号和医保卡号不能同时为空！");  
            }

            if (string.IsNullOrEmpty(jiuzhenKh))
            {
                jiuzhenKh = yibaokh;
            }

            if (string.IsNullOrEmpty(zhengjianHm)) {
                throw new Exception("证件号码不能为空！");
            }

            if (string.IsNullOrEmpty(xingMing))
            {
                throw new Exception("病人姓名不能为空！");
            }
            if (string.IsNullOrEmpty(xingBie))
            {
                throw new Exception("病人性别不能为空！");
            }

            if (string.IsNullOrEmpty(chushengRq))
            {
                throw new Exception("出生日期不能为空！");
            }

            if (string.IsNullOrEmpty(jiatingZz)) {
                throw new Exception("家庭住址不能为空！");
            }
            if (string.IsNullOrEmpty(lianxiDh)) {
                throw new Exception("联系电话不能为空！");
            }
            #endregion

            #region 检查档案状态
            

            StringBuilder sbSql = new StringBuilder("select a.bingrenid,a.jiuzhenkh ,a.xingming ");
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
            sbSql.Append(" and (a.jiuzhenkh='" + jiuzhenKh + "' or a.yibaokh='" + yibaokh + "' or nvl(a.shenfenzh,'*') = '" + zhengjianHm + "' ) ");
            sbSql.Append(" order by a.yibaokh asc, a.xiugaisj desc ");
            #endregion

            DataTable dtRenYuanXX = DBVisitor.ExecuteTable(sbSql.ToString());
            if (dtRenYuanXX == null|| dtRenYuanXX.Rows.Count <= 0)
            {

                #region 建档
                
                string sqlStr = "select FUN_GY_GETORDER('1', 'GY_BINGRENXX', 10) as bingrenid from dual ";
                string bingrenId = DBVisitor.ExecuteScalar(sqlStr).ToString();
                string shoukuanRq = Convert.ToDateTime(caozuoRq).ToString("yyyy-MM-dd HH:mm:ss");
                string jiaoyanMa = Unity.GetMD5(bingrenId + Convert.ToSingle(chongzhiJe).ToString("f4") + shoukuanRq + "1" + jiandangRen).ToLower();

                //医保卡信息特殊处理
                if (!string.IsNullOrEmpty(yibaokXx) &&  yibaokXx.Contains('$'))
                    yibaokXx = yibaokXx.Replace('$', '&');

                #region 医保病人需要转化获取所需的性质ID
                //联众医保yibaobrxz不填,要在包里取//add by renj
                //if (bingrenLb != "1")
                //{
                //    string sqlYb = string.Format("select xingzhiid from gy_feiyongxz where feiyonglb='{0}' and yibaobrxz='{1}'", bingrenLb, bingrenXz);
                //    bingrenXz = new DBServer().GetCurrData(sqlYb);
                //}
                #endregion
                #region 性别信息转换
                if (xingBie == "0")
                {
                    xingBie = "未知";
                }
                else if (xingBie == "1")
                {
                    xingBie = "男";
                }
                else if (xingBie == "2")
                {
                    xingBie = "女";
                }
                else
                {
                    xingBie = "其他";
                }
                #endregion

                #region 入参信息拼装
                //病人性质？ 建档人？
                //数据库包中有建档自动开启电子账户功能 可通过注释包语句进行该功能的关闭
                string tradeMsgList = jiuzhenkLx + "|" + jiuzhenKh + "|" + bingrenLb + "|" + yibaokLx + "|" + yibaokXx + "|" + yibaokh + "|"
                    + gerenBh + "|" + bingliBh + "|" + xingMing + "|" + xingBie + "|" + minZu + "|" + chushengRq + "|"
                    + zhengjianLx + "|" + zhengjianHm + "|" + danweiLx + "|" + danweiBh + "|" + danweiMc + "|" + jiatingZz + "|"
                    + renyuanLb + "|" + lianxiDh + "|" + yinhangKh + "|" + qianyueBz + "|" + yiliaoLb + "|" + jiesuanLb + "|"
                    + yibaokMm + "|" + photo + "|" + shifouYk + "|" + bangdingYhk + "|" + zhongduansbXx + "|" + jiandangRen + "|"
                    + chongzhiJe + "|" + bingrenId + "|" + zhifuFs + "|" + shoukuanRq + "|" + jiaoyanMa + "|" + bingrenXz + "|";

                //LogHelper.WriteLog(typeof(GG_JiaoYiBLL), "建档交易入参：" + tradeMsgList);//交易入参日志
                #endregion

                OracleParameter[] paramJiaoYi = new OracleParameter[3];
                paramJiaoYi[0] = new OracleParameter("PRM_MSG", OracleType.VarChar);
                paramJiaoYi[0].Value = tradeMsgList;
                paramJiaoYi[0].Direction = ParameterDirection.Input;
                paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
                paramJiaoYi[1].Value = null;
                paramJiaoYi[1].Direction = ParameterDirection.Output;
                paramJiaoYi[2] = new OracleParameter("PRM_OUTBUFFER", OracleType.VarChar);
                paramJiaoYi[2].Value = null;
                paramJiaoYi[2].Size = 2000;
                paramJiaoYi[2].Direction = ParameterDirection.Output;

                string returnValue = string.Empty;
                DbTransaction transaction = null;
                DbConnection conn = DBVisitor.Connection;
                try
                {
                    transaction = conn.BeginTransaction();
                    DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_BINGRENJD", paramJiaoYi, transaction);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        //transaction.Connection.Close();
                    }
                    throw new Exception(ex.Message);
                }
                returnValue = paramJiaoYi[1].Value.ToString();
                string returnMsg = paramJiaoYi[2].Value.ToString();
                string zhanghuId = string.Empty;
                if (returnValue == "1")//建档及生成虚拟账户成功
                {
                    transaction.Commit();
                    //transaction.Connection.Close();
                    //zhanghuId = returnMsg.Split('|')[1];//账户ID
                    //此处需要处理返回成功的xml
                    OutObject.JIUZHENKH = jiuzhenKh;
                    OutObject.BINGRENID = bingrenId;
                    //OutObject.XUNIZH = zhanghuId;

                }
                else
                {
                    transaction.Rollback();
                    //transaction.Connection.Close();
                    throw new Exception(returnMsg);
                }
                #endregion
            }
            else {
                if (InObject.XINGMING != dtRenYuanXX.Rows[0]["xingming"].ToString()) {
                    throw new Exception("该卡号和病人姓名不匹配，请核查！");
                }

                OutObject.JIUZHENKH = dtRenYuanXX.Rows[0]["jiuzhenkh"].ToString();
                OutObject.BINGRENID = dtRenYuanXX.Rows[0]["bingrenid"].ToString();
            }

            if (!string.IsNullOrEmpty(OutObject.BINGRENID) && !string.IsNullOrEmpty(empiId)) {
                DBVisitor.ExecuteNonQuery(string.Format(" update gy_bingrenxx set empiid ='{0}' where bingrenid = '{1}' ",empiId,OutObject.BINGRENID ));
            }
        }
    }
}
