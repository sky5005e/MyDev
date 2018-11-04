/*
 * ActivityIndicatorScreen.java
 *
 * AUTO_COPY_RIGHT_SUB_TAG
 */

package com.rim.samples.device.ui.progressindicatordemo;

import net.rim.device.api.command.*;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.component.progressindicator.*;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.ui.decor.*;
import net.rim.device.api.util.*;


/**
 * A screen displaying a number of activity indicators
 */
public class ActivityIndicatorScreen extends MainScreen
{
    private ActivityIndicatorView[] _views;

   /**
    * Creates a new ActivityIndicatorScreen object
    */
    public ActivityIndicatorScreen ()
    {
      //  setTitle("Activity Indicator Screen");
        
        _views = new ActivityIndicatorView[7];          
                    
        Bitmap bitmap; // Reuse this reference            
        
       
        // Add an ActivityIndicatorView with label and animation centered in a horizontal layout
        bitmap = Bitmap.getBitmapResource("spinner.png");
        _views[1] = ActivityIndicatorFactory.createActivityIndicator(new HorizontalFieldManager(), Field.FIELD_HCENTER, bitmap, 5, 0,
                        "horizontal centered layout", Field.FIELD_HCENTER);
        add(_views[1]);
       
 
    }
        
    /**
    * Thread class which stops an ActivityIndicatorView animation after an
    * arbitrary delay. 
    */
    private static class DelayedStop extends Thread
    {
        private ActivityIndicatorView _view;
    
       /**
        * Creates a new DelayedStop object
        * @param view The ActivityIndicatorView to stop
        */
        public DelayedStop(ActivityIndicatorView view)
        {
            _view = view;
        }
        
    
       /**
        * @see Runnable#run()
        */
        public void run()
        {
            try
            {
                Thread.sleep(3000);
            }
            catch(InterruptedException e)
            {
            }
            _view.getModel().cancel();
        }
    }
    
    
   /**
    * Thread class which starts an ActivityIndicatorView animation after an
    * arbitrary delay. 
    */
    private static class DelayedStart extends Thread
    {
        private ActivityIndicatorView _view;
    
       /**
        * Creates a new DelayedStart object
        * @param view The ActivityIndicatorView to start
        */
        public DelayedStart(ActivityIndicatorView view)
        {
            _view = view;
        }
        
    
       /**
        * @see Runnable#run()
        */
        public void run()
        {
            try
            {
                Thread.sleep(2000);
            }
            catch(InterruptedException e)
            {
            }
            _view.getModel().reset();
        }
    }    
    
    
   /**
    * A popup screen class demonstrating the displaying of activity indicators
    * on a modal screen.
    */
    private static class ActivityPopupScreen extends PopupScreen implements FieldChangeListener
    {
        private ActivityIndicatorView _view;
        private ButtonField _buttonFieldStopStart;        
        private ButtonField _buttonFieldClose;
        private boolean _topSpinning;
        
        
       /**
        * Creates a new ProgressPopupScreen object
        * @param view The ActivityIndicatorView to display on this screen
        */
        public ActivityPopupScreen(ActivityIndicatorView view)
        {
            super(view);
            setChangeListener(this);
            _view = view;       
            _topSpinning = true;            
    
            _buttonFieldStopStart = new ButtonField("Stop Top Spinner", Field.FIELD_HCENTER | ButtonField.CONSUME_CLICK);            
            _buttonFieldStopStart.setChangeListener(this);            
            _view.add(_buttonFieldStopStart);            
    
            _buttonFieldClose = new ButtonField("Close Popup", Field.FIELD_HCENTER | ButtonField.CONSUME_CLICK);
            _buttonFieldClose.setChangeListener(this);
            _view.add(_buttonFieldClose);         
        }
        
        
       /**
        * @see FieldChangeListener#fieldChanged(Field, int)         
        */
        public void fieldChanged(Field field, int context)
        {
            if(field == _buttonFieldStopStart)
            {                
                if(_topSpinning)
                {
                    _view.getModel().cancel();
                    _buttonFieldStopStart.setLabel("Restart Top Spinner");
                    _topSpinning = false;            
                }
                else
                {
                    _view.getModel().reset();                
                    _buttonFieldStopStart.setLabel("Stop Top Spinner");
                    _topSpinning = true;            
                }
            }            
            else if(field == _buttonFieldClose)
            {                
                close();                
            }            
        }       
    
        
       /**
        * @see Screen#keyChar(char, int, int)
        */
        protected boolean keyChar(char ch, int status, int time)
        {
            if(ch == Characters.ESCAPE)
            {
                close();
                return true;
            }
            return super.keyChar(ch, status, time);
        }
    }
}

