using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_XIANGMUGLXX : IMessage<ZD_XIANGMUGLXX_IN, ZD_XIANGMUGLXX_OUT>
    {
        public override void ProcessMessage()
        {
            #region sql查询
            var listfyglxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00005));

            if (listfyglxx.Count == 0)
            {
                throw new Exception(string.Format("无项目归类信息！"));
            }
            else
            {
                OutObject = new ZD_XIANGMUGLXX_OUT();

                foreach (var fyglxx in listfyglxx)
                {
                    var fygllb = new XIANGMUGLXX();
                    fygllb.XIANGMUGL = fyglxx.Get("XIANGMUGL");
                    fygllb.XIANGMUGLMC = fyglxx.Get("XIANGMUGLMC");
                    OutObject.XIANGMUGLMX.Add(fygllb);
                }
            }
            #endregion
        }
    }
}
