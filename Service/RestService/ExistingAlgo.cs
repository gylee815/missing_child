using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RestService
{
    public class ExistingAlgo
    {
        private int[] kNN_Cal(Decimal[] rssi, DataTable dt, DataRowCollection rows, int Rm, int x, int y)
        {
            int i = 0;
            int count = 0;
            double nearlysignal = 1000;
            double Sub = 0;
            double Allsub = 0;
            int[] retval = new int[3];

            foreach (DataRow dr in rows)
            {
                for (i = 0; i < 5; i++)
                {
                    Allsub += Math.Abs(Convert.ToDouble(dr[i + 1]) - Convert.ToDouble(rssi[i]));
                    count++;
                }

                Sub = Allsub / count;
                if (nearlysignal > Sub)
                {
                    nearlysignal = Sub;
                    Rm = Convert.ToInt32(dr[0]);
                    x = Convert.ToInt32(dr[6]);
                    y = Convert.ToInt32(dr[7]);
                }
                Allsub = 0;
                count = 0;
            }

            //Console.WriteLine("\nRP번호 : {0} / X : {1} / Y : {2}", Rm, x, y);
            retval[0] = Rm; retval[1] = x; retval[2] = y;
            return retval;
        }
        //private static SqlConnection scon;
        //private static SqlCommand scom;

        public int[] Calc_Result(Decimal[] ARR_AP, DataTable table)
        {
            //int i;

            //string con = "server = servercom-pc\\SQLEXPRESS;uid=SC;password=2486;DataBase = awp";
            //scon = new SqlConnection(con);
            //scon.Open();
            //string comtxt = string.Format("Select RP, AP1, AP2, AP3, AP4, AP5, X, Y From DATA2");
            //scom = new SqlCommand(comtxt, scon);
            //SqlDataAdapter da = new SqlDataAdapter(scom);
            ////DataAdapter가 가져올 데이터를 담을 DataSet
            //DataSet ds = new DataSet("DSet");
            //da.Fill(ds, "DATA2");
            //scon.Close();
            //DataTable dt = ds.Tables["DATA2"];

            //DataRowCollection rows = dt.Rows;

            //Decimal[] rssi = new Decimal[5];

            //for (i = 0; i < 5; i++)
            //{
            //    Console.WriteLine("rssi_{0}를 입력해라", i + 1);
            //    rssi[i] = Convert.ToDecimal(Console.ReadLine());
            //}
            int i;
            int[] position = new int[3];
            int[] position_xy = new int[2];
            DataTable dt = table;

            DataColumnCollection column = dt.Columns;
            DataRowCollection rows = dt.Rows;

            Decimal[] rssi = new Decimal[5];

            for (i = 0; i < 5; i++)
            {
                rssi[i] = ARR_AP[i];
            }

            //double[,] temp = new double[4, dt.Rows.Count];
            int Rm = 0;
            int x = 0;
            int y = 0;

            position = kNN_Cal(rssi, dt, rows, Rm, x, y);
            position_xy[0] = position[1];
            position_xy[1] = position[2];

            return position_xy;
        }
    }
}