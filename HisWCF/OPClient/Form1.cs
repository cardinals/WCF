using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HisDllOp;

namespace OPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.textBox1.Text = "<Request>" +
"<FunCode> 2002</FunCode>                " +
"<CardNo>0001234567 </CardNo>            " +
"<CardType></CardType>                   " +
"<SecurityNo></SecurityNo>               " +
"<UserID> 810001</UserID>                " +
"</Request>";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HisDllOp.dll.HisDllOp opc = new HisDllOp.dll.HisDllOp();
            string outstring = "";
            string instring = this.textBox1.Text.ToString();
            opc.OpHis(instring, ref outstring);
            this.textBox2.Text = outstring;

        }
    }
}
