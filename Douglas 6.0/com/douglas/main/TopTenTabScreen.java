/*
 * TopTenTabScreen.java
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
public class TopTenTabScreen extends MainScreen 
{
   WebVerticalFieldManager _contentOne = new WebVerticalFieldManager(){
       public int getPreferredHeight()
   {
       return 20;
   }
       };
   WebVerticalFieldManager _contentTwo = new WebVerticalFieldManager();
    private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
    private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);
    
    Manager _bodyWrapper;
    Manager _currentBody;
    
    public TopTenTabScreen() {
       
       HeaderLabel HL=new HeaderLabel();
       VerticalFieldManager vfm = HL.HeaderLabel(DouglasConstants.TopTen,0,menuOn,menuOFF, this);
       setBanner(vfm); 
       DouglasFooter douglasFT = new DouglasFooter();
       VerticalFieldManager vfmStatus = new VerticalFieldManager();
       vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.HomeMenu));
       setStatus(vfmStatus);
        
        Manager foreground = new ForegroundManager();
        
      
        Bitmap damen= Bitmap.getBitmapResource(DouglasConstants.DamenTab);
        Bitmap herren= Bitmap.getBitmapResource(DouglasConstants.HerrenTab);
        Bitmap damenON= Bitmap.getBitmapResource(DouglasConstants.DamenTabOn);
        Bitmap herrenON= Bitmap.getBitmapResource(DouglasConstants.HerrenTabOn);
        TabButtonSet pills = new TabButtonSet(damen.getWidth(),herren.getWidth());
        TabButtonField tabOne = new TabButtonField(damen,damenON,0,0);
        tabOne.setMargin(15,0,0,5);
        TabButtonField tabTwo = new TabButtonField( herren,herrenON,0,0);
        tabTwo.setMargin(15,0,0,0);
       
        pills.add( tabOne );
        pills.add( tabTwo );
       
        pills.setMargin(0,0,0,0);
        vfm.add(pills);
       
       
        
        
        _bodyWrapper = new NegativeMarginVerticalFieldManager( USE_ALL_WIDTH );
         Thread t = new Thread(new Runnable()
        {
            
                public void run()
                {
                    TopTenScreen _tts = new TopTenScreen();
                    _tts.TopTenScreen(DouglasConstants.TopTenDamenURL, 0);
                    _tts.TopTenScreenLayout(_contentOne);
                    _tts = new TopTenScreen();
                    _tts.TopTenScreen(DouglasConstants.TopTenHerrenURL, 1);
                    _tts.TopTenScreenLayout(_contentTwo);
                    
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

