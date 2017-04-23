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
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopsWithStreets", ReplyAction="http://tempuri.org/IMPKService/GetStopsWithStreetsResponse")]
        System.Collections.Generic.Dictionary<string, string> GetStopsWithStreets();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopsWithStreets", ReplyAction="http://tempuri.org/IMPKService/GetStopsWithStreetsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, string>> GetStopsWithStreetsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopByName", ReplyAction="http://tempuri.org/IMPKService/GetStopByNameResponse")]
        System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>> GetStopByName(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopByName", ReplyAction="http://tempuri.org/IMPKService/GetStopByNameResponse")]
        System.Threading.Tasks.Task<System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>>> GetStopByNameAsync(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopWithCords", ReplyAction="http://tempuri.org/IMPKService/GetStopWithCordsResponse")]
        System.Collections.Generic.List<System.Tuple<string, double, double>> GetStopWithCords();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStopWithCords", ReplyAction="http://tempuri.org/IMPKService/GetStopWithCordsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<string, double, double>>> GetStopWithCordsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetAllLines", ReplyAction="http://tempuri.org/IMPKService/GetAllLinesResponse")]
        System.Collections.Generic.List<System.Tuple<int, string, string, string, string>> GetAllLines();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetAllLines", ReplyAction="http://tempuri.org/IMPKService/GetAllLinesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<int, string, string, string, string>>> GetAllLinesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetLineRoutes", ReplyAction="http://tempuri.org/IMPKService/GetLineRoutesResponse")]
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> GetLineRoutes(int lineNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetLineRoutes", ReplyAction="http://tempuri.org/IMPKService/GetLineRoutesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> GetLineRoutesAsync(int lineNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetDirectionsForLine", ReplyAction="http://tempuri.org/IMPKService/GetDirectionsForLineResponse")]
        System.Collections.Generic.List<string> GetDirectionsForLine(int lineNo, string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetDirectionsForLine", ReplyAction="http://tempuri.org/IMPKService/GetDirectionsForLineResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetDirectionsForLineAsync(int lineNo, string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStreetNameByStop", ReplyAction="http://tempuri.org/IMPKService/GetStreetNameByStopResponse")]
        string GetStreetNameByStop(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetStreetNameByStop", ReplyAction="http://tempuri.org/IMPKService/GetStreetNameByStopResponse")]
        System.Threading.Tasks.Task<string> GetStreetNameByStopAsync(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetTimeTable", ReplyAction="http://tempuri.org/IMPKService/GetTimeTableResponse")]
        System.Collections.Generic.List<System.Collections.Generic.List<string>> GetTimeTable(int lineNo, string stopName, string direction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetTimeTable", ReplyAction="http://tempuri.org/IMPKService/GetTimeTableResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<string>>> GetTimeTableAsync(int lineNo, string stopName, string direction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetRoutes", ReplyAction="http://tempuri.org/IMPKService/GetRoutesResponse")]
        System.Collections.Generic.List<System.Collections.Generic.List<System.Tuple<int, string, string, string, string, int>>> GetRoutes(string startName, string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetRoutes", ReplyAction="http://tempuri.org/IMPKService/GetRoutesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<System.Tuple<int, string, string, string, string, int>>>> GetRoutesAsync(string startName, string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/SaveRouteForUser", ReplyAction="http://tempuri.org/IMPKService/SaveRouteForUserResponse")]
        bool SaveRouteForUser(string userId, string firstStop, string lastStop);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/SaveRouteForUser", ReplyAction="http://tempuri.org/IMPKService/SaveRouteForUserResponse")]
        System.Threading.Tasks.Task<bool> SaveRouteForUserAsync(string userId, string firstStop, string lastStop);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetSavedRoutesForUser", ReplyAction="http://tempuri.org/IMPKService/GetSavedRoutesForUserResponse")]
        System.Collections.Generic.List<System.Tuple<string, string>> GetSavedRoutesForUser(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetSavedRoutesForUser", ReplyAction="http://tempuri.org/IMPKService/GetSavedRoutesForUserResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<string, string>>> GetSavedRoutesForUserAsync(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetMapPoints", ReplyAction="http://tempuri.org/IMPKService/GetMapPointsResponse")]
        System.Tuple<System.Collections.Generic.Dictionary<string, System.Windows.Vector>, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>, System.Collections.Generic.List<System.Tuple<System.Windows.Vector, System.Windows.Vector>>> GetMapPoints();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetMapPoints", ReplyAction="http://tempuri.org/IMPKService/GetMapPointsResponse")]
        System.Threading.Tasks.Task<System.Tuple<System.Collections.Generic.Dictionary<string, System.Windows.Vector>, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>, System.Collections.Generic.List<System.Tuple<System.Windows.Vector, System.Windows.Vector>>>> GetMapPointsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetPointNeighbours", ReplyAction="http://tempuri.org/IMPKService/GetPointNeighboursResponse")]
        System.Collections.Generic.List<string> GetPointNeighbours(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/GetPointNeighbours", ReplyAction="http://tempuri.org/IMPKService/GetPointNeighboursResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetPointNeighboursAsync(string stopName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/LoginFree", ReplyAction="http://tempuri.org/IMPKService/LoginFreeResponse")]
        bool LoginFree(string login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/LoginFree", ReplyAction="http://tempuri.org/IMPKService/LoginFreeResponse")]
        System.Threading.Tasks.Task<bool> LoginFreeAsync(string login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/EmailFree", ReplyAction="http://tempuri.org/IMPKService/EmailFreeResponse")]
        bool EmailFree(string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/EmailFree", ReplyAction="http://tempuri.org/IMPKService/EmailFreeResponse")]
        System.Threading.Tasks.Task<bool> EmailFreeAsync(string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/RegisterUser", ReplyAction="http://tempuri.org/IMPKService/RegisterUserResponse")]
        bool RegisterUser(string login, string password, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/RegisterUser", ReplyAction="http://tempuri.org/IMPKService/RegisterUserResponse")]
        System.Threading.Tasks.Task<bool> RegisterUserAsync(string login, string password, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/LoginUser", ReplyAction="http://tempuri.org/IMPKService/LoginUserResponse")]
        System.Tuple<bool, string> LoginUser(string login, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMPKService/LoginUser", ReplyAction="http://tempuri.org/IMPKService/LoginUserResponse")]
        System.Threading.Tasks.Task<System.Tuple<bool, string>> LoginUserAsync(string login, string password);
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
        
        public System.Collections.Generic.Dictionary<string, string> GetStopsWithStreets() {
            return base.Channel.GetStopsWithStreets();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, string>> GetStopsWithStreetsAsync() {
            return base.Channel.GetStopsWithStreetsAsync();
        }
        
        public System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>> GetStopByName(string stopName) {
            return base.Channel.GetStopByName(stopName);
        }
        
        public System.Threading.Tasks.Task<System.Tuple<int, string, string, double, double, System.Collections.Generic.List<int>>> GetStopByNameAsync(string stopName) {
            return base.Channel.GetStopByNameAsync(stopName);
        }
        
        public System.Collections.Generic.List<System.Tuple<string, double, double>> GetStopWithCords() {
            return base.Channel.GetStopWithCords();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<string, double, double>>> GetStopWithCordsAsync() {
            return base.Channel.GetStopWithCordsAsync();
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
        
        public System.Collections.Generic.List<string> GetDirectionsForLine(int lineNo, string stopName) {
            return base.Channel.GetDirectionsForLine(lineNo, stopName);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetDirectionsForLineAsync(int lineNo, string stopName) {
            return base.Channel.GetDirectionsForLineAsync(lineNo, stopName);
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
        
        public System.Collections.Generic.List<System.Collections.Generic.List<System.Tuple<int, string, string, string, string, int>>> GetRoutes(string startName, string stopName) {
            return base.Channel.GetRoutes(startName, stopName);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<System.Tuple<int, string, string, string, string, int>>>> GetRoutesAsync(string startName, string stopName) {
            return base.Channel.GetRoutesAsync(startName, stopName);
        }
        
        public bool SaveRouteForUser(string userId, string firstStop, string lastStop) {
            return base.Channel.SaveRouteForUser(userId, firstStop, lastStop);
        }
        
        public System.Threading.Tasks.Task<bool> SaveRouteForUserAsync(string userId, string firstStop, string lastStop) {
            return base.Channel.SaveRouteForUserAsync(userId, firstStop, lastStop);
        }
        
        public System.Collections.Generic.List<System.Tuple<string, string>> GetSavedRoutesForUser(int userId) {
            return base.Channel.GetSavedRoutesForUser(userId);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<System.Tuple<string, string>>> GetSavedRoutesForUserAsync(int userId) {
            return base.Channel.GetSavedRoutesForUserAsync(userId);
        }
        
        public System.Tuple<System.Collections.Generic.Dictionary<string, System.Windows.Vector>, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>, System.Collections.Generic.List<System.Tuple<System.Windows.Vector, System.Windows.Vector>>> GetMapPoints() {
            return base.Channel.GetMapPoints();
        }
        
        public System.Threading.Tasks.Task<System.Tuple<System.Collections.Generic.Dictionary<string, System.Windows.Vector>, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>, System.Collections.Generic.List<System.Tuple<System.Windows.Vector, System.Windows.Vector>>>> GetMapPointsAsync() {
            return base.Channel.GetMapPointsAsync();
        }
        
        public System.Collections.Generic.List<string> GetPointNeighbours(string stopName) {
            return base.Channel.GetPointNeighbours(stopName);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetPointNeighboursAsync(string stopName) {
            return base.Channel.GetPointNeighboursAsync(stopName);
        }
        
        public bool LoginFree(string login) {
            return base.Channel.LoginFree(login);
        }
        
        public System.Threading.Tasks.Task<bool> LoginFreeAsync(string login) {
            return base.Channel.LoginFreeAsync(login);
        }
        
        public bool EmailFree(string email) {
            return base.Channel.EmailFree(email);
        }
        
        public System.Threading.Tasks.Task<bool> EmailFreeAsync(string email) {
            return base.Channel.EmailFreeAsync(email);
        }
        
        public bool RegisterUser(string login, string password, string email) {
            return base.Channel.RegisterUser(login, password, email);
        }
        
        public System.Threading.Tasks.Task<bool> RegisterUserAsync(string login, string password, string email) {
            return base.Channel.RegisterUserAsync(login, password, email);
        }
        
        public System.Tuple<bool, string> LoginUser(string login, string password) {
            return base.Channel.LoginUser(login, password);
        }
        
        public System.Threading.Tasks.Task<System.Tuple<bool, string>> LoginUserAsync(string login, string password) {
            return base.Channel.LoginUserAsync(login, password);
        }
    }
}
