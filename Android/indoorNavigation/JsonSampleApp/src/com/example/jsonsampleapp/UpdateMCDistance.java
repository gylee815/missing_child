package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONException;

import android.content.Context;
import android.os.AsyncTask;

public class UpdateMCDistance extends AsyncTask<String, Void, Void>{
	HomeActivity act;
	URL textUrl;
	
	public UpdateMCDistance(HomeActivity HomeAct){
		super();
		this.act = HomeAct;
	}
	
	@Override
	protected Void doInBackground(String... params) {
		
		try
    	{
	    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/MCD/"+act.id+"/"+act.LimitX+"/"+act.LimitY);
	    	
	    	BufferedReader bufferReader 
			= new BufferedReader(new InputStreamReader(textUrl.openStream()));
    	}
		
    	catch (MalformedURLException e) 
    	{
			android.util.Log.i("Myconnection_MCD1", e.toString());
			e.printStackTrace();
		}
		catch (IOException e) 
		{
			android.util.Log.i("Myconnection_MCD2", e.toString());
			e.printStackTrace();
		}
		
		return null;
	}
	@Override
	protected void onPostExecute(Void result) {
		// TODO Auto-generated method stub			
		super.onPostExecute(result);
	}
}