using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;

namespace SWSoft.Framework
{
    public class Img
    {
        /// <summary>
        /// 绘制一幅密保卡图片
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="cdkey">序列号</param>
        /// <param name="keys">矩阵项</param>
        /// <param name="xlang">列数</param>
        /// <param name="ylang">行数</param>
        public static Bitmap Create(int width, int height, string cdkey, List<byte> keys, int xlang, int ylang)
        {
            Bitmap Img = new Bitmap(width, height);
            Graphics g = null;
            MemoryStream ms = null;
            g = Graphics.FromImage(Img);
            g.Clear(Color.White);
            Font f = new Font("宋体", 10, FontStyle.Regular);
            Font f1 = new Font("仿宋体", 11, FontStyle.Regular);
            Pen pen = new Pen(Color.FromArgb(71, 152, 211), 2);
            SolidBrush s = new SolidBrush(Color.FromArgb(71, 152, 211));
            g.DrawRectangle(pen, 1, 1, width - 2, height - 2);
            g.DrawString("序列号:", f, s, 6, 6);
            g.DrawString(cdkey, f1, new SolidBrush(Color.FromArgb(218, 58, 158)), 60, 3);
            int x = 46;
            int y = 48;
            //横向颜色变换
            for (int i = 0; i < xlang; i++)
            {
                if (i % 2 == 0)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(230, 230, 230)), 25, y, width - 23, 21);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(250, 228, 228)), 25, y, width - 23, 21);
                }
                y += 23;
            }
            x = 24;
            y = 24;
            //横线
            Font font = new Font("Arial", 12, FontStyle.Bold);
            for (int i = 0; i < xlang + 1; i++)
            {
                g.DrawLine(pen, new Point(0, x), new Point(width, x));
                x += 23;
            }
            //竖线
            for (int i = 0; i < ylang + 1; i++)
            {
                g.DrawLine(pen, new Point(y, 23), new Point(y, height));
                y += 23;
            }
            x = 0; y = 46;
            //列标题
            for (int i = 65; i < 65 + xlang; i++)
            {
                g.DrawString(Convert.ToChar(i).ToString(), font, s, 4, y + 4);
                y += 23;
            }
            x = 23;
            y = 0;
            //行标题
            for (int i = 1; i < xlang + 1; i++)
            {
                g.DrawString(i.ToString(), font, s, x + 4, 27);
                x += 23;
            }
            FillCode(g, keys);
            ms = new MemoryStream();
            Img.Save(ms, ImageFormat.Jpeg);
            return Img;
        }

        private static void FillCode(Graphics grap, List<byte> list)
        {
            Font font = new Font("仿宋体", 11, FontStyle.Regular);
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grap.DrawString(string.Format("{0:X2}", list[count]), font, new SolidBrush(Color.Black), j * 23 + 25, i * 23 + 48);
                    count++;
                }
            }
        }
    }
}
