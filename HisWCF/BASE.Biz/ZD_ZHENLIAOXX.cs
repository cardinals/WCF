using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_ZHENLIAOXX : IMessage<ZD_ZHENLIAOXX_IN, ZD_ZHENLIAOXX_OUT>
    {
        public override void ProcessMessage()
        {
            var fygl = InObject.XIANGMUGL;
            var xiangMuLX = InObject.XIANGMULX.ToString();
            var srmlx = InObject.SHURUMLX;
            var srm = InObject.SHURUM.ToUpper();

            #region 查询套餐 套餐明细
            if (!(string.IsNullOrEmpty(fygl)) && fygl.Trim() == "1" && (xiangMuLX == "8" || xiangMuLX == "9"))
            {
                if (xiangMuLX == "8")
                {
                    string xmgl = " and sfxm=99";
                    if (srmlx == "0")
                    {
                        xmgl += " and SRM1 like '" + srm + "%'";
                    }//拼音码
                    else if (srmlx == "1")
                    {
                        xmgl += " and SRM2 like '" + srm + "%'";
                    }//五笔码
                    else if (srmlx == "2")
                    {
                        xmgl += " and YLMC like '" + srm + "%'";
                    }//汉字
                    var xmmclist = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00013, xmgl));
                    if (xmmclist.Count == 0)
                    {
                        throw new Exception(string.Format("无套餐信息！"));
                    }
                    else
                    {
                        OutObject = new ZD_ZHENLIAOXX_OUT();
                        foreach (var zlxx in xmmclist)
                        {
                            var zhenliaolb = new ZHENLIAOXX();
                            zhenliaolb.XIANGMUGL = zlxx.Get("SFXM").ToString();
                            zhenliaolb.XIANGMUXH = zlxx.Get("YLXH").ToString();
                            zhenliaolb.XIANGMUMC = zlxx.Get("YLMC");
                            zhenliaolb.XIANGMUGLMC = "诊疗套餐";
                            zhenliaolb.XIANGMUDW = zlxx.Get("DW");
                            zhenliaolb.DANJIA = zlxx.Get("DJ").ToString();
                            zhenliaolb.YIBAODJ = "0";
                            OutObject.ZHENLIAOMX.Add(zhenliaolb);
                        }
                    }

                }
                else if (xiangMuLX == "9")
                {
                    if (string.IsNullOrEmpty(InObject.XIANGMUXH))
                    {
                        throw new Exception("查询项目明细时,项目序号不能为空！");
                    }
                    string xmgl = "";
                    if (srmlx == "0")
                    {
                        xmgl += " and SRM1 like '" + srm + "%'";
                    }//拼音码
                    else if (srmlx == "1")
                    {
                        xmgl += " and SRM2 like '" + srm + "%'";
                    }//五笔码
                    else if (srmlx == "2")
                    {
                        xmgl += " and YLMC like '" + srm + "%'";
                    }//汉字
                    var xmmclist = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00015, InObject.XIANGMUXH,xmgl));
                    if (xmmclist.Count == 0)
                    {
                        throw new Exception(string.Format("无套餐明细信息！"));
                    }
                    else
                    {
                        OutObject = new ZD_ZHENLIAOXX_OUT();
                        foreach (var zlxx in xmmclist)
                        {
                            var zhenliaolb = new ZHENLIAOXX();
                            zhenliaolb.XIANGMUGL = zlxx.Get("XIANGMUGL").ToString();
                            zhenliaolb.XIANGMUXH = zlxx.Get("XIANGMUXH").ToString();
                            zhenliaolb.XIANGMUMC = zlxx.Get("XIANGMUMC");
                            zhenliaolb.XIANGMUGLMC = zlxx.Get("XIANGMUGLMC");
                            zhenliaolb.XIANGMUDW = zlxx.Get("XIANGMUDW");
                            zhenliaolb.DANJIA = zlxx.Get("DANJIA").ToString();
                            zhenliaolb.YIBAODJ = zlxx.Get("YIBAODJ");
                            OutObject.ZHENLIAOMX.Add(zhenliaolb);
                        }
                    }

                }

                else
                {
                    throw new Exception(string.Format("诊疗项目类型错误，请参见数据字典！"));
                }
            }
            #endregion

            #region sql查询
            else
            {
                string xmgl = "";
                if (!(string.IsNullOrEmpty(fygl)) && fygl.Trim() == "1")
                {
                    fygl = "0";

                    switch (xiangMuLX)
                    {
                        case "1":
                            xmgl = " and ylmc = '检查费' and ylxh < 100 ";
                            break;
                        case "2":
                            xmgl = " and ylmc = '治疗费' and ylxh < 100 ";
                            break;
                        case "3":
                            xmgl = " and ylmc = '放射费' and ylxh < 100 ";
                            break;
                        case "4":
                            xmgl = " and ylmc = '手术费' and ylxh < 100 ";
                            break;
                        case "5":
                            xmgl = " and ylmc = '化验费' and ylxh < 100 ";
                            break;
                        case "6":
                            xmgl = " and ylmc = '输血费' and ylxh < 100 ";
                            break;
                        case "7":
                            xmgl = " and ylmc = '输氧费' and ylxh < 100 ";
                            break;
                        case "99":
                            xmgl = " and ylmc not in('检查费','治疗费','放射费','手术费','化验费','输血费','输氧费' ) and ylxh < 100 ";
                            break;
                        default:
                            xmgl = " and ylxh < 100 ";
                            break;

                    }

                    var xmmclist = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00013, xmgl));
                    if (xmmclist.Count == 0)
                    {
                        xmgl = "";
                    }
                    else
                    {
                        var temp = "";
                        foreach (var item in xmmclist)
                        {
                            temp += "'" + item.Get("ylmc") + "',";
                        }

                        xmgl = " and XIANGMUGLMC in ( " + temp.Trim(',').ToString() + " )";
                    }
                }
                var listzlxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00007, fygl, srmlx, srm, xmgl));

                if (listzlxx.Count == 0)
                {
                    throw new Exception(string.Format("无诊疗信息！"));
                }
                else
                {
                    OutObject = new ZD_ZHENLIAOXX_OUT();

                    foreach (var zlxx in listzlxx)
                    {
                        var zhenliaolb = new ZHENLIAOXX();
                        zhenliaolb.XIANGMUGL = zlxx.Get("XIANGMUGL").ToString();
                        zhenliaolb.XIANGMUXH = zlxx.Get("XIANGMUXH").ToString();
                        zhenliaolb.XIANGMUMC = zlxx.Get("XIANGMUMC");
                        zhenliaolb.XIANGMUGLMC = zlxx.Get("XIANGMUGLMC");
                        zhenliaolb.XIANGMUDW = zlxx.Get("XIANGMUDW");
                        zhenliaolb.DANJIA = zlxx.Get("DANJIA").ToString();
                        zhenliaolb.YIBAODJ = zlxx.Get("YIBAODJ");
                        OutObject.ZHENLIAOMX.Add(zhenliaolb);
                    }
                }
            }
            #endregion
        }
    }
}
