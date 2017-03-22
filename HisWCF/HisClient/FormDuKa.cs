using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Web.Hosting;
using SWSoft.Framework;
using HIS1.Biz;

namespace HisClient
{
    public partial class FormDuKa : Form
    {
        public FormDuKa()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var trade = comboBox1.Text;// .SelectedItem.ToString();
            if (string.IsNullOrEmpty(trade))
            {
                trade = "HIS4.Biz.";
            }
            if (checkBox2.Checked)
            {
                try
                {
                    File.WriteAllText(Application.StartupPath + "\\test.log", richTextBox1.Text, Encoding.UTF8);
                    var xmldom = new XmlDocument();
                    xmldom.LoadXml(richTextBox1.Text);
                    using (var channelFactory = new SRR.HisApplayClient())
                    {
                        var tradetype = trade + xmldom.LastChild.Name.Replace("_IN", "");
                        string outxml = "";
                        var out1 = channelFactory.RunService(tradetype, richTextBox1.Text, ref outxml);
                        if (checkBox1.Checked)
                        {
                            richTextBox2.Clear();
                        }
                        richTextBox2.AppendText(outxml);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    File.WriteAllText(Application.StartupPath + "\\test.log", richTextBox1.Text, Encoding.UTF8);
                    var xmldom = new XmlDocument();
                    xmldom.LoadXml(richTextBox1.Text);
                    using (var channelFactory = new SR.HisApplayClient())
                    {
                        var tradetype = trade + xmldom.LastChild.Name.Replace("_IN", "");
                        var outxml = "";
                        var i = channelFactory.RunService(tradetype, richTextBox1.Text, ref outxml);
                        if (checkBox1.Checked)
                        {
                            richTextBox2.Clear();
                        }
                        richTextBox2.AppendText(outxml);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Application.StartupPath + "\\test.log", richTextBox1.Text, Encoding.UTF8);
            var xmldom = new XmlDocument();
            xmldom.LoadXml(richTextBox1.Text);
            using (var channelFactory = new SR.HisApplayClient())
            {
                var strb = new StringBuilder();
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    var tradetype = "HIS1.Biz." + xmldom.LastChild.Name.Replace("_IN", "");
                    var outxml = "";
                    var j = channelFactory.RunService(tradetype, richTextBox1.Text, ref outxml);
                    strb.AppendLine(outxml);
                }
                File.WriteAllText(Application.StartupPath + "\\result.log", strb.ToString(), Encoding.UTF8);
                //Process.Start(Application.StartupPath + "\\result.log");
            }
        }

        private void FormDuKa_Load(object sender, EventArgs e)
        {
            //var ass = Assembly.LoadFile(Application.StartupPath + "\\SWSoft.Caller.dll");
            //var type = ass.GetType("HisClient.Class1");
            //type.GetMethod("Init").Invoke(Activator.CreateInstance(type, null), null);
            //new Class1().Init();

            try
            {

                richTextBox1.Text = File.ReadAllText(Application.StartupPath + "\\test.log", Encoding.UTF8);
            }
            catch (Exception)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var callparam = new BankParam();
            callparam["qingqiubm"] = "27";
            callparam["xitongxh"] = "70";//	系统序号
            callparam["yinhangkh"] = "6227001542720003501";//	银行卡号
            callparam["tuikuanje"] = "4";//	退款金额
            callparam["CaoZuoGH"] = "ZZ999";//	操作工号
            callparam["DengJiXH"] = "378648";//	登记序号
            BankAPI.Call(callparam);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SRR.HisApplayClient SS = new SRR.HisApplayClient();
            var outxml = SS.sendToHis(richTextBox1.Text);
            richTextBox2.Text = outxml;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SR.HisApplayClient SS = new SR.HisApplayClient();
            string key = "";
            string erro = "";
          //  SS.DoBusiness();
          var outxml = SS.Signature("123", ref key, ref erro);

        }
    }
}
