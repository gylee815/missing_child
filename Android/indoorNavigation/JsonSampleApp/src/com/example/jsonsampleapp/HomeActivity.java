package com.example.jsonsampleapp;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.sql.Date;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.List;
import java.util.concurrent.ExecutionException;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;

import android.annotation.SuppressLint;
import android.annotation.TargetApi;
import android.app.Activity;
import android.app.ActivityManager;
import android.app.ActivityManager.RecentTaskInfo;
import android.app.ActivityManager.RunningAppProcessInfo;
import android.app.ActivityManager.RunningTaskInfo;
import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.app.Notification;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.Matrix;
import android.graphics.drawable.BitmapDrawable;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.media.AudioManager;
import android.media.MediaPlayer;
import android.net.Uri;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiConfiguration;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Looper;
import android.os.Vibrator;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

public class HomeActivity extends Activity implements SensorEventListener, OnClickListener{
	private int refreshRate = 400;
	private static final String TAG = "WIFIScanner";
	WifiManager.WifiLock wifiLock;
	String info, id, name, relaCP;
	Intent intent;
	ReadData rd;
	Button btnRefresh;
	SendRssi sendrssi;
	WifiManager wifiManager;
	private List<ScanResult> mScanResult;
	private int count = 0;
	int AP1, AP2;
	Context context;
	AccessPoint UsingAP;
	boolean IsRunning;
	String Position_X, Position_Y, Position_X2, Position_Y2;
	HomeActivity home;
	

    private String Direction; //방향
    private int Degrees; //각도
    
    //컨트롤러 객체 ------------------------------------------
    public RelativeLayout RL;
    public TextView Text;
    public TextView SpeedText;
    public TextView nullText;
    public ImageView bkgImage;
    public ImageView device;
    public ImageView device2;
    public ImageView DirectImage;
    public btnRoom room_btn;   
    public Button[] btn = new Button[6];
    //public Button missing_btn;
    public Button find_btn;
    //스크린 크기----------------------------------------------
    public int screenWidth;
    public int screenHeight;
    
    //센서 -------------------------------------------------
  	private long lastTime;
  	private long StepTime;
  	private long LastStepTime;
  	public float speed;
  	private float lastX;
  	private float lastY;
  	private float lastZ;
  	private int W_flag;
  	private float x, y, z;
  	
  	private static final int DATA_X = SensorManager.DATA_X;
  	private static final int DATA_Y = SensorManager.DATA_Y;
  	private static final int DATA_Z = SensorManager.DATA_Z;
  	int step = 0;
  	int stepover = 0 ;
  	private int mag_deg_count = 0;
    int mag_degree;
    
  	//자이로,가속도,자기장 센서----------------------------------------------
  	private SensorManager mSensorManager;
    private Sensor mGyroscope; 		//자이로
    private Sensor accSensor; 		//가속도
    private Sensor mField; 			//자기장 객체 저장
    
	float[] mGravity= new float[3];;
    float[] mMagnetic= new float[3];;
    float[] Real_gyroValues= new float[3];
    float[] Prev_gyroValues= new float[3];
    float[] gyroMatrix = new float[9];
    float[] gyroOrientation = new float[3];
    
    String Azimuth;//자기장 방위
    String EWSN;//자이로 방위
  
    private long timestamp;
    private static final float NS2S = 1.0f / 1000000000.0f;
    public static final float EPSILON = 0.000000001f;
    
    DecimalFormat d = new DecimalFormat("#.##");
  	
    //자이로 조정부분
	static final private int GYRO = Menu.FIRST; // 메뉴 id로 상수 생성
	static final private int MCD = Menu.FIRST+1;
	static final private int ID2 = Menu.FIRST+2;
	private static final int GyroAct = 0;
	private static final int MCDistanceAct = 1;
	private static final int RelationAct = 2;
	private int[] GA_array_value;
	private float[] default_gyval;
	String GYROtable_value_X, GYROtable_value_Y, GYROtable_value_Z;
	public float drift_revision_X;
    public float drift_revision_Y;
    public float drift_revision_Z;
    UpdateGyroData UGD;
    UpdateMCDistance UMC;
    MediaPlayer mp;
    int missingCheck = 0;
    private String[] Limit_info;
    //private int[] default_LimitXY;
    String LimitX, LimitY;
    //int mcdx, mcdy;
    //String [] mcd;
    String selectRoom = "NONE";
    String limit_xx, limit_yy;
    String ID2_NAME;
    //진동
    Vibrator vibrator;
    long[] vibPattern = {0, 200, 200, 200, 200, 200, 500, 500, 200, 500, 200, 500, 500, 200, 200, 200, 200, 200, 1000};
    //백그라운드 to 포그라운드
    //ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
    //List<RunningTaskInfo> tasks =am.getRunningTasks(3); 
    
	//Region - JSONArray
    /*public JSONArray getPosition(String Ap1) throws ClientProtocolException, IOException, JSONException
    {
    	long now = System.currentTimeMillis();
    	Date dt = new Date(now);
    	SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd a hh:mm:ss"); 
    	String strNow = sdf.format(dt);
        HttpGet get = new HttpGet(URL+id+"/"+Ap1);
        HttpResponse response = client.execute(get);
        int status = response.getStatusLine().getStatusCode();
 
        if(status == 200) //sucess
        {
            HttpEntity e = response.getEntity();
            String data = EntityUtils.toString(e);
            JSONArray positionData = new JSONArray(data); 
            return positionData;
        }
        else
        {
        	printToast("FAIL!!!"); 
            return null;
        } 
    }*/
	//EndRegion
    
	//Region - BroadCastReciver를 사용한 wifiScan
	/*public void initWIFIScan() {
		// init WIFISCAN
		try{
			if(!wifiManager.isWifiEnabled())
			{
				while(wifiManager.isWifiEnabled() == false)
				{
					wifiManager.setWifiEnabled(true);
				}
				Thread.sleep(10000);
			}
			
			//if(wifiManager.isWifiEnabled()){
				scanCount = 0;
				final IntentFilter filter = new IntentFilter(
						WifiManager.SCAN_RESULTS_AVAILABLE_ACTION);
				filter.addAction(WifiManager.NETWORK_STATE_CHANGED_ACTION);
				filter.addAction(WifiManager.RSSI_CHANGED_ACTION);
				registerReceiver(mReceiver, filter);
				HandlerThread handlerThread = new HandlerThread("ht");
				handlerThread.start();
				Looper looper = handlerThread.getLooper();
				handler = new Handler(looper);
				this.registerReceiver(mReceiver, filter, null, handler); // Will not run on main thread
				wifiManager.startScan();
				Log.d(TAG, "initWIFIScan()");
			//}
		}
		catch(Exception e)
		{
		 printToast(e.toString());
		}
	}
	
	private BroadcastReceiver mReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			try{
				WifiConfiguration wfc;
				final String action = intent.getAction();
				if(!wifiManager.isWifiEnabled())
				{
					while(wifiManager.isWifiEnabled() == false)
					{
						wifiManager.setWifiEnabled(true);
					}
					Thread.sleep(5000);
					
					List<WifiConfiguration> list = wifiManager.getConfiguredNetworks();
					for( WifiConfiguration i : list ) {
					    if(i.SSID != null && i.SSID.equals("\"" + "AP-CENTER" + "\"")) {
			            	wfc = WifiConnect.ConnectWPA(i.SSID, UsingAP.getPassword());
			            	WifiConnect.connect(wfc, wifiManager, i.SSID);
					        break;
					    }       
					}
					String SSid = null;
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
					
					Thread.sleep(5000);
				}
				
				if(wifiManager.isWifiEnabled()){
					if (action.equals(WifiManager.SCAN_RESULTS_AVAILABLE_ACTIONWifiManager.RSSI_CHANGED_ACTION)) {
						getWIFIScanResult(); // get WIFISCanResult
						wifiManager.startScan(); // for refresh
					
					} else if (action.equals(WifiManager.NETWORK_STATE_CHANGED_ACTION)) {
					sendBroadcast(new Intent("wifi.ON_NETWORK_STATE_CHANGED"));
					}
				}
			}
			catch(Exception e){
				printToast(e.toString());
			}
		}			
	};
	
	private void getWIFIScanResult() throws InterruptedException {
		// TODO Auto-generated method stub
		if(!wifiManager.isWifiEnabled())
		{
			WifiConfiguration wfc;
			wifiManager.setWifiEnabled(true);
			Thread.sleep(6000);
			
			String SSid = null;
			 List<ScanResult> scanResults = wifiManager.getScanResults();
		    	for(ScanResult scanResult : scanResults)
		    	{

		             if(scanResult.SSID.compareTo(UsingAP.getSSID()) == 0)
		             {
		            	if(Math.abs(scanResult.level) < 75)
		            	{
		            		SSid = scanResult.SSID;
		            	}

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
			Thread.sleep(5000);
		}

		if(wifiManager.isWifiEnabled()){
			final String str[] = new String[2];
					try
					{
					mScanResult = wifiManager.getScanResults();
					String Ap1_Mac = "00:26:66:9a:f5:44";
					String Ap2_Mac = "90:9f:33:7e:d6:de";
					//AP1 = -100;
					//AP2 = -100;
					textStatus.setText("Scan count is \t" + ++scanCount + " times \n");
					
					textStatus.append("=======================================\n");
					for (int i = 0; i < mScanResult.size(); i++) {
						ScanResult result = mScanResult.get(i);
						if(result.BSSID.equals(Ap1_Mac) || result.BSSID.equals(Ap2_Mac))
						textStatus.append((i + 1) + ". SSID : " + result.SSID.toString() + "\t\t MAC : " +result.BSSID.toString() 
								+ "\t\t RSSI : " + result.level + " dBm\n");
						
						String string = result.BSSID.toString();
						if (string.equals(Ap1_Mac)) {
							AP1 = result.level;
						}
						else if(string.equals(Ap2_Mac))
						{
							AP2 = result.level;
						}
					}
					textStatus.append("=======================================\n");
					
					str[0] = String.valueOf(AP1);
					str[1] = String.valueOf(AP2);
					sendrssi = (SendRssi) new SendRssi(this).execute(str);
					
					
					count++;
					}
					catch(Exception e)
					{
					 printToast(e.toString());
					}
				}			
			}
*/
//EndRegion	
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.home_activity);
        
        home = this;
        Position_X = "0";
        Position_Y = "0";
        default_gyval = new float[3];
        intent = getIntent();
        String info = intent.getExtras().getString("info");
        String[] str = new String(info).split("/");
        id= (str[0]);
        name = (str[1]);
        drift_revision_X = Float.valueOf(str[2]);
        drift_revision_Y = Float.valueOf(str[3]);
        drift_revision_Z = Float.valueOf(str[4]);
        relaCP = (str[5]);
        LimitX = str[6];
        LimitY = str[7];
        /*mcd = new String[2];
        mcd[0] = String.valueOf(mcdx);
        mcd[1] = String.valueOf(mcdy);*/
        wifiManager = (WifiManager) this.getSystemService(Context.WIFI_SERVICE);
    	UsingAP = new AccessPoint();
		UsingAP.setSSID("AP-CENTER");
		UsingAP.setMacAddress("64:e5:99:63:67:40");
		UsingAP.setPassword("wifi2486");     
        IsRunning = false;
		sendrssi = (SendRssi) new SendRssi(this).execute();
		
		// 시스템서비스로부터 SensorManager 객체를 얻는다. 
		mSensorManager = (SensorManager)getSystemService(SENSOR_SERVICE);
        // SensorManager를 이용해서 가속도센서 객체를 얻는다.
		accSensor = mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
        mField = mSensorManager.getDefaultSensor(Sensor.TYPE_MAGNETIC_FIELD);
        mGyroscope = mSensorManager.getDefaultSensor(Sensor.TYPE_GYROSCOPE);
        
        //화면 컨트롤 정보값 획득 --------------------------------------------------------------------------
        Text = (TextView) findViewById(R.id.textView1);
        SpeedText = (TextView) findViewById(R.id.textView2);
        nullText = (TextView) findViewById(R.id.textView3);
        btnRefresh = (Button)findViewById(R.id.btnRefresh);
        btnRefresh.setOnClickListener(this);
        device = (ImageView) findViewById(R.id.pinImageView);
        device2 = (ImageView) findViewById(R.id.pinImageView2);
        device2.setVisibility(View.INVISIBLE);
        bkgImage = (ImageView) findViewById(R.id.bkgImageView);
        DirectImage = (ImageView) findViewById(R.id.DirectImageView);
        DirectImage.setScaleType(ScaleType.MATRIX);
        RL = (RelativeLayout)findViewById(R.id.relativelayout1);       
        
        //missing_btn = (Button)findViewById(R.id.missing_btn);
        find_btn =  (Button)findViewById(R.id.find_btn);
        //missing_btn.setVisibility(View.INVISIBLE);
        find_btn.setVisibility(View.INVISIBLE);
    	      		
        DisplayMetrics metrics = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(metrics);
        screenWidth = metrics.widthPixels;
        screenHeight = metrics.heightPixels;
        
        //******************
        gyroOrientation[0] = 0.0f;
        gyroOrientation[1] = 0.0f;
        gyroOrientation[2] = 0.0f;
        // initialise gyroMatrix with identity matrix
        gyroMatrix[0] = 1.0f; gyroMatrix[1] = 0.0f; gyroMatrix[2] = 0.0f;
        gyroMatrix[3] = 0.0f; gyroMatrix[4] = 1.0f; gyroMatrix[5] = 0.0f;
        gyroMatrix[6] = 0.0f; gyroMatrix[7] = 0.0f; gyroMatrix[8] = 1.0f;
        //******************
        
      //버특 객체 초기화
        for(int i = 0 ; i < btn.length ; i++)
        {
        	btn[i] = new Button(this);
        	RL.addView(btn[i]);
        }
        //버튼객체, 스크린 넓이, 높이
        room_btn = new btnRoom(btn, screenWidth, screenHeight);
        
        bkgImage.setBackgroundResource(R.drawable.floor_3);//3층이미지
        room_btn.Viewbtn();//버튼보여줌
        room_btn.SetF3();//3층 세팅/ 버튼,레이아웃
        
        limit_xx = LimitX;
        limit_yy = LimitY;
        //client = new DefaultHttpClient();
        //진동 
        vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE); 
        //vibrator.vibrate(vibPattern, 0);
        
        //ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
        //List<RunningTaskInfo> tasks =am.getRunningTasks(10); 
	}  
	
  	// 해당액티비티가포커스를잃으면방향데이터를얻어도소용이없으므로리스너를해제한다.
    protected void onPause() {
        super.onPause();
        // 센서 값이 필요하지 않는 시점에 리스너를 해제해준다.
        //mSensorManager.unregisterListener(this);
        if(mp!=null)
			mp.release();
        if(vibrator!=null)
        	vibrator.cancel();
    }  
    
    // 해당액티비티가포커스를얻으면방향데이터를얻을수있도록리스너를등록한다.    
    protected void onResume() {//재개하다
        super.onResume();
        // 센서값을이컨텍스트에서받아볼수있도록리스너를등록한다.
        mSensorManager.registerListener((SensorEventListener) this, mGyroscope,SensorManager.SENSOR_DELAY_NORMAL);
        mSensorManager.registerListener((SensorEventListener) this, accSensor,  SensorManager.SENSOR_DELAY_NORMAL); //가속도 센서 리스너 오브젝트 등록
        mSensorManager.registerListener((SensorEventListener) this,mField, SensorManager.SENSOR_DELAY_NORMAL); //자기장 센서 리스너 오브젝트 등록
    }
    
	
/*    @Override
    protected void onDestroy(){ 	
		//android.os.Process.killProcess(android.os.Process.myPid());	//모든 프로세스가 종료되게되는둣..
    	while(sendrssi.isCancelled())
    	{
    		try {
				Thread.sleep(100);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
    	}

    }*/
	
	@Override
	public boolean onKeyDown(int KeyCode, KeyEvent event)
	{
		switch(KeyCode)
		{
		case KeyEvent.KEYCODE_BACK:
			new AlertDialog.Builder(this)
			.setTitle("프로그램 종료")
			.setMessage("프로그램을 종료 하시겠습니까?")
			.setPositiveButton("예", new DialogInterface.OnClickListener() {	
				
				public void onClick(DialogInterface dialog, int which) {
					// TODO Auto-generated method stub
					IsRunning = true;
					if(sendrssi.cancel(IsRunning))
					{		
						rd = (ReadData) new ReadData().execute(id);
						finish();
					}
				}
			})
			.setNegativeButton("아니오", null)
			.show();
			
			break;
		default:
			break;
		}
		
		return super.onKeyDown(KeyCode, event);
	}
    
	
	  /* @Override
	    public void onClick(View view) {
	    	if(view.getId() == R.id.btnLogout)
	    	{
		        try
		        {	
		        	rd = (ReadData) new ReadData().execute(id);
		        	printToast("Success");
		        	
		        	finish();
		        } catch(Exception e)
		        {
		        	printToast(e.toString());
		        }
	    	}
	    	
	    	if(view.getId() == R.id.Start)
	    	{
	    		IsRunning = false;
	    		printToast("Start Navigation");
				count=0;
				Log.d(TAG, "OnClick() btnScanStart()");
				sendrssi = (SendRssi) new SendRssi(this).execute();
				//initWIFIScan();
	    	}
	    	
	    	if(view.getId() == R.id.Stop)
	    	{
	    		printToast("Stop Navigation");
	    		Log.d(TAG, "OnClick() btnScanStop()");
	    		IsRunning = true;
				//unregisterReceiver(mReceiver); // stop WIFISCan --> BroadCastReciever는 사용안하는 걸로...
	    	}
	    	
	    }*/
	
    public void printToast(String messageToast) {
		Toast.makeText(this, messageToast, Toast.LENGTH_LONG).show();
	}
    //Region - WfifAsyncTask
   /* private boolean isStopScanning = false;	
    private WifiInfo wifiinfo;

   
    private class WifiScanningAsyncTask extends AsyncTask<Void, String, Void> 
    {
    	private WifiConfiguration wfc;
    	private int StayPointstep = 0;
    	int count = 0;
    	int old_step = 0;
    	int nullstep = 0;
    	int TwoStep = 0;
    	String Old_Direction;
    	int F_flag;
    	int F1_flag = 0;
    	int degrees;
    	
    		@Override
            protected Void doInBackground(Void... arg0) 
            {
                while (!loginActivity.this.isStopScanning) 
                {
                	try
                	{
                     
                            WifiManager.WifiLock wifiLock = wifiManager.createWifiLock(
                                            WifiManager.WIFI_MODE_SCAN_ONLY,
                                            "WifiSignalStrengthCollector");
                            
                            //Wifi 가 꺼져있다면 자동적으로 On
                            if (!wifiManager.isWifiEnabled())
                            {
                                wifiManager.setWifiEnabled(true);
                            }
                            if(wifiManager.isWifiEnabled())
                            {
    	                        wifiLock.acquire();
    	
    	                        publishProgress("Scanning...");
    	
    	                        wifiManager.startScan();
    	                        
    	                        List<ScanResult> scanResults = wifiManager.getScanResults();
    	                        
    	                        if (scanResults != null)
    	                        {
    	                            //신호강도가 약해지면 서버와 연결이 끈기므로 일정강도 이하에선 다른 wifi에 접속하도록 유도
    	                        	wifiinfo = wifiManager.getConnectionInfo();

    	                			if(Math.abs(wifiinfo.getRssi()) > 75)
    	                			{
    	                				String SSid = null;
    	                        		for(ScanResult scanResult : scanResults)
    	                        		{
    	                                    for(AccessPoint Ap : UsingAP)
    	                                    {
    		                                	if(scanResult.SSID.compareTo(Ap.getSSID()) == 0)
    		                                	{
    			                        			if(Math.abs(scanResult.level) < 75)
    			                        			{
    			                        				SSid = scanResult.SSID;
    			                        			}
    		                                	}
    	                                    }
    	                        		}
    	                        		
    	                                for(AccessPoint Ap : UsingAP)
    	                                {
    	                                	if(SSid.compareTo(Ap.getSSID()) == 0)
    	                                	{
    	                                		if(SSid != null)
    	                                		{
    	                                			//이부분에서 시간차에 따른 에러가 존재할수있음 (서버로 전송하는데에 연결이 늦어지는 경우가 생김)
    	                                			wfc = WifiConnect.ConnectWPA(SSid, Ap.getPassword());
    	                                			WifiConnect.connect(wfc, wifiManager, SSid);
    	                                		}
    	                                	}
    	                                }
    	                            }
    	                			
    	
    	                    		
    	                        	//신호강도 30회 검색후 평균 값
    	                        	while(count < 30)
    	                        	{
    	                        		if(scanResults != null)
    	                        		{
    		                            	for(int i = 0 ;  i < FingerDB.getAP_MACAddress().length ; i++)
    		                            	{
    		                            		for(ScanResult scanResult : scanResults)
    		                            		{
    		                            			if(FingerDB.getAP_MACAddress()[i].getMacAddress().toLowerCase().compareTo(scanResult.BSSID.toLowerCase()) == 0)
    		                            			{
    		                            				FingerDB.getAP_MACAddress()[i].setSignalLevel(scanResult.level);
    		                            				FingerDB.getAP_MACAddress()[i].setSSID(scanResult.SSID);
    		                            				FingerDB.getAP_MACAddress()[i].setCount(1);
    		                            				FingerDB.getAP_MACAddress()[i].SetAvgSignal(FingerDB.getAP_MACAddress()[i].getSignalLevel(), count+1);
    		                            			}
    		                            		}
    		                            	}
    		                            	wifiManager.startScan();
    		                                scanResults = wifiManager.getScanResults();
    	                        		}
    	                        		count++;
    	                        	}
    	                    		
    	                        	Log.d("Fingerprint", "CheckMYLocation. Start!");
    	                       
    	               	      
    	                        }
    	                        wifiLock.release();
    	                    
                    	}
                	}
                	catch(Exception e)
                	{
                		
                	}
               	
                	if(step == 0)
                	{
                		step = 1;
                	}
                	
                	
                	count = 0;
                	
                    try {
                        Thread.sleep(refreshRate);
    	            } catch (InterruptedException e) {
    	                    // TODO Auto-generated catch block
    	                    e.printStackTrace();
    	            }

            	}                	            
                
            return null;
        }
    		
    		 protected void onProgressUpdate(String... progress) 
    	        {
    	                Log.d("SCANNER", progress[0]);
    	        }

    	        @Override
    	        protected void onPostExecute(Void result) 
    	        {
    	                super.onPostExecute(result);
    	        }
    	        
    	        @Override
    	        protected void onCancelled(){
    	        	super.onCancelled();
    	        	this.cancel(isStopScanning);
    	        	Log.d("BackWork", "END!!!!");
    	        }
    }*/
    //EndRegion

	@Override
	public void onAccuracyChanged(Sensor arg0, int arg1) {

	}

	@Override
	public void onSensorChanged(SensorEvent event) {
		 switch(event.sensor.getType()) {
	        
	       case Sensor.TYPE_GYROSCOPE: 
	          gyro_degree(event);
	            break; 
	       
	       case Sensor.TYPE_ACCELEROMETER:    
	          mGravity = event.values.clone(); 
	            break;
	       
	       case Sensor.TYPE_MAGNETIC_FIELD:   
	          mMagnetic  = event.values.clone();
	            break;
	            
	        default:
	            return;    
	        }
	       if(mag_deg_count<40)
	       {
	          mag_degree();
	       }
	        if (event.sensor.getType() == Sensor.TYPE_ACCELEROMETER) {
	           long currentTime = System.currentTimeMillis();
	         long gabOfTime = (currentTime - lastTime); //현시- 전시
	         //최근측정한시간과현재시간이0.1초이상차이나면흔듬을감지함
	         if (gabOfTime > 100) { 
	            lastTime = currentTime; //현재시간을lastTime에값대입하고다음에사용
	            x = event.values[SensorManager.DATA_X]; //float형 변수x에 센서로받은X축에적용되는힘
	            y = event.values[SensorManager.DATA_Y];
	            z = event.values[SensorManager.DATA_Z];
	            //시간에 대한 가속도의 변화를 측정
	            //가속도 = 속도의 변화량(나중속도-처음속도)/걸린시간
	            speed = Math.abs(x + y + z - lastX - lastY - lastZ) / gabOfTime * 10000;
	            if (speed > 150) {
	               if(System.currentTimeMillis() - LastStepTime > 350) //0.35초
	               {
	                  step++;
	                  Log.e("Step!", "SHAKE"+ " : " + String.valueOf(step) +"///"+ String.valueOf(speed));
	                  if(speed > 550)
	                  {
	                     stepover++;
	                     Log.e("Step!", "StepOver"+ " : " + String.valueOf(stepover)+ "///"+ String.valueOf(speed));
	                  }
	               }
	               LastStepTime = System.currentTimeMillis();
	            }
	            lastX = event.values[DATA_X];
	            lastY = event.values[DATA_Y];
	            lastZ = event.values[DATA_Z];
		        nullText.setText("  걸음 : " + String.valueOf(step) /*+ "  뜀 : "+String.valueOf(stepover)*/);
	         }
	      }   
	}
	
    public void gyro_degree(SensorEvent event)
    {
    	 if(mag_deg_count<40)
             return;
           
           float[] deltaVector = new float[4];

           if(mag_deg_count>=40)
           {
              Real_gyroValues = event.values.clone();
           }
           if(mGravity != null && Real_gyroValues != null) {
              if(timestamp != 0) {
                    final float dT = (event.timestamp - timestamp) * NS2S;
                    //System.arraycopy(event.values, 0, gyro, 0, 3);
                    Prev_gyroValues[0] = Real_gyroValues[0] + drift_revision_X;
                    Prev_gyroValues[1] = Real_gyroValues[1] + drift_revision_Y;
                    Prev_gyroValues[2] = Real_gyroValues[2] + drift_revision_Z;
                    /*System.out.println(drift_revision_X+"@@@@@@@@");
                    System.out.println(drift_revision_Y+"@@@@@@@@");
                    System.out.println(drift_revision_Z+"@@@@@@@@");
                    System.out.println(relaCP+"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    System.out.println(LimitX+"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    System.out.println(LimitY+"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    System.out.println(selectRoom+"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");*/
                    getRotationVectorFromGyro(Prev_gyroValues, deltaVector, dT / 2.0f);                            
               }
              float[] deltaMatrix = new float[9];
                SensorManager.getRotationMatrixFromVector(deltaMatrix, deltaVector);     

                gyroMatrix = matrixMultiplication(gyroMatrix, deltaMatrix);  

                SensorManager.getOrientation(gyroMatrix, gyroOrientation);
                updateDirection(gyroOrientation[0]);
                //updateDirection(Prev_gyroValues[2]);
               timestamp = event.timestamp;              
            }
           
           //Text.setText(EWSN + " // ");
           //SpeedText.setText(Azimuth);
    }
	
    public void mag_degree()
    {
    	 float[] temp = new float[9];
         float[] Mag_Degree = new float[9];
         SensorManager.getRotationMatrix(temp, null, mGravity, mMagnetic);
          SensorManager.getOrientation(temp, Mag_Degree); //RP에값이포함되어반환된다  
          //Radian 값을Degree 값으로변환한다.
          Mag_Degree[0] = (float)Math.toDegrees(Mag_Degree[0]);
       
          //0 보다 작은 값인 경우 360을 더한다.
          if(Mag_Degree[0] < 0)
             Mag_Degree[0] += 360;
          
          if(mag_deg_count<40)
          {
             mag_degree = (int)Mag_Degree[0]; 
             Azimuth=AzimuthCheck(Mag_Degree[0]);
          }

          Matrix mt = new Matrix();
          
          if(mag_deg_count<40)
         {
            mt.postRotate(mag_degree + 110);
            mag_deg_count++;
         }
          mt.postTranslate(100, 100);
          
          DirectImage.setImageMatrix(mt);//위에서설정한matrix를 사용하겠다.
          BitmapDrawable back = (BitmapDrawable) this.getResources().getDrawable(R.drawable.direct);//비트맵을얻어옴
          Bitmap bm = back.getBitmap();//얻어온거비트맵bm에 대입해줌
          bm = Bitmap.createScaledBitmap(bm, 55, 55, true);//이미지리사이징/55씩줄임
          
          DirectImage.setImageBitmap(bm); //이미지세팅
          DirectImage.setAlpha(100); //알파값조절
    }
	//방향
    private void updateDirection(float Degree) {
    	 double gyro_degrees = Math.toDegrees(Degree);//자이로      
         Matrix mt = new Matrix();    
         if(mag_deg_count>=40)
         {
            gyro_degrees=gyro_degrees+(mag_degree + 110);
            mt.postRotate((int)gyro_degrees);
         }
          mt.postTranslate(100, 100);
          
          DirectImage.setImageMatrix(mt);//위에서설정한matrix를 사용하겠다.
          BitmapDrawable back = (BitmapDrawable) this.getResources().getDrawable(R.drawable.direct);//비트맵을얻어옴
          Bitmap bm = back.getBitmap();//얻어온거비트맵bm에 대입해줌
          bm = Bitmap.createScaledBitmap(bm, 55, 55, true);//이미지리사이징/55씩줄임
          
          DirectImage.setImageBitmap(bm); //이미지세팅
          DirectImage.setAlpha(100); //알파값조절     
          
          EWSN = gyroAzimuthCheck(gyroOrientation[0], Azimuth);
    }
    
    private void getRotationVectorFromGyro(float[] gyroValues,
            float[] deltaRotationVector,
            float timeFactor)
   {
      float[] normValues = new float[3];
      gyroValues[0] = Prev_gyroValues[0]-SIGN(Prev_gyroValues[0])*0.0006f; 
      gyroValues[1] = Prev_gyroValues[1]-SIGN(Prev_gyroValues[1])*0.0006f; 
      gyroValues[2] = Prev_gyroValues[2]-SIGN(Prev_gyroValues[2])*0.0006f; 
      // Calculate the angular speed of the sample
      float omegaMagnitude =
      (float)Math.sqrt(gyroValues[0] * gyroValues[0] +
      gyroValues[1] * gyroValues[1] +
      gyroValues[2] * gyroValues[2]);
      
      // Normalize the rotation vector if it's big enough to get the axis
      if(omegaMagnitude > EPSILON) {
      normValues[0] = gyroValues[0] / omegaMagnitude;
      normValues[1] = gyroValues[1] / omegaMagnitude;
      normValues[2] = gyroValues[2] / omegaMagnitude;
      }
      
      // Integrate around this axis with the angular speed by the timestep
      // in order to get a delta rotation from this sample over the timestep
      // We will convert this axis-angle representation of the delta rotation
      // into a quaternion before turning it into the rotation matrix.
      float thetaOverTwo = omegaMagnitude * timeFactor;
      float sinThetaOverTwo = (float)Math.sin(thetaOverTwo);
      float cosThetaOverTwo = (float)Math.cos(thetaOverTwo);
      deltaRotationVector[0] = sinThetaOverTwo * normValues[0];
      deltaRotationVector[1] = sinThetaOverTwo * normValues[1];
      deltaRotationVector[2] = sinThetaOverTwo * normValues[2];
      deltaRotationVector[3] = cosThetaOverTwo;
   }
    
    private float[] matrixMultiplication(float[] A, float[] B) {
        float[] result = new float[9];
     
        result[0] = A[0] * B[0] + A[1] * B[3] + A[2] * B[6];
        result[1] = A[0] * B[1] + A[1] * B[4] + A[2] * B[7];
        result[2] = A[0] * B[2] + A[1] * B[5] + A[2] * B[8];
     
        result[3] = A[3] * B[0] + A[4] * B[3] + A[5] * B[6];
        result[4] = A[3] * B[1] + A[4] * B[4] + A[5] * B[7];
        result[5] = A[3] * B[2] + A[4] * B[5] + A[5] * B[8];
     
        result[6] = A[6] * B[0] + A[7] * B[3] + A[8] * B[6];
        result[7] = A[6] * B[1] + A[7] * B[4] + A[8] * B[7];
        result[8] = A[6] * B[2] + A[7] * B[5] + A[8] * B[8];
     
        return result;
    }
    
    public String AzimuthCheck(float mag_degree)
    {
       if(mag_degree>=337.5||0<=mag_degree && mag_degree<=22.5)//북
       {
          return "N";
       }
       else if(22.5<=mag_degree&&mag_degree<=67.5)//북동
       {
          return "NE";
       }
       else if(292.5<=mag_degree&&mag_degree<=337.5)//북서
       {
          return "NW";
       }
       else if(67.5<=mag_degree&&mag_degree<=112.5)//동
       {
          return "E";
       }
       else if(247.5<=mag_degree&&mag_degree<=292.5)//서
       {
          return "W";
       }
       else if(157.5<=mag_degree&&mag_degree<=202.5)//남
       {
          return "S";
       }
       else if(112.5<=mag_degree&&mag_degree<=157.5)//남동
       {
          return "SE";
       }
       else if(202.5<=mag_degree&&mag_degree<=247.5)//남서
       {
          return "SW";
       }
       
       return "NONE";
    }
    public String gyroAzimuthCheck(float gyroOrientationValue, String Azimuth)
    {
       double gyro_degrees = Math.toDegrees(gyroOrientationValue);
       
       if(Azimuth=="N")
       {
          if(-45<=gyro_degrees&&gyro_degrees<=45)
             return "N";
          else if(45<=gyro_degrees&&gyro_degrees<=135)
             return "E";
          else if((135<=gyro_degrees&&gyro_degrees<=179.9f)||(-179.9f<=gyro_degrees&&gyro_degrees<=-135))
             return "S";
          else if(-135<=gyro_degrees&&gyro_degrees<=-45)
             return "W";
       }
       else if(Azimuth=="E")
       {
          if(-45<=gyro_degrees&&gyro_degrees<=45)
             return "E";
          else if(45<=gyro_degrees&&gyro_degrees<=135)
             return "S";
          else if((135<=gyro_degrees&&gyro_degrees<=179.9f)||(-179.9f<=gyro_degrees&&gyro_degrees<=-135))
             return "W";
          else if(-135<=gyro_degrees&&gyro_degrees<=-45)
             return "N";
       }
       else if(Azimuth=="S")
       {
          if(-45<=gyro_degrees&&gyro_degrees<=45)
             return "S";
          else if(45<=gyro_degrees&&gyro_degrees<=135)
             return "W";
          else if((135<=gyro_degrees&&gyro_degrees<=179.9f)||(-179.9f<=gyro_degrees&&gyro_degrees<=-135))
             return "N";
          else if(-135<=gyro_degrees&&gyro_degrees<=-45)
             return "E";
       }
       else if(Azimuth=="W")
       {
          if(-45<=gyro_degrees&&gyro_degrees<=45)
             return "W";
          else if(45<=gyro_degrees&&gyro_degrees<=135)
             return "N";
          else if((135<=gyro_degrees&&gyro_degrees<=179.9f)||(-179.9f<=gyro_degrees&&gyro_degrees<=-135))
             return "E";
          else if(-135<=gyro_degrees&&gyro_degrees<=-45)
             return "S";
       }
       else if(Azimuth=="NE")
       {
          if(0<=gyro_degrees&&gyro_degrees<=90)
             return "E";
          else if(90<=gyro_degrees&&gyro_degrees<=179.9)
             return "S";
          else if(-179.9<=gyro_degrees&&gyro_degrees<=-90)
             return "W";
          else if(-90<=gyro_degrees&&gyro_degrees<=0)
             return "N";
       }
       else if(Azimuth=="NW")
       {
          if(0<=gyro_degrees&&gyro_degrees<=90)
             return "N";
          else if(90<=gyro_degrees&&gyro_degrees<=179.9)
             return "E";
          else if(-179.9<=gyro_degrees&&gyro_degrees<=-90)
             return "S";
          else if(-90<=gyro_degrees&&gyro_degrees<=0)
             return "W";
       }
       else if(Azimuth=="SE")
       {
          if(0<=gyro_degrees&&gyro_degrees<=90)
             return "S";
          else if(90<=gyro_degrees&&gyro_degrees<=179.9)
             return "W";
          else if(-179.9<=gyro_degrees&&gyro_degrees<=-90)
             return "N";
          else if(-90<=gyro_degrees&&gyro_degrees<=0)
             return "E";
       }
else if(Azimuth=="SW")
       {
          if(0<=gyro_degrees&&gyro_degrees<=90)
             return "W";
          else if(90<=gyro_degrees&&gyro_degrees<=179.9)
             return "N";
          else if(-179.9<=gyro_degrees&&gyro_degrees<=-90)
             return "E";
          else if(-90<=gyro_degrees&&gyro_degrees<=0)
             return "S";
       }
       return null;
    }
    
    public float SIGN(float w)
    {
    	if(w>0) return 1;
    	if(w<0) return -1;
    	return 0;
    }
    
    @Override
    public void onClick(View view) {
    	if(view.getId() == R.id.btnRefresh)
    	{
    		 mag_deg_count = 0;
             mGravity[0] = 0; mGravity[1] = 0; mGravity[2] = 0;
              mMagnetic[0] = 0; mMagnetic[1] = 0; mMagnetic[2] = 0;
              Real_gyroValues[0] = 0; Real_gyroValues[1] = 0; Real_gyroValues[2] = 0;
              Prev_gyroValues[0] = 0; Prev_gyroValues[1] = 0; Prev_gyroValues[2] = 0;
              //******************
               gyroOrientation[0] = 0.0f;
               gyroOrientation[1] = 0.0f;
               gyroOrientation[2] = 0.0f;
               // initialise gyroMatrix with identity matrix
               gyroMatrix[0] = 1.0f; gyroMatrix[1] = 0.0f; gyroMatrix[2] = 0.0f;
               gyroMatrix[3] = 0.0f; gyroMatrix[4] = 1.0f; gyroMatrix[5] = 0.0f;
               gyroMatrix[6] = 0.0f; gyroMatrix[7] = 0.0f; gyroMatrix[8] = 1.0f;
               //******************
             if(mag_deg_count==0)
                  Toast.makeText(HomeActivity.this, "방향을 수정합니다.", Toast.LENGTH_SHORT).show();	
    	}
    }
    
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuItem itemAdd = menu.add(0, GYRO, Menu.NONE, R.string.GYRO).setIcon(R.drawable.gyroscope);

        //itemAdd.setShortcut('0','a');
    	menu.add(1, MCD, Menu.FIRST, R.string.MCD).setIcon(R.drawable.distance);
    	menu.add(2, ID2, Menu.FIRST, R.string.ID2).setIcon(R.drawable.missing);

        return true;
    }
    
    public boolean onOptionsItemSelected(MenuItem item) {
        super.onOptionsItemSelected(item);
        switch(item.getItemId()){
            case(GYRO):{
            	Intent intent = new Intent(HomeActivity.this, GyroActivity.class);
            	startActivityForResult(intent, GyroAct);
            	
                return true;
            }
            case(MCD):{
            	Intent intent1 = new Intent(HomeActivity.this, MCDistanceActivity.class);
            	int x = Integer.parseInt(limit_xx) / 1000;
            	int y = Integer.parseInt(limit_yy) / 1000;
            	/*float x = Float.parseFloat(limit_xx) / 1000;
            	float y = Float.parseFloat(limit_yy) / 1000;*/
            	
            	intent1.putExtra("mcdx",String.valueOf(x));
            	intent1.putExtra("mcdy",String.valueOf(y));
            	intent1.putExtra("room", selectRoom);
            	startActivityForResult(intent1, MCDistanceAct);
            	
            	return true;
            }
            case(ID2):{
            	Intent intent2 = new Intent(HomeActivity.this, RelationActivity.class);	
            	intent2.putExtra("myID",id);
            	intent2.putExtra("myNAME",name);
            	startActivityForResult(intent2, RelationAct);
            	
            	return true;
            }
        }
        return false;
    }
    
    public void onActivityResult(int requestCode, int resultCode, Intent intent){
    	super.onActivityResult(requestCode, resultCode, intent);
    	switch(requestCode){
	    	case GyroAct:{ // requestCode가 GyroAct인 케이스
	    		Log.e("rstcode",String.valueOf(resultCode));
		    	if(resultCode == RESULT_OK)//GyroAct에서 넘겨진 resultCode가 OK일때만 실행
		    	{ ////////////////////////////////////////////////////////////////////////////////////////////////////////
					mag_deg_count = 0;
					mGravity[0] = 0; mGravity[1] = 0; mGravity[2] = 0;
					mMagnetic[0] = 0; mMagnetic[1] = 0; mMagnetic[2] = 0;
					Real_gyroValues[0] = 0; Real_gyroValues[1] = 0; Real_gyroValues[2] = 0;
					Prev_gyroValues[0] = 0; Prev_gyroValues[1] = 0; Prev_gyroValues[2] = 0;
					//******************
					gyroOrientation[0] = 0.0f;
					gyroOrientation[1] = 0.0f;
					gyroOrientation[2] = 0.0f;
					// initialise gyroMatrix with identity matrix
					gyroMatrix[0] = 1.0f; gyroMatrix[1] = 0.0f; gyroMatrix[2] = 0.0f;
					gyroMatrix[3] = 0.0f; gyroMatrix[4] = 1.0f; gyroMatrix[5] = 0.0f;
					gyroMatrix[6] = 0.0f; gyroMatrix[7] = 0.0f; gyroMatrix[8] = 1.0f;
	
		            if(mag_deg_count==0)
		                 Toast.makeText(HomeActivity.this, "자이로 설정 적용", Toast.LENGTH_SHORT).show();
		           /////////////////////////////////////////////////////////////////////////////////////////////////////////
		    		GA_array_value = intent.getExtras().getIntArray("gyro_data");
		    		
		    		if(GA_array_value[0] == 0)
		    			drift_revision_X = drift_revision_X;
		    		else
		    		{
		    			drift_revision_X = (float)GA_array_value[0]*0.0001f;
		    		}
		    		if(GA_array_value[1] == 0)
		    			drift_revision_Y = drift_revision_Y;
		    		else
		    		{
		    			drift_revision_Y = (float)GA_array_value[1]*0.0001f;
		    		}
		    		if(GA_array_value[2] == 0)
		    			drift_revision_Z = drift_revision_Z;
		    		else
		    		{
		    			drift_revision_Z = (float)GA_array_value[2]*0.0001f;
		    		}
		    		
		    		UGD = (UpdateGyroData) new UpdateGyroData(this).execute();
		    	}
		    	else if(resultCode == 2)
		    	{
					mag_deg_count = 0;
					mGravity[0] = 0; mGravity[1] = 0; mGravity[2] = 0;
					mMagnetic[0] = 0; mMagnetic[1] = 0; mMagnetic[2] = 0;
					Real_gyroValues[0] = 0; Real_gyroValues[1] = 0; Real_gyroValues[2] = 0;
					Prev_gyroValues[0] = 0; Prev_gyroValues[1] = 0; Prev_gyroValues[2] = 0;
					//******************
					gyroOrientation[0] = 0.0f;
					gyroOrientation[1] = 0.0f;
					gyroOrientation[2] = 0.0f;
					// initialise gyroMatrix with identity matrix
					gyroMatrix[0] = 1.0f; gyroMatrix[1] = 0.0f; gyroMatrix[2] = 0.0f;
					gyroMatrix[3] = 0.0f; gyroMatrix[4] = 1.0f; gyroMatrix[5] = 0.0f;
					gyroMatrix[6] = 0.0f; gyroMatrix[7] = 0.0f; gyroMatrix[8] = 1.0f;
					
					if(mag_deg_count==0)
					Toast.makeText(HomeActivity.this, "자이로 설정 초기화 x:0.02205,y:-0.008,z:-0.021", Toast.LENGTH_SHORT).show();
					/////////////////////////////////////////////////////////////////////////////////////////////////////////
					default_gyval = intent.getExtras().getFloatArray("default_gyro");
					drift_revision_X = default_gyval[0];
					drift_revision_Y = default_gyval[1];
					drift_revision_Z = default_gyval[2];
					UGD = (UpdateGyroData) new UpdateGyroData(this).execute();
		    	}
		    	break;
	    	}
	    	case MCDistanceAct :{ 
	    		Log.e("rstcode",String.valueOf(resultCode));
	    		if(resultCode == RESULT_OK)
	    		{	    		
		    		Limit_info = intent.getExtras().getStringArray("data");
		    		Toast.makeText(HomeActivity.this, "제한거리 설정 X:"+Limit_info[0]+", Y:"+Limit_info[1], Toast.LENGTH_SHORT).show();
		    		LimitX = Limit_info[0]; LimitY = Limit_info[1]; selectRoom = Limit_info[2];
		            limit_xx = LimitX;
		            limit_yy = LimitY;
		    		
		    		UMC = (UpdateMCDistance) new UpdateMCDistance(this).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
	    		}
	    		else if(resultCode == 2)
	    		{
	    			Limit_info = intent.getExtras().getStringArray("default_limit");
	    			Toast.makeText(HomeActivity.this, "제한거리 초기화 X:24000 Y:32000", Toast.LENGTH_SHORT).show();
	    			LimitX = Limit_info[0]; LimitY = Limit_info[1]; selectRoom = Limit_info[2];
	    	        limit_xx = LimitX;
	    	        limit_yy = LimitY;
	    	        
	    			UMC = (UpdateMCDistance) new UpdateMCDistance(this).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
	    		}
	    		break;
	    	}
	    	case RelationAct :{
	    		if(resultCode == RESULT_OK)
	    		{
	    			ID2_NAME = intent.getExtras().getString("ID2_NAME");
	    			Toast.makeText(HomeActivity.this, ID2_NAME+"님을 등록 하셨습니다.", Toast.LENGTH_SHORT).show();
	    		}
	    		
	    		break;
	    	}
    	
    	}
    }
    
    public void missing_music()
    {
    	//this.getResources().openRawResource(R.raw.kisang);
    	//mp = MediaPlayer.create(context, R.raw.kisang); 
    	//mp.setAudioStreamType(AudioManager.STREAM_MUSIC);
		//mp.start(); 
    	////////////////////////////////////
    	/*ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
    	List<RunningTaskInfo> tasks =am.getRunningTasks(3); 
    	if(!tasks.isEmpty()) 
    	{
    		int tasksSize = tasks.size();
			 for(int i = 0; i >= tasksSize;  i++) 
			 {
				 RunningTaskInfo taskinfo = tasks.get(i);
				 if(taskinfo.topActivity.getPackageName().equals(getPackageName())) 
				 {
					 am.moveTaskToFront(taskinfo.id, 0);
				 }
			 }
		 }*/
          /*ActivityManager activityapp = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);	  
    	  List<RunningAppProcessInfo> list = (List<RunningAppProcessInfo>)activityapp.getRunningAppProcesses();
    	  for(int i = 0 ; i < list.size() ; i++) {
    	      RunningAppProcessInfo info = list.get(i);
    	      if ( info.processName.equals("com.example.jsonsampleapp") && info.importance == info.IMPORTANCE_BACKGROUND ){ 
    	    	  ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
    	    	  List<RunningTaskInfo> tasks =am.getRunningTasks(3); 
    	    	  if(!tasks.isEmpty()) {
						 int tasksSize = tasks.size();
						 for(int q = 0; q >= tasksSize;  q++) {
							 RunningTaskInfo taskinfo = tasks.get(q);
							 if(taskinfo.topActivity.getPackageName().equals("com.example.jsonsampleapp")) {
								 am.moveTaskToFront(taskinfo.id, 0);
							 }
						 }
					}
    	      }
    	  }*/ 
    	 /*boolean check = false;
         if (!check) {
             // 앱이 실행 중이 아니면 앱을 기동시킨다.
             Intent intent = this.getPackageManager().getLaunchIntentForPackage(getPackageName());
             //intent.setAction(Intent.ACTION_MAIN);
             intent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
             startActivity(intent);
         } else {
             // 앱이 실행 중이면 앱을 FORGROUND로 이동시킨다.
             ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
             List<RunningTaskInfo> tasks = am.getRunningTasks(100);
             if (!tasks.isEmpty()) {
                 int taskSize = tasks.size();
                 for (int i=0;i< taskSize; i ++) {
                     RunningTaskInfo taskInfo = tasks.get(i);
                     if (taskInfo.topActivity.getPackageName().equals(getPackageName())) {
                         check = true; 
                         am.moveTaskToFront(taskInfo.id, 0);
                         System.out.println("486486423865423865423658423654238654238654236542356523656235455623455623455623455623455623455623456234556");
                     }
                 }
             }            
         } */
    	/*ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
        List<RunningTaskInfo> tasks = am.getRunningTasks(100);
        if (!tasks.isEmpty()) {
            int taskSize = tasks.size();
            for (int i=0;i< taskSize; i ++) {
                RunningTaskInfo taskInfo = tasks.get(i);
                if (taskInfo.topActivity.getPackageName().equals(getPackageName())) {
                    am.moveTaskToFront(taskInfo.id, 0);
                    System.out.println("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                }
            }
        } */
    	/*if (Build.VERSION.SDK_INT >= 11) {
    	    ActivityManager am = (ActivityManager) getSystemService(Activity.ACTIVITY_SERVICE);
    	    List<RunningTaskInfo> rt = am.getRunningTasks(Integer.MAX_VALUE);

    	    for (int i = 0; i < rt.size(); i++) 
    	    {
    	           // bring to front
    	           if (rt.get(i).baseActivity.toShortString().indexOf(getPackageName()) > -1) {                     
    	              am.moveTaskToFront(rt.get(i).id, ActivityManager.MOVE_TASK_WITH_HOME);
    	           }
    	    }
    	}*/
    	/*if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) { // 11     
    	    final ActivityManager activityManager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
    	    final List<RecentTaskInfo> recentTasks = activityManager.getRecentTasks(Integer.MAX_VALUE, ActivityManager.RECENT_IGNORE_UNAVAILABLE);
    	     
    	    RecentTaskInfo recentTaskInfo = null;
    	     
    	    for (int i = 0; i < recentTasks.size(); i++) 
    	    {
    	        if (recentTasks.get(i).baseIntent.getComponent().getPackageName().equals(getPackageName())) {
    	           recentTaskInfo = recentTasks.get(i);
    	           break;
    	        }
    	    }
    	     
    	    if(recentTaskInfo != null && recentTaskInfo.id > -1) {
    	        activityManager.moveTaskToFront(recentTaskInfo.persistentId, ActivityManager.MOVE_TASK_WITH_HOME);
    	    }
    	}*/
    	/*Intent intent = new Intent(this, HomeActivity.class);
    	PendingIntent pendingIntent = PendingIntent.getActivity(this, 1, intent, 0);
    	Notification noti = new Notification(R.drawable.missing, "미아발생", System.currentTimeMillis());
    	noti.setLatestEventInfo(this, "missing", "미아발생", pendingIntent);
    	noti.flags = noti.flags|Notification.FLAG_ONGOING_EVENT;
    	startForeground(1000,noti);*/
    	//boolean check = true;
    	/*if(check) {
            // 앱이 실행 중이면 앱을 FORGROUND로 이동시킨다.
            ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
            List<RunningTaskInfo> tasks = am.getRunningTasks(100);
            if (!tasks.isEmpty()) {
                int taskSize = tasks.size();
                for (int i=0;i< taskSize; i ++) {
                    RunningTaskInfo taskInfo = tasks.get(i);
                    if (taskInfo.topActivity.getPackageName().equals(getPackageName())) {
                        //check = true; 
                        am.moveTaskToFront(taskInfo.id, 0);
                        System.out.println("486486423865423865423658423654238654238654236542356523656235455623455623455623455623455623455623456234556");
                    }
                }
            }            
        }*/
    	/*//if (check) {
            // 앱이 실행 중이 아니면 앱을 기동시킨다.
            //Intent intent = this.getPackageManager().getLaunchIntentForPackage(getPackageName());
            intent = new Intent(HomeActivity.this, HomeActivity.class);
            //intent.setAction(Intent.ACTION_MAIN);
            intent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
            startActivity(intent);
        //}    */
    	/*//Intent intent = this.getPackageManager().getLaunchIntentForPackage(getPackageName());
    	Intent intent=new Intent("com.example.jsonsampleapp");
    	intent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
    	startActivity(intent);*/
    	
    	/*ActivityManager am = (ActivityManager)getSystemService(Context.ACTIVITY_SERVICE);
        List<RunningTaskInfo> tasks = am.getRunningTasks(100);
        if (!tasks.isEmpty()) {
            int taskSize = tasks.size();
            for (int i=0;i< taskSize; i ++) {
                RunningTaskInfo taskInfo = tasks.get(i);
                if (taskInfo.baseActivity.getPackageName().equals("com.example.jsonsampleapp")) {
                    //check = true; 
                    am.moveTaskToFront(taskInfo.id, 0);
                }
            }
        } */  
    	callActivity("com.example.jsonsampleapp");
    	/////////////////////////////////
		int tmpID = getApplicationContext().getResources().getIdentifier( "kisang", "raw", "com.example.jsonsampleapp");
		Context con = getApplicationContext();
		mp = MediaPlayer.create(con, tmpID);
		mp.setLooping(true);
		mp.start();
		vibrator.vibrate(vibPattern, 0);
		
		switch(relaCP)
		{
		case "C" : dialogToParent(); break;
		case "P" : dialogToChild(); break;
		}
    }
    
    public void dialogToParent()
    {
    	runOnUiThread(new Runnable() {					
			@Override
			public void run() {
				//Toast.makeText(HomeActivity.this, "미아 발생", Toast.LENGTH_LONG).show();		
		    	AlertDialog.Builder builder = new AlertDialog.Builder(HomeActivity.this);
		    	builder.setTitle("미아 발생!!")
		    	.setMessage("아이의 마지막 위치\n X : "+ Position_X2 + ", Y : " + Position_Y2)
		    	.setCancelable(false)
		    	.setPositiveButton("경고음 종료", new DialogInterface.OnClickListener() {
					
					@Override
					public void onClick(DialogInterface dialog, int whichButton) {
						if(mp!=null)
							mp.release();
						if(vibrator!=null)
							vibrator.cancel();
						
						find_btn.setVisibility(View.VISIBLE);
						//missing_btn.setVisibility(View.INVISIBLE);
						Toast.makeText(HomeActivity.this, "경고음 종료", Toast.LENGTH_SHORT).show();		
					}
				});/*.setNegativeButton("취소", new DialogInterface.OnClickListener() {
					
					@Override
					public void onClick(DialogInterface dialog, int whichButton) {
						dialog.cancel();
					}
				});*/
		    	AlertDialog dialog = builder.create();
		    	
		    	dialog.show();
			}
		});

    }
    
    public void dialogToChild()
    {
    	runOnUiThread(new Runnable() {					
			@Override
			public void run() {
				//Toast.makeText(HomeActivity.this, "미아 발생", Toast.LENGTH_LONG).show();		
		    	AlertDialog.Builder builder = new AlertDialog.Builder(HomeActivity.this);
		    	builder.setTitle("미아 발생!!")
		    	.setMessage("자리에서 기다리세요")
		    	.setCancelable(false)
		    	.setPositiveButton("경고음 종료", new DialogInterface.OnClickListener() {
					
					@Override
					public void onClick(DialogInterface dialog, int whichButton) {
						if(mp!=null)
							mp.release();
						if(vibrator!=null)
							vibrator.cancel();
						
						find_btn.setVisibility(View.VISIBLE);
						//missing_btn.setVisibility(View.INVISIBLE);
						Toast.makeText(HomeActivity.this, "경고음 종료", Toast.LENGTH_SHORT).show();		
					}
				});/*.setNegativeButton("취소", new DialogInterface.OnClickListener() {
					
					@Override
					public void onClick(DialogInterface dialog, int whichButton) {
						dialog.cancel();
					}
				});*/
		    	AlertDialog dialog = builder.create();
		    	
		    	dialog.show();
			}
		});
    }
    
    /*public void moveToFront() {
        if (Build.VERSION.SDK_INT >= 11) { // honeycomb
            final ActivityManager activityManager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
            final List<RunningTaskInfo> recentTasks = activityManager.getRunningTasks(Integer.MAX_VALUE);

            for (int i = 0; i < recentTasks.size(); i++) 
            {
			   Log.d("Executed app", "Application executed : " 
			           +recentTasks.get(i).baseActivity.toShortString()
			           + "\t\t ID: "+recentTasks.get(i).id+"");  
			   // bring to front                
			   if (recentTasks.get(i).baseActivity.toShortString().indexOf("com.example.jsonsampleapp") > -1) {                     
			      activityManager.moveTaskToFront(recentTasks.get(i).id, ActivityManager.MOVE_TASK_WITH_HOME);
			   }
            }
        }
	}*/
    
    public void callActivity(String packageName)
    {	 
    	if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) // 11버전 이상 
    	{  	 
    	    final ActivityManager activityManager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);   	 
    	    List<RecentTaskInfo> recentTasks = activityManager.getRecentTasks(Integer.MAX_VALUE, ActivityManager.RECENT_IGNORE_UNAVAILABLE);  	 
    	    RecentTaskInfo recentTaskInfo = null;
  	 
    	    for (int i = 0; i < recentTasks.size(); i++)    	 
    	    {
    	    //Log.e(LOGKEY, "recentTasks.get(i).baseIntent.getComponent().getPackageName() : "+recentTasks.get(i).baseIntent.getComponent().getPackageName());  	 
    	        if (recentTasks.get(i).baseIntent.getComponent().getPackageName().equals(packageName)) 
    	        { 	 
    	          recentTaskInfo = recentTasks.get(i);   	 
    	          break; 
    	        } 
    	    }    	 
    	    if(recentTaskInfo != null && recentTaskInfo.id > -1) {    	 
    	        activityManager.moveTaskToFront(recentTaskInfo.persistentId, ActivityManager.MOVE_TASK_WITH_HOME);   	 
    	        return;    	 
    	    }  	 
    	}   	 
    }
    
    public void onClick1(View v) 
    {
		switch (v.getId()) 
		{
/*		case R.id.missing_btn:
			//sendrssi.missing_control = 0;	
			if(mp!=null)
				mp.release();
			
			find_btn.setVisibility(View.VISIBLE);
			missing_btn.setVisibility(View.INVISIBLE);
			Toast.makeText(HomeActivity.this, "경고음 종료", Toast.LENGTH_SHORT).show();		
			break;
			*/
		case R.id.find_btn:
			sendrssi.missing_control = 0;
			missingCheck = 1;
			
			if(mp!=null)
				mp.release();
			if(vibrator!=null)
				vibrator.cancel();
			//vibrator.cancel();
			
			runOnUiThread(new Runnable() {					
				@Override
				public void run() {
					find_btn.setVisibility(View.INVISIBLE);
					//missing_btn.setVisibility(View.INVISIBLE);	
				}
			});
			
			Toast.makeText(HomeActivity.this, "찾음 눌림", Toast.LENGTH_SHORT).show();	
			
			break;
		}
	}

    //Region - HttpGet을 이용한 통신
/*    public JSONObject getPositionData(String URL) throws ClientProtocolException, IOException, JSONException
    {
	  	HttpParams params = new BasicHttpParams();
	  	params.setParameter(CoreProtocolPNames.PROTOCOL_VERSION, HttpVersion.HTTP_1_1);
	  	HttpClient httpclient = new DefaultHttpClient(params);
	    HttpGet get = new HttpGet(URL);
	  	//HttpPost get = new HttpPost(URL);
	    HttpResponse response = httpclient.execute(get);
	    int status = response.getStatusLine().getStatusCode();
	 
	    if(status == 200) //sucess
	    {
	    	HttpEntity e = response.getEntity();
			String data = EntityUtils.toString(e);	
			JSONObject positionData = new JSONObject(data);
	
			return positionData;
	    }
	    else
	    {
			return null;
	    }
    }*/
    //EndRegion

    
}




