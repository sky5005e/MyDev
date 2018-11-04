/*
 * EventsTabScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
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
public class EventsTabScreen extends MainScreen 
{
   public static int Index;
   WebVerticalFieldManager _contentOne = new WebVerticalFieldManager();
   WebVerticalFieldManager _contentTwo = new WebVerticalFieldManager();
    private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchBackImageON);  
    private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchBackImageOFF);
    
    Manager _bodyWrapper;
    Manager _currentBody;
    
    public EventsTabScreen() {
       
        
       HeaderLabel HL=new HeaderLabel();
       VerticalFieldManager vfm = HL.HeaderLabel(DouglasConstants.Events,0,menuOn,menuOFF,this);
       setBanner(vfm); 
       DouglasFooter douglasFT = new DouglasFooter();
       VerticalFieldManager vfmStatus = new VerticalFieldManager();
       vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
       setStatus(vfmStatus);
        
       Manager foreground = new ForegroundManager();
        
       
       Bitmap info= Bitmap.getBitmapResource(DouglasConstants.InfoTab);
       Bitmap parmit= Bitmap.getBitmapResource(DouglasConstants.ParMitTab);
       Bitmap infoON= Bitmap.getBitmapResource(DouglasConstants.InfoTabOn);
       Bitmap parmitON= Bitmap.getBitmapResource(DouglasConstants.ParMitTabOn);
       TabButtonSet pills = new TabButtonSet(info.getWidth(),parmit.getWidth());
       TabButtonField tabOne = new TabButtonField(info,infoON,0,0);
       tabOne.setMargin(15,0,0,5);
       TabButtonField tabTwo = new TabButtonField( parmit,parmitON,0,0);
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
                   
                   
                     EventsScreenDetail _tts = new EventsScreenDetail();
                    
                    _tts.EventsScreenLayoutDetail(_contentOne);
                    _tts = new EventsScreenDetail();
                    _tts.EventsScreenDetail(finder.StoredUrl, 1);
                    _tts.DiesemEventsLayout(_contentTwo);
                   
                    
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


