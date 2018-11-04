/*
 * WebDataCallback.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.common;
import net.rim.device.api.ui.container.*;




/**
 * 
 */
public interface WebDataCallback   
{   
    public void callback(String data);   
    public void callback(VerticalFieldManager vfm); 
}  
