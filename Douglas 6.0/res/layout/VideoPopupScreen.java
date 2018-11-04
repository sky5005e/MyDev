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


public class VideoPopupScreen extends PopupScreen implements FieldChangeListener{
        
        private VerticalFieldManager _vfManager;
        private RichTextField _rtField;
        private RichTextField _rtdField;
        private ButtonField _jaButton;
        private ButtonField _neinButton;
        private String url;
        private String videoname;
        private int len;
        public VideoPopupScreen(){
               super(new VerticalFieldManager(),NO_VERTICAL_SCROLL);
                 url=VideoScreen.selectedTab == 0 ? VideoScreen.lookstvspotsMakeUp[VideoScreen.selectedIndex].getVideourl() : VideoScreen.lookstvspotsTV[VideoScreen.selectedIndex].getVideourl();
                 int len=url.length();
                 videoname=url.substring(51,len);
               //  System.out.println("VIDEO NAME:"+videoname);
                _vfManager = new VerticalFieldManager(){
                        public void paint(Graphics g){
                                g.setBackgroundColor(Color.BLACK);
                                g.setGlobalAlpha(255);
                                g.clear();
                                super.paint(g);
                        }
                };
                _rtField = new RichTextField("Video jetzt abspielen?",RichTextField.TEXT_ALIGN_HCENTER | RichTextField.NON_FOCUSABLE){
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
                

                _rtdField = new RichTextField("Durch das Abspielen der Videos können möglicherweise weitere Kosten für Sie entstehen. Bitte überprüfen Sie Ihren Datentarif!",RichTextField.TEXT_ALIGN_HCENTER | RichTextField.NON_FOCUSABLE){
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
                _jaButton = new ButtonField("  Ja  ",ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
                
                _jaButton.setChangeListener(this);
                
                _neinButton = new ButtonField("Nein",ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
                _neinButton.setChangeListener(this);
                
                _rtField.setFont(DouglasConstants.mediumfont);
                _rtdField.setFont(DouglasConstants.mediumfont);
                _jaButton.setFont(DouglasConstants.mediumfont);
                _neinButton.setFont(DouglasConstants.mediumfont);
                
                _vfManager.add(_rtField);
                _vfManager.add(_rtdField);
                _vfManager.add(_jaButton);
                _vfManager.add(_neinButton);
                add(_vfManager);
        }
        public void fieldChanged(Field field,int context){
                if (field == _jaButton){
                     
         UiApplication.getUiApplication().invokeLater(new Runnable(){
         public void run()
         {
             try{
                    FileConnection videoFile=null;
                    videoFile = (FileConnection) Connector.open("file:///SDCard/"+videoname);
                    if(!videoFile.exists())
                    {
                       Browser.getDefaultSession().displayPage(url); 
                       if(videoFile.exists())
                       {
                        Browser.getDefaultSession().displayPage("file:///SDCard/"+videoname);
                        
                       }
                    }else
                    {
                       Browser.getDefaultSession().displayPage("file:///SDCard/"+videoname);
                    }
         } catch(IOException ioe)
         {
        //handle exception
         } 
                 
          }
                        });
                        
                } else if (field == _neinButton){
                        UiApplication.getUiApplication().invokeLater(new Runnable(){
                                public void run(){
                                        UiApplication.getUiApplication().popScreen(getScreen());
                                }
                        });
                        
                }
        }
        
}
