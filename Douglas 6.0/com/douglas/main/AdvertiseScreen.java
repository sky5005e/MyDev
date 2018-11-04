/*
 * AdvertiseScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;

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
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.system.*;

public class AdvertiseScreen extends Thread{
    MainScreen  _screen;
      public AdvertiseScreen(MainScreen  _screen) 
  {   
   this._screen=_screen;
  
  }
  public void run()
  {
      try{
           Thread.sleep(0);
         }catch(InterruptedException ex)
         {
         }
   
   UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                  UiApplication.getUiApplication().pushScreen(new AdvertisePicture());
                 
            }
        });
  }
  
} 
