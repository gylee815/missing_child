using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace projectclient
{
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
        }

        //등록
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string id = textBox2.Text;
            string password = textBox3.Text;
            string phone = textBox4.Text;

            MainForm.mainForm.proxy.admininsert(name, id, password, phone);
            MessageBox.Show("등록이 완료되었습니다.");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        //ID중복확인
        private void button4_Click(object sender, EventArgs e)
        {
            string id = textBox2.Text;

            if (MainForm.mainForm.proxy.adminidcheck(id))
            {
                MessageBox.Show("사용 가능한 ID입니다.");
            }
            else
            {
                MessageBox.Show("이미 등록되어있는 ID입니다.");
            }
        }
        //확인
        private void button5_Click(object sender, EventArgs e)
        {
            string id = textBox5.Text;
            string name = string.Empty;
            string password = string.Empty;
            string phone = string.Empty;

            String[] position = MainForm.mainForm.proxy.adminselect(id);
            for (int i = 0; i < position.Length; i++)
            {
                if (position != null)
                {
                    string[] token = position[i].Split('#');
                    name = token[0];
                    password = token[1];
                    phone = token[2];
                }
                else
                {
                    MessageBox.Show("없는 ID입니다.");
                }
            }

            textBox6.Text = name;
            textBox7.Text = password;
            textBox8.Text = phone;
            MessageBox.Show("ID확인 완료.");
        }
        //수정
        private void button2_Click(object sender, EventArgs e)
        {
            string id = textBox5.Text;
            string name = textBox6.Text;
            string password = textBox7.Text;
            string phone = textBox8.Text;

            MainForm.mainForm.proxy.adminupdate(id, name, password, phone);
            MessageBox.Show("수정되었습니다.");
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }
        //삭제
        private void button3_Click(object sender, EventArgs e)
        {
            string id = textBox5.Text;
            MainForm.mainForm.proxy.admindelete(id);
            MessageBox.Show("삭제되었습니다.");
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }
    }
}
