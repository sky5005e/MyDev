/*
 * UIExamplePillButtonScreen.java
 *
 * Research In Motion Limited proprietary and confidential
 * Copyright Research In Motion Limited, 2009-2009
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
 * 
 */
public class VideoTabScreen extends MainScreen 
{
   WebVerticalFieldManager _contentOne = new WebVerticalFieldManager();
   WebVerticalFieldManager _contentTwo = new WebVerticalFieldManager();
    private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
    private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);
    
    Manager _bodyWrapper;
    Manager _currentBody;
    
    public VideoTabScreen() {
      
       HeaderLabel HT=new HeaderLabel();
       VerticalFieldManager vfm = HT.HeaderLabel(DouglasConstants.Videos,0,menuOn,menuOFF, this);
      
       setBanner(vfm); 
       
       DouglasFooter douglasFT = new DouglasFooter();
       VerticalFieldManager vfmStatus = new VerticalFieldManager();
       vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.VIDEOMenu));
       setStatus(vfmStatus);
       
       
        Manager foreground = new ForegroundManager();
        
        
         Bitmap makeup= Bitmap.getBitmapResource(DouglasConstants.MakeupTab);
         Bitmap tvspot= Bitmap.getBitmapResource(DouglasConstants.TVSpotTab);
         Bitmap makeupON= Bitmap.getBitmapResource(DouglasConstants.MakeupTabOn);
         Bitmap tvspotON= Bitmap.getBitmapResource(DouglasConstants.TVSpotTabOn);
         TabButtonSet pills = new TabButtonSet(makeup.getWidth(),tvspot.getWidth());
        //
         TabButtonField tabOne = new TabButtonField(makeup,makeupON,0,0);
         tabOne.setMargin(15,0,0,5);
         TabButtonField tabTwo = new TabButtonField( tvspot,tvspotON,0,0);
         tabTwo.setMargin(15,0,0,0);
       
        pills.add( tabOne );
        pills.add( tabTwo );
       
        pills.setMargin( 0, 0, 0, 0 );
        vfm.add(pills);
        setBanner(vfm);
       
        
        
        _bodyWrapper = new NegativeMarginVerticalFieldManager( USE_ALL_WIDTH );
         Thread t = new Thread(new Runnable()
        {
           
                public void run()
                {
                    VideoScreen _vs = new VideoScreen();
                    _vs.VideoScreen(DouglasConstants.MakeupURL ,0);
                    _vs.VideoScreenLayout(_contentOne);
                    _vs = new VideoScreen();
                    _vs.VideoScreen(DouglasConstants.TVSpotURL ,1);
                    _vs.VideoScreenLayout(_contentTwo);
                    
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

