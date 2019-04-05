using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RestService
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 Service1.svc나 Service1.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class Service1 : IService1
    {
        Mydb db = new Mydb();

        public Service1()
        {
        }
        //전체출력
        public String[] WhoisLogon(String tf)
        {
            db.Open();
            String[] loginmemb = db.LoginMember_Manager(tf);
            db.Close();
            return loginmemb;
        }

        public String[] relationmember(String tf)
        {
            db.Open();
            String[] loginmemb = db.RelationeMember_Manager(tf);
            db.Close();
            return loginmemb;
        }

        public String[] MyPosition(String id)
        {
            db.Open();
            String[] loginposition = db.SelectAllPosition(id);
            db.Close();
            return loginposition;
        }
        //검색
        public String[] TimeToXml(DateTime start, DateTime end)
        {
            db.Open();
            String[] loginposition = db.SelectPeople(start, end);
            db.Close();
            return loginposition;
        }

        public String[] MissingLogon(string info)
        {
            db.Open();
            String[] missingmemb = db.MissingMember_Manager(info);
            db.Close();
            return missingmemb;
        }

        public void Missingmember(String[] name)
        {
            db.Open();
            db.MissingMember_missing(name);
            db.Close();
        }

        public String[] LoadDayTo(DateTime st, DateTime la)
        {
            db.Open();
            String[] logindate = db.LoadDay(st, la);
            db.Close();
            return logindate;
        }

        public void admininsert(string name, string id, string password, string phone)
        {
            db.Open();
            db.adminInsert(name, id, password, phone);
            db.Close();
        }

        public String[] adminselect(string id)
        {
            db.Open();
            String[] select = db.adminSelect(id);
            db.Close();
            return select;
        }

        public void adminupdate(string id, string name, string password, string phone)
        {
            db.Open();
            db.adminUpdate(id, name, password, phone);
            db.Close();
        }

        public void admindelete(string id)
        {
            db.Open();
            db.adminDelete(id);
            db.Close();
        }

        public bool adminidcheck(string id)
        {
            db.Open();
            db.Close();

            return db.adminIDCheck(id);

        }

        public bool Insertmember(String id, String pwd, String name, String age, String phone, String loginfo, String sex)
        {
            db.Open();
            bool result = db.InsertMember(id, pwd, name, age, phone, loginfo, sex);
            db.Close();
            return result;
        }
        public bool returnmem()
        {
            db.Open();
            bool ch = db.returnmem();
            db.Close();
            return ch;
        }
        public String returnnamejpg()
        {
            db.Open();
            String returnnamejpg = db.returnnamejpg();
            db.Close();
            return returnnamejpg;
        }
        public void Logoutmember(String name)
        {
            db.Open();
            db.Logoutmember(name);
            db.Close();
        }
    }
}
