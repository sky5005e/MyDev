/*
 * TelephoneLabel.java
 *

 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import net.rim.blackberry.api.invoke.*;
import  com.douglas.common.*;
import res.layout.*;
import java.io.*;
import  com.douglas.main.*;
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
import net.rim.device.api.system.*;


/**
 * 
 */
public class TelephoneLabel extends  HorizontalFieldManager implements FieldChangeListener{
   private CustomButtonField telno;
   private String labelEmailText;
   private String text;
   private long style;
   private int[] offsets;
   private byte[] attributes;
   private Font[] fonts;
   private int[] foregroundColors;
   private int[] backgroundColors;
   private Font myFont;
   public HorizontalFieldManager TelephoneLabel(String _text,long _style){
         
       text = _text;
      
       style=_style;
       //Add Footer
      
        HorizontalFieldManager hfmnew=new HorizontalFieldManager();
       
       
      
         telno=new CustomButtonField(text,style);
       //  telno = new LabelField(text,style);
      
      
        telno.setChangeListener(this);
        //telno.setPadding(4,0,0,0);
       hfmnew.add(telno);
      //add(telno);
       return hfmnew;
    
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
  public boolean navigationClick(int status, int time) {
  if ((status & KeypadListener.STATUS_TRACKWHEEL) == KeypadListener.STATUS_TRACKWHEEL) {
      fieldChangeNotify(0);  
            return true;
   //Input came from the trackwheel
   } else if ((status & KeypadListener.STATUS_FOUR_WAY) == KeypadListener.STATUS_FOUR_WAY) {
   //Input came from a four way navigation input device
    fieldChangeNotify(0);  
            return true;
  }
return super.navigationClick(status, time); } 
    protected boolean keyChar(char character, int status, int time) {  
            if (character == Keypad.KEY_ENTER) {  
                FieldChangeListener listener = getChangeListener();
                    if (null != listener)
                          listener.fieldChanged(this, 1);
                          super.setFocus(); 
                return true;  
            }  
            return super.keyChar(character, status, time);  
        }
  public void paint(Graphics g)
    {     g.setFont(DouglasConstants.smfont);
          String hexcolor = DouglasConstants.hexBlack; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
          g.setColor(hexValue);
          super.paint(g);
    }
        public void fieldChanged(Field field, int context)
        {
               if(field==telno)
               {
                 UiApplication.getUiApplication().pushScreen(new RouteScreen());  
               }
        }
    
} 
