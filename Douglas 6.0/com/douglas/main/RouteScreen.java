/*
 * RouteScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;
import res.layout.*;
import com.douglas.common.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.blackberry.api.browser.Browser;
import net.rim.blackberry.api.browser.BrowserSession;
import net.rim.device.api.browser.field2.*;
import java.util.Timer;  
import java.util.TimerTask; 
import net.rim.blackberry.api.invoke.*;
import net.rim.blackberry.api.maps.*;
import net.rim.device.api.lbs.maps.*;
import net.rim.device.api.lbs.maps.model.*;
import net.rim.device.api.lbs.maps.ui.*;

 import net.rim.blackberry.api.invoke.MapsArguments;

/**
 * 
 */
public class RouteScreen extends MainScreen {
    private VerticalFieldManager vfm;
    private BrowserField _browserField;
    public double shoplng;
    public double shoplat;
    public String StreetName;
    private static EncodedImage ZuruckOn = EncodedImage.getEncodedImageResource(DouglasConstants.ZuruckOn);  
    private static EncodedImage ZuruckOFF = EncodedImage.getEncodedImageResource(DouglasConstants.ZuruckOff);
    GPS gps;  
    Timer timer;
    
    public RouteScreen() 
    {    
         gps = new GPS();  
         gps.start();
         timer = new Timer();  
         timer.schedule(new CheckGPS(),6000);
         StoreScreen _ms=new StoreScreen();
         String Latitude=_ms.getLat();
         String Longitude= _ms.getLng();
         StreetName=_ms.getAddress();
         //shop=store[i].getShop();
         shoplng=Double.parseDouble(Longitude);
         shoplat=Double.parseDouble(Latitude);
     
    }
        public boolean onClose()  
     {  
         timer.cancel();  //cleanup  
         this.close();  
         return true;  
     }       
   public class CheckGPS extends TimerTask{  
         long interval;
         
         public CheckGPS() {  
          
          
         }  
   
        public void run() {  
           
             double lat;  
             double lng;  
             int ilat;
             int ilng;
             lat = 0;  
             lng = 0;  
             double slat;  
             double slng; 
             String sname;
             lat = gps.getLatitude();  
             lng = gps.getLongitude();  
             slng=RouteScreen.this.shoplng;
             slat=RouteScreen.this.shoplat;
             ilat=(int)slat;
             ilng=(int)slng;
             sname=RouteScreen.this.StreetName;
             if (lat != 0.0 & lng != 0.0) {  
                 synchronized (DouglasMenu.getEventLock()){
                     double acc = gps.getAccuracy();
                     //RouteScreen rs=new RouteScreen();
                      
           String document = "<lbs><GetRoute>" + "<location x='slng' y='slat' />" +
          "<location x='lng' y='lat'/>" + "</GetRoute></lbs>";                        
          
           Invoke.invokeApplication(Invoke.APP_TYPE_MAPS, new MapsArguments
          ( MapsArguments.ARG_LOCATION_DOCUMENT,document));
            
              
                }
            
             }else
             {
                MapView mapView = new MapView();
                mapView.setLatitude(ilat);
                mapView.setLongitude(ilng);
                mapView.setZoom(10);
                Invoke.invokeApplication(Invoke.APP_TYPE_MAPS, new MapsArguments(mapView));

             }
      
        }
        public boolean cancel()
        {
            return true;
        }
        
     }
 }

