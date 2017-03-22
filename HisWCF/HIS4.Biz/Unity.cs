using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using SWSoft.Framework;
using System.Configuration;

namespace HIS4.Biz
{
    public class Unity
    {
        /// <summary>
        ///  Md5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMD5(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string secStr = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
            secStr = secStr.Replace("-", "");
            return secStr;
        }

        /// <summary>
        /// 获取就诊时间
        /// </summary>
        /// <param name="yuyuerq"></param>
        /// <param name="keshidm"></param>
        /// <param name="guahaoxh"></param>
        /// <param name="yuyuelx"></param>
        /// <param name="shangxiawpb"></param>
        /// <returns></returns>
        public static string getJiuZhenSJD(string yishengdm, string keshidm, string guahaoxh, string yuyuelx, int shangxiawpb,string guahaolb,string yuyuerq)
        {
            string MRJZSJD = ConfigurationManager.AppSettings["MRHaoYanJiuZhensjd"];
            if (string.IsNullOrEmpty(MRJZSJD)) {
                MRJZSJD = "";
            }
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append("select kaishisj||'-'||jieshusj as sjd from v_mz_houzhensj where keshiid = '" + keshidm + "' and " + guahaoxh + " >= qishighxh and " + guahaoxh + "<=jieshughxh " +
                "and shangxiawbz = " + shangxiawpb + " and nvl(yishengid,'*') = '" + yishengdm + "' ");

            if (guahaolb != "1") {
                sbsql.Append(" and ( xingqi =  to_char(to_date('" + yuyuerq + "','yyyy-mm-dd'),'d') or xingqi = '0' ) ");
            }

            DataTable dt = DBVisitor.ExecuteTable(sbsql.ToString());
            if (dt.Rows.Count > 0)//返回数据空
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["SJD"].ToString()))
                    return MRJZSJD;
                return dt.Rows[0]["SJD"].ToString();
            }
            return MRJZSJD;
        }

        //获取有效天数
        public static string GetYouXiaoTs()
        {
            //1手工处方2手工医技3电子处方4电子检验检查其他5处置6体检接口7代收挂号诊疗费中间用|分隔；0无穷天
            string youxiaoTs = "0";
            string sqlStr = "select pkg_pt_gongyong.Fun_GY_GetCanShu('00','门诊收费_结算有效天数','3') as youxiaots from dual ";
            DataTable dtCount = DBVisitor.ExecuteTable(sqlStr);
            if (dtCount.Rows.Count > 0 && dtCount.Rows[0][0].ToString() != string.Empty)
                youxiaoTs = dtCount.Rows[0][0].ToString();
            return youxiaoTs;
        }

        public static string GetXiangMuFY(string xiangMuDM) {
            string sqlXMFY = "select danjia1 from gy_shoufeixm where shoufeixmid = '{0}'";
            
            DataTable dtXMFY = DBVisitor.ExecuteTable(string.Format(sqlXMFY, xiangMuDM));
            string xmfy = string.Empty;
            if (dtXMFY.Rows.Count > 0)
            {
                xmfy = dtXMFY.Rows[0][0].ToString();
            }
            return xmfy;
        }

        //改年龄是否可挂号
        public static Boolean shiFouKeGHNianLin(string keShiDM,string zhenJianHM){
            string nianLinPd = ConfigurationManager.AppSettings["NianLinPD"];
            if (string.IsNullOrEmpty(nianLinPd)) {
                return true;
            }
            else if (nianLinPd == "0")
            {
                return true;
            }
            else {
                int age = getAGE(zhenJianHM);

                string sqlBuf = "select * from v_guahaoKeShiNianLin where keshidm = '{0}' and {1} >= nvl(qishinl,0) and {1} <= nvl(jieshunl,999)";

                DataTable dtCount = DBVisitor.ExecuteTable(string.Format(sqlBuf, keShiDM, age));

                if (dtCount.Rows.Count>0) {
                    return false;
                }
            }
            return true;
        }

        private static int getAGE(string zhenJianHM) {
             DateTime dTime;
            if (zhenJianHM.Length == 15) {
                dTime = DateTime.ParseExact("19" + zhenJianHM.Substring(6, 6), "yyyyMMdd", null);
            }
            else if (zhenJianHM.Length == 18)
            {
                dTime = DateTime.ParseExact(zhenJianHM.Substring(6, 8), "yyyyMMdd", null);
            }
            else {
                throw new Exception("身份证号码有误，请到服务台建档窗口补录!");
            }
            DateTime now = DateTime.Now;
          
            int fullYear = (now.Month - dTime.Month + 12 * (now.Year - dTime.Year)) / 12;
            if (fullYear < 0) {
                throw new Exception("身份证号码有误，请到服务台建档窗口补录!");
            }
            if (fullYear == 0) {
                fullYear = 1;
            }

            return fullYear;
        }

        /// <summary>
        /// 判断科室可挂号性别
        /// </summary>
        /// <param name="keShiDM"></param>
        /// <param name="zhenJianHM"></param>
        /// <returns></returns>
        public static Boolean shiFouKeGHXingBie(string keShiDM, string zhenJianHM) {
            string xingBiePd = ConfigurationManager.AppSettings["XingBiePD"];
            if (string.IsNullOrEmpty(xingBiePd) || xingBiePd == "0")
            {
                return true;
            }
            else {
                string sqlBuf = "select canshuzhi from gy_canshu where canshuid like '挂号_挂号科室与性别排斥' and yingyongid = '00'";
                string canShuZ = DBVisitor.ExecuteScalar(sqlBuf).ToString();
                int xingBie = getXingBie(zhenJianHM);
                if (xingBie == 1)//男
                {
                    string nanXingKS = canShuZ.Split('|')[0].ToString();
                    if (string.IsNullOrEmpty(nanXingKS) || nanXingKS == "*")
                    {
                        return true;
                    }
                    foreach (string item in nanXingKS.Split('^'))
                    {
                        if (keShiDM == item)
                        {
                            return false;
                        }
                    }
                }
                else {//女
                    string nvXingKS = canShuZ.Split('|')[1].ToString();
                    if (string.IsNullOrEmpty(nvXingKS)||nvXingKS == "*")
                    {
                        return true;
                    }
                    foreach (string item in nvXingKS.Split('^'))
                    {
                        if (keShiDM == item)
                        {
                            return false;
                        }
                    }
                
                }
            }
            
            return true;
        }

        private static int getXingBie(string zhenJianHM) {
            int Sex = 0;
            if (zhenJianHM.Length == 15)
            {
                Sex = Convert.ToInt32(zhenJianHM.Substring(14,1));
            }
            else if (zhenJianHM.Length == 18)
            {
                Sex = Convert.ToInt32(zhenJianHM.Substring(16, 1));
            }
            else
            {
                throw new Exception("身份证号码有误，请到服务台建档窗口补录!");
            }

            return 9;
        }
    }
}
