/*
 * EventLabel.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import net.rim.device.api.ui.Font;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.Graphics;
import com.douglas.common.DouglasConstants;




/**
 * 
 */
public class EventLabel extends LabelField {
    
  public EventLabel(String _text,long style) 
  {
      super(_text,style);
      this.setText(_text); 
  
   }
    
  
    public void paint(Graphics g)
    {
          String hexcolor = DouglasConstants.hexBlack; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
          g.setColor(hexValue);
          super.paint(g);
    }
} 
