/*
 * CheckGPS.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

 package com.douglas.common;
 import com.douglas.common.*;
 import res.layout.*;
 import net.rim.device.api.system.*;
 import net.rim.device.api.ui.*;
 import net.rim.device.api.ui.component.*;
 import net.rim.device.api.ui.container.*;
 import res.layout.*;
 import java.util.Timer;  
 import java.util.TimerTask;  



/**
 * 
 */
public class CheckGPS extends TimerTask{  
         GPS gps;  
         Timer timer;  
         public static String URL;
         public static String StoredUrl;
         public String gpstxt;
         public PopupScreen popUp;
         public CheckGPS() {  
          
         }  
   
        public void run() {  
             double lat;  
             double lng;  
             lat = 0;  
             lng = 0;  
   
             lat = gps.getLatitude();  
             lng = gps.getLongitude();  
             
             if (lat != 0.0 & lng != 0.0) {  
                // synchronized (DouglasMenu.getEventLock()){
                     double acc = gps.getAccuracy();  
                     setURL("getStoresFromCoords.php5?"+"lt="+lat+"&ln="+lng);
                
               // }
            
             }
       
         
        
     }  
   public String getURL()
         {
             return URL;
         }
         
         public void setURL(String _url)
         {
             URL = _url;
            
         }
}
