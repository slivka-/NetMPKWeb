using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

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
        [OperationContract]
        List<string> GetStopsNames();

        [OperationContract]
        Dictionary<string, string> GetStopsWithStreets();

        [OperationContract]
        Tuple<int, string, string, double, double, IEnumerable<int>> GetStopByName(string stopName);
        #endregion

        #region Lines
        [OperationContract]
        List<Tuple<int, string, string, string, string>> GetAllLines();

        [OperationContract]
        Dictionary<string, List<string>> GetLineRoutes(int lineNo);

        [OperationContract]
        List<string> GetDirectionsForLine(int lineNo);
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

        // TODO: Add your service operations here
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
