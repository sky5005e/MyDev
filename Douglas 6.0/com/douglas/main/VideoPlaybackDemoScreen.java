/*
 * VideoPlaybackDemoScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */
import javax.microedition.media.Player;
import javax.microedition.media.PlayerListener;
import javax.microedition.media.control.VideoControl;
import javax.microedition.media.control.VolumeControl;

import java.io.IOException;
import java.io.InputStream;
import javax.microedition.io.Connector;
import javax.microedition.io.file.FileConnection;

import net.rim.device.api.ui.Field;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;

/**
 * A screen for playing a video
 */
public class VideoPlaybackScreen extends MainScreen
{
    private Player _videoPlayer;
    private VideoControl _videoControl;

    /**
     * Constructs a screen to playback the video from a specified input stream
     * 
     * @param inputStream The InputStream of the video to display
     * 
     * @throws NullPointerException Thrown if <code>inputStream</code> is null
     */
    public VideoPlaybackScreen(InputStream inputStream)
    {
        if (inputStream == null)
        {
            throw new NullPointerException("'inputStream' cannot be null");
        }
        
        try
        {  
            _videoPlayer = javax.microedition.media.Manager.createPlayer(inputStream, "video/sbv");
            initScreen();
        }
        catch( Exception e )
        {
            Dialog.alert("Exception while initializing the playback video player\n\n" + e);
        }
    }
    

    /**
     * Constructs the screen to playback the video from a file
     * 
     * @param file A locator string in URI syntax that points to the video file
     */
    public VideoPlaybackScreen(String file)
    {        
        boolean notEmpty;
        try
        {
            FileConnection fconn = (FileConnection) Connector.open(file);
            notEmpty = fconn.exists() && fconn.fileSize() > 0;
            fconn.close();
        }
        catch( IOException e )
        {
            Dialog.alert("Exception while accessing the video filesize:\n\n" + e);     
            notEmpty = false;           
        }

        // Initialize the player if the video is not empty
        if( notEmpty )
        {
            try
            {
                _videoPlayer = javax.microedition.media.Manager.createPlayer(file);
                initScreen();
            }
            catch( Exception e )
            {
                Dialog.alert("Exception while initializing the playback video player\n\n" + e);  
            }
        }
        else
        {
            add(new LabelField("The video file you are trying to play is empty"));
        }
    }
    

    /**
     * Initializes the screen after the player has been created
     * 
     * @throws Exception Thrown if an error occurs when initializing the video player, video display or volume control
     */
    private void initScreen() throws Exception
    {
        _videoPlayer.realize();
        _videoPlayer.addPlayerListener(new PlayerListener()
        {
            /**
             * @see javax.microedition.media.PlayerListener#playerUpdate(Player, String, Object)
             */
            public void playerUpdate(Player player, String event, Object eventData)
            {
                // Alert the user and close the screen after the video has
                // finished playing.
                if( event == PlayerListener.END_OF_MEDIA )
                {
                    UiApplication.getUiApplication().invokeLater(new Runnable()
                    {
                        public void run()
                        {
                            Dialog.alert("Finished playing");
                            close();
                        }
                    });
                }
            }
        });

        // Set up the playback
        _videoControl = (VideoControl) _videoPlayer.getControl("VideoControl");
        Field vField = (Field) _videoControl.initDisplayMode(VideoControl.USE_GUI_PRIMITIVE,
                "net.rim.device.api.ui.Field");
        add(vField);

        VolumeControl vol = (VolumeControl) _videoPlayer.getControl("VolumeControl");
        vol.setLevel(30);
    }
    

    /**
     * @see net.rim.device.api.ui.Field#onVisibilityChange(boolean)
     */
    protected void onVisibilityChange(boolean visible)
    {
        // If the screen becomes visible and the video player was created, then
        // start the playback.
        if( visible && _videoPlayer != null )
        {
            try
            {
                _videoPlayer.start();
            }
            catch( Exception e )
            {
                // If starting the video fails, terminate the playback
                Dialog.alert("Exception while starting the video\n\n" + e);       
                close();
            }
        }
    }
    

    /**
     * @see net.rim.device.api.ui.Screen#onClose()
     */
    public void close()
    {
        // Close the video player if it was created
        if( _videoPlayer != null )
        {
            _videoPlayer.close();
        }
        super.close();
    }
}


package com.douglas.main;
import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.container.*;

import java.io.*;
import javax.microedition.media.*;
import javax.microedition.media.control.*;




/**
 * 
 */
public class VideoPlaybackDemoScreen extends MainScreen{
    
    public VideoPlaybackDemoScreen() 
    {    
  try 
{
    Player player = javax.microedition.media.Manager.createPlayer("file:///SDCard/BlackBerry/videos/soccer1.avi");
    player.realize();
    VideoControl videoControl = (VideoControl) player.getControl("VideoControl");
    Field videoField = (Field)videoControl.initDisplayMode( VideoControl.USE_GUI_PRIMITIVE, "net.rim.device.api.ui.Field" );

    add(videoField);

    VolumeControl volume = (VolumeControl) player.getControl("VolumeControl");
    volume.setLevel(30);

    player.start();
}
catch(MediaException me)
{
    Dialog.alert(me.toString());
}
catch(IOException ioe)
{
    Dialog.alert(ioe.toString());
}
} 
}
