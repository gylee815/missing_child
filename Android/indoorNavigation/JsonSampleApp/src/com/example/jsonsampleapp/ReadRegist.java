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
import android.os.Handler;
import android.os.Looper;
import android.view.View.OnClickListener;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;


public class ReadRegist extends  AsyncTask<String, Integer, String>
{

	String rst;
	RegistActivity act;
	Context ctx1;
	String ID;
	String PASSWORD;
	String AGE;
	String NAME;
	String LOGINFO;
	String MISSING;
	String PHONE;
	String RELATION;
	String ID2;
	String NAME2;
	String SEX;
	public ReadRegist(Context context, RegistActivity act){
		super();
		this.act = act;
		this.ctx1 = context;
	}

	@Override
	protected void onPostExecute(String data) {
		super.onPostExecute(data);

		Handler mHandler = new Handler(Looper.getMainLooper());
		mHandler.postDelayed(new Runnable() {
			@Override
			public void run() {
				// 내용

			}
		}, 0);
	}
	@Override
	protected String doInBackground(String... params) {
		try {
			String isOkay = act.RegistData();
			if(isOkay.equals("\"success\""))
			return isOkay;// 받아온 값이 "success"면 받아온다
			else
				return "\"fail\"";//success 아니면 fail을 리턴한다.

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
