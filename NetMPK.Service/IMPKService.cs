using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web.Services.Protocols;
using System.Windows;

namespace NetMPK.Service
{
    [ServiceContract]
    public interface IMPKService
    {
        #region Default
        /*
        [OperationContract]
        string GetData(string value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        */
        #endregion

        #region Stops
        [SoapHeader("Authentication")]   
        [OperationContract]
        List<string> GetStopsNames();

        [OperationContract]
        Dictionary<string, string> GetStopsWithStreets();

        [OperationContract]
        Tuple<int, string, string, double, double, List<int>> GetStopByName(string stopName);

        [OperationContract]
        List<Tuple<string, double, double>> GetStopWithCords();
        #endregion
   
        #region Lines
        [OperationContract]
        List<Tuple<int, string, string, string, string>> GetAllLines();

        [OperationContract]
        Dictionary<string, List<string>> GetLineRoutes(int lineNo);

        [OperationContract]
        List<string> GetDirectionsForLine(int lineNo, string stopName);
        #endregion

        #region Streets
        [OperationContract]
        string GetStreetNameByStop(string stopName);
        #endregion
        
        #region Timetables
        [OperationContract]
        List<List<string>> GetTimeTable(int lineNo, string stopName, string direction);
        #endregion

        #region Routes

        [OperationContract]
        List<List<Tuple<int, string, string, string, string, int>>> GetRoutes(string startName, string stopName);

        #endregion

        #region MapDrawing

        [OperationContract]
        Tuple<Dictionary<string, Vector>, Dictionary<string, List<string>>, List<Tuple<Vector, Vector>>> GetMapPoints();

        [OperationContract]
        List<string> GetPointNeighbours(string stopName);

        #endregion
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "NetMPK.Service.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
