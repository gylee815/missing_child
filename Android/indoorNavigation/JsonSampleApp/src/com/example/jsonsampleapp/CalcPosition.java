package com.example.jsonsampleapp;
import android.util.DisplayMetrics;

public class CalcPosition {

	public static double[] CalculDistance(int[] position, int Width, int Height)
	{
		double Per_X = (double)24500 / Width;
		double Per_Y = (double)32000 / Height;
		
		//Point2D D_Distance = new Point2D(R_Distance.getX()/Per_X, R_Distance.getY()/Per_Y);
		//D_Distance.setFloor(R_Distance.getFloor());
		double[] Calc_position = new double[4];
		Calc_position[0] = position[0] / Per_X;
		Calc_position[1] = position[1] / Per_Y;
		
		Calc_position[2] = position[2] / Per_X;
		Calc_position[3] = position[3] / Per_Y;
		
		return Calc_position;
	}
}
