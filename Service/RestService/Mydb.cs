using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RestService
{
    public class Mydb
    {
        private static SqlConnection scon;
        private SqlCommand scom;
        DateTime date;
        int[] coordinatedata = new int[3]; //X,Y 좌표 받아올 배열 선언
        int num = 0;
        private DataTable dt;
        string s_name;
        string truename;
        bool check = false;

        public Mydb()
        {
            //this.Open();
        }
        // DB연결
        public void Open()
        {
            string constr = @"Data Source=servercom-PC\SQLEXPRESS;Initial Catalog=awp; User ID=SC; Password=2486";
            scon = new SqlConnection(constr);
            scon.Open();
        }
        // DB연결해제
        public void Close()
        {
            scon.Close();
        }
        #region 기연오빠소스(클라이언트 사용 X).
        public DataTable select(String ID, String PW)
        {
            DataTable dt = new DataTable();
            string comtxt = string.Format("Select * From MyService where ID = '{0}' AND PASSWORD = '{1}'", ID, PW);
            scom = new SqlCommand(comtxt, scon);
            try
            {
                scom.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Login(String ID)
        {
            string comtxt = string.Format("Update MyService Set Loginfo = '{0}' where ID = '{1}'", "1", ID);
            scom = new SqlCommand(comtxt, scon);
            scom.ExecuteNonQuery();
        }

        public void Logout(String ID)
        {
            string comtxt = string.Format("Update MyService Set Loginfo = '{0}' where ID = '{1}'", "0", ID);
            scom = new SqlCommand(comtxt, scon);
            scom.ExecuteNonQuery();
        }

        public void InsertMyPosition(String id, DateTime date, int x, int y, String Floor)
        {
            string comtxt = string.Format("insert into MyService_info (ID, DATE, X, Y, FLOOR) values(@ID,@DATE,@X,@Y,@FLOOR)");
            scom = new SqlCommand(comtxt, scon);
            scom.Parameters.AddWithValue("@ID", id);
            scom.Parameters.AddWithValue("@DATE", date);
            scom.Parameters.AddWithValue("@X", x);
            scom.Parameters.AddWithValue("@Y", y);
            scom.Parameters.AddWithValue("@FLOOR", Floor);
            scom.ExecuteNonQuery();
        }

        public DataTable MyPosition(String id, String x, String y)
        {
            DataTable dt = new DataTable();
            string comtxt = string.Format("Select X,Y From MyService_info where ID = '{0}'", id);
            scom = new SqlCommand(comtxt, scon);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            da.Fill(dt);

            return dt;
        }
        #endregion

        #region 전체출력
        public String[] LoginMember_Manager(String tf)
        {
            try
            {
                dt = new DataTable();
                string comtxt = string.Format("SELECT  * FROM MEMBER3");
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                string Filter = "Loginfo=" + tf;
                DataRow[] Row = dt.Select(Filter);
                String[] SendLoginMember = new String[Row.Length];

                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = (String)Row[i]["ID"] + "#" + (String)Row[i]["NAME"] + "#" + (String)Row[i]["PHONE"];// +"#" + (String)Row[i]["RELATION"];
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }
        public String[] RelationeMember_Manager(String tf)
        {
            try
            {
                dt = new DataTable();
                string comtxt = string.Format("SELECT  ID, NAME,  PHONE,  ID2, NAME2, LOGINFO FROM MEMBER3 WHERE  (ID IN (SELECT  ID2 FROM MEMBER3 AS MEMBER3_2 WHERE  (ID2 IN (SELECT  ID FROM     MEMBER3 AS MEMBER3_1 WHERE  (RELATION = 'C') AND (REQUEST = 1)))))");
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);
                string Filter = "Loginfo=" + tf;

                DataRow[] Row = dt.Select(Filter);
                String[] SendLoginMember = new String[Row.Length];

                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = (String)Row[i]["ID"] + "#" + (String)Row[i]["NAME"] + "#" + (String)Row[i]["PHONE"] + "#" + (String)Row[i]["ID2"] + "#" + (String)Row[i]["NAME2"];
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }
        public String[] SelectAllPosition(String id)
        {
            try
            {
                //String t = "1";
                dt = new DataTable();
                string comtxt = string.Format("SELECT  TimePosition.X, TimePosition.Y, TimePosition.FLOOR, MEMBER3.NAME, TimePosition.DATE FROM MEMBER3 INNER JOIN TimePosition ON MEMBER3.ID = TimePosition.ID WHERE  (MEMBER3.LOGINFO = '1') AND (TimePosition.DATE = (SELECT  MAX(DATE) AS Expr1 FROM     TimePosition AS TimePosition_1 WHERE  (ID = '{0}')))", id);
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                //string Filter = "Loginfo=" + t;
                DataRow[] Row = dt.Select();
                String[] SendLoginMember = new String[Row.Length];
                if (Row.Length == 0)
                    return null;
                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = (String)Row[i]["X"] + "#" + (String)Row[i]["Y"] + "#" + (String)Row[i]["FLOOR"] + "#" + (String)Row[i]["NAME"] + "#" + Row[i]["DATE"].ToString();
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }
        #endregion
        public String[] SelectPeople(DateTime start, DateTime end)
        {
            try
            {
                dt = new DataTable();
                string comtxt = string.Format("SELECT  MEMBER3.NAME, RealtimePosition.ID, RealtimePosition.X, RealtimePosition.Y, RealtimePosition.DATE, RealtimePosition.FLOOR FROM MEMBER3 INNER JOIN RealtimePosition ON MEMBER3.ID = RealtimePosition.ID WHERE(RealtimePosition.DATE BETWEEN '{0}' AND  '{1}')", start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"));
                //string comtxt = string.Format("SELECT  MEMBER3.NAME, RealtimePosition.ID, RealtimePosition.X, RealtimePosition.Y, RealtimePosition.DATE, RealtimePosition.FLOOR FROM MEMBER3 INNER JOIN RealtimePosition ON MEMBER3.ID = RealtimePosition.ID WHERE (RealtimePosition.DATE >= '{0}') AND (RealtimePosition.DATE <= '{1}')", start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"));
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                DataRow[] Row = dt.Select();
                String[] SendLoginMember = new String[Row.Length];
                SqlDataReader reader = scom.ExecuteReader();

                String[] data = new String[6];

                int i = 0;
                while (reader.Read())
                {
                    data[0] = (string)reader["NAME"];
                    data[1] = (string)reader["ID"];
                    data[2] = reader["X"].ToString();
                    data[3] = reader["Y"].ToString();
                    data[4] = reader["DATE"].ToString();
                    data[5] = reader["FLOOR"].ToString();
                    SendLoginMember[i] = data[0] + "#" + data[1] + "#" + data[2] + "#" + data[3] + "#" + data[4] + "#" + data[5];
                    i++;
                }

                reader.Close();
                scom.Dispose();

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }

        //#region 미아정보
        ////미아정보 리스트 뷰 띄울정보
        //public String[] M_ListView(String tf)
        //{
        //    try
        //    {
        //        dt = new DataTable();
        //        string comtxt = string.Format("SELECT  NAME, NAME2, PHONE,MISSING FROM  MEMBER3 WHERE (LOGINFO = 1) AND (RELATION = 'P') AND (MISSING = 1)");
        //        scom = new SqlCommand(comtxt, scon);
        //        SqlDataAdapter da = new SqlDataAdapter(scom);
        //        da.Fill(dt);

        //        string Filter = "MISSING=" + tf;
        //        DataRow[] Row = dt.Select(Filter);
        //        String[] SendLoginMember = new String[Row.Length];

        //        for (int i = 0; i < Row.Length; i++)
        //        {
        //            SendLoginMember[i] = (String)Row[i]["NAME"] + "#" + (String)Row[i]["NAME2"] + "#" + (String)Row[i]["PHONE"];
        //        }
        //        return SendLoginMember;
        //    }
        //    catch (Exception ex)
        //    {
        //        String msg = ex.Message;
        //        return null;
        //    }
        //}
        ////MISSING 0 으로 돌려놓기
        //public void returnmissing(String[] name22)
        //{
        //    for (int i = 0; i < name22.Length; i++)
        //    {
        //        string comtxt1 = string.Format("UPDATE MEMBER3 SET MISSING = 0 WHERE  (NAME = '{0}')", name22[i]);
        //        SqlCommand scom1 = new SqlCommand(comtxt1, scon);
        //        if (scom1.ExecuteNonQuery() == 1)
        //        {
        //            //    //MessageBox.Show("수정 완료");
        //        }
        //    }
        //}
        //#endregion

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public String[] MissingMember_Manager(string info)
        {
            try
            {
                DataTable dt = new DataTable();
                string comtxt = string.Format("SELECT NAME, NAME2, PHONE, MISSING FROM MEMBER3 WHERE(LOGINFO = 1) AND (RELATION = 'C')");
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                string Filter = "MISSING=" + info;
                DataRow[] Row = dt.Select(Filter);
                String[] SendLoginMember = new String[Row.Length];

                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = (string)Row[i]["NAME"] + "#" + (string)Row[i]["NAME2"] + "#" + (string)Row[i]["PHONE"];
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }

        public void MissingMember_missing(String[] name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                string comtxt = string.Format("UPDATE MEMBER3 SET MISSING = 0 WHERE (NAME = '{0}')", name[i]);
                scom = new SqlCommand(comtxt, scon);
                if (scom.ExecuteNonQuery() == 1)
                {
                    //    //MessageBox.Show("수정 완료");
                }
            }
        }

        public String[] LoadDay(DateTime st, DateTime la)
        {
            try
            {
                dt = new DataTable();
                string comtxt = string.Format("SELECT  MEMBER3.NAME, timePosition.ID, timePosition.X, timePosition.Y, timePosition.DATE, timePosition.FLOOR FROM MEMBER3 INNER JOIN timePosition ON MEMBER3.ID = timePosition.ID WHERE(timePosition.DATE BETWEEN '{0}' AND '{1}')", st.ToString("yyyy-MM-dd HH:mm:ss"), la.ToString("yyyy-MM-dd HH:mm:ss"));
                //string comtxt = string.Format("SELECT  MEMBER3.NAME, RealtimePosition.ID, RealtimePosition.X, RealtimePosition.Y, RealtimePosition.DATE, RealtimePosition.FLOOR FROM MEMBER3 INNER JOIN RealtimePosition ON MEMBER3.ID = RealtimePosition.ID WHERE (RealtimePosition.DATE >= '{0}') AND (RealtimePosition.DATE <= '{1}')", st.ToString("yyyy-MM-dd HH:mm:ss"), la.ToString("yyyy-MM-dd HH:mm:ss"));
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                DataRow[] Row = dt.Select();
                String[] SendLoadDay = new String[Row.Length];

                SqlDataReader reader = scom.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    string[] data = new string[6];
                    data[0] = (string)reader["NAME"];
                    data[1] = (string)reader["ID"];
                    data[2] = (string)reader["X"];
                    data[3] = (string)reader["Y"];
                    data[4] = reader["DATE"].ToString();
                    data[5] = (string)reader["FLOOR"];
                    //(string)reader["NAME"] + "#" + (string)reader["ID"] + "#" + reader["X"].ToString() + "#" + reader["Y"].ToString() + "#" + (string)reader["FLOOR"];
                    SendLoadDay[i] = data[0] + "#" + data[1] + "#" + data[2] + "#" + data[3] + "#" + data[4] + "#" + data[5];
                    i++;
                }
                reader.Close();
                scom.Dispose();
                return SendLoadDay;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }

        public void adminInsert(string name, string id, string password, string phone)
        {
            int num = 0;
            string comtxt = string.Format("insert into ADMIN (NAME,ID,PASSWORD,PHONE) values('{0}','{1}','{2}','{3}')", name, id, password, phone);
            scom = new SqlCommand(comtxt, scon);

            if (scom.ExecuteNonQuery() == 1)
            {
                num++;
            }
        }

        public String[] adminSelect(string id)
        {
            dt = new DataTable();
            string comtxt = string.Format("SELECT  NAME, PASSWORD, PHONE FROM ADMIN WHERE  (ID = '{0}')", id);
            scom = new SqlCommand(comtxt, scon);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            da.Fill(dt);

            String[] SendLoadDay = new String[1];

            SqlDataReader reader = scom.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                string[] send = new string[3];
                send[0] = (string)reader["NAME"];
                send[1] = (string)reader["PASSWORD"];
                send[2] = (string)reader["PHONE"];
                SendLoadDay[i] = send[0] + '#' + send[1] + '#' + send[2];
                i++;
            }
            reader.Close();
            return SendLoadDay;
        }

        public void adminUpdate(string id, string name, string password, string phone)
        {
            string comtxt = string.Format("UPDATE ADMIN SET NAME = '{0}', PASSWORD = '{1}', PHONE = '{2}' WHERE ID = '{3}'", name, password, phone, id);
            scom = new SqlCommand(comtxt, scon);
            if (scom.ExecuteNonQuery() == 1)
            {
            }
        }

        public void adminDelete(string id)
        {
            string comtxt = string.Format("DELETE FROM ADMIN WHERE (ID = '{0}')", id);
            scom = new SqlCommand(comtxt, scon);
            if (scom.ExecuteNonQuery() == 0)
            {
            }
        }

        public bool adminIDCheck(string id)
        {
            string comtxt = string.Format("select * from ADMIN where ID = '{0}'", id);

            scom = new SqlCommand(comtxt, scon);
            SqlDataReader reader = scom.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                return false;
            }
            reader.Close();
            return true;
        }

        public bool InsertMember(String id, String pwd, String name, String age, String phone, String loginfo/*,  String name2, String Rea*/, String sex)
        {
            DataTable dt = new DataTable();
            string comtxt = string.Format("insert into MEMBER3 (ID, PASSWORD, NAME,AGE,PHONE,LOGINFO,MISSING,REQUEST,SEX) values(@ID,@PASSWORD,@NAME,@AGE,@PHONE,@LOGINFO,@MISSING,@REQUEST,@SEX)");
            string comtxt1 = string.Format("insert into GYRO (ID) values (@ID)");
            scom = new SqlCommand(comtxt, scon);
            SqlCommand scom1 = new SqlCommand(comtxt1, scon);

            scom.Parameters.AddWithValue("@ID", id);
            scom1.Parameters.AddWithValue("@ID", id);
            scom.Parameters.AddWithValue("@PASSWORD", pwd);
            scom.Parameters.AddWithValue("@NAME", name);
            scom.Parameters.AddWithValue("@AGE", age);
            scom.Parameters.AddWithValue("@PHONE", phone);
            scom.Parameters.AddWithValue("@LOGINFO", "0");
            /*
             scom.Parameters.AddWithValue("@NAME2", name2);
             scom.Parameters.AddWithValue("@RELATION", Rea);*/
            scom.Parameters.AddWithValue("@REQUEST", "0");
            scom.Parameters.AddWithValue("@MISSING", "0");
            scom.Parameters.AddWithValue("@SEX", sex);

            if (scom.ExecuteNonQuery() == 1 && scom1.ExecuteNonQuery() == 1)
            {
                truename = name;
                check = true;
                //return truename;
            }
            else
            {
                check = false;
            }
            return false;

        }
        public bool returnmem()
        {
            if (check == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public String returnnamejpg()
        {
            check = false;
            return truename;
        }
        public void Logoutmember(String name)
        {
               string comtxt = string.Format("UPDATE MEMBER3 SET LOGINFO = 0 WHERE (NAME = '{0}')", name);
                scom = new SqlCommand(comtxt, scon);
                if (scom.ExecuteNonQuery() == 1)
                {
                    //    //MessageBox.Show("수정 완료");
                }
        }
    }
}