using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HIS1.Schemas;
using SWSoft.Framework;
using HIS1.Biz;

namespace HisClient
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();
        }
        RENYUANXX_OUT renyout;
        MENZHENFYMX_OUT mxxx;
        private void txtJZKH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                using (var channelFactory = new SR.HisApplayClient())
                {
                    var tradetype = "HIS1.Biz.RENYUANXX";
                    var renyuanxx = new HIS1.Schemas.RENYUANXX_IN();
                    renyuanxx.BASEINFO = GetBASEINFO();
                    renyuanxx.JIUZHENKH = txtJZKH.Text;//就诊卡号
                    renyuanxx.BINGRENLB = comboBox1.Text;//病人类别
                    renyuanxx.YIBAOKLX = "";//医保卡类型
                    renyuanxx.YIBAOKMM = "";//医保卡密码
                    renyuanxx.YIBAOKXX = "";//医保卡信息
                    renyuanxx.YILIAOLB = "";//医疗类别
                    renyuanxx.JIESUANLB = "";//结算类别
                    renyuanxx.JIUZHENRQ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//就诊日期
                    switch (comboBox1.Text)
                    {
                        case "15": break;
                        case "16":
                            renyuanxx.YIBAOKLX = "3";
                            renyuanxx.YIBAOKXX = txt102kaxinxi.Text;
                            break;
                        case "55": break;
                        case "56": break;
                        default:
                            break;
                    }
                    var outxml = channelFactory.RunService(tradetype, MessageParse.GetXml(renyuanxx));
                    renyout = MessageParse.ToXmlObject<RENYUANXX_OUT>(outxml);
                    if (renyout.OUTMSG.ERRNO != "0")
                    {
                        MessageBox.Show(renyout.OUTMSG.ERRMSG);
                    }
                    else
                    {
                        textBox1.Text = renyout.JIUZHENKH;//	就诊卡号
                        textBox2.Text = renyout.BINGRENLB;//	病人类别
                        textBox3.Text = renyout.BINGRENXZ;//	病人性质
                        textBox4.Text = renyout.YIBAOKH;//	医保卡号
                        textBox5.Text = renyout.GERENBH;//	个人编号
                        textBox6.Text = renyout.BINGLIBH;//	病历本号
                        textBox7.Text = renyout.XINGMING;//	姓名
                        textBox8.Text = renyout.XINGBIE == "1" ? "男" : "女";//	性别
                        textBox9.Text = renyout.MINZU;//	民族
                        textBox10.Text = renyout.CHUSHENGRQ;//	出生日期
                        textBox11.Text = renyout.ZHENGJIANLX;//	证件类型
                        textBox12.Text = renyout.ZHENGJIANHM;//	证件号码
                        textBox13.Text = renyout.DANWEILX;//	单位类型
                        textBox14.Text = renyout.DANWEIBH;//	单位编号
                        textBox15.Text = renyout.DANWEIMC;//	单位名称
                        textBox16.Text = renyout.JIATINGZZ;//	家庭地址
                        textBox17.Text = renyout.RENYUANLB;//	人员类别
                        textBox18.Text = renyout.DANNIANZHYE;//	当年帐户余额
                        textBox19.Text = renyout.LINIANZHYE;//	历年帐户余额
                        textBox20.Text = renyout.TESHUBZBZ;//	特殊病种标志
                        textBox21.Text = renyout.TESHUBZSPBH;//	特殊病种审批编号
                        textBox22.Text = renyout.YIBAOBRXX;//	医保病人信息
                        textBox23.Text = renyout.TISHIXX;//	提示信息
                        textBox24.Text = renyout.DAIYULB;//	待遇类别
                        textBox25.Text = renyout.CANBAOXZDM;//	参保行政代码
                        textBox26.Text = renyout.TESHUDYLB;//	特殊待遇类别
                        foreach (var item in renyout.TESHUBZMX)
                        {
                            textBox26.Text += item.JIBINGICD + "=" + item.JIBINGMC + "|";
                        }
                        textBox27.Text = renyout.HISBRXX;
                    }
                }
            }
        }

        public BASEINFO GetBASEINFO()
        {
            var baseinfo = new BASEINFO();
            baseinfo.CAOZUOYDM = txtczgh.Text;//操作员代码
            baseinfo.CAOZUOYXM = txtczxm.Text;//操作员姓名
            baseinfo.CAOZUORQ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作日期
            baseinfo.XITONGBS = "TESTDemo";//系统标识
            baseinfo.FENYUANDM = "0";//分院代码
            baseinfo.ZHONGDUANJBH = "0";//	终端机编号
            baseinfo.ZHONGDUANLSH = Guid.NewGuid().ToString("N");
            return baseinfo;
        }

        private void btnfymx_Click(object sender, EventArgs e)
        {
            if (renyout == null)
            {
                MessageBox.Show("请先取病人信息");
                return;
            }
            var fymx = new MENZHENFYMX_IN();
            fymx.BASEINFO = GetBASEINFO();
            fymx.JIUZHENKH = txtJZKH.Text;//就诊卡号
            fymx.BINGRENLB = comboBox1.Text;//病人类别
            fymx.BINGRENXZ = renyout.BINGRENXZ;//病人性质
            fymx.YIBAOKLX = "3";//医保卡类型
            fymx.YIBAOKMM = "";//医保卡密码
            fymx.YIBAOKXX = "";//医保卡信息
            fymx.YIBAOBRXX = "";//医保病人信息
            fymx.YILIAOLB = "";//医疗类别
            fymx.JIESUANLB = "";//结算类别
            fymx.HISBRXX = renyout.HISBRXX;

            switch (comboBox1.Text)
            {
                case "15": break;
                case "16":
                    fymx.JIUZHENKH = renyout.YIBAOKH;
                    fymx.YIBAOKLX = "3";
                    fymx.YIBAOKXX = txt102kaxinxi.Text;
                    break;
                case "55": break;
                case "56": break;
                default:
                    break;
            }

            using (var channelFactory = new SR.HisApplayClient())
            {
                mxxx = MessageParse.ToXmlObject<MENZHENFYMX_OUT>(channelFactory.RunService("His1.Biz.MENZHENFYMX", MessageParse.GetXml(fymx)));
                if (mxxx.OUTMSG.ERRNO == "-1")
                {
                    MessageBox.Show(mxxx.OUTMSG.ERRMSG);
                }
                else
                {
                    var table = new DataTable();
                    foreach (var item in mxxx.FEIYONGMX)
                    {
                        table.Columns.Add("RowNum");
                        foreach (var col in item.GetType().GetProperties())
                        {
                            table.Columns.Add(col.Name);
                        }
                        int i = 0;
                        foreach (var mx in mxxx.FEIYONGMX)
                        {
                            i++;
                            var newrow = table.NewRow();
                            newrow["RowNum"] = i.ToString();
                            foreach (var mxitem in mx.GetType().GetProperties())
                            {
                                newrow[mxitem.Name] = mxitem.GetValue(mx, null);
                            }
                            table.Rows.Add(newrow);
                        }
                        break;
                    }
                    dataGridView1.DataSource = table;
                    MessageBox.Show("共取到明细条数: " + mxxx.FEIYONGMXTS);
                    tabControl1.SelectedTab = tabPage2;
                }
            }
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }
        MENZHENYJS_IN mzyjsin;
        MENZHENYJS_OUT yjsout;
        private void button1_Click(object sender, EventArgs e)
        {
            if (mxxx == null)
            {
                MessageBox.Show("请先取费用明细");
                return;
            }
            mzyjsin = new MENZHENYJS_IN();
            mzyjsin.BASEINFO = GetBASEINFO();
            mzyjsin.JIUZHENKH = renyout.JIUZHENKH;//就诊卡号
            mzyjsin.BINGRENLB = renyout.BINGRENLB;//病人类别
            mzyjsin.BINGRENXZ = renyout.BINGRENXZ;//病人性质
            mzyjsin.YIBAOKLX = "3";//医保卡类型
            mzyjsin.YIBAOKMM = "";//医保卡密码
            mzyjsin.YIBAOKXX = "";//医保卡信息
            mzyjsin.YIBAOBRXX = renyout.YIBAOBRXX;//医保病人信息
            mzyjsin.YILIAOLB = "00";//医疗类别
            mzyjsin.JIESUANLB = "02";//结算类别
            mzyjsin.JIBINGMX = mxxx.JIBINGMX;//疾病明细信息
            mzyjsin.FEIYONGMXTS = mxxx.FEIYONGMXTS;//费用明细条数
            mzyjsin.FEIYONGMX = mxxx.FEIYONGMX;//费用明细
            switch (comboBox1.Text)
            {
                case "15": break;
                case "16":
                    mzyjsin.JIUZHENKH = renyout.YIBAOKH;
                    mzyjsin.YIBAOKLX = "3";
                    mzyjsin.YIBAOKXX = txt102kaxinxi.Text;
                    mzyjsin.YIBAOBRXX = renyout.YIBAOBRXX;
                    mzyjsin.HISBRXX = renyout.HISBRXX;
                    break;
                case "55": break;
                case "56": break;
                default:
                    break;
            }
            mzyjsin.HISBRXX = renyout.HISBRXX; using (var channelFactory = new SR.HisApplayClient())
            {
                yjsout = MessageParse.ToXmlObject<MENZHENYJS_OUT>(channelFactory.RunService("His1.Biz.MENZHENYJS", MessageParse.GetXml(mzyjsin)));
                if (yjsout.OUTMSG.ERRNO == "-1")
                {
                    MessageBox.Show(yjsout.OUTMSG.ERRMSG);
                }
                else
                {
                    txtFEIYONGZE.Text = yjsout.JIESUANJG.FEIYONGZE;//	费用总额
                    txtZILIJE.Text = yjsout.JIESUANJG.ZILIJE;//自理金额
                    txtZIFEIJE.Text = yjsout.JIESUANJG.ZIFEIJE;//自费金额
                    txtZIFUJE.Text = yjsout.JIESUANJG.ZIFUJE;//自负金额
                    txtYIYUANCDJE.Text = yjsout.JIESUANJG.YIYUANCDJE;//医院承担金额
                    txtBAOXIAOJE.Text = yjsout.JIESUANJG.BAOXIAOJE;//报销金额
                    txtXIANJINZF.Text = yjsout.JIESUANJG.XIANJINZF;//现金支付
                    txtDONGJIEJE.Text = yjsout.JIESUANJG.DONGJIEJE;//冻结金额
                    txtYOUHUIJE.Text = yjsout.JIESUANJG.YOUHUIJE;//优惠金额
                }
            }
        }

        private void btnjs_Click(object sender, EventArgs e)
        {
            if (mxxx == null)
            {
                MessageBox.Show("请先取费用明细");
                return;
            }
            var mzjsin = new MENZHENJS_IN();
            mzjsin.BASEINFO = GetBASEINFO();
            mzjsin.JIESUANID = yjsout.JIESUANID;
            mzjsin.JIUZHENKH = renyout.JIUZHENKH;//就诊卡号
            mzjsin.BINGRENLB = renyout.BINGRENLB;//病人类别
            mzjsin.BINGRENXZ = renyout.BINGRENXZ;//病人性质
            mzjsin.YIBAOKLX = "3";//医保卡类型
            mzjsin.YIBAOKMM = "";//医保卡密码
            mzjsin.YIBAOKXX = "";//医保卡信息
            mzjsin.YIBAOBRXX = renyout.YIBAOBRXX;//医保病人信息
            mzjsin.YILIAOLB = "00";//医疗类别
            mzjsin.JIESUANLB = "02";//结算类别
            mzjsin.JIBINGMX = mxxx.JIBINGMX;//疾病明细信息
            mzjsin.FEIYONGMXTS = mxxx.FEIYONGMXTS;//费用明细条数
            mzjsin.FEIYONGMX = mxxx.FEIYONGMX;//费用明细
            mzjsin.HISBRXX = renyout.HISBRXX; using (var channelFactory = new SR.HisApplayClient())
            {
                var jieguo = MessageParse.ToXmlObject<MENZHENJS_OUT>(channelFactory.RunService("His1.Biz.MENZHENJS", MessageParse.GetXml(mzjsin)));
                if (jieguo.OUTMSG.ERRNO == "-1")
                {
                    MessageBox.Show(jieguo.OUTMSG.ERRMSG);
                }
                else
                {
                    if (mzjsin.BINGRENLB == "1")
                    {
                        txtFEIYONGZE.Text = jieguo.JIESUANJG.FEIYONGZE;//	费用总额
                        txtZILIJE.Text = jieguo.JIESUANJG.ZILIJE;//自理金额
                        txtZIFEIJE.Text = jieguo.JIESUANJG.ZIFEIJE;//自费金额
                        txtZIFUJE.Text = jieguo.JIESUANJG.ZIFUJE;//自负金额
                        txtYIYUANCDJE.Text = jieguo.JIESUANJG.YIYUANCDJE;//医院承担金额
                        txtBAOXIAOJE.Text = jieguo.JIESUANJG.BAOXIAOJE;//报销金额
                        txtXIANJINZF.Text = jieguo.JIESUANJG.XIANJINZF;//现金支付
                        txtDONGJIEJE.Text = jieguo.JIESUANJG.DONGJIEJE;//冻结金额
                        txtYOUHUIJE.Text = jieguo.JIESUANJG.YOUHUIJE;//优惠金额

                    }
                }
            }
        }

        private void FormDemo_Load(object sender, EventArgs e)
        {
            var proc = SqlLoad.GetProcedure("T00018");
            proc["Prm_Jzkh"] = "8987";
            proc["Prm_AppCode"] = 0;
            proc["Prm_OutData"] = string.Empty.PadRight(1000);
            //DBVisitor.ExecuteProcedure(proc);
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("15");
            comboBox1.Items.Add("16");
            comboBox1.Items.Add("55");
            comboBox1.Items.Add("56");
            comboBox1.SelectedIndex = 0;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
