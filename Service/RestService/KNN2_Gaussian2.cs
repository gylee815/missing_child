using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RestService
{
    public class KNN2_Gaussian2
    {
        private void Swap(ref double x, ref double y)
        {
            double temp = x;
            x = y;
            y = temp;
        }

        public void Quick_Sort(double[,] arr, int left, int right)
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

        public void kNN_Cal(Decimal[] rssi, Decimal[,] TD, double[,] temp, int[] Rm, double[] comp, int[] x, int[] y)
        {
            int i, j;

            for (i = 0; i < 91; i++)
            {
                for (j = 2; j < 7; j++)
                {
                    temp[0, i] = temp[0, i] + Math.Sqrt(Math.Pow(Convert.ToDouble(rssi[j - 2] - TD[i, j]), 2.0));
                }
                temp[1, i] = Convert.ToDouble(TD[i, 1]);
                temp[2, i] = Convert.ToDouble(TD[i, 7]);
                temp[3, i] = Convert.ToDouble(TD[i, 8]);
            }
            Quick_Sort(temp, 0, temp.Length - 274);

            for (i = 0; i < 3; i++)
            {
                comp[i] = temp[0, i];
                Rm[i] = Convert.ToInt32(temp[1, i]);
                x[i] = Convert.ToInt32(temp[2, i]);
                y[i] = Convert.ToInt32(temp[3, i]);

                Console.WriteLine("\n거리 : {0} / RP번호 : {1} / X : {2} / Y : {3}", comp[i], Rm[i], x[i], y[i]);
            }
        }

        public void mDiv_Cal(int[] Rm, double[,] mDiv, double[] mAvg, Decimal[,] TD)
        {
            int i, j;

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    mAvg[i] = mAvg[i] + Convert.ToDouble(TD[Convert.ToInt32(Rm[i] - 1), j + 2]);
                }
                mAvg[i] = mAvg[i] / j;
            }

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    mDiv[i, j] = Math.Sqrt(Math.Pow(Convert.ToDouble(TD[Convert.ToInt32(Rm[i] - 1), j + 2]) - mAvg[i], 2));
                }
            }
        }

        public void Gauss_Cal(Decimal[] rssi, double[,] Gauss, Decimal[,] TD, int[] Rm, double[,] mDiv)
        {
            int i, j;

            for (i = 0; i < 3; i++)
                for (j = 0; j < 5; j++)
                    Gauss[i, j] = (Math.Exp(Math.Pow(Convert.ToDouble(rssi[j]) - Convert.ToDouble(TD[Rm[i] - 1, j + 2]), 2) /
                        (-2 * Math.Pow(mDiv[i, j], 2)))) / (Math.Sqrt(2 * Math.PI) * mDiv[i, j]);

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (double.IsNaN(Gauss[i, j]))
                        Gauss[i, j] = 1;
                }
            }
        }

        public void G_Avg_Cal(double[] G_Avg, double[,] f, double[] G_Sum, double[] G_add, double[,] Gauss, int[] Rm, Decimal[,] TD)
        {
            int i, j;
            for (i = 0; i < 3; i++)
                for (j = 0; j < 5; j++)
                    f[i, j] = Convert.ToDouble(TD[Rm[i] - 1, j + 2]) * Gauss[i, j];

            for (i = 0; i < 5; i++)
                for (j = 0; j < 3; j++)
                    G_Sum[i] = G_Sum[i] + f[j, i];

            for (i = 0; i < 5; i++)
                for (j = 0; j < 3; j++)
                    G_add[i] = G_add[i] + Gauss[j, i];

            for (i = 0; i < 5; i++)
                G_Avg[i] = G_Sum[i] / G_add[i];
        }

        public void Result(double[] new1, double[] G_Avg, int[] Rm, Decimal[,] TD, double[,] tempd)
        {
            int i, j;
            for (i = 0; i < 3; i++)
            {
                for (j = 2; j < 7; j++)
                {
                    new1[i] = new1[i] + Math.Sqrt(Math.Pow((G_Avg[j - 2] - Convert.ToDouble(TD[Convert.ToInt32(Rm[i] - 1), j])), 2));
                }
            }
            for (i = 0; i < 3; i++)
            {
                tempd[0, i] = new1[i];//값을 저장
                tempd[1, i] = Rm[i];//RP번호 저장
                tempd[2, i] = Convert.ToDouble(TD[Convert.ToInt32(Rm[i] - 1), 7]);
                tempd[3, i] = Convert.ToDouble(TD[Convert.ToInt32(Rm[i] - 1), 8]);
            }
        }

        public void bubble(int tempCount, double hold, int loop, double[,] tempd)
        {
            int i;

            for (loop = 0; loop < tempCount - 1; loop++)
            {
                for (i = 0; i < tempCount - 1 - loop; i++)
                {
                    if (tempd[0, i] > tempd[0, i + 1])
                    {   //거리값 정렬
                        hold = tempd[0, i];
                        tempd[0, i] = tempd[0, i + 1];
                        tempd[0, i + 1] = hold;
                        //거리값에 따른 RP번호 정렬
                        hold = tempd[1, i];
                        tempd[1, i] = tempd[1, i + 1];
                        tempd[1, i + 1] = hold;
                        //거리값에 따른 X 정렬
                        hold = tempd[2, i];
                        tempd[2, i] = tempd[2, i + 1];
                        tempd[2, i + 1] = hold;
                        //거리값에 따른 Y 정렬
                        hold = tempd[3, i];
                        tempd[3, i] = tempd[3, i + 1];
                        tempd[3, i + 1] = hold;
                    }
                }
            }
        }

        public int[] Calc_Result(Decimal[] ARR_RP)
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
                rssi[i] = ARR_RP[i];
            }

            Decimal[,] TD = new Decimal[91, 9];
            //===============================// DB Data Save //===============================//  
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
            //======================//  최근접 kNN값을 3개 얻어옴  //===============================// 
            double[,] temp = new double[4, 91];
            int[] Rm = new int[3];
            double[] comp = new double[3];
            int[] x = new int[3];
            int[] y = new int[3];
            kNN_Cal(rssi, TD, temp, Rm, comp, x, y);
            //==========================================표준편차=============================================//
            double[,] mDiv = new double[3, 5];
            double[] mAvg = new double[3];
            mDiv_Cal(Rm, mDiv, mAvg, TD);
            //========================================== Gauss ==============================================//
            double[,] Gauss = new double[3, 5];
            Gauss_Cal(rssi, Gauss, TD, Rm, mDiv);
            //==========================================가중평균=============================================//
            double[] G_Avg = new double[5];     //  최죵적인 결과 가중평균을 통해 예측 수신세기 도출                 
            double[,] f = new double[3, 5];     //  데이터 * 가중치 
            double[] G_Sum = new double[5];     //  (데이터 * 가중치) 들의 합
            double[] G_add = new double[5];     //  가중치 합
            G_Avg_Cal(G_Avg, f, G_Sum, G_add, Gauss, Rm, TD);
            //==================================== 예측값으로 kNN 구하기 ====================================//
            double[] new1 = new double[3];
            double[,] tempd = new double[4, 3];
            double compv, compn, compx, compy;
            Result(new1, G_Avg, Rm, TD, tempd);
            //==========================================정렬=================================================//
            int tempCount = 3;
            double hold = 0;
            int loop = 0;
            bubble(tempCount, hold, loop, tempd);
            //====================================가장 가까운 RP 1개=========================================//            
            compv = tempd[0, 0];
            compn = tempd[1, 0];
            compx = tempd[2, 0];
            compy = tempd[3, 0];

            int[] position = new int[] { Convert.ToInt32(compx), Convert.ToInt32(compy) };

            sd.Close();
            conn.Close();

            return position;
        }
    }
}