/*
 * PerfumerieScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;

import  com.douglas.common.*;
import res.layout.*;
import java.io.*;
import net.rim.device.api.i18n.*;
import net.rim.device.api.i18n.MessageFormat;
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
import javax.microedition.global.Formatter;


public class PerfumerieScreen{
   
    public static Store[] perfumerie;
    public static Store[] events;
    public static Event[] eventsurl;
    public static int selectedIndex;
    public static int selectedIndexNew;
    public static int selectedTab;
    private String PerfumerieLink;
    private String distance;
    private VerticalFieldManager vfm = new VerticalFieldManager();
    private int flag;
    public static Store[] store;
    public static Event[] event;
    private Shop shop;
    private double f;
    public static int ID;
    private int j;
    private int s;
    public int k;
    private Integer exclusive;
    public void PerfumerieScreen(String _PerfumerieLink, int _flag,int k,int s)
    {
        PerfumerieLink = _PerfumerieLink;
        flag = _flag;
        k=k;
        s=s;
    }
    
    public void PerfumerieScreenLayout(final WebDataCallback callback,int j,int n) 
    {
       n=n;
       try
        { 
               
                k=k>0?k:0;
               
                perfumerie = NetworkUtilities.fetchStores("getStoresFromCoords.php5?offset="+j+""+PerfumerieLink);
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
                        TableLayoutManager.USE_PREFERRED_SIZE
                }, Manager.USE_ALL_WIDTH | Field.FIELD_VCENTER );
               
                
               TitleLabel titleField=new TitleLabel(shop.getStreetName()+DouglasConstants.Space+shop.getStreetNumber(),LabelField.FOCUSABLE);
               titleField.setFont(DouglasConstants.menufont);
               LabelField space=new LabelField(DouglasConstants.Space);
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
              
               
               
             //  outerTable.add(distanceField);  
              
               
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
           
             DouglasFooter douglasFT = new DouglasFooter();
             PrevNext prevNextButton = new PrevNext();
         
             VerticalFieldManager vfmStatus = new VerticalFieldManager();
             this.setID(j);
             if(n==0)
             {
             vfmStatus.add(prevNextButton.PrevNext(DouglasConstants.PerfumerieScreenNumber,j,60,0)); 
             }else
             {
             vfmStatus.add(prevNextButton.PrevNext(DouglasConstants.PerfumerieScreenNumber,j,60,1));     
             }
             //  vfmStatus.add(prevNextButton.PrevNext(DouglasConstants.MaternusScreenNumber,j,60,1));
             
             vfm.add(vfmStatus);
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
      

    public void EventsScreenLayout(final WebDataCallback callback,int j) 
    {
       
       try
        {
         
                
                eventsurl = NetworkUtilities.fetchEvents("getStoresFromCoords.php5?offset="+j+""+PerfumerieLink+"&events=1");
                event = new Event[eventsurl.length];
                event = eventsurl;
           
            
           
            for (int i = 0; i < event.length; i++)
            {
                 
                
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
               
                TableLayoutManager imageTable = new TableLayoutManager(new int[]
                {
                        TableLayoutManager.USE_PREFERRED_SIZE
                },0);
               TitleLabel topicField=new TitleLabel(event[i].getTopic(),LabelField.FOCUSABLE | LabelField.ELLIPSIS){
                      
             public int getPreferredHeight(){
               return getFont().getHeight();
  }
};
               topicField.setFont(DouglasConstants.menufont);
             // topicField.setPadding(10,0,0,0);
               LabelField space=new LabelField(DouglasConstants.Space);
               SubTitleLabel schedule=new SubTitleLabel(event[i].getSchedule()+DouglasConstants.Hypen+event[i].getScheduleShort(),LabelField.FOCUSABLE | LabelField.ELLIPSIS){
                      
             public int getPreferredHeight(){
               return getFont().getHeight();
  }
};
               schedule.setFont(DouglasConstants.smfont);
            // schedule.setPadding(0,0,10,0);
              
              
               innerTable.add(topicField);
               innerTable.add(space);
               innerTable.add(schedule);
              
             
             
               BitmapField img =new WebBitmapField(event[i].getTeaserImage(),4);
               imageTable.add(img);
               Integer exclusive=Integer.valueOf(event[i].getExclusive());
                if(exclusive.intValue()==1)
                {
                EncodedImage dcexc = EncodedImage.getEncodedImageResource("res/drawable/label_cardexklusiv.gif");  
               // dcexc.setScale(2);
                BitmapField dcimg =new BitmapField(dcexc.getBitmap());
                innerTable.add(dcimg);
                }
               outerTable.add(imageTable);
               outerTable.add(innerTable);
               
               distance=event[i].getDistance();
               
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
               outerTable.setPadding(10,5,10,0);
               String hex = (i % 2 == 0) ?DouglasConstants.hexWhite:DouglasConstants.hexLightBlue;
               int intValue = Integer.parseInt(hex, 16); 
               Background bg = BackgroundFactory.createSolidBackground(intValue);
               outerTable.setBackground(bg);
               //outerTable.setMargin(10,0,10,0);
                
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
   public int getID()
       {
           return ID;
       }
       public void setID(int _id)
       {
           ID=_id;
        }
   FieldChangeListener itemlistener = new FieldChangeListener() {
 
              public void fieldChanged(Field field, int context)
              {
                      TableLayoutManager _tlm = (TableLayoutManager) field;
                      PerfumerieScreen ps=new PerfumerieScreen();
                    //  selectedStoreList = ps.getID();
                      selectedIndex=_tlm.getID();
                      selectedTab = _tlm.getFlag();
                      if(selectedTab==0)
                      {
                      UiApplication.getUiApplication().pushScreen(new StoreTabScreen(0,0,0));
                      }else
                      {
                         UiApplication.getUiApplication().pushScreen(new EventsTabScreen());
                       }
                        
              }
    };
    
  

} 
