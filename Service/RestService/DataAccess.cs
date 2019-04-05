using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

namespace RestService
{
    public class DataAccess
    {
        SqlConnection scon;
        SqlCommand scom;
        SqlCommand scom1;



        public void connect()
        {
            scon = new SqlConnection();
            scon.ConnectionString = ConfigurationManager.ConnectionStrings["MyService"].ConnectionString;
            scon.Open();
        }

        public void disconnect()
        {
            scon.Close();
        }

        public DataTable select(String ID, String PW)
        {
            DataTable dt = new DataTable();
            string comtxt = string.Format("Select * From MEMBER3 INNER JOIN GYRO ON MEMBER3.ID=GYRO.ID where MEMBER3.ID = '{0}' AND MEMBER3.PASSWORD = '{1}' AND MEMBER3.LOGINFO = 0", ID, PW);
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
  
           

            #region "JavascriptSerializer"
            /*System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }*/
            #endregion
        }

        public String[] LoginMember_Manager(String tf)
        {
            try
            {
                DataTable dt = new DataTable();
                string comtxt = string.Format("Select * From MEMBER3");
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                string Filter = "Loginfo=" + tf;
                DataRow[] Row = dt.Select(Filter);
                string[] SendLoginMember = new string[Row.Length];

                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = (string)Row[i]["ID"] + "#" + (string)Row[i]["NAME"];
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }

        public void Login(String ID)
        {
            string comtxt = string.Format("Update MEMBER3 Set Loginfo = '{0}' where ID = '{1}'", "1", ID);
            scom = new SqlCommand(comtxt, scon);
            scom.ExecuteNonQuery();
        }

        public void Logout(String ID)
        {
            string comtxt = string.Format("Update MEMBER3 Set Loginfo = '{0}' where ID = '{1}'", "0", ID);
            scom = new SqlCommand(comtxt, scon);
            scom.ExecuteNonQuery();
        }

        public void InsertMyPosition(String id, DateTime date, int x, int y, String Floor)
        {
            string comtxt = string.Format("insert into TimePosition (ID, DATE, X, Y, FLOOR) values(@ID,@DATE,@X,@Y,@FLOOR)");
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
            string comtxt = string.Format("Select X,Y From TimePosition where ID = '{0}'", id);
            scom = new SqlCommand(comtxt, scon);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            da.Fill(dt);

            return dt;
        }

        public String[] LoginMember_Position(String id)
        {
            try
            {
                String t = "1";
                DataTable dt = new DataTable();
                string comtxt = string.Format("Select * From MEMBER3 INNER JOIN TimePosition ON MEMBER3.ID = TimePosition.ID AND (TimePosition.DATE = (SELECT  MAX(DATE) FROM TimePosition AS Myinfo WHERE Myinfo.ID = {0}))", id);
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);

                string Filter = "Loginfo=" + t;
                DataRow[] Row = dt.Select(Filter);
                string[] SendLoginMember = new string[Row.Length];
                
                if (Row.Length == 0)
                    return null;
                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = Row[i]["X"].ToString() + "#" + Row[i]["Y"].ToString() + "#" + Row[i]["FLOOR"];
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }

        public String[] RelationMember_Position(String id)
        {
            try
            {
                String t = "1";
                DataTable dt = new DataTable();
                string comtxt = string.Format("Select * From MEMBER3 INNER JOIN TimePosition ON MEMBER3.ID = TimePosition.ID AND (TimePosition.DATE = (SELECT  MAX(DATE) FROM TimePosition AS Myinfo WHERE (ID =(SELECT ID2 FROM MEMBER3 WHERE(ID = '{0}')))))", id);
                scom = new SqlCommand(comtxt, scon);
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);
                
                string Filter = "Loginfo=" + t + "AND Request=" + t;
                DataRow[] Row = dt.Select(Filter);
                string[] SendLoginMember = new string[Row.Length];

                if (Row.Length == 0)
                    return null;
                for (int i = 0; i < Row.Length; i++)
                {
                    SendLoginMember[i] = Row[i]["X"].ToString() + "#" + Row[i]["Y"].ToString() + "#" + Row[i]["FLOOR"];
                }

                return SendLoginMember;
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                return null;
            }
        }

        public DataTable ReadTrainingData()
        {
            DataTable dt = new DataTable();
            string comtxt = string.Format("Select RP, AP1, AP2, AP3, AP4, AP5, X, Y From DATA2");
            scom = new SqlCommand(comtxt, scon);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            DataSet ds = new DataSet("DSet");
            da.Fill(ds, "DATA2");
            dt = ds.Tables["DATA2"];

            return dt;
        }

        public void GyroUpdate(String ID, float X, float Y, float Z)
        {
            try
            {
                string comtxt = string.Format("Update GYRO Set X = '{0}', Y = '{1}', Z = '{2}' where ID = '{3}'", X, Y, Z, ID);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }

        public void MissingUpdate_On(String ID)
        {
            try
            {
                string comtxt = string.Format("UPDATE MEMBER3 SET MISSING = '{0}' WHERE  (ID = '{1}') OR (ID2 = '{1}')", 1, ID);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }
        public void MissingUpdate_Off(String ID)
        {
            try
            {
                string comtxt = string.Format("UPDATE MEMBER3 SET MISSING = '{0}' WHERE  (ID = '{1}') OR (ID2 = '{1}')", 0, ID);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }

        public void MCDUpdate(String ID, int MCDX, int MCDY)
        {
            try
            {
                string comtxt = string.Format("Update GYRO Set MCDX = '{0}', MCDY = '{1}' where ID = '{2}'", MCDX, MCDY, ID);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }

        public String RelaSelect(String ID, String ID2, String RELATION)
        {
            string id2 = String.Empty;
            try
            {
                DataTable dt = RelaIDreturn(ID2);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        id2 = dr["ID"].ToString();
                    }
                    return id2;
                }
                else
                    return null;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable RelaIDreturn(String ID2)
        {
            DataTable dt = new DataTable();
            string comtxt2 = string.Format("Select ID From MEMBER3 where ID = '{0}'", ID2);
            scom = new SqlCommand(comtxt2, scon);
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

        public String Rela_NAME2_Select(String ID2)
        {
            string id2_name = String.Empty;
            try
            {
                DataTable dt = Rela_NAME2_Select_return(ID2);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        id2_name = dr["NAME"].ToString();
                    }
                    return id2_name;
                }
                else
                    return null;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Rela_NAME2_Select_return(String ID2)
        {
            DataTable dt = new DataTable();
            string comtxt2 = string.Format("Select NAME From MEMBER3 where ID = '{0}'", ID2);
            scom = new SqlCommand(comtxt2, scon);
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

        public void RelaUpdate_me(String ID, String ID2, String NAME2, String RELATION)
        {
            try
            {
                string comtxt = string.Format("Update MEMBER3 Set ID2 = '{0}', NAME2 = '{1}', RELATION = '{2}' where ID = '{3}'", ID2, NAME2, RELATION, ID);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }

        public void RelaUpdate_ID2(String ID, String ID2, String NAME, String RELATION)
        {
            try
            {
                string comtxt = string.Format("Update MEMBER3 Set ID2 = '{0}', NAME2 = '{1}', RELATION = '{2}', REQUEST = '{3}' where ID = '{4}'", ID, NAME, RELATION, 2, ID2);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }

        //////////////////////////////////////////////////////////////
        public String RequestSelect(String ID)
        {
            string request = String.Empty;
            try
            {
                DataTable dt = RequestReturn(ID);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        request = dr["REQUEST"].ToString();
                    }
                    return request;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable RequestReturn(String ID)
        {
            DataTable dt = new DataTable();
            string comtxt2 = string.Format("Select REQUEST From MEMBER3 where ID = '{0}'", ID);
            scom = new SqlCommand(comtxt2, scon);
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

        public void AcceptUpdate(String ID, String ID2)
        {
            try
            {
                string comtxt = string.Format("UPDATE MEMBER3 SET REQUEST = '{0}' WHERE  (ID = '{1}') OR (ID = '{2}')", 1, ID, ID2);
                scom = new SqlCommand(comtxt, scon);
                scom.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
            }
        }

        public String Read_ID2(String ID)
        {
            string id2 = String.Empty;
            try
            {
                DataTable dt = Read_ID2_Return(ID);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        id2 = dr["ID2"].ToString();
                    }
                    return id2;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Read_ID2_Return(String ID)
        {
            DataTable dt = new DataTable();
            string comtxt2 = string.Format("Select ID2 From MEMBER3 where ID = '{0}'", ID);
            scom = new SqlCommand(comtxt2, scon);
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

        public String isID_Duplicated(String ID) // ID중복검사 함수
        {
            DataTable dt = new DataTable();
            string comtxt = string.Format("Select ID From MEMBER3 where ID = '{0}'", ID);
            scom = new SqlCommand(comtxt, scon);
            try
            {
                scom.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(scom);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return "fail";
                    // 조회한 ID 가 존재하면 Fail 반환 
                }
                else
                {
                    return "success";
                    // 없으면 Success 반환 
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public String InsertMember(String id, String pwd, String name, String age, String phone, String loginfo/*,  String name2, String Rea*/, String sex)
        {
            DataTable dt = new DataTable();

            string comtxt = string.Format("insert into MEMBER3 (ID, PASSWORD, NAME,AGE,PHONE,LOGINFO,MISSING,REQUEST,SEX) values(@ID,@PASSWORD,@NAME,@AGE,@PHONE,@LOGINFO,@MISSING,@REQUEST,@SEX)");
            string comtxt1 = string.Format("insert into GYRO (ID) values (@ID)");
            scom = new SqlCommand(comtxt, scon);
            scom1 = new SqlCommand(comtxt1, scon);
            try
            {
                scom.Parameters.AddWithValue("@ID", id);
                scom1.Parameters.AddWithValue("@ID", id);
                scom.Parameters.AddWithValue("@PASSWORD", pwd);
                scom.Parameters.AddWithValue("@NAME", name);
                scom.Parameters.AddWithValue("@AGE", age);
                scom.Parameters.AddWithValue("@PHONE", phone);
                scom.Parameters.AddWithValue("@LOGINFO", "0");
                scom.Parameters.AddWithValue("@REQUEST", "0");
                scom.Parameters.AddWithValue("@MISSING", "0");
                scom.Parameters.AddWithValue("@SEX", sex);

                scom.ExecuteNonQuery();
                scom1.ExecuteNonQuery();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////


        //public DataTable relaSelect(String ID)
        //{
        //    DataTable dt = new DataTable();
        //    string comtxt = string.Format("Select RELATION From MEMBER3 where ID = '{0}'", ID);
        //    scom = new SqlCommand(comtxt, scon);
        //    try
        //    {
        //        scom.ExecuteNonQuery();
        //        SqlDataAdapter da = new SqlDataAdapter(scom);
        //        da.Fill(dt);

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public String relaSelect(String ID)
        //{
        //    try
        //    {
        //        //String t = "1";
        //        DataTable dt = new DataTable();
        //        string comtxt = string.Format("Select RELATION From MEMBER3 where ID = '{0}'", ID);
        //        scom = new SqlCommand(comtxt, scon);
        //        SqlDataAdapter da = new SqlDataAdapter(scom);
        //        da.Fill(dt);

        //        //string Filter = "Loginfo=" + t;
        //        //DataRow[] Row = dt.Select(Filter);
        //        string SendRelation;

        //        //if (Row.Length == 0)
        //        //    return null;
        //        //for (int i = 0; i < Row.Length; i++)
        //        //{
        //            SendRelation = da.ToString();//["RELATION"].ToString();
        //        //}

        //        return SendRelation;
        //    }
        //    catch (Exception ex)
        //    {
        //        String msg = ex.Message;
        //        return null;
        //    }
        //}
    }
}
