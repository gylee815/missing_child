package com.example.jsonsampleapp;

public class Room {

	public String RoomCheck(int x, int y)
	{
		if((x == 5400 && y == 21020) || (x == 5400 && y == 25910) || (x == 2700 && y == 21020) || (x == 2700 && y == 25910))
		{
			return "R301";
		}
	    else if(x>=540 && x<=7560) //테스트 해볼것
	    {
		  if(y>=19580 && y<=29510)
		     return "R301";
		  else
		     return "noRoom";
	    }
		else
			return "noRoom";
	}
}
