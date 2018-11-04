/*
 * AdvertisePcture.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;
import com.douglas.common.*;
import com.douglas.utils.*;

import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.*;
import res.layout.*;
import java.io.*;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.ui.component.SeparatorField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.component.TextField;
import net.rim.device.api.ui.decor.Background;
import net.rim.device.api.ui.decor.BackgroundFactory;
import net.rim.device.api.ui.Graphics;
import net.rim.device.api.system.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.Font;
import net.rim.device.api.ui.Manager; 
import net.rim.device.api.system.Bitmap; 
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.container.MainScreen;
import net.rim.device.api.ui.container.HorizontalFieldManager;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.system.*;
import net.rim.device.api.io.FileOutputStream; 
import javax.microedition.io.Connector;
import net.rim.device.api.io.IOUtilities;
import javax.microedition.io.file.FileConnection;
import javax.microedition.io.StreamConnection;
/**
 * 
 */
public class AdvertisePicture extends MainScreen
{
   private Thread t;
   private String content;
   private InputStream inputStream = null;
  
   public AdvertisePicture() { 
    
 //MD5 Logic
       try
       {
       final String response= NetworkUtilities.fetchURL(DouglasConstants.BASE_URL,"content/dynamic_pics/entry_android_de.jpg.md5");
       FileConnection fc = (FileConnection)Connector.open("file:///SDCard/"+"test.txt",Connector.READ_WRITE);
       if(!fc.exists())
       fc.create();
       OutputStream os =fc.openOutputStream();
       os.write(response.getBytes());
       os.close(); 
       fc.close(); 
 /**----Read text file from phone-----**/
       
       byte[] dataResult=this.readFile("test.txt");
       
       String str = new String(dataResult); 
      
       
 //store image locally 
       Bitmap bitmap; 
       EncodedImage encodedImage;
       final String downloadImage= NetworkUtilities.fetchURL(DouglasConstants.BASE_URL,"content/dynamic_pics/entry_android_de.jpg");
       byte[] dataArray = downloadImage.getBytes();   
            encodedImage = EncodedImage.createEncodedImage(dataArray, 0,   
                   dataArray.length);   
        FileConnection imageFile=null;   
        
      byte[] rawData = encodedImage.getData();
       
       try{
        //You can change the folder location on the SD card if you want
        imageFile = (FileConnection) Connector.open("file:///SDCard/"+"image.jpg");
        if(!imageFile.exists()){
            imageFile.create();
        }

        //Write raw data
        OutputStream outStream = imageFile.openOutputStream();
        outStream.write(rawData);
        outStream.close();
        imageFile.close();
    } catch(IOException ioe){
        //handle exception
    } finally {
        try{
            if(imageFile != null){
                imageFile.close();
            } 
        } catch(IOException ioe){

        }
    }
   
  
   String retrivedFileResult=str.substring(0,32);
   String retrivedResponse=response.substring(0,32);
   //check whether MD5 data matches with stored text file
     if(retrivedFileResult.equals(retrivedResponse)) 
     {
     
      FileConnection photoFile = (FileConnection) Connector.open("file:///SDCard/"+"image.jpg");
      InputStream input = photoFile.openInputStream();

      int fileSize = (int) photoFile.fileSize();
      byte[] data = new byte[fileSize];
      input.read(data, 0, fileSize);

      EncodedImage photoBitmap = EncodedImage.createEncodedImage(data, 0, data.length);
      photoBitmap.setScale(2);
      
       // compose bitmap field and add it to the manager
        
        BitmapField imageCanvas = new BitmapField(photoBitmap.getBitmap());     
        HorizontalFieldManager hfm = new HorizontalFieldManager(HorizontalFieldManager.FIELD_VCENTER);            
       hfm.add(imageCanvas);
       VerticalFieldManager vfm=new VerticalFieldManager(VerticalFieldManager.FIELD_HCENTER);
       vfm.add(hfm);
       add(vfm);
     }else
     {
       BitmapField img =new WebBitmapField("http://douglasapp.solidground.de/content/dynamic_pics/entry_android_de.jpg",2);
       HorizontalFieldManager hfm = new HorizontalFieldManager(HorizontalFieldManager.FIELD_VCENTER);            
       hfm.add(img);
       VerticalFieldManager vfm=new VerticalFieldManager(VerticalFieldManager.FIELD_HCENTER);
       vfm.add(hfm);
       add(vfm);
     } 
   
       
    }catch(Exception e)
       {
           
       }
 
  t = new Thread(new Runnable()
  {
           
  public void run()
  {   
       
      try{
           t.sleep(10000);
         }catch(InterruptedException ex)
         {
         }
   
   UiApplication.getUiApplication().invokeLater(new Runnable()
        {
            public void run()
            {
                  UiApplication.getUiApplication().pushScreen(new MenuScreen());
                 
            }
        });
  }
               
 });
 t.start();
 }
      private static byte[] readFile(String fileName) {  
       String fName = "file:///SDCard/" + fileName;  
        byte[] data = null;  
        FileConnection fconn = null;  
        DataInputStream is = null;  
        try {  
           fconn = (FileConnection) Connector.open(fName, Connector.READ);  
           is = fconn.openDataInputStream();  
            data = IOUtilities.streamToBytes(is);  
           
     } catch (IOException e) {  
           System.out.println(e.getMessage());  
       } finally {  
          try {  
              if (null != is)  
                  is.close();  
             if (null != fconn)  
                   fconn.close();  
           } catch (IOException e) {  
              System.out.println(e.getMessage());  
           }  
      }  
       return data;  
   }  
   
   
} 

