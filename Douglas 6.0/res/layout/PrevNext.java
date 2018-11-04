/*
 * PrevNext.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */


package res.layout;
import com.douglas.common.*;
import com.douglas.main.*;
import com.douglas.utils.*;

import net.rim.device.api.ui.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;

import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.*; 

public class PrevNext extends HorizontalFieldManager implements FieldChangeListener{
     
     private BitmapField prev;
     private BitmapField next;
     private int currentIndex;
     private int maxIndex;
     private int screenNumber;
     private int n;
     public HorizontalFieldManager PrevNext(int _screenNumber, int _currentIndex, int _maxIndex,int _n){
         
       screenNumber = _screenNumber;
       currentIndex = _currentIndex;
       maxIndex = _maxIndex;
       n = _n;   
       //Add Footer
       HorizontalFieldManager hfmnew=new HorizontalFieldManager();
      
        EncodedImage ei;
        if (currentIndex > 0)
        {
            ei = EncodedImage.getEncodedImageResource(DouglasConstants.prevImage);
           // ei.setScale(2);
            
            prev = new TouchBitmapField(ei.getBitmap(),BitmapField.FOCUSABLE){
                public boolean isFocusable()
                 {         
                   return true;  
          
                  }  
                };   
            prev.setChangeListener(this);
            prev.setMargin(15,0,5,Display.getWidth()/2 -20- ei.getWidth()/2 );
            hfmnew.add(prev);
        }
        
        if (currentIndex < maxIndex)
        {
            ei = EncodedImage.getEncodedImageResource(DouglasConstants.nextImage);
           // ei.setScale(2);
            
            next = new TouchBitmapField(ei.getBitmap(),BitmapField.FOCUSABLE){
                
            public boolean isFocusable()
                 {         
                   return true;  
          
                  }};   
            next.setChangeListener(this);
            next.setMargin(15,0,5,(currentIndex == 0 ? Display.getWidth()/2-20 - ei.getWidth()/2+50: 20));
            hfmnew.add(next);
        }
        
        
        return hfmnew;
         
   }

   public void fieldChanged(Field field, int context)
   {
        if(field == prev)
        {
            //Switch Case to select
            switch (screenNumber) {
                     case DouglasConstants.NeuheitenScreenNumber:  
                        NeuheitenScreen.selectedIndex = NeuheitenScreen.selectedIndex - 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                  UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                  UiApplication.getUiApplication().pushScreen(new NeuheitenDetailScreen(maxIndex));
                               }
                        });
                        break;
                    case DouglasConstants.TopTenScreenNumber:  
                        TopTenScreen.selectedIndex = TopTenScreen.selectedIndex - 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                   UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                   UiApplication.getUiApplication().pushScreen(new TopTenDetailScreen());
                               }
                        });
                        break; 
                        case DouglasConstants.MaternusScreenNumber:  
                        StoreScreen.selectedIndex = StoreScreen.selectedIndex - 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                   UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                   UiApplication.getUiApplication().pushScreen(new StoreTabScreen(0,1,StoreScreen.selectedIndex));
                               }
                        });
                        break;   
                      case DouglasConstants.MaternusEventScreenNumber:  
                        StoreScreen.selectedIndex = StoreScreen.selectedIndex - 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                   UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                   UiApplication.getUiApplication().pushScreen(new StoreTabScreen(1,1,StoreScreen.selectedIndex));
                               }
                        });
                        break;   
                        case DouglasConstants.PerfumerieScreenNumber:  
                        if(n==0)
                        {
                        PerfumerieScreen.selectedIndex =currentIndex-10;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                  UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                  UiApplication.getUiApplication().pushScreen(new PerfumerieTabScreen(PerfumerieScreen.selectedIndex));
                               }
                        });
                       }else
                       {
                          PerfumerieScreen.selectedIndex =currentIndex-10;
                         UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                  UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                  UiApplication.getUiApplication().pushScreen(new GPSScreen(PerfumerieScreen.selectedIndex));
                               }
                        }); 
                       }
                        break;
                    default: 
                       break;
             }
        }
        else if(field == next)
        {
            //Switch Case to select
            switch (screenNumber) {
                    case DouglasConstants.NeuheitenScreenNumber:  
                        NeuheitenScreen.selectedIndex = NeuheitenScreen.selectedIndex + 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                  UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                  UiApplication.getUiApplication().pushScreen(new NeuheitenDetailScreen(maxIndex));
                               }
                        });
                        break;
                     case DouglasConstants.TopTenScreenNumber:  
                        TopTenScreen.selectedIndex = TopTenScreen.selectedIndex + 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                   UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                   UiApplication.getUiApplication().pushScreen(new TopTenDetailScreen());
                               }
                        });
                        break; 
                        case DouglasConstants.MaternusScreenNumber:  
                        StoreScreen.selectedIndex = StoreScreen.selectedIndex + 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                   UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                   UiApplication.getUiApplication().pushScreen(new StoreTabScreen(0,1,StoreScreen.selectedIndex));
                               }
                        });
                        break;   
                       case DouglasConstants.MaternusEventScreenNumber:  
                        StoreScreen.selectedIndex = StoreScreen.selectedIndex + 1;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                   UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                   UiApplication.getUiApplication().pushScreen(new StoreTabScreen(1,1,StoreScreen.selectedIndex));
                               }
                        });
                        break;
                         case DouglasConstants.PerfumerieScreenNumber:  
                        if(n==0)
                        {
                        PerfumerieScreen.selectedIndex =currentIndex+10;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                  UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                  UiApplication.getUiApplication().pushScreen(new PerfumerieTabScreen(PerfumerieScreen.selectedIndex));
                               }
                        });
                       }else
                       {
                           PerfumerieScreen.selectedIndex =currentIndex+10;
                        UiApplication.getUiApplication().invokeLater(new Runnable()
                        {
                               public void run()
                               {
                                  UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
                                  UiApplication.getUiApplication().pushScreen(new GPSScreen(PerfumerieScreen.selectedIndex));
                               }
                        });
                       }
                        break;
                    default: 
                        break;
            }
        }
   }
}


