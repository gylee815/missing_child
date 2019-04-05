package com.example.jsonsampleapp;

//import android.support.v7.app.ActionBarActivity;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;
import java.sql.Date;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;
import java.util.concurrent.ExecutionException;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.HttpVersion;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.CoreProtocolPNames;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.content.BroadcastReceiver;
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
import android.net.Uri;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiConfiguration;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Looper;
import android.os.Message;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RadioGroup.OnCheckedChangeListener;
import android.widget.RelativeLayout;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

public class RegistActivity extends Activity implements OnClickListener,
		OnItemSelectedListener, RadioGroup.OnCheckedChangeListener {

	HttpClient client1;
	EditText id, pwd, name, age, loginfo, phone, rname, id2;
	Spinner s;
	RadioGroup rg1;
	RadioButton man, woman;
	WifiManager.WifiLock wifiLock;
	Intent intent;
	ReadDuplicateCheck check;
	ReadRegist rd_regist;
	SendRssi sendrssi;
	WifiManager wifiManager;
	Context context;
	AccessPoint UsingAP;
	boolean IsRunning;
	InputMethodManager imm;
	final static String URL_regist = "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Insert/";
	final static String URL_duplicate = "http://61.81.99.71:8080/RestService/RestServiceImpl.svc/Idcheck/";
	RegistActivity regist;
	int isChecked = 0;
	// ��Ʈ�ѷ� ��ü ------------------------------------------
	public RelativeLayout RL;
	ArrayAdapter adapter;
	// ��ũ�� ũ��----------------------------------------------

	TextView tv;
	private String[] phone_first = { "010", "011", "016", "017", "018", "019" };
	String ID, PW, AGE, NAME, LOGINFO, MISSING/* ,RELATION */, PHONE;
	int SEX/*
			 * ,ID2,NAME2
			 */;
	// ������ ���� ����

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.regist_activity);
		setTitle("Regist Memeber");
		// findViewById(R.id.picker).setOnClickListener(this);
		findViewById(R.id.regist).setOnClickListener(this);
		findViewById(R.id.back).setOnClickListener(this);
		findViewById(R.id.check).setOnClickListener(this);
		findViewById(R.id.age1).setOnClickListener(this);
		rg1 = (RadioGroup) findViewById(R.id.radioGroup1);
		rg1.setOnCheckedChangeListener(this);
		id = (EditText) findViewById(R.id.id1);
		pwd = (EditText) findViewById(R.id.pwd1);
		name = (EditText) findViewById(R.id.name1);
		age = (EditText) findViewById(R.id.age1);
		phone = (EditText) findViewById(R.id.phone1);
		s = (Spinner) findViewById(R.id.spinner);
		man = (RadioButton) findViewById(R.id.radio0);
		woman = (RadioButton) findViewById(R.id.radio1);
		client1 = new DefaultHttpClient();
		wifiManager = (WifiManager) this.getSystemService(Context.WIFI_SERVICE);
		UsingAP = new AccessPoint();
		UsingAP.setSSID("AP-CENTER");
		UsingAP.setMacAddress("64:e5:99:63:67:40");
		UsingAP.setPassword("wifi2486");
		ArrayAdapter<String> list;
		list = new ArrayAdapter(this, android.R.layout.simple_spinner_item, phone_first);
		// ���ǳʿ� adapter ����
		s.setAdapter(list);
		imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);

		// ���ǳʰ� ���� ���� �� �̺�Ʈ ó��
		s.setOnItemSelectedListener(this);

		getWindow().setSoftInputMode(
				WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		//�� ���� ������ id �� �Ӽ� ���� 
	}

	@Override
	public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2,
			long arg3) {
		// TODO Auto-generated method stub

		// ���ǳʿ� �������� �ִ� ��
		tv = (TextView) arg1;

		// ���� �� ���� id���� �������� �ؽ�Ʈ�伱��
		TextView bb = (TextView) findViewById(R.id.pnum);

	

	} // spinner ���� �� �ȿ� �� ������ ����

	@Override
	public void onNothingSelected(AdapterView<?> arg0) {
		// TODO Auto-generated method stub

	}// spinner�� �������� �������� �ʾ��� �� ������ ��

	@Override
	public void onCheckedChanged(RadioGroup arg0, int arg1) {
		// TODO Auto-generated method stub
		switch (arg1) {

		case R.id.radio0:
			SEX = 0;
			break;

		case R.id.radio1:
			SEX = 1;
			break;
		}

	}// ���� ��ư ���� �D

	public String RegistData() throws ClientProtocolException,
			IOException, JSONException {

		ID = id.getText().toString();
		PW = pwd.getText().toString();
		AGE = age.getText().toString();
		NAME = name.getText().toString();
		LOGINFO = "0";
		MISSING = "0";
		PHONE = tv.getText() + phone.getText().toString();

		try {

			HttpGet get = new HttpGet(URL_regist + ID + "/" + PW + "/" + NAME + "/"
					+ AGE + "/" + PHONE + "/" + LOGINFO + "/" + SEX);
			HttpResponse response = client1.execute(get);// ����
			int status = response.getStatusLine().getStatusCode();

			if (status == 200) // sucess
			{
				HttpEntity e = response.getEntity();
				String data = EntityUtils.toString(e);
				int i = 0;
				return data;
			} else {
				printToast("Regist FAIL!!");

				return null;
			}
		} catch (Exception e) {
			printToast(e.toString());
			return null;
		}
	} // ������ insert�� �� ����ϴ� �Լ�

	public String isDuplicated() throws ClientProtocolException,
			IOException, JSONException {

		String ID;

		ID = id.getText().toString();

		try {
			HttpGet get = new HttpGet(URL_duplicate + ID);
			HttpResponse response = client1.execute(get);// ����
			int status = response.getStatusLine().getStatusCode();

			if (status == 200) // sucess
			{
				HttpEntity e = response.getEntity();
				String data = EntityUtils.toString(e);
				int i = 0;
				return data;
			} else {
				printToast("Duplicate Check FAIL!!");

				return null;
			}
		} catch (Exception e) {
			printToast(e.toString());
			return null;
		}
	}// ������ idcheck�Ҷ� ���� �Լ�

	private void Dialog_DatePicker() {
		Calendar c = Calendar.getInstance();
		int cyear = c.get(Calendar.YEAR);
		int cmonth = c.get(Calendar.MONTH);
		int cday = c.get(Calendar.DAY_OF_MONTH);

		DatePickerDialog.OnDateSetListener mDateSetListener = new DatePickerDialog.OnDateSetListener() {
			public void onDateSet(DatePicker view, int year, int monthOfYear,
					int dayOfMonth) {
				String _dateStr = year + "" + (monthOfYear + 1) + ""
						+ dayOfMonth + "";
				Toast.makeText(RegistActivity.this, "������ ��¥��" + _dateStr,
						Toast.LENGTH_SHORT).show();
				age.setText(_dateStr);
			}

		};
		DatePickerDialog alert = new DatePickerDialog(this, mDateSetListener,
				cyear, cmonth, cday);
		alert.show();
	}

	private void combobox_com() {
		Spinner jc;
	}// spinner ������

	@Override
	public void onClick(View v) {

		if (v.getId() == R.id.regist) { //ȸ������ getPersonsData1�� �������� insert�Լ��� ���� ����ȴ�
			if (name.getText().toString().equals("")|| id.getText().toString().equals("")
				|| pwd.getText().toString().equals("")|| phone.getText().toString().equals("")
				|| age.getText().toString().equals("")) {
				Toast.makeText(RegistActivity.this, "�Է¿����Դϴ�", Toast.LENGTH_SHORT).show();
				return;
			}
			
			if(isChecked==0){				
				Toast.makeText(RegistActivity.this, "�ߺ�Ȯ���� ���ּ���!", Toast.LENGTH_SHORT)// �� �κп� 
				.show();
				return;
			}
			
			
			try {

				rd_regist = (ReadRegist) new ReadRegist(this.context, this).execute();
				String data = rd_regist.get();
				if (data.equals("\"success\"")) {
					Toast.makeText(this, "ȸ������ ����", Toast.LENGTH_SHORT).show();
					finish();
				} else
					Toast.makeText(this, "ȸ������ ����", Toast.LENGTH_SHORT).show();

			} catch (Exception e) {
				Log.v("join", e.toString());
				printToast(e.toString());
			}

		} 
		
		else if (v.getId() == R.id.back) {// �ڷΰ��� ��ư
			finish();
		} 
		
		else if (v.getId() == R.id.check) {// ID �ߺ�üũ ��ư
			try {
			
				if (id.getText().toString().equals("")) {
					Toast.makeText(RegistActivity.this, "�Է¿����Դϴ�",
							Toast.LENGTH_SHORT).show();
					return;
				}
				check = (ReadDuplicateCheck) new ReadDuplicateCheck(this.context, this).execute();
				String data = check.get();
				if (data.equals("\"success\"")) {
					Toast.makeText(this, "ID�� ����� �� �ֽ��ϴ�", Toast.LENGTH_SHORT)
							.show();
					isChecked = 1;
					
				} else
					Toast.makeText(this, "�̹� ������� ID�Դϴ�", Toast.LENGTH_SHORT)
							.show();

			} catch (Exception e) {
				Log.v("rcheck", e.toString());
				printToast(e.toString());
			}			
		} 
		
		else if (v.getId() == R.id.age1) {// ������� Ȯ�ι�ư
			try {
				imm.hideSoftInputFromWindow(age.getWindowToken(),5);
			
				Dialog_DatePicker();
			} catch (Exception e) {
				Log.v("picker", e.toString());
				printToast(e.toString());
			}
		}
	} // �� ��ư Ŭ���� �̺�Ʈ�߻� �Լ�

	@Override
	public boolean onKeyDown(int KeyCode, KeyEvent event) {
		switch (KeyCode) {
		case KeyEvent.KEYCODE_BACK:
			finish();
			break;
		default:
			break;
		}
		return super.onKeyDown(KeyCode, event);
	}// �ڵ����� �ڷΰ��� ��ư Ŭ���� �߻��ϴ� �̺�Ʈ

	public void printToast(String messageToast) {
		 Toast.makeText(this, messageToast, Toast.LENGTH_LONG).show();
	}

}
