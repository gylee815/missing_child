package com.example.jsonsampleapp;

public class AccessPoint {

	String SSID;
	String macAddress;
	int signalLevel = 0;
	int sum_signalLevel = 0;
	int step = 0;
	int avg_signalLevel;
	int sub_signalLevel;
	int nearlySubSignal;
	int count;
	

	boolean State = true;
	String Password;
	
	public String getPassword() {
		return Password;
	}
	
	public void setPassword(String password) {
		Password = password;
	}    
	
	public void SetAvgSignal(int signalLevel, int step)
	{
		sum_signalLevel += signalLevel;
		if(step == 30)
		{
			avg_signalLevel = sum_signalLevel / count;
		}
	}
	
	public boolean getState() {
		return State;
	}

	public void setState(boolean state) {
		State = state;
	}

	
	public int getCount() {
		return count;
	}

	public void setCount(int count) {
		this.count += count;
	}

	public void resetCount()
	{
		this.count = 0;
	}

	public int getNearlySubSignal() {
		return nearlySubSignal;
	}

	public void setNearlySubSignal(int nearlySubSignal) {
		this.nearlySubSignal = nearlySubSignal;
	}

	public String getMacAddress() 
	{
		return macAddress;
	}

	public void setMacAddress(String macAddress) 
	{
		this.macAddress = macAddress.toUpperCase();
	}

	public int getSignalLevel() 
	{
		return signalLevel;
	}

	public int getAvg_signalLevel() {
		return avg_signalLevel;
	}


	public void setAvg_signalLevel(int avg_signalLevel) {
		this.avg_signalLevel = avg_signalLevel;
	}


	public void setSignalLevel(int signalLevel) 
	{
		this.signalLevel = signalLevel;
	}

	public String getSSID() 
	{
		return SSID;
	}

	public void setSSID(String sSID) 
	{
		SSID = sSID;
	}
}
