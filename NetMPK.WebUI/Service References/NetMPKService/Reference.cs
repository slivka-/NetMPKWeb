﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetMPK.WebUI.NetMPKService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NetMPKService.IMPKService")]
    public interface IMPKService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopsNames", ReplyAction="http://tempuri.org/IMPKService/GetStopsNamesResponse")]
        System.Collections.Generic.List<string> GetStopsNames();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopsNames", ReplyAction="http://tempuri.org/IMPKService/GetStopsNamesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetStopsNamesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopByName", ReplyAction="http://tempuri.org/IMPKService/GetStopByNameResponse")]
        System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>> GetStopByName(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopByName", ReplyAction="http://tempuri.org/IMPKService/GetStopByNameResponse")]
        System.Threading.Tasks.Task<System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>>> GetStopByNameAsync(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetAllLines", ReplyAction="http://tempuri.org/IMPKService/GetAllLinesResponse")]
        System.Collections.Generic.List<System.Tuple<int, string, string, string, string>> GetAllLines();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetAllLines", ReplyAction="http://tempuri.org/IMPKService/GetAllLinesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<int, string, string, string, string>>> GetAllLinesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetLineRoutes", ReplyAction="http://tempuri.org/IMPKService/GetLineRoutesResponse")]
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> GetLineRoutes(int lineNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetLineRoutes", ReplyAction="http://tempuri.org/IMPKService/GetLineRoutesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> GetLineRoutesAsync(int lineNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetDirectionsForLine", ReplyAction="http://tempuri.org/IMPKService/GetDirectionsForLineResponse")]
        System.Collections.Generic.List<string> GetDirectionsForLine(int lineNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetDirectionsForLine", ReplyAction="http://tempuri.org/IMPKService/GetDirectionsForLineResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetDirectionsForLineAsync(int lineNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStreetNameByStop", ReplyAction="http://tempuri.org/IMPKService/GetStreetNameByStopResponse")]
        string GetStreetNameByStop(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStreetNameByStop", ReplyAction="http://tempuri.org/IMPKService/GetStreetNameByStopResponse")]
        System.Threading.Tasks.Task<string> GetStreetNameByStopAsync(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetTimeTable", ReplyAction="http://tempuri.org/IMPKService/GetTimeTableResponse")]
        System.Collections.Generic.List<System.Collections.Generic.List<string>> GetTimeTable(int lineNo, string stopName, string direction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetTimeTable", ReplyAction="http://tempuri.org/IMPKService/GetTimeTableResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<string>>> GetTimeTableAsync(int lineNo, string stopName, string direction);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMPKServiceChannel : NetMPK.WebUI.NetMPKService.IMPKService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MPKServiceClient : System.ServiceModel.ClientBase<NetMPK.WebUI.NetMPKService.IMPKService>, NetMPK.WebUI.NetMPKService.IMPKService {
        
        public MPKServiceClient() {
        }
        
        public MPKServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MPKServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MPKServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MPKServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<string> GetStopsNames() {
            return base.Channel.GetStopsNames();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetStopsNamesAsync() {
            return base.Channel.GetStopsNamesAsync();
        }
        
        public System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>> GetStopByName(string stopName) {
            return base.Channel.GetStopByName(stopName);
        }
        
        public System.Threading.Tasks.Task<System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>>> GetStopByNameAsync(string stopName) {
            return base.Channel.GetStopByNameAsync(stopName);
        }
        
        public System.Collections.Generic.List<System.Tuple<int, string, string, string, string>> GetAllLines() {
            return base.Channel.GetAllLines();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<int, string, string, string, string>>> GetAllLinesAsync() {
            return base.Channel.GetAllLinesAsync();
        }
        
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> GetLineRoutes(int lineNo) {
            return base.Channel.GetLineRoutes(lineNo);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> GetLineRoutesAsync(int lineNo) {
            return base.Channel.GetLineRoutesAsync(lineNo);
        }
        
        public System.Collections.Generic.List<string> GetDirectionsForLine(int lineNo) {
            return base.Channel.GetDirectionsForLine(lineNo);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetDirectionsForLineAsync(int lineNo) {
            return base.Channel.GetDirectionsForLineAsync(lineNo);
        }
        
        public string GetStreetNameByStop(string stopName) {
            return base.Channel.GetStreetNameByStop(stopName);
        }
        
        public System.Threading.Tasks.Task<string> GetStreetNameByStopAsync(string stopName) {
            return base.Channel.GetStreetNameByStopAsync(stopName);
        }
        
        public System.Collections.Generic.List<System.Collections.Generic.List<string>> GetTimeTable(int lineNo, string stopName, string direction) {
            return base.Channel.GetTimeTable(lineNo, stopName, direction);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<string>>> GetTimeTableAsync(int lineNo, string stopName, string direction) {
            return base.Channel.GetTimeTableAsync(lineNo, stopName, direction);
        }
    }
}