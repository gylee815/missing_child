package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.ByteArrayEntity;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.protocol.HTTP;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

import android.net.wifi.ScanResult;
import android.net.wifi.WifiConfiguration;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;

public class MainActivity extends Activity implements OnClickListener {
  private String values ="";
  Button btn1,btn2;
  EditText txt1, txt2;
  HttpClient client;
  ReadData2 rd2;
  final static String URL = "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Login/";
  JSONObject json;
  Context context;
  WifiManager wifiManager;
  private AccessPoint UsingAP = new AccessPoint();
  private WifiConfiguration wfc;
  int okay = 0;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login_activity);
        setTitle("실내 위치기반 미아찾기");
        btn1 = (Button)this.findViewById(R.id.btnRegist);
        btn2 = (Button)this.findViewById(R.id.btnLogin);
        txt1 = (EditText)this.findViewById(R.id.EditText01);
        txt2 = (EditText)this.findViewById(R.id.EditText02);
        btn1.setOnClickListener(this);
        btn2.setOnClickListener(this);
        client = new DefaultHttpClient();
        context = this.getApplicationContext();
        wifiManager = (WifiManager) this.getSystemService(Context.WIFI_SERVICE);
        
    	UsingAP = new AccessPoint();
		UsingAP.setSSID("AP-CENTER");
		UsingAP.setMacAddress("64:e5:99:63:67:40");
		UsingAP.setPassword("wifi2486");
		
/*	    String SSid = null;
	    if (!wifiManager.isWifiEnabled())
	    {
	    	wifiManager.setWifiEnabled(true);
	        try {
			Thread.sleep(10000);
	        } catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			}
	    }
			
		if(wifiManager.isWifiEnabled())
		{
			 List<ScanResult> scanResults = wifiManager.getScanResults();
		    	for(ScanResult scanResult : scanResults)
		    	{

		             if(scanResult.SSID.compareTo(UsingAP.getSSID()) == 0)
		             {
		            	//if(Math.abs(scanResult.level) < 75)
		            	//{
		            		SSid = scanResult.SSID;
		            	//}

		             }
		    	}

		        if(SSid.compareTo(UsingAP.getSSID()) == 0)
		        {
		            if(SSid != null)
		            {
		            	//이부분에서 시간차에 따른 에러가 존재할수있음 (서버로 전송하는데에 연결이 늦어지는 경우가 생김)
		            	wfc = WifiConnect.ConnectWPA(SSid, UsingAP.getPassword());
		            	WifiConnect.connect(wfc, wifiManager, SSid);
		            }
		        }

		}
*/
    }

    public JSONArray getPersonsData() throws ClientProtocolException, IOException, JSONException
    {
    	String ID,PW;
    	ID = txt1.getText().toString();
    	PW = txt2.getText().toString();
    	if(ID.getBytes().length<=0 || PW.getBytes().length<=0){
    			ID ="0";
    			PW ="0";
    	}
        HttpGet get = new HttpGet(URL+ID+"/"+PW);
        HttpResponse response = client.execute(get);
        int status = response.getStatusLine().getStatusCode();
 
        if(status == 200) //sucess
        {
            HttpEntity e = response.getEntity();
            String data = EntityUtils.toString(e);
            JSONArray personsData = new JSONArray(data);
 
            return personsData;
        }
        else
        {
        	printToast("FAIL1 !!!");
 
            return null;
        }
 
    }
    
    public void printToast(String messageToast) {
		Toast.makeText(this, messageToast, Toast.LENGTH_LONG).show();
	}
    
    @Override
    public void onResume()
    {
    	super.onResume();
    	String SSid = null;
    	try {
			Thread.sleep(1000);
		} catch (InterruptedException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
    	wifiManager.disconnect();
 	    if (!wifiManager.isWifiEnabled())
 	    {
 	    	wifiManager.setWifiEnabled(true);
 	        try {
 			Thread.sleep(5000);
 	        } catch (InterruptedException e) {
 			// TODO Auto-generated catch block
 			e.printStackTrace();
 			}
 	    }
 			
 		if(wifiManager.isWifiEnabled())
 		{
 			 List<ScanResult> scanResults = wifiManager.getScanResults();
 		    	for(ScanResult scanResult : scanResults)
 		    	{

 		             if(scanResult.SSID.compareTo(UsingAP.getSSID()) == 0)
 		             {
 		            	SSid = scanResult.SSID;
 		             }
 		    	}

 		        if(SSid.compareTo(UsingAP.getSSID()) == 0)
 		        {
 		            if(SSid != null)
 		            {
 		            	//이부분에서 시간차에 따른 에러가 존재할수있음 (서버로 전송하는데에 연결이 늦어지는 경우가 생김)
 		            	wfc = WifiConnect.ConnectWPA(SSid, UsingAP.getPassword());
 		            	WifiConnect.connect(wfc, wifiManager, SSid);
 		            }
 		        }
 		}
    }

    
    @Override
    public void onClick(View view) {
    	if(view.getId() == R.id.btnLogin)
    	{
	        try
	        {
	        	rd2 = (ReadData2) new ReadData2(this.context,this).execute();///////////////
	        	String data;/////////////
	        	if(rd2.get().size()>0)
	        	{
	        		data = rd2.get().get(0);/////////////////
	        		Intent intent = new Intent(getApplicationContext(), HomeActivity.class);
	        		intent.putExtra("info", data);
	        		startActivity(intent);	        		
	        	}
	        	else
	        		printToast("로그인 실패");
	        } catch(Exception e)
	        {
	        	printToast("로그인 실패");
	        	printToast(e.toString());
	        }
    	}
    	
    	else if(view.getId() == R.id.btnRegist)
    	{
    		try{
	        Intent intent = new Intent(
	                MainActivity.this,  //Context, 부모 액티비티
	                RegistActivity.class); //자식 액티비티의 클래스명
	                //액티비티를 실행하기 
	                startActivity(intent);
    		}
    		catch(Exception ex){
    			printToast(ex.toString());
    			Log.i("regist_info", ex.toString());
    		}
    	}
    }
}
