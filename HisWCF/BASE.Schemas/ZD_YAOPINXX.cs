using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_YAOPINXX_IN : MessageIn
    {
        /// <summary>
        ///项目归类
        /// </summary>
        public string XIANGMUGL	{get;set;}  
        /// <summary>
        ///输入码类型
        /// </summary>
        public string SHURUMLX	{get;set;}  
        /// <summary>
        ///输入码
        /// </summary>
        public string SHURUM	{get;set;}  

    }
    public class ZD_YAOPINXX_OUT : MessageOUT
    {
        /// <summary>
        /// 药品信息列表
        /// </summary>
        public List<YAOPINXX> YAOPINMX { get; set; }
        public ZD_YAOPINXX_OUT()
        {
            YAOPINMX = new List<YAOPINXX>();
        }
    }

    public class YAOPINXX
    {
        /// <summary>
        ///项目归类
        /// </summary>
        public string XIANGMUGL	    {get;set;}  
        /// <summary>
        ///项目序号
        /// </summary>
        public string XIANGMUXH	    {get;set;}  
        /// <summary>
        ///项目产品代码
        /// </summary>
        public string XIANGMUCDDM	{get;set;}  
        /// <summary>
        ///项目名称
        /// </summary>
        public string XIANGMUMC	    {get;set;}  
        /// <summary>
        ///项目归类名称
        /// </summary>
        public string XIANGMUGLMC	{get;set;}  
        /// <summary>
        ///项目规格
        /// </summary>
        public string XIANGMUGG	    {get;set;}  
        /// <summary>
        ///项目剂型
        /// </summary>
        public string XIANGMUJX	    {get;set;}  
        /// <summary>
        ///项目单位
        /// </summary>
        public string XIANGMUDW	    {get;set;}  
        /// <summary>
        ///项目产地名称
        /// </summary>
        public string XIANGMUCDMC	{get;set;}  
        /// <summary>
        ///单价
        /// </summary>
        public string DANJIA	    {get;set;}  
        /// <summary>
        /// 医保等级
        /// </summary>
        public string YIBAODJ	    {get;set;}
    }
}
