using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RestService
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IService1"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        String[] WhoisLogon(String tf);

        [OperationContract]
        String[] relationmember(String tf);

        [OperationContract]
        String[] MyPosition(String id);

        [OperationContract]
        String[] TimeToXml(DateTime start, DateTime end);

        [OperationContract]
        String[] MissingLogon(string info);

        [OperationContract]
        void Missingmember(String[] name);

        [OperationContract]
        String[] LoadDayTo(DateTime st, DateTime la);

        [OperationContract]
        void admininsert(string name, string id, string password, string phone);

        [OperationContract]
        String[] adminselect(string id);

        [OperationContract]
        void adminupdate(string id, string name, string password, string phone);

        [OperationContract]
        void admindelete(string id);

        [OperationContract]
        bool adminidcheck(string id);

        [OperationContract]
        bool Insertmember(String id, String pwd, String name, String age, String phone, String loginfo, String sex);

        [OperationContract]
        bool returnmem();

        [OperationContract]
        String returnnamejpg();

        [OperationContract]
        void Logoutmember(String name);
    }
}
