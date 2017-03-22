using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HisWCFSVR
{
    [ServiceContract]
    public interface IHisApplay
    {
        [OperationContract]
        int RunService(string TradeType, string TradeMsg, ref string TradeMsgOut);
        [OperationContract]
        DateTime PCall();

        /// <summary>
        /// 北京华康医院药房自动化系统接口
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        string sendToHis(string code);

        /// <summary>
        /// 转诊预约接口
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <param name="ehrXml"></param>
        [OperationContract]
        void DoBusiness(string header, string body, ref string ehrXml);

        /// <summary>
        /// 注册获取交易安全校验码
        /// </summary>
        /// <param name="MAC"></param>
        /// <param name="key"></param>
        /// <param name="Erro"></param>
        [OperationContract]
        int Signature(string MAC, ref string key, ref string Erro);

    }
}
