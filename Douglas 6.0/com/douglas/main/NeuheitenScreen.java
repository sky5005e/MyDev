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
import net.rim.device.api.ui.container.*;

public class NeuheitenScreen extends MainScreen
{
   
    public static Neuheiten[] neuheiten;
    public static int selectedIndex;
    private WebVerticalFieldManager wvfm = new WebVerticalFieldManager();
    private WebVerticalFieldManager twvfm = new WebVerticalFieldManager();
    private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
    private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);  
    public  NeuheitenScreen()
    {
           
            HeaderLabel HL=new HeaderLabel();
            VerticalFieldManager vfm = HL.HeaderLabel(DouglasConstants.NEWSMenu,0,menuOn,menuOFF,this);
            Bitmap bkimg = Bitmap.getBitmapResource(DouglasConstants.BackgroundBright);
            BitmapField bkimge = new BitmapField(bkimg);
            vfm.add(bkimge);
            setBanner(vfm); 
            DouglasFooter douglasFT = new DouglasFooter();
            VerticalFieldManager vfmStatus = new VerticalFieldManager();
            vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.NEWSMenu));
            setStatus(vfmStatus);
            add(wvfm);
            
            Thread t = new Thread(new Runnable()
            {
                
                    public void run()
                    {
                        try{
                                neuheiten = NetworkUtilities.fetchNeuheiten(DouglasConstants.NeuheitenURL);
                        }catch (Exception e){}
                           
                            NeuheitenScreenLayout(wvfm);
                          
                            
                    }
            });
            
            t.start();
        
    }
     
    public void NeuheitenScreenLayout(final WebDataCallback callback){     
        
        try
        {
            neuheiten=NetworkUtilities.fetchNeuheiten(DouglasConstants.NeuheitenURL);
              if(neuheiten.length==0)
            {
                TitleLabel parseError=new TitleLabel("Es tut uns leid,z.Z.sind keine Daten verfugbar",LabelField.READONLY);
                parseError.setFont(DouglasConstants.smfont);
                twvfm.add(parseError);
            }
            for (int i = 0; i < neuheiten.length; i++)
            {
                 TableLayoutManager outerTable = new TableLayoutManager(new int[]
                {
                        TableLayoutManager.USE_PREFERRED_SIZE,
                        TableLayoutManager.USE_PREFERRED_SIZE,
                        TableLayoutManager.SPLIT_REMAINING_WIDTH
                },TableLayoutManager.FOCUSABLE);
                
                TableLayoutManager innerTable = new TableLayoutManager(new int[]
                {
                        TableLayoutManager.USE_PREFERRED_SIZE
                },Manager.USE_ALL_WIDTH);
                VerticalFieldManager vfmt=new VerticalFieldManager(VerticalFieldManager.USE_ALL_WIDTH | VerticalFieldManager.FOCUSABLE | VerticalFieldManager.HIGHLIGHT_FOCUS);
                TitleLabel titleField=new TitleLabel(neuheiten[i].getBrand(),LabelField.READONLY);
                titleField.setFont(DouglasConstants.titlefont);
                titleField.setMargin(4,0,0,0); 
                vfmt.add(titleField);
              
                SubTitleLabel dataField = new SubTitleLabel(neuheiten[i].getProduct(), LabelField.READONLY);
                dataField.setFont(DouglasConstants.mediumfont);
                dataField.setMargin(4,0,0,0);
                vfmt.add(dataField);
                SubTitleLabel descriptionField = new SubTitleLabel(neuheiten[i].getCategory(), LabelField.READONLY);
                descriptionField.setFont(DouglasConstants.mediumfont);
                descriptionField.setMargin(4,0,0,0);
                vfmt.add(descriptionField);
                LabelField spacenew=new LabelField(DouglasConstants.Space);
               
               //prepare inner table
                innerTable.add(spacenew);
                innerTable.add(vfmt);
           
               //Outer Table 
             
                BitmapField img =new WebBitmapField(neuheiten[i].getPictureurl(),1);
                
                img.setPadding(3,18,3,15);
                outerTable.add(img);
                
             
                outerTable.add(innerTable);
            
              
                String hex = (i % 2 == 0) ? DouglasConstants.hexWhite:DouglasConstants.hexLightBlue;
                int intValue = Integer.parseInt(hex, 16); 
                Background bg = BackgroundFactory.createSolidBackground(intValue);
                outerTable.setBackground(bg);
              
                outerTable.setID(i);
                
                outerTable.setChangeListener(itemlistener);   
                
                twvfm.add(outerTable);
           
            }
   
        }
        catch (Exception e){
            
            }
         UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                callback.callback(twvfm);
            }
        });
    }
     
    FieldChangeListener itemlistener = new FieldChangeListener() {
 
               public void fieldChanged(Field field, int context)
             {
                      TableLayoutManager _tlm = (TableLayoutManager) field;
                      selectedIndex = _tlm.getID();
                      UiApplication.getUiApplication().pushScreen(new NeuheitenDetailScreen(neuheiten.length - 1));
                        
               }
      };
    
}
