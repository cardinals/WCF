using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;

namespace HIS4.Biz
{
    /// <summary>
    /// 支付宝签约信息查询
    /// </summary>
    public class ZHIFUBAOBDCX : IMessage<ZHIFUBAOBDCX_IN, ZHIFUBAOBDCX_OUT>
    {
        public override void ProcessMessage()
        {
            if (string.IsNullOrEmpty(InObject.ZHENGJIANHM))
            {
                throw new Exception("证件号码不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.XINGMING))
            {
                throw new Exception("姓名不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
            {
                throw new Exception("分院代码不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.JIUZHENKLX))
            {
                throw new Exception("就诊卡类型不能为空");
            }
            if (string.IsNullOrEmpty(InObject.JIUZHENKH))
            {
                throw new Exception("就诊卡号不能为空");
            }
            //判断是否存在传入身份证号的病人信息
            string fydmwhere = "";
            if (InObject.JIUZHENKLX != "2")
            { //如果是社保卡 则不增加医院条件 社保卡 全区都可以用

                fydmwhere = string.Format(" and  FENYUANDM='{0}'", InObject.BASEINFO.FENYUANDM);
            }
            InObject.JIUZHENKH = System.Convert.ToString(InObject.JIUZHENKH.ToUpper()).PadLeft(10, '0');
            var existInfo = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.HIS00004, InObject.ZHENGJIANHM.ToLower(), InObject.JIUZHENKH, InObject.JIUZHENKLX, fydmwhere));
            //if (InObject.JIUZHENKLX == "2" && InObject.BASEINFO.FENYUANDM == "10006" && existInfo == null)
            //{
            //    if (InObject.JIUZHENKH.Length == 10)
            //    {
            //        existInfo = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.HIS00004, InObject.ZHENGJIANHM.ToLower(), InObject.JIUZHENKH.Substring(1, 9), InObject.JIUZHENKLX, fydmwhere));
            //    }
            //}

            if (existInfo == null || existInfo.Items["ZUOFEIPB"].ToString() == "1")
            {
                throw new Exception("查询不到身份证号为【" + InObject.ZHENGJIANHM + "】的签约信息!");
            }
            if (InObject.XINGMING.ToString() != existInfo.Items["BINGRENXM"].ToString())
            {
                throw new Exception(string.Format("传入病人姓名[{0}]与签约账户姓名[{1}]不符", InObject.XINGMING, existInfo.Items["BINGRENXM"]));
            }
            OutObject = new ZHIFUBAOBDCX_OUT();
            OutObject.ZHIFUBXYH = existInfo.Items["XIEYIHM"].ToString();
        }
    }
}
