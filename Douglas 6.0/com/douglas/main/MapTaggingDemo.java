/*
 * MapTaggingDemo.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package com.douglas.main;


import net.rim.device.api.ui.*;
import net.rim.device.api.ui.container.*;
import net.rim.device.api.lbs.maps.*;
import net.rim.device.api.lbs.maps.model.*;
import net.rim.device.api.lbs.maps.ui.*;
import net.rim.device.api.ui.component.*;
public class MapTaggingDemo extends MainScreen
{
    
    public MapTaggingDemo() 
    {
        super(FullScreen.DEFAULT_CLOSE | FullScreen.DEFAULT_MENU | FullScreen.VERTICAL_SCROLL | FullScreen.VERTICAL_SCROLLBAR);
           
        RichMapField map = MapFactory.getInstance().generateRichMapField();
        add(map);
        
        MapDataModel data = map.getModel();
            
        MapLocation julieHome = new MapLocation( 43.47751, -80.54817, "Julie - Home", null );
        MapLocation headOffice = new MapLocation( 43.47550, -80.53900, "Head Office", null );
        
        int julieHomeId = data.add( (Mappable) julieHome, "julie" );
        data.tag( julieHomeId, "home" );
        int headOfficeId = data.add( (Mappable) headOffice, "julie" );
        data.tag( headOfficeId, "work" );
        
        MapLocation paulHome = new MapLocation( 43.49487, -80.55335, "Paul - Home", null );
        int paulHomeId = data.add( (Mappable) paulHome, "paul" );
        data.tag( paulHomeId, "home" );
        data.tag( headOfficeId, "paul" );
        
        data.tag( paulHomeId, "sarah" );
        MapLocation manufacturing = new MapLocation( 43.46514, -80.50506, "Manufacturing", null );
        int manufacturingId = data.add( (Mappable) manufacturing, "sarah" );
        data.tag( manufacturingId, "work" );
        
        data.setVisibleNone();
        data.setVisible( "work" );
        map.getMapField().update( true );          
    }
}

