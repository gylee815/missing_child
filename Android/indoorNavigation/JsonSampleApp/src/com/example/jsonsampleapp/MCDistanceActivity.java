package com.example.jsonsampleapp;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.text.InputFilter;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.NumberPicker;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RadioGroup.OnCheckedChangeListener;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

public class MCDistanceActivity extends Activity implements OnItemSelectedListener, NumberPicker.OnValueChangeListener{

	private String[] spinnerItem = {"NONE","301ȣ"}; 
	Bundle extra;
	Intent intent;
	EditText edtlimit_X;
	EditText edtlimit_Y;
	//TextView text_X;
	//TextView text_Y;
	String[] limit_info;
    String[] default_limit_info;
    String getEdit_X;
    String getEdit_Y;
    RadioGroup group;
    String spinner_curItem;
    String[] mcd;
    int setting_info;
	InputMethodManager imm;
	
	public void onCreate(Bundle savedInstanceState) {
	    super.onCreate(savedInstanceState);
	    setContentView(R.layout.mcdistance_activity);
	    setTitle("�̾� ����");
	    mcd = new String[3];
	    Intent intent1 = getIntent();
	    mcd[0] = intent1.getStringExtra("mcdx");
	    mcd[1] = intent1.getStringExtra("mcdy");
	    mcd[2] = intent1.getStringExtra("room");
	    
	    extra = new Bundle();
	    intent = new Intent();
	    
	    Spinner sp1 = (Spinner) findViewById (R.id.spinner1);
	    //sp1.setPrompt("��� ����");
	    //���ڿ� ����� ����
        ArrayAdapter<String> list;   
        //����� ��ü�� �����ϰ� ������ ������ ���ҽ��� ���ڿ� ����
        list = new ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, spinnerItem);     
        //���ǳʿ� adapter ����
        sp1.setAdapter(list);       
        //���ǳʰ� ���� ���� �� �̺�Ʈ ó��
        sp1.setOnItemSelectedListener((OnItemSelectedListener) this);
        if(mcd[2].equals("NONE"))
        	sp1.setSelection(0);
        else
        	sp1.setSelection(1);
	    //LayoutInflater inflater = LayoutInflater.from(MCDistanceActivity.this);
	    //View v = inflater.inflate(R.layout.mcdistance_activity, null);

	    edtlimit_X = (EditText)findViewById(R.id.limit_x);
	    edtlimit_Y = (EditText)findViewById(R.id.limit_y);
	    //edtlimit_X.setFilters(new InputFilter[] { new InputFilter.LengthFilter(5) }); 
	    //edtlimit_Y.setFilters(new InputFilter[] { new InputFilter.LengthFilter(5) }); 
	    edtlimit_X.setText(mcd[0]);
	    edtlimit_Y.setText(mcd[1]);

	    limit_info = new String[3];

	    default_limit_info = new String[3];
	    default_limit_info[0] = "24000";
	    default_limit_info[1] = "32000";
	    default_limit_info[2] = "NONE";
	    spinner_curItem = "NONE";
	    
		imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
	}
								//arg1=>���ǳʿ� �������� �ִ� ��, arg2=>�ش� id��
	public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2, long arg3) 
	{
		//���ǳʿ� �������� �ִ� �� 
		TextView tv = (TextView)arg1;
		spinner_curItem =String.valueOf(tv.getText());
	}

	public void onNothingSelected(AdapterView<?> arg0) 
	{
		spinner_curItem = "NONE";
	}
	
	public void onValueChange(NumberPicker picker, int oldVal, int newVal) {

        Log.i("value is",""+newVal);

    }
	
	public void show()
    {
         final Dialog d = new Dialog(MCDistanceActivity.this);
         d.setTitle("     �Ÿ� ����");
         d.setContentView(R.layout.meter_set);
         Button b1 = (Button) d.findViewById(R.id.button1);
         Button b2 = (Button) d.findViewById(R.id.button2);
         final NumberPicker np = (NumberPicker) d.findViewById(R.id.numberPicker1);
         if(setting_info == 0){
	         np.setMaxValue(24);
	         np.setMinValue(0);
         }
         else if(setting_info == 1){
	         np.setMaxValue(32);
	         np.setMinValue(0);
         }
         np.setWrapSelectorWheel(false);
         np.setOnValueChangedListener(this);
         b1.setOnClickListener(new OnClickListener()
         {
          @Override
          public void onClick(View v) {
        	  if(setting_info == 0)
        		  edtlimit_X.setText(String.valueOf(np.getValue()));     	  
        	  else if(setting_info == 1)
        		  edtlimit_Y.setText(String.valueOf(np.getValue()));
        	  
              d.dismiss();
           }    
          });
         b2.setOnClickListener(new OnClickListener()
         {
          @Override
          public void onClick(View v) {
              d.dismiss();
           }    
          });
       d.show();


    }
	
	public void onClick2(View view)
	{
		switch (view.getId())
		{
		case R.id.mcd_init:
			extra.putStringArray("default_limit", default_limit_info);
			intent.putExtras(extra);
			this.setResult(2, intent);
			this.finish();
			break;
			
		case R.id.limit_x:
			setting_info = 0;
			imm.hideSoftInputFromWindow(edtlimit_X.getWindowToken(),5);
			show();
			break;
			
		case R.id.limit_y:
			setting_info = 1;
			imm.hideSoftInputFromWindow(edtlimit_Y.getWindowToken(),5);
			show();
			break;
			
		case R.id.mcd_settings:
			try{
				String zero = "000";
				zero = zero.trim();
			getEdit_X = edtlimit_X.getText().toString(); getEdit_X = getEdit_X.trim();
			getEdit_Y = edtlimit_Y.getText().toString(); getEdit_Y = getEdit_Y.trim();
			 if(getEdit_X.getBytes().length <= 0 || getEdit_Y.getBytes().length <= 0)
			 {//���� �Ѿ�ö��� ó��		      
			       Toast.makeText(MCDistanceActivity.this, "���� �Է��ϼ���.", Toast.LENGTH_SHORT).show();
			       break;
			 }
			 if(Integer.parseInt(getEdit_X)*1000>24000||Integer.parseInt(getEdit_Y)*1000>32000)
			 {
				 Toast.makeText(MCDistanceActivity.this, "������ �ʰ��߽��ϴ�.", Toast.LENGTH_SHORT).show();
			     break;
			 }

			limit_info[0] = edtlimit_X.getText().toString() + zero;//"000";//getEdit_X + zero;
			limit_info[1] = edtlimit_Y.getText().toString() + zero;//"000";//getEdit_Y + zero;
			limit_info[2] = spinner_curItem;
			
			extra.putStringArray("data", limit_info);
			intent.putExtras(extra);
			this.setResult(RESULT_OK, intent);
			this.finish();
			break;
			}
			catch(Exception ex)
			{
				Toast.makeText(this, ex.toString(),Toast.LENGTH_SHORT).show();
				break;
			}

		case R.id.room_cancel_btn:
			this.finish();
	    	   break;
		}
		
	}

}
