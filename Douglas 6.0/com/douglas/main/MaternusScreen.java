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
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.util.MathUtilities;
import java.util.*;
import net.rim.device.api.browser.field2.*;
import net.rim.blackberry.api.browser.Browser;
import net.rim.blackberry.api.browser.BrowserSession;
import net.rim.device.api.ui.component.ActiveRichTextField;
import net.rim.blackberry.api.invoke.*;
 import net.rim.blackberry.api.maps.*;
import net.rim.device.api.lbs.maps.*;
import net.rim.device.api.lbs.maps.model.*;
import net.rim.device.api.lbs.maps.ui.*;



public class MaternusScreen extends MainScreen implements FieldChangeListener{
   
    public static Store[] perfumerie;
    public static Store[] events;
    public static int selectedIndex;
    public static int eventIndex;
    public static Event[] eventdetail;
    public static int selectedTab;
    private String PerfumerieLink;
    public CustomButtonField route;
    public CustomButtonField telefonLabel;
    private String distance;
    private VerticalFieldManager vfm = new VerticalFieldManager();
    private static EncodedImage beauty = EncodedImage.getEncodedImageResource(DouglasConstants.BeautyServices);
    private int flag;
    private int screen;
    private Store[] store;
    private Shop shop;
    final Vector eventVector = new Vector();
    private Event[] event;
    private Event[] eventnew;
    public static int ID;
    public static String Lat;
    public static String Lng;
    public static String Add;
    //private ActiveRichTextField telno;
    private BrowserField _browserField;
    private double f;
    public void MaternusScreen()
    {
    }
    public void MaternusScreen(int _flag,int _screen)
    {
      
       flag = _flag;
       screen = _screen;
    }
    public void MaternusScreenLayout(final WebDataCallback callback) 
    {
       
       try
        {
            GPS gps= new GPS();
            gps.start();
            double acc = gps.getAccuracy();
        
         shop=flag==1?EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop():PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop();
       /*  if(flag==1)
         {   
         shop=EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop(); 
        
         }else
         {
             shop=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop();
         } */     
            TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             },0);
        
             TableLayoutManager outerTable = new TableLayoutManager(new int[]
           {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            },0);
            
            TableLayoutManager detailTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            },0);
            
             TableLayoutManager infoinTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            },0);
            
            TableLayoutManager disTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
               
            },new int[]{(Display.getWidth()/2)},0,Field.FIELD_LEFT);
            TableLayoutManager infoTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
               
            },new int[]{(Display.getWidth()/2)},0,Field.FIELD_LEFT);
            
            TableLayoutManager innerTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
            },new int[]{(Display.getWidth()/2)},0,Field.FIELD_LEFT);
            
            TableLayoutManager mapTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.FIXED_WIDTH
            },new int[]{(Display.getWidth()/2)},0,Field.FIELD_LEFT);
       
        LabelField douglas=new LabelField(DouglasConstants.Douglas);
        douglas.setPadding(2,0,2,0);
        innerTable.add(douglas);
        
        EventLabel streetName=new EventLabel(shop.getStreetName()+DouglasConstants.Space+shop.getStreetNumber(),LabelField.FOCUSABLE);
        streetName.setFont(DouglasConstants.mediumfont);
        streetName.setPadding(2,0,2,0);
        innerTable.add(streetName);
        
        EventLabel postalField = new EventLabel(shop.getPostalCode()+DouglasConstants.Space+shop.getTown(),LabelField.FOCUSABLE);
        postalField.setFont(DouglasConstants.mediumfont); 
        postalField.setPadding(2,0,2,0);
        innerTable.add(postalField);
        
        Bitmap IMG = Bitmap.getBitmapResource(DouglasConstants.HandSet);
        BitmapField calimg = new BitmapField(IMG,BitmapField.FOCUSABLE);
        infoinTable.add(calimg);
       
        String labelEmailText =shop.getTelefon();
       // ARichTextField phnno=new ARichTextField();
        int defaultTextHeight = Math.min(Display.getHeight() / 15, 15);
        Font myFont = FontFamily.forName("BBAlpha Sans").getFont(Font.PLAIN, defaultTextHeight);
        
        int[] offsets = {0, labelEmailText.length()};
        Font[] fonts = {myFont};
        byte[] attributes = {0};
        ARichTextField phnno=new ARichTextField();
        infoinTable.add(phnno.ARichTextField(labelEmailText,offsets, attributes, fonts, null, null,FIELD_LEFT | FOCUSABLE | (long)ActiveRichTextField.USE_TEXT_WIDTH,0)); 
        // infoinTable.add(telefonLabel);
        innerTable.add(infoinTable);
        outerTable.add(innerTable);
        
//**Add picture from URL
        String url = shop.getImageLarge();
        BitmapField img = new WebBitmapField(url, 3);
        outerTable.add(img); 
        distance=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getDistance();
        f=Double.parseDouble(distance);
        NumberFormat nf=new NumberFormat();
        double d=f/1000;
        String parsedNumber=f>1000? nf.formatNumber(d, 1, ","):nf.formatNumber(f, 0, ",");
        EventLabel distance=new EventLabel(DouglasConstants.ThrLabel+parsedNumber+DouglasConstants.DISTANCE,LabelField.FOCUSABLE);
        distance.setFont(DouglasConstants.smfont);
        mapTable.add(distance);
        groupTable.add(outerTable);
        LabelField spc=new LabelField(DouglasConstants.Space,LabelField.FOCUSABLE);
        groupTable.add(spc);
      
   //**Google Map
       double lng=Double.parseDouble(shop.getAddresslng());
       double lat=Double.parseDouble(shop.getAddresslat());
     
       MapDataModel data = new MapDataModel();
       MapLocation andrew = new MapLocation(lat,lng, shop.getStreetName(), null );
       data.add( (Mappable) andrew );
      // specify the image size, center and zoom level
        MapDimensions dim = new MapDimensions( 180, 120 );
        dim.setCentre( andrew );
        dim.setZoom(6);
       // create the image
        Bitmap map = MapFactory.getInstance().generateStaticMapImage( dim, data );
        BitmapField imgb = new BitmapField(map);
        mapTable.add(imgb);
       
   //**Route Label  
     route=new CustomButtonField(DouglasConstants.Route,Field.FOCUSABLE );
     route.setChangeListener(this);
     route.setFont(Font.getDefault().derive(Font.BOLD, 16));
     route.setMargin(0,0,0,15);
     this.setLat(shop.getAddresslat());
     this.setLng(shop.getAddresslng());
     this.setAddress(shop.getStreetName());
      
      mapTable.add(route);
     // mapTable.add(routeLabel.RouteLabel(DouglasConstants.Route,Field.FOCUSABLE));
      detailTable.add(mapTable);
        
    //**Parking Info
    
         LabelField offnungszeiten=new LabelField(DouglasConstants.Offnungszeiten);
         infoTable.add(offnungszeiten);
         EventLabel openingtext=new EventLabel(shop.getRegularOpeningsText(),LabelField.FOCUSABLE);
         openingtext.setFont(DouglasConstants.smfont);
         infoTable.add(openingtext);
         if(shop.getParkingInfo()!=null)
         {
         LabelField parkingInfoLabel=new LabelField(DouglasConstants.Parkmöglichkeiten);
         parkingInfoLabel.setPadding(10,0,0,0);
         infoTable.add(parkingInfoLabel);
         EventLabel parkingInfo=new EventLabel(shop.getParkingInfo(),LabelField.FOCUSABLE);
         parkingInfo.setFont(DouglasConstants.smfont);
         infoTable.add(parkingInfo);
         }
         detailTable.add(infoTable);
       
       
         groupTable.add(detailTable);
        
        
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
   
 public void BeautyScreenLayout(final WebDataCallback callback) 
    {
       
       try
        {
           
         if(flag==1)
         {
             event=EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getEvents();
          }else
          {
            event=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getEvents();
          } 
             TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, 0);
        
           TableLayoutManager detailTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.USE_PREFERRED_SIZE
            },Field.FIELD_VCENTER);
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
            },new int[]{(Display.getWidth()/2-20)},0,Field.FIELD_RIGHT);
           TableLayoutManager inTable = new TableLayoutManager(new int[]
           {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
            },0);
         TitleLabel streetName=new TitleLabel(DouglasConstants.BeautyServices,LabelField.FOCUSABLE);
         streetName.setFont(DouglasConstants.mediumfont);
         innerTable.add(streetName);
      
         for(int i=0;i<event.length;i++)
             {
                
                 Integer type=Integer.valueOf(event[i].getType());
               if(type.intValue()==2)
               {
                 Bitmap IC_LOGO = Bitmap.getBitmapResource(DouglasConstants.ic);
                 BitmapField imgic = new BitmapField(IC_LOGO);
                 imgic.setPadding(5,0,5,0);
                 detailTable.add(imgic); 
                 Integer appointment=Integer.valueOf(event[i].getAppointment());  
                 EventLabel townLabel =appointment.intValue()==1? new EventLabel(event[i].getTopic()+"*",LabelField.FIELD_LEFT): new EventLabel(event[i].getTopic(),LabelField.FIELD_LEFT);
               
                 townLabel.setFont(DouglasConstants.mediumfont);
                 townLabel.setPadding(5,0,5,0);
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
        descriptionLabel.setFont(DouglasConstants.mediumfont);
        EventLabel teleLabel = new EventLabel(DouglasConstants.Rufen,LabelField.FOCUSABLE);
        teleLabel.setFont(DouglasConstants.mediumfont);
        infoTable.add(descriptionLabel);
        infoTable.add(teleLabel);
        
        Bitmap IMG = Bitmap.getBitmapResource(DouglasConstants.HandSet);
        BitmapField calimg = new BitmapField(IMG);
        inTable.add(calimg);
        int defaultTextHeight = Math.min(Display.getHeight() / 15, 15);
        Font myFont = FontFamily.forName("BBAlpha Sans").getFont(Font.PLAIN, defaultTextHeight);
        String labelEmailText = flag==1?EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop().getTelefon():PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop().getTelefon();
        int[] offsets = {0, labelEmailText.length()};
        Font[] fonts = {myFont};
        byte[] attributes = {0};
     
        ARichTextField phnno=new ARichTextField();
       // telno.setPadding(4,0,0,0);
        inTable.add(phnno.ARichTextField(labelEmailText,offsets, attributes, fonts, null, null,FIELD_LEFT | FOCUSABLE | (long)ActiveRichTextField.USE_TEXT_WIDTH,0)); 
     
        infoTable.add(inTable);
        outerTable.add(infoTable);
       
        groupTable.setMargin(15,10,0,10);
        groupTable.add(outerTable);
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
   public void EventTypeScreenLayout(final WebDataCallback callback) 
    {
       
     try
     {
        if(screen==1)
        {  
          event=EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getEvents();
        }else
        {
          event=PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getEvents();  
        }
        for(int i=0;i<event.length;i++)
        {
            Integer type=Integer.valueOf(event[i].getType());
           
           if(type.intValue()==3 || type.intValue()==1)
           {
             eventVector.addElement(new Event(event[i].getEventId(),event[i].getTopic(),event[i].getType(),event[i].getAppointment(),event[i].getSchedule(),event[i].getScheduleShort(), event[i].getText(),event[i].getTeasertText(),event[i].getTeaserImage(),event[i].getDistance(),event[i].getExclusive()));
          
           }
        }        
         eventdetail=vectorToEventArray(eventVector);
         
            TableLayoutManager groupTable = new TableLayoutManager(new int[]
            {
                  TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, 0);
        
              TableLayoutManager outerTable = new TableLayoutManager(new int[]
            {
               TableLayoutManager.USE_PREFERRED_SIZE,
               TableLayoutManager.SPLIT_REMAINING_WIDTH
             }, new int[]{Display.getWidth() + 50, 50},10,Field.FIELD_HCENTER);
            
             TableLayoutManager innerTable = new TableLayoutManager(new int[]
             {
               TableLayoutManager.FIXED_WIDTH
             },new int[] {Display.getWidth()/2},0,0);
               int len=(eventdetail.length)-1;
               int cnt=eventdetail.length;
               if(cnt==0)
               {
                  TitleLabel errorLabel = new TitleLabel("Leider haben wir für diese Parfümerie keine Events gefunden.",LabelField.FIELD_LEFT); 
                  errorLabel.setFont(DouglasConstants.mfont);
                  groupTable.add(errorLabel);
                  groupTable.setMargin(15,10,0,10);
                  vfm.add(groupTable);
               }
               int j = flag>0?flag:0;
            
             TitleLabel topicLabel = new TitleLabel(eventdetail[j].getTopic(),LabelField.FIELD_LEFT);
             LabelField shedullbl=new LabelField(DouglasConstants.Space);
             
             TitleLabel scheduleLabel = new TitleLabel(eventdetail[j].getSchedule()+DouglasConstants.Hypen+eventdetail[j].getScheduleShort(),LabelField.FIELD_LEFT);
             topicLabel.setFont(DouglasConstants.titlefont);
             scheduleLabel.setFont(DouglasConstants.smfont);
             innerTable.add(topicLabel);
             innerTable.add(shedullbl);
             innerTable.add(scheduleLabel);
       
        
             outerTable.add(innerTable);
             
             //Add picture
             String url = eventdetail[j].getTeaserImage();
             BitmapField img = new WebBitmapField(url, 2);
             outerTable.add(img);  
           
             groupTable.add(outerTable);
             LabelField newsp=new LabelField(DouglasConstants.Space);
             groupTable.add(newsp);
             
             
             RichTextLabel descriptionLabel = new RichTextLabel(eventdetail[j].getText(),(DrawStyle.HFULL |  RichTextField.FIELD_VCENTER | RichTextField.NO_NEWLINE));
         
             descriptionLabel.setFont(DouglasConstants.mediumfont);
             groupTable.add(descriptionLabel);
             
             groupTable.setMargin(15,10,0,10);
             vfm.add(groupTable);
             this.setID(j);
             DouglasFooter douglasFT = new DouglasFooter();
             PrevNext prevNextButton = new PrevNext();
         
             VerticalFieldManager vfmStatus = new VerticalFieldManager();
             if(screen==1)
             {
                vfmStatus.add(prevNextButton.PrevNext(DouglasConstants.MaternusEventScreenNumber,j,len,0)); 
             }else
             {
                vfmStatus.add(prevNextButton.PrevNext(DouglasConstants.MaternusScreenNumber,j,len,0));
             }
             vfm.add(vfmStatus);
           
       
        }catch (Exception e){}
        
        UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                callback.callback(vfm);
            }
        });
   }
    public static Event[] vectorToEventArray(Vector vector)
    {
        Event[] array =  new Event[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Event) vector.elementAt(i);
        }
        return array;
    }  
    public int getID()
       {
           return ID;
       }
       public void setID(int _id)
       {
           ID=_id;
        }
        public String getLat()
       {
           return Lat;
       }
       public void setLat(String _lat)
       {
           Lat=_lat;
        }
         public String getLng()
       {
           return Lng;
       }
       public void setLng(String _lng)
       {
           Lng=_lng;
        }
          public String getAddress()
       {
           return Add;
       }
       public void setAddress(String _add)
       {
           Add=_add;
       }

 
              public void fieldChanged(Field field, int context)
              {
                 if (field instanceof CustomButtonField)
                  {  
                    if(field==route)
                    {
                       UiApplication.getUiApplication().pushScreen(new RouteScreen());   
                    }
                 }
           
             }
           
  
  
}  

