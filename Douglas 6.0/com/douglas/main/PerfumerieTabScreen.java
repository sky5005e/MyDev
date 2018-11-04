/*
 * PerfumerieTabScreen.java
 *
 * One-Associates Technologies Pvt Ltd.
 * info@one-associates.com
 */

package com.douglas.main;
import res.layout.*;

import com.douglas.main.*;

import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.*;
import com.douglas.common.*;


/**
 * Create Tab Screen
 */
public class PerfumerieTabScreen extends MainScreen 
{
   WebVerticalFieldManager _contentOne = new WebVerticalFieldManager();
   WebVerticalFieldManager _contentTwo = new WebVerticalFieldManager();
   private static EncodedImage LupeOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderLupeImageON);  
   public static String StoredURL;
   private static EncodedImage LupeOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderLupeImageOFF); 
   private static EncodedImage NeueSucheOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderNeueSucheImageON);  
   private static EncodedImage NeueSucheOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderNeueSucheImageOFF);
   public int Index; 
    Manager _bodyWrapper;
    Manager _currentBody;
    
    public PerfumerieTabScreen(int index) {
       Index=index; 
       StoreHeaderLabel HL=new StoreHeaderLabel();
       String StoredURL="getStoresFromCoords.php5?offset="+index+""+finder.StoredUrl;
       VerticalFieldManager vfm =HL.StoreHeaderLabel(LupeOn,LupeOFF,DouglasConstants.Suchergebnis,NeueSucheOn,NeueSucheOFF,0,this,StoredURL,0);
       setBanner(vfm); 
       
       DouglasFooter douglasFT = new DouglasFooter();
       VerticalFieldManager vfmStatus = new VerticalFieldManager();
       vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
       setStatus(vfmStatus);
        
        Manager foreground = new ForegroundManager();
        
       // TabButtonSet pills = new TabButtonSet();
        
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
       
        //foreground.add( pills );
        
        
        _bodyWrapper = new NegativeMarginVerticalFieldManager( USE_ALL_WIDTH );
         Thread t = new Thread(new Runnable()
        {
            
                public void run()
                {
                    
                   PerfumerieScreen _tts = new PerfumerieScreen();
                   _tts.PerfumerieScreen(finder.StoredUrl,0,Index,0);
                   _tts.PerfumerieScreenLayout(_contentOne,Index,0);
                   _tts = new PerfumerieScreen();
                   _tts.PerfumerieScreen(finder.StoredUrl,1,Index,0);
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

