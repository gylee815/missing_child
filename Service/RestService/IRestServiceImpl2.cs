using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RestService
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IRestServiceImpl2"을 변경할 수 있습니다.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IRestServiceImpl2Callback))]
    public interface IRestServiceImpl2
    {
        [OperationContract(IsOneWay = false)]
        String[] WhoisLogon(String tf);

        [OperationContract(IsOneWay = false)]
        String[] MyPosition(String id);
    }

    public interface IRestServiceImpl2Callback
    {
        [OperationContract(IsOneWay = true)]
        void ViewXmlID(string[] Info);
    }

}
