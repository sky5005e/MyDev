/*
 * ZumShop.java
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

public class ZumShop extends MainScreen
{    
        private int flag;
        private BrowserField _browserField;
        private static EncodedImage NeuMenuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderNeuImageON);  
        private static EncodedImage NeuMenuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderNeuImageOFF);     
        private static EncodedImage TopMenuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderTopBackImageON);  
        private static EncodedImage TopMenuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderTopBackImageOFF);     
        public ZumShop(int _flag)  
        {   
           
           
         flag=_flag;     
            
                 if(flag==0)
                 {
                  Browser.getDefaultSession().displayPage(TopTenDetailScreen.URL);     
                  }else
                 {
                  Browser.getDefaultSession().displayPage(NeuheitenDetailScreen.URL); 
                 }
        }

     
} 
