package com.example.jsonsampleapp;

import android.view.View;
import android.widget.RelativeLayout;

public class DrawCharacter {
	
	int old_N = 0;
	int old_E = 0;
	int old_S = 0;
	int old_W = 0;
	public void drawing(HomeActivity act, double position[]){		
		act.device.setVisibility(View.VISIBLE);
		act.DirectImage.setVisibility(View.VISIBLE);
        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(32, 32);
        params.leftMargin = Math.abs((int)(position[0]));
        params.topMargin = Math.abs((int)(position[1]));
        act.device.setLayoutParams(params);
        
        params = new RelativeLayout.LayoutParams(220, 220);
        params.leftMargin = Math.abs((int)(position[0])) - 85;
        params.topMargin = Math.abs((int)(position[1])) - 85;
        act.DirectImage.setLayoutParams(params);
        
        act.device.bringToFront();
        act.device.invalidate();
        
        act.device2.setVisibility(View.VISIBLE);
        RelativeLayout.LayoutParams params2 = new RelativeLayout.LayoutParams(32, 32);
        params2.leftMargin = Math.abs((int)(position[2]));
        params2.topMargin = Math.abs((int)(position[3]));
        act.device2.setLayoutParams(params2);

        act.device2.bringToFront();
        act.device2.invalidate();
	}
	
	public void drawing2(HomeActivity act, double positionX, double positionY){		
/*		act.device.setVisibility(View.VISIBLE);
		act.DirectImage.setVisibility(View.VISIBLE);
        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(32, 32);
        params.leftMargin = Math.abs((int)(position[0]));
        params.topMargin = Math.abs((int)(position[1]));
        act.device.setLayoutParams(params);
        
        params = new RelativeLayout.LayoutParams(220, 220);
        params.leftMargin = Math.abs((int)(position[0])) - 85;
        params.topMargin = Math.abs((int)(position[1])) - 85;
        act.DirectImage.setLayoutParams(params);
        
        act.device.bringToFront();
        act.device.invalidate();*/
        
        act.device2.setVisibility(View.VISIBLE);
        RelativeLayout.LayoutParams params2 = new RelativeLayout.LayoutParams(32, 32);
        params2.leftMargin = Math.abs((int)(positionX));
        params2.topMargin = Math.abs((int)(positionY));
        act.device2.setLayoutParams(params2);

        act.device2.bringToFront();
        act.device2.invalidate();
	}
	
	public void drawing(HomeActivity act, double position[], int moving, String direction)
	{
		act.device.setVisibility(View.VISIBLE);
		act.DirectImage.setVisibility(View.VISIBLE);
		
		if(direction.equals("N")){
			if(moving == 0)
				old_N = 0;
			
	        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(32, 32);
	        params.leftMargin = Math.abs((int)(position[0]) - moving);
	        params.topMargin = Math.abs((int)(position[1]));
	        act.device.setLayoutParams(params);
	        
	        params = new RelativeLayout.LayoutParams(220, 220);
	        params.leftMargin = Math.abs((int)(position[0]) - moving) - 85;
	        params.topMargin = Math.abs((int)(position[1])) - 85;
	        act.DirectImage.setLayoutParams(params);
	        old_N = moving;
		}
		else if(direction.equals("E")){
			if(moving == 0)
				old_E = 0;
			
	        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(32, 32);
	        params.leftMargin = Math.abs((int)(position[0]));
	        params.topMargin = Math.abs((int)(position[1]) - moving);
	        act.device.setLayoutParams(params);
	        
	        params = new RelativeLayout.LayoutParams(220, 220);
	        params.leftMargin = Math.abs((int)(position[0])) - 85;
	        params.topMargin = Math.abs((int)(position[1]) - moving) - 85;
	        act.DirectImage.setLayoutParams(params);
	        old_E = moving;
			}
		else if(direction.equals("S")){
			if(moving == 0)
				old_S = 0;
			
	        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(32, 32);
	        params.leftMargin = Math.abs((int)(position[0]) + moving);
	        params.topMargin = Math.abs((int)(position[1]));
	        act.device.setLayoutParams(params);
	        
	        params = new RelativeLayout.LayoutParams(220, 220);
	        params.leftMargin = Math.abs((int)(position[0]) + moving) - 85;
	        params.topMargin = Math.abs((int)(position[1])) - 85;
	        act.DirectImage.setLayoutParams(params);
	        old_S = moving;
			}
		else if(direction.equals("W")){
			if(moving == 0)
				old_W = 0;
			
	        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(32, 32);
	        params.leftMargin = Math.abs((int)(position[0]));
	        params.topMargin = Math.abs((int)(position[1]) + moving);
	        act.device.setLayoutParams(params);
	        
	        params = new RelativeLayout.LayoutParams(220, 220);
	        params.leftMargin = Math.abs((int)(position[0])) - 85;
	        params.topMargin = Math.abs((int)(position[1]) + moving) - 85;
	        act.DirectImage.setLayoutParams(params);
	        old_W = moving;
			}      
		
	     act.device.bringToFront();
	     act.device.invalidate();
	}
}
