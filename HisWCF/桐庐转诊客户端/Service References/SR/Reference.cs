﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 桐庐转诊客户端.SR {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SR.IHisApplay")]
    public interface IHisApplay {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/RunService", ReplyAction="http://tempuri.org/IHisApplay/RunServiceResponse")]
        桐庐转诊客户端.SR.RunServiceResponse RunService(桐庐转诊客户端.SR.RunServiceRequest request);
        
        // CODEGEN: 正在生成消息协定，应为该操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/RunService", ReplyAction="http://tempuri.org/IHisApplay/RunServiceResponse")]
        System.Threading.Tasks.Task<桐庐转诊客户端.SR.RunServiceResponse> RunServiceAsync(桐庐转诊客户端.SR.RunServiceRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/PCall", ReplyAction="http://tempuri.org/IHisApplay/PCallResponse")]
        System.DateTime PCall();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/PCall", ReplyAction="http://tempuri.org/IHisApplay/PCallResponse")]
        System.Threading.Tasks.Task<System.DateTime> PCallAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/sendToHis", ReplyAction="http://tempuri.org/IHisApplay/sendToHisResponse")]
        string sendToHis(string code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/sendToHis", ReplyAction="http://tempuri.org/IHisApplay/sendToHisResponse")]
        System.Threading.Tasks.Task<string> sendToHisAsync(string code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/DoBusiness", ReplyAction="http://tempuri.org/IHisApplay/DoBusinessResponse")]
        桐庐转诊客户端.SR.DoBusinessResponse DoBusiness(桐庐转诊客户端.SR.DoBusinessRequest request);
        
        // CODEGEN: 正在生成消息协定，应为该操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/DoBusiness", ReplyAction="http://tempuri.org/IHisApplay/DoBusinessResponse")]
        System.Threading.Tasks.Task<桐庐转诊客户端.SR.DoBusinessResponse> DoBusinessAsync(桐庐转诊客户端.SR.DoBusinessRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/Signature", ReplyAction="http://tempuri.org/IHisApplay/SignatureResponse")]
        桐庐转诊客户端.SR.SignatureResponse Signature(桐庐转诊客户端.SR.SignatureRequest request);
        
        // CODEGEN: 正在生成消息协定，应为该操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHisApplay/Signature", ReplyAction="http://tempuri.org/IHisApplay/SignatureResponse")]
        System.Threading.Tasks.Task<桐庐转诊客户端.SR.SignatureResponse> SignatureAsync(桐庐转诊客户端.SR.SignatureRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RunService", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RunServiceRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string TradeType;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string TradeMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string TradeMsgOut;
        
        public RunServiceRequest() {
        }
        
        public RunServiceRequest(string TradeType, string TradeMsg, string TradeMsgOut) {
            this.TradeType = TradeType;
            this.TradeMsg = TradeMsg;
            this.TradeMsgOut = TradeMsgOut;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RunServiceResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RunServiceResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public int RunServiceResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string TradeMsgOut;
        
        public RunServiceResponse() {
        }
        
        public RunServiceResponse(int RunServiceResult, string TradeMsgOut) {
            this.RunServiceResult = RunServiceResult;
            this.TradeMsgOut = TradeMsgOut;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DoBusiness", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class DoBusinessRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string header;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string body;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string ehrXml;
        
        public DoBusinessRequest() {
        }
        
        public DoBusinessRequest(string header, string body, string ehrXml) {
            this.header = header;
            this.body = body;
            this.ehrXml = ehrXml;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DoBusinessResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class DoBusinessResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string ehrXml;
        
        public DoBusinessResponse() {
        }
        
        public DoBusinessResponse(string ehrXml) {
            this.ehrXml = ehrXml;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Signature", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class SignatureRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string MAC;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string key;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string Erro;
        
        public SignatureRequest() {
        }
        
        public SignatureRequest(string MAC, string key, string Erro) {
            this.MAC = MAC;
            this.key = key;
            this.Erro = Erro;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SignatureResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class SignatureResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public int SignatureResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string key;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string Erro;
        
        public SignatureResponse() {
        }
        
        public SignatureResponse(int SignatureResult, string key, string Erro) {
            this.SignatureResult = SignatureResult;
            this.key = key;
            this.Erro = Erro;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IHisApplayChannel : 桐庐转诊客户端.SR.IHisApplay, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HisApplayClient : System.ServiceModel.ClientBase<桐庐转诊客户端.SR.IHisApplay>, 桐庐转诊客户端.SR.IHisApplay {
        
        public HisApplayClient() {
        }
        
        public HisApplayClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public HisApplayClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HisApplayClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HisApplayClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        桐庐转诊客户端.SR.RunServiceResponse 桐庐转诊客户端.SR.IHisApplay.RunService(桐庐转诊客户端.SR.RunServiceRequest request) {
            return base.Channel.RunService(request);
        }
        
        public int RunService(string TradeType, string TradeMsg, ref string TradeMsgOut) {
            桐庐转诊客户端.SR.RunServiceRequest inValue = new 桐庐转诊客户端.SR.RunServiceRequest();
            inValue.TradeType = TradeType;
            inValue.TradeMsg = TradeMsg;
            inValue.TradeMsgOut = TradeMsgOut;
            桐庐转诊客户端.SR.RunServiceResponse retVal = ((桐庐转诊客户端.SR.IHisApplay)(this)).RunService(inValue);
            TradeMsgOut = retVal.TradeMsgOut;
            return retVal.RunServiceResult;
        }
        
        public System.Threading.Tasks.Task<桐庐转诊客户端.SR.RunServiceResponse> RunServiceAsync(桐庐转诊客户端.SR.RunServiceRequest request) {
            return base.Channel.RunServiceAsync(request);
        }
        
        public System.DateTime PCall() {
            return base.Channel.PCall();
        }
        
        public System.Threading.Tasks.Task<System.DateTime> PCallAsync() {
            return base.Channel.PCallAsync();
        }
        
        public string sendToHis(string code) {
            return base.Channel.sendToHis(code);
        }
        
        public System.Threading.Tasks.Task<string> sendToHisAsync(string code) {
            return base.Channel.sendToHisAsync(code);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        桐庐转诊客户端.SR.DoBusinessResponse 桐庐转诊客户端.SR.IHisApplay.DoBusiness(桐庐转诊客户端.SR.DoBusinessRequest request) {
            return base.Channel.DoBusiness(request);
        }
        
        public void DoBusiness(string header, string body, ref string ehrXml) {
            桐庐转诊客户端.SR.DoBusinessRequest inValue = new 桐庐转诊客户端.SR.DoBusinessRequest();
            inValue.header = header;
            inValue.body = body;
            inValue.ehrXml = ehrXml;
            桐庐转诊客户端.SR.DoBusinessResponse retVal = ((桐庐转诊客户端.SR.IHisApplay)(this)).DoBusiness(inValue);
            ehrXml = retVal.ehrXml;
        }
        
        public System.Threading.Tasks.Task<桐庐转诊客户端.SR.DoBusinessResponse> DoBusinessAsync(桐庐转诊客户端.SR.DoBusinessRequest request) {
            return base.Channel.DoBusinessAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        桐庐转诊客户端.SR.SignatureResponse 桐庐转诊客户端.SR.IHisApplay.Signature(桐庐转诊客户端.SR.SignatureRequest request) {
            return base.Channel.Signature(request);
        }
        
        public int Signature(string MAC, ref string key, ref string Erro) {
            桐庐转诊客户端.SR.SignatureRequest inValue = new 桐庐转诊客户端.SR.SignatureRequest();
            inValue.MAC = MAC;
            inValue.key = key;
            inValue.Erro = Erro;
            桐庐转诊客户端.SR.SignatureResponse retVal = ((桐庐转诊客户端.SR.IHisApplay)(this)).Signature(inValue);
            key = retVal.key;
            Erro = retVal.Erro;
            return retVal.SignatureResult;
        }
        
        public System.Threading.Tasks.Task<桐庐转诊客户端.SR.SignatureResponse> SignatureAsync(桐庐转诊客户端.SR.SignatureRequest request) {
            return base.Channel.SignatureAsync(request);
        }
    }
}
