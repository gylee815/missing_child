namespace projectclient
{
    partial class S_Time
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.cancle = new System.Windows.Forms.Button();
            this.Search = new System.Windows.Forms.Button();
            this.end_date = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.start_date = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.t_minute = new System.Windows.Forms.ComboBox();
            this.t_hour = new System.Windows.Forms.ComboBox();
            this.p_minute = new System.Windows.Forms.ComboBox();
            this.p_hour = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NAME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(9, 9);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // cancle
            // 
            this.cancle.BackColor = System.Drawing.Color.White;
            this.cancle.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cancle.Location = new System.Drawing.Point(439, 299);
            this.cancle.Name = "cancle";
            this.cancle.Size = new System.Drawing.Size(116, 38);
            this.cancle.TabIndex = 10;
            this.cancle.Text = "Cancle";
            this.cancle.UseVisualStyleBackColor = false;
            this.cancle.Click += new System.EventHandler(this.cancle_Click);
            // 
            // Search
            // 
            this.Search.BackColor = System.Drawing.Color.White;
            this.Search.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Search.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Search.Location = new System.Drawing.Point(439, 243);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(116, 38);
            this.Search.TabIndex = 11;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = false;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // end_date
            // 
            this.end_date.Font = new System.Drawing.Font("양재소슬체S", 9F);
            this.end_date.ForeColor = System.Drawing.Color.Teal;
            this.end_date.Location = new System.Drawing.Point(244, 113);
            this.end_date.Name = "end_date";
            this.end_date.Size = new System.Drawing.Size(144, 19);
            this.end_date.TabIndex = 26;
            this.end_date.MouseDown += new System.Windows.Forms.MouseEventHandler(this.end_date_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(241, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 25;
            this.label3.Text = "마지막날짜";
            // 
            // start_date
            // 
            this.start_date.Font = new System.Drawing.Font("양재소슬체S", 9F);
            this.start_date.ForeColor = System.Drawing.Color.Teal;
            this.start_date.Location = new System.Drawing.Point(244, 41);
            this.start_date.Name = "start_date";
            this.start_date.Size = new System.Drawing.Size(144, 19);
            this.start_date.TabIndex = 24;
            this.start_date.MouseDown += new System.Windows.Forms.MouseEventHandler(this.start_date_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(241, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 23);
            this.label2.TabIndex = 23;
            this.label2.Text = "시작날짜";
            // 
            // t_minute
            // 
            this.t_minute.Font = new System.Drawing.Font("양재소슬체S", 9F);
            this.t_minute.ForeColor = System.Drawing.Color.Teal;
            this.t_minute.FormattingEnabled = true;
            this.t_minute.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.t_minute.Location = new System.Drawing.Point(475, 113);
            this.t_minute.Name = "t_minute";
            this.t_minute.Size = new System.Drawing.Size(52, 20);
            this.t_minute.TabIndex = 22;
            // 
            // t_hour
            // 
            this.t_hour.Font = new System.Drawing.Font("양재소슬체S", 9F);
            this.t_hour.ForeColor = System.Drawing.Color.Teal;
            this.t_hour.FormattingEnabled = true;
            this.t_hour.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.t_hour.Location = new System.Drawing.Point(408, 113);
            this.t_hour.Name = "t_hour";
            this.t_hour.Size = new System.Drawing.Size(52, 20);
            this.t_hour.TabIndex = 21;
            // 
            // p_minute
            // 
            this.p_minute.Font = new System.Drawing.Font("양재소슬체S", 9F);
            this.p_minute.ForeColor = System.Drawing.Color.Teal;
            this.p_minute.FormattingEnabled = true;
            this.p_minute.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.p_minute.Location = new System.Drawing.Point(475, 40);
            this.p_minute.Name = "p_minute";
            this.p_minute.Size = new System.Drawing.Size(52, 20);
            this.p_minute.TabIndex = 19;
            // 
            // p_hour
            // 
            this.p_hour.Font = new System.Drawing.Font("양재소슬체S", 9F);
            this.p_hour.ForeColor = System.Drawing.Color.Teal;
            this.p_hour.FormattingEnabled = true;
            this.p_hour.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.p_hour.Location = new System.Drawing.Point(408, 40);
            this.p_hour.Name = "p_hour";
            this.p_hour.Size = new System.Drawing.Size(52, 20);
            this.p_hour.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(318, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 29);
            this.button1.TabIndex = 27;
            this.button1.Text = "↓";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.NAME});
            this.listView1.Location = new System.Drawing.Point(229, 183);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(186, 154);
            this.listView1.TabIndex = 31;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 90;
            // 
            // NAME
            // 
            this.NAME.Text = "NAME";
            this.NAME.Width = 88;
            // 
            // S_Time
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(577, 349);
            this.Controls.Add(this.cancle);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.end_date);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.start_date);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.t_minute);
            this.Controls.Add(this.t_hour);
            this.Controls.Add(this.p_minute);
            this.Controls.Add(this.p_hour);
            this.Controls.Add(this.monthCalendar1);
            this.Name = "S_Time";
            this.Text = "S_Time";
            this.Load += new System.EventHandler(this.S_Time_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Button cancle;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.TextBox end_date;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox start_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox t_minute;
        private System.Windows.Forms.ComboBox t_hour;
        private System.Windows.Forms.ComboBox p_minute;
        private System.Windows.Forms.ComboBox p_hour;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader NAME;
        public System.Windows.Forms.ListView listView1;
    }
}