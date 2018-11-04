/*
 * TabButtonField.java
 *
 * Research In Motion Limited proprietary and confidential
 * Copyright Research In Motion Limited, 2009-2009
 */

package com.douglas.main;
import com.douglas.common.*;
import com.douglas.main.TabButtonSet;

import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.*;
/**
 * A custom button field
 */
public class TabButtonField extends BaseButtonField
{
    private static final int CORNER_RADIUS = 0;
    
    public static int DRAWPOSITION_LEFT = 0;
    public static int DRAWPOSITION_RIGHT = 1;
    public static int DRAWPOSITION_MIDDLE = 2;
    public static int DRAWPOSITION_SINGLE = 3;
    
    public static final int COLOUR_BORDER              = 0xB5CBCB;
    public static final int COLOUR_BORDER_FOCUS        = 0xB5CBCB;
    public static final int COLOUR_BORDER_SELECTED     = 0xB5CBCB;
    public static final int COLOUR_TEXT                = 0xB5CBCB;
    public static final int COLOUR_TEXT_FOCUS          = 0x69C7D1;
    public static final int COLOUR_TEXT_SELECTED       = 0x69C7D1;
    public static final int COLOUR_BACKGROUND          = 0xC5EEF6;
    public static final int COLOUR_BACKGROUND_FOCUS    = 0xFFFFFF;
    public static final int COLOUR_BACKGROUND_SELECTED = 0xFFFFFF;

    private static final int XPADDING = Display.getWidth() <= 320 ? 2 : 3;
    private static final int YPADDING = Display.getWidth() <= 320 ? 2 : 3;
    
    private String _text;
    private Font _buttonFont;
    private boolean _pressed = false;
    private boolean _selected = false;
  //  public Graphics gg;
    //public Graphics ga;
    private int _width;
    private int _height;
    
    private int _drawPosition = -1;
    private Bitmap selectedImage;
    private Bitmap image;
    private int field;
    private int n;
    //HorizontalFieldManager hfm=new HorizontalFieldManager();
    public TabButtonField( Bitmap selectedImage,Bitmap image,int n,int field)
    {
        super( Field.FOCUSABLE );
        this.selectedImage = selectedImage;
        this.image=image;
        this.n=n;
        this.field=field;
      /*  if(n>0)
        {
             
            Graphics gg = new Graphics(image);
            gg.setColor(Color.ALICEBLUE);
            gg.setFont(DouglasConstants.vsmfont);
            gg.drawText("("+n+")",3,0);
            
          
            Graphics ga = new Graphics(selectedImage);
            ga.setColor(Color.ALICEBLUE);
            ga.setFont(DouglasConstants.vsmfont);
            ga.drawText("("+n+")",3,0);
            
        
        }else if( n<=1 && field==1)
        {
            Graphics gg = new Graphics(image);
            gg.setColor(Color.ALICEBLUE);
            gg.setFont(DouglasConstants.vsmfont);
            gg.drawText("("+n+")",3,0);
            
            paint(gg); 
            Graphics ga = new Graphics(selectedImage);
            ga.setColor(Color.ALICEBLUE);
            ga.setFont(DouglasConstants.vsmfont);
            ga.drawText("("+n+")",3,0);
            
            paint(ga);
        }*/
      //  
    }
   
    /**
     * DRAWPOSITION_LEFT | DRAWPOSITION_RIGHT | DRAWPOSITION_MIDDLE | DRAWPOSITION_SINGLE
     * Determins how the field is drawn (border corners)
     */
    public void setDrawPosition( int drawPosition )
    {
        _drawPosition = drawPosition;
    }
    
    public void setSelected( boolean selected )
    {
        _selected = selected;
        invalidate();
    }
    
    public int getPreferredWidth()
    {
        return 2 * XPADDING + _buttonFont.getAdvance( _text );   // not actually used
    }

    public int getPreferredHeight()
    {
        return 2 * YPADDING + _buttonFont.getHeight();
    }
        
    protected void layout( int width, int height )
    {
        _buttonFont = getFont();
        setExtent( width, getPreferredHeight());
        _width = getWidth();
        _height = getHeight();
    }
    
    protected void onUnfocus()
    {
        super.onUnfocus();
        if( _pressed ) {
            _pressed = false;
            invalidate();
        }
    }
    
    /**
     * A public way to click this button
     */
    public void clickButton() 
    {
        Manager manager = getManager();
        if( manager instanceof TabButtonSet ) {
            ( ( TabButtonSet ) manager ).setSelectedField( this );
        }
        super.clickButton();
    }
    
    protected boolean navigationClick(int status, int time) {
        _pressed = true;
        invalidate();
        return super.navigationClick( status, time );
    }
    
    protected boolean navigationUnclick(int status, int time) {
        _pressed = false;
        invalidate();
        return true;
    }
    
    protected void paint( Graphics g )
    {
        String hexcolor = DouglasConstants.EventCounts; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
       // int oldColour = g.getColor();
            
            //int foregroundColor;
            //MaternusTabScreen _mts=new MaternusTabScreen(0);
            if ( _pressed || _selected  ) {
              
                 if(n>0)
                 {
                  g.drawBitmap(0, 0, image.getWidth(), image.getHeight(), image, 0, 0);
                  g.drawBitmap(0, 0, image.getWidth(), image.getHeight(), image, 0, 0); 
                  g.setColor(hexValue);
                  g.setFont(DouglasConstants.smfont);
                  g.drawText( "("+n+")",11,5); 
                 }else if( n<=1 && field==1)
                 {
                   g.drawBitmap(0, 0, image.getWidth(), image.getHeight(), image, 0, 0);
                   g.drawBitmap(0, 0, image.getWidth(), image.getHeight(), image, 0, 0);  
                   g.setColor(hexValue);
                   g.setFont(DouglasConstants.smfont);
                   g.drawText( "("+n+")",11,5);
                 }else
                 {
                 g.drawBitmap(0, 0, image.getWidth(), image.getHeight(), image, 0, 0);
                 g.drawBitmap(0, 0, image.getWidth(), image.getHeight(), image, 0, 0); 
                }
            } 
          
     
     }
    
    protected void paintBackground( Graphics g )
    {    
          String hexcolor = DouglasConstants.EventCounts; 
          int hexValue = Integer.parseInt(hexcolor, 16); 
     if(n>0)
          {
            if(isFocus())
             {
                
                
                g.drawBitmap(0, 0, selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.drawBitmap(0, 0, selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.setColor(hexValue);
                g.setFont(DouglasConstants.smfont);
                g.drawText( "("+n+")",11,5);
              } else 
              {
               
                g.drawBitmap(0, 0,selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.drawBitmap(0, 0,selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.setColor(hexValue);
                g.setFont(DouglasConstants.smfont);
                g.drawText( "("+n+")",11,5);
               }
        }else if( n<=1 && field==1)
        {
           if(isFocus())
             {
                
                g.drawBitmap(0, 0, selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.drawBitmap(0, 0, selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.setColor(hexValue);
                g.setFont(DouglasConstants.smfont);
                g.drawText( "("+n+")",11,5);
              } else
              {
                
                g.drawBitmap(0, 0,selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.drawBitmap(0, 0,selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.setColor(hexValue);
                g.setFont(DouglasConstants.smfont);
                g.drawText( "("+n+")",11,5);
               } 
        }else
        {
            if(isFocus())
             {
               // image
                g.drawBitmap(0, 0, selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.drawBitmap(0, 0, selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                
              } else
              {
                
                g.drawBitmap(0, 0,selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
                g.drawBitmap(0, 0,selectedImage.getWidth(), selectedImage.getHeight(), selectedImage, 0, 0);
               
              } 
        }
      }
    
  
}

