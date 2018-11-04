/*
 * TextLabel.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import net.rim.device.api.ui.Font;
import net.rim.device.api.ui.component.EditField;
import net.rim.device.api.ui.Graphics;
import com.douglas.common.DouglasConstants;
import net.rim.device.api.ui.Color;
import net.rim.device.api.ui.*;
import net.rim.device.api.system.*; 


/**
 * 
 */
public class TextLabel extends EditField {
   
  public  TextLabel(String _text,String label,int n,long style) 
  {
      super( _text,label,n,style); 
     
     // this.setText(label);
  }
   
  

    
        

  protected boolean keyChar(char key, int status, int time)
{
      switch (key)
      {
          case Characters.BACKSPACE:
          {
                this.setText("");
                 return true;
          }
      }
      return super.keyChar(key, status, time);
}
      protected void onFocus(int direction) { 
      
        super.onFocus(direction);  
        invalidate();  
    }  
      
    protected void onUnfocus() {  
   
        super.onUnfocus();  
        invalidate();  
    }  
     protected void paintBackground( Graphics g )
     {
       
        if(isFocus())
      {
         g.setBackgroundColor(Color.WHITE);
         g.clear();
         super.paint(g);
      }else
      { 
         
         g.setBackgroundColor(Color.WHITE);
         g.clear();
         super.paint(g);
     
      }
      
      }
       
  
} 



