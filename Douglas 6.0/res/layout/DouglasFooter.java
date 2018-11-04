/*
 * DouglasFooter.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */


package res.layout;
import com.douglas.common.*;
import com.douglas.main.*;
import com.douglas.utils.*;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;

import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.*; 

public class DouglasFooter extends HorizontalFieldManager implements FieldChangeListener{
     private MainScreen _screen;
     private BitmapField menu;
     private BitmapField parfumerien;
     private BitmapField neu;
     private BitmapField video;
     private BitmapField angebote;
       
     public HorizontalFieldManager DouglasFooter(String str){
         
          
       //Add Footer
     HorizontalFieldManager hfmnew=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH )
     {
       public void paint(Graphics g)
       {
           String hex="1C1C1C";
           int intValue = Integer.parseInt(hex, 16);
           g.setBackgroundColor(intValue);
           g.setColor(intValue);
           g.clear();
           super.paint(g);
        }
      } ;
      
        EncodedImage ei;
        ei = (DouglasConstants.HOMEMenu == str) ? EncodedImage.getEncodedImageResource(DouglasConstants.FooterHOMEMenuImageON): EncodedImage.getEncodedImageResource(DouglasConstants.FooterHOMEMenuImageOFF);
        //ei.setScale(2);
        
        menu=new TouchBitmapField(ei.getBitmap(),BitmapField.HIGHLIGHT_FOCUS);   
        menu.setChangeListener(this);
        menu.setMargin(5,0,5,(Display.getWidth() -5*ei.getWidth())/5-15);
        hfmnew.add(menu);
        
        
        ei = (DouglasConstants.STOREMenu == str) ? EncodedImage.getEncodedImageResource(DouglasConstants.FooterSTOREMenuImageON): EncodedImage.getEncodedImageResource(DouglasConstants.FooterSTOREMenuImageOFF);
        //ei.setScale(2);
        
        parfumerien=new TouchBitmapField(ei.getBitmap(),BitmapField.HIGHLIGHT_FOCUS);   
        parfumerien.setChangeListener(this);
        parfumerien.setMargin(5,0,5,(Display.getWidth() - 5*ei.getWidth())/5);
        hfmnew.add(parfumerien);
        
        ei = (DouglasConstants.ANGEBOTEMenu == str) ? EncodedImage.getEncodedImageResource(DouglasConstants.FooterANGEBOTEMenuImageON): EncodedImage.getEncodedImageResource(DouglasConstants.FooterANGEBOTEMenuImageOFF);
        //ei.setScale(2);
        
        angebote=new TouchBitmapField(ei.getBitmap(),BitmapField.HIGHLIGHT_FOCUS);   
        angebote.setChangeListener(this);
        angebote.setMargin(5,0,5,(Display.getWidth() - 5*ei.getWidth())/5);
        hfmnew.add(angebote);
        
        
        ei = (DouglasConstants.VIDEOMenu == str) ? EncodedImage.getEncodedImageResource(DouglasConstants.FooterVIDEOMenuImageON): EncodedImage.getEncodedImageResource(DouglasConstants.FooterVIDEOMenuImageOFF);
        //ei.setScale(2);
        
        video=new TouchBitmapField(ei.getBitmap(),BitmapField.HIGHLIGHT_FOCUS);   
        video.setChangeListener(this);
        video.setMargin(5,0,5,(Display.getWidth() - 5*ei.getWidth())/5);
        hfmnew.add(video);
        
        ei = (DouglasConstants.NEWSMenu == str) ? EncodedImage.getEncodedImageResource(DouglasConstants.FooterNEWSMenuImageON): EncodedImage.getEncodedImageResource(DouglasConstants.FooterNEWSMenuImageOFF); 
        //ei.setScale(2);
        
        neu=new TouchBitmapField(ei.getBitmap(),BitmapField.HIGHLIGHT_FOCUS);   
        neu.setChangeListener(this);
        neu.setMargin(5,0,5,(Display.getWidth() - 5*ei.getWidth())/5);
        hfmnew.add(neu);
         
       return hfmnew;
        
}

public void fieldChanged(Field field, int context)
   {
        for (int i = UiApplication.getUiApplication().getScreenCount(); i >3; i--)
        {
                UiApplication.getUiApplication().popScreen(UiApplication.getUiApplication().getActiveScreen());
        }
        if(field == menu)
        {
            UiApplication.getUiApplication().pushScreen(new MenuScreen());
            
        } else if(field == parfumerien)
        {
            UiApplication.getUiApplication().pushScreen(new finder());
        
        } else  if(field == neu)
        {
            UiApplication.getUiApplication().pushScreen(new NeuheitenScreen());
        
        } else  if(field == video)
        {
            UiApplication.getUiApplication().pushScreen(new VideoTabScreen());
            
        } else  if(field == angebote)
        {
            UiApplication.getUiApplication().pushScreen(new ShopScreen());
            
        } 
               
   }
}


