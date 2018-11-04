/*
 * StoreHeaderLabel.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import com.douglas.common.*;
import com.douglas.main.*;
import net.rim.device.api.ui.component.LabelField;
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
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.decor.Background;
import net.rim.device.api.ui.decor.BackgroundFactory;
import com.douglas.common.DouglasConstants;


public class StoreHeaderLabel extends MainScreen{
  
   private static LabelField label;
   private static TouchBitmapMenuField rectangle;
   private static EncodedImage ei;
   private static TouchBitmapMenuField menu;
   private FontFamily fontFamily1;
   public static String url;
   public static String URL;
   public static String StoredUrl;
   public int n;
   public VerticalFieldManager StoreHeaderLabel(EncodedImage _LeftMenuOn,EncodedImage _LeftMenuOff,String _headerTxt,EncodedImage _RightMenuOn,EncodedImage _RightMenuOff, int flag,MainScreen screen,String url,int n) 
   {  
          VerticalFieldManager vfm = new VerticalFieldManager(VerticalFieldManager.LEAVE_BLANK_SPACE);
          Bitmap DOUGLAS_LOGO = Bitmap.getBitmapResource(DouglasConstants.DouglasLogoSmall);
          BitmapField douglasBmpField = new BitmapField(DOUGLAS_LOGO);
          vfm.add(douglasBmpField);
          Background bg = BackgroundFactory.createBitmapBackground(Bitmap.getBitmapResource(DouglasConstants.MenuBackground));
          HorizontalFieldManager hfmnew=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH );
          fontFamily1 = Font.getDefault().getFontFamily();
          int txtMargin = 0;
          n=n;
          setURL(url);
          if(_LeftMenuOn.getBitmap()!=null)
          {
              
          TouchBitmapMenuField menu= new TouchBitmapMenuField(_LeftMenuOn.getBitmap(),_LeftMenuOff.getBitmap(),0,BitmapField.FOCUSABLE);  
          if(n==0)
              {
          menu.setChangeListener(new FieldChangeListener() {
            public void fieldChanged(Field field, int context) {
             StoredUrl=getURL();
             UiApplication.getUiApplication().pushScreen(new SearchMapScreen());
            }
         });
           }else
           {
               menu.setChangeListener(new FieldChangeListener() {
              public void fieldChanged(Field field, int context) {
               StoredUrl=getURL();
               UiApplication.getUiApplication().pushScreen(new finder());
             }
            });
           }
          menu.setMargin(4,0,4,5);
          hfmnew.add(menu);
          txtMargin = txtMargin+menu.getPreferredWidth();
          }
          
          
          int labelMargin=0;
          if(_headerTxt!=DouglasConstants.NONE)
          {
          LabelField label=new LabelField(_headerTxt)
          {
              public void paint(Graphics g)
              {
                  g.setColor(Color.WHITE);
                   super.paint(g);
               }
            };
            Font font1 = fontFamily1.getFont(Font.BOLD,18);
           label.setFont(font1);
           label.setMargin(9,0,10,((Display.getWidth()/2-txtMargin/2))/2+(Display.getWidth()/2)/40);
           hfmnew.add(label);
           labelMargin =labelMargin+(_LeftMenuOn.getWidth()+label.getPreferredWidth());
           
          }
       
           TouchBitmapMenuField rectangle= new TouchBitmapMenuField(_RightMenuOn.getBitmap(),_RightMenuOff.getBitmap(),0,BitmapField.FOCUSABLE);
           rectangle.setChangeListener(new FieldChangeListener() {
            public void fieldChanged(Field field, int context) {
            
              UiApplication.getUiApplication().getActiveScreen().close();
            }
         });
           rectangle.setMargin(4,0,4,(((Display.getWidth())-labelMargin- rectangle.getPreferredWidth()/2)/2)-Display.getWidth()/8);
          
           hfmnew.add(rectangle);
      
           hfmnew.setBackground(bg);
          
           hfmnew.setPadding(5,0,2,0);
           vfm.add(hfmnew);
         
           return vfm; 
          
      }
 
      public String getURL()
         {
             return URL;
         }
         
         public void setURL(String _url)
         {
             URL = _url;
            
         }
}

