/*
 * ActiveRichTextField.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import net.rim.blackberry.api.invoke.*;
import  com.douglas.common.*;
import  com.douglas.main.*;

import res.layout.*;
import java.io.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.SeparatorField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.component.TextField;
import net.rim.device.api.ui.decor.Background;
import net.rim.device.api.ui.decor.BackgroundFactory;
import net.rim.device.api.ui.Graphics;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.Font;
import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.Bitmap; 
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.util.MathUtilities;
import java.util.*;
import net.rim.device.api.browser.field2.*;



/**
 * 
 */
public class ARichTextField extends  HorizontalFieldManager implements FieldChangeListener{
   private ActiveRichTextField telno;
   private ActiveRichTextField route;
   private String labelEmailText;
   private String text;
   private long style;
   private int[] offsets;
   private byte[] attributes;
   private Font[] fonts;
   private int[] foregroundColors;
   private int[] backgroundColors;
   private Font myFont;
   private int n;
   public HorizontalFieldManager ARichTextField(String _text, int[] _offsets, byte[] _attributes, Font[] _fonts, int[] _foregroundColors, int[] _backgroundColors, long _style,int _n){
         
       text = _text;
       offsets= _offsets;
       attributes= _attributes;
       fonts=_fonts;
       foregroundColors=_foregroundColors;
       backgroundColors=_backgroundColors;
       style=_style;
       n=_n;
       //Add Footer
       
       int defaultTextHeight = Math.min(Display.getHeight() / 15, 15);
       try
       {
         Font myFont = FontFamily.forName("BBAlpha Sans").getFont(Font.PLAIN, defaultTextHeight);
       }catch(Exception e){} 
       
        HorizontalFieldManager hfmnew=new HorizontalFieldManager();
     
      
      
      
        ActiveRichTextField telno = new ActiveRichTextField(text,offsets, attributes, fonts, null, null, ActiveRichTextField.FOCUSABLE | (long)ActiveRichTextField.USE_TEXT_WIDTH );
        telno.setChangeListener(this);
        telno.setMargin(0,3,0,0);
        telno.setFont(DouglasConstants.vsmfont);
        hfmnew.add(telno);
     
    // hfmnew.setMargin(4,0,0,0);
       return hfmnew;
    
}
    public boolean isFocusable()
     {         
        return true;  
          
     }  
    
 protected boolean touchEvent(TouchEvent message) 
    {
      if (TouchEvent.CLICK == message.getEvent()) 
         {
                  FieldChangeListener listener = getChangeListener();
                    if (null != listener)
                          listener.fieldChanged(this, 1);
                          super.setFocus();
         }
          
      return super.touchEvent(message);
     }
 /*protected boolean navigationClick(int status, int time) {  
            fieldChangeNotify(0);  
            return true;  
        } 
 */
   public boolean navigationClick(int status, int time) 
   {
     if ((status & KeypadListener.STATUS_TRACKWHEEL) == KeypadListener.STATUS_TRACKWHEEL) 
     {
        fieldChangeNotify(1);  
        return true;
        //Input came from the trackwheel
     } else if ((status & KeypadListener.STATUS_FOUR_WAY) == KeypadListener.STATUS_FOUR_WAY) 
     {
        //Input came from a four way navigation input device
        fieldChangeNotify(1);  
        return true;
     }
    return super.navigationClick(status, time); 
  } 
 protected boolean navigationMovement(int dx, int dy, int status, int time)
        {
                int focusIndex = getFieldWithFocusIndex();
                int dirY = (dy > 0) ? 1 : -1;
                int absY = Math.abs(dy);

                for (int y = 0; y < absY; y++)
                {
                        focusIndex += 1 * dirY;
                        if (focusIndex < 0 || focusIndex >= getFieldCount())
                        {
                                return false;
                        }
                        else
                        {
                                Field f = getField(focusIndex);
                                if (f.isFocusable())
                                {
                                        f.setFocus();
                                }
                                else
                                        y--; // do it over again
                        }
                }

                int dirX = (dx > 0) ? 1 : -1;
                int absX = Math.abs(dx);
                for (int x = 0; x < absX; x++)
                {
                        focusIndex += dirX;
                        if (focusIndex < 0 || focusIndex >= getFieldCount())
                        {
                                return false;
                        }
                        else
                        {
                                Field f = getField(focusIndex);
                                if (f.isFocusable())
                                {
                                        f.setFocus();
                                }
                                else
                                        x--; // do it over again
                        }
                }
                return true;
        }
  protected boolean keyChar(char character, int status, int time) {  
            if (character == Keypad.KEY_ENTER) {  
                fieldChangeNotify(0);  
                return true;  
            }  
            return super.keyChar(character, status, time);  
        }
         protected void onFocus(int direction) { 
      
        super.onFocus(direction);  
        invalidate();  
       }  
      
       protected void onUnfocus() {  
   
        super.onUnfocus();  
        invalidate();  
      }  
        public void fieldChanged(Field field, int context)
        {
               if((field==telno && context==0) | (field==telno && context==1))
               {
                 String phoneNum = text; // _phonefield is an EditField
                 Invoke.invokeApplication(Invoke.APP_TYPE_PHONE,
                 new PhoneArguments(PhoneArguments.ARG_CALL, phoneNum));     
               }
              
        }
      
    
} 
