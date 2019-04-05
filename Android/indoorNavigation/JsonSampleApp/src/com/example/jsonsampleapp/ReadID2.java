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

public class ReadID2 extends AsyncTask<String, Void, String>{
	RelationActivity act;
	URL textUrl;
	String stringText;
	
	public ReadID2(RelationActivity RelaAct){
		super();
		this.act = RelaAct;
	}
	
	@Override
	protected String doInBackground(String... params) {
		try
    	{
			String ID = URLEncoder.encode(act.myid,"UTF-8");
	    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Read_ID2/"+ID);	    	
	    	BufferedReader bufferReader 
			= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));    	
	    	String  stringBuffer;
			stringText = null;
			while((stringBuffer = bufferReader.readLine())!=null){
				stringText = stringBuffer;
			}	
			bufferReader.close();	
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
	@Override
	protected void onPostExecute(String result) {
		// TODO Auto-generated method stub			
		super.onPostExecute(result);
	}
}