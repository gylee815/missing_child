using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projectclient
{
    public class Info
    {

        public string MemberNAME,MemberID, LDate ,Floor, Xposi, Yposi, Zposi;

        public Info()
        {
        }
        public Info(string NAME,string ID, string X, string Y, string F, string L)
        {
            MemberNAME = NAME;
            MemberID = ID;
            Xposi = X;
            Yposi = Y;
            Floor = F;
            LDate = L;
        }

        //public Info(string ID, string X, string Z, string F,string L)
        //{
        //    MemberID = ID;
        //    Xposi = X;
        //    Zposi = Z; 
        //    Floor = F;
        //    LDate = L;
        //}
    }
}
