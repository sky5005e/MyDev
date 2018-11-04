/*
 * SkinTestScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;


import net.rim.device.api.ui.container.*;
import net.rim.device.api.browser.field2.*;
import net.rim.device.api.ui.*;
import javax.microedition.io.*;
import com.douglas.common.*;
import res.layout.*;

import net.rim.blackberry.api.browser.Browser;
import net.rim.blackberry.api.browser.BrowserSession;
import net.rim.device.api.system.*;

public class SkinTestScreen extends MainScreen
{    
        private BrowserField _browserField;
        private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
        private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);    
        public SkinTestScreen()  
        {               
               
                HeaderLabel HL=new HeaderLabel();
                VerticalFieldManager vfm = HL.HeaderLabel(DouglasConstants.Hauttyptest,0,menuOn,menuOFF,this);
                setBanner(vfm);
                
                DouglasFooter douglasFT = new DouglasFooter();
                VerticalFieldManager vfmStatus = new VerticalFieldManager();
                vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.HomeMenu));
                setStatus(vfmStatus);
        
                BrowserFieldConfig fieldConfig = new BrowserFieldConfig();
                fieldConfig.setProperty(BrowserFieldConfig.NAVIGATION_MODE, BrowserFieldConfig.NAVIGATION_MODE_POINTER);
                _browserField = new BrowserField(fieldConfig);
                add(_browserField);
                _browserField.requestContent("http://m.douglas.de/hauttyptestiphone");
                
        }

     
} 
