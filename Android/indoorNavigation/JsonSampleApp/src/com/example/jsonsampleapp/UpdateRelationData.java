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

public class UpdateRelationData extends AsyncTask<String, Void, String>{
	RelationActivity act;
	URL textUrl;
	String rst;
	String stringText;
	String stringName2;
	String[] str;
	public UpdateRelationData(RelationActivity RelaAct){
		super();
		this.act = RelaAct;
	}
	
	@Override
	protected String doInBackground(String... params) {
		
		try
    	{
			//String name2 = URLEncoder.encode(act.getEdit_name,"UTF-8");
			String id1 = URLEncoder.encode(act.myid,"UTF-8");
			String id2 = URLEncoder.encode(act.getEdit_id,"UTF-8");
	    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/RelaSelect/"+id1+"/"+id2+"/"+act.radio_text);	    	
	    	/*BufferedReader bufferReader 
			= new BufferedReader(new InputStreamReader(textUrl.openStream()));*/
	    	BufferedReader bufferReader 
			= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
	    	
	    	String  stringBuffer;
			stringText = null;
			while((stringBuffer = bufferReader.readLine())!=null){
				stringText = stringBuffer;
			}	
			bufferReader.close();

			if(stringText!=null)
			{
		    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/RelaName2/"+id2);	    	
		    	BufferedReader bufferReader3 
				= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
		    	
		    	String  stringBuffer_NAME2;
		    	stringName2 = null;
				while((stringBuffer_NAME2 = bufferReader3.readLine())!=null){
					stringName2 = stringBuffer_NAME2;
				}
		    	bufferReader3.close();
		    	str = new String(stringName2).split("\"");
				////////////////////////////////////////////////////
				String name1 = URLEncoder.encode(act.myname,"UTF-8");
				String name2 = URLEncoder.encode(str[1],"UTF-8");
		    	textUrl = new URL( "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/RelaUpdate/"+id1+"/"+id2+"/"+name2+"/"+act.radio_text+"/"+name1);	    	
		    	BufferedReader bufferReader2 
				= new BufferedReader(new InputStreamReader(textUrl.openStream(),"UTF-8"));
		    	bufferReader2.close();
		    	//return stringText;
			}
			//else //if(stringText==null)
				//return stringText;
    	}
		
    	catch (MalformedURLException e) 
    	{
			android.util.Log.i("Myconnection_URD", e.toString());
			e.printStackTrace();
		}
		catch (IOException e) 
		{
			android.util.Log.i("Myconnection_URD2", e.toString());
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