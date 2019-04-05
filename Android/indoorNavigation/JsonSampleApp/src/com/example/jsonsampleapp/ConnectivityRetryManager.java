package com.example.jsonsampleapp;

import android.app.Application;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Handler;

public class ConnectivityRetryManager{

	 private static final int INITIAL_DELAY_MILLISECONDS = 5 * 1000;
	    private static final int MAX_DELAY_MILLISECONDS = 5 * 60 * 1000;

	    private int delay;
	    HomeActivity act;
	    Context ctx;
	    public ConnectivityRetryManager(HomeActivity act) {
	        reset();
	        this.act = act;
	        ctx = this.act.getApplicationContext();
	    }

	    /**
	     * Called after a network operation succeeds. Resets the delay to the minimum time and unregisters the listener for restoration of network connectivity.
	     */
	    public void reset() {
	        delay = INITIAL_DELAY_MILLISECONDS;
	    }

	    /**
	     * Retries after a delay or when connectivity is restored. Typically called after a network operation fails.
	     * 
	     * The delay increases (up to a max delay) each time this method is called. The delay resets when {@link reset} is called.
	     */
	    public void retryLater(final Runnable retryRunnable) {
	        // Registers to retry after a delay. If there is no Internet connection, always uses the maximum delay.
	        boolean isInternetAvailable = isInternetAvailable();
	        delay = isInternetAvailable ? Math.min(delay * 2, MAX_DELAY_MILLISECONDS) : MAX_DELAY_MILLISECONDS;
	        new RetryReciever(retryRunnable, isInternetAvailable);
	    }

	    /**
	     * Indicates whether network connectivity exists.
	     */
	    public boolean isInternetAvailable() {
	        NetworkInfo network = ((ConnectivityManager) ctx.getSystemService(Context.CONNECTIVITY_SERVICE)).getActiveNetworkInfo();
	        return network != null && network.isConnected();
	    }

	    /**
	     * Calls a retry runnable after a timeout or when the network is restored, whichever comes first.
	     */
	    private class RetryReciever extends BroadcastReceiver implements Runnable {
	        private final Handler handler = new Handler();
	        private final Runnable retryRunnable;
	        private boolean isInternetAvailable;

	        public RetryReciever(Runnable retryRunnable, boolean isInternetAvailable) {
	            this.retryRunnable = retryRunnable;
	            this.isInternetAvailable = isInternetAvailable;
	            handler.postDelayed(this, delay);
	            ctx.registerReceiver(this, new IntentFilter(ConnectivityManager.CONNECTIVITY_ACTION));
	        }

	        @Override
	        public void onReceive(Context context, Intent intent) {
	            boolean wasInternetAvailable = isInternetAvailable;
	            isInternetAvailable = isInternetAvailable();
	            if (isInternetAvailable && !wasInternetAvailable) {
	                reset();
	                handler.post(this);
	            }
	        }

	        @Override
	        public void run() {
	        	
	            handler.removeCallbacks(this);
	            ctx.unregisterReceiver(this);
	            retryRunnable.run();
	        }
	    }
	
}
