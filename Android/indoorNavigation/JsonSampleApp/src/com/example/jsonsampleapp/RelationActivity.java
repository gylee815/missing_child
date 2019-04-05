package com.example.jsonsampleapp;

import java.util.concurrent.ExecutionException;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RadioGroup.OnCheckedChangeListener;
import android.widget.Toast;

public class RelationActivity extends Activity implements RadioGroup.OnCheckedChangeListener{

	Bundle extra;
	Intent intent;
	EditText edt_id;
	//EditText edt_name;
	String getEdit_id;
    //String getEdit_name;
    String myid;
    String myname;
    UpdateRelationData URD;
    RadioButton radio1;
    RadioButton radio2;
    String radio_text;
    String URDstring;    
    ReadRequest RR;
    String requestText;
    Button accept_btn;	
    int Request_control;
    ReadID2 RID;
    String RIDstring;
    String[] str;
    public void onCreate(Bundle savedInstanceState) {
	    super.onCreate(savedInstanceState);
	    setContentView(R.layout.relation_activity);
	    setTitle("관계인 설정");
	    
	    Request_control = 0;
	    String requestCheck = "\"2\"";
	    accept_btn = (Button)findViewById(R.id.accept_btn);
	    //accept_btn.setVisibility(View.INVISIBLE);
	    
	    Intent intent1 = getIntent();
	    myid = intent1.getStringExtra("myID");
	    myname = intent1.getStringExtra("myNAME");
	    extra = new Bundle();
	    intent = new Intent();
	    
	    edt_id = (EditText)findViewById(R.id.rela_id_edit);
	    //edt_name = (EditText)findViewById(R.id.rela_name_edit);
	    radio1 = (RadioButton)findViewById(R.id.radio1);
	    radio2 = (RadioButton)findViewById(R.id.radio2);
	    RadioGroup rg1 = (RadioGroup) findViewById(R.id.radioGroup1);
		rg1.setOnCheckedChangeListener((OnCheckedChangeListener) this);
		radio_text = radio1.getText().toString();
		if(radio_text.equals("보호자") )
			radio_text = "C";
		else if(radio_text.equals("피보호자") )
			radio_text = "P";	
				
		try {
			RR = (ReadRequest) new ReadRequest(this).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
			requestText = RR.get().toString();
			//Toast.makeText(RelationActivity.this, requestText, Toast.LENGTH_SHORT).show();
		} catch (InterruptedException e) {
			e.printStackTrace();
		} catch (ExecutionException e) {
			e.printStackTrace();
		}
		
		if(!requestText.equals(requestCheck))
			accept_btn.setVisibility(View.INVISIBLE);
		
	}
	
	public void onCheckedChanged(RadioGroup arg0, int arg1) 
	{
		switch (arg1) {
		case R.id.radio1: //보호자
			radio_text = radio1.getText().toString();
			//if(radio_text.equals("보호자") )
				radio_text = "C";
			/*else if(radio_text.equals("피보호자") )
				radio_text = "P";*/
			//Toast.makeText(RelationActivity.this, "radio1", Toast.LENGTH_SHORT).show();
			break;

		case R.id.radio2: //피보호자
			radio_text = radio2.getText().toString();
			/*if(radio_text.equals("보호자") )
				radio_text = "C";*/
			//else if(radio_text.equals("피보호자") )
				radio_text = "P";
			//Toast.makeText(RelationActivity.this, "radio2", Toast.LENGTH_SHORT).show();
			break;

		}
	}
	
	//public void setOnCheckedChangeListener (CompoundButton.OnCheckedChangeListener listener) 
	
	public void onClick3(View view) throws InterruptedException, ExecutionException
	{
		switch (view.getId())
		{	
		case R.id.id2_setting_btn:
			//extra.putString("room", spinner_curItem);
			//intent.putExtras(extra);
			//this.setResult(3, intent);
			getEdit_id = edt_id.getText().toString(); getEdit_id = getEdit_id.trim();
			//getEdit_name = edt_name.getText().toString(); getEdit_name = getEdit_name.trim();
			 if(getEdit_id.getBytes().length <= 0 /*|| getEdit_name.getBytes().length <= 0*/)
			 {//빈값이 넘어올때의 처리		      
			       Toast.makeText(RelationActivity.this, "정보를 입력하세요.", Toast.LENGTH_SHORT).show();
			       break;
			 }
			myid = myid.trim();
			if(getEdit_id.equals(myid))
			{
				Toast.makeText(RelationActivity.this, "자신에게는 요청할 수 없습니다.", Toast.LENGTH_SHORT).show();
				break;
			}
			URD = (UpdateRelationData) new UpdateRelationData(this).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
			URDstring = URD.get();
			if(URDstring == null)
			{
				Toast.makeText(RelationActivity.this, "존재하지 않는 ID입니다.", Toast.LENGTH_SHORT).show();
				break;
		    }
			else
			{
				extra.putString("ID2_NAME", getEdit_id);
				intent.putExtras(extra);
				this.setResult(RESULT_OK, intent);
				//Toast.makeText(RelationActivity.this, radio_text, Toast.LENGTH_SHORT).show();
				this.finish();
			}
	    	   break;
	    	   
		case R.id.accept_btn:
			Request_control = 1;
			
			RID = (ReadID2) new ReadID2(this).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
			RIDstring = RID.get();
			str = new String(RIDstring).split("\"");
			
			RR = (ReadRequest) new ReadRequest(this).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
			
			//Request_control = 0;
			Toast.makeText(RelationActivity.this, "요청수락", Toast.LENGTH_SHORT).show();
			accept_btn.setVisibility(View.INVISIBLE);	
	    	   break;
	    	   
		case R.id.cancel_btn:
			this.finish();
	    	   break;
		}
		
	}	
	
}
