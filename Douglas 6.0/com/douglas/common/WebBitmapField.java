package com.douglas.common;
import com.douglas.utils.*;
import net.rim.device.api.ui.component.BitmapField;
import java.io.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.SeparatorField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.component.TextField;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.Bitmap; 
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.math.Fixed32;


public class WebBitmapField extends BitmapField implements WebDataCallback   
{   
    private EncodedImage bitmap = null;
    private int scale;   
   
    public WebBitmapField(String _url, int _scale)   
    {   
      
        scale = _scale;
        Util util=new Util();
        try  
        {   
           util.getWebData(_url, this);   
        }   
        catch (Exception e) {}   
           
   }   
  
   public Bitmap getBitmap()   
   {   
      if (bitmap == null)
       return null;   
       return bitmap.getBitmap();   
   }   
 
   protected void drawFocus(Graphics g, boolean on) {
        //Do nothing. Ignore the focus color.  
   } 
 
   public boolean isFocusable()
   {         
        return true;   
   }  
     
   public void callback(final String data)   
    {   
       if(data==null)
       {
         EncodedImage SERVICE_LOGO = EncodedImage.getEncodedImageResource(DouglasConstants.DouglasLogo);
       //  BitmapField img = new BitmapField(SERVICE_LOGO.,BitmapField.FIELD_RIGHT); 
         setImage(SERVICE_LOGO); 
        }
       
        if (data.startsWith("Exception")) return;   
      
       try  
       {   
            byte[] dataArray = data.getBytes();   
            bitmap = EncodedImage.createEncodedImage(dataArray, 0,   
                   dataArray.length);   
           
             bitmap.setScale(scale);
             setImage(bitmap);
            
       }   
       catch (final Exception e){}   
    }   
    
     public void callback(VerticalFieldManager vfm)   
    { 
    }
}  
