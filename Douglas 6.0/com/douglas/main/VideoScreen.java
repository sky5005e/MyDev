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
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.*;
import net.rim.blackberry.api.browser.Browser;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import javax.microedition.io.Connector;
import javax.microedition.io.HttpConnection;
import javax.microedition.io.StreamConnection;
import javax.microedition.lcdui.List;
import net.rim.device.api.io.IOCancelledException;

import javax.microedition.media.Player;
import java.io.IOException;

public class VideoScreen{
    public static LooksTVSpots[] lookstvspotsMakeUp;
    public static LooksTVSpots[] lookstvspotsTV;
    private LooksTVSpots[] lookstvspots;
    public static int selectedIndex;
    public static int selectedTab;
    private String VideoLink;
    private LabelField dataField;
    private int flag;
    private VerticalFieldManager vfm = new VerticalFieldManager();
    private RichTextField _myRTF;
    private ByteArrayOutputStream _outStream;
    private TableLayoutManager _tlm;
    
    public void VideoScreen(String _VideoLink,int _flag)
    {
        VideoLink = _VideoLink;
        flag = _flag;
    }
    
    public void VideoScreenLayout(final WebDataCallback callback) 
    {
       
       try
        {
            if (flag == 0)
           {
                lookstvspotsMakeUp = NetworkUtilities.fetchLooksTVSpots(VideoLink);
                lookstvspots = new LooksTVSpots[lookstvspotsMakeUp.length];
                lookstvspots = lookstvspotsMakeUp;
           }
           else
           {
                lookstvspotsTV = NetworkUtilities.fetchLooksTVSpots(VideoLink);
                lookstvspots = new LooksTVSpots[lookstvspotsTV.length];
                lookstvspots = lookstvspotsTV;
           }
            
           
           for (int i = 0; i < lookstvspots.length; i++)
            {
                 TableLayoutManager outerTable = new TableLayoutManager(new int[]
                 {
                     TableLayoutManager.USE_PREFERRED_SIZE,
                     TableLayoutManager.SPLIT_REMAINING_WIDTH,
                     TableLayoutManager.USE_PREFERRED_SIZE
                 },0);
             
                TableLayoutManager innerTable = new TableLayoutManager(new int[]
                {
                     TableLayoutManager.FIXED_WIDTH
                },new int[] {Display.getWidth()/2+20},0,Field.FIELD_TOP);
       
             
                TitleLabel titleField = new TitleLabel(lookstvspots[i].getTitle(),LabelField.FOCUSABLE | Field.FIELD_VCENTER){
                     protected void layout(int width, int height){
                      super.layout(320, 20);
                      setExtent(320, 20);
                    }
                    };
                titleField.setFont(DouglasConstants.mfont);
                String desc=lookstvspots[i].getDescription();
                int n=desc.length();
                SubTitleLabel dataField = new SubTitleLabel(lookstvspots[i].getDescription(),(LabelField.FOCUSABLE | Field.FIELD_VCENTER | LabelField.USE_ALL_HEIGHT ));
               
               
                /*SubTitleLabel dataField = new SubTitleLabel(lookstvspots[i].getDescription(),(DrawStyle.VALIGN_MASK | DrawStyle.ELLIPSIS | LabelField.FOCUSABLE | Field.FIELD_VCENTER | LabelField.USE_ALL_HEIGHT ));
                RichTextField _myRTF = new RichTextField(lookstvspots[i].getDescription(), (DrawStyle.HFULL.ELLIPSIS |  RichTextField.FIELD_VCENTER |
                RichTextField.FOCUSABLE | RichTextField.NO_NEWLINE));*/
                dataField.setFont(DouglasConstants.smfont);
                
                innerTable.add(titleField);
                innerTable.add(dataField);
               
                 //Add picture
                String url=lookstvspots[i].getPreviewpictureurl();
                BitmapField img = new WebBitmapField(url,1);
                outerTable.add(img);
            
                TitleLabel lengthField = new TitleLabel(lookstvspots[i].getVlength() + DouglasConstants.MINUTETEXT,LabelField.FOCUSABLE | LabelField.FIELD_VCENTER);
                lengthField.setFont(DouglasConstants.msfont);
                
                outerTable.add(innerTable);
              
                outerTable.add(lengthField);
             
                
                String hex = (i % 2 == 0) ? DouglasConstants.hexWhite:DouglasConstants.hexLightBlue;
                int intValue = Integer.parseInt(hex, 16); 
                Background bg = BackgroundFactory.createSolidBackground(intValue);
                outerTable.setBackground(bg);
                
                outerTable.setID(i);
                outerTable.setChangeListener(itemlistener);
                outerTable.setPadding(5,4,5,4);
                vfm.add(outerTable);
            if(i==10)
            {
                break;
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
                     _tlm = (TableLayoutManager) field;
                
                      selectedIndex = _tlm.getID();
                      selectedTab = _tlm.getFlag();
                       UiApplication.getUiApplication().pushScreen(new VideoPopupScreen());
                   /*   VideoPopupScreen videoscreen=new VideoPopupScreen();
                      //selectedIndex = _tlm.getID();
                    UiApplication.getUiApplication().invokeAndWait( new Runnable() 
                    {  
                        public void run() 
                        { 
                      
                          int response = Dialog.ask( Dialog.D_YES_NO, "Video jetzt abspielen?.\nDurch das Abspielen der Videos konnen moglicherweise weitere Kosten fur Sie entstehen.Bitte uberprufen Sie Ihren Datentarif!" ); // waits here for user interaction 
                          if ( response == Dialog.YES ) 
                         { 
                          
                           // UiApplication.getUiApplication().pushScreen(new VideoPlaybackScreen(lookstvspots[_tlm.getID()].getVideourl()));
                          Browser.getDefaultSession().displayPage(lookstvspots[_tlm.getID()].getVideourl());
                           
                         } else 
                         { 
                            
                         } 
                        } 
                     } );*/
                     

              }
      };

} 








