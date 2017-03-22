using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_CHUANGWEIXX : IMessage<ZD_CHUANGWEIXX_IN, ZD_CHUANGWEIXX_OUT>
    {
        public override void ProcessMessage()
        {
            var bqdm = InObject.BINGQUDM;

            #region sql查询
            var listcwxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00004,bqdm));

            if (listcwxx.Count == 0)
            {
                throw new Exception(string.Format("无床位信息！"));
            }
            else
            {
                OutObject = new ZD_CHUANGWEIXX_OUT();

                foreach (var cwxx in listcwxx)
                {
                    var chuangweilb = new CHUANGWEIXX();
                    chuangweilb.CHUANGWEIH = cwxx.Get("CHUANGWEIH").ToString();
                    chuangweilb.CHUANGWEISM = cwxx.Get("CHUANGWEISM");
                    OutObject.CHUANGWEIMX.Add(chuangweilb);
                }
            }
            #endregion
        }
    }
}
