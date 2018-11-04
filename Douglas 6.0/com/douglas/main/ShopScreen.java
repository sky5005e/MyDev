/*
 * ShopScreen.java
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
import net.rim.device.api.system.*;

import net.rim.blackberry.api.browser.Browser;
import net.rim.blackberry.api.browser.BrowserSession;

public class ShopScreen extends MainScreen
{    
        private BrowserField _browserField;
        private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
        private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);         
        public ShopScreen()  
        {       
        
                Browser.getDefaultSession().displayPage("http://mshop.douglas.de/douglas/"); 
       }

     
} 


