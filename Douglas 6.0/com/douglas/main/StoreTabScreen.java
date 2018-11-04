/*
 * MaternusTabScreen.java
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
public class StoreTabScreen extends MainScreen 
{
   
   WebVerticalFieldManager _contentOne = new WebVerticalFieldManager();
   WebVerticalFieldManager _contentTwo = new WebVerticalFieldManager();
   WebVerticalFieldManager _contentThree = new WebVerticalFieldManager();
   private static EncodedImage SuchMenuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchBackImageON);  
   private static EncodedImage SuchMenuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchBackImageOFF);
   
   private int flag;
   public static int CNT;
   public int _screenno;
   private int screenInfo;
   private Event[] event;
   Manager _bodyWrapper;
   Manager _currentBody;
   public VerticalFieldManager vfm;
   public StoreTabScreen(int _flag,int _screenno,int _screenInfo) {
        flag = _flag;
        _screenno= _screenno;
        screenInfo=_screenInfo;
        HeaderLabel HL=new HeaderLabel();
      
        vfm=flag==0?HL.HeaderLabel(PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop().getStreetName()+DouglasConstants.Space+PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop().getStreetNumber(),0,SuchMenuOn,SuchMenuOFF, this):HL.HeaderLabel(EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop().getStreetName()+DouglasConstants.Space+EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop().getStreetNumber(),0,SuchMenuOn,SuchMenuOFF, this);
        setBanner(vfm); 
        DouglasFooter douglasFT = new DouglasFooter();
        VerticalFieldManager vfmStatus = new VerticalFieldManager();
        vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
        setStatus(vfmStatus);
        
        Manager foreground = new ForegroundManager();
        
         event=flag==1?EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getEvents():PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getEvents();
         int j=event.length;
         int cnt=0;
         for(int i=0;i<event.length;i++)
         {
            Integer type=Integer.valueOf(event[i].getType());
           
           if(type.intValue()==3 || type.intValue()==1)
           {
              cnt++; 
            }
         }   
        this.setCNT(cnt); 
       
         Bitmap info= Bitmap.getBitmapResource(DouglasConstants.InfoTab);
         Bitmap infoON= Bitmap.getBitmapResource(DouglasConstants.InfoTabOn);
         Bitmap events= Bitmap.getBitmapResource(DouglasConstants.EventsTab);
         Bitmap eventsON= Bitmap.getBitmapResource(DouglasConstants.EventsTabOn);
         Bitmap event= Bitmap.getBitmapResource(DouglasConstants.EventTab);
         Bitmap eventON= Bitmap.getBitmapResource(DouglasConstants.EventTabOn);
         Bitmap beautyServices= Bitmap.getBitmapResource(DouglasConstants.BeautyServicesTab);
         Bitmap beautyServicesON= Bitmap.getBitmapResource(DouglasConstants.BeautyServicesTabOn);
         TabButtonSet pills = new TabButtonSet(info.getWidth(),events.getWidth());
         TabButtonField tabOne = new TabButtonField(info,infoON,0,0);
         tabOne.setMargin(15,0,0,5);
         pills.add( tabOne );
         TabButtonField tabTwo=(this.getCNT()>1)?new TabButtonField(events,eventsON,this.getCNT(),1):new TabButtonField(event,eventON,this.getCNT(),1); 
         tabTwo.setMargin(15,0,0,0);
         
         pills.add( tabTwo );
        
        

         TabButtonField tabThree=new TabButtonField(beautyServices,beautyServicesON,0,0);
         tabThree.setMargin(13,0,0,0);
       
        pills.add( tabThree );
        pills.setMargin( 0, 0, 0, 0 );
        vfm.add(pills);
       
      
        _bodyWrapper = new NegativeMarginVerticalFieldManager( USE_ALL_WIDTH );
         Thread t = new Thread(new Runnable()
        {
            
                public void run()
                {
                   StoreScreen _tts = new StoreScreen();
                   if(flag==0)
                   {
                    
                    _tts.StoreScreen(0,0);
                    _tts.MaternusScreenLayout(_contentOne);
                    _tts = new StoreScreen();
                     int k = screenInfo>0?screenInfo:0;
                    _tts.StoreScreen(k,0);
                    _tts.EventTypeScreenLayout(_contentTwo);
                    _tts=new StoreScreen();
                    _tts.StoreScreen(0,0);
                    _tts.BeautyScreenLayout(_contentThree);
                  }else
                  {
                    _tts.StoreScreen(1,1);
                    _tts.MaternusScreenLayout(_contentOne);
                    _tts = new StoreScreen();
                     int k = screenInfo>0?screenInfo:0;
                    _tts.StoreScreen(k,1);
                    _tts.EventTypeScreenLayout(_contentTwo);
                    _tts=new StoreScreen();
                    _tts.StoreScreen(1,1);
                    _tts.BeautyScreenLayout(_contentThree);
                    
                  }
                    
                }
        });
        
        t.start();
         if(_screenno==1)
         {
             pills.setSelectedField( tabTwo );
             _currentBody = _contentTwo;
             _bodyWrapper.add( _currentBody );
         }else
         {
         pills.setSelectedField( tabOne );
        _currentBody = _contentOne;
        _bodyWrapper.add( _currentBody );
         }
        
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
        
       tabThree.setChangeListener( new FieldChangeListener(){
       public void fieldChanged(Field field, int context){
    
    if(_currentBody !=_contentThree){
      _bodyWrapper.replace(_currentBody,_contentThree);
        _currentBody=_contentThree;
      }
      }
      });
        foreground.add( _bodyWrapper );
        add( foreground );
    }
    public int getCNT()
       {
           return CNT;
       }
       public void setCNT(int _cnt)
       {
           CNT=_cnt;
        }

}
