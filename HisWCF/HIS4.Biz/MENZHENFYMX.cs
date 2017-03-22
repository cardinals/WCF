using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using HIS4.Schemas;
using System.Configuration;

namespace HIS4.Biz
{
    public class MENZHENFYMX : IMessage<MENZHENFYMX_IN,MENZHENFYMX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new MENZHENFYMX_OUT();
            string jiuZhenKH = InObject.JIUZHENKH;//就诊卡号
            string jiuZhenKLX = InObject.JIUZHENKLX;//就诊卡类型
            string bingRenLB = InObject.BINGRENLB;//病人类别
            string bingRenXZ = InObject.BINGRENXZ;//病人性质
            string chaXunFS = InObject.CHAXUNFS;//查询方式
            
            string fenYuanDM = InObject.BASEINFO.FENYUANDM;//分院代码
            string MZFYXXKYQHQ = ConfigurationManager.AppSettings["MZFYXXKYQHQ"];//门诊费用明细 是否可以跨院区获取

            string fenYuanCXXX = string.Empty;

            #region 基础入参判断
            //就诊卡号
            if (string.IsNullOrEmpty(jiuZhenKH) && string.IsNullOrEmpty(InObject.BINGRENID))
            {
                throw new Exception("就诊卡号和病人ID不能同时为空！");
            }

            if (string.IsNullOrEmpty(MZFYXXKYQHQ)) {
                MZFYXXKYQHQ = "0";
            }
            if (MZFYXXKYQHQ == "1") {
                fenYuanCXXX = string.Format(" and a.yuanquid = '{0}' ", fenYuanDM);
            }

            ////就诊卡类型
            //if (string.IsNullOrEmpty(jiuZhenKH))
            //{
            //    throw new Exception("就诊卡类型获取失败");
            //}

            ////病人类别
            //if (string.IsNullOrEmpty(bingRenLB))
            //{
            //    throw new Exception("病人类别获取失败");
            //}

            ////病人性质
            //if (string.IsNullOrEmpty(bingRenXZ))
            //{
            //    throw new Exception("病人性质获取失败");
            //}
            #endregion
            DataTable dt=null;
            #region 获取病人信息
            if (string.IsNullOrEmpty(InObject.BINGRENID))
            {
                dt = DBVisitor.ExecuteTable(string.Format("select bingrenid from gy_v_bingrenxx where jiuzhenkh = '{0}' or  yibaokh = '{0}' order by yibaokh asc, xiugaisj desc ", jiuZhenKH));
            }
            else {
                dt = DBVisitor.ExecuteTable(string.Format("select bingrenid from gy_v_bingrenxx where bingrenid ='{0}' order by yibaokh asc, xiugaisj desc ", InObject.BINGRENID));
            }
            string bingRenID = "";
            if (dt != null && dt.Rows.Count <= 0)
            {
                throw new Exception("病人信息获取错误");
            }
            else
            {
                bingRenID = dt.Rows[0]["bingrenid"].ToString();
            }
            #endregion

            #region 获取费用信息
            if (chaXunFS == "1")
            {
                #region 通过处方id和医技id进行检索费用明细
                string chuFangID = InObject.CHUFANGID;
                string yiJiID = InObject.YIJIID;

                if (string.IsNullOrEmpty(chuFangID) && string.IsNullOrEmpty(yiJiID))
                {
                    throw new Exception("检索模式一（通过处方/医技id检索）时，处方id和医技id不能同时为空");
                }

                #region 获取处方数据
                if (!(string.IsNullOrEmpty(chuFangID)))
                {
                    chuFangID = chuFangID.Replace('|', ',');
                    //获取处方明细sql
                    StringBuilder sqlBuf = new StringBuilder();
                    sqlBuf.Append("select RPAD(nvl(a.kongzhisx,'0000'),4,'0') as kongzhisx,a.chufangid chufangxh,d.chufangmxid mingxixh,a.feiyonglb feiyonglx,l.yaopinid xiangmuxh,")
                        .Append("l.chandi xiangmucddm,l.yaopinmc xiangmumc,l.yaopinlx xiangmugl,m.daimamc xiangmuglmc,l.yaopingg xiangmugg,")
                        .Append("l.jixing xiangmujx,l.jiliangdw xiangmudw,l.candimc xiangmucdmc,l.baozhuangliang baozhuangsl,l.baozhuangdw,")
                        .Append("l.zuixiaodw zuixiaojldw,'' danciyl,'' yongliangdw,null meitiancs,d.yongyaots,l.fufangbz danfufbz,")
                        .Append("d.chufangts2 zhongcaoyts,d.jiesuanjia danjia,d.shuliang,d.jiesuanje jine,d.yibaodj,d.yibaodm,d.yibaozfbl,")
                        .Append("d.xianjia xiangmuxj,d.zifeije,d.zilije,d.shenpibh,d.zifeibz,0 teshuyybz,d.yibaoxx yibaoxmfzxx,d.yicijl dancisl,")
                        .Append("d.pinci pinlvsz,a.kaidanks kaidanksdm,a.kaidanksmc kaidanksmc,a.kaidanys kaidanysdm,a.kaidanysxm kaidanysxm,")
                        .Append("d.zifubl,to_char(a.kaidanriq,'yyyy-mm-dd hh24:mm:ss') kaidanrq,l.daguigid yaopindggxh,'' yaopindggcd,null yaopindggsl")
                        .Append("from mz_chufang1 a,gy_feiyonglb b,mz_chufang2 d,gy_yaopincdjg2 l,gy_xiangmulx m")
                        .Append("where b.leibieid(+)=a.feiyonglb and d.chufangid = a.chufangid {2} and a.bingrenid = '{0}' ")
                        .Append("and l.jiageid = d.jiageid and m.daimaid(+) = l.yaopinlx and a.chufangid in ({1})");

                    dt = DBVisitor.ExecuteTable(string.Format(sqlBuf.ToString(), bingRenID, chuFangID, fenYuanCXXX));

                    if (dt.Rows.Count > 0)
                    {
                        #region 费用明细拼装
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var temp = new MENZHENFYXX();
                            temp.CHUFANGLX = "1"; //处方类型
                            temp.CHUFANGXH = dt.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                            temp.MINGXIXH = dt.Rows[i]["MINGXIXH"].ToString(); //明细序号
                            temp.FEIYONGLX = dt.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                            temp.XIANGMUXH = dt.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                            temp.XIANGMUCDDM = dt.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                            temp.XIANGMUMC = dt.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                            temp.XIANGMUGL = dt.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                            temp.XIANGMUGLMC = dt.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                            temp.XIANGMUGG = dt.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                            temp.XIANGMUJX = dt.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                            temp.XIANGMUDW = dt.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                            temp.XIANGMUCDMC = dt.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                            temp.BAOZHUANGSL = dt.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                            temp.BAOZHUANGDW = dt.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                            temp.ZUIXIAOJLDW = dt.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                            temp.DANCIYL = dt.Rows[i]["DANCIYL"].ToString(); //单次用量
                            temp.YONGLIANGDW = dt.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                            temp.MEITIANCS = dt.Rows[i]["MEITIANCS"].ToString(); //每天次数
                            temp.YONGYAOTS = dt.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                            temp.DANFUFBZ = dt.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                            temp.ZHONGCAOYTS = dt.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                            temp.DANJIA = dt.Rows[i]["DANJIA"].ToString(); //单价
                            temp.SHULIANG = dt.Rows[i]["SHULIANG"].ToString(); //数量
                            temp.JINE = dt.Rows[i]["JINE"].ToString(); //金额
                            temp.YIBAODJ = dt.Rows[i]["YIBAODJ"].ToString(); //医保等级
                            temp.YIBAODM = dt.Rows[i]["YIBAODM"].ToString(); //医保代码
                            temp.YIBAOZFBL = dt.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                            temp.XIANGMUXJ = dt.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                            temp.ZIFEIJE = dt.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                            temp.ZILIJE = dt.Rows[i]["ZILIJE"].ToString(); //自理金额
                            temp.SHENGPIBH = dt.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                            temp.ZIFEIBZ = dt.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                            temp.TESHUYYBZ = dt.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                            temp.YIBAOXMFZXX = dt.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                            temp.DANCISL = dt.Rows[i]["DANCISL"].ToString(); //单次数量
                            temp.PINLVSZ = dt.Rows[i]["PINLVSZ"].ToString(); //频率数值
                            temp.KAIDANKSDM = dt.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                            temp.KAIDANKSMC = dt.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                            temp.KAIDANYSDM = dt.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                            temp.KAIDANYSXM = dt.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                            temp.ZIFUBL = dt.Rows[i]["ZIFUBL"].ToString(); //自负比例
                            temp.KAIDANRQ = dt.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                            temp.YAOPINDGGXH = dt.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                            temp.YAOPINDGGCD = dt.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                            temp.YAOPINDGGSL = dt.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                            temp.KONGZHISX = dt.Rows[i]["kongzhisx"].ToString();
                            switch (temp.KONGZHISX) { 
                                case "0100":
                                    temp.KONGZHISX = "02";
                                    temp.KONGZHISXMC = "特病";
                                    break;
                                case "0010":
                                    temp.KONGZHISX = "03";
                                    temp.KONGZHISXMC = "生育";
                                    break;
                                default:
                                    temp.KONGZHISX = "00";
                                    temp.KONGZHISXMC = "普通";
                                    break;
                            }
                            OutObject.FEIYONGMX.Add(temp);
                        }
                        #endregion
                    }
                }
                #endregion

                #region 获取医技数据
                if (!(string.IsNullOrEmpty(yiJiID)))
                {
                    yiJiID.Replace('|', ',');
                    StringBuilder sqlBuf = new StringBuilder();
                    sqlBuf.Append("select nvl(RPAD(nvl(a.kongzhisx,(select RPAD(nvl(cf.kongzhisx, '0000'), 4, '0') From mz_chufang1 cf where cf.chufangid = a.guanlianid)),4,'0'),'0000') as kongzhisx,")
                        .Append("a.yijiid chufangxh,d.yijimxid mingxixh,a.feiyonglb feiyonglx, l.shoufeixmid xiangmuxh,'' xiangmucddm, ")
                        .Append("l.shoufeixmmc xiangmumc,'' xiangmugl,'' XIANGMUGLMC,'' xiangmugg,null xiangmujx,l.jijiadw xiangmudw, ")
                        .Append("'' xiangmucdmc,null baozhuangsl,'' baozhuangdw,'' zuixiaojldw,'' danciyl,'' yongliangdw, ")
                        .Append("null meitiancs,null yongyaots,null danfufbz,null zhongcaoyts, d.jiesuanjia danjia,d.shuliang,d.jiesuanje jine,d.yibaodj, ")
                        .Append("d.yibaodm,d.yibaozfbl,d.xianjia xiangmuxj,d.zifeije,d.zilije,d.shenpibh,d.zifeibz,0 teshuyybz,d.yibaoxx yibaoxmfzxx,d.shuliang dancisl, ")
                        .Append("null pinlvsz,a.kaidanks kaidanksdm,a.kaidanksmc kaidanksmc,a.kaidanys kaidanysdm,a.kaidanysxm kaidanysxm,d.zifubl, ")
                        .Append("to_char(a.kaidanrq,'yyyy-mm-dd hh24:mi:ss')kaidanrq,'' yaopindggxh,'' yaopindggcd,null yaopindggsl ")
                        .Append("from mz_yiji1 a,gy_feiyonglb b,mz_yiji2 d,gy_shoufeixm l ")
                        .Append("where b.leibieid(+)=a.feiyonglb ")
                        .Append("and d.yijiid=a.yijiid and l.shoufeixmid = d.shoufeixm and  a.bingrenid = '{0}'")
                        .Append("and a.yijiid in ({1}) {2} ;");

                    dt = DBVisitor.ExecuteTable(string.Format(sqlBuf.ToString(), bingRenID, chuFangID, fenYuanCXXX));

                    if (dt.Rows.Count > 0)
                    {
                        #region 费用明细拼装
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var temp = new MENZHENFYXX();
                            temp.CHUFANGLX = "0"; //处方类型
                            temp.CHUFANGXH = dt.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                            temp.MINGXIXH = dt.Rows[i]["MINGXIXH"].ToString(); //明细序号
                            temp.FEIYONGLX = dt.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                            temp.XIANGMUXH = dt.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                            temp.XIANGMUCDDM = dt.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                            temp.XIANGMUMC = dt.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                            temp.XIANGMUGL = dt.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                            temp.XIANGMUGLMC = dt.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                            temp.XIANGMUGG = dt.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                            temp.XIANGMUJX = dt.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                            temp.XIANGMUDW = dt.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                            temp.XIANGMUCDMC = dt.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                            temp.BAOZHUANGSL = dt.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                            temp.BAOZHUANGDW = dt.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                            temp.ZUIXIAOJLDW = dt.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                            temp.DANCIYL = dt.Rows[i]["DANCIYL"].ToString(); //单次用量
                            temp.YONGLIANGDW = dt.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                            temp.MEITIANCS = dt.Rows[i]["MEITIANCS"].ToString(); //每天次数
                            temp.YONGYAOTS = dt.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                            temp.DANFUFBZ = dt.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                            temp.ZHONGCAOYTS = dt.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                            temp.DANJIA = dt.Rows[i]["DANJIA"].ToString(); //单价
                            temp.SHULIANG = dt.Rows[i]["SHULIANG"].ToString(); //数量
                            temp.JINE = dt.Rows[i]["JINE"].ToString(); //金额
                            temp.YIBAODJ = dt.Rows[i]["YIBAODJ"].ToString(); //医保等级
                            temp.YIBAODM = dt.Rows[i]["YIBAODM"].ToString(); //医保代码
                            temp.YIBAOZFBL = dt.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                            temp.XIANGMUXJ = dt.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                            temp.ZIFEIJE = dt.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                            temp.ZILIJE = dt.Rows[i]["ZILIJE"].ToString(); //自理金额
                            temp.SHENGPIBH = dt.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                            temp.ZIFEIBZ = dt.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                            temp.TESHUYYBZ = dt.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                            temp.YIBAOXMFZXX = dt.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                            temp.DANCISL = dt.Rows[i]["DANCISL"].ToString(); //单次数量
                            temp.PINLVSZ = dt.Rows[i]["PINLVSZ"].ToString(); //频率数值
                            temp.KAIDANKSDM = dt.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                            temp.KAIDANKSMC = dt.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                            temp.KAIDANYSDM = dt.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                            temp.KAIDANYSXM = dt.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                            temp.ZIFUBL = dt.Rows[i]["ZIFUBL"].ToString(); //自负比例
                            temp.KAIDANRQ = dt.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                            temp.YAOPINDGGXH = dt.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                            temp.YAOPINDGGCD = dt.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                            temp.YAOPINDGGSL = dt.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                            temp.KONGZHISX = dt.Rows[i]["kongzhisx"].ToString();
                            switch (temp.KONGZHISX)
                            {
                                case "0100":
                                    temp.KONGZHISX = "02";
                                    temp.KONGZHISXMC = "特病";
                                    break;
                                case "0010":
                                    temp.KONGZHISX = "03";
                                    temp.KONGZHISXMC = "生育";
                                    break;
                                default:
                                    temp.KONGZHISX = "00";
                                    temp.KONGZHISXMC = "普通";
                                    break;
                            }
                            OutObject.FEIYONGMX.Add(temp);
                        }
                        #endregion
                    }
                }
                #endregion
                #endregion
            }
            else
            {
                #region 普通方式检索费用明细
                #region 获取处方医技有效天数
                //1手工处方2手工医技3电子处方4电子检验检查其他5处置6体检接口7代收挂号诊疗费中间用|分隔；0无穷天
                string youxiaoTs = Unity.GetYouXiaoTs();
                string chuFangYXTS = youxiaoTs.Split('|')[0].ToString();//处方有效天数
                string yiJiYXTS = youxiaoTs.Split('|')[1].ToString();//医技有效天数
                #endregion

                #region 获取处方数据
                StringBuilder sqlBufCF = new StringBuilder();
                sqlBufCF.Append(@"select chufanglx,shoufeibz,jiaoyilx,chongxiaobz,bingrenid,
                                         yuanquid,kongzhisx,chufangxh,mingxixh,feiyonglx,
                                         xiangmuxh,xiangmucddm,xiangmumc,xiangmugl,xiangmuglmc,
                                         xiangmugg,xiangmujx,xiangmudw,xiangmucdmc,baozhuangsl,
                                         baozhuangdw,zuixiaojldw,danciyl,yongliangdw,meitiancs,
                                         yongyaots,danfufbz,zhongcaoyts,danjia,shuliang,
                                         jine,yibaodj,yibaodm,yibaozfbl,xiangmuxj,
                                         zifeije,zilije,shenpibh,zifeibz,teshuyybz,
                                         yibaoxmfzxx,dancisl,pinlvsz,kaidanksdm,kaidanksmc,
                                         kaidanysdm,kaidanysxm,zifubl,to_char(kaidanrq,'yyyy-mm-dd hh24:mi:ss') kaidanrq,yaopindggxh,
                                         yaopindggcd,yaopindggsl 
                                from v_mz_shoufeixx_zzj a where a.shoufeibz=0 and a.jiaoyilx=1 and a.chongxiaobz = 0 and a.chufanglx = 1 and a.bingrenid = '{0}' {2} ");
                    if(!string.IsNullOrEmpty(chuFangYXTS) && chuFangYXTS != "0"){
                        sqlBufCF.Append("and a.KAIDANRQ > sysdate- {1} ");
                    }
                dt = DBVisitor.ExecuteTable(string.Format(sqlBufCF.ToString(), bingRenID, chuFangYXTS, fenYuanCXXX));

                if (dt.Rows.Count > 0)
                {
                    #region 费用明细拼装
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var temp = new MENZHENFYXX();
                        temp.CHUFANGLX = dt.Rows[i]["chufanglx"].ToString(); //处方类型
                        temp.CHUFANGXH = dt.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                        temp.MINGXIXH = dt.Rows[i]["MINGXIXH"].ToString(); //明细序号
                        temp.FEIYONGLX = dt.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                        temp.XIANGMUXH = dt.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                        temp.XIANGMUCDDM = dt.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                        temp.XIANGMUMC = dt.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                        temp.XIANGMUGL = dt.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                        temp.XIANGMUGLMC = dt.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                        temp.XIANGMUGG = dt.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                        temp.XIANGMUJX = dt.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                        temp.XIANGMUDW = dt.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                        temp.XIANGMUCDMC = dt.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                        temp.BAOZHUANGSL = dt.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                        temp.BAOZHUANGDW = dt.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                        temp.ZUIXIAOJLDW = dt.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                        temp.DANCIYL = dt.Rows[i]["DANCIYL"].ToString(); //单次用量
                        temp.YONGLIANGDW = dt.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                        temp.MEITIANCS = dt.Rows[i]["MEITIANCS"].ToString(); //每天次数
                        temp.YONGYAOTS = dt.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                        temp.DANFUFBZ = dt.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                        temp.ZHONGCAOYTS = dt.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                        temp.DANJIA = dt.Rows[i]["DANJIA"].ToString(); //单价
                        temp.SHULIANG = dt.Rows[i]["SHULIANG"].ToString(); //数量
                        temp.JINE = dt.Rows[i]["JINE"].ToString(); //金额
                        temp.YIBAODJ = dt.Rows[i]["YIBAODJ"].ToString(); //医保等级
                        temp.YIBAODM = dt.Rows[i]["YIBAODM"].ToString(); //医保代码
                        temp.YIBAOZFBL = dt.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                        temp.XIANGMUXJ = dt.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                        temp.ZIFEIJE = dt.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                        temp.ZILIJE = dt.Rows[i]["ZILIJE"].ToString(); //自理金额
                        //temp.SHENGPIBH = dt.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                        temp.ZIFEIBZ = dt.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                        temp.TESHUYYBZ = dt.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                        temp.YIBAOXMFZXX = dt.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                        temp.DANCISL = dt.Rows[i]["DANCISL"].ToString(); //单次数量
                        temp.PINLVSZ = dt.Rows[i]["PINLVSZ"].ToString(); //频率数值
                        temp.KAIDANKSDM = dt.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                        temp.KAIDANKSMC = dt.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                        temp.KAIDANYSDM = dt.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                        temp.KAIDANYSXM = dt.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                        temp.ZIFUBL = dt.Rows[i]["ZIFUBL"].ToString(); //自负比例
                        temp.KAIDANRQ = dt.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                        temp.YAOPINDGGXH = dt.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                        temp.YAOPINDGGCD = dt.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                        temp.YAOPINDGGSL = dt.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                        temp.KONGZHISX = dt.Rows[i]["kongzhisx"].ToString();
                        switch (temp.KONGZHISX)
                        {
                            case "0100":
                                temp.KONGZHISX = "02";
                                temp.KONGZHISXMC = "特病";
                                break;
                            case "0010":
                                temp.KONGZHISX = "03";
                                temp.KONGZHISXMC = "生育";
                                break;
                            default:
                                temp.KONGZHISX = "00";
                                temp.KONGZHISXMC = "普通";
                                break;
                        }
                        OutObject.FEIYONGMX.Add(temp);
                    }
                    #endregion
                }
                #endregion

                #region 获取医技数据
                StringBuilder sqlBufYJ = new StringBuilder();
                sqlBufYJ.Append(@"select chufanglx,shoufeibz,jiaoyilx,chongxiaobz,bingrenid,
                                         yuanquid,kongzhisx,chufangxh,mingxixh,feiyonglx,
                                         xiangmuxh,xiangmucddm,xiangmumc,xiangmugl,xiangmuglmc,
                                         xiangmugg,xiangmujx,xiangmudw,xiangmucdmc,baozhuangsl,
                                         baozhuangdw,zuixiaojldw,danciyl,yongliangdw,meitiancs,
                                         yongyaots,danfufbz,zhongcaoyts,danjia,shuliang,
                                         jine,yibaodj,yibaodm,yibaozfbl,xiangmuxj,
                                         zifeije,zilije,shenpibh,zifeibz,teshuyybz,
                                         yibaoxmfzxx,dancisl,pinlvsz,kaidanksdm,kaidanksmc,
                                         kaidanysdm,kaidanysxm,zifubl,to_char(kaidanrq,'yyyy-mm-dd hh24:mi:ss') kaidanrq,yaopindggxh,
                                         yaopindggcd,yaopindggsl 
                                from v_mz_shoufeixx_zzj a where a.shoufeibz=0 and a.jiaoyilx=1 and a.chongxiaobz = 0 and a.chufanglx = 0 and a.bingrenid = '{0}' {2}  ");
                    if(!string.IsNullOrEmpty(yiJiYXTS) && yiJiYXTS != "0"){
                    sqlBufYJ.Append("and a.KAIDANRQ > sysdate-{1} ");
                    }
                    
                dt = DBVisitor.ExecuteTable(string.Format(sqlBufYJ.ToString(), bingRenID, yiJiYXTS, fenYuanCXXX));

                if (dt.Rows.Count > 0)
                {
                    #region 费用明细拼装
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var temp = new MENZHENFYXX();
                        temp.CHUFANGLX = dt.Rows[i]["chufanglx"].ToString(); //处方类型
                        temp.CHUFANGXH = dt.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                        temp.MINGXIXH = dt.Rows[i]["MINGXIXH"].ToString(); //明细序号
                        temp.FEIYONGLX = dt.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                        temp.XIANGMUXH = dt.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                        temp.XIANGMUCDDM = dt.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                        temp.XIANGMUMC = dt.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                        temp.XIANGMUGL = dt.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                        temp.XIANGMUGLMC = dt.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                        temp.XIANGMUGG = dt.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                        temp.XIANGMUJX = dt.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                        temp.XIANGMUDW = dt.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                        temp.XIANGMUCDMC = dt.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                        temp.BAOZHUANGSL = dt.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                        temp.BAOZHUANGDW = dt.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                        temp.ZUIXIAOJLDW = dt.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                        temp.DANCIYL = dt.Rows[i]["DANCIYL"].ToString(); //单次用量
                        temp.YONGLIANGDW = dt.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                        temp.MEITIANCS = dt.Rows[i]["MEITIANCS"].ToString(); //每天次数
                        temp.YONGYAOTS = dt.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                        temp.DANFUFBZ = dt.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                        temp.ZHONGCAOYTS = dt.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                        temp.DANJIA = dt.Rows[i]["DANJIA"].ToString(); //单价
                        temp.SHULIANG = dt.Rows[i]["SHULIANG"].ToString(); //数量
                        temp.JINE = dt.Rows[i]["JINE"].ToString(); //金额
                        temp.YIBAODJ = dt.Rows[i]["YIBAODJ"].ToString(); //医保等级
                        temp.YIBAODM = dt.Rows[i]["YIBAODM"].ToString(); //医保代码
                        temp.YIBAOZFBL = dt.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                        temp.XIANGMUXJ = dt.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                        temp.ZIFEIJE = dt.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                        temp.ZILIJE = dt.Rows[i]["ZILIJE"].ToString(); //自理金额
                        //temp.SHENGPIBH = dt.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                        temp.ZIFEIBZ = dt.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                        temp.TESHUYYBZ = dt.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                        temp.YIBAOXMFZXX = dt.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                        temp.DANCISL = dt.Rows[i]["DANCISL"].ToString(); //单次数量
                        temp.PINLVSZ = dt.Rows[i]["PINLVSZ"].ToString(); //频率数值
                        temp.KAIDANKSDM = dt.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                        temp.KAIDANKSMC = dt.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                        temp.KAIDANYSDM = dt.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                        temp.KAIDANYSXM = dt.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                        temp.ZIFUBL = dt.Rows[i]["ZIFUBL"].ToString(); //自负比例
                        temp.KAIDANRQ = dt.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                        temp.YAOPINDGGXH = dt.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                        temp.YAOPINDGGCD = dt.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                        temp.YAOPINDGGSL = dt.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                        temp.KONGZHISX = dt.Rows[i]["kongzhisx"].ToString();
                        switch (temp.KONGZHISX)
                        {
                            case "0100":
                                temp.KONGZHISX = "02";
                                temp.KONGZHISXMC = "特病";
                                break;
                            case "0010":
                                temp.KONGZHISX = "03";
                                temp.KONGZHISXMC = "生育";
                                break;
                            default:
                                temp.KONGZHISX = "00";
                                temp.KONGZHISXMC = "普通";
                                break;
                        }
                        OutObject.FEIYONGMX.Add(temp);
                    }
                    #endregion
                }
                #endregion
                #endregion
            }

            #region 获取医保自负比例

            #endregion
            if (OutObject.FEIYONGMX != null)
            {
                OutObject.FEIYONGMXTS = OutObject.FEIYONGMX.Count;
            }
            else
            {
                OutObject.FEIYONGMXTS = 0;
            }
            #endregion

            #region 获取疾病信息
            StringBuilder sqlBufJBXX = new StringBuilder(); //疾病信息
            sqlBufJBXX.Append("select jibingid jibingdm,icd10 jibingicd,jibingmc,'' jibingms from gy_jibingdm where jibingid in (  ")
                    .Append("select SUBSTR(a.linchuangzd,0,INSTR(a.linchuangzd,'|',1,1)-1) from zj_jiuzhenxx a ,mz_guahao1 b where a.yuanquid = '{0}' ")
                    .Append("and a.guahaoid = b.guahaoid and a.linchuangzd is not null and a.bingrenid = '{1}' ) ");
            dt = DBVisitor.ExecuteTable(string.Format(sqlBufJBXX.ToString(), fenYuanDM, bingRenID));

            if (dt.Rows.Count <= 0)
            {
                #region 获取默认疾病信息

                //var jbxx = new JIBINGXX();
                //jbxx.JIBINGDM = dt.Rows[i]["JIBINGDM"].ToString();//疾病代码
                //jbxx.JIBINGICD = dt.Rows[i]["JIBINGICD"].ToString();//疾病ICD
                //jbxx.JIBINGMC = dt.Rows[i]["JIBINGMC"].ToString();//疾病名称
                //jbxx.JIBINGMS = dt.Rows[i]["JIBINGMS"].ToString();//疾病描述
                //OutObject.JIBINGMX.Add(jbxx);
                #endregion
            }
            else if (dt.Rows.Count > 0)
            {
                #region 疾病信息拼装
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var jbxx = new JIBINGXX();
                    jbxx.JIBINGDM = dt.Rows[i]["JIBINGDM"].ToString();//疾病代码
                    jbxx.JIBINGICD = dt.Rows[i]["JIBINGICD"].ToString();//疾病ICD
                    jbxx.JIBINGMC = dt.Rows[i]["JIBINGMC"].ToString();//疾病名称
                    jbxx.JIBINGMS = dt.Rows[i]["JIBINGMS"].ToString();//疾病描述
                    OutObject.JIBINGMX.Add(jbxx);
                }
                #endregion
            }
            #endregion

            //医疗类别
            OutObject.YILIAOLB = InObject.YILIAOLB;
        }
    }
}
