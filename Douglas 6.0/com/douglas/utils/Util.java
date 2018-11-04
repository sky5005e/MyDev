package com.douglas.utils;
import com.douglas.common.*;
import java.io.*;
import net.rim.device.api.ui.UiApplication;
import javax.microedition.io.HttpConnection;
import javax.microedition.io.Connector;
import net.rim.device.api.ui.component.BitmapField;
import net.rim.device.api.system.Bitmap;



public class Util{

//private static 
String _url;
public void getWebData(String url,final WebDataCallback callback) throws IOException
{
        
        _url = replaceAll(url,"{size}","100");
        Thread t = new Thread(new Runnable()
        {
            //String url;
                public void run()
                {
                        
                        HttpConnection connection = null;
                        InputStream inputStream = null;
                        
                        try
                        {
                                connection = (HttpConnection) Connector.open(_url+";interface=wifi", Connector.READ, true);
                                inputStream = connection.openInputStream();
                                byte[] responseData = new byte[10000];
                                int length = 0;
                                StringBuffer rawResponse = new StringBuffer();
                                while (-1 != (length = inputStream.read(responseData)))
                                {
                                        rawResponse.append(new String(responseData, 0, length));
                                }
                                int responseCode = connection.getResponseCode();
                                if (responseCode != HttpConnection.HTTP_OK)
                                {
                                        throw new IOException("HTTP response code: "
                                                        + responseCode);
                                }

                                final String result = rawResponse.toString();
                               
                                UiApplication.getUiApplication().invokeLater(new Runnable()
                                {
                                        public void run()
                                        {
                                                callback.callback(result);
                                        }
                                });
                        }
                        catch (final Exception ex)
                        {
                                UiApplication.getUiApplication().invokeLater(new Runnable()
                                {
                                        public void run()
                                        {
                                                callback.callback("Exception (" + ex.getClass() + "): " + ex.getMessage());
                                        }
                                });
                        }
                        finally
                        {
                                try
                                {
                                        inputStream.close();
                                        inputStream = null;
                                        connection.close();
                                        connection = null;
                                }
                                catch(Exception e){}
                        }
                }
        });
        t.start();
}

public static String replaceAll(String source, String pattern,
            String replacement) {
        if (source == null) {
            return "";
        }
       
        StringBuffer sb = new StringBuffer();
        int idx = -1;
        int patIdx = 0;

        while ((idx = source.indexOf(pattern, patIdx)) != -1) {
            sb.append(source.substring(patIdx, idx));
            sb.append(replacement);
            patIdx = idx + pattern.length();
        }
        sb.append(source.substring(patIdx));
        return sb.toString();

    }
    
  public static Bitmap resizeImage(Bitmap originalImage, int newWidth, int newHeight) {
    Bitmap newImage = new Bitmap(newWidth, newHeight);
    originalImage.scaleInto(newImage, Bitmap.FILTER_BILINEAR, Bitmap.SCALE_TO_FILL);
    return newImage;
}
  


}


