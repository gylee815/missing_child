package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLEncoder;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.os.AsyncTask;
import android.view.View;

public class ReadRequest extends AsyncTask<String, Void, String>{
	RelationActivity act;
	URL textUrl;
	String stringText;
	
	public ReadRequest(RelationActivity RelaAct){
		super();
		this.act = RelaAct;
	}
	
	@Override
	protected String doInBackground(String... params) {
		if(act.Request_control == 0)
		{
			try
	    	{
				String name = URLEncoder.encode(act.myid,"UTF-8");
		    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Request/"+name);	    	
		    	BufferedReader bufferReader 
				= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
		    	
		    	String  stringBuffer;
				stringText = null;
				while((stringBuffer = bufferReader.readLine())!=null){
					stringText = stringBuffer;
				}	
				bufferReader.close();	
				act.accept_btn.setVisibility(View.VISIBLE);	
	    	}
			
	    	catch (MalformedURLException e) 
	    	{
				android.util.Log.i("Myconnection_RR", e.toString());
				e.printStackTrace();
			}
			catch (IOException e) 
			{
				android.util.Log.i("Myconnection_RR2", e.toString());
				e.printStackTrace();
			}
			
			return stringText;
		}
		else if(act.Request_control == 1)
		{
			try
	    	{
				String id1 = URLEncoder.encode(act.myid , "UTF-8");
				String id2 = URLEncoder.encode(act.str[1] , "UTF-8");
		    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Accept/"+id1+"/"+id2);	    	
		    	BufferedReader bufferReader 
				= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
		    	bufferReader.close();	
				//act.accept_btn.setVisibility(View.INVISIBLE);	
	    	}
			
	    	catch (MalformedURLException e) 
	    	{
				android.util.Log.i("Myconnection_RR3", e.toString());
				e.printStackTrace();
			}
			catch (IOException e) 
			{
				android.util.Log.i("Myconnection_RR4", e.toString());
				e.printStackTrace();
			}
		}
		
		return stringText;
	}
	@Override
	protected void onPostExecute(String result) {
		// TODO Auto-generated method stub			
		super.onPostExecute(result);
	}
}