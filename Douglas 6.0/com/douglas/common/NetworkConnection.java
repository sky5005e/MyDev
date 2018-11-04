/*
 * NetworkConnection.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.common;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import javax.microedition.io.Connector;
import javax.microedition.io.HttpConnection;
import javax.microedition.io.StreamConnection;
import javax.microedition.lcdui.List;
import net.rim.device.api.io.IOCancelledException;




public class ImageField {
    
     
    
    public BitmapField getImageField(String url) throws IOException
        {
              StreamConnection conn;
         if (DeviceInfo.isSimulator())
          {
                      url = url + ";deviceSide=true";
                }
              conn = (StreamConnection) Connector.open(url);
         InputStream is = conn.openInputStream();
               BitmapField bf = null;
         try
            {
                      ByteArrayOutputStream baos = new ByteArrayOutputStream();
                      int ch;
                        while ((ch = is.read()) != -1)
                 {
                              baos.write(ch);
                        }
                      byte imageData[] = baos.toByteArray();
                 bf = new BitmapField(EncodedImage.createEncodedImage(imageData, 0, imageData.length).getBitmap());
             } finally
              {
                      if (is != null)
                        {
                              is.close();
                    }
              }
              return (bf == null ? null : bf);
       }
} 



