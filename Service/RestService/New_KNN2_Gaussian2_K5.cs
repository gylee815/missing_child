using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RestService
{
    public class New_KNN2_Gaussian2_K5
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
                    }
                temp = j;
                Swap(ref arr[0, left], ref arr[0, (int)temp]);
                Swap(ref arr[1, left], ref arr[1, (int)temp]);

                Quick_Sort(arr, left, (int)temp - 1);
                Quick_Sort(arr, (int)temp + 1, right);
            }
        }

        private void kNN_Cal(Decimal[] rssi, double[] sum, DataColumnCollection column, DataRowCollection rows, double[,] temp, double[] comp, int[] Rm, DataTable knn)
        {
            int i = 0;
            int j = 0;

            foreach (DataRow dr in rows)
            {
                for (i = 0; i < 5; i++)
                {
                    temp[0, j] += Math.Abs(Convert.ToDouble(rssi[i]) - Convert.ToDouble(dr[i + 1]));
                }
                temp[1, j] = Convert.ToDouble(dr[0]);
                j++;
            }
            Quick_Sort(temp, 0, j - 1);

            for (i = 0; i < 3; i++)
            {
                Rm[i] = Convert.ToInt32(temp[1, i]);
                Console.WriteLine("{0}", Rm[i]);
            }


            DataRow kr = null;

            knn.Columns.Add(new DataColumn("RP", typeof(int)));
            knn.Columns.Add(new DataColumn("AP1", typeof(double)));
            knn.Columns.Add(new DataColumn("AP2", typeof(double)));
            knn.Columns.Add(new DataColumn("AP3", typeof(double)));
            knn.Columns.Add(new DataColumn("AP4", typeof(double)));
            knn.Columns.Add(new DataColumn("AP5", typeof(double)));
            knn.Columns.Add(new DataColumn("X", typeof(int)));
            knn.Columns.Add(new DataColumn("Y", typeof(int)));

            foreach (DataRow dr in rows)
            {
                if (Convert.ToDouble(dr[0]) == Rm[0])
                {
                    kr = knn.NewRow();
                    kr.ItemArray = new object[8] { dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7] };
                    knn.Rows.Add(kr);
                    break;
                }
            }
            foreach (DataRow dr in rows)
            {
                if (Convert.ToDouble(dr[0]) == Rm[1])
                {
                    kr = knn.NewRow();
                    kr.ItemArray = new object[8] { dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7] };
                    knn.Rows.Add(kr);
                    break;
                }
            }
            foreach (DataRow dr in rows)
            {
                if (Convert.ToDouble(dr[0]) == Rm[2])
                {
                    kr = knn.NewRow();
                    kr.ItemArray = new object[8] { dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7] };
                    knn.Rows.Add(kr);
                    break;
                }
            }
            //datarowcollection 으로 Rp번호 3개를 행으로 찾게 하여 데이터를 가져와라
        }

        private void mDiv_Cal(int[] Rm, double[] mDiv, double[] mAvg, DataTable knn)
        {
            int i = 0;

            for (i = 0; i < 5; i++)
            {
                foreach (DataRow kr in knn.Rows)
                {
                    mAvg[i] += Convert.ToDouble(kr[i + 1]);
                }
                mAvg[i] = mAvg[i] / 3; // 0,1,2,3,4   5개  
            }

            for (i = 0; i < 5; i++)
            {
                foreach (DataRow kr in knn.Rows)
                {
                    mDiv[i] += Math.Sqrt(Math.Pow(Convert.ToDouble(kr[i + 1]) - mAvg[i], 2) / 2);
                }
            }
        }

        private void Gauss_Cal(double[] Gauss, DataTable knn, int[] Rm, double[] mDiv, double[] mAvg)
        {
            int i = 0;
            int j = 0;

            for (i = 0; i < 5; i++)
            {
                foreach (DataRow kr in knn.Rows)
                {
                    Gauss[j] = (Math.Exp(Math.Pow(Convert.ToDouble(kr[i + 1]) - mAvg[i], 2) /
                        (-2 * Math.Pow(mDiv[i], 2)))) / (Math.Sqrt(2 * Math.PI) * mDiv[i]);
                    j++;
                }
            }
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (double.IsNaN(Gauss[j]))
                        Gauss[j] = 1;
                }
            }
        }

        private void G_Avg_Cal(double[] G_Avg, double[,] f, double[] G_Sum, double[] G_add, double[] Gauss, int[] Rm, DataTable knn)
        {
            int i = 0;
            int j = 0;
            int u = 0;
            for (j = 0; j < 5; j++)
            {
                i = 0;
                foreach (DataRow kr in knn.Rows)
                {
                    f[j, i] = Convert.ToDouble(kr[j + 1]) * Gauss[u];
                    i++;
                    u++;
                }
            }
            int y = 0;
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    G_Sum[i] += f[i, j];
                    G_add[i] += Gauss[y];
                    y++;
                }
            }
            for (i = 0; i < 5; i++)
            {
                G_Avg[i] = G_Sum[i] / G_add[i];
                Console.WriteLine("{0}", G_Avg[i]);
            }
        }

        private void Result(double[] new1, double[] G_Avg, int[] Rm, DataTable knn, double[,] tempd)
        {
            int i = 0;
            int j = 0;
            foreach (DataRow kr in knn.Rows)
            {
                for (j = 0; j < 5; j++)
                {
                    new1[i] += Math.Sqrt(Math.Pow(G_Avg[j] - Convert.ToDouble(kr[j + 1]), 2));
                }
                i++;
            }
            i = 0;
            foreach (DataRow kr in knn.Rows)
            {
                tempd[0, i] = new1[i];//값을 저장
                tempd[1, i] = Rm[i];//RP번호 저장
                tempd[2, i] = Convert.ToDouble(kr[6]);
                tempd[3, i] = Convert.ToDouble(kr[7]);

                i++;
            }
        }

        private void bubble(int tempCount, double hold, int loop, double[,] tempd)
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

        public int[] Calc_Result(Decimal[] ARR_AP, DataTable table)
        {
            int i;
            DataTable dt = table;

            DataColumnCollection column = dt.Columns;
            DataRowCollection rows = dt.Rows;

            Decimal[] rssi = new Decimal[5];

            for (i = 0; i < 5; i++)
            {
                rssi[i] = ARR_AP[i];
            }

            double[] sum = new double[dt.Rows.Count];
            double[,] temp = new double[2, dt.Rows.Count];
            int[] Rm = new int[3];
            double[] comp = new double[3];
            DataTable knn = new DataTable("kNN_No.3");
            kNN_Cal(rssi, sum, column, rows, temp, comp, Rm, knn);

            double[] mDiv = new double[5];
            double[] mAvg = new double[5];
            mDiv_Cal(Rm, mDiv, mAvg, knn);

            double[] Gauss = new double[15];
            Gauss_Cal(Gauss, knn, Rm, mDiv, mAvg);
            //==========================================가중평균=============================================//
            double[] G_Avg = new double[5];     //  최죵적인 결과 가중평균을 통해 예측 수신세기 도출                 
            double[,] f = new double[5, 3];     //  데이터 * 가중치 
            double[] G_Sum = new double[5];     //  (데이터 * 가중치) 들의 합
            double[] G_add = new double[5];     //  가중치 합
            G_Avg_Cal(G_Avg, f, G_Sum, G_add, Gauss, Rm, knn);
            //==================================== 예측값으로 kNN 구하기 ====================================//
            double[] new1 = new double[3];
            double[,] tempd = new double[4, 3];
            double compv, compn, compx, compy;
            Result(new1, G_Avg, Rm, knn, tempd);
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
            return position;
        }
    }
}