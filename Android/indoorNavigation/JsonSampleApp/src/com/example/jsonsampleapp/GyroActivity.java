package com.example.jsonsampleapp;

import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.SeekBar;
import android.widget.TextView;

public class GyroActivity extends Activity {

	Bundle extra;
	Intent intent;
	public TextView helpXText;
	public TextView helpYText;
	public TextView helpZText;
	public Button set_btn;
	public SeekBar seekX;
	public SeekBar seekY;
	public SeekBar seekZ;
	public int seekX_value;
	public int seekY_value;
	public int seekZ_value;
	public int seekX_value_change;
	public int seekY_value_change;
	public int seekZ_value_change;
	public int[] seek_value_array;
	public float[] default_gyval;
	@Override
	public void onCreate(Bundle savedInstanceState) {
	    super.onCreate(savedInstanceState);
	    setContentView(R.layout.gyro_activity);
	    setTitle("Gyroscope 설정");
	    
	    extra = new Bundle();
	    intent = new Intent();
	    seek_value_array = new int[3];
	    default_gyval = new float[3];
	    default_gyval[0] = 0.02205f;
	    default_gyval[1] = -0.008f;
	    default_gyval[2] = -0.021f;
	    //TextView
	    helpXText = (TextView) findViewById(R.id.helpx);
	    helpYText = (TextView) findViewById(R.id.helpy);
	    helpZText = (TextView) findViewById(R.id.helpz);
	    //Button
	    set_btn = (Button) findViewById(R.id.gyro_settings);
	    //SeekBar
	    seekX = (SeekBar) findViewById(R.id.seek_x);
	    seekY = (SeekBar) findViewById(R.id.seek_y);
	    seekZ = (SeekBar) findViewById(R.id.seek_z);
	    seekX.setProgress(500);
	    seekY.setProgress(500);
	    seekZ.setProgress(500);
	    
	    seekX.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
	    	//SeekBar의 값이 변경되면 실행되는 이벤트
	    	@Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
            	//SeekBar의 값을 TextView에 표시
	    		seekX_value = seekX.getProgress();
            	if(seekX_value>500)
            	{
            		seekX_value_change = (seekX_value-500);
            	}
            	else if(seekX_value<500)
            	{
            		seekX_value_change = (seekX_value-500);
            	}
            	else
            		seekX_value_change = 0;
            	helpXText.setText(String.valueOf(seekX_value_change));
            }

	    	//SeekBar의 슬라이더가 트래킹되고 있을때 실행되는 이벤트
            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
            	
            }

            //SeekBar의 슬라이더가 트래킹이 끝나면 실행되는 이벤트
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
            	seek_value_array[0] = seekX_value_change;        	
            }
        });
	    seekY.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
	    	//SeekBar의 값이 변경되면 실행되는 이벤트
	    	@Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
            	//SeekBar의 값을 TextView에 표시
	    		seekY_value = seekY.getProgress();
            	if(seekY_value>500)
            	{
            		seekY_value_change = (seekY_value-500);
            	}
            	else if(seekY_value<500)
            	{
            		seekY_value_change = (seekY_value-500);
            	}
            	else
            		seekY_value_change = 0;
            	helpYText.setText(String.valueOf(seekY_value_change));
            }

	    	//SeekBar의 슬라이더가 트래킹되고 있을때 실행되는 이벤트
            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
            	
            }

            //SeekBar의 슬라이더가 트래킹이 끝나면 실행되는 이벤트
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
            	seek_value_array[1] = seekY_value_change;
            }
        });
	    seekZ.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
	    	//SeekBar의 값이 변경되면 실행되는 이벤트
	    	@Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
            	//SeekBar의 값을 TextView에 표시
	    		seekZ_value = seekZ.getProgress();
            	if(seekZ_value>500)
            	{
            		seekZ_value_change = (seekZ_value-500);
            	}
            	else if(seekZ_value<500)
            	{
            		seekZ_value_change = (seekZ_value-500);
            	}
            	else
            		seekZ_value_change = 0;
            	helpZText.setText(String.valueOf(seekZ_value_change));
            }

	    	//SeekBar의 슬라이더가 트래킹되고 있을때 실행되는 이벤트
            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
            	
            }

            //SeekBar의 슬라이더가 트래킹이 끝나면 실행되는 이벤트
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
            	seek_value_array[2] = seekZ_value_change;
            }
        });
	    
	}
	
	/*public void onClick(View view)
	{
		extra.putIntArray("data", seek_value_array);
		intent.putExtras(extra);
		this.setResult(RESULT_OK, intent);
		this.finish();
	}*/
	public void onClick(View view)
	{
		switch (view.getId())
		{
		case R.id.gyro_init:
			extra.putFloatArray("default_gyro", default_gyval);
			intent.putExtras(extra);
			this.setResult(2, intent);
			this.finish();
			break;
		case R.id.gyro_settings:
			extra.putIntArray("gyro_data", seek_value_array);
			intent.putExtras(extra);
			this.setResult(RESULT_OK, intent);
			this.finish();
			break;
		}
		
	}

}