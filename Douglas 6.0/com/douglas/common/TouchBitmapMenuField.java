/*
 * TouchBitmapMenuField.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.common;

import com.douglas.main.*;
import net.rim.device.api.ui.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;

import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.Bitmap; 
import net.rim.device.api.system.*;


/**
 * 
 */
public class TouchBitmapMenuField extends Field {
    
    
    private Bitmap image;
    private Bitmap selectedImage;
    private int n;
    private long style;
    public TouchBitmapMenuField(Bitmap image, Bitmap selectedImage,int n,long style) 
    {
           this.image = image;
           this.selectedImage = selectedImage;
           this.n=n;
           this.style=style;
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
                fieldChangeNotify(0);  
                return true;  
            }  
            return super.keyChar(character, status, time);  
        } 
       
     
      protected void layout(int maxWidth, int maxHeight) {  
        setExtent(image.getWidth(), image.getHeight());
    }

     protected void paint(Graphics graphics) {
        
        // Draw img             
        if (isFocus()) {
            
           graphics.drawBitmap(0, 0, image.getWidth(), image.getHeight(), selectedImage, 0, 0);
        } else {
                graphics.drawBitmap(0, 0, image.getWidth(),image.getHeight(), image, 0, 0);
        }
    }

   protected void drawFocus(Graphics g, boolean on) {
      
        XYRect focusRect = new XYRect();
         getFocusRect( focusRect );
         int yOffset = 0;
         if ( isSelecting() )
        {

           yOffset = focusRect.height >> 1;
           focusRect.height = yOffset;
           focusRect.y += yOffset;
        }
       g.pushRegion( focusRect.x, focusRect.y, focusRect.width-200, focusRect.height-200, -focusRect.x, -focusRect.y );
      
       String hexcolor = "828282"; 
       int hexValue = Integer.parseInt(hexcolor, 16); 
       g.setBackgroundColor(hexValue);
       g.setColor(hexValue);
       g.clear();
       this.paint( g );
       g.popContext();
       } 
      protected void onFocus(int direction) { 
      
        super.onFocus(direction);  
        invalidate();  
    }  
      
    protected void onUnfocus() {  
   
        super.onUnfocus();  
        invalidate();  
    } 
    
} 


