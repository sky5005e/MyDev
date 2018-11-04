/*
 * DouglasMenu.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;
import  com.douglas.common.*;

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
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;
import java.util.Timer;  
 import java.util.TimerTask; 

public class DouglasMenu extends UiApplication{
   public GPS gps;
    public static void main(String[] args)
    {
        DouglasMenu theApp = new DouglasMenu();
        theApp.enterEventDispatcher();
    }


        public DouglasMenu() {

             gps = new GPS();  
             gps.start();
           
            pushScreen(new startApp());
         

     }
     public void close()
    {
        // Display a farewell message before closing the application
       // Dialog.alert("Goodbye!");     
        System.exit(0);
    }  
}

