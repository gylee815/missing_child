using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShadowEngine;
using ShadowEngine.OpenGL;
using Tao.OpenGl;
using ShadowEngine.ContentLoading;
using System.Runtime.InteropServices;
//using projectclient.ServiceReference4;
using projectclient.ServiceReference2;
using System.ServiceModel;
using System.Drawing.Imaging;
using System.IO;
using Tao.FreeGlut;
using System.Collections;

namespace projectclient
{
    public partial class MainForm : Form
    {
        //handle del viewport
        uint hdc;
        int moving;
        static Vector2 formPos;
        bool lines;
        public MainClass controladora;
        public static MainForm mainForm;
        public Service1Client proxy;
        public S_Time s_time;
        //==전체출력
        bool Print_All;
        public bool Rewind;
        public bool PrintSelectDB;
        //======
        public ListView list;
        //콤보박스
        enum Origin : byte { Buling, Floor, Class };
        Origin Turn;
        string class_info;
        bool all_select;
        string c_name;
        //========
        bool OneSelect = false;
        string stabox = "null";
        bool M_ListB = false;
        public String truename = null;


        public static Vector2 FormPos
        {
            get { return MainForm.formPos; }
            set { MainForm.formPos = value; }
        }

        public MainForm()
        {
            mainForm = this;

            InitializeComponent();
            list = listViewMember;
            //InstanceContext site = new InstanceContext(this);
            //proxy = new Service1Client(site);
            proxy = new Service1Client();

            //identificador del lugar en donde voy a dibujar
            controladora = new MainClass(list);
            s_time = new S_Time();
            hdc = (uint)pnlViewPort.Handle;
            //toma el error que sucedio
            string error = "";
            //Comando de inicializacion de la ventana grafica
            OpenGLControl.OpenGLInit(ref hdc, pnlViewPort.Width, pnlViewPort.Height, ref error);

            if (error != "")
            {
                MessageBox.Show("Ocurrio un error al inicializar OpenGl");
            }
            //controladora.Camara.PlaceCamera(this.Width, this.Height);
            controladora.Camara.InitCamera();

            Lighting.SetupLighting();
            ContentManager.SetTextureList("textures\\");
            ContentManager.LoadTextures();
            ContentManager.SetModelList("model\\");
            ContentManager.LoadModels();

            Camara.CenterMouse();

            controladora.CrearObjetos(floor_combo.Text);

            Print_All = true;
            OneSelect = false;
            //콤보박스 
            Turn = Origin.Buling;
            class_info = floor_combo.Text;
            c_name = "303";
            statebox.Text = "전체출력";
            statebox.TextAlign = HorizontalAlignment.Center;
            //statebox.BackColor = Color.White;
        }
        private void tmrPaint_Tick_1(object sender, EventArgs e)
        {
            if (Print_All != true) //Print_All == false
            {
                if (OneSelect != true)  //OneSelect == false
                {
                    if (M_ListB != true)
                    {
                        tmrPaint.Interval = 300;
                        statebox.Text = "검색 중";
                        statebox.BackColor = Color.DarkRed;
                        statebox.ForeColor = Color.White;
                        statebox.TextAlign = HorizontalAlignment.Center;
                    }
                }
            }
            else //Print_All == true
            {
                if (OneSelect != true)
                {
                    tmrPaint.Interval = 25;
                }
            }

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);//초기화-->opengl clean paint
            //this.pnlViewPort.Controls.Clear();

            controladora.Camara.Update(moving, controladora.building, Print_All, OneSelect, M_ListB);
            controladora.DibujarEscena();
            Winapi.SwapBuffers(hdc);
            Gl.glFlush();
            label();
            if (Print_All == true)
            {
                missingchild();
                send();
                relationmember();
            }
        }
        //===============================
        #region 라벨, 기타 버튼 초기화
        private void label()
        {
            label2.Text = DateTime.Now.ToString("hh:mm:ss");
            label3.Text = DateTime.Now.ToString("tt");
            label4.Text = DateTime.Now.ToString("dd");
            label5.Text = DateTime.Now.ToString("MM");
            label6.Text = DateTime.Now.ToString("yyyy");
            label7.Text = DateTime.Now.ToString("dddd");
        }
        #endregion

        //===============================
        #region 실시간전체출력
        //실시간전체출력
        public void send()
        {
            ListViewItem lvi;
            String[] login_id = proxy.WhoisLogon("1");
            txtCurrent.Text = login_id.Length.ToString();
            if (login_id.Length == 0)
                listViewMember.Items.Clear();

            try
            {
                for (int i = 0; i < login_id.Length; i++)
                {
                    string[] token1 = login_id[i].Split('#');
                    lvi = new ListViewItem(new string[] { token1[0], token1[1], token1[2] });
                    String[] logout_id = proxy.WhoisLogon("0");
                    bool exits = false;

                    int j = 0;
                    foreach (ListViewItem itemn in listViewMember.Items)
                    {
                        for (int k = 0; k < logout_id.Length; k++)
                        {
                            string[] token2 = logout_id[k].Split('#');
                            if (token2[0] == itemn.SubItems[0].Text)
                            {
                                ListViewItem listView = listViewMember.Items[j];
                                listViewMember.Items.Remove(listView);
                                listViewMember.Refresh();
                            }
                        }

                        if (itemn.SubItems[0].Text == lvi.SubItems[0].Text)
                        {
                            exits = true;
                            //break;
                        }
                        j++;
                    }
                    if (listViewMember.Items.Count == 0)
                        listViewMember.Items.Clear();
                    if (exits != true)
                        listViewMember.Items.Add(lvi);

                    String s = "C:\\Users\\62\\Desktop\\projectclient서버연결변경\\projectclient\bin\\Debug\textures\\" + token1[1] + "_1.jpg";
                    if (!File.Exists(s))
                    {
                        Bitmapjpg(token1[1]);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void missingchild()
        {
            ListViewItem lvi;
            String[] missing_info = proxy.MissingLogon("1");
            if (missing_info.Length == 0)
                listViewInfo.Items.Clear();

            for (int i = 0; i < missing_info.Length; i++)
            {
                string[] token1 = missing_info[i].Split('#');
                lvi = new ListViewItem(new string[] { token1[0], token1[1], token1[2] });
                String[] logout_id = proxy.MissingLogon("0");
                bool exits = false;
                int j = 0;
                foreach (ListViewItem itemn in listViewInfo.Items)
                {
                    for (int k = 0; k < logout_id.Length; k++)
                    {
                        string[] token2 = logout_id[k].Split('#');
                        if (token2[0] == itemn.SubItems[0].Text)
                        {
                            ListViewItem listView = listViewInfo.Items[j];
                            listViewInfo.Items.Remove(listView);
                            listViewInfo.Refresh();
                        }
                    }

                    if (itemn.SubItems[0].Text == lvi.SubItems[0].Text)
                    {
                        exits = true;
                    }
                    j++;
                }
                if (listViewInfo.Items.Count == 0)
                    listViewInfo.Items.Clear();
                if (exits != true)
                    listViewInfo.Items.Add(lvi);
            }
        }

        public void relationmember()
        {
            ListViewItem lvi;
            String[] relation = proxy.relationmember("1");
            if (relation.Length == 0)
                listView1.Items.Clear();
            string kids = "아이";
            for (int i = 0; i < relation.Length; i++)
            {
                string[] token1 = relation[i].Split('#');
                lvi = new ListViewItem(new string[] { token1[0], token1[1], token1[2], kids, token1[3], token1[4] });
                String[] logout_id = proxy.relationmember("0");
                bool exits = false;
                int j = 0;
                foreach (ListViewItem itemn in listView1.Items)
                {
                    for (int k = 0; k < logout_id.Length; k++)
                    {
                        string[] token2 = logout_id[k].Split('#');
                        if (token2[0] == itemn.SubItems[0].Text)
                        {
                            ListViewItem listView = listView1.Items[j];
                            listView1.Items.Remove(listView);
                            listView1.Refresh();
                        }
                    }

                    if (itemn.SubItems[0].Text == lvi.SubItems[0].Text)
                    {
                        exits = true;
                    }
                    j++;
                }
                if (listView1.Items.Count == 0)
                    listView1.Items.Clear();
                if (exits != true)
                    listView1.Items.Add(lvi);
            }
        }
        #endregion
        //===============================
        #region mainform
        private void MainForm_Load(object sender, EventArgs e)
        {
            formPos = new Vector2(this.Left, this.Top); // 카메라의 첫번째 좌표
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (e.KeyCode == Keys.W)
            {
                moving = 3;

            }
            if (e.KeyCode == Keys.S)
            {
                //축소                
                moving = 4;
            }
            if (e.KeyCode == Keys.L)
            {
                if (lines)
                {
                    Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
                    lines = false;
                }
                else
                {
                    Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
                    lines = true;
                }
            }
            if (e.KeyCode == Keys.A)
            {
                mainForm.Enabled = true;
                statebox.Text = stabox;
                statebox.BackColor = Color.White;
                statebox.ForeColor = Color.Black;
            }
            if (e.KeyCode == Keys.Q)
            {
                mainForm.Enabled = false;
                stabox = statebox.Text;
                statebox.Text = " 잠금 ";
                statebox.ForeColor = Color.DarkRed;
                statebox.BackColor = Color.DarkRed;
                statebox.TextAlign = HorizontalAlignment.Center;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            moving = 0;
        }
        #endregion
        #region pnlViewPor
        private void pnlViewPort_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moving = 1;
                // Cursor.Hide();

            }
            if (e.Button == MouseButtons.Right)
            {
                moving = -1;
                //Cursor.Hide();
            }
        }
        private void pnlViewPort_MouseUp(object sender, MouseEventArgs e)
        {
            moving = 0;
            //Cursor.Show();
        }

        private void pnlViewPort_MouseMove(object sender, MouseEventArgs e)
        {
        }
        #endregion

        private void floor_combo_SelectedValueChanged(object sender, EventArgs e)
        {
            controladora.CrearObjetos(floor_combo.SelectedItem.ToString());
            floor_combo.SelectedItem.ToString();

            class_info = floor_combo.Text;

            //  MessageBox.Show(floor_combo.Text);
            switch (class_info)
            {
                case "ALL":
                    c_name = null;
                    all_select = true;
                    break;
                case "F1":
                    c_name = "103";
                    all_select = false;
                    break;
                case "F2":
                    c_name = "203";
                    all_select = false;
                    break;
                case "F3":
                    c_name = "303";
                    all_select = false;
                    break;
            }
            Turn = Origin.Floor;
        }

        private void floor_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; // combobox에 설정된  item외에 직접 입력이 안되게 설정
        }

        //검색다이얼로그
        private void button1_Click(object sender, EventArgs e)
        {
            s_time.ShowDialog();
            while (true)
            {
                if (s_time.DialogResult == DialogResult.OK)
                {
                    Print_All = false;
                    OneSelect = false;
                    M_ListB = false;
                    break;
                }
                else if (s_time.DialogResult == DialogResult.Cancel)
                    break;
            }
        }

        //실시간 전체출력 
        private void button2_Click(object sender, EventArgs e)
        {
            Print_All = true;
            OneSelect = false;
            statebox.Text = "전체출력";
            statebox.BackColor = Color.White;
            statebox.ForeColor = Color.Black;
            statebox.TextAlign = HorizontalAlignment.Center;
            foreach (ListViewItem item in listViewMember.CheckedItems)
            {
                item.Checked = false;
            }
            foreach (ListViewItem item in listViewInfo.CheckedItems)
            {
                item.Checked = false;
            }
        }

        //실시간전체중크검색 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewMember.CheckedItems.Count <= 0)
                {
                    MessageBox.Show("체크 박스를 표기해 주시기 바랍니다.");
                    return;
                }
                foreach (ListViewItem item in listViewMember.CheckedItems)
                {
                    Print_All = false;
                    OneSelect = true;
                    M_ListB = false;
                    statebox.Text = "전체 체크 출력 중";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //실시간미아목록체크검색기능
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewInfo.CheckedItems.Count <= 0)
                {
                    MessageBox.Show("체크 박스를 표기해 주시기 바랍니다.");
                    return;
                }
                foreach (ListViewItem item in listViewInfo.CheckedItems)
                {
                    Print_All = false;
                    OneSelect = true;
                    M_ListB = true;
                    statebox.Text = "미아 체크 출력 중";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //미아 정보 클릭하여 MISSING 값 0으로 바꾸어주기
        private void listViewInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("미아해제");
            String[] ab = new String[2];
            try
            {
                if (listViewInfo.SelectedIndices.Count <= 0)
                {
                    return;
                }
                int intselectedindex = listViewInfo.SelectedIndices[0];
                if (intselectedindex >= 0)
                {
                    ab[0] = listViewInfo.SelectedItems[intselectedindex].SubItems[0].Text;
                    ab[1] = listViewInfo.SelectedItems[intselectedindex].SubItems[1].Text;
                }
                proxy.Missingmember(ab);
                Print_All = true;
                OneSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int count = 0;
        private void image_timer_Tick(object sender, EventArgs e)
        {
            if (count == 0)
                this.pictureBox1.Image = Properties.Resources.BIT2;
            else if (count == 1)
                this.pictureBox1.Image = Properties.Resources.BIT1;
            else if (count == 2)
                this.pictureBox1.Image = Properties.Resources.BIT3;
            else if (count == 3)
                this.pictureBox1.Image = Properties.Resources.BIT4;
            else if (count == 4)
                this.pictureBox1.Image = Properties.Resources.BIT5;

            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; //이미지의 크기를 picturebox에 맞춘다.
            count++;
            if (count == 5)
                count = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Print_All = true;
            OneSelect = false;
            statebox.Text = "전체출력";
            statebox.BackColor = Color.White;
            statebox.ForeColor = Color.Black;
            statebox.TextAlign = HorizontalAlignment.Center;
            foreach (ListViewItem item in listViewMember.CheckedItems)
            {
                item.Checked = false;
            }
            foreach (ListViewItem item in listViewInfo.CheckedItems)
            {
                item.Checked = false;
            }
            s_time.getselect = null;
            controladora.Camara.position = null;
        }

        //jpg파일 만들기
        public void Bitmapjpg(string name)
        {
            Bitmap pic = new Bitmap(300, 130); // 비트맵 이미지 객체 생성

            if (name.Length == 3) // name 의 길이 으으으 --> 3개
            {

                String[] names = new String[name.Length];
                //Bitmap pic1 = new Bitmap(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + @"Person.jpg", true);
                Graphics g = Graphics.FromImage(pic);//비트맵 위에 draw를 실행하기위해서 그래픽객체 얻음
                g.FillRectangle(Brushes.RoyalBlue, 0, 0, pic.Width, pic.Height);
                String[] lines = new string[name.Length];
                Font drawFont = new Font("Arial", 68, FontStyle.Bold);

                lines = names;


                float x = -10.0f; // 비트맵의 drawtarget 좌표
                float y = 20.0f;

                SolidBrush drawBrush = new SolidBrush(Color.Black);
                PointF drawPoint = new PointF(x, y);
                g.DrawString(name, drawFont, drawBrush, drawPoint);
                //text를 drawString으로 비트맵 위에 draw처리
                y = y + 13.0f;
                //}

                pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name + ".jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                //      pic1.Save(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + (name + "hu.jpg"), ImageFormat.Bmp);
                if (pic != null)
                {
                    pic.RotateFlip(RotateFlipType.Rotate180FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_0.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
       
                    pic.RotateFlip(RotateFlipType.Rotate90FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_1.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장

                    pic.RotateFlip(RotateFlipType.Rotate270FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_2.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
    
                    pic.RotateFlip(RotateFlipType.Rotate90FlipXY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_3.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                    //pic1.Save(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + (name + "hu3.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                }
            }
            else if (name.Length == 4)// name의 길이 아아아아->4개 
            {

                String[] names = new String[name.Length];
                //Bitmap pic1 = new Bitmap(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + @"Person.jpg", true);
                Graphics g = Graphics.FromImage(pic);//비트맵 위에 draw를 실행하기위해서 그래픽객체 얻음
                g.FillRectangle(Brushes.RoyalBlue, 0, 0, pic.Width, pic.Height);
                String[] lines = new string[name.Length];
                Font drawFont = new Font("Arial", 48, FontStyle.Bold);

                lines = names;
                float x = -6.0f; // 비트맵의 drawtarget 좌표
                float y = 30.0f;

                SolidBrush drawBrush = new SolidBrush(Color.Black);

                PointF drawPoint = new PointF(x, y);
                g.DrawString(name, drawFont, drawBrush, drawPoint);
                //text를 drawString으로 비트맵 위에 draw처리
                y = y + 13.0f;
                //}

                pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name + ".jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                //pic1.Save(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + (truename + "hu.jpg"), ImageFormat.Bmp);
                if (pic != null)
                {
                    pic.RotateFlip(RotateFlipType.Rotate180FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_0.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장

                    pic.RotateFlip(RotateFlipType.Rotate90FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_1.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
               
                    pic.RotateFlip(RotateFlipType.Rotate270FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_2.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장

                    pic.RotateFlip(RotateFlipType.Rotate90FlipXY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_3.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                    //           pic1.Save(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + (name + "hu3.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                }
            }

            else if (name.Length == 2) // name의 길이 졸림--> 2개
            {

                String[] names = new String[name.Length];
                //Bitmap pic1 = new Bitmap(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + @"Person.jpg", true);
                Graphics g = Graphics.FromImage(pic);//비트맵 위에 draw를 실행하기위해서 그래픽객체 얻음
                g.FillRectangle(Brushes.RoyalBlue, 0, 0, pic.Width, pic.Height);
                String[] lines = new string[name.Length];
                Font drawFont = new Font("Arial", 68, FontStyle.Bold);


                lines = names;
                float x = 40.5f; // 비트맵의 drawtarget 좌표
                float y = 12.0f;

                SolidBrush drawBrush = new SolidBrush(Color.Black);
                PointF drawPoint = new PointF(x, y);

                g.DrawString(name, drawFont, drawBrush, drawPoint);
                //text를 drawString으로 비트맵 위에 draw처리
                y = y + 13.0f;
                //}
                pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name + ".jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                //pic1.Save(@"C:\Users\ebay\Documents\카카오톡 받은 파일\RestService(기본)\RestService\obj\Debug\TempPE\" + (truename + "hu.jpg"), ImageFormat.Bmp);
                if (pic != null)
                {
                    pic.RotateFlip(RotateFlipType.Rotate180FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_0.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
          
                    pic.RotateFlip(RotateFlipType.Rotate90FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_1.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
       
                    pic.RotateFlip(RotateFlipType.Rotate270FlipY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_2.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
           
                    pic.RotateFlip(RotateFlipType.Rotate90FlipXY);

                    pic.Save(@"C:\Users\62\Desktop\projectclient서버연결변경\projectclient\bin\Debug\textures\" + (name.ToString() + "_3.jpg"), ImageFormat.Bmp);// 지정된 폴더에 저장
                       }
            }
        }

        private void 전체출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print_All = true;
            OneSelect = false;
            statebox.Text = "전체출력";
            statebox.BackColor = Color.White;
            statebox.ForeColor = Color.Black;
            statebox.TextAlign = HorizontalAlignment.Center;
            foreach (ListViewItem item in listViewMember.CheckedItems)
            {
                item.Checked = false;
            }
            foreach (ListViewItem item in listViewInfo.CheckedItems)
            {
                item.Checked = false;
            }
        }

        private void 정보검색ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            s_time.ShowDialog();
            while (true)
            {
                if (s_time.DialogResult == DialogResult.OK)
                {
                    Print_All = false;
                    OneSelect = false;
                    M_ListB = false;
                    break;
                }
                else if (s_time.DialogResult == DialogResult.Cancel)
                    break;
            }
        }

        private void 프로그램종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
