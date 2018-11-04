/*
 * EventsScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;

import  com.douglas.common.*;
import res.layout.*;
import java.io.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.component.*;
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



public class EventsScreenDetail{
    public static Store[] perfumerie;
    public static Store[] events;
    public static int selectedIndex;
    public static int selectedTab;
    private String PerfumerieLink;
    private VerticalFieldManager vfm = new VerticalFieldManager();
    private Event[] event;
    public static Store[] store;
    private int flag;
    private double f;
    private Shop shop;
    private String distance;
     public void EventsScreenDetail(String _PerfumerieLink, int _flag)
    {
        PerfumerieLink = _PerfumerieLink;
        flag = _flag;
      
    }
    public void EventsScreenLayoutDetail(final WebDataCallback callback) 
    {
       
       try
        {
         
            TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, 0);
        
            TableLayoutManager outerTable = new TableLayoutManager(new int[]
           {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            }, new int[]{Display.getWidth() - 50, 50},5,Field.FIELD_HCENTER | Field.FIELD_HCENTER);
            
            TableLayoutManager innerTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
            },new int[] {Display.getWidth()/2},20,Field.FIELD_VCENTER | Field.FIELD_HCENTER);
    
        TitleLabel topicLabel = new TitleLabel(PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getTopic(),LabelField.FIELD_LEFT);
      
        TitleLabel scheduleLabel = new TitleLabel(PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getSchedule()+DouglasConstants.Hypen+PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getScheduleShort(),LabelField.FIELD_LEFT);
        topicLabel.setFont(DouglasConstants.titlefont);
        scheduleLabel.setFont(DouglasConstants.smfont);
        LabelField spc=new LabelField(DouglasConstants.Space);
        innerTable.add(topicLabel);
        innerTable.add(spc);
        innerTable.add(scheduleLabel);
        Integer exclusive=Integer.valueOf(PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getExclusive());
        if(exclusive.intValue()==1)
        {
                EncodedImage dcexc = EncodedImage.getEncodedImageResource("res/drawable/label_cardexklusiv.gif");  
               // dcexc.setScale(2);
                BitmapField dcimg =new BitmapField(dcexc.getBitmap());
                innerTable.add(dcimg);
        }
        
        outerTable.add(innerTable);
        
        //Add picture
        String url = PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getTeaserImage();
        BitmapField img = new WebBitmapField(url, 3);
        outerTable.add(img);  
        LabelField lbl=new LabelField(DouglasConstants.Space);
        outerTable.add(lbl);
        groupTable.add(outerTable);
        
       // SubTitleLabel descriptionLabel = new SubTitleLabel(PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getText(),LabelField.FIELD_LEFT);
        RichTextLabel descriptionLabel = new RichTextLabel(PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getText(),(DrawStyle.HFULL |  RichTextField.FIELD_VCENTER |
                RichTextField.FOCUSABLE | RichTextField.NO_NEWLINE)); 
        descriptionLabel.setFont(DouglasConstants.smfont);
        groupTable.add(descriptionLabel);
        groupTable.setMargin(15,10,0,10);
      
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
    public void DiesemEventsLayout(final WebDataCallback callback) 
    {
          
       try
        {
           perfumerie = NetworkUtilities.fetchStores("getStoresFromCoords.php5?"+PerfumerieLink+"&eventid="+PerfumerieScreen.eventsurl[PerfumerieScreen.selectedIndex].getEventId());
           store = new Store[perfumerie.length];
           store = perfumerie;
             for (int i = 0; i < store.length; i++)
            {
                shop=store[i].getShop();
             TableLayoutManager outerTable = new TableLayoutManager(new int[]
                {
                        TableLayoutManager.USE_PREFERRED_SIZE,
                        TableLayoutManager.SPLIT_REMAINING_WIDTH,
                        TableLayoutManager.USE_PREFERRED_SIZE
                },0);
                
                TableLayoutManager innerTable = new TableLayoutManager(new int[]
                {
                        TableLayoutManager.USE_PREFERRED_WIDTH_WITH_MAXIMUM
                }, Manager.USE_ALL_WIDTH | Field.FIELD_VCENTER);
               
                
               TitleLabel titleField=new TitleLabel(shop.getStreetName()+DouglasConstants.Space+shop.getStreetNumber(),LabelField.FOCUSABLE);
               titleField.setFont(DouglasConstants.mfont);
               LabelField space=new LabelField(DouglasConstants.Space,LabelField.FOCUSABLE);
               SubTitleLabel postalField = new SubTitleLabel(shop.getPostalCode()+DouglasConstants.Space+shop.getTown(), LabelField.FOCUSABLE);
               postalField.setFont(DouglasConstants.smfont);
               
               innerTable.add(titleField);
               innerTable.add(space);
               innerTable.add(postalField);
              
             
               BitmapField img =new WebBitmapField(shop.getImageSmall(),2);
               outerTable.add(img);
            
               outerTable.add(innerTable);
               distance=store[i].getDistance();
               
               f=Double.parseDouble(distance);
            
               NumberFormat nf=new NumberFormat();
               double d=f/1000;
               String parsedNumber=f>1000? nf.formatNumber(d, 1, ",")+DouglasConstants.KMDISTANCE:nf.formatNumber(f, 0, ",")+DouglasConstants.DISTANCE;

               TitleLabel distanceField = new TitleLabel(parsedNumber, LabelField.FOCUSABLE | LabelField.FIELD_VCENTER);
               distanceField.setFont(DouglasConstants.mediumfont);
               
               outerTable.add(distanceField);
               outerTable.setID(i);
               outerTable.setFlag(flag);
                
               outerTable.setChangeListener(itemlistener);  
               outerTable.setMargin(0,5,0,0);
               String hex = (i % 2 == 0) ? DouglasConstants.hexWhite:DouglasConstants.hexLightBlue;
               int intValue = Integer.parseInt(hex, 16); 
               Background bg = BackgroundFactory.createSolidBackground(intValue);
               outerTable.setBackground(bg);
            
                
               vfm.add(outerTable);
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
                     
                     UiApplication.getUiApplication().pushScreen(new StoreTabScreen(1,0,0));
                    
                        
              }
    };
   
}  

