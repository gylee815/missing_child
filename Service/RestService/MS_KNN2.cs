﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RestService
{
    public class MS_KNN2
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

        private void kNN_Cal(Decimal[] rssi, DataTable dt, DataRowCollection rows, double[,] temp, int Rm, ref int x, ref int y)
        {
            int i = 0;
            int j = 0;

            foreach (DataRow dr in rows)
            {
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    temp[0, j] += //Math.Abs(Convert.ToDouble(rssi[i]) - Convert.ToDouble(dr[i + 1]));
                        Math.Abs(Math.Pow(Convert.ToDouble(rssi[i]), 2) - Math.Pow(Convert.ToDouble(dr[i + 1]), 2));
                    if (i == 4)
                    {
                        break;
                    }
                }
                temp[1, j] = Convert.ToDouble(dr[0]);
                temp[2, j] = Convert.ToDouble(dr[6]);
                temp[3, j] = Convert.ToDouble(dr[7]);
                j++;
            }
            Quick_Sort(temp, 0, j - 1);

            Rm = Convert.ToInt32(temp[1, 0]);
            x = Convert.ToInt32(temp[2, 0]);
            y = Convert.ToInt32(temp[3, 0]);
        }

        public int[] Calc_Result(Decimal[] ARR_AP, DataTable table)
        {
            int i;
            DataTable dt = table;
            DataRowCollection rows = dt.Rows;
            Decimal[] rssi = new Decimal[5];

            for (i = 0; i < 5; i++)
            {
                rssi[i] = ARR_AP[i];
            }

            double[,] temp = new double[4, dt.Rows.Count];
            int Rm = 0;
            int x = 0;
            int y = 0;

            kNN_Cal(rssi, dt, rows, temp, Rm, ref x, ref y);

            int[] position = new int[] { x, y };
            return position;
        }
    }
}