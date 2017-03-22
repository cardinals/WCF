using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_BINGQUXX : IMessage<ZD_BINGQUXX_IN, ZD_BINGQUXX_OUT>
    {
        public override void ProcessMessage()
        {
            #region sql查询
            var listbqxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00003));

            if (listbqxx.Count == 0)
            {
                throw new Exception(string.Format("无病区信息！"));
            }
            else
            {
                OutObject = new ZD_BINGQUXX_OUT();

                foreach (var bqxx in listbqxx)
                {
                    var bingqulb = new BINGQUXX();
                    bingqulb.BINGQUDM = bqxx.Get("BINGQUDM");
                    bingqulb.BINGQUMC = bqxx.Get("BINGQUMC");
                    OutObject.BINGQUMX.Add(bingqulb);
                }
            }
            #endregion
        }
    }
}
