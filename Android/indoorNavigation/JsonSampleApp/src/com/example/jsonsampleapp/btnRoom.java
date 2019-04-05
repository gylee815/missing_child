package com.example.jsonsampleapp;

import android.content.Context;
import android.graphics.Color;
import android.view.View;
import android.widget.Button;
import android.widget.RelativeLayout;

public class btnRoom {

	private Button[] btn;
	private double Per_X;
	private double Per_Y;
	private int ALPA  = 20;
	
	public btnRoom(Button[] b, int DWidth, int DHeight)
	{
		btn = b;
		Per_X = (double)24500 / DWidth;
		Per_Y = (double)32000 / DHeight;
	}
	
	public int[] CalculRoom(int btnWidth, int btnHeight)
	{
		int[] rectRange = new int[2];
		rectRange[0] = (int)(btnWidth / Per_X);
		rectRange[1] = (int)(btnHeight / Per_Y);
		return rectRange; 
	}

	public void SetF3()
	{
    	int[] H308 = CalculRoom(8050, 11400);
    	int[] H307 = CalculRoom(8050, 11350);
    	int[] H303 = CalculRoom(8065, 11545);
    	int[] H304 = CalculRoom(8065, 3330);
    	int[] H305 = CalculRoom(8065, 3500);
    	int[] H306 = CalculRoom(8065, 3900);
    	
    	//308ȣ
    	RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(H308[0],H308[1]);
    	params.topMargin = (int)(7690 / Per_Y);
    	params.leftMargin = 10;	
    	btn[0].setLayoutParams(params);
    	btn[0].setText("R302");
    	btn[0].setTextColor(Color.WHITE);
    	btn[0].setTextSize(18);
    	btn[0].setBackgroundColor(Color.argb(ALPA, 255, 255, 255));
    	
    	//307ȣ
    	params = new RelativeLayout.LayoutParams(H307[0],H307[1]);
    	params.topMargin = (int)(19300 / Per_Y);//19090
    	params.leftMargin = 15;	   	//10
    	btn[1].setLayoutParams(params);
    	btn[1].setText("R301");
    	btn[1].setTextColor(Color.WHITE);
    	btn[1].setTextSize(18);
    	btn[1].setBackgroundColor(Color.argb(ALPA, 255, 255, 255));    	

    	//303ȣ
    	params = new RelativeLayout.LayoutParams(H303[0],H303[1]);
    	params.topMargin = 3;
    	params.leftMargin = (int)(16435 / Per_X);	   	
    	btn[2].setLayoutParams(params);
    	
    	btn[2].setText("R303");
    	btn[2].setTextColor(Color.WHITE);
    	btn[2].setTextSize(18);
    	btn[2].setBackgroundColor(Color.argb(ALPA, 255, 255, 255)); 
   
    	//304ȣ
    	params = new RelativeLayout.LayoutParams(H304[0],H304[1]);
    	params.topMargin = (int)(11730 / Per_Y);
    	params.leftMargin = (int)(16435 / Per_X);	   	
    	btn[3].setLayoutParams(params);
    	btn[3].setText("R304");
    	btn[3].setTextColor(Color.WHITE);
    	btn[3].setTextSize(18);
    	btn[3].setBackgroundColor(Color.argb(ALPA, 255, 255, 255));     	
    	
    	//305ȣ
    	params = new RelativeLayout.LayoutParams(H305[0],H305[1]);
    	params.topMargin = (int)(15230 / Per_Y);
    	params.leftMargin = (int)(16435 / Per_X);	   	
    	btn[4].setLayoutParams(params);
    	btn[4].setText("R305");
    	btn[4].setTextColor(Color.WHITE);
    	btn[4].setTextSize(18);
    	btn[4].setBackgroundColor(Color.argb(ALPA, 255, 255, 255));
    	
    	//306ȣ
    	params = new RelativeLayout.LayoutParams(H306[0],H306[1]);
    	params.topMargin = (int)(18830 / Per_Y);
    	params.leftMargin = (int)(16435 / Per_X);	   	
    	btn[5].setLayoutParams(params);
    	btn[5].setText("R306");
    	btn[5].setTextColor(Color.WHITE);
    	btn[5].setTextSize(18);
    	btn[5].setBackgroundColor(Color.argb(ALPA, 255, 255, 255));        	
	}

	public void Viewbtn()
	{
		for(int i = 0 ; i < btn.length ; i++)
		{
			btn[i].setVisibility(View.VISIBLE);
		}		
	}

	public void Hidebtn()
	{
		for(int i = 0 ; i < btn.length ; i++)
		{
			btn[i].setVisibility(View.INVISIBLE);
		}
	}
}
