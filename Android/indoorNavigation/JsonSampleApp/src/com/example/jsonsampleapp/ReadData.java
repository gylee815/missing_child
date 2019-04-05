package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONArray;
import org.json.JSONException;

import android.app.Activity;
import android.content.Context;
import android.os.AsyncTask;
import android.text.Editable;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;

public class ReadData extends AsyncTask<String, Void, Void>{
	String rst;
	HomeActivity act;
	String ID;
	String PW;
/*	public ReadData( HomeActivity loginActivity){
		super();
		this.act = loginActivity;

	}*/

	@Override
	protected Void doInBackground(String... params) {
		URL textUrl;
		try {
			String myid = URLEncoder.encode(params[0],"UTF-8");
			textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Logout/"+myid);
			BufferedReader bufferReader 
			= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
			String  stringBuffer;
			String stringText = "";
			while((stringBuffer = bufferReader.readLine())!=null){
				stringText += stringBuffer;
			}
			
			bufferReader.close();
			rst = stringText;
			
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		catch (IOException e) {
			// TODO Auto-generated catch block
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
