using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowEngine;
using Tao.OpenGl;
using System.ServiceModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;
using System.Web;
using Microsoft.Http;
using Microsoft.Http.Headers;
using Microsoft.ServiceModel.Web.SpecializedServices;
using System.Collections;
using System.Threading; 
using Microsoft.Expression.Interactivity.Media;
using Microsoft.Expression.Interactivity.Core;
using Tao.FreeGlut;
using System.Drawing;

namespace ManagerClient
{
    public class MouseVision 
    {
        #region Camera constants
        const double div1 = Math.PI / 180;
        const double div2 = 180 / Math.PI;

        #endregion

        #region Private atributes

        static float eyex, eyey, eyez;
        static float centerx, centery, centerz;
        static float forwardSpeed = 0.3f;
        static float yaw, pitch;
        static float rotationSpeed = 1 / 5f;
        static double i, j, k;
        float rot = 5;
       
        
        //물체 생성
        Exterior sky = new Exterior();
        public r_Rectangle rectangleDraw;       
        //UI Thread를 위한 준비
        List<Thread> Thread_ARR = new List<Thread>();

        //이동좌표 변수
        float personx = -30;
        float persony = -3;
        float personz = 50;

        //main폼에 접근하기 위한.
        public string c_cursor = null;
        #endregion

        #region Web연결을 할 주소, 좌표 저장공간
        static string uri;
        string[] FinalPosition; // 좌표 저장공간
        public string Floor_num;
        public  string now_Id = null;

        //public string xml_ID;
       
        #endregion

        #region private atributes
        //멤버추가 및 좌표 저장을 위한 변수
        public DB db;
        public List<Info> ARR;
        string LogOutARR;
        ArrayList DB_ARR;
        Info meminfo;
        public XML xml;
        int num = 0;
        int num1 = 0;
        int AllViewState;
        int Old_AllViewState;
        ComboBox MvSendToDb;
       public List<Info> FinalXmlPosi;
        int step = 0;
        public int exit_st = 0;
        //public ArrayList Exit_ARR;
        public string[] Exit_ARR;
        List<f_Info> M_F_ARR;
        double c_Zoom = 0.8;
        int[] count =null;
        bool[,] f_inner;
        List<in_floorInfo> in_Floor;
        in_floorInfo in_FloorInfo;
       
        
        #endregion


        //xml 에 저장된 ID 값 받아오는 객체
        public string[] XmlAllID;


        public Thread UI_Thread;
        public Thread th;
        public Thread th1;
        string[] Textstring;
        public  string[] text_ID;
        public string[] text_name;
        public string[] m_name;
        public string s_name;
        ListView list_v;

        public MouseVision(ComboBox ControlCom, ListView listview2)
        {
            rectangleDraw = new r_Rectangle();           
            MvSendToDb = ControlCom;
            ARR = new List<Info>();
            db = new DB(MvSendToDb);
            db.StartCon();
            xml = new XML();
            DB_ARR = new ArrayList();
            UI_Thread = new Thread(new ThreadStart(Rectangle));           
            UI_Thread.Start();
            meminfo = new Info();
           // Exit_ARR = new ArrayList();
            th = new Thread(SendPosiArr);
            th1 = new Thread(AllViewViewXml);
            FinalXmlPosi = new List<Info>();
            in_Floor = new List<in_floorInfo>();
            list_v = listview2;
            //초기화
            Textstring = xml.S_All();
            text_ID = new string[Textstring.Length];
            text_name = new string[Textstring.Length];

            for (int i = 0; i < Textstring.Length; i++)
            {
                string[] a = Textstring[i].Split(' ');
                text_ID[i] = a[0];
                text_name[i] = a[1];
            }
        }

         
        #region  get set
        public void setnum(int n)
        {
            num = n;
        }
        public void setnum1(int n)
        {
            num1 = n;
        }
        public static float Pitch
        {
            get { return MouseVision.pitch; }
            set { MouseVision.pitch = value; }
        }

        public static float Yaw
        {
            get { return MouseVision.yaw; }
            set { MouseVision.yaw = value; }
        }
        
        public void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Exception Error = e.Error;
            bool Cancelled = e.Cancelled;
            string result = e.Result;
        }
        public void SetAllxmlID(string[] str)
        {
            XmlAllID = new string[str.Length];
            int i = 0;
            foreach (string temp in str)
            {
                XmlAllID[i] = (string)temp.Clone();
                i++;
            }
        }
        #endregion

        //좌표찍기 
        public void Rectangle()
        {
            while (true)
            {
                uri = "http://61.81.99.100:8080/Xml/Service1.svc/";
                try
                {
                    using (HttpResponseMessage response = new HttpClient().Get(uri))
                    {
                        string res = response.Content.ReadAsString();
                        string[] R = res.Split('>');
                        string[] R2 = R[1].Split('<');                        
                        FinalPosition = R2[0].Split(' ');
                    }
                    //로그인 확인

                    if (ARR.Count == 0)
                    {
                        M_ratio_3D(FinalPosition);
                        meminfo.MemberID = FinalPosition[0];                       
                        meminfo.Xposi = FinalPosition[1];
                        meminfo.Yposi = FinalPosition[2];
                        meminfo.Zposi = FinalPosition[3];
                        meminfo.Floor = FinalPosition[4];
                        //meminfo = new Info(FinalPosition[0], FinalPosition[1], FinalPosition[2], FinalPosition[3]);
                        ARR.Add(meminfo);
                    }

                    else
                    {
                        #region 로그아웃 상황

                        //ARR안에 있는 로그아웃된 Person들을 화면밖으로 내보냄.
                        DB_ARR.Clear();
                        try
                        {
                            string[] logout_name = xml.Client.LoadLogOutPerson("false");
                            for (int i = 0; i < logout_name.Length; i++)
                            {
                                DB_ARR.Add(logout_name[i]);                              
                            }
                            Exit_ARR = new string[DB_ARR.Count];
                            DB_ARR.CopyTo(Exit_ARR);

                            foreach (string LogOutARR in DB_ARR)
                            {
                                if (LogOutARR == FinalPosition[0])
                                {
                                    FinalPosition[0] = "";
                                    FinalPosition[1] = "99999";
                                    FinalPosition[2] = "99999";
                                    FinalPosition[3] = "99999";
                                    FinalPosition[4] = "";
                                }

                                for (int num = 0; num < ARR.Count; num++)
                                {
                                    if (LogOutARR == ARR[num].MemberID)
                                    {
                                        ARR[num].MemberID = "";
                                        ARR[num].Xposi = "99999";
                                        ARR[num].Yposi = "99999";
                                        ARR[num].Zposi = "99999";
                                        ARR[num].Floor = "";


                                        // if (!Exit_ARR.Contains(ARR[num].MemberID))
                                        //      exit_st += 1;
                                    }
                                }
                            }
                        }
                        catch (Exception logout)
                        {
                            MessageBox.Show(logout.ToString());
                        }

                        #endregion

                        #region 접속상황
                        for (int i = 0; i < ARR.Count; i++)
                        {
                            //기존 접속자에 따른 좌표값 갱신
                            Info n = ARR[i];
                            if (n.MemberID == FinalPosition[0].ToString())
                            {
                                M_ratio_3D(FinalPosition);
                                meminfo = new Info(FinalPosition[0], FinalPosition[1], FinalPosition[2], FinalPosition[3], FinalPosition[4]);
                                ARR[i] = meminfo;
                                break;
                            }
                            else
                            {
                                if (ARR[i].MemberID == "")
                                {
                                    M_ratio_3D(FinalPosition);
                                    meminfo = new Info(FinalPosition[0], FinalPosition[1], FinalPosition[2], FinalPosition[3], FinalPosition[4]);
                                    ARR[i] = meminfo;
                                    break;
                                }
                                if (i + 1 >= ARR.Count)
                                {
                                    if (FinalPosition[0] == "")
                                     break;

                                    M_ratio_3D(FinalPosition);
                                    meminfo = new Info(FinalPosition[0], FinalPosition[1], FinalPosition[2], FinalPosition[3], FinalPosition[4]);
                                    ARR.Add(meminfo);
                                    break;
                                }
                            }
                        }
                       
                    }
                }
                catch (Exception e)
                {
                    //값이 없음.
                    //meminfo.MemberID = "";
                    //meminfo.Xposi = "0";
                    //meminfo.Yposi = "0";
                    //meminfo.Zposi = "0";
                    //ARR.Add(meminfo);
                    e.ToString();
                }
                        #endregion
            }
           
        }
        //좌표 계산
        void M_ratio_3D(string[] _Point)
        {
            //X좌표
            double X = (int.Parse(_Point[1]) * 460) / 24500;
            _Point[1] = (215 - (int)X).ToString();  //215 --> 230이었음;
            //Y좌표
            double Y = (int.Parse(_Point[2]) * 570) / 32000;
            _Point[2] = (310 - (int)Y).ToString();      //320이었으
        }

        public void InitCamera() //initilizing pos of x,yz,
        {
            eyex = -80f;
            eyey = 70f;
            eyez = -0f;

            centerx = 0;
            centery = 0;
            centerz = 0;
            Look();

            //eyex = -90.1f;
            //eyey = 50.3f;
            //eyez = -30.4f;
            //centerx = -3;
            //centery = 2;
            //centerz = -2;
            //Look();        
        }
      
        public void Look()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluLookAt(eyex, eyey, eyez, centerx, centery, centerz, 0, 1, 0);           
        }

        static public float AngleToRad(double pAngle)
        {
            return (float)(pAngle * div1);
        }

        static public float RadToAngle(double pAngle)
        {
            return (float)(pAngle * div2);
        }

        public void UpdateDirVector()
        {
            k = Math.Cos(AngleToRad((double)yaw));
            i = -Math.Sin(AngleToRad((double)yaw));
            j = Math.Sin(AngleToRad((double)pitch));          
        }
        
        public static void CenterMouse()
        {
            Winapi.SetCursorPos(Form1.FormPos.X + 512, Form1.FormPos.Y + 384);
        }

        public void Update(int pressedButton, Building building, bool Print_All, bool Rewind, bool AllPrintXml)
        {
            #region Target chamber
            Gl.glScalef(0.1f, 0.1f, 0.1f);

            #region 마우스 왼
            if (pressedButton == 1) // pressed the left button of mouse
            {
                if (!Collision.CheckCollision(new Point3D(eyex - (float)i * forwardSpeed, eyez - (float)k * forwardSpeed, 0)))
                {
                    Pointer position = new Pointer();
                    Winapi.GetCursorPos(ref position);
                    
                    int difX = Form1.FormPos.X + 512 - position.x;
                    int difY = Form1.FormPos.Y + 384 - position.y;

                    Gl.glPopMatrix();
                    #region 마우스 위, 아래 이동!!!!!!!!
                        if (position.y < Form1.FormPos.Y + 384)
                        {
                            pitch -= rotationSpeed * difY;
                            building.BuildingDrawY(true);
                            
                          //  rectangleDraw.r_Rectangle_DrawY(true, building.rot, building.posIX, building.posIY, building.posIZ);

                        }
                        else
                        {
                            if (position.y > Form1.FormPos.Y + 384)
                            {
                                pitch += rotationSpeed * -difY;
                                building.BuildingDrawY(false);
                                
                               // rectangleDraw.r_Rectangle_DrawY(false, building.rot, building.posIX, building.posIY, building.posIZ);
                            }
                        }
                    #endregion

                    Gl.glPopMatrix();
                    #region 마우스 좌, 우 이동!!!!!!!!!!!

                        if (position.x < Form1.FormPos.X + 512)
                        {
                            yaw += rotationSpeed * -difX;
                          
                            building.BuildingDrawX(true);
                          //  rectangleDraw.r_Rectangle_DrawX(true, building.rot, building.posIX, building.posIY, building.posIZ);
                        }

                        else
                        {
                            if (position.x > Form1.FormPos.X + 512)
                            {
                                yaw -= rotationSpeed * difX;
                                
                                building.BuildingDrawX(false);
                             //   rectangleDraw.r_Rectangle_DrawX(false, building.rot, building.posIX, building.posIY, building.posIZ);
                            }

                        }
                    // Gl.glPopMatrix(); 
                    #endregion

                    Gl.glPopMatrix();
                }
                CenterMouse();
            }
            #endregion
          
            #region 마우스 오른
          if (pressedButton == -1) // pressed the left button of mouse
                    {
                        for (int num = 0; num < ARR.Count; num++)
                        {
                            int []x = {int.Parse(ARR[num].Xposi)-10, int.Parse(ARR[num].Xposi)+10};
                            int []y = {int.Parse(ARR[num].Yposi)-10, int.Parse(ARR[num].Yposi)+10};
                           
                            if ((x[0] <= Cursor.Position.X && Cursor.Position.X <= x[1]) && (Cursor.Position.Y <= y[0] && y[1] <= Cursor.Position.Y))
                               c_cursor = ARR[num].MemberID.ToString();

                        }
                    }

            #endregion

            #region 줌인아웃
            if (pressedButton == 3) // pressed the left button of mouse
            {
                if (!Collision.CheckCollision(new Point3D(eyex - (float)i * forwardSpeed, eyez - (float)k * forwardSpeed, 0)))
                {
                    Pointer position = new Pointer();
                    Winapi.GetCursorPos(ref position);

                        Gl.glPopMatrix();
                    //여기 바뀌면됨.
                        if (c_Zoom < 7.3)
                        {
                            Gl.glScalef(1.2f, 1.2f, 1.2f);
                            c_Zoom += 1.2;
                        }
                }
                Gl.glPopMatrix();
            }

            if (pressedButton == 4) // pressed the left button of mouse
            {
                //  Gl.glPopMatrix();
                if (!Collision.CheckCollision(new Point3D(eyex + (float)i * forwardSpeed, eyez + (float)k * forwardSpeed, 0)))
                {
                    Pointer position = new Pointer();
                    Winapi.GetCursorPos(ref position);
                    Gl.glPopMatrix();

                    if (c_Zoom > 1.2)
                    {
                        Gl.glScalef(0.8f, 0.8f, 0.8f);
                        c_Zoom -= 1.45;
                    }
                }
                Gl.glPopMatrix();
            }
            #endregion

            Gl.glPopMatrix();
            building.Draw();
            sky.Draw();       

            #endregion
            
            #region 실시간 좌표찍기
            #region
            //실시간으로 좌표 받기.
            if (Print_All != true)
            {
                #region 전체출력
                if (xml.B_AllxmlData == true)
                {
                    if (AllPrintXml == true)
                    {
                        if (th1.ThreadState.ToString() == "Unstarted")
                        {
                            th1.Start();
                        }
                        if (AllViewState >= 1)
                        {
                            if (Old_AllViewState == AllViewState)
                            {
                                for (int i = 0; i < FinalXmlPosi.Count; i++)
                                {
                                    if (Floor_num == FinalXmlPosi[i].Floor)
                                    {
                                        rectangleDraw.rectangle_Text(m_name[i], building.posIX, building.posIY, building.posIZ, int.Parse(FinalXmlPosi[i].Xposi), int.Parse(FinalXmlPosi[i].Zposi), int.Parse(FinalXmlPosi[i].Yposi));
                                        rectangleDraw.rectangleDraw(int.Parse(FinalXmlPosi[i].Xposi), int.Parse(FinalXmlPosi[i].Zposi), int.Parse(FinalXmlPosi[i].Yposi));

                                        bool exits = false;
                                        ListViewItem ar = new ListViewItem(new string[] { m_name[i], FinalXmlPosi[i].LDate });

                                        int a = 0;
                                        foreach (ListViewItem itemn in list_v.Items)
                                        {
                                            if (itemn.SubItems[0].Text == ar.SubItems[0].Text)
                                            {
                                                list_v.Items[a].SubItems[0].Text = m_name[i];
                                                list_v.Items[a].SubItems[1].Text = FinalXmlPosi[i].LDate;
                                                exits = true;
                                                break;
                                            }
                                            a++;
                                        }
                                        if (exits != true)
                                            list_v.Items.Add(ar);

                                        num1++;
                                    }
                                }
                                Old_AllViewState++;   
                            }
                            else
                            {
                                for (int i = 0; i < FinalXmlPosi.Count; i++)
                                {
                                    if (Floor_num == FinalXmlPosi[i].Floor)
                                    {
                                        rectangleDraw.rectangle_Text(m_name[i], building.posIX, building.posIY, building.posIZ, int.Parse(FinalXmlPosi[i].Xposi), int.Parse(FinalXmlPosi[i].Zposi), int.Parse(FinalXmlPosi[i].Yposi));
                                        rectangleDraw.rectangleDraw(int.Parse(FinalXmlPosi[i].Xposi), int.Parse(FinalXmlPosi[i].Zposi), int.Parse(FinalXmlPosi[i].Yposi));

                                        //bool exits = false;
                                        //ListViewItem ar = new ListViewItem(new string[] { m_name[i], FinalXmlPosi[i].LDate });

                                        //int a = 0;
                                        //foreach (ListViewItem itemn in list_v.Items)
                                        //{
                                        //    if (itemn.SubItems[0].Text == ar.SubItems[0].Text)
                                        //    {
                                        //        list_v.Items[a].SubItems[0].Text = m_name[i];
                                        //        list_v.Items[a].SubItems[1].Text = FinalXmlPosi[i].LDate;
                                        //        exits = true;
                                        //        break;
                                        //    }
                                        //    a++;
                                        //}
                                        //if (exits != true)
                                        //    list_v.Items.Add(ar);

                                    }
                                }                      
                            }
                        }

                        if (num1 == xml.XML_ARR.Count)
                        {
                            AllPrintXml = false;
                            num1 = 0;
                            FinalXmlPosi.Clear();
                        }
                    }
                }
                #endregion

                #region 하나 출력
                if (xml.B_SingleData == true)
                {
                    if (Rewind == false && AllPrintXml == false)
                    {
                        if (num1 < xml.XML_ARR.Count)
                        {
                            if (th.ThreadState.ToString() == "Unstarted")
                            {
                                th.Start();
                            }

                            if (num1 != num)
                            {
                                if (FinalXmlPosi.Count == 0)
                                {
                                    return;
                                }

                                else if (FinalXmlPosi[num1].Xposi == null)
                                {
                                    return;
                                }

                                else if (Floor_num == FinalXmlPosi[num1].Floor)
                                {
                                    rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, int.Parse(FinalXmlPosi[num1].Xposi), int.Parse(FinalXmlPosi[num1].Zposi), int.Parse(FinalXmlPosi[num1].Yposi));
                                   
                                    rectangleDraw.rectangleDraw(int.Parse(FinalXmlPosi[num1].Xposi), int.Parse(FinalXmlPosi[num1].Zposi), int.Parse(FinalXmlPosi[num1].Yposi));
                                    /////////////////////////////////////
                                    bool exits = false;
                                    ListViewItem ar = new ListViewItem(new string[] { s_name, FinalXmlPosi[num1].LDate });

                                    int a = 0;
                                    foreach (ListViewItem itemn in list_v.Items)
                                    {
                                        if (itemn.SubItems[0].Text == ar.SubItems[0].Text)
                                        {
                                            list_v.Items[a].SubItems[0].Text = s_name;
                                            list_v.Items[a].SubItems[1].Text = FinalXmlPosi[num1].LDate;
                                            exits = true;
                                            break;
                                        }
                                        a++;
                                    }
                                    if (exits != true)
                                        list_v.Items.Add(ar);
                                    step++;
                                }
                                num1++;
                            }

                            if (step != 0 && num1 != 0)
                            {
                                if (Floor_num == FinalXmlPosi[num1 - 1].Floor)
                                {
                                    rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, int.Parse(FinalXmlPosi[num1-1].Xposi), int.Parse(FinalXmlPosi[num1-1].Zposi), int.Parse(FinalXmlPosi[num1-1].Yposi));

                                    rectangleDraw.rectangleDraw(int.Parse(FinalXmlPosi[num1 - 1].Xposi), int.Parse(FinalXmlPosi[num1 - 1].Zposi), int.Parse(FinalXmlPosi[num1 - 1].Yposi));
                                    ///////////////
                                    bool exits = false;
                                    ListViewItem ar = new ListViewItem(new string[] { s_name, FinalXmlPosi[num1 - 1].LDate });

                                    int a = 0;
                                    foreach (ListViewItem itemn in list_v.Items)
                                    {
                                        if (itemn.SubItems[0].Text == ar.SubItems[0].Text)
                                        {
                                            list_v.Items[a].SubItems[0].Text = s_name;
                                            list_v.Items[a].SubItems[1].Text = FinalXmlPosi[num1 - 1].LDate;
                                            exits = true;
                                            break;
                                        }
                                        a++;
                                    }
                                    if (exits != true)
                                        list_v.Items.Add(ar);
                                }
                            }
                            else
                            { num1 = 0; }
                        }
                    }
                }

                /*
                if (Rewind == true && AllPrintXml == false)
                {
                  //  FinalXmlPosi.Clear();

                    if (num1 < xml.XML_ARR.Count)
                    {
                        if (th.ThreadState.ToString() == "Unstarted")
                        {
                            th.Start();
                        }

                        if (num1 != num)
                        {
                            if (FinalXmlPosi.Count == 0)
                            {
                                return;
                            }

                            else if (FinalXmlPosi[num1].Xposi == null)
                            {
                                return;
                            }

                            else if (Floor_num == FinalXmlPosi[num1].Floor)
                            {
                                rectangleDraw.rectangleDraw(int.Parse(FinalXmlPosi[num1].Xposi), int.Parse(FinalXmlPosi[num1].Zposi), int.Parse(FinalXmlPosi[num1].Yposi));
                                step++;
                            }
                            num1++;
                        }

                        if (step != 0 && num1 != 0)
                        {
                            if (Floor_num == FinalXmlPosi[num1 - 1].Floor)
                            {
                                rectangleDraw.rectangleDraw(int.Parse(FinalXmlPosi[num1 - 1].Xposi), int.Parse(FinalXmlPosi[num1 - 1].Zposi), int.Parse(FinalXmlPosi[num1 - 1].Yposi));
                            }
                        }
                        else
                        { num1 = 0; }
                    }

                }
                */
                #endregion

            }
            #endregion

            else
            {
                bool inout;
                
                f_rffff();         
                for (int num = 0; num < ARR.Count; num++)
                {
                    string textname = null;
                    inout = false;
                    //아디가 존재하는지 여부
                    if (ARR[num].MemberID != "")
                    {
                    //    string textname = xml.S_Id(ARR[num].MemberID);
                        for (int j = 0; j < text_ID.Length; j++)
                        {
                            if (text_ID[j] == ARR[num].MemberID)
                            {
                               textname = text_name[j];
                                break;
                            }
                        }
                            //층을 비교                            
                            if (Floor_num == ARR[num].Floor)
                            {
                                //강의실 안에 있는 지 비교..ㅠㅠ
                                for (int i = 0; i < M_F_ARR.Count; i++)
                                {
                                    if (f_inner[i, num] == true)
                                    {
                                        inout = true;
                                    }
                                }
                                if (!inout)
                                {
                                    rectangleDraw.rectangleDraw(int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                                    //콤보박스의 아이디와 비교                      
                                    if (now_Id == ARR[num].MemberID.ToString() && now_Id != null)
                                    {
                                        rectangleDraw.R_rectangleDraw(textname, building.posIX, building.posIY, building.posIZ, int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                                        //rectangleDraw.R_rectangleDraw(int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                                    }
                                    else
                                    {
                                        rectangleDraw.rectangle_Text(textname, building.posIX, building.posIY, building.posIZ, int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                                    }
                                }

                            }
                            else if (Floor_num == "ALL")
                            {
                                int z = 0;
                                // string textname = xml.S_Id(ARR[num].MemberID);
                                switch (ARR[num].Floor)
                                {
                                    case "F1": z = -35; break;
                                    case "F2": z = 140; break;
                                    case "F3": z = 260; break;
                                }

                                rectangleDraw.rectangleDraw(int.Parse(ARR[num].Xposi), z, int.Parse(ARR[num].Yposi));
                                if (now_Id == ARR[num].MemberID.ToString() && now_Id != null)
                                {
                                    rectangleDraw.R_rectangleDraw(textname, building.posIX, building.posIY, building.posIZ, int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                                }
                                else
                                    rectangleDraw.rectangle_Text(textname, building.posIX, building.posIY, building.posIZ, int.Parse(ARR[num].Xposi), z, int.Parse(ARR[num].Yposi));

                            }
                    }
                }

                f_rectang(building);
                #region
                //for (int num = 0; num < ARR.Count; num++)
                //{
                //    //아디가 존재하는지 여부
                //    if (ARR[num].MemberID != "")
                //    {
                //        //층을 비교
                //        if (Floor_num == ARR[num].Floor)
                //        {

                //            rectangleDraw.rectangle_Text(ARR[num].MemberID, building.posIX, building.posIY, building.posIZ, int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                //            //콤보박스의 아이디와 비교                      
                //            if (now_Id == ARR[num].MemberID.ToString() && now_Id != null)
                //            {

                //                rectangleDraw.R_rectangleDraw(int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                //            }
                //            else
                //                rectangleDraw.rectangleDraw(int.Parse(ARR[num].Xposi), int.Parse(ARR[num].Zposi), int.Parse(ARR[num].Yposi));
                //        }
                //        else if (Floor_num == "ALL")
                //        {
                //            int z = 0;
                //            switch (ARR[num].Floor)
                //            {
                //                case "F1": z = -35; break;
                //                case "F2": z = 140; break;
                //                case "F3": z = 260; break;
                //            }


                //            rectangleDraw.rectangle_Text(ARR[num].MemberID, building.posIX, building.posIY, building.posIZ, int.Parse(ARR[num].Xposi), z, int.Parse(ARR[num].Yposi));

                //            if (now_Id == ARR[num].MemberID.ToString() && now_Id != null)
                //            {
                //                rectangleDraw.R_rectangleDraw(int.Parse(ARR[num].Xposi), z, int.Parse(ARR[num].Yposi));
                //            }
                //            else
                //                rectangleDraw.rectangleDraw(int.Parse(ARR[num].Xposi), z, int.Parse(ARR[num].Yposi));

                //        }
                //    }
                //}
                #endregion
            }
                   
               
            #endregion
                //바닥붙이기
                rectangleDraw.Un_Floor(Floor_num);
                //rectangleDraw.Un_Floor(Floor_num);
                UpdateDirVector();

                Look();
            
        }
        public void f_rffff()
        {
            if (Floor_num != "ALL" && Floor_num != "F1")
            {
                M_F_ARR = xml.Floor_count(Floor_num);
                f_inner = new bool[M_F_ARR.Count, ARR.Count];
                //  f_inner.Initialize
                count = new int[M_F_ARR.Count];
                // f_inner.Initialize();
                for (int i = 0; i < M_F_ARR.Count; i++)
                {
                    for (int j = 0; j < ARR.Count; j++)
                    {
                        if (Floor_num == ARR[j].Floor)
                        {
                            in_FloorInfo = new in_floorInfo();
                            if ((int.Parse(ARR[j].Xposi) >= int.Parse(M_F_ARR[i].RBXposi) && int.Parse(ARR[j].Xposi) <= int.Parse(M_F_ARR[i].LTXposi)) && ((int.Parse(M_F_ARR[i].LTYposi) >= int.Parse(ARR[j].Yposi) && int.Parse(ARR[j].Yposi) >= int.Parse(M_F_ARR[i].RBYposi))))
                            {
                                count[i] += 1;
                                f_inner[i, j] = true;
                            }
                            //Test해봐야됨.
                            else
                            {
                                f_inner[i, j] = false;
                                //만일 처음에 들어올때면 0 , 왜냐면
                                if (count[i] == 0)
                                    count[i] = 0;
                                else
                                {
                                    if (f_inner[i, j] == true)
                                    { 
                                        count[i] -= 1;
                                    }
                                    count[i] += 0;
                                }
                            }
                        }
                        else
                            count[i] += 0;
                    }
                }
            }

        }

        public void f_rectang(Building building)
        {
            if (Floor_num != "ALL" && Floor_num != "F1")
            {
                M_F_ARR = xml.Floor_count(Floor_num);
                f_inner = new bool[M_F_ARR.Count, ARR.Count];
              //  f_inner.Initialize              
               // f_inner.Initialize();
                for (int i = 0; i < M_F_ARR.Count; i++)
                {
                    int x = int.Parse(M_F_ARR[i].LTXposi) - (int.Parse(M_F_ARR[i].LTXposi) - int.Parse(M_F_ARR[i].RBXposi)) / 2;
                    int y = int.Parse(M_F_ARR[i].LTYposi) - (int.Parse(M_F_ARR[i].LTYposi) - int.Parse(M_F_ARR[i].RBYposi)) / 2;

                    rectangleDraw.C_number(x, y, count[i], M_F_ARR[i].Name,building.posIX, building.posIY, building.posIZ);
                }
            }
        }


        public void SendAndroidMessage()
        {
            uri = "http://61.81.99.100:8080/Xml/Service1.svc/Message";
                /*
                 using (HttpResponseMessage response = new HttpClient().Get(uri))
                    {
                        string res = response.Content.ReadAsString();
                        string[] R = res.Split('>');
                        string[] R2 = R[1].Split('<');
                        FinalPosition = R2[0].Split(' ');
                    }
             
                 */
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);

                req.Method = "Post";
                req.ContentType = "application/x-www-form-urlencoded;";
                req.ContentLength = 30;

                string strRequest = "뿌웅";
                StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
                streamOut.Write(strRequest);
                streamOut.Close();
 
        }

      
        public void SendPosiArr()
        {
            FinalXmlPosi.Clear();
            foreach (Info str in xml.XML_ARR)
            {
                Thread.Sleep(500);
                Info temp = new Info();
                temp.MemberID = str.MemberID;
                temp.Xposi = str.Xposi;
                temp.Yposi = str.Yposi;
                temp.Zposi = str.Zposi;
                temp.Floor = str.Floor;
                temp.LDate = str.LDate;
                FinalXmlPosi.Add(temp);              
                num++;
            }    
        }

        public void AllViewViewXml()
        {
            int[] Section = new int[XmlAllID.Length];
            int xmlCount = 0;
            int[] Section2 = new int[XmlAllID.Length];
            int[] Section3 = new int[XmlAllID.Length];
            int j = 0;
            AllViewState = 0;
            Old_AllViewState = 1;
            FinalXmlPosi.Clear();

            ArrayList Final_IDARR = new ArrayList();
            foreach (string str in XmlAllID)
            {
                for (int i = 0; i < xml.XML_ARR.Count; i++)
                {
                    if (str == xml.XML_ARR[i].MemberID)
                    {
                        Section[j] = i;
                        break;
                    }
                }
                j++;
            }

            int sec2 = 0;

            foreach (int str in Section)
            {
                Section2[sec2] = str;
                sec2++;
            }


            int sec3 = 0;

            foreach (int str in Section)
            {
                Section3[sec3] = str;
                sec3++;
            }

            while (xmlCount <= xml.XML_ARR.Count)
            {
                Thread.Sleep(500);

                for (int i = 0; i < XmlAllID.Length; i++)
                {
                    if (i + 1 < XmlAllID.Length)
                    {
                        if (XmlAllID[i].ToString() == xml.XML_ARR[int.Parse(Section[i].ToString())].MemberID.ToString() && int.Parse(Section[i + 1].ToString()) > int.Parse(Section2[i].ToString()))
                        {
                            if (Section2[i] != Section[i + 1])
                            {
                                //  Thread.Sleep(500);
                                Info temp = new Info();
                                temp.MemberID = xml.XML_ARR[int.Parse(Section2[i].ToString())].MemberID;
                                temp.Xposi = xml.XML_ARR[int.Parse(Section2[i].ToString())].Xposi;
                                temp.Yposi = xml.XML_ARR[int.Parse(Section2[i].ToString())].Yposi;
                                temp.Zposi = xml.XML_ARR[int.Parse(Section2[i].ToString())].Zposi;
                                temp.Floor = xml.XML_ARR[int.Parse(Section2[i].ToString())].Floor;
                                temp.LDate = xml.XML_ARR[int.Parse(Section2[i].ToString())].LDate;

                                if (FinalXmlPosi.Count == 0)
                                {
                                    FinalXmlPosi.Add(temp);
                                    //ID추가
                                    Final_IDARR.Add(temp.MemberID);
                                    num++;
                                    xmlCount++;

                                    Section2[i] = Section2[i] + 1;
                                }

                                else
                                {
                                    for (int m = 0; m < FinalXmlPosi.Count; m++)
                                    {
                                        //if (temp.MemberID == FinalXmlPosi[m].MemberID)
                                        if (temp.MemberID == FinalXmlPosi[m].MemberID)
                                        {
                                            FinalXmlPosi[m].MemberID = temp.MemberID;
                                            FinalXmlPosi[m].Xposi = temp.Xposi;
                                            FinalXmlPosi[m].Yposi = temp.Yposi;
                                            FinalXmlPosi[m].Zposi = temp.Zposi;
                                            FinalXmlPosi[m].Floor = temp.Floor;
                                            FinalXmlPosi[m].LDate = temp.LDate;
                                            num++;


                                            if (Section3[i] < xml.XML_ARR.Count && Section2[i] < xml.XML_ARR.Count)
                                            {
                                                Section2[i] = Section2[i] + 1;
                                                xmlCount++;
                                            }

                                        }

                                        else
                                        {
                                            if (!Final_IDARR.Contains(temp.MemberID))
                                            {
                                                FinalXmlPosi.Add(temp);
                                                //ID추가 
                                                Final_IDARR.Add(temp.MemberID);
                                                num++;


                                                if (Section3[i] < xml.XML_ARR.Count && Section2[i] < xml.XML_ARR.Count)
                                                {
                                                    Section2[i] = Section2[i] + 1;
                                                    xmlCount++;
                                                }

                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }


                    #region 추가 해야됨
                    else
                    {
                        if (XmlAllID[i].ToString() == xml.XML_ARR[int.Parse(Section[i].ToString())].MemberID.ToString() && xml.XML_ARR.Count > int.Parse(Section3[i].ToString()))
                        {
                            if (Section3[i] != xml.XML_ARR.Count)
                            {
                                // Thread.Sleep(500);
                                Info temp = new Info();
                                temp.MemberID = xml.XML_ARR[int.Parse(Section3[i].ToString())].MemberID;
                                temp.Xposi = xml.XML_ARR[int.Parse(Section3[i].ToString())].Xposi;
                                temp.Yposi = xml.XML_ARR[int.Parse(Section3[i].ToString())].Yposi;
                                temp.Zposi = xml.XML_ARR[int.Parse(Section3[i].ToString())].Zposi;
                                temp.Floor = xml.XML_ARR[int.Parse(Section3[i].ToString())].Floor;
                                temp.LDate = xml.XML_ARR[int.Parse(Section3[i].ToString())].LDate;

                                for (int m = 0; m < FinalXmlPosi.Count; m++)
                                {
                                    if (temp.MemberID == FinalXmlPosi[m].MemberID)
                                    {
                                        FinalXmlPosi[m].MemberID = temp.MemberID;
                                        FinalXmlPosi[m].Xposi = temp.Xposi;
                                        FinalXmlPosi[m].Yposi = temp.Yposi;
                                        FinalXmlPosi[m].Zposi = temp.Zposi;
                                        FinalXmlPosi[m].Floor = temp.Floor;
                                        FinalXmlPosi[m].LDate = temp.LDate;
                                        num++;
                                        if (Section3[i] < xml.XML_ARR.Count && Section2[i] < xml.XML_ARR.Count)
                                        {
                                            Section3[i] = Section3[i] + 1;
                                            xmlCount++;
                                        }
                                    }

                                    else
                                    {
                                        if (!Final_IDARR.Contains(temp.MemberID))
                                        {
                                            FinalXmlPosi.Add(temp);
                                            //추가
                                            Final_IDARR.Add(temp.MemberID);
                                            num++;
                                            if (Section3[i] < xml.XML_ARR.Count && Section2[i] < xml.XML_ARR.Count)
                                            {
                                                Section3[i] = Section3[i] + 1;
                                                xmlCount++;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    #endregion

                }
                AllViewState++;

            }


        }
    }    
}
