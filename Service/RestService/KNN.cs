using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RestService
{
    public class KNN
    {
        private void Swap(ref double x, ref double y)
        {
            double temp = x;
            x = y;
            y = temp;
        }
        private void Quick_Sort(double[,] arr, int left, int right)
        {
            if (right > left)
            {
                double temp = arr[0, left], j = left;
                for (int i = left + 1; i <= right; i++)
                    if (arr[0, i] < temp)
                    {
                        j++;
                        Swap(ref arr[0, i], ref arr[0, (int)j]);
                        Swap(ref arr[1, i], ref arr[1, (int)j]);
                        Swap(ref arr[2, i], ref arr[2, (int)j]);
                        Swap(ref arr[3, i], ref arr[3, (int)j]);
                    }
                temp = j;
                Swap(ref arr[0, left], ref arr[0, (int)temp]);
                Swap(ref arr[1, left], ref arr[1, (int)temp]);
                Swap(ref arr[2, left], ref arr[2, (int)temp]);
                Swap(ref arr[3, left], ref arr[3, (int)temp]);

                Quick_Sort(arr, left, (int)temp - 1);
                Quick_Sort(arr, (int)temp + 1, right);
            }
        }

        private void kNN_Cal(Decimal[] rssi, double[] sum, Decimal[,] TD, double[,] temp, int Rm, double comp, int x, int y)
        {
            int i, j;

            for (i = 0; i < 91; i++)
            {
                for (j = 2; j < 7; j++)
                {
                    sum[i] = sum[i] + Math.Pow(Convert.ToDouble(rssi[j - 2] - TD[i, j]), 2.0);
                }
            }
            for (i = 0; i < 91; i++)
            {
                temp[0, i] = Math.Sqrt(sum[i]);
                temp[1, i] = Convert.ToDouble(TD[i, 1]);
                temp[2, i] = Convert.ToDouble(TD[i, 7]);
                temp[3, i] = Convert.ToDouble(TD[i, 8]);
            }
            Quick_Sort(temp, 0, temp.Length - 274);
        }

        public int[] KNN_Calc(Decimal[] ARR_AP)
        {
            string con = "server = servercom-pc\\SQLEXPRESS;uid=SC;password=2486;DataBase = awp";
            SqlConnection conn = new SqlConnection(con);
            SqlCommand scom = new SqlCommand();
            scom.Connection = conn;
            scom.CommandText = "SELECT * FROM DATA";
            conn.Open();
            SqlDataReader sd = scom.ExecuteReader();

            int i, j;
            Decimal[] rssi = new Decimal[5];

            for (i = 0; i < 5; i++)
            {
                rssi[i] = ARR_AP[i];
            }
            double[] sum = new double[91];
            Decimal[,] TD = new Decimal[91, 9];
            //=============================// DB Data Save //=============================//  
            i = 0;
            while (sd.Read())
            {
                for (j = 0; j < 9; j++)
                {
                    TD[i, j] = Convert.ToDecimal(sd[j].ToString());
                }
                i++;
            } // end reading
            //===============================// End Save //===============================//
            double[,] temp = new double[4, 91];
            double comp = 0;
            int Rm = 0;
            int x = 0;
            int y = 0;
            kNN_Cal(rssi, sum, TD, temp, Rm, comp, x, y);
            //====================================가장 가까운 RP 1개=========================================//            
            comp = temp[0, 0];
            Rm = Convert.ToInt32(temp[1, 0]);
            x = Convert.ToInt32(temp[2, 0]);
            y = Convert.ToInt32(temp[3, 0]);

            int[] position = new int[2] { Convert.ToInt32(x), Convert.ToInt32(y) };

            sd.Close();
            conn.Close();

            return position;
        }
    }
}