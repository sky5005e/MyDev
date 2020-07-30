//<![CDATA[
/* The author of this code is Geir K. Engdahl, and can be reached
 * at geir.engdahl (at) gmail.com
 * 
 * If you intend to use the code or derive code from it, please
 * consult with the author.
 */ 
// Create a directions object and register a map and DIV to hold the 
// resulting computed directions

var gebMap;           // The map DOM object
var directionsPanel;  // The driving directions DOM object
var gebDirections;    // The driving directions returned from GMAP API
var gebGeocoder;      // The geocoder for addresses
var maxTspSize = 20;  // A limit on the size of the problem, mostly to save Google servers from undue load.
var maxTspBF = 9;     // Max size for brute force, may seem conservative, but many browsers have limitations on run-time.
var maxSize = 24;     // Max number of waypoints in one Google driving directions request.
var exportOrder = false; // Save lat/lng for text export.
var maxTripSentry = 2000000000; // Approx. 63 years., this long a route should not be reached...
var distIndex;      
var reasons = new Array();
reasons[G_GEO_SUCCESS]            = "Success";
reasons[G_GEO_MISSING_ADDRESS]    = "Missing Address: The address was either missing or had no value.";
reasons[G_GEO_UNKNOWN_ADDRESS]    = "Unknown Address:  No corresponding geographic location could be found for the specified address.";
reasons[G_GEO_UNAVAILABLE_ADDRESS]= "Unavailable Address:  The geocode for the given address cannot be returned due to legal or contractual reasons.";
reasons[G_GEO_BAD_KEY]            = "Bad Key: The API key is either invalid or does not match the domain for which it was given";
reasons[G_GEO_TOO_MANY_QUERIES]   = "Too Many Queries: The daily geocoding quota for this site has been exceeded.";
reasons[G_GEO_SERVER_ERROR]       = "Server error: The geocoding request could not be successfully processed.";
var gebMarkers = new Array();
var waypoints = new Array();
var addresses = new Array();
var wpActive = new Array();
var addressRequests;


function loadAtStart(lat, lng, zoom) {
 
   //if (exprt != null) exportOrder = exprt;
    
  if (GBrowserIsCompatible()) {
 
    addressRequests = 0;
    gebMap = new GMap2(document.getElementById("map"));
    //directionsPanel = document.getElementById("my_textual_div");
    gebGeocoder = new GClientGeocoder();
  
		  // gebMap.addControl(new GSmallMapControl());
				//gebMap.addControl(new GMapTypeControl());
				//gebMap.addControl(new GOverviewMapControl());
				gebMap.enableDoubleClickZoom();
				gebMap.enableContinuousZoom();
				gebMap.enableScrollWheelZoom();
  
    //  map.setCenter(new GLatLng((40.730885,-73.997383), 15);
     
      gebMap.setCenter(new GLatLng(40.730885,-73.997383), 15);
    //gebMap.setCenter(new GLatLng(37.4419, -122.1419), 13);
    gebMap.addControl(new GLargeMapControl());
    gebMap.addControl(new GMapTypeControl());
    
     

  }
   
}
function addWaypoint(latLng) {
  var freeInd = -1;
  
  for (var i = 0; i < waypoints.length; ++i) {
    if (!wpActive[i]) {
      freeInd = i;
      break;
    }
  }
  if (freeInd == -1) {
    if (waypoints.length < 20) {
      waypoints.push(latLng);
      wpActive.push(true);
      freeInd = waypoints.length-1;
    } else {
      return(-1);
    }
  } else {
    waypoints[freeInd] = latLng;
    wpActive[freeInd] = true;
  }
  var myIcn1;
  if (freeInd == 0) {
    myIcn1 = new GIcon(G_DEFAULT_ICON,"icons/iconr" + (freeInd+1) + ".png");
    myIcn1.printImage = "icons/iconr" + (freeInd+1) + ".png";
    myIcn1.mozPrintImage = "icons/iconr" + (freeInd+1) + ".gif";
  } else {
    myIcn1 = new GIcon(G_DEFAULT_ICON,"icons/iconb" + (freeInd+1) + ".png");
    myIcn1.printImage = "icons/iconb" + (freeInd+1) + ".png";
    myIcn1.mozPrintImage = "icons/iconb" + (freeInd+1) + ".gif";
  }
  gebMarkers[freeInd] = new GMarker(latLng,myIcn1);
  gebMap.addOverlay(gebMarkers[freeInd]);
  return(freeInd);
} 


function clickedAddAddress(img,title,address) {
 //document.getElementById("tblcord").style.display = "none";
 //document.getElementById("tbladdress").style.display = "none";

   addAddress(address ,img,title); 

}
function addCoordinates(img,title)
{        
      
       gebMap.setCenter(new GLatLng(parseFloat(document.getElementById("LatitudeStr").value), parseFloat(document.getElementById("LognitudeStr").value)),5);
	   var marker = new GMarker(new GLatLng(parseFloat(document.getElementById("LatitudeStr").value), parseFloat(document.getElementById("LognitudeStr").value)));
	  GEvent.addListener(marker, "click", function() 
	           {
	           marker.openInfoWindowHtml("<div class=Myrtle_text align=center><div align=center><b>"+ title +"</b></div><div align=center><img witdh=125 height=80 src="+ img +"></div></div>");               
               }
      ); 
	   gebMap.addOverlay(marker);
	   setTimeout(function() 
	   {
			
               marker.openInfoWindowHtml("<div class=Myrtle_text align=center><div align=center><b>"+ title +"</b></div><div align=center><img witdh=125 height=80 src="+ img +"></div></div>");               
    		
		}, 0);
}



function addAddress(address,img,title)
{
  addressRequests++;
 
  gebGeocoder.getLatLng(address, function(latLng) 
  {
 
      addressRequests--;
      if (!latLng) 
      {  
        
         addAddress(title,img,title)
      } 
      else 
     {
	    gebMap.setCenter(latLng, 13);
	  
	    var freeInd = addWaypoint(latLng);
	    addresses[freeInd] = address;
                    var marker = new GMarker(latLng);
							GEvent.addListener(marker, "click", function() {
    						//marker.openInfoWindowHtml("<div class=Myrtle_text align=center ><br/><div align=center><b>"+ title +"</b></div><br/><div align=center><img witdh=125 height=80 src="+ img +"></div></div>");               
						}); 
						gebMap.addOverlay(marker);
						//Assign the default zoom level to map
						gebMap.setZoom(15);
						setTimeout(function() {
    						//marker.openInfoWindowHtml("<div class=Myrtle_text align=center ><br/><div align=center><b>"+ title +"</b></div><br/><div align=center><img witdh=125 height=80 src="+ img +"></div><br/></div>");               
						}, 0);
     }
   }
);	
}
//]]>
    
