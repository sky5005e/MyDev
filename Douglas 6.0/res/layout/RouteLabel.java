/*
 * RouteLabel.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */


package res.layout;

import com.douglas.main.*;
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


/**
 * 
 */
 
 public class RouteLabel extends LabelField {
        public RouteLabel(){super();}
    public RouteLabel(String string, long style) {
      super(string, style);    
         }
    public RouteLabel(String string){
           super(string);
     }
    protected boolean navigationClick(int status, int time) {
      fieldChangeNotify(0);
        return true;
       }
    protected void fieldChangeNotify(int context){
         if(context == 0){
             try {
         this.getChangeListener().fieldChanged(this, context);
              } catch (Exception e){}
           }
         }
 }
/*public class RouteLabel extends  HorizontalFieldManager implements FieldChangeListener{
   private LabelField route;
   private String text;
   private long style;
   public HorizontalFieldManager RouteLabel(String _text,long _style){
         
       text = _text;
      
       style=_style;
      
       //Add Footer
      
        HorizontalFieldManager hfmnew=new HorizontalFieldManager();
     
      
      
      
          LabelField route = new LabelField(text,FOCUSABLE | FIELD_LEFT  );
          route.setChangeListener(this);
          route.setMargin(0,0,0,25);
          hfmnew.add(route);
          
     
         
    // hfmnew.setMargin(4,0,0,0);
       return hfmnew;
    
}
 
    public boolean navigationClick(int status, int time) {
  if ((status & KeypadListener.STATUS_TRACKWHEEL) == KeypadListener.STATUS_TRACKWHEEL) {
      fieldChangeNotify(0);  
            return true;
   //Input came from the trackwheel
   } else if ((status & KeypadListener.STATUS_FOUR_WAY) == KeypadListener.STATUS_FOUR_WAY) {
   //Input came from a four way navigation input device
    fieldChangeNotify(0);  
            return true;
  }
return super.navigationClick(status, time); } 
    
        public void fieldChanged(Field field, int context)
        {
              if(field==route)
               {
                   UiApplication.getUiApplication().pushScreen(new RouteScreen());
               }
              
        }
    
} */
