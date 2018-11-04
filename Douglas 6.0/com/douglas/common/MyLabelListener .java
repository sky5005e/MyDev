/*
 * MyLabelListener .java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.common;
import java.io.*;

import net.rim.device.api.ui.*;
import com.douglas.main.*;

/**
 * 
 */
 public class MyLabelListener implements FieldChangeListener {
       
         public void fieldChanged(Field field, int context) {
          UiApplication.getUiApplication().pushScreen(new RouteScreen());
          }
   }
