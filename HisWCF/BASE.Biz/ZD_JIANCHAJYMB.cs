using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_JIANCHAJYMB : IMessage<ZD_JIANCHAJYMB_IN, ZD_JIANCHAJYMB_OUT>
    {
        public override void ProcessMessage()
        {
            #region sql查询
            if (InObject.XIAZAILX == "1")
            {
                var listjcxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00009));

                if (listjcxx.Count == 0)
                {
                    throw new Exception(string.Format("无检查模版信息！"));
                }
                else
                {
                    OutObject = new ZD_JIANCHAJYMB_OUT();

                    foreach (var jcxx in listjcxx)
                    {
                        var jianchalb = new MOBANXX();
                        jianchalb.MOBANDM = jcxx.Get("MBLXDM");
                        jianchalb.MOBANMC = jcxx.Get("MBMC");
                        OutObject.MOBANMX.Add(jianchalb);
                    }
                }
            }
            else
            {
                var listjyxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00011));

                if (listjyxx.Count == 0)
                {
                    throw new Exception(string.Format("无检验模版信息！"));
                }
                else
                {
                    OutObject = new ZD_JIANCHAJYMB_OUT();

                    foreach (var jyxx in listjyxx)
                    {
                        var jianyanlb = new MOBANXX();
                        jianyanlb.MOBANDM = jyxx.Get("MBLXDM");
                        jianyanlb.MOBANMC = jyxx.Get("MBMC");
                        OutObject.MOBANMX.Add(jianyanlb);
                    }
                }
            }
            #endregion
        }
    }
}
