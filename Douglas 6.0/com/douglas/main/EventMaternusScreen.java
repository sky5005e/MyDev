/*
 * MaternusScreen.java
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
import net.rim.device.api.util.MathUtilities;



public class EventMaternusScreen{
   
    public static Store[] perfumerie;
    public static Store[] events;
    public static int selectedIndex;
    public static Event[] eventdetail;
   
    public static int selectedTab;
    private String PerfumerieLink;
    private String distance;
    private VerticalFieldManager vfm = new VerticalFieldManager();
    private static EncodedImage beauty = EncodedImage.getEncodedImageResource(DouglasConstants.BeautyServices);
    private int flag;
    private Store[] store;
    private Shop shop;
    private Event[] event;
    
    private double f;
    
    public void MaternusScreen(int _flag)
    {
       flag = _flag;
    }
    public void EventMaternusLayout(final WebDataCallback callback) 
    {
       
       try
        {
            
         shop=EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop();
                
                 TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, 0);
        
             TableLayoutManager outerTable = new TableLayoutManager(new int[]
           {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            }, new int[]{Display.getWidth() - 50, 50},5,0);
            TableLayoutManager detailTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            },0);
            TableLayoutManager infoTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE
               
            },0);
            TableLayoutManager innerTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE
            },0);
        
        LabelField douglas=new LabelField(DouglasConstants.Douglas,LabelField.FOCUSABLE);
        EventLabel streetName=new EventLabel(shop.getStreetName()+shop.getStreetNumber(),LabelField.FOCUSABLE);
        streetName.setFont(DouglasConstants.mediumfont);
        EventLabel postalField = new EventLabel(shop.getPostalCode(), LabelField.FOCUSABLE);
        postalField.setFont(DouglasConstants.mediumfont); 
        EventLabel townLabel = new EventLabel(shop.getTown(),LabelField.FIELD_LEFT);
        
        EventLabel telefonLabel = new EventLabel(shop.getTelefon(),LabelField.FIELD_LEFT);
        townLabel.setFont(DouglasConstants.mediumfont);
        telefonLabel.setFont(DouglasConstants.mediumfont);
        innerTable.add(douglas);
        innerTable.add(streetName);
        innerTable.add(postalField);
        innerTable.add(townLabel);
        innerTable.add(telefonLabel);
       
        distance=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getDistance();
               
            
        EventLabel distance=new EventLabel(DouglasConstants.ThrLabel+MathUtilities.round(f)+DouglasConstants.DISTANCE,LabelField.FOCUSABLE);
        distance.setFont(DouglasConstants.smfont);
        innerTable.add(distance);
        outerTable.add(innerTable);
         //Add picture
        String url = shop.getImageSmall();
        BitmapField img = new WebBitmapField(url, 1);
        outerTable.add(img);  
        LabelField newsp=new LabelField(DouglasConstants.Space,LabelField.FOCUSABLE);
        groupTable.add(newsp);
        groupTable.add(outerTable);
       
       //google map
        Bitmap SERVICE_LOGO = Bitmap.getBitmapResource(DouglasConstants.service);
        BitmapField imgg = new BitmapField(SERVICE_LOGO,BitmapField.FIELD_RIGHT);
        detailTable.add(imgg);
       
       // LabelField route=new LabelField(DouglasConstants.Route,LabelField.FOCUSABLE);
       // detailTable.add(route);
        LabelField offnungszeiten=new LabelField(DouglasConstants.Offnungszeiten,LabelField.FOCUSABLE);
        infoTable.add(offnungszeiten);
        EventLabel openingtext=new EventLabel(shop.getRegularOpeningsText(),LabelField.FOCUSABLE);
        openingtext.setFont(DouglasConstants.mediumfont);
        infoTable.add(openingtext);
        detailTable.add(infoTable);
        outerTable.add(detailTable);
        groupTable.setMargin(15,10,0,10);
        outerTable.setMargin(0,5,0,0);
        vfm.add(groupTable);
             
            
        }   
        catch (Exception e){}
        
        UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                callback.callback(vfm);
            }
        });
   }
   
   public void EventBeautyLayout(final WebDataCallback callback) 
    {
       
       try
        {
           
         event=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getEvents();
             
             TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, 0);
        
           TableLayoutManager detailTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.USE_PREFERRED_SIZE
            },0);
            TableLayoutManager outerTable = new TableLayoutManager(new int[]
           {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            }, new int[]{Display.getWidth()-50, 50},0,0);
            
            TableLayoutManager innerTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
            },new int[]{(Display.getWidth()/2)},0,0);
    
            TableLayoutManager infoTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
            },new int[]{(Display.getWidth()/2)},0,Field.FIELD_RIGHT);
         TitleLabel streetName=new TitleLabel(DouglasConstants.BeautyServices,LabelField.FOCUSABLE);
         streetName.setFont(DouglasConstants.titlefont);
         innerTable.add(streetName);
      
         for(int i=0;i<event.length;i++)
             {
                
                 Integer type=Integer.valueOf(event[i].getType());
               if(type.intValue()==2)
               {
                 Bitmap IC_LOGO = Bitmap.getBitmapResource(DouglasConstants.ic);
                 BitmapField imgic = new BitmapField(IC_LOGO);
                 detailTable.add(imgic);   
                 EventLabel townLabel = new EventLabel(event[i].getTopic(),LabelField.FIELD_LEFT);
                 townLabel.setFont(DouglasConstants.mediumfont);
                 detailTable.add(townLabel);
                 
                }
               
             }  
         
        innerTable.add(detailTable);
        outerTable.add(innerTable);
        
        //Add picture
        
        Bitmap SERVICE_LOGO = Bitmap.getBitmapResource(DouglasConstants.service);
        BitmapField img = new BitmapField(SERVICE_LOGO,BitmapField.FIELD_RIGHT);
        infoTable.add(img); 
       
        TitleLabel descriptionLabel = new TitleLabel(DouglasConstants.Bitte,LabelField.FIELD_RIGHT);
        descriptionLabel.setFont(DouglasConstants.titlefont);
        infoTable.add(descriptionLabel);
        EventLabel teleLabel = new EventLabel(DouglasConstants.Rufen,LabelField.FOCUSABLE);
        teleLabel.setFont(DouglasConstants.mediumfont);
        infoTable.add(teleLabel);
        EventLabel telno=new EventLabel(PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop().getTelefon(),LabelField.FOCUSABLE);
        telno.setFont(DouglasConstants.mediumfont);
        infoTable.add(telno);
        
        outerTable.add(infoTable);
       
       
        groupTable.add(outerTable);
        groupTable.setMargin(15,10,0,10);
        outerTable.setMargin(0,5,0,0);
        vfm.add(groupTable);
             
            
        }   
        catch (Exception e){}
        
        UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                callback.callback(vfm);
            }
        });
   }
   public void EventTypeLayout(final WebDataCallback callback) 
    {
       
       try
        {
           
           
         event=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getEvents();
         for(int i=0;i<event.length;i++)
         {
            Integer type=Integer.valueOf(event[i].getType());
            
           if(type.intValue()==1 || type.intValue()==3)
           {
             
               TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, 0);
        
              TableLayoutManager outerTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, new int[]{Display.getWidth() - 50, 50},5,0);
            
             TableLayoutManager innerTable = new TableLayoutManager(new int[]
             {
               TableLayoutManager.FIXED_WIDTH
             },new int[] {Display.getWidth()/2},0,0);
                
               
             TitleLabel topicLabel = new TitleLabel(event[i].getTopic(),LabelField.FIELD_LEFT);
        
             TitleLabel scheduleLabel = new TitleLabel(event[i].getSchedule()+DouglasConstants.Hypen+event[i].getScheduleShort(),LabelField.FIELD_LEFT);
             topicLabel.setFont(DouglasConstants.mediumfont);
             scheduleLabel.setFont(DouglasConstants.smfont);
             innerTable.add(topicLabel);
             innerTable.add(scheduleLabel);
       
        
             outerTable.add(innerTable);
             outerTable.setID(i);
             outerTable.setFlag(flag);
             //Add picture
             String url = event[i].getTeaserImage();
             BitmapField img = new WebBitmapField(url, 3);
             outerTable.add(img);  
             
            
             groupTable.add(outerTable);
             
             SubTitleLabel descriptionLabel = new SubTitleLabel(event[i].getText(),LabelField.FIELD_LEFT);
         
             descriptionLabel.setFont(DouglasConstants.mediumfont);
             groupTable.add(descriptionLabel);
             groupTable.setMargin(15,10,0,10);
             outerTable.setMargin(0,5,0,0);
             String hex = (i % 2 == 0) ? DouglasConstants.hexLightBlue : DouglasConstants.hexWhite;
             int intValue = Integer.parseInt(hex, 16); 
             Background bg = BackgroundFactory.createSolidBackground(intValue);
             groupTable.setBackground(bg);
             vfm.add(groupTable);
        }
        }        
            
        }   
        catch (Exception e){}
        
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
                      
                   //   UiApplication.getUiApplication().pushScreen(new EventsScreen());
                        
              }
    };
}  

