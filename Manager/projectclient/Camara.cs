using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;
using System.Drawing;
using ShadowEngine;
using System.Windows.Forms;
using projectclient.ServiceReference2;
//using projectclient.ServiceReference4;
using System.Threading;

namespace projectclient
{
    public class Camara
    {
        #region Camera constants
        const double div1 = Math.PI / 180;
        //const double div2 = 180 / Math.PI;

        #endregion
        #region Private atributes
        public string Floor_num;
        static float eyex, eyey, eyez;
        static float forwardSpeed = 0.2f;
        static float rotationSpeed = 1 / 5f;
        static float centerx, centery, centerz;
        static float yaw, pitch;
        static double i, j, k;
        double c_Zoom = 0.8;
        Exterior sky = new Exterior();
        public int num;
        int[] data = new int[3];
        ListView list;
        public String[] position;
        //======================
        //Control con = new Control();
        //=======================
        #endregion
        public r_Rectangle rectangleDraw = new r_Rectangle();
        public string s_name;

        public Camara(ListView list)
        {
            this.list = list;
        }
        public Camara(int num)
        {
            this.num = num;
        }
        //public void PlaceCamera(int width, int height)
        //{
        //    Gl.glMatrixMode(Gl.GL_PROJECTION);
        //    Gl.glLoadIdentity();
        //    Glu.gluPerspective(50, width / (float)height, 0.1f, 300);
        //}

        public void InitCamera() //initilizing pos of x,yz,
        {
            eyex = -80;
            eyey = 70;
            eyez = -0f;

            centerx = 0;
            centery = 0;
            centerz = 0;
            Look();
        }
        //#region get& ser
        //public static float Pitch
        //{
        //    get { return Camara.pitch; }
        //    set { Camara.pitch = value; }
        //}

        //public static float Yaw
        //{
        //    get { return Camara.yaw; }
        //    set { Camara.yaw = value; }
        //}
        //#endregion
        public void Look()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluLookAt(eyex, eyey, eyez, centerx, centery, centerz, 0, 1, 0);
        }
        public static void CenterMouse()
        {
            Winapi.SetCursorPos(MainForm.FormPos.X + 512, MainForm.FormPos.Y + 384);
        }
        static public float AngleToRad(double pAngle)
        {
            return (float)(pAngle * div1);
        }

        //static public float RadToAngle(double pAngle)
        //{
        //    return (float)(pAngle * div2);
        //}

        public void UpdateDirVector()
        {
            k = Math.Cos(AngleToRad((double)yaw));
            i = -Math.Sin(AngleToRad((double)yaw));
            j = Math.Sin(AngleToRad((double)pitch));
        }
        //좌표변환
        public Vector3 screenTo3D(int x, int y, int z)
        {
            double X = (x * 460) / 24500;
            x = (215 - (int)X);
            //Y좌표
            double Z = (z * 590) / 32000;
            z = (310 - (int)Z);
            return new Vector3(x, y, z);
        }
        public void Update(int pressedButton, Building building, bool Print_All, bool OneSelect, bool M_ListB)
        {
            #region Target chamber
            #region 마우스 왼
            if (pressedButton == 1) // pressed the left button of mouse
            {
                if (!Collision.CheckCollision(new Point3D(eyex - (float)i * forwardSpeed, eyez - (float)k * forwardSpeed, 0)))
                {
                    Pointer position = new Pointer();
                    // Vector2 position = new Vector2();
                    Winapi.GetCursorPos(ref position);

                    int difX = MainForm.FormPos.X + 512 - position.x;
                    int difY = MainForm.FormPos.Y + 384 - position.y;

                    Gl.glPopMatrix();
                    #region 마우스 위, 아래 이동!!!!!!!!
                    if (position.y < MainForm.FormPos.Y + 384)
                    {
                        pitch -= rotationSpeed * difY;
                        building.BuildingDrawY(true);
                        //  rectangleDraw.r_Rectangle_DrawY(true, building.rot, building.posIX, building.posIY, building.posIZ);
                    }
                    else
                    {
                        if (position.y > MainForm.FormPos.Y + 384)
                        {
                            pitch += rotationSpeed * -difY;
                            building.BuildingDrawY(false);

                            // rectangleDraw.r_Rectangle_DrawY(false, building.rot, building.posIX, building.posIY, building.posIZ);
                        }
                    }
                    #endregion

                    Gl.glPopMatrix();
                    #region 마우스 좌, 우 이동!!!!!!!!!!!

                    if (position.x < MainForm.FormPos.X + 512)
                    {
                        yaw += rotationSpeed * -difX;

                        building.BuildingDrawX(true);
                        //rectangleDraw.r_Rectangle_DrawX(true, building.rot, building.posIX, building.posIY, building.posIZ);
                    }

                    else
                    {
                        if (position.x > MainForm.FormPos.X + 512)
                        {
                            yaw -= rotationSpeed * difX;

                            building.BuildingDrawX(false);
                            //rectangleDraw.r_Rectangle_DrawX(false, building.rot, building.posIX, building.posIY, building.posIZ);
                        }

                    }
                    // Gl.glPopMatrix(); 
                    #endregion

                    Gl.glPopMatrix();
                }
                //CenterMouse();
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
            building.Dibujar(Floor_num);
            sky.Draw();

            #endregion

            #region 출력
            f_rectang(building);
            #region 검색출력
            //검색출력
            if (Print_All != true)
            {
                //검색 출력
                if (OneSelect != true)
                {
                    string x = "99999";
                    int y = 0;
                    string z = "99999";      
                    String getFloor = String.Empty;

                   position = MainForm.mainForm.s_time.turn();
                   foreach (ListViewItem itemRow in MainForm.mainForm.s_time.listView1.CheckedItems)
                   {
                       String id = itemRow.SubItems[1].Text;
                       for (int i = num; i < position.Length; i++)
                       {
                           bool isbool = false;
                           if (position != null)
                           {
                               string[]  token = position[i].Split('#');
                               //x = Math.Abs(Convert.ToInt32(token[2]));
                               //z = Math.Abs(Convert.ToInt32(token[3]));
                               x = token[2];
                               z = token[3];
                               getFloor = token[5];
                               s_name = token[0];

                               if (token[1] == id)
                               {
                                   isbool = true;
                               }
                           }
                           if (isbool == true)
                           {
                               switch (Floor_num)
                               {
                                   case "ALL":
                                       {
                                           Vector3 get = screenTo3D(int.Parse(x), 250, int.Parse(z));
                                           rectangleDraw.rectangleDraw(get.X, get.Y, get.Z);
                                           rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, 250, get.Z);
                                       }
                                       break;
                                   case "F3":
                                       {
                                           if (getFloor == "3")
                                           {
                                               Vector3 get = screenTo3D(int.Parse(x), y, int.Parse(z));
                                               rectangleDraw.rectangleDraw(get.X, get.Y, get.Z);
                                               rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, get.Y, get.Z);
                                           }
                                       }
                                       break;
                               }
                               num++;
                               break;
                           }
                       }
                   }
                }           
            #endregion
                #region 리스트뷰출력(체크 출력)
                //미아리스트뷰출력
                else if (OneSelect == true)
                {
                    //string selectid = MainForm.mainForm.sendid(); // 메이폼에서 선택한 이름을 받아와서 넣어준다 
                    string x = "99999";
                    int y = 0;
                    string z = "99999";
                    String getFloor = String.Empty;
                    String e_name = null;

                    if (M_ListB != false)
                    {
                        //MessageBox.Show("미아목록 체크있음");
                        bool signab = false;
                        foreach (ListViewItem itemRow in list.Items)
                        {
                            String id = itemRow.SubItems[0].Text;
                            e_name = itemRow.SubItems[1].Text;

                            foreach (ListViewItem m_infoitem in MainForm.mainForm.listViewInfo.CheckedItems)
                            {
                                String m_name = m_infoitem.SubItems[0].Text;
                                String m_name1 = m_infoitem.SubItems[1].Text;
                                if (e_name == m_name)
                                {
                                    signab = true;
                                }
                                else if (e_name == m_name1)
                                {
                                    signab = true;
                                }
                            }
                            if (signab == true)
                            {
                                String[] position = MainForm.mainForm.proxy.MyPosition(id);

                                if (position != null)
                                {
                                    string[] token = position[0].Split('#');
                                    //x = Math.Abs(Convert.ToInt32(token[0]));
                                    //z = Math.Abs(Convert.ToInt32(token[1]));
                                    x = token[0];
                                    z = token[1];
                                    getFloor = token[2];
                                    s_name = token[3];
                                    //DateTime oldDate = DateTime.Parse(token[4]);
                                    //DateTime newDate = DateTime.Now;

                                    //// Difference in days, hours, and minutes.
                                    //TimeSpan ts = newDate - oldDate;

                                    //// Difference in days.
                                    //int differenceInDays = ts.Seconds;
                                    //if (differenceInDays >= 3)
                                    //{
                                    //    MainForm.mainForm.proxy.Logoutmember(s_name);
                                    //    MessageBox.Show(differenceInDays.ToString());
                                    //}
                                    //else
                                    //{
                                    //    token = position[0].Split('#');
                                    //    x = Math.Abs(Convert.ToInt32(token[0]));
                                    //    z = Math.Abs(Convert.ToInt32(token[1]));
                                    //    getFloor = token[2];
                                    //    s_name = token[3];
                                    //}
                                    
                                }
                            }

                            switch (Floor_num)
                            {
                                case "ALL":
                                    {
                                        Vector3 get = screenTo3D(int.Parse(x), 250, int.Parse(z));
                                        rectangleDraw.missingrectangleDraw(get.X, get.Y, get.Z);
                                        signab = false;
                                        rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, 250, get.Z);
                                    }
                                    break;
                                case "F3":
                                    {
                                        if (getFloor == "3")
                                        {
                                            Vector3 get = screenTo3D(int.Parse(x), y, int.Parse(z));
                                            rectangleDraw.missingrectangleDraw(get.X, get.Y, get.Z);
                                            signab = false;
                                            rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, get.Y, get.Z);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else//메인리스트뷰출력
                    {
                        bool sign = false;
                        foreach (ListViewItem itemRow in list.CheckedItems)
                        {
                            String id = itemRow.SubItems[0].Text;
                            if (MainForm.mainForm.listViewInfo.Items.Count != 0)
                            {
                                e_name = itemRow.SubItems[1].Text;
                                foreach (ListViewItem m_infoitem in MainForm.mainForm.listViewInfo.Items)
                                {
                                    String m_name = m_infoitem.SubItems[0].Text;
                                    String m_name1 = m_infoitem.SubItems[1].Text;
                                    if (e_name == m_name)
                                    {
                                        sign = true;
                                    }
                                    else if (e_name == m_name1)
                                    {
                                        sign = true;
                                    }
                                }
                            }
                            String[] position = MainForm.mainForm.proxy.MyPosition(id);
                            if (position != null)
                            {
                                string[] token = position[0].Split('#');
                                //x = Math.Abs(Convert.ToInt32(token[0]));
                                //z = Math.Abs(Convert.ToInt32(token[1]));
                                x = token[0];
                                z = token[1];
                                getFloor = token[2];
                                s_name = token[3];
                                //DateTime oldDate = DateTime.Parse(token[4]);
                                //DateTime newDate = DateTime.Now;

                                //// Difference in days, hours, and minutes.
                                //TimeSpan ts = newDate - oldDate;

                                //// Difference in days.
                                //int differenceInDays = ts.Seconds;
                                //if (differenceInDays >= 3)
                                //{
                                //    MainForm.mainForm.proxy.Logoutmember(s_name);
                                //    MessageBox.Show(differenceInDays.ToString());
                                //}
                                //else
                                //{
                                //    token = position[0].Split('#');
                                //    x = Math.Abs(Convert.ToInt32(token[0]));
                                //    z = Math.Abs(Convert.ToInt32(token[1]));
                                //    getFloor = token[2];
                                //    s_name = token[3];
                                //}
                            }

                            switch (Floor_num)
                            {
                                case "ALL":
                                    {
                                        Vector3 get = screenTo3D(int.Parse(x), 250, int.Parse(z));
                                        if (sign != true)
                                        {
                                            rectangleDraw.rectangleDraw(get.X, get.Y, get.Z);
                                        }
                                        else
                                        {
                                            rectangleDraw.missingrectangleDraw(get.X, get.Y, get.Z);
                                            sign = false;
                                        }
                                        rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, 250, get.Z);
                                    }
                                    break;
                                case "F3":
                                    {
                                        if (getFloor == "3")
                                        {
                                            Vector3 get = screenTo3D(int.Parse(x), y, int.Parse(z));
                                            if (sign != true)
                                            {
                                                rectangleDraw.rectangleDraw(get.X, get.Y, get.Z);
                                            }
                                            else
                                            {
                                                rectangleDraw.missingrectangleDraw(get.X, get.Y, get.Z);
                                                sign = false;
                                            }
                                            rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, get.Y, get.Z);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
                #endregion
            #region 전체출력
            //전체출력
            else
            {
                if (OneSelect != true)
                {
                    string x = "99999";
                    int y = 0;
                    string z = "99999";
                    String getFloor = String.Empty;
                    bool sign = false;
                    String e_name = null;
                    foreach (ListViewItem itemRow in list.Items)
                    {
                        String id = itemRow.SubItems[0].Text;
                        if (MainForm.mainForm.listViewInfo.Items.Count != 0)
                        {
                            e_name = itemRow.SubItems[1].Text;
                            foreach (ListViewItem m_infoitem in MainForm.mainForm.listViewInfo.Items)
                            {
                                String m_name = m_infoitem.SubItems[0].Text;
                                String m_name1 = m_infoitem.SubItems[1].Text;
                                if (e_name == m_name)
                                {
                                    sign = true;
                                }
                                else if (e_name == m_name1)
                                {
                                    sign = true;
                                }
                            }
                        }

                        String[] position = MainForm.mainForm.proxy.MyPosition(id);
                        if (position != null)
                        {
                            string[] token = position[0].Split('#');
                            //x = Math.Abs(Convert.ToInt32(token[0]));
                            //z = Math.Abs(Convert.ToInt32(token[1]));
                            x = token[0];
                            z = token[1];
                            getFloor = token[2];
                            s_name = token[3];
                            //DateTime oldDate = DateTime.Parse(token[4]);
                            //DateTime newDate = DateTime.Now;

                            //// Difference in days, hours, and minutes.
                            //TimeSpan ts = newDate - oldDate;

                            //// Difference in days.
                            //int differenceInDays = ts.Seconds;
                            //if (differenceInDays >= 3)
                            //{
                            //    MainForm.mainForm.proxy.Logoutmember(s_name);
                            //    MessageBox.Show(differenceInDays.ToString());
                            //}
                            //else
                            //{
                            //    token = position[0].Split('#');
                            //    x = Math.Abs(Convert.ToInt32(token[0]));
                            //    z = Math.Abs(Convert.ToInt32(token[1]));
                            //    getFloor = token[2];
                            //    s_name = token[3];
                            //}
                        }
                        

                        switch (Floor_num)
                        {
                            case "ALL":
                                {
                                    Vector3 get = screenTo3D(int.Parse(x), 250, int.Parse(z));
                                    if (sign != true)
                                    {
                                        rectangleDraw.rectangleDraw(get.X, get.Y, get.Z);
                                    }
                                    else
                                    {
                                        rectangleDraw.missingrectangleDraw(get.X, get.Y, get.Z);
                                        sign = false;
                                    }
                                    rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, 250, get.Z);
                                    //rectangleDraw.rectangle_classText("301", building.posIX, building.posIY, building.posIZ, 190, -20, -140);

                                }
                                break;
                            case "F3":
                                {
                                    if (getFloor == "3")
                                    {
                                        Vector3 get = screenTo3D(int.Parse(x), y, int.Parse(z));
                                        if (sign != true)
                                        {
                                            rectangleDraw.rectangleDraw(get.X, get.Y, get.Z);
                                        }
                                        else
                                        {
                                            rectangleDraw.missingrectangleDraw(get.X, get.Y, get.Z);
                                            sign = false;
                                        }
                                        rectangleDraw.rectangle_Text(s_name, building.posIX, building.posIY, building.posIZ, get.X, get.Y, get.Z);
                                    }
                                }
                                break;
                        }
                    }
            #endregion
                }
            }
            #endregion

            rectangleDraw.Un_Floor(Floor_num);
            UpdateDirVector(); //기능?
            Look();  //기능 ?
        }

        //강의실이름 띄우기
        public void f_rectang(Building building)
        {
            switch (Floor_num)
            {
                case "ALL":
                    {
                        rectangleDraw.rectangle_classText("301", building.posIX, building.posIY, building.posIZ, 190, 240, -140);
                        rectangleDraw.rectangle_classText("302", building.posIX, building.posIY, building.posIZ, 190, 240, 90);
                        rectangleDraw.rectangle_classText("303", building.posIX, building.posIY, building.posIZ, -100, 240, 230);
                        rectangleDraw.rectangle_classText("304", building.posIX, building.posIY, building.posIZ, -100, 240, 92);
                        rectangleDraw.rectangle_classText("305", building.posIX, building.posIY, building.posIZ, -100, 240, 25);
                        rectangleDraw.rectangle_classText("306", building.posIX, building.posIY, building.posIZ, -100, 240, -45);
                        rectangleDraw.rectangle_classText("001", building.posIX, building.posIY, building.posIZ, -70, 240, -130);
                        rectangleDraw.rectangle_classText("002", building.posIX, building.posIY, building.posIZ, 50, 240, 250);
                    }
                    break;
                case "F3":
                    {
                        rectangleDraw.rectangle_classText("301", building.posIX, building.posIY, building.posIZ, 190, -20, -140);
                        rectangleDraw.rectangle_classText("302", building.posIX, building.posIY, building.posIZ, 190, -20, 90);
                        rectangleDraw.rectangle_classText("303", building.posIX, building.posIY, building.posIZ, -100, -20, 230);
                        rectangleDraw.rectangle_classText("304", building.posIX, building.posIY, building.posIZ, -100, -20, 92);
                        rectangleDraw.rectangle_classText("305", building.posIX, building.posIY, building.posIZ, -100, -20, 25);
                        rectangleDraw.rectangle_classText("306", building.posIX, building.posIY, building.posIZ, -100, -20, -45);
                        rectangleDraw.rectangle_classText("001", building.posIX, building.posIY, building.posIZ, -70, -20, -130);
                        rectangleDraw.rectangle_classText("002", building.posIX, building.posIY, building.posIZ, 50, -20, 250);
                    }
                    break;
            }
        }
    }
}


