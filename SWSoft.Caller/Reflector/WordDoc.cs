using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sw.AutoCode.Bll
{
    public class WordDoc
    {
        //public void CreateWordFile(DataBase data)
        //{
        //    Object Nothing = System.Reflection.Missing.Value;

        //    //创建Word文档
        //    Application wordApp = new ApplicationClass();
        //    Document wordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        //    //添加页眉
        //    wordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
        //    wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
        //    wordApp.ActiveWindow.ActivePane.Selection.InsertAfter("数据库字典");
        //    wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;//设置右对齐
        //    wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;//跳出页眉设置

        //    wordApp.Selection.ParagraphFormat.LineSpacing = 15f;//设置文档的行间距
        //    //开始添加表格
        //    foreach (Sw.AutoCode.Model.Table table in data.Tables.Values)
        //    {
        //        //移动焦点并换行
        //        object count = 14;
        //        object WdLine = Microsoft.Office.Interop.Word.WdUnits.wdLine;//换一行;
        //        wordApp.Selection.MoveDown(ref WdLine, ref count, ref Nothing);//移动焦点
        //        wordApp.Selection.TypeParagraph();//插入段落
        //        //文档中创建表格
        //        Microsoft.Office.Interop.Word.Table newTable = wordDoc.Tables.Add(wordApp.Selection.Range, table.Columns.Values.Count + 2, 5, ref Nothing, ref Nothing);

        //        //设置表格边框样式
        //        newTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
        //        newTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleNone;
        //        //设置列的宽度
        //        //newTable.Columns[1].Width = 100f;
        //        //newTable.Columns[2].Width = 100f;
        //        //newTable.Columns[3].Width = 65f;
        //        //newTable.Columns[4].Width = 65f;
        //        //newTable.Columns[5].Width = 150f;

        //        //填充表格内容
        //        newTable.Cell(1, 1).Range.Text = string.Format("{0}（{1}表）", table.Name, table.Description);
        //        newTable.Cell(1, 1).Range.Bold = 2;//设置单元格中字体为粗体
        //        newTable.Cell(1, 1).Range.Font.Color = WdColor.wdColorDarkRed;
        //        //合并单元格
        //        newTable.Cell(1, 1).Merge(newTable.Cell(1, 5));
        //        wordApp.Selection.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;//垂直居中
        //        wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;//水平居中

        //        newTable.Cell(2, 1).Range.Text = "列名";
        //        newTable.Cell(2, 2).Range.Text = "数据类型";
        //        newTable.Cell(2, 3).Range.Text = "最大长度";
        //        newTable.Cell(2, 4).Range.Text = "是否为空";
        //        newTable.Cell(2, 5).Range.Text = "说明";
        //        for (int i = 1; i <= 5; i++)
        //        {
        //            newTable.Cell(2, i).Range.Bold = 2;
        //            newTable.Cell(2, i).Range.Font.Color = WdColor.wdColorBlue;
        //        }
        //        int row = 3;
        //        foreach (Sw.AutoCode.Model.Column column in table.Columns.Values)
        //        {
        //            newTable.Cell(row, 1).Range.Text = column.Name;
        //            newTable.Cell(row, 2).Range.Text = column.SqlType;
        //            newTable.Cell(row, 3).Range.Text = column.Length.ToString();
        //            newTable.Cell(row, 4).Range.Text = column.IsNull.ToString();
        //            newTable.Cell(row, 5).Range.Text = column.Description;
        //            row++;
        //        }

        //        //在表格中增加行
        //        wordDoc.Content.Tables[1].Rows.Add(ref Nothing);

        //        wordDoc.Paragraphs.Last.Range.Text = "文档创建时间：" + DateTime.Now.ToString();//“落款”
        //        wordDoc.Paragraphs.Last.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
        //    }
        //    //文件保存
        //    try
        //    {
        //        wordDoc.Save();
        //        wordDoc.Select();
        //    }
        //    catch
        //    {
        //    }
        //    //wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
        //    //wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
        //}
    }
}
