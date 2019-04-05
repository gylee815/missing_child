using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace RestService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RestServiceImpl : IRestServiceImpl
    {
        public KNN knn = new KNN();
        public KNN_Gaussian knn_gaussian = new KNN_Gaussian();
        public KNN_Gaussian2 knn_gaussian2 = new KNN_Gaussian2();

        public KNN_2 knn2 = new KNN_2();
        public KNN2_Gaussian knn2_gaussian = new KNN2_Gaussian();
        public KNN2_Gaussian2 knn2_gaussian2 = new KNN2_Gaussian2();

        public MS_KNN ms_knn = new MS_KNN();
        public MS_KNN_Gaussian ms_knn_gaussian = new MS_KNN_Gaussian();
        public MS_KNN_Gaussian2 ms_knn_gaussian2 = new MS_KNN_Gaussian2();

        public MS_KNN2 ms_knn2 = new MS_KNN2();
        public MS_KNN2_Gaussian ms_knn2_gaussian = new MS_KNN2_Gaussian();
        public MS_KNN2_Gaussian2 ms_knn2_gaussian2 = new MS_KNN2_Gaussian2();
        
        public New_KNN_Gaussian new_knn_gaussian = new New_KNN_Gaussian();
        public New_KNN2_Gaussian2 new_knn2_gaussian2 = new New_KNN2_Gaussian2();
        public New_KNN_Gaussian2 new_knn_gaussian2 = new New_KNN_Gaussian2();
        public New_KNN2_Gaussian new_knn2_gaussian = new New_KNN2_Gaussian();

        public ExistingAlgo exalgo = new ExistingAlgo();  
        
        public New_KNN_Gaussian2_K5 new_knn_gaussian2_k5 = new New_KNN_Gaussian2_K5();
        public New_KNN_Gaussian2_K4 new_knn_gaussian2_k4 = new New_KNN_Gaussian2_K4();
        public New_KNN_Gaussian2_K6 new_knn_gaussian2_k6 = new New_KNN_Gaussian2_K6();

        public New_KNN_Gaussian_K5 new_knn_gaussian_k5 = new New_KNN_Gaussian_K5();
        public New_KNN2_Gaussian2_K5 new_knn2_gaussian2_k5 = new New_KNN2_Gaussian2_K5();
        public New_KNN2_Gaussian_K5 new_knn2_gaussian_k5 = new New_KNN2_Gaussian_K5();

        public WallCheck wallCheck = new WallCheck();

        #region Callback시 사용할 부분
        //public delegate void Service(string type, int value);
        ////동기화 작업을 위해서 가상의 객체 생성
        //private static Object syncObj = new Object();
        ////채팅방에 있는 유저 이름 목록
        //private static ArrayList User = new ArrayList();
        ////델리게이트 =========================================================
        //// 개인용 델리게이트
        //private Service MyService;
        ////전체에게 보낼 정보를 담고 있는 델리게이트
        //private static Service List;
        //IRestServiceImplCallback callback = null;     //  

        //public RestServiceImpl()
        //{
        //    MyService = new Service(UserHandler);
        //    callback = OperationContext.Current.GetCallbackChannel<IRestServiceImplCallback>();
        //    List += MyService;
        //}

        //private void UserHandler(string msgType, int value)       //List마다 UserHandler를 갖고 있고 Broadcast의 msgtype에 따라 callback함수 호출
        //{
        //    try
        //    {
        //        //클라이언트에게 보내기
        //        switch (msgType)
        //        {

        //        }
        //    }
        //    catch//에러가 발생했을 경우
        //    {

        //    }
        //}
        #endregion

        public List<Memberinfo> Member_check(string ID, string PW)
        {
            List<Memberinfo> memb = new List<Memberinfo>();
            DataAccess da = new DataAccess();
            da.connect();
            DataTable dt = da.select(ID,PW);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Memberinfo memb1 = new Memberinfo
                    {
                        ID = dr["ID"].ToString(),
                        NAME = dr["NAME"].ToString(),
                        RELATION = dr["RELATION"].ToString(),
                        GYRO_X = dr["X"].ToString(),
                        GYRO_Y = dr["Y"].ToString(),
                        GYRO_Z = dr["Z"].ToString(),
                        MCDX = dr["MCDX"].ToString(),
                        MCDY = dr["MCDY"].ToString()
                    };
                    memb.Add(memb1);
                }
                da.Login(ID);
                da.disconnect();
                return memb;
            }
            else
                da.disconnect();
                return memb;
         
            #region "JavaScriptSerializer"
            /*var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<dynamic>(da.select(ID, PW));

            var json = jss.Serialize(dict);

            if (da.select(ID, PW) == "")
            {
                da.disconnect();
                return null;
            }
            else
                da.disconnect();

            return json;*/
            #endregion
        }

        public String Logout(String id)
        {
            DataAccess da = new DataAccess();
            da.connect();
            try
            {
                da.Logout(id);
                da.disconnect();
                return id + ":Logout!!";
            }
            catch (Exception ex)
            {
                da.disconnect();
                return ex.Message;
            }
        }

        public String Request_info()
        {   //oncreate
            //request검사 하는 디비 함수 사용 해서 0,1,2값을 얻어서 return
            return null;
        }

        public Positioninfo Positioning(String ID, String OldX, String OldY, String Direction, String Rssi1, String Rssi2, String Rssi3, String Rssi4, String Rssi5, String Floor)
        {
            Positioninfo posi = new Positioninfo();
            DataAccess da = new DataAccess();
            da.connect();

            Decimal AP1 = Convert.ToDecimal(Rssi1);
            Decimal AP2 = Convert.ToDecimal(Rssi2);
            Decimal AP3 = Convert.ToDecimal(Rssi3);
            Decimal AP4 = Convert.ToDecimal(Rssi4);
            Decimal AP5 = Convert.ToDecimal(Rssi5);
            DateTime myDate = DateTime.Now;
            int[] position2 = new int[2];
            //RSSI값을 받아 X좌표 하고 Y좌표 계산하는 방식을 적용만 하면 된다!!!!
            Decimal[] ARR_AP = new Decimal[5] { AP1, AP2, AP3, AP4, AP5 };
            DataTable dt = da.ReadTrainingData();

            #region 기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ]
            //int[] position = knn.KNN_Calc(ARR_AP);
            #endregion

            #region 기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian total
            //int[] position = knn_gaussian.KNNGaussian_Calc(ARR_AP);
            #endregion

            #region 기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian seperate
            //int[] position = knn_gaussian2.Calc_Result(ARR_AP);
            #endregion

            #region 변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ]
            //int[] position = knn2.Reuslt(ARR_AP);
            #endregion

            #region 변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian total
            //int[] position = knn2_gaussian.Calc_Result(ARR_AP);
            #endregion

            #region 변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian seperate
            //int[] position = knn2_gaussian2.Calc_Result(ARR_AP);
            #endregion

            #region 문수_기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ]
            //int[] position = ms_knn.Calc_Result(ARR_AP, dt);
            #endregion

            #region 문수_기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian total
            //int[] position = ms_knn_gaussian.Calc_Result(ARR_AP, dt);
            #endregion

            #region 문수_기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian seperate
            //int[] position = ms_knn_gaussian2.Calc_Result(ARR_AP, dt);
            #endregion

            #region 문수_변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ]
            //int[] position = ms_knn2.Calc_Result(ARR_AP, dt);
            #endregion

            #region 문수_변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian total
            //int[] position = ms_knn2_gaussian.Calc_Result(ARR_AP, dt);
            #endregion

            #region 문수_변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian seperate 많이쓰던거
            //int[] position = ms_knn2_gaussian2.Calc_Result(ARR_AP, dt);
            #endregion

            #region 기본 KNN + 기존 Gaussian (표준 편차 변형) 별로.. 그저그럼
            //int[] position = new_knn_gaussian.Calc_Result(ARR_AP, dt);
            #endregion

            #region 변현 KNN + 변형 Gaussian (표준 편차 변형) 별로 센터만...
            //int[] position = new_knn2_gaussian2.Calc_Result(ARR_AP, dt);
            #endregion 

            #region 기본 KNN + 변형 Gaussian (표준 편차 변형) 조금 괜찮음 1@@@@
            //int[] position = new_knn_gaussian2.Calc_Result(ARR_AP, dt);
            #endregion
            
            #region 변형 KNN + 기본 Gaussian (표준 편차 변형) 별로 2
            //int[] position = new_knn2_gaussian.Calc_Result(ARR_AP, dt);
            #endregion 

            #region 기본 KNN + 변형 Gaussian (표준 편차 변형, K5)
            int[] position = new_knn_gaussian2_k5.Calc_Result(ARR_AP, dt);
            #endregion 

            #region 기본 KNN + 변형 Gaussian (표준 편차 변형, K4)
            //int[] position = new_knn_gaussian2_k4.Calc_Result(ARR_AP, dt);
            #endregion

            #region 기본 KNN + 변형 Gaussian (표준 편차 변형, K6)
            //int[] position = new_knn_gaussian2_k6.Calc_Result(ARR_AP, dt);
            #endregion 

            #region 기본 KNN + 기본 Gaussian (표준 편차 변형, K5)
            //int[] position = new_knn_gaussian_k5.Calc_Result(ARR_AP, dt);
            #endregion

            #region 변형 KNN + 변형 Gaussian (표준 편차 변형, K5) 괜찮은편
            //int[] position = new_knn2_gaussian2_k5.Calc_Result(ARR_AP, dt);
            #endregion 

            #region 변형 KNN + 기본 Gaussian (표준 편차 변형, K5) 
            //int[] position = new_knn2_gaussian_k5.Calc_Result(ARR_AP, dt);
            #endregion

            #region 기존기수 알고리즘... 별로
            //int[] position = exalgo.Calc_Result(ARR_AP, dt);
            #endregion

            #region 관계된 아이디의 위치를 받아옴
            String[] id2_position = da.RelationMember_Position(ID);
            if (id2_position == null)
            {
                position2[0] = 99999;
                position2[1] = 99999;
            }
            else
            {
                string[] token = id2_position[0].Split('#');
                position2[0] = Math.Abs(Convert.ToInt32(token[0]));
                position2[1] = Math.Abs(Convert.ToInt32(token[1]));
            }
            #endregion

            if (Convert.ToInt32(OldX) == 0 || Convert.ToInt32(OldY) == 0)
            {
                posi.Position_X = Convert.ToString(position[0]);
                posi.Position_Y = Convert.ToString(position[1]);
                posi.Position_X2 = Convert.ToString(position2[0]);
                posi.Position_Y2 = Convert.ToString(position2[1]);
                da.InsertMyPosition(ID, myDate, position[0], position[1], Floor);
                da.disconnect();
                return posi;
            }
            else if (Math.Abs(Convert.ToInt32(OldX) - position[0]) >= 7000 || Math.Abs(Convert.ToInt32(OldY) - position[1]) >= 12000)
            {
                posi.Position_X = OldX;
                posi.Position_Y = OldY;
                posi.Position_X2 = Convert.ToString(position2[0]);
                posi.Position_Y2 = Convert.ToString(position2[1]);
                da.InsertMyPosition(ID, myDate, Convert.ToInt32(OldX), Convert.ToInt32(OldY), Floor);
                da.disconnect();
                return posi;
            }
            else if (Convert.ToInt32(OldX) == position[0] && Convert.ToInt32(OldY) == position[1])
            {
                bool isWall;
                posi.Position_X = Convert.ToString(position[0]);
                posi.Position_Y = Convert.ToString(position[1]);
                posi.Position_X2 = Convert.ToString(position2[0]);
                posi.Position_Y2 = Convert.ToString(position2[1]);
                String[] old_position = da.LoginMember_Position(ID);
                string[] token = old_position[0].Split('#');
                int move_x = Convert.ToInt32(token[0]);
                int move_y = Convert.ToInt32(token[1]);
                switch (Direction)
                {
                    case "N":
                        isWall = wallCheck.check_wall(move_x, move_y);
                        if (isWall == true)
                        {
                            da.InsertMyPosition(ID, myDate, move_x, move_y, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x);
                            posi.PositionMove_Y = Convert.ToString(move_y);
                        }
                        else
                        {
                            da.InsertMyPosition(ID, myDate, move_x - 720, move_y, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x - 720);
                            posi.PositionMove_Y = Convert.ToString(move_y);
                        }
                        break;
                    case "E":
                        isWall = wallCheck.check_wall(move_x, move_y);
                        if (isWall == true)
                        {
                            da.InsertMyPosition(ID, myDate, move_x, move_y, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x);
                            posi.PositionMove_Y = Convert.ToString(move_y);
                        }
                        else
                        {
                            da.InsertMyPosition(ID, myDate, move_x, move_y - 720, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x);
                            posi.PositionMove_Y = Convert.ToString(move_y - 720);
                        }
                        break;
                    case "W":
                        isWall = wallCheck.check_wall(move_x, move_y);
                        if (isWall == true)
                        {
                            da.InsertMyPosition(ID, myDate, move_x, move_y, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x);
                            posi.PositionMove_Y = Convert.ToString(move_y);
                        }
                        else
                        {
                            da.InsertMyPosition(ID, myDate, move_x, move_y + 720, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x);
                            posi.PositionMove_Y = Convert.ToString(move_y + 720);
                        }
                        break;
                    case "S":
                        isWall = wallCheck.check_wall(move_x, move_y);
                        if (isWall == true)
                        {
                            da.InsertMyPosition(ID, myDate, move_x, move_y, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x);
                            posi.PositionMove_Y = Convert.ToString(move_y);
                        }
                        else
                        {
                            da.InsertMyPosition(ID, myDate, move_x + 720, move_y, Floor);
                            posi.PositionMove_X = Convert.ToString(move_x + 720);
                            posi.PositionMove_Y = Convert.ToString(move_y);
                        }
                        break;
                }
                da.disconnect();
                return posi;
            }
            else
            {
                posi.Position_X = Convert.ToString(position[0]);
                posi.Position_Y = Convert.ToString(position[1]);
                posi.Position_X2 = Convert.ToString(position2[0]);
                posi.Position_Y2 = Convert.ToString(position2[1]);
                da.InsertMyPosition(ID, myDate, position[0], position[1], Floor);
                da.disconnect();
                return posi;
            }
        }

        public Positioninfo NoMovePositioning(String ID, String OldX, String OldY, String Rssi1, String Rssi2, String Rssi3, String Rssi4, String Rssi5, String Floor)
        {
            Positioninfo posi = new Positioninfo();
            DataAccess da = new DataAccess();
            int[] position2 = new int[2];
            da.connect();

            #region 관계된 아이디의 위치를 받아옴
            String[] id2_position = da.RelationMember_Position(ID);
            if (id2_position == null)
            {
                position2[0] = 99999;
                position2[1] = 99999;
            }
            else
            {
                string[] token = id2_position[0].Split('#');
                position2[0] = Math.Abs(Convert.ToInt32(token[0]));
                position2[1] = Math.Abs(Convert.ToInt32(token[1]));
            }
            #endregion

            DateTime myDate = DateTime.Now;

            #region 주석...
            //Decimal AP1 = Convert.ToDecimal(Rssi1);
            //Decimal AP2 = Convert.ToDecimal(Rssi2);
            //Decimal AP3 = Convert.ToDecimal(Rssi3);
            //Decimal AP4 = Convert.ToDecimal(Rssi4);
            //Decimal AP5 = Convert.ToDecimal(Rssi5);
            
            ////RSSI값을 받아 X좌표 하고 Y좌표 계산하는 방식을 적용만 하면 된다!!!!
            //Decimal[] ARR_AP = new Decimal[5] { AP1, AP2, AP3, AP4, AP5 };
            //DataTable dt = da.ReadTrainingData();

            //#region 기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ]
            ////int[] position = knn.KNN_Calc(ARR_AP);
            //#endregion

            //#region 기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian total
            ////int[] position = knn_gaussian.KNNGaussian_Calc(ARR_AP);
            //#endregion

            //#region 기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian seperate
            ////int[] position = knn_gaussian2.Calc_Result(ARR_AP);
            //#endregion

            //#region 변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ]
            ////int[] position = knn2.Reuslt(ARR_AP);
            //#endregion

            //#region 변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian total
            ////int[] position = knn2_gaussian.Calc_Result(ARR_AP);
            //#endregion

            //#region 변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian seperate
            ////int[] position = knn2_gaussian2.Calc_Result(ARR_AP);
            //#endregion

            //#region 문수_기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ]
            ////int[] position = ms_knn.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 문수_기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian total
            ////int[] position = ms_knn_gaussian.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 문수_기본KNN[ sqrt( pow(AP1-AP1') + .... + pow(AP5 - AP') ) ] + Gaussian seperate
            ////int[] position = ms_knn_gaussian2.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 문수_변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ]
            ////int[] position = ms_knn2.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 문수_변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian total
            ////int[] position = ms_knn2_gaussian.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 문수_변형KNN[ sqrt( pow(AP1 - AP1') ) + .... + sqrt( pow(AP5 - AP5') ) ] + Gaussian seperate
            ////int[] position = ms_knn2_gaussian2.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 기본 KNN + 기존 Gaussian (표준 편차 변형)
            ////int[] position = new_knn_gaussian.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 변현 KNN + 변형 Gaussian (표준 편차 변형)
            ////int[] position = new_knn2_gaussian2.Calc_Result(ARR_AP, dt);
            //#endregion

            //#region 기본 KNN + 변형 Gaussian (표준 편차 변형)
            ////int[] position = new_knn_gaussian2.Calc_Result(ARR_AP, dt);
            //#endregion
            #endregion

            posi.Position_X = Convert.ToString(OldX);
            posi.Position_Y = Convert.ToString(OldY);
            posi.Position_X2 = Convert.ToString(position2[0]);
            posi.Position_Y2 = Convert.ToString(position2[1]);
            da.InsertMyPosition(ID, myDate, Convert.ToInt32(OldX), Convert.ToInt32(OldY), Floor);
            da.disconnect();
            return posi;
        }

        public String GyroUpdate(String id, String revision_X, String revision_Y, String revision_Z)
        {
            //String[] gyro_value = new String[3];
            float revision_TofX, revision_TofY, revision_TofZ;
            revision_TofX = (float)Convert.ToDouble(revision_X);
            revision_TofY = (float)Convert.ToDouble(revision_Y);
            revision_TofZ = (float)Convert.ToDouble(revision_Z);
            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                da.GyroUpdate(id, revision_TofX, revision_TofY, revision_TofZ);
                da.disconnect();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        public String MissingUpdate_On(String id)
        {
            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                da.MissingUpdate_On(id);
                da.disconnect();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        public String MissingUpdate_Off(String id)
        {
            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                da.MissingUpdate_Off(id);
                da.disconnect();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        public String MCDUpdate(String ID, String LimitX, String LimitY)
        {
            int limit_x, limit_y;
            limit_x = Convert.ToInt32(LimitX);
            limit_y = Convert.ToInt32(LimitY);

            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                da.MCDUpdate(ID, limit_x, limit_y);
                da.disconnect();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
   
        //public String RelaSelect(String ID, String ID2, String NAME2, String RELATION)
        //{
        //    DataAccess da = new DataAccess();
        //    da.connect();
        //    String IDselect = da.RelaSelect(ID, ID2, RELATION);

        //    da.disconnect();

        //    return IDselect;
        //}
        //public String RelaUpdate(String ID, String ID2, String NAME2, String RELATION, String NAME)
        //{
        //    DataAccess da = new DataAccess();
        //    try
        //    {
        //        da.connect();
        //        da.RelaUpdate_me(ID, ID2, NAME2, RELATION);

        //        if (RELATION.Equals("P"))
        //            RELATION = "C";
        //        else if (RELATION.Equals("C"))
        //            RELATION = "P";

        //        da.RelaUpdate_ID2(ID, ID2, NAME, RELATION);

        //        da.disconnect();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }

        //    return null;
        //}

        public String RelaSelect(String ID, String ID2, String RELATION)
        {
            DataAccess da = new DataAccess();
            da.connect();
            String IDselect = da.RelaSelect(ID, ID2, RELATION);

            da.disconnect();

            return IDselect;
        }
        public String Rela_NAME2_Select(String ID2)
        {
            DataAccess da = new DataAccess();
            da.connect();
            String NAME2_select = da.Rela_NAME2_Select(ID2);

            da.disconnect();

            return NAME2_select;
        }
        public String RelaUpdate(String ID, String ID2, String NAME2, String RELATION, String NAME)
        {
            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                da.RelaUpdate_me(ID, ID2, NAME2, RELATION);

                if (RELATION.Equals("P"))
                    RELATION = "C";
                else if (RELATION.Equals("C"))
                    RELATION = "P";

                da.RelaUpdate_ID2(ID, ID2, NAME, RELATION);

                da.disconnect();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return null;
        }
       
        public String Request(String ID)
        {
            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                String request = string.Empty;
                request = da.RequestSelect(ID);
                da.disconnect();

                return request;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public String Accept(String ID, String ID2)
        {
            DataAccess da = new DataAccess();
            try
            {
                da.connect();
                da.AcceptUpdate(ID, ID2);
                da.disconnect();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return null;
        }

        public String Read_ID2(String ID)
        {
            DataAccess da = new DataAccess();
            da.connect();
            String IDselect = da.Read_ID2(ID);
            da.disconnect();

            return IDselect;
        }

        public string insertMember(String ID, String PW, String NAME, String AGE, String PHONE, String LOGINFO/*, String NAME2, String RELATION*/, String SEX)
        {
            DataAccess da = new DataAccess();
            da.connect();

            String info = da.InsertMember(ID, PW, NAME, AGE, PHONE, LOGINFO, SEX);
            da.disconnect();

            return info;// 여기서 이클립스 insertmember로 데이터를 보내준다     
        }

        public String isDuplicated(String ID)
        {
            DataAccess da = new DataAccess();
            da.connect();

            String info = da.isID_Duplicated(ID);
            da.disconnect();

            return info;
        }
         
        //public Relation RelationCheck(string ID)
        //{
        //    //Relation rela = new Relation();
        //    string RELATION;
        //    DataAccess da = new DataAccess();
        //    da.connect();
        //    DataTable dt = da.relaSelect(ID);
        //    if (dt.Rows.Count != 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            RELATION = dr["RELATION"].ToString();  
        //        }
        //        da.disconnect();
        //        return RELATION;
        //    }
        //    else
        //        da.disconnect();
        //    return RELATION;
        //}

        //public List<Relation> RelationCheck(string ID)
        //{
        //    List<Relation> rela = new List<Relation>();
        //    DataAccess da = new DataAccess();
        //    da.connect();
        //    DataTable dt = da.relaSelect(ID);
        //    if (dt.Rows.Count != 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            Relation rela1 = new Relation
        //            {
        //                RELATION = dr["RELATION"].ToString()
        //            };
        //            rela.Add(rela1);
        //        }
        //        da.disconnect();
        //        return rela;
        //    }
        //    else
        //        da.disconnect();
        //    return rela;
        //}

        //public Relation RelationCheck(string ID)
        //{
        //    List<Relation> rela = new List<Relation>();
        //    DataAccess da = new DataAccess();
        //    da.connect();
        //    DataTable dt = da.relaSelect(ID);
        //    if (dt.Rows.Count != 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            Relation rela1 = new Relation
        //            {
        //                RELATION = dr["RELATION"].ToString()
        //            };
        //            rela.Add(rela1);
        //        }
        //        da.disconnect();
        //        return rela;
        //    }
        //    else
        //        da.disconnect();
        //    return rela;
        //}

    }
}