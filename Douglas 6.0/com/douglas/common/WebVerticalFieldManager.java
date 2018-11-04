/*
 * WebVerticalFieldManager.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.common;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.*;


/**
 * 
 */
public class WebVerticalFieldManager extends VerticalFieldManager implements WebDataCallback {
    private int width;
    private int height;
    
    public WebVerticalFieldManager() {    
    
        
    }
 /*  public int getPreferredHeight()
   {
       return 200;
   }*/
    public void callback(final VerticalFieldManager _vfm)   
    {   
        
       try  
       {   
         add(_vfm);
       }   
       catch (final Exception e){}   
    }  
    
    public void callback(String data)   
    {   
        
    }  
    
   
} 
