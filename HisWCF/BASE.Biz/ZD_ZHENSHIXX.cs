using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_ZHENSHIXX : IMessage<ZD_ZHENSHIXX_IN, ZD_ZHENSHIXX_OUT>
    {
        public override void ProcessMessage()
        {
            #region sql查询
            var listpbxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00002));

            if (listpbxx.Count == 0)
            {
                throw new Exception(string.Format("无诊室信息！"));
            }
            else
            {
                OutObject = new ZD_ZHENSHIXX_OUT();

                foreach (var zsxx in listpbxx)
                {
                    var zhenshilb = new ZHENSHIXX();
                    zhenshilb.ZHENSHIDM = zsxx.Get("ZSDM");
                    zhenshilb.ZHENSHIMC = zsxx.Get("ZSMC");
                    OutObject.ZHENSHILB.Add(zhenshilb);
                }
            }
            #endregion
        }
    }
}
