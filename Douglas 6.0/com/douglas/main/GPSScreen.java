/*
 * GPSScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

 package com.douglas.main;
 import com.douglas.common.*;
 import res.layout.*;
 import net.rim.device.api.system.*;
 import net.rim.device.api.ui.*;
 import net.rim.device.api.ui.component.*;
 import net.rim.device.api.ui.container.*;
 import res.layout.*;
 import java.util.Timer;  
 import java.util.TimerTask;  
 import net.rim.device.api.ui.component.RichTextField; 

/**
 * 
 */
public class GPSScreen extends MainScreen {
       GPS gps;  
       Timer timer;  
       RichTextField txtGPS;  
       public static String URL;
       public static String StoredUrl;
       public String gpstxt;
       public PopupScreen popUp;
       public static String StoredURL;
       public int Index; 
       WebVerticalFieldManager _contentOne = new WebVerticalFieldManager();
       WebVerticalFieldManager _contentTwo = new WebVerticalFieldManager();
       Manager _bodyWrapper;
       Manager _currentBody;
       private static EncodedImage LupeOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderLupeImageON);  
       private static EncodedImage LupeOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderLupeImageOFF); 
       private static EncodedImage NeueSucheOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderNeueSucheImageON);  
       private static EncodedImage NeueSucheOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderNeueSucheImageOFF);
    public GPSScreen(int index) { 
       Index=index;
       Manager manageLayout = new HorizontalFieldManager( VERTICAL_SCROLLBAR);
       PopupScreen popUp = new PopupScreen(manageLayout);
       gps = new GPS();  
       gps.start();
       timer = new Timer();  
       timer.schedule(new CheckGPS(),15000);
      
       GPSScreen.CheckGPS gpschk=this.new CheckGPS();
       if(getURL()==null)
       {
       
        Status.show("Warten Sie bitte, während die Standortbestimmung geladen wird. Ihr Endgerät muss hierzu Javascript unterstützen."); 
        timer.cancel();
        UiApplication.getUiApplication().invokeLater(new Runnable() {
            public void run() {
                                  UiApplication.getUiApplication().popScreen(getScreen());
                                       
                               }
                      });
        //UiApplication.getUiApplication().pushScreen(new StoreScreen());
        }
       if(getURL() != null)
        {
        StoreHeaderLabel HL=new StoreHeaderLabel();
        StoredURL="getStoresFromCoords.php5?offset="+Index+""+getURL();
        VerticalFieldManager vfm =HL.StoreHeaderLabel(LupeOn,LupeOFF,DouglasConstants.Suchergebnis,NeueSucheOn,NeueSucheOFF,0,this,StoredURL,0);
        setBanner(vfm); 
        DouglasFooter douglasFT = new DouglasFooter();
        VerticalFieldManager vfmStatus = new VerticalFieldManager();
        vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
        setStatus(vfmStatus);    
           
        Manager foreground = new ForegroundManager();
     //   StoredUrl=getURL();
        
        Bitmap parfumerie= Bitmap.getBitmapResource(DouglasConstants.ParfumerieTab);
        Bitmap parfumerieON= Bitmap.getBitmapResource(DouglasConstants.ParfumerieTabOn);
        Bitmap events= Bitmap.getBitmapResource(DouglasConstants.EventsTab);
        Bitmap eventsON= Bitmap.getBitmapResource(DouglasConstants.EventsTabOn);
        TabButtonSet pills = new TabButtonSet(parfumerie.getWidth(),events.getWidth());
        TabButtonField tabOne = new TabButtonField(parfumerie,parfumerieON,0,0);
        tabOne.setMargin(15,0,0,5);
        TabButtonField tabTwo = new TabButtonField(events,eventsON,0,0);
        tabTwo.setMargin(15,0,0,0);
        
        pills.add( tabOne );
        pills.add( tabTwo );
       
        pills.setMargin( 0, 0, 0, 0 );
        vfm.add(pills);
      
        _bodyWrapper = new NegativeMarginVerticalFieldManager( USE_ALL_WIDTH );
         Thread t = new Thread(new Runnable()
        {
            
                public void run()
                {
                    PerfumerieScreen _tts = new PerfumerieScreen();
                   _tts.PerfumerieScreen(getURL(), 0,Index,1);
                   _tts.PerfumerieScreenLayout(_contentOne,Index,1);
                   _tts = new PerfumerieScreen();
                   _tts.PerfumerieScreen(getURL(), 1,Index,1);
                   _tts.EventsScreenLayout(_contentTwo,Index);
                    
                }
        });
        
         t.start();
         pills.setSelectedField( tabOne );
        _currentBody = _contentOne;
        _bodyWrapper.add( _currentBody );
        
        
        tabOne.setChangeListener( new FieldChangeListener( ) {
            public void fieldChanged( Field field, int context ) {
                if( _currentBody != _contentOne ) {
                        _bodyWrapper.replace( _currentBody, _contentOne );
                        _currentBody = _contentOne;
                }
            }
        } );
        
        tabTwo.setChangeListener( new FieldChangeListener( ) {
            public void fieldChanged( Field field, int context ) {
                if( _currentBody != _contentTwo ) {
                        _bodyWrapper.replace( _currentBody, _contentTwo );
                        _currentBody = _contentTwo;
                }
            }
        } );
        
      
        foreground.add( _bodyWrapper );
        add( foreground );
      
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
  
     public boolean onClose()  
     {  
         timer.cancel();  //cleanup  
         this.close();  
         return true;  
     }  
      public class CheckGPS extends TimerTask{  
     
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
                 synchronized (DouglasMenu.getEventLock()){
                     double acc = gps.getAccuracy();
                     setURL("&lt="+lat+"&ln="+lng);
       
        }
        
     }  
  
} 
}
}
