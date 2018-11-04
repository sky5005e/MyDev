/*
 * AngeboteActivity.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */


package res.layout;
import com.douglas.common.*;
import com.douglas.main.*;
import net.rim.device.api.ui.*;
import com.douglas.common.TableLayoutManager;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.ui.decor.Background;
import net.rim.device.api.ui.decor.BackgroundFactory;

import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.Bitmap; 

public class DouglasHeader  {
    
    private static LabelField label;
       
      public  DouglasHeader(MainScreen screen ){
         
         VerticalFieldManager vfm = new VerticalFieldManager(VerticalFieldManager.LEAVE_BLANK_SPACE);
         Bitmap DOUGLAS_LOGO = Bitmap.getBitmapResource(DouglasConstants.DouglasLogoSmall);
         BitmapField douglasBmpField = new BitmapField(DOUGLAS_LOGO);
         vfm.add(douglasBmpField);
         Background bg = BackgroundFactory.createBitmapBackground(Bitmap.getBitmapResource(DouglasConstants.MenuBackground));
         HorizontalFieldManager hfmnew=new HorizontalFieldManager(HorizontalFieldManager.USE_ALL_WIDTH );
        
         LabelField label=new LabelField("Menü", Field.USE_ALL_WIDTH | DrawStyle.HCENTER)
          {
              public void paint(Graphics g)
              {
                  g.setColor(Color.WHITE);
                   super.paint(g);
                }
            };
          
         
         label.setMargin(5,0,5,5);
         hfmnew.setBackground(bg);
         hfmnew.add(label);
         vfm.add(hfmnew);
          
         screen.setBanner(vfm);  
            
      
         
}

}


