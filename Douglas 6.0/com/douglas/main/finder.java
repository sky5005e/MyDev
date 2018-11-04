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
import res.layout.*;
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
import net.rim.device.api.i18n.*;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.ui.decor.*;

/**
 * 
 */
public class finder extends MainScreen{
   
    private ObjectChoiceField _choiceField;
    private TextLabel plz;
    private EditField stadt;
    public static ButtonField suchen;
    public static ButtonField ok;
    private String aStr;
    private String plztxt;
    private String stdtxt;
    private Object country;
    public static String str;
    public static String URL;
    public static String StoredUrl;
    public static String de="de";
    public static String txt;
    private WebVerticalFieldManager wvfm = new WebVerticalFieldManager();
    private static EncodedImage LupeOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderLupeImageON);  
    private static EncodedImage LupeOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderLupeImageOFF); 
    private static EncodedImage OkOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderOkImageON);  
    private static EncodedImage OkOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderOkImageOFF);
    private static EncodedImage SuchenOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchenImageON);  
    private static EncodedImage SuchenOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchenImageOFF);  
    private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
    private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);  
    private static EncodedImage AbbrechenOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderAbbrechenImageON);  
    private static EncodedImage AbbrechenOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderAbbrechenImageOFF); 
     public finder() 
    {
           
            int intValue = Integer.parseInt(DouglasConstants.hexWhite, 16); 
            Background white = BackgroundFactory.createSolidBackground(intValue);
            int border=Integer.parseInt("84A6A5",16);
            StoreHeaderLabel HL=new StoreHeaderLabel();
            VerticalFieldManager vfm = HL.StoreHeaderLabel(LupeOn,LupeOFF,DouglasConstants.Suchergebnis,AbbrechenOn,AbbrechenOFF,1,this,"",1);
            setBanner(vfm); 
            DouglasFooter douglasFT = new DouglasFooter();
            VerticalFieldManager vfmStatus = new VerticalFieldManager();
            vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
            setStatus(vfmStatus);
            add(wvfm);
            VerticalFieldManager vfmnew=new VerticalFieldManager(VerticalFieldManager.LEAVE_BLANK_SPACE){
                public void paint(Graphics graphics)
                {
                     int intValue = Integer.parseInt(DouglasConstants.hexBackground, 16);
                     graphics.setBackgroundColor(intValue);
                     graphics.clear();
                     super.paint(graphics);
                }
                };
            TitleLabel title=new TitleLabel(DouglasConstants.DarfDouglas,LabelField.FIELD_HCENTER);
            title.setFont(DouglasConstants.pfont);
            TitleLabel stitle=new TitleLabel(DouglasConstants.Verwenden,LabelField.FIELD_HCENTER);
            stitle.setFont(DouglasConstants.pfont);
            ok=new ButtonField(DouglasConstants.Ok,ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER );
           
            ok.setMargin(10,0,10,0);
            ok.setChangeListener(new FieldChangeListener() {
            public void fieldChanged(Field field, int context) {
                
     //UiApplication.getUiApplication().pushScreen(new CustomPopupScreen("Warten Sie bitte, während die Standortbestimmung geladen wird. Ihr Endgerät muss hierzu Javascript unterstützen.",0));
             UiApplication.getUiApplication().pushScreen(new GPSScreen(0));
            }
          });
            TitleLabel subtitle=new TitleLabel(DouglasConstants.OrderSuche,LabelField.FIELD_HCENTER);
            subtitle.setFont(DouglasConstants.pfont);
            subtitle.setMargin(10,0,10,0);
        
            String choicestrs[] = {"Deutschland", "Niederlande", "Frankreich"};
            _choiceField = new ObjectChoiceField("",choicestrs,0,ObjectChoiceField.FIELD_HCENTER);
            
           
            HorizontalFieldManager hfm=new HorizontalFieldManager();
            TitleLabel plzLabel=new TitleLabel(DouglasConstants.Plz,LabelField.FIELD_HCENTER);
            plzLabel.setFont(DouglasConstants.pfont);    
            plzLabel.setMargin(10,0,10,75);
            plzLabel.setPadding(2,0,0,0);
            hfm.add(plzLabel); 
            plz=new TextLabel("","",EditField.DEFAULT_MAXCHARS,EditField.EDITABLE | EditField.FIELD_HCENTER);
            plz.setBorder((BorderFactory.createRoundedBorder(new XYEdges(3, 3, 3, 3),border,Border.STYLE_FILLED)));
            plz.setMargin(10,70,10,0);
            hfm.add(plz);
            
            HorizontalFieldManager hfmn=new HorizontalFieldManager(HorizontalFieldManager.FIELD_HCENTER);
            TitleLabel stadtLabel=new TitleLabel(DouglasConstants.Stadt,LabelField.FIELD_HCENTER);
            stadtLabel.setFont(DouglasConstants.pfont);    
            stadtLabel.setMargin(11,0,10,75);
            stadtLabel.setPadding(2,0,0,0);
            hfmn.add(stadtLabel);
            
            stadt=new TextLabel("","",EditField.DEFAULT_MAXCHARS,EditField.EDITABLE | EditField.FIELD_HCENTER);
            stadt.setBorder((BorderFactory.createRoundedBorder(new XYEdges(3, 3, 3, 3),border,Border.STYLE_FILLED)));     
            
            stadt.setMargin(11,70,10,0);
            hfmn.add(stadt);
            LabelField spc=new LabelField(DouglasConstants.Space);
           // spc.setMargin(13,0,0,0);
            suchen=new ButtonField(DouglasConstants.Suchen,ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
            suchen.setMargin(13,0,0,0);
            suchen.setChangeListener(new FieldChangeListener() {
            public void fieldChanged(Field field, int context) {
                showAddResult();
               
            }
         });
          
          
            vfmnew.add(title);
            vfmnew.add(stitle);
            vfmnew.add(ok);
            vfmnew.add(subtitle);
            vfmnew.add(hfm);
         
            vfmnew.add(hfmn);
           
            vfmnew.add(_choiceField);
            vfmnew.add(suchen);
            vfmnew.add(spc);
            add(vfmnew);
           
           }
         

    /**
     * Close the serial port
     */
   
        public String getURL()
         {
             return URL;
         }
         
         public void setURL(String _url)
         {
             URL = _url;
            
         }
   public void showAddResult() {
        String message = "";
        country = _choiceField.getChoice(_choiceField.getSelectedIndex());
        str = country.toString();
        String plztxt=String.valueOf(plz.getText());
        String stdtxt=String.valueOf(stadt.getText());
       if(plztxt.length() != 0 && stdtxt.length() != 0)
        {
            message ="&zip="+plztxt+"&count="+10+""+"&town="+stdtxt+""+"&cc="+de;
            this.setURL(message);
        }else if(plztxt.length() != 0)
          
        {
           message ="&zip="+plztxt+""+"&count="+10+""+"&cc="+de;  
           this.setURL(message);
         
        }else if(stdtxt.length() != 0)
        {
            message ="&town="+stdtxt+""+"&count="+10+""+"&cc="+de;
            this.setURL(message);
        }
        if(plztxt.length() == 0 && stdtxt.length() == 0)
        {
           Status.show("Bitte geben Sie eine Adresse fur die Suche ein",1000);
          
        }else
        {
             StoredUrl=getURL();
            UiApplication.getUiApplication().pushScreen(new PerfumerieTabScreen(0));
            
        }
      
    }
}

