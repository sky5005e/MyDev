/*
 * TabButtonSet.java
 *
 * Research In Motion Limited proprietary and confidential
 * Copyright Research In Motion Limited, 2009-2009
 */

package com.douglas.main;

import com.douglas.main.*;
import com.douglas.common.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.decor.Background;
import net.rim.device.api.ui.decor.BackgroundFactory;
import net.rim.device.api.system.Bitmap; 
import net.rim.device.api.ui.Manager.*;
import net.rim.device.api.ui.container.HorizontalFieldManager;

/**
 * 
 */
public class TabButtonSet extends HorizontalFieldManager 
{    private static final int SYSTEM_STYLE_SHIFT = 32;
    private Field _selectedField;
    private int width1;
    private int width2;
   
     Background bg = BackgroundFactory.createBitmapBackground(Bitmap.getBitmapResource(DouglasConstants.BackgroundBright));
   
    public TabButtonSet()
    {
    }
    public TabButtonSet(int width1,int width2)
    {
     //   super(width1,width2);
        this.width1=width1;
        this.width2=width2;
        
    }
  
    
     protected void sublayout( int width, int height )
    {
        int availableWidth = width;
        int widthForBt=0;
        int highestWidth;
        int numFields = getFieldCount();
        int maxPreferredWidth = 0;
        int maxHeight = 0;


        // There may be a few remaining pixels after dividing up the space
        // we must split up the space between the first and last buttons
        int fieldWidth = (width / (numFields+1));
        int firstFieldExtra = 0;
        int lastFieldExtra = 0;
        
        int unUsedWidth = width - (fieldWidth * numFields)-width / (numFields+1);
        if( unUsedWidth > 0 ) {
            firstFieldExtra = unUsedWidth;
            lastFieldExtra = unUsedWidth - firstFieldExtra;
        }
        
        int prevRightMargin = 0;
        
        // Layout the child fields, and calculate the max height
        for( int i = 0; i < numFields; i++ ) {
            
            int nextLeftMargin = 0;
            if( i < numFields - 1 ) {
                Field nextField = getField( i );
                nextLeftMargin = nextField.getMarginLeft();
            }
            
            Field currentField = getField( i );
            int leftMargin = i == 0 ? currentField.getMarginLeft() : Math.max( prevRightMargin, currentField.getMarginLeft() ) / 2;
            int rightMargin = i < numFields - 1 ? Math.max( nextLeftMargin, currentField.getMarginRight() ) / 2 : currentField.getMarginRight();
            int currentVerticalMargins = currentField.getMarginTop() + currentField.getMarginBottom();
            int currentHorizontalMargins =leftMargin + rightMargin;
            
            int widthForButton = fieldWidth;
           
            if( i == 0 )
            {
              widthForButton = width1+7;
              widthForBt=widthForButton;
            }else if(i<numFields -1)
            {
              widthForButton = width1+1;
              widthForBt=widthForButton;  
            }else if( i == numFields -1 ) 
            {
              widthForButton = width1+100;
            }
            //highestWidth=widthForBt+widthForButton-75;
            
            layoutChild( currentField, widthForButton - currentHorizontalMargins, height - currentVerticalMargins );
            maxHeight = Math.max( maxHeight, currentField.getHeight() + currentVerticalMargins );
            
            prevRightMargin = rightMargin;
            nextLeftMargin = 0;
        }

        // Now position the fields, respecting the Vertical style bits
        int usedWidth = 0;
        int y;
        prevRightMargin = 0;
        for( int i = 0; i < numFields; i++ ) {
            
            Field currentField = getField( i );
            int marginTop = currentField.getMarginTop();
            int marginBottom = currentField.getMarginBottom();
            int marginLeft = Math.max( currentField.getMarginLeft(), prevRightMargin );
            int marginRight = currentField.getMarginRight();
            
            switch( (int)( ( currentField.getStyle() & FIELD_VALIGN_MASK ) >> SYSTEM_STYLE_SHIFT ) ) {
                case (int)( FIELD_BOTTOM >> SYSTEM_STYLE_SHIFT ):
                    y = maxHeight - currentField.getHeight() - currentField.getMarginBottom();
                    break;
                case (int)( FIELD_VCENTER >> SYSTEM_STYLE_SHIFT ):
                    y = marginTop + ( maxHeight - marginTop - currentField.getHeight() - marginBottom ) >> 1;
                    break;
                 
                default:
                    y = marginTop;
            }
            setPositionChild( currentField, usedWidth+ marginLeft, y );
            usedWidth += currentField.getWidth() + marginLeft;
            prevRightMargin = marginRight;
        }
        setExtent( width, maxHeight-9);
        this.setBackground(bg);
    }
 
    
    public void setSelectedField( Field selectedField ) 
    {
        //Field _selectedField ;
        if( _selectedField == selectedField ) {
            return; // already selected
        }
        
        // Clear old one
        if( _selectedField instanceof TabButtonField ) {
            ( (TabButtonField) _selectedField ).setSelected( false );
        }
        
        _selectedField = selectedField;
        
        // Select New Field
        if( _selectedField instanceof TabButtonField ) {
            ( (TabButtonField) _selectedField ).setSelected( true );
        }
    }
}
