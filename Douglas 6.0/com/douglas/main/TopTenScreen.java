/*
 * ToptenScreen.java
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


public class TopTenScreen{
   
    public static TopTen[] toptenDamen;
    public static TopTen[] toptenHerren;
    public static int selectedIndex;
    public static int selectedTab;
    private String TopTenLink;
    private WebVerticalFieldManager vfm = new WebVerticalFieldManager();
    private int flag;
    private TopTen[] topten;
    
    public void TopTenScreen(String _TopTenLink, int _flag)
    {
        TopTenLink = _TopTenLink;
        flag = _flag;
    
    }
    
    public void TopTenScreenLayout(final WebDataCallback callback) 
    {
       
       try
        {
          
           if (flag == 0)
           {
                toptenDamen = NetworkUtilities.fetchTopTen(TopTenLink);
                topten = new TopTen[toptenDamen.length];
                topten = toptenDamen;
           }
           else
           {
                toptenHerren = NetworkUtilities.fetchTopTen(TopTenLink);
                topten = new TopTen[toptenHerren.length];
                topten = toptenHerren;
           }
            
           
            for (int i = 0; i < topten.length; i++)
            {
                
                    TableLayoutManager outerTable = new TableLayoutManager(new int[]
                    {       TableLayoutManager.USE_PREFERRED_SIZE,
                            TableLayoutManager.USE_PREFERRED_SIZE,
                            TableLayoutManager.USE_PREFERRED_SIZE,
                            TableLayoutManager.SPLIT_REMAINING_WIDTH
                    },Field.FOCUSABLE | Field.HIGHLIGHT_FOCUS | Field.HIGHLIGHT_SELECT);
                     TableLayoutManager staticTable = new TableLayoutManager(new int[]
                    {
                            TableLayoutManager.FIXED_WIDTH,
                           
                            
                    },new int[] {Display.getWidth()/3-50},0,0);
                    TableLayoutManager innerTable = new TableLayoutManager(new int[]
                    {
                           TableLayoutManager.USE_PREFERRED_SIZE
                    },Field.FIELD_VCENTER);
                 
               
                
                 TitleLabel titleField=new TitleLabel(topten[i].getBrand(),LabelField.FOCUSABLE);
                 titleField.setFont(DouglasConstants.titlefont);
                 titleField.setPadding(4,0,0,0);
                 
                 SubTitleLabel productField=new SubTitleLabel(topten[i].getProduct(), LabelField.FOCUSABLE);
                 productField.setFont(DouglasConstants.mediumfont);
                 productField.setPadding(4,0,0,0);
                 
                 SubTitleLabel categoryField=new SubTitleLabel(topten[i].getCategory(), LabelField.FOCUSABLE);
                 categoryField.setFont(DouglasConstants.mediumfont);
                 categoryField.setPadding(4,0,0,0);
                
               
               //Prepare Inner Table
                 innerTable.add(titleField);
                 innerTable.add(productField);
                 innerTable.add(categoryField);
                 
                
                 
               //Add Number Image  
               Integer no=Integer.valueOf(topten[i].getRank());
              if(no.intValue()==topten.length)
              {
                EncodedImage numberImage = EncodedImage.getEncodedImageResource("res/drawable/" + topten[i].getRank() +".png");  
                numberImage.setScale(2);
                BitmapField numberImageField = new BitmapField(numberImage.getBitmap());
                numberImageField.setPadding(33,0,30,10);
                staticTable.add(numberImageField);
              }else
              {
                EncodedImage numberImage = EncodedImage.getEncodedImageResource("res/drawable/" + topten[i].getRank() +".png");  
                numberImage.setScale(2);
                BitmapField numberImageField = new BitmapField(numberImage.getBitmap());
                numberImageField.setPadding(33,0,30,15);
                staticTable.add(numberImageField);
              } 
               
               
               outerTable.add(staticTable);
                //Add picture
                String url = topten[i].getPictureurl();
                BitmapField img = new WebBitmapField(url,1);
                
                outerTable.add(img);
                
                
                String hex = (i % 2 == 0) ?DouglasConstants.hexWhite:DouglasConstants.hexLightBlue;
                int intValue = Integer.parseInt(hex, 16); 
                Background bg = BackgroundFactory.createSolidBackground(intValue);
                outerTable.setBackground(bg);
                
                outerTable.add(innerTable);
                
                outerTable.setID(i);
                outerTable.setFlag(flag);
                
                outerTable.setChangeListener(itemlistener);  
                
                vfm.add(outerTable);
             
            }
        }   
        catch (Exception e){
           
            LabelField error=new LabelField("Not Found");
                           vfm.add(error);
            }
        
        UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                callback.callback(vfm);
            }
        });
   }
   
   
   FieldChangeListener itemlistener = new FieldChangeListener() {
 
              public void fieldChanged(Field field, int context)
              {
                      TableLayoutManager _tlm = (TableLayoutManager) field;
                      selectedIndex = _tlm.getID();
                      selectedTab = _tlm.getFlag();
                       
                      UiApplication.getUiApplication().pushScreen(new TopTenDetailScreen());
                        
              }
    };

} 








