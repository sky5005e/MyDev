/*
 * finder.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;
import res.layout.*;
import  com.douglas.common.*;

import java.io.*;
import com.douglas.main.*;
import com.douglas.utils.*;

import net.rim.device.api.ui.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;
import net.rim.device.api.ui.container.VerticalFieldManager;
import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.*; 
import net.rim.device.api.command.*;
import net.rim.device.api.i18n.*;
 import net.rim.device.api.ui.container.*;
/**
 * 
 */
public class finder extends MainScreen implements FieldChangeListener{
    private ObjectChoiceField _choiceField;
    // Create a ResourceBundle object to contain the localized resources.
    private static ResourceBundle _resources = ResourceBundle.getBundle(BUNDLE_ID, BUNDLE_NAME);
      private WebVerticalFieldManager wvfm = new WebVerticalFieldManager();
    public finder() 
    {
            StoreHeaderLabel HL=new StoreHeaderLabel();
            VerticalFieldManager vfm = HL.StoreHeaderLabel(DouglasConstants.MenuSearch,DouglasConstants.STOREMenu,DouglasConstants.RIGHTMenu, this);
            setBanner(vfm); 
            DouglasFooter douglasFT = new DouglasFooter();
            VerticalFieldManager vfmStatus = new VerticalFieldManager();
            vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
            setStatus(vfmStatus);
            add(wvfm);
            VerticalFieldManager vfmnew=new VerticalFieldManager(VerticalFieldManager.LEAVE_BLANK_SPACE);
            LabelField title=new LabelField("Darf 'Douglas' Ihren aktuellen Ort",LabelField.FIELD_HCENTER);
            LabelField stitle=new LabelField("Verwenden?",LabelField.FIELD_HCENTER);
            ButtonField ok=new ButtonField("OK",ButtonField.FIELD_HCENTER);
       
            LabelField subtitle=new LabelField("order Suche nachi",LabelField.FIELD_HCENTER);
            EditField plz=new EditField("PLZ ","");
          // String strr[] = new String[] {"PLZ:", "Value"};
           //int off[] = new int[] {0, strr[0].length(), strr[0].length() + strr[1].length()};
           
            RichTextField stadt=new RichTextField("Stadt");
            String str[] = new String[] {"Stadt:", "Value"};
            int offf[] = new int[] {0, str[0].length(), str[0].length() + str[1].length()};
           //choice field
            String choices[] = _resources.getStringArray(FIELD_COUNTRIES);
           _choiceField = new ObjectChoiceField(_resources.getString(FIELD_CHOICE), choices);
           _choiceField.setChangeListener(this);
          
           
            vfmnew.add(title);
            vfmnew.add(stitle);
            vfmnew.add(ok);
            vfmnew.add(subtitle);
            vfmnew.add(plz);
            vfmnew.add(stadt);
            vfmnew.add(_choiceField);
     
           add(vfmnew);
           
           
         
 
              
      
    }
     public void fieldChanged(Field field, int context)
             {
                     /* TableLayoutManager _tlm = (TableLayoutManager) field;
                      selectedIndex = _tlm.getID();
                      UiApplication.getUiApplication().pushScreen(new NeuheitenDetailScreen(neuheiten.length - 1));*/
                        
              }
} 
