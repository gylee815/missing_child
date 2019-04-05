using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace projectclient
{
    public partial class S_Time : Form
    {
        int m_down = 1;
        public String[] getselect; //
        public String[] r_select;
        public String[] g_select;
        public List<Info> f_select;
        public string return_start_date;
        public string return_end_date;
        public string[] return_date;
        public string[] start;  //시작날짜
        public string[] end; //마지막날짜

        public ListBox box;

        public S_Time()
        {
            InitializeComponent();
            f_select = new List<Info>();
            Search.TabStop = false;
            Search.FlatStyle = FlatStyle.Flat;
            Search.FlatAppearance.BorderSize = 0;
            cancle.TabStop = false;
            cancle.FlatStyle = FlatStyle.Flat;
            cancle.FlatAppearance.BorderSize = 0;
            
        }
        private void S_Time_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            listView1.Items.Clear();
            //날짜선택
            if (m_down == 1)
                start_date.Text = monthCalendar1.SelectionStart.ToShortDateString();

            else
                end_date.Text = monthCalendar1.SelectionEnd.ToShortDateString();         
        }
        private void start_date_MouseDown(object sender, MouseEventArgs e)
        {
            m_down = 1;
        }
        private void end_date_MouseDown(object sender, MouseEventArgs e)
        {
            m_down = -1;
        }
  
        private void cancle_Click(object sender, EventArgs e)
        {
            start_date.Text = " ";
            p_hour.Text = " ";
            p_minute.Text = " ";
            end_date.Text = " ";
            t_hour.Text = " ";
            t_minute.Text = " ";
            DialogResult = DialogResult.Cancel;
        }
        #region 값 받아오고 넘겨주고
        private void button1_Click(object sender, EventArgs e)
        {
            string sTime = p_hour.Text + ":" + p_minute.Text;
            string eTime = t_hour.Text + ":" + t_minute.Text ;


            DateTime s_date = DateTime.ParseExact(start_date.Text + " " + sTime, "yyyy-MM-dd H:mm",null);
            DateTime e_date = DateTime.ParseExact(end_date.Text + " " + eTime, "yyyy-MM-dd H:mm", null);
            if (return_start_date == "--" || return_end_date == "--")
            {
                DialogResult = DialogResult.Retry;
            }
            else
            {
                getselect = MainForm.mainForm.proxy.LoadDayTo(s_date,e_date);
               
                for (int i = 0; i < getselect.Length; i++)
                {
                    string[] token = getselect[i].Split('#'); 
                    
                    bool isContain = false;

                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.SubItems[1].Text == token[1])
                    {
                        isContain = true;
                    }         
                }
                    if (!isContain)
                    {
                        ListViewItem item1 = new ListViewItem(new string[] { token[0], token[1] });
                        listView1.Items.Add(item1);
                    }
                    f_select.Add(new Info(token[0], token[1], token[2], token[3], token[5], token[4]));
                }
            }
        }
        private void Search_Click(object sender, EventArgs e)
        {
            start_date.Text = " ";
            p_hour.Text = " ";
            p_minute.Text = " ";
            end_date.Text = " ";
            t_hour.Text = " ";
            t_minute.Text = " ";
            MainForm.mainForm.controladora.Camara.num = 0;  
        }
        public String[] turn()
        {
            return getselect;
        } 
       
        #endregion
 
    }
}
