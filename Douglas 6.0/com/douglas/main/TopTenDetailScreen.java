/*
 * TopTenScreen.java
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
import net.rim.device.api.ui.container.*;


public class TopTenDetailScreen extends MainScreen implements FieldChangeListener {
    private TouchBitmapMenuField shopimg;
    public static String URL;
    private VerticalFieldManager vfm = new VerticalFieldManager();
     private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderTopBackImageON);  
    private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderTopBackImageOFF);
    public TopTenDetailScreen()
    {
        HeaderLabel HL = new HeaderLabel();
        VerticalFieldManager vfm = HL.HeaderLabel((TopTenScreen.selectedTab == 0 ? DouglasConstants.TopTenDamen : DouglasConstants.TopTenHerren),0,menuOn,menuOFF,this);
        
        setBanner(vfm);
        TableLayoutManager groupTable = new TableLayoutManager(new int[]
        {
                TableLayoutManager.SPLIT_REMAINING_WIDTH
        }, 0);
        TableLayoutManager imgTable = new TableLayoutManager(new int[]
        {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
        },0);
        TableLayoutManager outerTable = new TableLayoutManager(new int[]
        {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
        }, new int[]{Display.getWidth() - 50, 50},5,0);
            
        TableLayoutManager innerTable = new TableLayoutManager(new int[]
        {
              TableLayoutManager.FIXED_WIDTH
        },new int[] {Display.getWidth()/2+30},0,0);
      
        TitleLabel platzLabel = new TitleLabel(String.valueOf(TopTenScreen.selectedIndex + 1) + ". " + DouglasConstants.PLATZ,LabelField.FIELD_LEFT);
        TitleLabel brandLabel = new TitleLabel(TopTenScreen.selectedTab == 0 ? TopTenScreen.toptenDamen[TopTenScreen.selectedIndex].getBrand() : TopTenScreen.toptenHerren[TopTenScreen.selectedIndex].getBrand(),LabelField.FIELD_LEFT);
        TitleLabel productLabel = new TitleLabel(TopTenScreen.selectedTab == 0 ? TopTenScreen.toptenDamen[TopTenScreen.selectedIndex].getProduct() : TopTenScreen.toptenHerren[TopTenScreen.selectedIndex].getProduct(),LabelField.FIELD_LEFT);
        platzLabel.setFont(DouglasConstants.titlefont);
        brandLabel.setFont(DouglasConstants.titlefont);
        productLabel.setFont(DouglasConstants.titlefont);
        platzLabel.setPadding(4,0,0,0);
        brandLabel.setPadding(4,0,0,0);
        productLabel.setPadding(4,0,0,0);
        innerTable.add(platzLabel);
        innerTable.add(brandLabel);
        innerTable.add(productLabel);
        
        outerTable.add(innerTable);
        
        //Add picture
        LabelField space=new LabelField(DouglasConstants.Space);
        imgTable.add(space);
        LabelField sp=new LabelField(DouglasConstants.Space);
        imgTable.add(sp);
        String url=TopTenScreen.selectedTab == 0 ? TopTenScreen.toptenDamen[TopTenScreen.selectedIndex].getPictureurl() : TopTenScreen.toptenHerren[TopTenScreen.selectedIndex].getPictureurl();
        BitmapField img = new WebBitmapField(url, 1);
        imgTable.add(img);
        outerTable.add(imgTable);  
       
        groupTable.add(outerTable);
        LabelField spacenew=new LabelField(DouglasConstants.Space);
        groupTable.add(spacenew);
        LabelField descriptionLabel = new LabelField(TopTenScreen.selectedTab == 0 ? TopTenScreen.toptenDamen[TopTenScreen.selectedIndex].getDescription() : TopTenScreen.toptenHerren[TopTenScreen.selectedIndex].getDescription(),LabelField.FIELD_LEFT){
            public void paint(Graphics g)
    {
          String hexcolor = DouglasConstants.hexSubTitleText; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
          g.setColor(hexValue);
          super.paint(g);
    }
            };
         
        descriptionLabel.setFont(DouglasConstants.mediumfont);
        groupTable.add(descriptionLabel);
        groupTable.setMargin(15,10,0,10);
        add(groupTable);
        String shopurl = (TopTenScreen.selectedTab == 0 ? TopTenScreen.toptenDamen[TopTenScreen.selectedIndex].getShopurl() : TopTenScreen.toptenHerren[TopTenScreen.selectedIndex].getShopurl());
        this.setURL(shopurl);
        DouglasFooter douglasFT = new DouglasFooter();
        PrevNext prevNextButton = new PrevNext();
        Bitmap zumshop = Bitmap.getBitmapResource(DouglasConstants.zumshop);
        Bitmap zumshopon = Bitmap.getBitmapResource(DouglasConstants.zumshopon);
        shopimg = new TouchBitmapMenuField(zumshop,zumshopon,0,BitmapField.FOCUSABLE);
        shopimg.setMargin(0,0,5,(Display.getWidth()/2-40));
        shopimg.setChangeListener(this);
        VerticalFieldManager vfmStatus = new VerticalFieldManager();
        vfmStatus.add(shopimg);
        vfmStatus.add(prevNextButton.PrevNext(DouglasConstants.TopTenScreenNumber, TopTenScreen.selectedIndex, 9,0));
        vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.HomeMenu));
       
        setStatus(vfmStatus);
        
    }
      public String getURL()
         {
             return URL;
         }
         
         public void setURL(String _url)
         {
             URL = _url;
            
         }
   
   public void fieldChanged(Field field, int context)
 {
    if(field==shopimg)
     {
            UiApplication.getUiApplication().pushScreen(new ZumShop(0));
          
     }
               
 }      


} 








