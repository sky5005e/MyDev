/*
 * LabelListener.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.common;

import res.layout.*;
import java.io.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.container.*;

import java.util.*;
import net.rim.device.api.browser.field2.*;
import net.rim.blackberry.api.browser.Browser;
import net.rim.blackberry.api.browser.BrowserSession;
import net.rim.device.api.ui.component.ActiveRichTextField;
import net.rim.blackberry.api.invoke.*;


/**
 * 
 */
public class LabelListener implements FieldChangeListener{
  private String telno;
    public LabelListener(String telno)
    {
        this.telno=telno;
    }
    public void fieldChanged(Field field, int context)
              {
                 String phoneNum = telno; // _phonefield is an EditField
                     Invoke.invokeApplication(Invoke.APP_TYPE_PHONE,
                      new PhoneArguments(PhoneArguments.ARG_CALL, phoneNum));     
              }
} 


