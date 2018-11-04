/*
 * SubTitleLabel.java
 *
 * © One-Associates Technologies Pvt Ltd., 2011
 *  info@one-associates.com
 */

package res.layout;
import net.rim.device.api.ui.Font;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.Graphics;
import com.douglas.common.DouglasConstants;
import net.rim.device.api.ui.Color;
import net.rim.device.api.ui.*;


/**
 * Class defined for Sub Title Label
 */

public class SubTitleLabel extends LabelField 
{
    public SubTitleLabel(String _text,long style) 
    {    
        super(_text,style);
        this.setText(_text);
        
    }
     public boolean isFocusable()
     {         
        return true;  
          
     }  
    
     protected void drawFocus(Graphics g, boolean on) {
        
         XYRect focusRect = new XYRect();
         getFocusRect( focusRect );
         int yOffset = 0;
         if ( isSelecting() )
        {

           yOffset = focusRect.height >> 1;
           focusRect.height = yOffset;
           focusRect.y += yOffset;
        }
       g.pushRegion( focusRect.x, focusRect.y, focusRect.width+100, focusRect.height+100, -focusRect.x, -focusRect.y );
      
       String hexcolor = "DDEFF3"; 
       int hexValue = Integer.parseInt(hexcolor, 16); 
       g.setBackgroundColor(hexValue);
       g.setColor(hexValue);
       g.clear();
       this.paint( g );
       g.popContext();
       } 
      protected void onFocus(int direction) { 
      
        super.onFocus(direction);  
        invalidate();  
    }  
      
    protected void onUnfocus() {  
   
        super.onUnfocus();  
        invalidate();  
    }
    public void paint(Graphics g)
    {
          String hexcolor = DouglasConstants.hexSubTitleText; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
          g.setColor(hexValue);
          super.paint(g);
    }
} 



