using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Common.WSCall
{
    public class JsonHandle
    {

        /// <summary>  
        /// 对象转为json  
        /// </summary>  
        /// <typeparam name="ObjType"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string ObjToJsonString(object obj)
        {

            string s = JsonConvert.SerializeObject(obj);
            return s;
        }
        /// <summary>  
        /// json转为对象  
        /// </summary>  
        /// <typeparam name="ObjType"></typeparam>  
        /// <param name="JsonString"></param>  
        /// <returns></returns>  
        public static ObjType JsonStringToObj<ObjType>(string JsonString) where ObjType : class
        {
            ObjType s = JsonConvert.DeserializeObject<ObjType>(JsonString);
            return s;
        }
    }
}
