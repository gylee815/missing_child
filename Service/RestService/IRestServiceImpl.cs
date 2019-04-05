using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;
using System;

namespace RestService
{

    [ServiceContract]
    public interface IRestServiceImpl
    {    
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Login/{ID}/{PW}")]
        List<Memberinfo> Member_check(string ID, string PW);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Logout/{ID}")]
        String Logout(String id);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Position/{ID}/{OldX}/{OldY}/{Direction}/{Rssi1}/{Rssi2}/{Rssi3}/{Rssi4}/{Rssi5}/{Floor}")]
        Positioninfo Positioning(String ID, String OldX, String OldY, String Direction, String Rssi1, String Rssi2, String Rssi3, String Rssi4, String Rssi5, String Floor);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/NoMovePosition/{ID}/{OldX}/{OldY}/{Rssi1}/{Rssi2}/{Rssi3}/{Rssi4}/{Rssi5}/{Floor}")]
        Positioninfo NoMovePositioning(String ID, String OldX, String OldY, String Rssi1, String Rssi2, String Rssi3, String Rssi4, String Rssi5, String Floor);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Gyro/{ID}/{revision_X}/{revision_Y}/{revision_Z}")]
        String GyroUpdate(String ID, String revision_X, String revision_Y, String revision_Z);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Missing/{ID}")]
        String MissingUpdate_On(String ID);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/MissingOff/{ID}")]
        String MissingUpdate_Off(String ID);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/MCD/{ID}/{LimitX}/{LimitY}")]
        String MCDUpdate(String ID, String LimitX, String LimitY);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/RelaSelect/{ID}/{ID2}/{RELATION}")]
        String RelaSelect(String ID, String ID2, String RELATION);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/RelaName2/{ID2}")]
        String Rela_NAME2_Select(String ID2);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/RelaUpdate/{ID}/{ID2}/{NAME2}/{RELATION}/{NAME}")]
        String RelaUpdate(String ID, String ID2, String NAME2, String RELATION, String NAME);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Request/{ID}")]
        String Request(String ID);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Accept/{ID}/{ID2}")]
        String Accept(String ID, String ID2);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Read_ID2/{ID}")]
        String Read_ID2(String ID);

        #region "Regist"
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Insert/{ID}/{PW}/{NAME}/{AGE}/{PHONE}/{LOGINFO}/{SEX}")]
        string insertMember(String ID, String PW, String NAME, String AGE, String PHONE, String LOGINFO/*,  String NAME2, String RELATION*/, String SEX);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Idcheck/{ID}")]
        String isDuplicated(String ID);
        #endregion
    }
    
    [DataContract]
    public class Memberinfo
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string NAME { get; set; }
        [DataMember]
        public string GYRO_X { get; set; }
        [DataMember]
        public string GYRO_Y { get; set; }
        [DataMember]
        public string GYRO_Z { get; set; }
        [DataMember]
        public string RELATION { get; set; }
        [DataMember]
        public string MCDX { get; set; }
        [DataMember]
        public string MCDY { get; set; }
    }

    [DataContract]
    public class Positioninfo
    {
        [DataMember]
        public string Position_X { get; set; }
        [DataMember]
        public string Position_Y { get; set; }
        [DataMember]
        public string Position_X2 { get; set; }
        [DataMember]
        public string Position_Y2 { get; set; }
        [DataMember]
        public string PositionMove_X { get; set; }
        [DataMember]
        public string PositionMove_Y { get; set; }
    }
    
    //[DataContract]
    //public class RelationSelect
    //{
    //    [DataMember]
    //    public string IDselect { get; set; }
    //}

}