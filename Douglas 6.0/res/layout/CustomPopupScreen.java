package res.layout;


import com.douglas.common.*;
import com.douglas.main.*;
import net.rim.device.api.ui.Color;
import net.rim.device.api.ui.Field;
import net.rim.device.api.ui.FieldChangeListener;
import net.rim.device.api.ui.Graphics;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.ButtonField;
import net.rim.device.api.ui.component.RichTextField;
import net.rim.device.api.ui.container.PopupScreen;
import net.rim.device.api.ui.container.VerticalFieldManager;
import net.rim.blackberry.api.browser.Browser;
import javax.microedition.media.Manager;
import javax.microedition.media.Player;
import java.io.IOException;
import javax.microedition.media.control.*;
import javax.microedition.io.Connector;
import javax.microedition.io.HttpConnection;
import javax.microedition.io.StreamConnection;
import java.io.*;
import javax.microedition.media.MediaException;
import javax.microedition.io.file.FileConnection;


public class CustomPopupScreen extends PopupScreen implements FieldChangeListener{
        
        private VerticalFieldManager _vfManager;
        private RichTextField _rtField;
        private ButtonField _jaButton;
        private ButtonField _neinButton;
        private String url;
        private String _message;
        private int _flag;
        public  CustomPopupScreen(String message,int flag){
               super(new VerticalFieldManager(),NO_VERTICAL_SCROLL);
               _message=message;
               _flag=flag;
              _vfManager = new VerticalFieldManager(){
                        public void paint(Graphics g){
                                g.setBackgroundColor(Color.BLACK);
                                g.setGlobalAlpha(255);
                                g.clear();
                                super.paint(g);
                        }
                };
                _rtField = new RichTextField(_message,RichTextField.TEXT_ALIGN_HCENTER | RichTextField.NON_FOCUSABLE){
                        public void paintBackground(Graphics g){
                                g.setBackgroundColor(Color.WHITE);
                                g.setGlobalAlpha(0);
                                g.clear();
                                super.paint(g);
                        }
                        public void paint(Graphics g){
                                g.setColor(Color.WHITE);
                                g.setGlobalAlpha(255);
                                super.paint(g);
                        }
                };
                

              if(_flag==0)
                          {
                _jaButton = new ButtonField(DouglasConstants.RIGHTMenu,ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
                _jaButton.setChangeListener(this);
              } 
               // _neinButton = new ButtonField("Nein",ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
                //_neinButton.setChangeListener(this);
                
                _rtField.setFont(DouglasConstants.mediumfont);
                _jaButton.setFont(DouglasConstants.mediumfont);
                //_neinButton.setFont(DouglasConstants.mediumfont);
                
                _vfManager.add(_rtField);
                //_vfManager.add(_rtdField);
                _vfManager.add(_jaButton);
                //_vfManager.add(_neinButton);
                add(_vfManager);
        }
        public void fieldChanged(Field field,int context){
          if (field == _jaButton){
                        UiApplication.getUiApplication().invokeLater(new Runnable(){
                                public void run(){
                                        UiApplication.getUiApplication().popScreen(getScreen());
                                }
                        });
                        
                }
        }
        
}
