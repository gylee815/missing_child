package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONArray;
import org.json.JSONException;

import android.app.Activity;
import android.content.Context;
import android.os.AsyncTask;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;


public class ReadData2 extends  AsyncTask<String, Integer, ArrayList<String>>
{
	
	String rst;
	MainActivity act;
	Context ctx;

	public ReadData2(Context context, MainActivity mainact){
		super();
		this.act = mainact;
		this.ctx = context;
	}

    @Override
    protected void onPostExecute(ArrayList<String> data) {
        super.onPostExecute(data);
    }

    @Override
    protected ArrayList<String> doInBackground(String... params) {
        try {
            JSONArray personsNames = act.getPersonsData();
            ArrayList<String> person = new ArrayList<String>();

            for(int i=0; i< personsNames.length(); i++)
            {
            	String info = personsNames.getJSONObject(i).getString("ID") + "/" + personsNames.getJSONObject(i).getString("NAME")
            			+ "/" + personsNames.getJSONObject(i).getString("GYRO_X") + "/" + personsNames.getJSONObject(i).getString("GYRO_Y") + "/" + personsNames.getJSONObject(i).getString("GYRO_Z") 
            			+ "/" + personsNames.getJSONObject(i).getString("RELATION") + "/" + personsNames.getJSONObject(i).getString("MCDX") + "/" + personsNames.getJSONObject(i).getString("MCDY") ;
                person.add(info);
            }

            return person;

        } catch (ClientProtocolException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return null;
    }

}
