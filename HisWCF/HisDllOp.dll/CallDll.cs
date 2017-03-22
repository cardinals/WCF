using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using MEDI.SIIM.SelfServiceWeb.Entity;
using MEDI.SIIM.SelfServiceWeb;
using System.Data;


namespace HisDllOp.dll
{
    public class CallDll
    {
        /// <summary>
        /// 网络通讯测试
        /// </summary>
        /// <returns></returns>
        public string PingWL()
        {
            string reXML = "<Response>";
            DateTime dt = DateTime.Now;
            try
            {

                WCFServer.PingCall(ref dt);
                reXML += "<ResponseCode>0</ResponseCode>";
                reXML += "<ResponseMsg>网络正常</ResponseMsg>";//应答信息
                reXML += "<SysDateTime>" + dt + "</SysDateTime>";
            }
            catch
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>网络异常或服务平台异常</ResponseMsg>";//应答信息
                reXML += "<SysDateTime>" + dt + "</SysDateTime>";
            }

            reXML += "</Response>";
            return reXML;
        }
        /// <summary>
        /// 获取患者信息接口（居民健康卡、医保）2002
        /// 人员信息
        /// </summary>
        /// <param name="KeShiId"></param>
        public string RENYUANXX(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;
            #endregion

            #region 将长城的入参 转化为服务平台入参
            RENYUANXX_IN renyuanxx = new RENYUANXX_IN();
            renyuanxx.BASEINFO = baseInfo;
            renyuanxx.JIUZHENKLX = 1;//暂定永远是1
            renyuanxx.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();//就诊卡号
            renyuanxx.BINGRENLB = 1;//所有病人都按自费病人处理
            renyuanxx.YILIAOLB = "00";//医疗类别,00-普通
            renyuanxx.JIESUANLB = "02";//结算类别，02-门诊收费
            renyuanxx.JIUZHENRQ = DateTime.Now.Date;
            #endregion
            //和服务平台交易
            RENYUANXX_OUT Out = WCFServer.Call<RENYUANXX_IN, RENYUANXX_OUT>(renyuanxx);
            //判断性别
            var xingbiesb = Unity.XINGBIEFCCFZH(Out.XINGBIE);


            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                if (Out.OUTMSG.ERRNO == "-2")
                {
                    reXML += "<ResponseCode>0</ResponseCode>";
                    reXML += "<ResponseMsg>卡未注册</ResponseMsg>";//应答信息
                    reXML += "<CardStatus>1</CardStatus>";
                    reXML += "<PatientID></PatientID> ";//病人编号
                    reXML += "<PatientType></PatientType> ";//病人类型
                    reXML += "<Name></Name> ";//姓名
                    reXML += "<Sex></Sex> ";//性别     
                    reXML += "<Age></Age> ";//年龄
                    reXML += "<IDCardNo></IDCardNo> ";//与证件类型数据字典对应的证件号
                    reXML += "<IDCardType></IDCardType> ";//证件类型
                    reXML += "<Mobile></Mobile> ";//手机号
                    reXML += "<BankCardNo></BankCardNo> ";//银行卡号，居民健康卡的银行卡号
                    reXML += "<FarmInsurCard></FarmInsurCard> ";//农保卡号，非农保则为空
                }
                else
                {
                    reXML += "<ResponseCode>-1</ResponseCode>";
                    reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";
                }
            }
            /*
        else if (Out.OUTMSG.ERRNO == "-2")
        {
            reXML += "<ResponseCode>-1</ResponseCode>";
            reXML += "<CardStatus>1</CardStatus>";
        }*/
            else
            {
                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg></ResponseMsg>";//应答信息
                //**
                reXML += "<CardStatus>0</CardStatus>";        //卡状态，联众无这个关键字
                //**
                reXML += "<PatientID>" + Out.JIUZHENKH + "</PatientID> ";//病人编号
                reXML += "<PatientType>" + Unity.BingRenLb2(Out.BINGRENLB.ToString()) + "</PatientType> ";//病人类型
                reXML += "<Name>" + Out.XINGMING + "</Name> ";//姓名
                reXML += "<Sex>" + xingbiesb + "</Sex> ";//性别
                DateTime m_Str = DateTime.Parse(Out.CHUSHENGRQ.ToString());
                int m_Y1 = m_Str.Year;
                int m_Y2 = DateTime.Now.Year;
                int m_Age = m_Y2 - m_Y1;
                reXML += "<Age>" + m_Age + "</Age> ";//年龄
                reXML += "<IDCardNo>" + Out.ZHENGJIANHM + "</IDCardNo> ";//与证件类型数据字典对应的证件号
                reXML += "<IDCardType>" + Out.ZHENGJIANLX + "</IDCardType> ";//证件类型
                reXML += "<Mobile>" + Out.LIANXIDH + "</Mobile> ";//手机号
                reXML += "<BankCardNo> </BankCardNo> ";//银行卡号，居民健康卡的银行卡号
                reXML += "<FarmInsurCard>" + Out.YIBAOKH + "</FarmInsurCard> ";//农保卡号，非农保则为空
            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;


        }//获取患者信息接口（居民健康卡、医保）2002---人员信息
        /// <summary>
        ///  居民健康卡 /农信银行卡 建档
        ///  人员注册
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string RENYUANZC(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;
            #endregion

            #region 将长城的入参 转化为服务平台入参
            RENYUANZC_IN renyuanzc = new RENYUANZC_IN();
            renyuanzc.BASEINFO = baseInfo;
            renyuanzc.JIUZHENKH = "1";//暂定永远是1
            renyuanzc.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();//就诊卡号
            renyuanzc.BINGRENLB = "1";//所有病人都按自费病人处理
            renyuanzc.YIBAOKXX = "";//医保卡信息
            renyuanzc.YIBAOKH = "";//医保卡号
            renyuanzc.GERENBH = "";//dt.Rows[0]["TerTranSerNo"].ToString();//个人编号
            renyuanzc.BINGLIBH = "";//病历本号
            renyuanzc.XINGMING = dt.Rows[0]["Name"].ToString();//姓名
            renyuanzc.XINGBIE = Unity.XINGBIEZH(dt.Rows[0]["Sex"].ToString());//性别
            renyuanzc.MINZU = "";//民族
            renyuanzc.CHUSHENGRQ = dt.Rows[0]["Birthday"].ToString();//出生日期
            renyuanzc.ZHENGJIANLX = "";//暂时未找到
            renyuanzc.ZHENGJIANHM = dt.Rows[0]["IDCardNo"].ToString();//未找到
            renyuanzc.DANWEILX = "00";//暂定单位类型无
            renyuanzc.DANWEIBH = "";//单位编号
            renyuanzc.DANWEIMC = "";//单位名称
            renyuanzc.JIATINGZZ = dt.Rows[0]["Address"].ToString();//家庭地址
            renyuanzc.RENYUANLB = "";//人员类别
            renyuanzc.LIANXIDH = dt.Rows[0]["Mobile"].ToString();//联系电话
            renyuanzc.YINHANGKH = "";//银行卡号
            renyuanzc.QIANYUEBZ = "0";//签约标志，0未签约，1签约。不知道是什么
            renyuanzc.YILIAOLB = "00";//医保类型，暂定为 00普通
            renyuanzc.JIESUANLB = "02";//就算类别，暂定为 02门诊收费
            renyuanzc.YIBAOKMM = "";//医保卡密码
            renyuanzc.ZHAOPIAN = "";//照片
            renyuanzc.SHIFOUYK = "1";//是否有卡，暂定为1有卡，传0为无卡
            renyuanzc.BANGDINGYHK = "0";//绑定银行卡。暂定为 0否，传1 为是
            renyuanzc.ZHONGDUANSBXX = "";//终端设备信息

            #endregion
            //和服务平台交易
            RENYUANZC_OUT Out = WCFServer.Call<RENYUANZC_IN, RENYUANZC_OUT>(renyuanzc);

            if (Out.OUTMSG.ERRNO != "0")
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {
                reXML += "<ResponseCode>0</ResponseCode>";
                reXML += "<ResponseMsg>建档成功</ResponseMsg>";
                reXML += "<OpDateTime>" + baseInfo.CAOZUORQ + "</OpDateTime>";
                reXML += "<HosTranSerNo></HosTranSerNo>";
            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;


        }//人员注册
        /// <summary>
        /// 查询挂号科室2201
        /// 挂号科室信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GUAHAOKSXX(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;
            #endregion

            #region 将长城的入参 转化为服务平台入参
            GUAHAOKSXX_IN guahaoksxx = new GUAHAOKSXX_IN();
            guahaoksxx.BASEINFO = baseInfo;//固定base信息
            guahaoksxx.GUAHAOFS = 0;//0.全部，1.挂号，2.预约 
            guahaoksxx.RIQI = null;  //DateTime.Parse("");//DateTime.Parse(dt.Rows[0]["parDeptID"].ToString());//空,返回全部可挂号科室(YYYY-MM-DD)
            guahaoksxx.GUAHAOBC = 0;//0全部 1上午 2 下午
            guahaoksxx.GUAHAOLB = Unity.GUAHAOLBZH(dt.Rows[0]["TypeID"].ToString());//0全部 1普通 2急诊 >2相关专家类别

            #endregion
            //和服务平台交易
            GUAHAOKSXX_OUT Out = WCFServer.Call<GUAHAOKSXX_IN, GUAHAOKSXX_OUT>(guahaoksxx);

            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {

                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg></ResponseMsg>";//应答信息
                //循环
                for (int i = 0; i < Out.KESHIMX.Count; i++)
                {
                    reXML += "<Item>";//科室信息
                    reXML += "<DeptID>" + Out.KESHIMX[i].KESHIDM + "</DeptID>";//科室编码
                    reXML += "<DeptName>" + Out.KESHIMX[i].KESHIMC + "</DeptName>";//科室名称
                    reXML += "</Item>";
                }

            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;


        }//查询挂号科室2201---挂号科室信息
        ///<summary>
        ///查询挂号医生2202
        ///挂号医生信息
        /// </summary> 
        public string GUAHAOYSXX(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;
            #endregion

            #region 将长城的入参 转化为服务平台入参
            GUAHAOYSXX_IN guahaoysxx = new GUAHAOYSXX_IN();
            //挂号医生信息入参
            guahaoysxx.BASEINFO = baseInfo;//固定base信息
            guahaoysxx.GUAHAOFS = 0;//0.全部，1.挂号，2.预约 
            guahaoysxx.RIQI = null;//DateTime.Parse("");//DateTime.Parse(dt.Rows[0]["parDeptID"].ToString());//空,返回全部可挂号科室(YYYY-MM-DD)
            guahaoysxx.GUAHAOBC = 0;//0全部 1上午 2 下午
            guahaoysxx.GUAHAOLB = Unity.GUAHAOLBZH(dt.Rows[0]["TypeID"].ToString());//0普通 1专家 <->  0全部 1普通 2急诊 >2相关专家类别
            guahaoysxx.KESHIDM = dt.Rows[0]["DeptID"].ToString();//科室代码,空显示全部科室的医生

            //挂号号源信息

            #endregion
            //和服务平台交易
            GUAHAOYSXX_OUT Out = WCFServer.Call<GUAHAOYSXX_IN, GUAHAOYSXX_OUT>(guahaoysxx);

            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {

                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg></ResponseMsg>";//应答信息
                reXML += "<Item>";//医生信息
                reXML += "<RowID>" + guahaoysxx.KESHIDM + "/</RowID>";//号源ID
                reXML += "<DoctID></DoctID>";//医生代码
                reXML += "<DoctName>普通</DoctName>";//医生姓名
                reXML += "<RankID></RankID>";//医生级别编码
                reXML += "<RankName></RankName>";//医生级别名称,不确定
                reXML += "<RegFee></RegFee>";//挂号费
                reXML += "<ClinicFee></ClinicFee>";//诊查费
                reXML += "<Specialty></Specialty>";//医生特长
                reXML += "<TimeRegion>全天</TimeRegion>";//号别时段
                reXML += "<RegID></RegID>";//排版序号
                reXML += "<RegRemained></RegRemained>";//剩余号源
                reXML += "</Item>";
                //循环
                for (int i = 0; i < Out.YISHENGMX.Count; i++)
                {
                    var aa = Out.YISHENGMX.Count;
                    reXML += "<Item>";
                    reXML += "<RowID>" + guahaoysxx.KESHIDM + "/" + Out.YISHENGMX[i].YISHENGDM + "</RowID>";//号源ID
                    reXML += "<DoctID>" + Out.YISHENGMX[i].YISHENGDM + "</DoctID>";//医生代码
                    reXML += "<DoctName>" + Out.YISHENGMX[i].YISHENGXM + "</DoctName>";//医生姓名
                    reXML += "<RankID>" + Out.YISHENGMX[i].YISHENGZC + "</RankID>";//医生级别编码
                    reXML += "<RankName>" + Out.YISHENGMX[i].YISHENGTC + "</RankName>";//医生级别名称,不确定
                    reXML += "<RegFee></RegFee>";//挂号费
                    reXML += "<ClinicFee></ClinicFee>";//诊查费
                    reXML += "<Specialty>" + Out.YISHENGMX[i].YISHENGJS + "</Specialty>";//医生特长
                    reXML += "<TimeRegion>全天</TimeRegion>";//号别时段
                    reXML += "<RegID></RegID>";//排版序号
                    reXML += "<RegRemained></RegRemained>";//剩余号源
                    reXML += "</Item>";
                }

            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;


        }//查询挂号医生2202---挂号医生信息
        /// <summary>
        ///挂号预算2203
        ///挂号预结算 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GUAHAOCL(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;
            #endregion

            #region 将长城的入参 转化为服务平台入参
            GUAHAOCL_IN guahaoycl = new GUAHAOCL_IN();
            //科室和医生代码
            string xinxi = dt.Rows[0]["RowID"].ToString();
            string[] xin = xinxi.Split('/');
            //挂号医生信息入参
            guahaoycl.BASEINFO = baseInfo;//固定base信息
            guahaoycl.JIUZHENKLX = 1;//卡类型，暂定是 1
            guahaoycl.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();//就诊卡号
            guahaoycl.BINGRENLB = 1;//所有病人都按自费病人处理
            guahaoycl.BINGRENXZ = "";//病人性质
            guahaoycl.YIBAOKLX = "3";//医保卡类型
            guahaoycl.YIBAOKMM = dt.Rows[0]["SecurityNo"].ToString();//
            guahaoycl.YIBAOKXX = "";//医保卡信息
            guahaoycl.YILIAOLB = "00";//医疗类别，00-普通
            guahaoycl.JIESUANLB = "01";//结算类别，02-门诊收费
            guahaoycl.YIZHOUPBID = "";//一周排班ID
            guahaoycl.DANGTIANPBID = "";//当天排班ID
            guahaoycl.RIQI = DateTime.Now.Date;//日期
            guahaoycl.GUAHAOBC = 1;//挂号班次，1上午，2下午
            guahaoycl.GUAHAOLB = 1;//挂号类别，暂定为 1普通
            guahaoycl.KESHIDM = xin[0];   //科室代码
            guahaoycl.YISHENGDM = xin[1];  //医生代码
            guahaoycl.GUAHAOXH = "0";   //挂号序号，传0由HIS分配
            guahaoycl.GUAHAOID = "0";   //预处理传0
            guahaoycl.DAISHOUFY = 1;//代收费用，0不代收，1代收
            guahaoycl.YUYUELY = "";//暂时不知是什么
            guahaoycl.BINGLIBH = "";//病历本号

            #endregion
            //和服务平台交易
            GUAHAOCL_OUT Out = WCFServer.Call<GUAHAOCL_IN, GUAHAOCL_OUT>(guahaoycl);

            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {

                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg></ResponseMsg>";//应答信息
                reXML += "<SumFee>" + Out.JIESUANJG.FEIYONGZE + "</SumFee>";//费用总额
                reXML += "<SociatyFee></SociatyFee>";//社保金额，暂时为空
                reXML += "<PatFee>" + Out.JIESUANJG.FEIYONGZE + "</PatFee>";//自费金额;
                reXML += "<RegFee></RegFee>";//挂号费，空
                reXML += "<ClinicFee>" + Out.JIESUANJG.FEIYONGZE + "</ClinicFee>";//诊查费，空
                reXML += "<HosTranSerNo>" + Out.GUAHAOID + "</HosTranSerNo>";//医院流水号。暂定为挂号ID
                reXML += "<CalInfo></CalInfo>";//预算信息。暂时为空
                reXML += "<ReceiptInfo></ReceiptInfo>";//医保打印信息，暂时为空

            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;
        }//挂号预处理2203----挂号预结算
        /// <summary>
        /// 挂号退号
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GUAHAOTH(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));

            BASEINFO baseInfo = new BASEINFO();
           // baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
           // baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;

            #endregion
            #region 将长城的入参 转化为服务平台入参
            GUAHAOTH_IN guahaoth = new GUAHAOTH_IN();
            guahaoth.JIUZHENKLX = 1;//就诊卡类型
            guahaoth.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();//就诊卡号
            guahaoth.XINGMING = "";
            guahaoth.GUAHAOID = dt.Rows[0]["HosTranSerNo"].ToString();//挂号ID
            guahaoth.TUIHAOLX = 1;//退号类型

            #endregion
            //和服务平台交易

            GUAHAOTH_OUT Out = WCFServer.Call<GUAHAOTH_IN, GUAHAOTH_OUT>(guahaoth);
            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {

                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg>退号成功</ResponseMsg>";//应答信息


            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;

        }//挂号退号22031

        public string MENZHENCFJL(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));

            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;

            #endregion

            #region 将长城的入参 转化为服务平台入参
            MENZHENCFJL_IN menzhencfjl = new MENZHENCFJL_IN();
            menzhencfjl.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();//就诊卡号
            menzhencfjl.YILIAOLB = "00";//暂定为00，普通

            #endregion
            //和服务平台交易

            MENZHENCFJL_OUT Out = WCFServer.Call<MENZHENCFJL_IN, MENZHENCFJL_OUT>(menzhencfjl);
            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {
                //var bb = Out.CHUFANGJL.Count;
                var aa = Out.FEIYONGMXTS;
                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg></ResponseMsg>";//应答信息

                for (int j = 0; j < Out.FEIYONGMXTS; j++)
                {
                    reXML += "<Item>";
                    reXML += "<PressID>" + Out.CHUFANGJL[j].CHUFANGID + "</PressID>";
                    reXML += "<PressTpye>" + Out.CHUFANGJL[j].CHUFANGLB + "</PressTpye>";
                    reXML += "<PressDate>" + Out.CHUFANGJL[j].CHUFANGRQ + "</PressDate>";
                    reXML += "<PressName>" + Out.CHUFANGJL[j].CHUFANGMC + "<PressName>";
                    reXML += "<TotalFee><TotalFee>";
                    reXML += "<DeptName>" + Out.CHUFANGJL[j].JIUZHENKS + "</DeptName>";
                    reXML += "<Doctor>" + Out.CHUFANGJL[j].JIUZHENYS + "</Doctor>";
                    reXML += "</Item>";
                }

            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;

        }

        public string MENZHENFYMX(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));

            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;

            #endregion

            #region 将长城的入参 转化为服务平台入参
            MENZHENFYMX_IN menzhenfymx = new MENZHENFYMX_IN();
            menzhenfymx.JIUZHENKLX = "1"; //就诊卡类型
            menzhenfymx.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();
            menzhenfymx.BINGRENLB = "1";
            menzhenfymx.YIBAOKLX = "3";
            menzhenfymx.YILIAOLB = "00";
            menzhenfymx.JIESUANLB = "02";
            #endregion

            MENZHENFYMX_OUT Out = WCFServer.Call<MENZHENFYMX_IN, MENZHENFYMX_OUT>(menzhenfymx);
            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {

                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg>退号成功</ResponseMsg>";//应答信息


            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;

        }//f费用明细---查询处方明细2302


        

        /// <summary>
        ///缴费预结算2303
        ///门诊预结算
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /*
         public string MENZHENYJS(DataTable dt)
         {
             string reXML = "<Response>";
             #region 固定 base信息
             dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
             BASEINFO baseInfo = new BASEINFO();
             baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
             baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
             baseInfo.CAOZUORQ = DateTime.Now;
             baseInfo.XITONGBS = 0;
             baseInfo.FENYUANDM = 0;
             #endregion

             #region 将长城的入参 转化为服务平台入参
             GUAHAOYCL_IN guahaoycl = new GUAHAOYCL_IN();
             //挂号医生信息入参
             guahaoycl.BASEINFO = baseInfo;//固定base信息
             guahaoycl.JIUZHENKLX = 1;//卡类型，暂定是 1
             guahaoycl.JIUZHENKH = dt.Rows[0]["CardNo"].ToString();//就诊卡号
             guahaoycl.BINGRENLB = 1;//所有病人都按自费病人处理
             guahaoycl.BINGRENXZ = "";//
             guahaoycl.YIBAOKLX = "3";//
             guahaoycl.YIBAOKMM = dt.Rows[0]["SecurityNo"].ToString();//
             guahaoycl.YIBAOKXX = "";//
             guahaoycl.YILIAOLB = "00";//医疗类别，00-普通
             guahaoycl.JIESUANLB = "02";//结算类别，02-门诊收费
             guahaoycl.YIZHOUPBID = "";
             guahaoycl.DANGTIANPBID = "";
             guahaoycl.RIQI = DateTime.Now.Date;
             guahaoycl.GUAHAOBC = Int32.Parse(dt.Rows[0]["RowID"].ToString());
             guahaoycl.GUAHAOLB = 1;
             guahaoycl.KESHIDM = "";//科室代码
             guahaoycl.YISHENGDM = "";//医生代码
             guahaoycl.GUAHAOXH = "0";
             guahaoycl.GUAHAOID = "0";
             guahaoycl.DAISHOUFY = 1;
             guahaoycl.YUYUELY = "";
             guahaoycl.BINGLIBH = "";

             #endregion
             //和服务平台交易
             GUAHAOYCL_OUT Out = WCFServer.Call<GUAHAOYCL_IN, GUAHAOYCL_OUT>(guahaoycl);

             if (Out.OUTMSG.ERRNO != "0")//交易错误
             {
                 reXML += "<ResponseCode>-1</ResponseCode>";
                 reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

             }
             else
             {

                 reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                 reXML += "<ResponseMsg></ResponseMsg>";//应答信息
                 reXML += "<SumFee>" + Out.JIESUANJG.FEIYONGZE + "</SumFee>";//费用总额
                 reXML += "<SociatyFee></SociatyFee>";//社保金额，暂时为空
                 reXML += "<PatFee>" + Out.JIESUANJG.ZIFEIJE + "</PatFee>";//自费金额;
                 reXML += "<RegFee></RegFee>";//挂号费，空
                 reXML += "<ClinicFee></ClinicFee>";//诊查费，空
                 reXML += "<HosTranSerNo>" + Out.GUAHAOID + "<HosTranSerNo>";//医院流水号。暂定为挂号ID
                 reXML += "<CalInfo><CalInfo>";//预算信息。暂时为空
                 reXML += "<ReceiptInfo></ReceiptInfo>";//医保打印信息，暂时为空

             }

             //将服务平台出差转化为长城需要的出参
             reXML += "</Response>";
             return reXML;


         }
         */
        /*
        public string FEIYONGCFCS(DataTable dt)
        {
            string reXML = "<Response>";
            #region 固定 base信息
            dynamic bentity = Activator.CreateInstance(Type.GetType("MEDI.SIIM.SelfServiceWeb.Entity.BASEINFO"));
            BASEINFO baseInfo = new BASEINFO();
            baseInfo.CAOZUOYDM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUOYXM = dt.Rows[0]["UserID"].ToString();
            baseInfo.CAOZUORQ = DateTime.Now;
            baseInfo.XITONGBS = 0;
            baseInfo.FENYUANDM = 0;
            #endregion

            #region 将长城的入参 转化为服务平台入参
            YIYUANCFCS_IN yiyuancfcs = new YIYUANCFCS_IN();
            string xinxi = dt.Rows[0]["RowID"].ToString();
            string[] xin = xinxi.Split('/');
        
            

            #endregion
            //和服务平台交易
            GUAHAOYCL_OUT Out = WCFServer.Call<GUAHAOYCL_IN, GUAHAOYCL_OUT>(guahaoycl);

            if (Out.OUTMSG.ERRNO != "0")//交易错误
            {
                reXML += "<ResponseCode>-1</ResponseCode>";
                reXML += "<ResponseMsg>" + Out.OUTMSG.ERRMSG + "</ResponseMsg>";

            }
            else
            {

                reXML += "<ResponseCode>0</ResponseCode>";//交易结果
                reXML += "<ResponseMsg></ResponseMsg>";//应答信息
                reXML += "<SumFee>" + Out.JIESUANJG.FEIYONGZE + "</SumFee>";//费用总额
                reXML += "<SociatyFee></SociatyFee>";//社保金额，暂时为空
                reXML += "<PatFee>" + Out.JIESUANJG.FEIYONGZE + "</PatFee>";//自费金额;
                reXML += "<RegFee>" + Out.JIESUANJG.FEIYONGZE + "</RegFee>";//挂号费，空
                reXML += "<ClinicFee></ClinicFee>";//诊查费，空
                reXML += "<HosTranSerNo>" + Out.GUAHAOID + "</HosTranSerNo>";//医院流水号。暂定为挂号ID
                reXML += "<CalInfo></CalInfo>";//预算信息。暂时为空
                reXML += "<ReceiptInfo></ReceiptInfo>";//医保打印信息，暂时为空

            }

            //将服务平台出差转化为长城需要的出参
            reXML += "</Response>";
            return reXML;


        }*/
    }

}
