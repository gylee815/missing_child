using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RestService
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "RestServiceImpl2"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 RestServiceImpl2.svc나 RestServiceImpl2.svc.cs를 선택하고 디버깅을 시작하십시오.
     [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RestServiceImpl2 : IRestServiceImpl2
    {
        IRestServiceImpl2Callback callback;
        DataAccess data;

        public RestServiceImpl2()
        {
            callback = OperationContext.Current.GetCallbackChannel<IRestServiceImpl2Callback>();
        }

        public String[] WhoisLogon(String tf)
        {
            data = new DataAccess();
            data.connect();
            String[] loginmemb = data.LoginMember_Manager(tf);
            data.disconnect();
            return loginmemb;
        }

        public String[] MyPosition(String id)
        {
            data = new DataAccess();
            data.connect();
            String[] loginposition = data.LoginMember_Position(id);
            data.disconnect();
            return loginposition;
        }
    }
}
