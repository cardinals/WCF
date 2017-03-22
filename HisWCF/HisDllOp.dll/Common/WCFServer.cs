using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Reflection;
using System.Xml;
using MEDI.SIIM.SelfServiceWeb.Entity;
using System.Configuration;
using System.Text;
using System.Runtime.InteropServices;
using HisDllOp;

namespace MEDI.SIIM.SelfServiceWeb
{

    [ServiceContract]
    public interface IHisApplay
    {
        [OperationContract]
        int RunService(string TradeType, string TradeMsg, ref string TradeMsgOut);
        [OperationContract]
        DateTime PCall();
    }

    public class WCFServer
    {
        static string WCFaddr = string.Empty;
        static string Method = string.Empty;
        static Dictionary<string, string> MethodMappings = new Dictionary<string, string>();
        static WCFServer()
        {
            string inii = AppDomain.CurrentDomain.BaseDirectory + "webconfig.ini";
            INIClass ini = new INIClass(inii);
            var wcf = ini.IniReadValue("服务平台地址", "ServiceAdd");
            var met = ini.IniReadValue("服务平台主方法", "Method");

            WCFaddr = wcf.ToString();//ini.IniReadValue(""); 
            //"http://localhost/wacf//MediInfoHis.svc";//ConfigurationManager.AppSettings["WCFaddr"].ToString();
            Method = met.ToString();
            //"RunService";//ConfigurationManager.AppSettings["Method"].ToString();
            //if (MethodMappings == null)
            //{
            //MethodMappings = (Dictionary<string, string>)ConfigurationManager.GetSection("MethodMapping");
            MethodMappings.Add("HIS1.Biz.RENYUANXX", "RENYUANXX_IN");
            MethodMappings.Add("HIS1.Biz.RENYUANZC", "RENYUANZC_IN");
            MethodMappings.Add("HIS1.Biz.GUAHAOKSXX", "GUAHAOKSXX_IN");
            MethodMappings.Add("HIS1.Biz.GUAHAOYSXX", "GUAHAOYSXX_IN");
            MethodMappings.Add("HIS1.Biz.GUAHAOCL", "GUAHAOCL_IN");
            MethodMappings.Add("HIS1.Biz.MENZHENYJS", "MENZHENYJS_IN");
            MethodMappings.Add("HIS1.Biz.GUAHAOTH", "GUAHAOTH_IN");
            MethodMappings.Add("HIS1.Biz.MENZHENCFJL", "MENZHENCFJL_IN");
            MethodMappings.Add("HIS1.Biz.MENZHENFYMX", "MENZHENFYMX_IN");


            // }
        }

        public static OUT Call<IN, OUT>(IN entity)
            where IN : BaseInEntity, new()
            where OUT : BaseOutEntity, new()
        {
            foreach (var map in MethodMappings)
            {
                if (map.Value == typeof(IN).Name)
                {
                    var Out = XMLServer.XMLtoEntity<OUT>(Call(WCFaddr, Method, new string[] { map.Key, XMLServer.EntitytoXML<IN>(entity) }));
                    //if (Out.OUTMSG.ERRNO != "0")
                    //{
                    //    throw new Exception(Out.OUTMSG.ERRMSG);
                    //}
                    //else
                    //{
                    return Out;
                    //  }
                }
            }
            throw new Exception("没有设置WebConfig.MethodMapping的节点信息!");
        }

        public static string Call(string url, string methodName, params string[] args)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.ReaderQuotas = new XmlDictionaryReaderQuotas() { MaxStringContentLength = 2147483647 };
            EndpointAddress endpoint = new EndpointAddress(url);
            using (ChannelFactory<IHisApplay> channelFactory = new ChannelFactory<IHisApplay>(binding, endpoint))
            {
                IHisApplay instance = channelFactory.CreateChannel();
                using (instance as IDisposable)
                {
                    try
                    {
                        string TradeOut = string.Empty;
                        instance.RunService(args[0], args[1], ref TradeOut);
                        return TradeOut;

                        //Type type = typeof(IHisApplay);
                        //MethodInfo mi = type.GetMethod(methodName);
                        //mi.Invoke(instance, args);
                        //return args[2].ToString();
                    }
                    catch (TimeoutException tErr)
                    {
                        (instance as ICommunicationObject).Abort();
                        throw tErr;
                    }
                    catch (CommunicationException cErr)
                    {
                        (instance as ICommunicationObject).Abort();
                        throw cErr;
                    }
                    catch (Exception vErr)
                    {
                        (instance as ICommunicationObject).Abort();
                        throw vErr;
                    }
                }
            }
        }
        /// <summary>
        /// ping通网络测试
        /// </summary>
        /// <returns></returns>
        public static void PingCall(ref System.DateTime TradeMsgOut)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.ReaderQuotas = new XmlDictionaryReaderQuotas() { MaxStringContentLength = 2147483647 };
            EndpointAddress endpoint = new EndpointAddress(WCFaddr);
            using (ChannelFactory<IHisApplay> channelFactory = new ChannelFactory<IHisApplay>(binding, endpoint))
            {
                IHisApplay instance = channelFactory.CreateChannel();
                using (instance as IDisposable)
                {
                    try
                    {


                        TradeMsgOut = instance.PCall();
                        // return TradeMsgOut;
                    }
                    catch (TimeoutException tErr)
                    {
                        (instance as ICommunicationObject).Abort();
                        throw tErr;
                    }
                    catch (CommunicationException cErr)
                    {
                        (instance as ICommunicationObject).Abort();
                        throw cErr;
                    }
                    catch (Exception vErr)
                    {
                        (instance as ICommunicationObject).Abort();
                        throw vErr;
                    }
                }
            }
        }
    }

}