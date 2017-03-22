using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_YAOPINXX : IMessage<ZD_YAOPINXX_IN, ZD_YAOPINXX_OUT>
    {
        public override void ProcessMessage()
        {
            var fygl = InObject.XIANGMUGL;
            var srmlx = InObject.SHURUMLX;
            var srm = InObject.SHURUM;

            #region sql查询
            var listypxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00006, fygl,srmlx,srm));

            if (listypxx.Count == 0)
            {
                throw new Exception(string.Format("无药品信息！"));
            }
            else
            {
                OutObject = new ZD_YAOPINXX_OUT();

                foreach (var ypxx in listypxx)
                {
                    var yaopinlb = new YAOPINXX();
                    yaopinlb.XIANGMUGL = ypxx.Get("XIANGMUGL").ToString();
                    yaopinlb.XIANGMUXH = ypxx.Get("XIANGMUXH").ToString();
                    yaopinlb.XIANGMUCDDM = ypxx.Get("XIANGMUCDDM").ToString();
                    yaopinlb.XIANGMUMC = ypxx.Get("XIANGMUMC");
                    yaopinlb.XIANGMUGLMC = ypxx.Get("XIANGMUGLMC");
                    yaopinlb.XIANGMUGG = ypxx.Get("XIANGMUGG");
                    yaopinlb.XIANGMUJX = ypxx.Get("XIANGMUJX").ToString();
                    yaopinlb.XIANGMUDW = ypxx.Get("XIANGMUDW");
                    yaopinlb.XIANGMUCDMC = ypxx.Get("XIANGMUCDMC");
                    yaopinlb.DANJIA = ypxx.Get("DANJIA").ToString();
                    yaopinlb.YIBAODJ = ypxx.Get("YIBAODJ");
                    OutObject.YAOPINMX.Add(yaopinlb);
                }
            }
            #endregion
        }
    }
}
