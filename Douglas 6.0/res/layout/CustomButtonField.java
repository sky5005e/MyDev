package res.layout;
import com.douglas.common.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.system.*;
import com.douglas.main.*;
import net.rim.device.api.ui.Manager; 

/*
 * CustomButtonField.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

/**
 * CustomButtonField - class which creates button fields of various shapes.  
 * Demonstrates how to create custom ui fields.
 */
public class CustomButtonField extends Field implements DrawStyle
{
    static final int RECTANGLE = 1;
    static final int TRIANGLE = 2;
    static final int OCTAGON = 3;
    static final int FIXED_WIDTH = 4;
    static final int FULLSCREEN = 5;
    static final int COLOUR_BACKGROUND = 6;
    public static String No;
    private String _label;
    private int _shape;
    private Font _font;
    private int _labelHeight;
    private int _labelWidth;

    /**
     * Constructs a button with specified label, and default style and shape.
     */
     public CustomButtonField()
     {
      }
    public CustomButtonField(String label) 
    {
        this(label, RECTANGLE, 0);
    }

    /**
     * Constructs a button with specified label and shape, and default style.
     */
    public CustomButtonField(String label, int shape) 
    {
        this(label, shape, 0);
    }

    /**
     * Constructs a button with specified label and style, and default shape.
     */
    public CustomButtonField(String label, long style) 
    {
        this(label, RECTANGLE, style);
        
    }

    public int getLabelWidth()
    {
        return _labelWidth;
    }
    /**
     * Constructs a button with specified label, shape, and style.
     */
    public CustomButtonField(String label, int shape, long style) 
    {
        super(style);
        
        _label = label;
        _shape = shape;
        _font = getFont();
        _labelHeight = _font.getHeight();
        _labelWidth = _font.getAdvance(_label);
    }

    /**
     * @return The text on the button
     */
    public String getText()
    {
        return _label;
    }
    public boolean isFocusable()
     {         
        return true;  
          
     }  
    /**
     * Field implementation.
     * @see net.rim.device.api.ui.Field#getPreferredWidth()
     */
    public int getPreferredWidth() 
    {
        switch(_shape)
        {
            case TRIANGLE:
                if (_labelWidth < _labelHeight) 
                {
                    return _labelHeight << 2;
                } 
                else 
                {
                    return _labelWidth << 1;
                }
                
            case OCTAGON:
                if (_labelWidth < _labelHeight) 
                {
                    return _labelHeight + 4;
                } 
                else 
                {
                    return _labelWidth + 8;
                }
                
            case FIXED_WIDTH:
                return (_font.getAdvance(" ")) * 35; // Always set to 35 spaces wide
                
            case FULLSCREEN:
                return Display.getWidth();
                
            default:
                return _labelWidth + 8;
            

        }
    }

    /**
     * Field implementation.
     * @see net.rim.device.api.ui.Field#getPreferredHeight()
     */
    public int getPreferredHeight() 
    {
        switch(_shape) 
        {
            case TRIANGLE:
                if (_labelWidth < _labelHeight) 
                {
                    return _labelHeight << 1;
                } 
                else 
                {
                    return _labelWidth;
                }
                
            case OCTAGON:
                return getPreferredWidth();
            
            default:
                return _labelHeight + 4;
        }
    }

    /**
     * Field implementation.
     * @see net.rim.device.api.ui.Field#drawFocus(Graphics, boolean)
     */
    protected void drawFocus(Graphics graphics, boolean on)
    {
        
        
    }
    
    /**
     * Field implementation.
     * @see net.rim.device.api.ui.Field#layout(int, int)
     */
    protected void layout(int width, int height) 
    {
        // Update the cached font - in case it has been changed.
        _font = getFont();
        _labelHeight = _font.getHeight();
        _labelWidth = _font.getAdvance(_label);

        // Calc width.
        width = Math.min( width, getPreferredWidth() );

        // Calc height.
        height = Math.min( height, getPreferredHeight() );

        // Set dimensions.
        setExtent( width, height );
    }

    /**
     * Field implementation.
     * @see net.rim.device.api.ui.Field#paint(Graphics)
     */
    protected void paint(Graphics graphics) 
    {
        int textX, textY, textWidth;
        int w = getWidth();
        int h = getHeight();
        String hex=DouglasConstants.CustomField;
        int intValue = Integer.parseInt(hex, 16); 
        String focus="42A9BA";
        int intFocus = Integer.parseInt(focus, 16); 
        switch(_shape) 
        {
            case TRIANGLE:
                h = (w>>1);
                int m = (w>>1)-1;
                
                graphics.drawLine(0, h-1, m, 0);
                graphics.drawLine(m, 0, w-1, h-1);
                graphics.drawLine(0, h-1, w-1, h-1);

                textWidth = Math.min(_labelWidth,h);
                textX = (w - textWidth) >> 1;
                textY = h >> 1;
                break;
                
            case OCTAGON:
                int x = 5*w/17;
                int x2 = w-x-1;
                int x3 = w-1;
                
                graphics.drawLine(0, x, 0, x2);
                graphics.drawLine(x3, x, x3, x2);
                graphics.drawLine(x, 0, x2, 0);
                graphics.drawLine(x, x3, x2, x3);
                graphics.drawLine(0, x, x, 0);
                graphics.drawLine(0, x2, x, x3);
                graphics.drawLine(x2, 0, x3, x);
                graphics.drawLine(x2, x3, x3, x2);

                textWidth = Math.min(_labelWidth, w - 6);
                textX = (w-textWidth) >> 1;
                textY = (w-_labelHeight) >> 1;
                break;

            case COLOUR_BACKGROUND:
               
                graphics.fillRect(0, 0, w, h);
                graphics.setColor(Color.BLACK);
                graphics.setFont(DouglasConstants.mediumfont);
                textX = 4;
                textY = 2;
                textWidth = w - 6;
                break;
                
            default:
                //graphics.fillRoundRect(0, 0, w, h , 8, 8);
                
                graphics.drawRoundRect(0, 0, w, h, 8, 8);
                if(isFocus())
                {
                graphics.setColor(Color.BLUE);
                }else
                {
                 graphics.setColor(Color.WHITE);
                }
                graphics.fillRoundRect(0, 0, w, h , 8, 8);
                graphics.setColor(Color.BLACK);
                
                graphics.setFont(DouglasConstants.mediumfont);
                textX = 4;
                textY = 2;
                textWidth = w - 6;
                break;
        }
        graphics.drawText(_label, textX, textY, (int)( getStyle() & DrawStyle.ELLIPSIS | DrawStyle.HALIGN_MASK ), textWidth );
        
    }
    
       public String getTelNo()
       {
           return No;
       }
       public void setTelNo(String _no)
       {
           No=_no;
        }
      protected void onFocus(int direction) { 
      
        super.onFocus(direction);  
        invalidate();  
    }  
     
    protected void onUnfocus() {  
   
        super.onUnfocus();  
        invalidate();  
    } 
    /**
     * Overridden so that the Event Dispatch thread can catch this event
     * instead of having it be caught here.
     * @see net.rim.device.api.ui.Field#navigationClick(int, int)
     */
   
   protected boolean touchEvent(TouchEvent message) 
    {
      if (TouchEvent.CLICK == message.getEvent()) 
         {
                  FieldChangeListener listener = getChangeListener();
                    if (null != listener)
                          listener.fieldChanged(this, 1);
                          super.setFocus();
         }
          
      return super.touchEvent(message);
 
    }
     protected boolean navigationClick(int status, int time) 
     {
       if ((status & KeypadListener.STATUS_TRACKWHEEL) == KeypadListener.STATUS_TRACKWHEEL) 
       {
           
           fieldChangeNotify(1);  
            return true;
           //Input came from the trackwheel
        } else if ((status & KeypadListener.STATUS_FOUR_WAY) == KeypadListener.STATUS_FOUR_WAY) 
        {
          //Input came from a four way navigation input device
            fieldChangeNotify(1);  
            return true;
         }
       return super.navigationClick(status, time);
       } 
       
    protected boolean keyChar(char character, int status, int time)
     {  
            if (character == Keypad.KEY_ENTER) 
            {  
                fieldChangeNotify(0);  
                return true;  
            }  
            return super.keyChar(character, status, time);  
        }
       
}

