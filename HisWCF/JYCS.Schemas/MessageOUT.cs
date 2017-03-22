using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
	/// <summary>
	/// 出参消息基类
	/// </summary>
    public class MessageOUT
    {
		/// <summary>
		/// 基本出参
		/// </summary>
        public OUTMSG OUTMSG { get; set; }

        public MessageOUT()
        {
            OUTMSG = new OUTMSG();
        }
    }
}
