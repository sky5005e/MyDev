/*
 * TitleLable.java
 *
 * © One-Associates Technologies Pvt Ltd., 2011
 *  info@one-associates.com
 */

package res.layout;
import net.rim.device.api.ui.Font;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.Graphics;
import com.douglas.common.DouglasConstants;


/**
 * Class defined for Title Label
 */

public class TitleLabel extends LabelField 
{
    public TitleLabel(String _text,long style) 
    {    
        super(_text,style);
        this.setText(_text);
        
    }
    
    protected void drawFocus(Graphics g, boolean on) {
        //do nothing
    }
   

    public void paint(Graphics g)
    {
          String hexcolor = DouglasConstants.hexTitleText; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
          g.setColor(hexValue);
       //   g.setFont(DouglasConstants.menufont);
          super.paint(g);
    }
} 



