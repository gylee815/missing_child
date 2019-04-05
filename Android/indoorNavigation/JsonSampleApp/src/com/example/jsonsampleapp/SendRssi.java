package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.R.integer;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ActivityManager;
import android.app.ActivityManager.RunningTaskInfo;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Color;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiConfiguration;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
import android.os.Build;
import android.text.Editable;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;

public class SendRssi extends AsyncTask<String, Void, String>{
	String rst;
	//String relation;
	HomeActivity act;
	String ID2_roomCheck;
	String PW;
	int count;
	int[] direction;
	Room room;
	DrawCharacter draw;
	BigDecimal AvgAP1 = null;
	BigDecimal AvgAP2 = null;
	BigDecimal AvgAP3 = null;
	BigDecimal AvgAP4 = null;
	BigDecimal AvgAP5 = null;
	int old_step;
	int old_stepover;
	String pre_X = "0";
	String pre_Y = "0";
	String Floor = null;
	int[] position = new int[4];
	int[] Moveposition = new int[4];
	int old_AP1, old_AP2, old_AP3, old_AP4, old_AP5 = 0;
	double[] calcPosition;
	double[] MoveCalcPosition;
	boolean charact_moving;
	public int missing_control = 0;
	//ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
	//List<RunningTaskInfo> tasks =act.am.getRunningTasks(10); 
	
	public SendRssi( HomeActivity loginActivity){
		super();
		this.act = loginActivity;
		room = new Room();
		draw = new DrawCharacter();
		old_step = 0;
		old_stepover = 0;
	}

	@SuppressLint("NewApi") @Override
	protected String doInBackground(String... params) {
			URL textUrl;

			android.util.Log.i("threadable", "able");
			while(!act.IsRunning){
				long startTime = 0;
				long endTime = 0;
				String Move_x = "0", Move_y = "0";
				charact_moving = true;
				count = 0;
				int AP1_save = 0;
				int AP2_save = 0;
				int AP3_save = 0;
				int AP4_save = 0;
				int AP5_save = 0;
				startTime = System.currentTimeMillis();
			try {
				 if (!act.wifiManager.isWifiEnabled())
                 {
                     act.wifiManager.setWifiEnabled(true);
                 }
				 if(act.wifiManager.isWifiEnabled()){
					
					act.wifiManager.startScan();
                      
					String SSid = null;
					WifiConfiguration wfc;
                    List<ScanResult> scanResults = act.wifiManager.getScanResults();
                    String CurrentSSid= act.wifiManager.getConnectionInfo().getSSID().trim();
                    if(scanResults != null){
	                    for(ScanResult scanResult : scanResults)
				    	{
				             if(scanResult.SSID.compareTo(act.UsingAP.getSSID())== 0)
				             {
				            	SSid = scanResult.SSID;
				             }
				    	}
	                    
	                    if(SSid.compareTo(act.UsingAP.getSSID())==0)
	                    {
	                    	if(CurrentSSid.equalsIgnoreCase("\"AP-CENTER\""))
	                    	{
	                    		android.util.Log.i("Myconnection", SSid);	
	                    	}
	                        else
					        {
					        	android.util.Log.i("Myconnection", "reconnect");
					            if(SSid != null)
					            {
					            	//이부분에서 시간차에 따른 에러가 존재할수있음 (서버로 전송하는데에 연결이 늦어지는 경우가 생김)
					            	wfc = WifiConnect.ConnectWPA(SSid, act.UsingAP.getPassword());
					            	WifiConnect.connect(wfc, act.wifiManager, SSid);
					            	try {
										Thread.sleep(1000);//////////////////////////////////0826 2000->1000
									} catch (InterruptedException e) {
										// TODO Auto-generated catch block
										e.printStackTrace();
									}
					            }
					        }
	                    }

				        while(count < 130){
				        	if(scanResults != null){
				        		int AP1 = 0;
				        		int AP2 = 0;
				        		int AP3 = 0;
				        		int AP4 = 0;
				        		int AP5 = 0;
				        		for(ScanResult scanResult : scanResults){
				        			switch(scanResult.BSSID){
				        			case "64:e5:99:a8:64:60":
				        				 AP1 = scanResult.level;
				        				 AP1_save += AP1;
				        				 old_AP1 = AP1;
				        				 break;
				        			case "90:9f:33:7e:d6:de":
				        				 AP2 = scanResult.level;
				        				 AP2_save += AP2;
				        				 old_AP2 = AP2;
				        				 break;
				        			case "64:e5:99:63:67:40":
				        				 AP3 = scanResult.level;
				        				 AP3_save += AP3;
				        				 old_AP3 = AP3;
				        				 break;		
				        			case "02:30:0d:5c:6d:a0":
				        				 AP4 = scanResult.level;
				        				 AP4_save += AP4;
				        				 old_AP4 = AP4;
				        				 break;		
				        			case "90:9f:33:7e:f4:f0":
				        				 AP5 = scanResult.level;
				        				 AP5_save += AP5;
				        				 old_AP5 = AP5;
				        				 break;		
				        			}			        				
				        		}
/*			        			if(AP1 == 0)
			        			{
			        				AP1_save += -100; 
			        			}
			        			if(AP2 == 0)
			        			{
			        				AP2_save += -100;
			        			}
			        			if(AP3 == 0)
			        			{
			        				AP3_save += -100;
			        			}
			        			if(AP4 == 0)
			        			{
			        				AP4_save += -100;
			        			}
			        			if(AP5 == 0)
			        			{
			        				AP5_save += -100;
			        			}*/
				        		if(AP1 == 0 && old_AP1 == 0)
			        			{
			        				AP1_save += -100;
			        			}
			        			else if(AP1 == 0 && old_AP1 != 0)
			        			{
			        				AP1_save += old_AP1;
			        			}
			        			
			        			if(AP2 == 0 && old_AP2 == 0)
			        			{
			        				AP2_save += -100;
			        			}
			        			else if(AP2 == 0 && old_AP2 != 0)
			        			{
			        				AP2_save += old_AP2;
			        			}
			        			
			        			if(AP3 == 0 && old_AP3 == 0)
			        			{
			        				AP3_save += -100;
			        			}
			        			else if(AP3 == 0 && old_AP3 != 0)
			        			{
			        				AP3_save += old_AP3;
			        			}
			        			
			        			if(AP4 == 0 && old_AP4 == 0)
			        			{
			        				AP4_save += -100;
			        			}
			        			else if(AP4 == 0 && old_AP4 != 0)
			        			{
			        				AP4_save += old_AP4;
			        			}
			        			
			        			if(AP5 == 0 && old_AP5 == 0)
			        			{
			        				AP5_save += -100;
			        			}
			        			else if(AP5 == 0 && old_AP5 != 0)
			        			{
			        				AP5_save += old_AP5;
			        			}
			                	act.wifiManager.startScan();
	                            scanResults = act.wifiManager.getScanResults();
				        	}
				        	count++;
				        }
				        
				        BigDecimal maxCount = new BigDecimal(count);				        
						BigDecimal totalAp1 = new BigDecimal(AP1_save);
						BigDecimal totalAp2 = new BigDecimal(AP2_save);
						BigDecimal totalAp3 = new BigDecimal(AP3_save);
						BigDecimal totalAp4 = new BigDecimal(AP4_save);
						BigDecimal totalAp5 = new BigDecimal(AP5_save);
						
						//나눌때 무한대로 소수점이 발생할수있으므로 반올림 하는 과정이필요!!
						AvgAP1 = totalAp1.divide(maxCount, 3, RoundingMode.HALF_UP);
						AvgAP2 = totalAp2.divide(maxCount, 3, RoundingMode.HALF_UP);
						AvgAP3 = totalAp3.divide(maxCount, 3, RoundingMode.HALF_UP);
						AvgAP4 = totalAp4.divide(maxCount, 3, RoundingMode.HALF_UP);
						AvgAP5 = totalAp5.divide(maxCount, 3, RoundingMode.HALF_UP);
				        Floor = "3";
	                    if(act.step == 0 || act.step - old_step >= 1){
	                    	if(act.step == 0)
	                    	act.step += 1;
		/*				textUrl = new URL( "http://61.81.99.83:8080/RestService/RestServiceImpl.svc/Position/"+act.id+"/"
											+String.valueOf(AvgAP1)+"/"+String.valueOf(AvgAP2)+"/"+String.valueOf(AvgAP3)+"/"+String.valueOf(AvgAP4)+"/"+String.valueOf(AvgAP5)+"/"+Floor);
						BufferedReader bufferReader 
						= new BufferedReader(new InputStreamReader(textUrl.openStream()));
						String  stringBuffer;
						String stringText = "";
						while((stringBuffer = bufferReader.readLine())!=null){
							stringText += stringBuffer;
						}
						
						bufferReader.close();
						rst = stringText;*/
				        //--- 여기서 점프 뛰었는지 걸었는지 판별 해야될듯....//
						/*+String.valueOf(act.step)+"/"+String.valueOf(act.stepover)+"/"+String.valueOf(old_step)+"/"+String.valueOf(old_stepover)+"/"*/
	                    String actID = URLEncoder.encode(act.id,"UTF-8");
	                    textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Position/"+actID+"/"+act.Position_X+"/"+act.Position_Y+"/"+act.EWSN+"/"
								+String.valueOf(AvgAP1)+"/"+String.valueOf(AvgAP2)+"/"+String.valueOf(AvgAP3)+"/"+String.valueOf(AvgAP4)+"/"+String.valueOf(AvgAP5)+"/"+Floor);
						BufferedReader bufferReader 
						= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
						StringBuilder total = new StringBuilder();
						String line;
						while ((line = bufferReader.readLine()) != null) {
						    total.append(line);
						}
						
						bufferReader.close();
						rst = total.toString();
						
						//Region - HTTPGET을 이용한 통신
						/*String URL =  "http://61.81.99.83:8080/RestService/RestServiceImpl.svc/Position/"+act.id+"/"
								+String.valueOf(AvgAP1)+"/"+String.valueOf(AvgAP2)+"/"+String.valueOf(AvgAP3)+"/"+String.valueOf(AvgAP4)+"/"+String.valueOf(AvgAP5)+"/"+Floor;
				   					*/
						//EndRegion
				   					
						try {
							JSONObject jObject = new JSONObject(rst);
							act.Position_X = jObject.getString("Position_X");
							act.Position_Y = jObject.getString("Position_Y");
							act.Position_X2 = jObject.getString("Position_X2");
							act.Position_Y2 = jObject.getString("Position_Y2");
							Move_x = jObject.getString("PositionMove_X");
							Move_y = jObject.getString("PositionMove_Y");
							
							if(Integer.parseInt(act.Position_X)==position[0] && Integer.parseInt(act.Position_Y) == position[1]){
								charact_moving = false;
							}
							position[0] = Integer.parseInt(act.Position_X);
							position[1] = Integer.parseInt(act.Position_Y);
							position[2] = Integer.parseInt(act.Position_X2);
							position[3] = Integer.parseInt(act.Position_Y2);
							calcPosition = CalcPosition.CalculDistance(position, act.screenWidth, act.screenHeight);
							android.util.Log.i("move", position[2] + " / " + position[3]);	
							if(Move_x != "null" && Move_y != "null"){
								Moveposition[0] = Integer.parseInt(Move_x);
								Moveposition[1] = Integer.parseInt(Move_y);
								Moveposition[2] = Integer.parseInt(act.Position_X2);
								Moveposition[3] = Integer.parseInt(act.Position_Y2);
								MoveCalcPosition = CalcPosition.CalculDistance(Moveposition, act.screenWidth, act.screenHeight);
							}
							//Region - HTTPGET을 이용한통신
	/*						JSONObject positionData = act.getPositionData(URL);
							act.Position_X = positionData.getString("Position_X");
							act.Position_Y = positionData.getString("Position_Y");
							position[0] = Integer.parseInt(act.Position_X);
							position[1] = Integer.parseInt(act.Position_Y);
							calcPosition = CalcPosition.CalculDistance(position, act.screenWidth, act.screenHeight);*/
							//EndRegion
							old_step = act.step;
							
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
	                    }
	                    else
	                    {
	                    	String actID = URLEncoder.encode(act.id,"UTF-8");
							textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/NoMovePosition/"+actID+"/"+act.Position_X+"/"+act.Position_Y+"/"
									+String.valueOf(AvgAP1)+"/"+String.valueOf(AvgAP2)+"/"+String.valueOf(AvgAP3)+"/"+String.valueOf(AvgAP4)+"/"+String.valueOf(AvgAP5)+"/"+Floor);
							BufferedReader bufferReader 
							= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));	
							StringBuilder total = new StringBuilder();
							String line;
							while ((line = bufferReader.readLine()) != null) {
							    total.append(line);
							}
							
							bufferReader.close();
							rst = total.toString();						
							
							JSONObject jObject;
							try {
								jObject = new JSONObject(rst);
								act.Position_X = jObject.getString("Position_X");
								act.Position_Y = jObject.getString("Position_Y");
								act.Position_X2 = jObject.getString("Position_X2");
								act.Position_Y2 = jObject.getString("Position_Y2");
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
							position[0] = Integer.parseInt(act.Position_X);
							position[1] = Integer.parseInt(act.Position_Y);
							position[2] = Integer.parseInt(act.Position_X2);
							position[3] = Integer.parseInt(act.Position_Y2);
							calcPosition = CalcPosition.CalculDistance(position, act.screenWidth, act.screenHeight);
							charact_moving = true;
						/*	try {
								Thread.sleep(300);
				                endTime = System.currentTimeMillis();
				                android.util.Log.i("totaltime", String.valueOf((endTime - startTime) / 1000.0) + " : nomove");
							} catch (InterruptedException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
	                    	continue;*/
	                    }
	                          	
	                	/*String ID2_roomCheck = room.RoomCheck(Integer.parseInt(act.Position_X2), Integer.parseInt(act.Position_Y2));
	                	if(!ID2_roomCheck.equals("없음"))
	                	{
	                		
	                	}
	                	else if(!ID2_roomCheck.equals(""))
	                	{
	                		
	                	}*/
	                	/*if(act.selectRoom.equals("NONE"))
	                	{
	                		MiaCall();
	                	}
	                	else if(act.selectRoom.equals("301호"))
	                	{
	                		String ID2_roomCheck = room.RoomCheck(Integer.parseInt(act.Position_X2), Integer.parseInt(act.Position_Y2));
	                		if(!ID2_roomCheck.equals("R301"))
	                		{
	                			MiaCall2();
	                		}
	                	}
	                	
	                	if(act.missingCheck == 1)
	                	{
	                		try {
	        				URL textUrl2 = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/MissingOff/"+act.id);
	        		    	BufferedReader bufferReader 
	        				= new BufferedReader(new InputStreamReader(textUrl2.openStream()));
	        		    	//act.missing_btn.setVisibility(View.VISIBLE);	
	        				
		        			} catch (MalformedURLException e) {
		        				e.printStackTrace();
		        			} catch (IOException e) {
		        				e.printStackTrace();
		        			}
	                		act.missingCheck = 0;
	                	}*/
						act.runOnUiThread(new Runnable() {					
							@Override
							public void run() {
								try{
									if(charact_moving == true){
										direction = new int[]{0,0,0,0};
	                        			act.bkgImage.setBackgroundResource(R.drawable.floor_3);
	                        			act.room_btn.Viewbtn();
	                        			act.room_btn.SetF3();
										String roomNum = room.RoomCheck(Integer.parseInt(act.Position_X), Integer.parseInt(act.Position_Y));
										if(roomNum.equals("noRoom"))
										{
											android.util.Log.i("uithread", Math.abs(Integer.parseInt(act.Position_X)) + "&&" +  Math.abs(Integer.parseInt(act.Position_Y)));
											draw.drawing(act, calcPosition);
										}
										else if(roomNum.equals("R301"))
										{
											for(int i = 0 ; i < act.btn.length ;  i++)
	                                		{
	                                			if(act.btn[i].getText().toString().compareTo(roomNum) == 0)
	                                			{
	                                				act.btn[i].setBackgroundColor(Color.argb(100, 255, 0, 0));
	                                			}
	                                		}
	                                        act.device.setVisibility(View.INVISIBLE);
	                                        act.DirectImage.setVisibility(View.INVISIBLE);
	                                        draw.drawing2(act, calcPosition[2], calcPosition[3]);
										}
										android.util.Log.i("move", act.Position_X + "  :  " + act.Position_Y);
									}
									else
									{
				             			act.bkgImage.setBackgroundResource(R.drawable.floor_3);
	                        			act.room_btn.Viewbtn();
	                        			act.room_btn.SetF3();
										String roomNum = room.RoomCheck(Integer.parseInt(act.Position_X), Integer.parseInt(act.Position_Y));
										if(roomNum.equals("noRoom"))
										{
											android.util.Log.i("uithread", Math.abs(Integer.parseInt(act.Position_X)) + "&&" +  Math.abs(Integer.parseInt(act.Position_Y)));
		/*									if(act.EWSN.equals("N"))
											{	
												direction[0] += 0;
												draw.drawing(act, MoveCalcPosition, direction[0], act.EWSN);
											}
											else if(act.EWSN.equals("E"))
											{
												direction[1] += 0;
												draw.drawing(act, MoveCalcPosition, direction[1], act.EWSN);
											}
											else if(act.EWSN.equals("S"))
											{
												direction[2] += 0;
												draw.drawing(act, MoveCalcPosition, direction[2], act.EWSN);
											}
											else if(act.EWSN.equals("W"))
											{
												direction[3] += 0;
												draw.drawing(act, MoveCalcPosition, direction[3], act.EWSN);
											}*/
											draw.drawing(act, MoveCalcPosition);
											android.util.Log.i("move", act.EWSN + " : " + direction[0] +  "/" + direction[1] +  "/" + direction[2] +  "/" + direction[3] );
										}
										else if(roomNum.equals("R301"))
										{
											for(int i = 0 ; i < act.btn.length ;  i++)
	                                		{
	                                			if(act.btn[i].getText().toString().compareTo(roomNum) == 0)
	                                			{
	                                				act.btn[i].setBackgroundColor(Color.argb(100, 255, 0, 0));
	                                			}
	                                		}
	                                        act.device.setVisibility(View.INVISIBLE);
	                                        act.DirectImage.setVisibility(View.INVISIBLE);
	                                        draw.drawing2(act, MoveCalcPosition[2], MoveCalcPosition[3]);
	                                        android.util.Log.i("move", MoveCalcPosition[0] + "  :  " + MoveCalcPosition[1]);
										}
									}
								}
								catch(Exception e){
									android.util.Log.i("uithread", e.toString());	
								}
					
							}
						});
						if(act.selectRoom.equals("NONE"))
	                	{							
	                		MiaCall();
	                	}
	                	else if(act.selectRoom.equals("301호"))
	                	{
	                		String ID2_roomCheck = room.RoomCheck(Integer.parseInt(act.Position_X2), Integer.parseInt(act.Position_Y2));
	                		if(!ID2_roomCheck.equals("R301"))
	                		{
	                			MiaCall2();
	                		}
	                	}
	                	
	                	if(act.missingCheck == 1)
	                	{
	                		try {
	        				URL textUrl2 = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/MissingOff/"+act.id);
	        		    	BufferedReader bufferReader 
	        				= new BufferedReader(new InputStreamReader(textUrl2.openStream()));
	        		    	//act.missing_btn.setVisibility(View.VISIBLE);	
	        				
		        			} catch (MalformedURLException e) {
		        				e.printStackTrace();
		        			} catch (IOException e) {
		        				e.printStackTrace();
		        			}
	                		act.missingCheck = 0;
	                	}
					 }
				 }
				
			} catch (MalformedURLException e) {
				// TODO Auto-generated catch block
				android.util.Log.i("Myconnection", e.toString());
				e.printStackTrace();
			}
			catch(FileNotFoundException e){
				android.util.Log.i("Myconnection", e.toString());
				e.printStackTrace();
			}
			catch (IOException e) {
				// TODO Auto-generated catch block
				android.util.Log.i("Myconnection", e.toString());
				e.printStackTrace();
			}
		
			System.out.println(position[3]+"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
			////////////////////////////////////////////////////////////////////////////////
            try {
                Thread.sleep(300);
                endTime = System.currentTimeMillis();
                android.util.Log.i("totaltime", String.valueOf((endTime - startTime) / 1000.0));
            } catch (InterruptedException e) {
                    e.printStackTrace();
            }
		}
			
		return null;
	}
	
	public void MiaCall()
	{
		if(Math.abs(position[0]-position[2])>=Integer.parseInt(act.LimitX) && Math.abs(position[1]-position[3])>=Integer.parseInt(act.LimitY) && position[2]!=99999 && position[3]!=99999 && missing_control == 0 && act.missingCheck==0)
		{
			////
			//act.moveToFront();
			//act.callActivity("com.example.jsonsampleapp");
			
			try {
				String actID = URLEncoder.encode(act.id,"UTF-8");
				URL textUrl2 = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Missing/"+actID);
		    	BufferedReader bufferReader 
				= new BufferedReader(new InputStreamReader(textUrl2.openStream(),"UTF-8"));
				
			} catch (MalformedURLException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}
			
			missing_control = 1;
			act.missing_music();
		}
	}
	
	public void MiaCall2()
	{
		if(position[2]!=99999 && position[3]!=99999 && missing_control == 0 && act.missingCheck==0)
		{
			try {
				String actID = URLEncoder.encode(act.id,"UTF-8");
				URL textUrl2 = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Missing/"+actID);
		    	BufferedReader bufferReader 
				= new BufferedReader(new InputStreamReader(textUrl2.openStream(),"UTF-8"));
				
			} catch (MalformedURLException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}
			
			missing_control = 1;
			act.missing_music();
		}
	}
	
	@Override
	protected void onPostExecute(String result) {
		// TODO Auto-generated method stub			
		super.onPostExecute(result);
	}
}
