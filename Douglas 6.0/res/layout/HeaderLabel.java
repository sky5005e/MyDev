/*
 * HeaderLabel.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import com.douglas.common.*;
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

/**
 * 
 */
public class HeaderLabel extends MainScreen implements FieldChangeListener
 {
   private TouchBitmapMenuField  rectangle; 
   private static ButtonField menubtn;
   private FontFamily fontFamily1;
   
   public VerticalFieldManager HeaderLabel(String _headerTxt,int Flag, EncodedImage _menuOn,EncodedImage _menuOff, MainScreen screen)
    {   
          
          VerticalFieldManager vfm = new VerticalFieldManager(VerticalFieldManager.LEAVE_BLANK_SPACE);
          Bitmap DOUGLAS_LOGO = Bitmap.getBitmapResource(DouglasConstants.DouglasLogoSmall);
          BitmapField douglasBmpField = new BitmapField(DOUGLAS_LOGO,BitmapField.FOCUSABLE);
          vfm.add(douglasBmpField);
          Background bg = BackgroundFactory.createBitmapBackground(Bitmap.getBitmapResource(DouglasConstants.MenuBackground));
       
          HorizontalFieldManager hfmnew=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH);
          fontFamily1 = Font.getDefault().getFontFamily();
          int txtMargin = 0;
          if (Flag==0)
          {
                TouchBitmapMenuField rectangle= new TouchBitmapMenuField(_menuOn.getBitmap(),_menuOff.getBitmap(),0,BitmapField.FOCUSABLE);
                //CustomButtonField  rectangle = new CustomButtonField(_menuTxt, CustomButtonField.RECTANGLE, Field.FOCUSABLE);
                rectangle.setChangeListener(this);
               
                rectangle.setMargin(4,5,4,5);
                hfmnew.add(rectangle);
                txtMargin = rectangle.getPreferredWidth();
          }
          LabelField label=new LabelField(_headerTxt, Field.USE_ALL_WIDTH | DrawStyle.LEFT)
          {
              public void paint(Graphics g)
              {
                  g.setColor(Color.WHITE);
                 
                   super.paint(g);
                }
            };
           
          
           Font font1 = fontFamily1.getFont(Font.BOLD,18);
           label.setFont(font1);
           if(Flag==0)
           {
           label.setMargin(9,0,10,(Display.getWidth()/2 - txtMargin - 30 - label.getPreferredWidth()/2-35));
           }else
           {
           label.setMargin(9,0,10,(Display.getWidth()/2 - txtMargin - 10 - label.getPreferredWidth()/2+10));
            }
           hfmnew.setBackground(bg);
           hfmnew.setPadding(5,0,2,0);
           hfmnew.add(label);
           vfm.add(hfmnew);
         
           return vfm;  
        
   }
   
   
   public void fieldChanged(Field field, int context) 
   {
        if (field instanceof TouchBitmapMenuField) {
             UiApplication.getUiApplication().getActiveScreen().close();
        }
   }
  

}


    
