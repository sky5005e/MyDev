/*
 * VideoPlaybackDemoScreen.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */
package com.douglas.main;
 
import javax.microedition.media.Player;
import javax.microedition.media.PlayerListener;
import javax.microedition.media.control.VideoControl;
import javax.microedition.media.control.VolumeControl;
 import javax.microedition.media.MediaException;

import java.io.IOException;
import java.io.InputStream;
import javax.microedition.io.Connector;
import javax.microedition.io.file.FileConnection;

import net.rim.device.api.ui.*;
import net.rim.device.api.ui.component.Dialog;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;
import javax.microedition.media.Manager;
import java.io.*;
/**
 * A screen for playing a video
 */
public class VideoPlaybackScreen extends MainScreen
{
     

    public VideoPlaybackScreen(String url)
    {
       url=url;
      initVideo(url);
    }
    
/*
* Best practice is to invoke realize(), then prefetch(), then start().
* Following this sequence reduces delays in starting media playback.
*
* Invoking start() as shown below will cause start() to invoke prefetch(0),
* which invokes realize() before media playback is started.
*/
//String url=lookstvspots[_tlm.getID()].getVideourl();
private void initVideo(String url) {
try
{
     //Player p = Manager.createPlayer("http://www.test.rim.net/abc.wav");
      // p.start();
   
Player _player = javax.microedition.media.Manager.createPlayer(url);
_player.realize();
VideoControl _vc = (VideoControl) _player.getControl("VideoControl");
if (_vc != null)
{
Field _videoField = (Field) _vc.initDisplayMode(
VideoControl.USE_GUI_PRIMITIVE, "net.rim.device.api.ui.Field");
_vc.setVisible(true);
}
} catch(MediaException pe) {
System.out.println(pe.toString());
} catch (IOException ioe) {
System.out.println(ioe.toString());
}
}}
    

    /**
     * @see net.rim.device.api.ui.Screen#onClose()
     */
   




