using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MEDI.SIIM.SelfServiceWeb.Entity;

namespace HisDllOp.dll
{
    public class HisDllOp
    {
        public int OpHis(string input, ref string output)
        {

            CallDll call = new CallDll();
            int i = -1;
            //讲传过来的XML字符串 转化为DataSet
            DataSet ds = Unity.CXmlToDataSet(input);

            if (ds.Tables.Count > 0)
            {
                switch (ds.Tables[0].Rows[0]["FunCode"].ToString().Trim())//根据FunCode值判断是哪个交易
                {
                    case "2001"://网络通讯测试
                        output = call.PingWL();
                        break;
                    case "2002"://获取人员信息接口
                        i = 0;
                        output = call.RENYUANXX(ds.Tables[0]);
                        break;
                    case "2101"://建档
                        i = 0;
                        output = call.RENYUANZC(ds.Tables[0]);
                        break;
                    case "2201"://挂号医生信息
                        i = 0;
                        output = call.GUAHAOKSXX(ds.Tables[0]);
                        break;
                    case "2202"://查询挂号医生
                        i = 0;
                        output = call.GUAHAOYSXX(ds.Tables[0]);
                        break;
                    case "2203"://挂号预算
                        i = 0;
                        output = call.GUAHAOCL(ds.Tables[0]);
                        break;
                    case "22031":
                        i = 0;
                        output = call.GUAHAOTH(ds.Tables[0]);
                        break;
                    case "2301":
                        i = 0;
                        output = call.MENZHENCFJL(ds.Tables[0]);
                        break;
                    case "2302":
                        i = 0;
                        output = call.MENZHENFYMX(ds.Tables[0]);
                        break;

                    default:
                        output = "<Response>"
                                + "<ResponseCode>-1</ResponseCode>"
                                + "<ResponseMsg>找不到对应的接口</ResponseMsg>"//应答信息
                                + "</Response>";
                        break;

                }
            }


            return i;
        }
    }
}
