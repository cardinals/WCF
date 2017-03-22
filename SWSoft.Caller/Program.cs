using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWSoft.Framework;
using System.IO;
using System.Diagnostics;

namespace SWSoft
{
    class Program
    {
        static void Main(string[] args)
        {
            var pdi = new Privodi();
            using (var sw = new StreamWriter("data.html"))
            {
                sw.WriteLine("<html><head><title></title><body>");
                sw.WriteLine(pdi.Call());
                sw.WriteLine("</body></html>");
            }
            Process.Start("data.html");
        }
    }

    class Privodi : DBVisitor<Entry>
    {
        public string Call()
        {
            return ExecuteJson("select * from userlist");
        }
    }
}
