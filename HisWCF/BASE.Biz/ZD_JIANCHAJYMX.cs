using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_JIANCHAJYMX : IMessage<ZD_JIANCHAJYMX_IN, ZD_JIANCHAJYMX_OUT>
    {
        public override void ProcessMessage()
        {
            #region sql查询
            if (InObject.XIAZAILX == "1")
            {
                var listjcxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00010));

                if (listjcxx.Count == 0)
                {
                    throw new Exception(string.Format("无检查明细信息！"));
                }
                else
                {
                    OutObject = new ZD_JIANCHAJYMX_OUT();

                    foreach (var jcxx in listjcxx)
                    {
                        var jianchalb = new JIANCHAJYXX();
                        jianchalb.MOBANDM = jcxx.Get("MBLXDM");
                        jianchalb.JIANCHAJYDM = jcxx.Get("LBXH");
                        jianchalb.JIANCHAJYMC = jcxx.Get("LBMC");
                        jianchalb.FULEIXH = jcxx.Get("FLXH");
                        jianchalb.MOJIEPB = jcxx.Get("SFMJ");
                        jianchalb.ZHIXINGKS = jcxx.Get("JCKS");
                        jianchalb.BEIZHU = jcxx.Get("JCYQ");
                        OutObject.JIANCHAJYMX.Add(jianchalb);
                    }
                }
            }
            else
            {
                var listjyxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00012));

                if (listjyxx.Count == 0)
                {
                    throw new Exception(string.Format("无检验明细信息！"));
                }
                else
                {
                    OutObject = new ZD_JIANCHAJYMX_OUT();

                    foreach (var jyxx in listjyxx)
                    {
                        var jianyanlb = new JIANCHAJYXX();
                        jianyanlb.MOBANDM = jyxx.Get("MBLXDM");
                        jianyanlb.JIANCHAJYDM = jyxx.Get("LBXH");
                        jianyanlb.JIANCHAJYMC = jyxx.Get("LBMC");
                        jianyanlb.FULEIXH = "";
                        jianyanlb.MOJIEPB = "0";
                        jianyanlb.ZHIXINGKS = "";
                        jianyanlb.BEIZHU = jyxx.Get("YLXH");
                        OutObject.JIANCHAJYMX.Add(jianyanlb);
                    }
                }
            }
            #endregion
        }
    }
}
