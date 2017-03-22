using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_ZHIGONGXX : IMessage<ZD_ZHIGONGXX_IN, ZD_ZHIGONGXX_OUT>
    {
        public override void ProcessMessage()
        {
            var ZHIGONGLX = InObject.ZHIGONGLX;
            var ZHIGONGGH = InObject.ZHIGONGGH;
            var ZHIGONGXM = InObject.ZHIGONGXM;

            #region 参数验证
            //职工类型
            if (ZHIGONGLX == null || ZHIGONGLX == "")
            {
                throw new Exception(string.Format("职工类型不能为空！"));
            }
            else if (ZHIGONGLX != "1" && ZHIGONGLX != "2" && ZHIGONGLX != "0")
            {
                throw new Exception(string.Format("职工类型不正确，必须是：0.全部，1.医生，2.护士！"));
            }
            #endregion

            
            #region sql查询
            if(ZHIGONGLX!="0")
            {
                ZHIGONGLX=string.Format(" and ZGLB='{0}' ",ZHIGONGLX);
            }
            else
            {
                ZHIGONGLX="";
            }
            if (!string.IsNullOrEmpty(ZHIGONGGH))
            {
                ZHIGONGGH = string.Format(" and ZGGH='{0}' ", ZHIGONGGH);
            }
            if (!string.IsNullOrEmpty(ZHIGONGXM))
            {
                ZHIGONGXM = string.Format(" and ZGXM='{0}' ", ZHIGONGXM);
            }

            var listpbxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00001, ZHIGONGLX, ZHIGONGGH, ZHIGONGXM));

            if (listpbxx.Count == 0)
            {
                throw new Exception(string.Format("无职工信息！"));
            }
            else
            {
                OutObject = new ZD_ZHIGONGXX_OUT();

                foreach (var zgxx in listpbxx)
                {
                    var zhigonglb = new ZHIGONGXX();
                    zhigonglb.ZHIGONGGH = zgxx.Get("ZGGH");//工号
                    zhigonglb.ZHIGONGXM = zgxx.Get("ZGXM");//姓名
                    zhigonglb.ZHIGONGZC = zgxx.Get("ZGZC");//职称
                    zhigonglb.ZHIGONGJS = zgxx.Get("ZGJS");//介绍
                    zhigonglb.ZHIGONGTC = zgxx.Get("ZGTC");//特长
                    OutObject.ZHIGONGLB.Add(zhigonglb);
                }
            }
            #endregion
        }
    }
}
