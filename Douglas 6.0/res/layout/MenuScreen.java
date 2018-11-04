/*
 * MenuScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import  com.douglas.common.*;
import  com.douglas.main.*;
import res.layout.*;
import java.io.*;
import com.douglas.common.TableLayoutManager;
import com.douglas.common.TouchBitmapField;
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
import net.rim.device.api.ui.container.HorizontalFieldManager;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.system.*;



public class MenuScreen extends MainScreen implements FieldChangeListener
{
    private static EncodedImage menu1 = EncodedImage.getEncodedImageResource("res/drawable/parfumerie_events_button.png");  
    private static EncodedImage menu2 = EncodedImage.getEncodedImageResource("res/drawable/neuheiten_button.png");  
    private static EncodedImage menu3 = EncodedImage.getEncodedImageResource("res/drawable/hauttyptest_button.png");  
    private static EncodedImage menu4 = EncodedImage.getEncodedImageResource("res/drawable/top10_button.png");  
    private static EncodedImage menu5 = EncodedImage.getEncodedImageResource("res/drawable/looks_spots_button.png");  
    private static EncodedImage menu6 = EncodedImage.getEncodedImageResource("res/drawable/shop_button.png");  
    private static EncodedImage menu1on = EncodedImage.getEncodedImageResource(DouglasConstants.MenuStoreImageOn);  
    private static EncodedImage menu2on = EncodedImage.getEncodedImageResource(DouglasConstants.MenuShopImageOn);  
    private static EncodedImage menu3on = EncodedImage.getEncodedImageResource(DouglasConstants.MenuHauttyptestImageOn);  
    private static EncodedImage menu4on = EncodedImage.getEncodedImageResource(DouglasConstants.MenuTopTenImageOn); 
    private static EncodedImage menu5on = EncodedImage.getEncodedImageResource(DouglasConstants.MenuTVImageOn);  
    private static EncodedImage menu6on = EncodedImage.getEncodedImageResource(DouglasConstants.MenuNeuImageOn); 
    private TouchBitmapMenuField parfum;
    private TouchBitmapMenuField neuheiten;
    private TouchBitmapMenuField hauttyp;
    private TouchBitmapMenuField top10;
    private TouchBitmapMenuField videos;
    private TouchBitmapMenuField angebote;
    private static EncodedImage menuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageON);  
   private static EncodedImage menuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderMenuImageOFF);
    
    public  MenuScreen()
    {
        //Add Header
       
        HeaderLabel HL=new HeaderLabel();
        VerticalFieldManager vfm = HL.HeaderLabel(DouglasConstants.HomeMenu,1,menuOn,menuOFF,this);
        setBanner(vfm);
        VerticalFieldManager vfmStatus = new VerticalFieldManager();
        int intValue = Integer.parseInt(DouglasConstants.hexBackground, 16);
        Background bg = BackgroundFactory.createSolidBackground(intValue);
        
        // Menu Row 1 
         menu1on.setScale(2);
         menu2on.setScale(2);
         menu3on.setScale(2);
         menu4on.setScale(2);
         menu5on.setScale(2);
         menu6on.setScale(2);
         HorizontalFieldManager hf=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH | HorizontalFieldManager.FIELD_VCENTER); 
         menu1.setScale(2);menu2.setScale(2);menu3.setScale(2);menu4.setScale(2);menu5.setScale(2);menu6.setScale(2);
         parfum = new TouchBitmapMenuField(menu1.getBitmap(),menu1on.getBitmap(),1,BitmapField.FOCUSABLE);
         parfum.setChangeListener(this);
         parfum.setMargin(2,0,0,(Display.getWidth() - menu1.getWidth())/3);
         hf.add(parfum); 
         
         angebote = new TouchBitmapMenuField(menu6.getBitmap(),menu2on.getBitmap(),0,BitmapField.FOCUSABLE);
         angebote.setChangeListener(this);
         angebote.setMargin(0,0,0,(Display.getWidth() - menu1.getWidth())/3);
         hf.add(angebote);
         
         hf.setBackground(bg);
         
         
         // Menu Row 2
         HorizontalFieldManager hf1=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH | HorizontalFieldManager.FIELD_VCENTER); 
         hauttyp = new TouchBitmapMenuField(menu3.getBitmap(),menu3on.getBitmap(),0,BitmapField.FOCUSABLE);
         hauttyp.setChangeListener(this);
         hauttyp.setMargin(3,0,0,(Display.getWidth() - menu1.getWidth())/3);
         hf1.add(hauttyp);
         
         videos = new TouchBitmapMenuField(menu5.getBitmap(),menu5on.getBitmap(),0,BitmapField.FOCUSABLE);
         videos.setChangeListener(this);
         videos.setMargin(3,0,0,(Display.getWidth() - menu1.getWidth())/3);
         hf1.add(videos);
         
         hf1.setBackground(bg);
         
         
         // Menu Row 3
         HorizontalFieldManager hf2=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH | HorizontalFieldManager.FIELD_VCENTER); 
         neuheiten = new TouchBitmapMenuField(menu2.getBitmap(),menu6on.getBitmap(),0,BitmapField.FOCUSABLE);
         neuheiten.setChangeListener(this);
         neuheiten.setMargin(3,0,0,(Display.getWidth() - menu1.getWidth())/3);
         hf2.add(neuheiten);
         
         top10 = new TouchBitmapMenuField(menu4.getBitmap(),menu4on.getBitmap(),0,BitmapField.FOCUSABLE);
         top10.setChangeListener(this);
         top10.setMargin(3,0,0,(Display.getWidth() - menu1.getWidth())/3);
         hf2.add(top10);
        
         hf2.setBackground(bg);
         
         add(hf);
         add(hf1);
         add(hf2);
         
       
          //Add Footer 
          DouglasFooter douglasFT = new DouglasFooter();
          
          vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.HomeMenu));
          setStatus(vfmStatus);
       }
       
 
 public void fieldChanged(Field field, int context)
 {
    if(field==parfum)
     {
            UiApplication.getUiApplication().pushScreen(new finder());
          
     } else if(field==neuheiten)
     {
            UiApplication.getUiApplication().pushScreen(new NeuheitenScreen());
       
     } else  if(field==hauttyp)
     {
            UiApplication.getUiApplication().pushScreen(new SkinTestScreen());
       
     } else  if(field==top10)
     {
            UiApplication.getUiApplication().pushScreen(new TopTenTabScreen());
     
     } else  if(field==videos)
     {
            UiApplication.getUiApplication().pushScreen(new VideoTabScreen());
     
     } else  if(field==angebote)
     {
            UiApplication.getUiApplication().pushScreen(new ShopScreen());
     } 
               
 }      


}


