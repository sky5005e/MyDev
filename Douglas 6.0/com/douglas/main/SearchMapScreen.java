/*
 * SearchMapScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;
import res.layout.*;
import com.douglas.common.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.blackberry.api.browser.Browser;
import net.rim.blackberry.api.browser.BrowserSession;
import net.rim.device.api.browser.field2.*;
import net.rim.device.api.util.CRC32;
import net.rim.blackberry.api.invoke.*;
import net.rim.blackberry.api.maps.*;
import net.rim.device.api.lbs.maps.*;
import net.rim.device.api.lbs.maps.model.*;
import net.rim.device.api.lbs.maps.ui.*;
import net.rim.device.api.lbs.maps.utils.MappableVector;
import net.rim.device.api.ui.extension.container.ZoomScreen;

public class SearchMapScreen extends MainScreen {
   private VerticalFieldManager vfm;
   private BrowserField _browserField;
   public String url;
   
   public static Store[] perfumerie;
  
   
   public static Store[] store;
   
   private Shop shop;
   private EncodedImage fullImage;
   private static EncodedImage SuchMenuOn = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchBackImageON);  
   private static EncodedImage SuchMenuOFF = EncodedImage.getEncodedImageResource(DouglasConstants.HeaderSuchBackImageOFF);
  
   public SearchMapScreen() 
   {
        //super(FullScreen.DEFAULT_CLOSE | FullScreen.DEFAULT_MENU | FullScreen.VERTICAL_SCROLL | FullScreen.VERTICAL_SCROLLBAR);
         StoreHeaderLabel s=new StoreHeaderLabel();
         HeaderLabel HL=new HeaderLabel();
         vfm=HL.HeaderLabel(DouglasConstants.Karte,0,SuchMenuOn,SuchMenuOFF, this);
         setBanner(vfm); 
         DouglasFooter douglasFT = new DouglasFooter();
         VerticalFieldManager vfmStatus = new VerticalFieldManager();
         vfmStatus.add(douglasFT.DouglasFooter(DouglasConstants.STOREMenu));
         setStatus(vfmStatus);
         url=s.getURL();
    
  try
        {
        perfumerie = NetworkUtilities.fetchStores(url);
        store = new Store[perfumerie.length];
        store = perfumerie;
        MappableVector data = new MappableVector();
       // StringBuffer stringBuffer = new StringBuffer("<lbs>");
                     
           
            
         
        for (int i = 0; i < store.length; i++)
         {
              shop=store[i].getShop();
              double lng=Double.parseDouble(shop.getAddresslng());
              double lat=Double.parseDouble(shop.getAddresslat());
              data.addElement( new MapLocation(lat,lng, shop.getStreetName(), null ) );
            // stringBuffer.append("<location lon='lng' lat='lat' label='Douglas Shop, ON' description='Douglas Shop, jermani' />"); 
         }
         // stringBuffer.append("</lbs>");  
         // Invoke.invokeApplication(Invoke.APP_TYPE_MAPS, new MapsArguments( MapsArguments.ARG_LOCATION_DOCUMENT, stringBuffer.toString()));
        XYDimension imageSize = new XYDimension(480,360);
         
         Bitmap map = MapFactory.getInstance().generateStaticMapImage( imageSize, data );
       /*  PNGEncoder encoder = new PNGEncoder(map, true);
         byte[] imageBytes = encoder.encode(true);
         fullImage = EncodedImage.createEncodedImage(imageBytes, 0, imageBytes.length);*/
         BitmapField imgb = new BitmapField(map);
         
         add(imgb);
        
          // specify the image size, center and zoom level
       
     
         }catch(Exception e) {} 
        

   }
 /* protected boolean navigationClick(int status, int time)
        {
            // Push a new ZoomScreen if track ball or screen is clicked
            UiApplication.getUiApplication().pushScreen(new ZoomScreen(fullImage));                            
            return true;
        }
        
        
        /**
        * @see Screen#touchEvent(TouchEvent)
        */
      /*  protected boolean touchEvent(TouchEvent message)
        {     
            if(message.getEvent() == TouchEvent.CLICK)
            {
                UiApplication.getUiApplication().pushScreen(new ZoomScreen(fullImage));                            
            }
            return super.touchEvent(message);          
        }*/
} 


