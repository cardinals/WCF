using System;
using System.Windows;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace SWSoft.Forms
{
    /// <summary>
    /// ʮ�����Ʊ༭��
    /// </summary>
    public class HexEditor : Control
    {
        /// <summary>
        /// ���Ĵ�������
        /// </summary>
        public static class StreamEngine
        {
            /// <summary>
            /// ɾ�����е�һ������ ��
            /// </summary>
            /// <param name="stream">Ŀ����</param>
            /// <param name="position">��ʼλ��</param>
            /// <param name="length">ɾ���ĳ���</param>
            /// <returns>����ɾ���Ƿ�ɹ�</returns>
            public static bool Delete(Stream stream, int position, int length)
            {
                if (stream == null || position < 0 || length <= 0) return false;
                if (position + length >= stream.Length)
                    stream.SetLength(position);
                else
                {
                    byte[] vBuffer = new byte[0x1000];
                    int i = position;
                    int l = 0;
                    do
                    {
                        stream.Position = i + length;
                        l = stream.Read(vBuffer, 0, vBuffer.Length);
                        stream.Position = i;
                        stream.Write(vBuffer, 0, l);
                        i += l;
                    }
                    while (l >= vBuffer.Length);
                    stream.SetLength(stream.Length - length);
                }
                return true;
            }

            /// <summary>
            /// ������֮��ĸ��ơ�
            /// </summary>
            /// <param name="streamFrom">��Դ��</param>
            /// <param name="streamTo">Ŀ����</param>
            /// <returns>���ظ����Ƿ�ɹ�</returns>
            public static bool Copy(Stream streamFrom, Stream streamTo)
            {
                if (streamFrom == null || streamTo == null) return false;
                byte[] vBuffer = new byte[0x1000];
                int l;
                while ((l = streamFrom.Read(vBuffer, 0, vBuffer.Length)) > 0)
                    streamTo.Write(vBuffer, 0, l);
                return true;
            }

            /// <summary>
            /// ������ָ��λ�ò���һ�����ݡ�
            /// </summary>
            /// <param name="stream">Ŀ����</param>
            /// <param name="position">����λ��</param>
            /// <param name="buffer">����</param>
            /// <param name="count">���ݴ�С</param>
            /// <returns>���ز��������Ƿ�ɹ�</returns>
            public static bool Insert(Stream stream, int position, byte[] data)
            {
                if (stream == null || data.Length <= 0 || position < 0 ||
                    position > stream.Length) return false;
                int i = (int)stream.Length;
                byte[] vBuffer = new byte[0x1000];
                stream.SetLength(i + data.Length);
                int l;
                do
                {
                    l = position + data.Length <= i - vBuffer.Length ?
                        vBuffer.Length : i - position;
                    stream.Position = i - l;
                    stream.Read(vBuffer, 0, l);
                    stream.Position = i - l + data.Length;
                    stream.Write(vBuffer, 0, l);
                    i -= l - data.Length;
                } while (l >= vBuffer.Length);
                stream.Position = position;
                stream.Write(data, 0, data.Length);
                return true;
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        public enum HitTestType
        {
            /// <summary>
            /// ��
            /// </summary>
            None,
            /// <summary>
            /// ��ַ��
            /// </summary>
            Address,
            /// <summary>
            /// ��ǩ��
            /// </summary>
            Bookmark,
            /// <summary>
            /// ʮ��������
            /// </summary>
            Hex,
            /// <summary>
            /// �ַ���
            /// </summary>
            Char,
            /// <summary>
            /// �����������
            /// </summary>
            HorizontalScrollBar,
            /// <summary>
            /// �����������
            /// </summary>
            VerticalScrollBar
        }

        /// <summary>
        /// ����Ԫ�ز�����Ϣ
        /// </summary>
        public sealed class HitTestInfo
        {
            /// <summary>
            /// ������������
            /// </summary>
            internal HitTestType type;
            /// <summary>
            /// Ԫ��������
            /// </summary>
            internal int col = 0;
            /// <summary>
            /// Ԫ��������
            /// </summary>
            internal int row = 0;
            internal int x = 0;
            internal int y = 0;
            public int X
            {
                get
                {
                    return x;
                }
            }
            public int Y
            {
                get
                {
                    return y;
                }
            }

            public override string ToString()
            {
                return string.Format("{{type: {0}, col: {1}, row: {2}}}", type, col, row);
            }

            public int ColumnIndex
            {
                get
                {
                    return col;
                }
            }

            public int RowIndex
            {
                get
                {
                    return row;
                }
            }
        }

        /// <summary>
        /// ��ý�������λ�����ڵ�������Ϣ
        /// </summary>
        /// <param name="x">��������</param>
        /// <param name="y">��������</param>
        /// <returns>��������λ�����ڵ�������Ϣ</returns>
        public HitTestInfo HitTest(int x, int y)
        {
            x -= 2;
            HitTestInfo info = new HitTestInfo();
            info.y = (y + itemHeight * topOffset) / itemHeight;
            info.x = (x + itemWidth * leftOffset + itemWidth / 2) / itemWidth;
            info.row = info.y;
            if (horizScrollBar != null && horizScrollBar.Visible &&
                horizScrollBar.Bounds.Contains(x, y))
            {
                info.type = HitTestType.HorizontalScrollBar;
                return info;
            }
            if (vertScrollBar != null && vertScrollBar.Visible &&
                vertScrollBar.Bounds.Contains(x, y))
            {
                info.type = HitTestType.VerticalScrollBar;
                return info;
            }
            if (vertScrollBar != null && vertScrollBar.Visible &&
                horizScrollBar != null && horizScrollBar.Visible &&
                new Rectangle(ClientSize.Width - vertScrollBar.Width,
                ClientSize.Height - horizScrollBar.Height,
                vertScrollBar.Width, horizScrollBar.Height).Contains(x, y))
                return info;

            if (info.x >= 0 && info.x <= 7)
            {
                info.type = HitTestType.Address;
            }
            else if (info.x >= 8 && info.x <= 9)
            {
                info.type = HitTestType.Bookmark;
            }
            else if (info.x >= 10 && info.x <= 58)
            {
                info.type = HitTestType.Hex;
                if (info.x >= 10 && info.x <= 33)
                    info.col = (info.x - 10) / 3;
                else if (info.x >= 34 && info.x <= 35)
                    info.col = 8;
                else if (info.x >= 36 && info.x <= 58)
                    info.col = (info.x - 11) / 3;
            }
            else if (info.x >= 60 && info.x <= 76)
            {
                info.type = HitTestType.Char;
                info.col = Math.Min(15, info.x - 60);
            }
            switch (info.type)
            {
                case HitTestType.Char:
                case HitTestType.Hex:
                    if (info.row * 16 + info.col > memoryStream.Length)
                    {
                        Point vPoint = CoordinateFromPosistion((int)memoryStream.Length);
                        info.col = vPoint.X;
                        info.row = vPoint.Y;
                    }
                    break;
            }
            return info;
        }

        #region API�������
        private const int WM_IME_CHAR = 0x0286;

        [DllImport("user32.dll")]
        public static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap,
            int nWidth, int nHeight);

        [DllImport("user32.dll")]
        public static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool DestroyCaret();

        [DllImport("user32.dll")]
        public static extern bool SetCaretPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        public class LOGFONT
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COMPOSITIONFORM
        {
            public int dwStyle;
            public Point ptCurrentPos;
            public RECT rcArea;
        }

        public const int CFS_DEFAULT = 0x0000;
        public const int CFS_RECT = 0x0001;
        public const int CFS_POINT = 0x0002;
        public const int CFS_FORCE_POSITION = 0x0020;
        public const int CFS_CANDIDATEPOS = 0x0040;
        public const int CFS_EXCLUDE = 0x0080;

        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hImc);

        [DllImport("imm32.dll")]
        public static extern bool ImmSetCompositionFont(IntPtr hImc, LOGFONT lpLogfont);

        [DllImport("imm32.dll")]
        public static extern bool ImmSetCompositionWindow(IntPtr hImc,
            ref COMPOSITIONFORM lpCompForm);

        [DllImport("imm32.dll")]
        public static extern bool ImmNotifyIME(IntPtr hImc,
            int dwAction, int dwIndex, int dwValue);
        #endregion API�������

        #region �ֶ�
        /// <summary>
        /// ���������
        /// </summary>
        private HScrollBar horizScrollBar;
        /// <summary>
        /// ���������
        /// </summary>
        private VScrollBar vertScrollBar;
        /// <summary>
        /// ��ǰ�����������
        /// </summary>
        private MemoryStream memoryStream;
        /// <summary>
        /// ����ַ
        /// </summary>
        private int baseAddress;
        /// <summary>
        /// һ��ĸ߶�
        /// </summary>
        private int itemHeight;
        /// <summary>
        /// һ��Ŀ��
        /// </summary>
        private int itemWidth;
        /// <summary>
        /// ������
        /// </summary>
        private int lineCount;
        /// <summary>
        /// �ɼ�������
        /// </summary>
        private int viewColCount;
        /// <summary>
        /// �ɼ�������
        /// </summary>
        private int viewRowCount;
        /// <summary>
        /// ��������ƫ��
        /// </summary>
        private int topOffset = 0;
        /// <summary>
        /// ��߹���ƫ��
        /// </summary>
        private int leftOffset = 0;
        /// <summary>
        /// ѡ����ʼλ��
        /// </summary>
        private int selStart;
        /// <summary>
        /// ѡ�����ݳ���
        /// </summary>
        private int selLength;
        /// <summary>
        /// ��������е�����
        /// </summary>
        private HitTestType colType;
        /// <summary>
        /// ����Ƿ�ɼ�
        /// </summary>
        private bool caretVisible;
        /// <summary>
        /// ����Ƿ���ǰ
        /// </summary>
        private bool caretFirst;
        /// <summary>
        /// ���༭ʱ����һ��ʮ�������ַ���
        /// </summary>
        private char modifyHex = '\x00';
        /// <summary>
        /// ��ǩ�б���¼���Ǳ�ǩ���ڵ�������
        /// </summary>
        private List<int> bookmarks = new List<int>();
        /// <summary>
        /// ��ǰ��ǩ����š�
        /// </summary>
        private int bookmarkIndex = 0;
        /// <summary>
        /// ��갴��ʱ��������Ϣ��
        /// </summary>
        private HitTestInfo downHitInfo = new HitTestInfo();
        /// <summary>
        /// ����ƶ�ʱ����������
        /// </summary>
        private HitTestInfo moveHitInfo = new HitTestInfo();
        /// <summary>
        /// ����Ƿ�������λ�á�
        /// </summary>
        private bool endCaret = false;
        /// <summary>
        /// ���뷨�ַ���������
        /// </summary>
        Encoding encoding = Encoding.Default;
        /// <summary>
        /// �Ƿ�Ϊ����ģʽ��
        /// </summary>
        private bool insertMode = false;
        /// <summary>
        /// �Ƿ�ֻ����
        /// </summary>
        private bool readOnly = false;
        /// <summary>
        /// ���ݴ�С�Ƿ�̶���
        /// </summary>
        private bool fixedSize = false;
        #endregion �ֶ�

        #region ����
        /// <summary>
        /// ��ȡ������ݵ���ʼ��ַ��
        /// </summary>
        [Description("��ȡ������ݵ���ʼ��ַ��")]
        public int BaseAddress
        {
            get
            {
                return baseAddress;
            }
            set
            {
                if (baseAddress == value) return;
                baseAddress = value;
                Invalidate();
            }
        }
        /// <summary>
        /// ��ȡ�����ñ༭����ѡ����������ʼ�㡣
        /// </summary>
        [Description("��ȡ�����ñ༭����ѡ����������ʼ�㡣")]
        public int SelectionStart
        {
            get
            {
                return selStart;
            }
            set
            {
                if (selStart == value) return;
                selStart = Math.Max(Math.Min(value, (int)memoryStream.Length), 0);
                selLength = Math.Max(Math.Min(selLength,
                    (int)memoryStream.Length - selStart), 0);
                Invalidate();
                if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// ��ȡ�����ñ༭����ѡ�����ֽ�����
        /// </summary>
        [Description("��ȡ�����ñ༭����ѡ�����ֽ�����")]
        public int SelectionLength
        {
            get
            {
                return selLength;
            }
            set
            {
                if (selLength == value) return;
                selLength = Math.Max(0, Math.Min((int)memoryStream.Length - selStart, value));
                Invalidate();
                if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// ��ȡ��ǰ�����������š�
        /// </summary>
        [Description("��ȡ��ǰ�����������š�")]
        public int RowIndex
        {
            get
            {
                Point vPoint = CoordinateFromPosistion(
                    selStart + (caretFirst ? 0 : selLength));
                return vPoint.Y;
            }
            set
            {
                Point vPoint = CoordinateFromPosistion(
                    selStart + (caretFirst ? 0 : selLength));
                SelectionStart = value * 16 + vPoint.X;
            }
        }
        /// <summary>
        /// ��ȡ��ǰ�����������š�
        /// </summary>
        [Description("��ȡ��ǰ�����������š�")]
        public int ColIndex
        {
            get
            {
                Point vPoint = CoordinateFromPosistion(
                    selStart + (caretFirst ? 0 : selLength));
                return vPoint.X;
            }
            set
            {
                Point vPoint = CoordinateFromPosistion(
                    selStart + (caretFirst ? 0 : selLength));
                SelectionStart = vPoint.Y * 16 + value;
            }
        }
        /// <summary>
        /// ��ȡ�����������ַ�ʱʹ�õı���롣
        /// </summary>
        [Description("��ȡ�����������ַ�ʱʹ�õı���롣")]
        public Encoding Encoding
        {
            get { return encoding == null ? Encoding.Default : encoding; }
            set { encoding = value; }
        }

        /// <summary>
        /// ��ȡ�����õ��Ƿ���ò���ģʽ���롣
        /// </summary>
        [Description("��ȡ�����õ��Ƿ���ò���ģʽ���롣")]
        public bool InsertMode { get; set; }

        /// <summary>
        /// ��ȡ�����ñ༭��ֻ����
        /// </summary>
        [Description("��ȡ�����ñ༭��ֻ����")]
        public bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
            }
        }
        /// <summary>
        /// ��ȡ���������ݴ�С�Ƿ�̶����䡣
        /// </summary>
        [Description("��ȡ���������ݴ�С�Ƿ�̶�����")]
        public bool FixedSize
        {
            get
            {
                return fixedSize;
            }
            set
            {
                fixedSize = value;
            }
        }
        #endregion ����

        #region �¼�
        /// <summary>
        /// �ڵ�ǰѡ�����ݸ��ĺ󴥷���
        /// </summary>
        [Description("�ڵ�ǰѡ�����ݸ��ĺ󴥷���")]
        public event EventHandler SelectionChanged;
        /// <summary>
        /// �ڵ�ǰ�������ݸı�󴥷���
        /// </summary>
        [Description("�ڵ�ǰ�������ݸı�󴥷���")]
        public event EventHandler DataChanged;
        #endregion �¼�

        public HexEditor()
        {
            horizScrollBar = new HScrollBar();
            vertScrollBar = new VScrollBar();
            Width = 200;
            Height = 200;

            horizScrollBar.RightToLeft = RightToLeft.Inherit;
            horizScrollBar.Top = base.ClientRectangle.Height - horizScrollBar.Height;
            horizScrollBar.Left = 0;
            horizScrollBar.Visible = false;
            horizScrollBar.Scroll += new ScrollEventHandler(HexEditorHScroll);
            horizScrollBar.Cursor = Cursors.Arrow;
            base.Controls.Add(this.horizScrollBar);
            vertScrollBar.Top = 0;
            vertScrollBar.Left = base.ClientRectangle.Width - vertScrollBar.Width;
            vertScrollBar.Visible = false;
            vertScrollBar.Scroll += new ScrollEventHandler(HexEditorVScroll);
            vertScrollBar.Cursor = Cursors.Arrow;
            base.Controls.Add(this.vertScrollBar);
            memoryStream = new MemoryStream();
            colType = HitTestType.Hex;
            BackColor = SystemColors.Window;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// ��������λ�ü������ڱ༭���е��������ꡣ
        /// </summary>
        /// <param name="posistion">����λ��</param>
        /// <returns>�������ڱ༭���е���������</returns>
        public Point CoordinateFromPosistion(int posistion)
        {
            if (posistion < 0 || posistion > memoryStream.Length) return Point.Empty;
            return new Point(posistion % 16, posistion / 16);
        }

        /// <summary>
        /// �ı���档
        /// </summary>
        private void DoChange()
        {
            LayoutScrollBars();
            ScrollIntoView();
            UpdateCaret();
            Invalidate();
        }

        /// <summary>
        /// ��ȡ�������ַ�λ�á�
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="col">����</param>
        /// <returns>�����������ַ�λ��</returns>
        private int XFromCol(HitTestType type, int col)
        {
            switch (type)
            {
                case HitTestType.Char:
                    return 60 + col;
                case HitTestType.Hex:
                    return 10 + col * 3 + (col < 8 ? 0 : 1);
                default: return 0;
            }
        }

        /// <summary>
        /// ������������ڴ���ʹ���ɼ���
        /// </summary>
        /// <returns>�����Ƿ�������</returns>
        public bool ScrollIntoView()
        {
            bool vResult = false;
            Point vPoint = CoordinateFromPosistion(
                selStart + (caretFirst ? 0 : selLength));
            if (vPoint.Y < topOffset)
            {
                topOffset = vPoint.Y;
                vResult = true;
            }
            else if (vPoint.Y >= topOffset + viewRowCount - 1)
            {
                topOffset = vPoint.Y - (viewRowCount - 2);
                vResult = true;
            }
            int i = XFromCol(colType, vPoint.X);
            if (modifyHex != '\x00') i++;
            if (i < leftOffset)
            {
                leftOffset = i;
                vResult = true;
            }
            else if (i >= leftOffset + viewColCount - 1)
            {
                leftOffset = i - (viewColCount - 2);
                vResult = true;
            }
            if (vResult)
            {
                vertScrollBar.Value = topOffset;
                horizScrollBar.Value = leftOffset;
            }
            return vResult;
        }

        /// <summary>
        /// �������м��������������ꡣ
        /// </summary>
        /// <param name="type">������</param>
        /// <param name="col">��</param>
        /// <param name="row">��</param>
        /// <returns>��������������������</returns>
        public Point PointFromColRow(HitTestType type, int col, int row)
        {
            Point vResult = new Point();
            switch (type)
            {
                case HitTestType.Address:
                    vResult.X = 0;
                    vResult.Y = row * itemHeight;
                    break;
                case HitTestType.Char:
                case HitTestType.Hex:
                    vResult.X = XFromCol(type, col) * itemWidth;
                    vResult.Y = row * itemHeight;
                    break;
                default: return Point.Empty;
            }
            vResult.X -= leftOffset * itemWidth;
            vResult.Y -= topOffset * itemHeight;
            return vResult;
        }

        /// <summary>
        /// ���¹����ʾ��λ�ú�״̬��
        /// </summary>
        private void UpdateCaret()
        {
            if (caretVisible) DestroyCaret();
            caretVisible = Focused && selLength <= 0;
            if (!caretVisible) return;
            Point vPoint = CoordinateFromPosistion(
                selStart + (caretFirst ? 0 : selLength));
            CreateCaret(Handle, IntPtr.Zero, 2, itemHeight);
            ShowCaret(Handle);
            bool b = endCaret && vPoint.X == 0 && vPoint.Y > 0;
            if (b)
            {
                vPoint.X = 16;
                vPoint.Y--;
            }
            vPoint = PointFromColRow(colType, vPoint.X, vPoint.Y);

            vPoint.X += 1;
            if (colType == HitTestType.Hex && modifyHex != '\x0')
                vPoint.X += itemWidth * 2;
            if (b && colType == HitTestType.Hex)
                vPoint.X -= itemWidth;
            SetCaretPos(vPoint.X, vPoint.Y);
            IntPtr vImc = ImmGetContext(Handle);
            COMPOSITIONFORM vCompForm = new COMPOSITIONFORM();
            vCompForm.dwStyle = CFS_POINT;
            vCompForm.ptCurrentPos = vPoint;
            ImmSetCompositionWindow(vImc, ref vCompForm);
            ImmReleaseContext(Handle, vImc);
        }

        private byte[] stream;
        public byte[] Stream
        {
            get { return stream; }
            set
            {
                if (value != null)
                {
                    stream = value;
                    memoryStream.SetLength(0);
                    modifyHex = '\x00';
                    selLength = 0;
                    selStart = 0;
                    bookmarkIndex = -1;
                    bookmarks.Clear();
                    memoryStream.Write(value, 0, value.Length);
                    if (SelectionChanged != null)
                    {
                        SelectionChanged(this, EventArgs.Empty);
                    }
                    DoChange();
                }
            }
        }

        /// <summary>
        /// ���ļ����������ݡ�
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        public void LoadFromFile(string strFileName)
        {
            memoryStream.SetLength(0);
            #region �ֶγ�ʼ��
            modifyHex = '\x00';
            selLength = 0;
            selStart = 0;
            bookmarkIndex = -1;
            bookmarks.Clear();
            #endregion �ֶγ�ʼ��

            if (File.Exists(strFileName))
            {
                FileStream vFileStream = new FileStream(strFileName,
                    FileMode.Open, FileAccess.Read);
                try
                {
                    StreamEngine.Copy(vFileStream, memoryStream);
                    if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
                    DoChange();
                }
                finally
                {
                    vFileStream.Close();
                    vFileStream.Dispose();
                }
            }
        }

        /// <summary>
        /// �����ݱ��浽�ļ��С�
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        public void SaveToFile(string strFileName)
        {
            FileStream vFileStream = new FileStream(strFileName,
                FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                StreamEngine.Copy(memoryStream, vFileStream);
            }
            finally
            {
                vFileStream.Close();
                vFileStream.Dispose();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            moveHitInfo = HitTest(e.X, e.Y);
            switch (moveHitInfo.type)
            {
                case HitTestType.Hex:
                case HitTestType.Char:
                    Cursor = Cursors.IBeam;
                    break;
                default:
                    Cursor = Cursors.Default;
                    break;
            }
            switch (downHitInfo.type)
            {
                case HitTestType.Hex:
                case HitTestType.Char:
                    if (moveHitInfo.type == downHitInfo.type)
                    {
                        SelectData(downHitInfo.row * 16 + downHitInfo.col,
                            moveHitInfo.row * 16 + moveHitInfo.col);
                    }
                    else
                    {
                        SelectData(downHitInfo.row * 16 + downHitInfo.col,
                            moveHitInfo.row * 16 + (downHitInfo.x > moveHitInfo.x ? 0 : 16));
                    }
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            downHitInfo = new HitTestInfo();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!Focused && CanFocus) Focus();
            if (e.Button != MouseButtons.Left) return;
            downHitInfo = HitTest(e.X, e.Y);
            switch (downHitInfo.type)
            {
                case HitTestType.Hex:
                case HitTestType.Char:
                    colType = downHitInfo.type;
                    selLength = 0;
                    selStart = downHitInfo.row * 16 + downHitInfo.col;
                    endCaret = false;
                    modifyHex = '\x00';
                    if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
                    UpdateCaret();
                    Invalidate();
                    break;
                case HitTestType.Bookmark:
                    if (bookmarks.Contains(downHitInfo.row))
                        bookmarks.Remove(downHitInfo.row);
                    else bookmarks.Add(downHitInfo.row);
                    Invalidate();
                    break;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            #region ����������
            if ((Control.ModifierKeys & (Keys.Control | Keys.Alt | Keys.Shift)) == Keys.None)
            {
                if (!vertScrollBar.Visible) return;
                int i = -e.Delta / 120 * vertScrollBar.LargeChange - 1;
                if (i < 0 && vertScrollBar.Value <= 0) return;
                if (i > 0 && vertScrollBar.Value >= lineCount - viewRowCount + 1) return;
                vertScrollBar.Value = Math.Min(Math.Max(0, vertScrollBar.Value + i),
                    lineCount - viewRowCount + 1);
                topOffset = vertScrollBar.Value;
                UpdateCaret();
                Invalidate();
            }
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (!horizScrollBar.Visible) return;
                int i = -e.Delta / 120;
                if (i < 0 && horizScrollBar.Value <= 0) return;
                if (i > 0 && horizScrollBar.Value >= 76 - viewColCount + 1) return;
                horizScrollBar.Value = Math.Min(Math.Max(0, horizScrollBar.Value + i),
                    76 - viewColCount + 1);
                leftOffset = horizScrollBar.Value;
                UpdateCaret();
                Invalidate();
            }
            #endregion ����������
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt) return false;
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Escape:
                case Keys.Space:
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Delete:
                case Keys.D0:
                case Keys.Return:
                case Keys.NumPad0:
                case Keys.F2:
                case Keys.Tab:
                    return true;

                case Keys.Insert:
                case Keys.A:
                case Keys.C:
                    if ((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) != Keys.Control)
                    {
                        break;
                    }
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.F2:
                    #region ������ǩ
                    if (e.Control)
                    {
                        if (bookmarks.Contains(RowIndex))
                            bookmarks.Remove(RowIndex);
                        else bookmarks.Add(RowIndex);
                        Invalidate();
                    }
                    else
                    {
                        if (bookmarks.Count <= 0) return;
                        if (e.Shift)
                        {
                            if (bookmarkIndex <= 0)
                                bookmarkIndex = bookmarks.Count - 1;
                            else bookmarkIndex--;
                        }
                        else
                        {
                            if (bookmarkIndex >= bookmarks.Count - 1)
                                bookmarkIndex = 0;
                            else bookmarkIndex++;
                        }
                        RowIndex = bookmarks[bookmarkIndex];
                        modifyHex = '\x00';
                        ScrollIntoView();
                        UpdateCaret();
                        Invalidate();
                    }
                    break;
                    #endregion ������ǩ
                case Keys.Tab:
                    #region �л�������
                    if (colType == HitTestType.Hex)
                        colType = HitTestType.Char;
                    else colType = HitTestType.Hex;
                    modifyHex = '\x00';
                    ScrollIntoView();
                    UpdateCaret();
                    Invalidate();
                    break;
                    #endregion �л�������
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                    #region ������λ��
                    int A = caretFirst ? selStart : selStart + selLength;
                    int B = caretFirst ? selStart + selLength : selStart;
                    switch (e.KeyCode)
                    {
                        case Keys.PageUp:
                            if (RowIndex <= 0) return;
                            A -= Math.Max(1, 16 * (viewRowCount - 1));
                            break;
                        case Keys.PageDown:
                            if (RowIndex >= lineCount) return;
                            A += Math.Max(1, 16 * (viewRowCount - 1));
                            break;
                        case Keys.Up:
                            if (RowIndex <= 0) return;
                            A -= 16;
                            break;
                        case Keys.Down:
                            if (RowIndex >= lineCount) return;
                            A += 16;
                            break;
                        case Keys.Left:
                            A--;
                            endCaret = false;
                            break;
                        case Keys.Right:
                            A++;
                            endCaret = false;
                            break;
                        case Keys.Home:
                            if (e.Control)
                                A = 0;
                            else
                            {
                                if (endCaret)
                                {
                                    A = A - A % 16 - 16;
                                }
                                else A = A - A % 16;
                            }
                            endCaret = false;
                            break;
                        case Keys.End:
                            if (e.Control)
                                A = (int)memoryStream.Length;
                            else
                            {
                                if (!endCaret) A = A - A % 16 + 16;
                            }
                            if (A > memoryStream.Length)
                                A = (int)memoryStream.Length;
                            endCaret = A % 16 == 0;
                            break;
                    }
                    if (e.Shift)
                        SelectData(B, A);
                    else SelectData(A, A);
                    break;
                    #endregion ������λ��
                case Keys.Back:
                case Keys.Delete:
                    #region �����˸��ɾ��
                    if (fixedSize) return;
                    if (selLength > 0)
                    {
                        StreamEngine.Delete(memoryStream, selStart, selLength);
                        selLength = 0;
                    }
                    else
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Back:
                                if (selStart <= 0) return;
                                StreamEngine.Delete(memoryStream, selStart - 1, 1);
                                selStart--;
                                break;
                            case Keys.Delete:
                                StreamEngine.Delete(memoryStream, selStart, 1);
                                break;
                        }
                    }
                    DoChange();
                    if (DataChanged != null) DataChanged(this, EventArgs.Empty);
                    if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
                    break;
                    #endregion �����˸��ɾ��
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for (int i = 0; i < viewRowCount; i++)
            {
                int currentLine = topOffset + i;
                if (currentLine >= lineCount) break;
                memoryStream.Position = currentLine * 16;
                byte[] vBuffer = new byte[16];
                int vLength = memoryStream.Read(vBuffer, 0, vBuffer.Length);
                DrawAddress(e.Graphics, currentLine,
                    (0 - leftOffset) * itemWidth, i * itemHeight);
                DrawHex(e.Graphics, currentLine, vBuffer, vLength,
                    (8 + 2 - leftOffset) * itemWidth, i * itemHeight);
                DrawChar(e.Graphics, currentLine, vBuffer, vLength,
                    (8 + 2 + 3 * 16 + 2 - leftOffset) * itemWidth, i * itemHeight);
            }
            if (vertScrollBar.Visible && horizScrollBar.Visible)
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(
                    ClientSize.Width - vertScrollBar.Width,
                    ClientSize.Height - horizScrollBar.Height,
                    vertScrollBar.Width, horizScrollBar.Height));
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_IME_CHAR:
                    OnKeyPress(new KeyPressEventArgs((char)m.WParam));
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// �ڵ�ǰ���봦����һ�����ݡ�
        /// </summary>
        /// <param name="buffer">����</param>
        public void InsertData(byte[] buffer)
        {
            int i = (int)memoryStream.Length;
            if (insertMode && (colType == HitTestType.Char ||
                (colType == HitTestType.Hex && modifyHex != '\x00')))
            {
                if (!fixedSize)
                    StreamEngine.Delete(memoryStream, selStart, selLength);
                StreamEngine.Insert(memoryStream, selStart, buffer);
                if (fixedSize)
                    StreamEngine.Delete(memoryStream,
                        (int)memoryStream.Length - buffer.Length, buffer.Length);
            }
            else
            {
                memoryStream.Position = selStart;
                memoryStream.Write(buffer, 0, buffer.Length);
            }
            selLength = 0;
            if (colType == HitTestType.Char ||
                (colType == HitTestType.Hex && modifyHex == '\x00'))
                selStart += buffer.Length;
            ScrollIntoView();
            if (i != memoryStream.Length)
                DoChange();
            else
            {
                UpdateCaret();
                Invalidate();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (readOnly) return;
            if (e.KeyChar >= 0 && e.KeyChar <= 27) return;
            if (fixedSize && selStart >= memoryStream.Length) return;
            switch (colType)
            {
                case HitTestType.Char:
                    InsertData(encoding.GetBytes(new char[] { e.KeyChar }));
                    if (DataChanged != null) DataChanged(this, EventArgs.Empty);
                    break;
                case HitTestType.Hex:
                    if ("0123456789ABCDEFabcdef".IndexOf(e.KeyChar) < 0) return;
                    if (modifyHex == '\x00')
                    {
                        modifyHex = char.ToUpper((char)e.KeyChar);
                        byte vByte;
                        byte.TryParse(new string(new char[] { modifyHex }),
                            NumberStyles.HexNumber, null, out vByte);
                        InsertData(new byte[] { vByte });
                    }
                    else
                    {
                        byte vByte;
                        byte.TryParse(new string(new char[] { modifyHex, (char)e.KeyChar }),
                            NumberStyles.HexNumber, null, out vByte);
                        modifyHex = '\x00';
                        InsertData(new byte[] { vByte });
                    }
                    if (DataChanged != null) DataChanged(this, EventArgs.Empty);
                    if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// ѡ��һ�����ݡ�
        /// </summary>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">����λ��</param>
        public void SelectData(int start, int end)
        {
            start = Math.Min(Math.Max(start, 0), (int)memoryStream.Length);
            end = Math.Min(Math.Max(end, 0), (int)memoryStream.Length);
            bool b = caretFirst;
            int i = selStart;
            int j = selLength;
            caretFirst = start > end;
            selStart = Math.Min(Math.Max(0, Math.Min(start, end)), (int)memoryStream.Length);
            selLength = Math.Min(Math.Abs(start - end), (int)memoryStream.Length - selStart);
            if (b == caretFirst && i == selStart && j == selLength) return; // û�б仯
            modifyHex = '\x00';
            ScrollIntoView();
            UpdateCaret();
            Invalidate();
            if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// ��ȡ�ڱ༭����ʾ�ַ���
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="buf">����</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">����λ��</param>
        /// <returns>���ر༭����ʾ�ַ�</returns>
        public virtual string ViewText(HitTestType type, byte[] buf, int len, int start, int end)
        {
            if (len <= 0) return string.Empty;
            const string vCharHexs = "0123456789ABCDEF";
            StringBuilder vBuffer = new StringBuilder(128);
            start = Math.Max(0, start);
            end = Math.Min(len - 1, end);
            for (int i = start; i <= end; i++)
            {
                switch (type)
                {
                    case HitTestType.Hex:
                        if (i == 8) vBuffer.Append(" ");
                        vBuffer.Append(vCharHexs[buf[i] >> 4]);
                        vBuffer.Append(vCharHexs[buf[i] & 0x0F]);
                        vBuffer.Append(" ");
                        break;
                    case HitTestType.Char:
                        if (buf[i] >= 32 && buf[i] <= 126)
                            vBuffer.Append((char)buf[i]);
                        else vBuffer.Append('.');
                        break;
                }
            }
            if (type == HitTestType.Hex)
                return vBuffer.ToString().Trim();
            else return vBuffer.ToString();
        }

        /// <summary>
        /// ���Ƶ�ַ���顣
        /// </summary>
        /// <param name="g">����</param>
        /// <param name="addr">��ַ</param>
        /// <param name="x">��������</param>
        /// <param name="y">��������</param>
        protected virtual void DrawAddress(Graphics g, int line, int x, int y)
        {
            TextRenderer.DrawText(g, (baseAddress + line * 16).ToString("X8"), Font,
                new Point(x, y), Color.Blue, TextFormatFlags.Left);
            #region ������ǩ
            if (bookmarks.Contains(line))
            {
                Rectangle vRectangle = new Rectangle(
                    x + 8 * itemWidth + itemWidth / 2, y + itemHeight / 6,
                    itemWidth * 2 - itemWidth / 2, itemHeight - itemHeight / 3);
                Brush vBrush = new LinearGradientBrush(
                    vRectangle,
                    Color.White,
                    Color.Blue,
                    LinearGradientMode.Horizontal);
                g.FillRectangle(vBrush, vRectangle);
                g.DrawRectangle(Pens.Blue, vRectangle);
            }
            #endregion ������ǩ
        }

        /// <summary>
        /// ����ѡ��
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="g">����</param>
        /// <param name="line">������</param>
        /// <param name="buf">����</param>
        /// <param name="len">���ݳ���</param>
        /// <param name="x">��������</param>
        /// <param name="y">��������</param>
        protected virtual void DrawSelection(HitTestType type, Graphics g, int line,
            byte[] buf, int len, int x, int y)
        {
            #region ����ѡ����
            if (selLength <= 0) return;
            Point vSelStart = CoordinateFromPosistion(selStart);
            Point vSelEnd = CoordinateFromPosistion(selStart + selLength - 1);
            string vViewText = string.Empty;
            if (colType == type)
            {
                if (line == vSelStart.Y && line == vSelEnd.Y)
                {
                    Point vPoint = PointFromColRow(type, vSelStart.X, line);
                    vViewText = ViewText(type, buf, len, vSelStart.X, vSelEnd.X);
                    TextRenderer.DrawText(g, vViewText, Font,
                        vPoint, SystemColors.HighlightText, SystemColors.Highlight);
                }
                else if (line == vSelStart.Y)
                {
                    Point vPoint = PointFromColRow(type, vSelStart.X, line);
                    vViewText = ViewText(type, buf, len, vSelStart.X, len);
                    TextRenderer.DrawText(g, vViewText, Font,
                        vPoint, SystemColors.HighlightText, SystemColors.Highlight);
                }
                else if (line == vSelEnd.Y)
                {
                    Point vPoint = PointFromColRow(type, 0, line);
                    vViewText = ViewText(type, buf, len, 0, vSelEnd.X);
                    TextRenderer.DrawText(g, vViewText, Font,
                        vPoint, SystemColors.HighlightText, SystemColors.Highlight);
                }
                else if (line > vSelStart.Y && line < vSelEnd.Y)
                {
                    vViewText = ViewText(type, buf, len, 0, len);
                    TextRenderer.DrawText(g, vViewText, Font,
                        new Point(x, y), SystemColors.HighlightText, SystemColors.Highlight);
                }
            }
            else
            {
                int vSpace = type == HitTestType.Hex ? 2 : 1;
                if (line == vSelStart.Y && line == vSelEnd.Y)
                {
                    Point A = PointFromColRow(type, vSelStart.X, line);
                    Point B = PointFromColRow(type, vSelEnd.X, line);
                    g.DrawRectangle(Pens.Black, A.X, A.Y,
                        B.X - A.X + itemWidth * vSpace, itemHeight);
                }
                else if (line == vSelStart.Y)
                {
                    Point A = PointFromColRow(type, vSelStart.X, line);
                    Point B = PointFromColRow(type, 15, line);
                    g.DrawLine(Pens.Black, A.X, A.Y, A.X, B.Y + itemHeight);
                    g.DrawLine(Pens.Black,
                        B.X + itemWidth * vSpace, A.Y,
                        B.X + itemWidth * vSpace, B.Y + itemHeight);
                    g.DrawLine(Pens.Black,
                        A.X, A.Y,
                        B.X + itemWidth * vSpace, A.Y);
                    B = PointFromColRow(type, 0, line);
                    g.DrawLine(Pens.Black,
                        B.X, B.Y + itemHeight,
                        A.X, B.Y + itemHeight);
                }
                else if (line == vSelEnd.Y)
                {
                    Point A = PointFromColRow(type, 0, line);
                    Point B = PointFromColRow(type, vSelEnd.X, line);
                    g.DrawLine(Pens.Black, A.X, A.Y, A.X, A.Y + itemHeight);
                    g.DrawLine(Pens.Black,
                        B.X + itemWidth * vSpace, A.Y,
                        B.X + itemWidth * vSpace, A.Y + itemHeight);
                    g.DrawLine(Pens.Black,
                        A.X, A.Y + itemHeight,
                        B.X + itemWidth * vSpace, A.Y + itemHeight);
                    A = PointFromColRow(type, 15, line);
                    g.DrawLine(Pens.Black,
                        B.X + itemWidth * vSpace, A.Y,
                        A.X + itemWidth * vSpace, A.Y);
                }
                else if (line > vSelStart.Y && line < vSelEnd.Y)
                {
                    Point A = PointFromColRow(type, 0, line);
                    Point B = PointFromColRow(type, 15, line);
                    g.DrawLine(Pens.Black,
                        A.X, A.Y,
                        A.X, A.Y + itemHeight);
                    g.DrawLine(Pens.Black,
                        B.X + itemWidth * vSpace, A.Y,
                        B.X + itemWidth * vSpace, A.Y + itemHeight);
                }
            }
            #endregion ����ѡ����
        }

        /// <summary>
        /// ����ʮ���������顣
        /// </summary>
        /// <param name="g">����</param>
        /// <param name="buf">������</param>
        /// <param name="len">���峤��</param>
        /// <param name="x">��������</param>
        /// <param name="y">��������</param>
        protected virtual void DrawHex(Graphics g, int line,
            byte[] buf, int len, int x, int y)
        {
            TextRenderer.DrawText(g, ViewText(HitTestType.Hex, buf, len, 0, len), Font,
                new Point(x, y), Color.Brown);
            DrawSelection(HitTestType.Hex, g, line, buf, len, x, y);
        }

        /// <summary>
        /// �����ַ����顣
        /// </summary>
        /// <param name="g">����</param>
        /// <param name="buf">������</param>
        /// <param name="len">���峤��</param>
        /// <param name="x">��������</param>
        /// <param name="y">��������</param>
        protected virtual void DrawChar(Graphics g, int line,
            byte[] buf, int len, int x, int y)
        {
            TextRenderer.DrawText(g, ViewText(HitTestType.Char, buf, len, 0, len), Font,
                new Point(x, y), Color.Green, TextFormatFlags.Left);
            DrawSelection(HitTestType.Char, g, line, buf, len, x, y);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (horizScrollBar != null)
                {
                    horizScrollBar.Dispose();
                    horizScrollBar = null;
                }
                if (vertScrollBar != null)
                {
                    vertScrollBar.Dispose();
                    vertScrollBar = null;
                }
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// ���¶Թ��������֡�
        /// </summary>
        private void LayoutScrollBars()
        {
            if (horizScrollBar == null || vertScrollBar == null) return;
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
            base.SuspendLayout();
            try
            {
                Size vSize = TextRenderer.MeasureText("0123456789", Font);
                itemHeight = vSize.Height;
                itemWidth = vSize.Width / 10;
                lineCount = (int)(memoryStream.Length / 16 + 1);
                viewColCount = ClientSize.Width / itemWidth + 1;
                viewRowCount = ClientSize.Height / itemHeight + 1;
                if (viewColCount <= 76)
                    viewRowCount = (ClientSize.Height - horizScrollBar.Height) / itemHeight + 1;
                if (viewRowCount <= lineCount)
                    viewColCount = (ClientSize.Width - vertScrollBar.Width) / itemWidth + 1;
                horizScrollBar.Visible = viewColCount <= 76;
                vertScrollBar.Visible = viewRowCount <= lineCount; // �������ڿɼ���

                if (horizScrollBar.Visible)
                {
                    horizScrollBar.Minimum = 0;
                    horizScrollBar.Maximum = 76;
                    horizScrollBar.SmallChange = 1;
                    horizScrollBar.LargeChange = viewColCount;
                    horizScrollBar.Width = ClientSize.Width -
                        (vertScrollBar.Visible ? vertScrollBar.Width : 0);
                    horizScrollBar.Top = ClientSize.Height - horizScrollBar.Height;
                }

                if (vertScrollBar.Visible)
                {
                    vertScrollBar.Minimum = 0;
                    vertScrollBar.Maximum = lineCount;
                    vertScrollBar.SmallChange = 1;
                    vertScrollBar.LargeChange = viewRowCount;
                    vertScrollBar.Height = ClientSize.Height -
                        (horizScrollBar.Visible ? horizScrollBar.Height : 0);
                    vertScrollBar.Left = ClientSize.Width - vertScrollBar.Width;
                }
                int i = leftOffset;
                int j = topOffset;
                leftOffset = Math.Max(Math.Min(leftOffset, 76 - viewColCount + 1), 0);
                topOffset = Math.Max(Math.Min(topOffset, lineCount - viewRowCount + 1), 0);
            }
            finally
            {
                base.ResumeLayout(false);
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            if (memoryStream == null) return;
            LayoutScrollBars();
            UpdateCaret();
            Invalidate();
        }

        private void HexEditorHScroll(object sender, ScrollEventArgs e)
        {
            if (leftOffset == e.NewValue) return;
            leftOffset = e.NewValue;
            UpdateCaret();
            Invalidate();
        }

        private void HexEditorVScroll(object sender, ScrollEventArgs e)
        {
            if (topOffset == e.NewValue) return;
            topOffset = e.NewValue;
            UpdateCaret();
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            UpdateCaret();
            Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            UpdateCaret();
            Invalidate();
        }
    }
}
