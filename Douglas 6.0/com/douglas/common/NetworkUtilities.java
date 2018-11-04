/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
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
import net.rim.device.api.ui.component.Dialog;


/**
 *
 * @author Himanshu
 */
public class NetworkUtilities {
    private static final String TAG = "NetworkUtilities";
    public static final int REGISTRATION_TIMEOUT = 30 * 1000; // ms
   
   

    
    public static Store[] fetchStores(String params) throws Exception {
        Store[] storesList;
        
        String response = fetchURL(DouglasConstants.BASE_URL,params);

            InputStream is =   new ByteArrayInputStream(response.getBytes());
            DouglasXMLParser x = new DouglasXMLParser(is);
            storesList = x.parseStores();
            return storesList;
    }
        public static Event[] fetchEvents(String params) throws Exception {
        Event[] eventsList;
        
        String response = fetchURL(DouglasConstants.BASE_URL,params);

            InputStream is =   new ByteArrayInputStream(response.getBytes());
            DouglasXMLParser x = new DouglasXMLParser(is);
            eventsList = x.parseEvents();
            return eventsList;
    }
        
        public static TopTen[] fetchTopTen(String params) throws Exception
     {
        TopTen[] topTenList;

        final String response = fetchURL(DouglasConstants.BASE_URL,params);


        
            // Succesfully connected to the douglas server
            InputStream is =   new ByteArrayInputStream(response.getBytes());
            DouglasXMLParser x = new DouglasXMLParser(is);
            topTenList = x.parseTopTen();
           
            return topTenList;
        }
        public static Neuheiten[] fetchNeuheiten(String params) throws Exception
       {
       Neuheiten[] neuheitenList;

        
        final String response =fetchURL(DouglasConstants.BASE_URL,params);

       
            // Succesfully connected to the douglas server
            InputStream is =   new ByteArrayInputStream(response.getBytes());
            DouglasXMLParser x = new DouglasXMLParser(is);
            neuheitenList = x.parseNeuheiten();
            
            return neuheitenList;
        }

        public static Angebote[] fetchAngebote(String params) throws Exception {
        Angebote[] angeboteList;

        final String response =fetchURL(DouglasConstants.BASE_URL,params);
        // Succesfully connected to the douglas server
            InputStream is =   new ByteArrayInputStream(response.getBytes());
            DouglasXMLParser x = new DouglasXMLParser(is);
            
            angeboteList = x.parseAngebote();
            return angeboteList;
        }
       

        
       public static LooksTVSpots[] fetchLooksTVSpots(String params) throws Exception
        {
        LooksTVSpots[] lookstvspotsList;

        
        final String response = fetchURL(DouglasConstants.BASE_URL,params);
        
            InputStream is =   new ByteArrayInputStream(response.getBytes());
            DouglasXMLParser x = new DouglasXMLParser(is);
            lookstvspotsList = x.parselookstvspots();
            return lookstvspotsList;
        }

    
   
    /**
     * Fetches the content on the speicifed url.
     * @param url The url of the content to fetch
     */
    public static String fetchURL(String url, String parameters)
    {
        // Normalize the url.
        String lcase = url.toLowerCase();
        url = lcase + parameters;
        String content = "";

        
         // It is illegal to open a connection on the event thread. We need to
         // spawn a new thread for connection operations.
                try
                {
                    StreamConnection s = null;
                    s = (StreamConnection)Connector.open(url+";interface=wifi");
                    HttpConnection httpConn = (HttpConnection)s;
                     
                    int status = httpConn.getResponseCode();
                   
                    if (status == HttpConnection.HTTP_OK)
                    {

                        InputStream input = s.openInputStream();

                        byte[] data = new byte[256];
                        int len = 0;
                        int size = 0;
                        StringBuffer raw = new StringBuffer();

                        while ( -1 != (len = input.read(data)) )
                        {
                            // Exit condition for the thread. An IOException is
                            // thrown because of the call to  httpConn.close(),
                            // causing the thread to terminate.
                            String str = new String(data, 0, len); 
                            raw.append(str);
                            size += len;
                        }

                        //raw.insert(0, "bytes received]\n");
                        //raw.insert(0, size);
                        //raw.insert(0, '[');
                        content = raw.toString();
                        input.close();
                    }
                    else
                    {
                        content = "response code = " + status;
                        Dialog.alert(content);
                        System.exit(0);
                    }
                    s.close();
                }
                catch (IOCancelledException e)
                {
                    System.out.println(e.toString());

                }
                catch (IOException e)
                {
                    System.out.println(e.toString());

                }

        return content;
  }

}
