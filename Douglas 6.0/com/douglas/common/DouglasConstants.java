/*
 * douglasconstants.java
 *
 * © One-Associates Technologies Pvt Ltd., 2011
 *  info@one-associates.com
 */

package com.douglas.common;
import com.douglas.main.*;
import net.rim.device.api.ui.Font;
import net.rim.device.api.system.*;


/**
 * Class for define app level static values.
 */
public class DouglasConstants {
    
    //Font Definitions
    public static Font titlefont = Font.getDefault().derive(Font.BOLD, 18);
    public static Font mediumfont = Font.getDefault().derive(Font.PLAIN, 16);
    public static Font mfont = Font.getDefault().derive(Font.BOLD, 14);
    public static Font msfont = Font.getDefault().derive(Font.PLAIN, 11);
    public static Font smfont = Font.getDefault().derive(Font.PLAIN, 13);
    public static Font vsmfont = Font.getDefault().derive(Font.PLAIN, 8);
    public static Font menufont = Font.getDefault().derive(Font.BOLD, 16);
    public static Font fonts = Font.getDefault().derive(Font.PLAIN, 21);
    public static Font pfont = Font.getDefault().derive(Font.PLAIN, 18);
    public static Font dfont = Font.getDefault().derive(Font.BOLD, 16);
    //Colors Definitions
    public static String hexLightBlue = "EBF6F8";
    public static String hexWhite = "FFFFFF";
    public static String hexTitleText = "4ABAC6";
    public static String hexSubTitleText = "737973";
    public static String hexBackground = "C4EDF1";
    public static String CustomField="72CAD5";
    public static String hexBlack="202020"; 
    public static String beautyTitle="60D7D7";
    public static String Finderbg="BDEBEF";
    public static String EventCounts="088194";
  
    //Menu Definitions
    public static String HOMEMenu = "Menü";
    public static String STOREMenu = "Parfumerie";
    public static String NEWSMenu = "Neuheiten";
    public static String VIDEOMenu = "Video";
    public static String ANGEBOTEMenu = "Angebote";  
    public static String HAUTTYPTESTMenu = "Hauttyptest";  
    public static String TopTenMenu="TopTen";
    public static String NONE = "None";
    public static String OrderSuche="oder Suche nach:";
    public static String DarfDouglas="Darf 'Douglas' Ihren aktuellen Ort";
    public static String Verwenden="verwenden?";
    public static String RIGHTMenu="Abbrechen";
    public static String Plz="PLZ    ";
    public static String Stadt="Stadt ";
    public static String Suchen="      Suchen       ";
    public static String Ok="          OK          ";
    public static String Info="Info";
    public static String Events="Events";
    public static String Beauty="Beauty-Services";
    public static String neue="Neue Suche";
    public static String ParfumerienMit="Parfumerien mit diesem Event";
    public static String Space="  ";
    public static String Shop="Shop";
    public static String Karte="Karte";
    
    
    //Footer Menu Image 
    public static String FooterHOMEMenuImageON = "res/drawable/menu_on.png";
    public static String FooterHOMEMenuImageOFF = "res/drawable/menu_off.png";
    public static String FooterSTOREMenuImageON = "res/drawable/parfumerie_on.png";
    public static String FooterSTOREMenuImageOFF = "res/drawable/parfumerie_off.png";
    public static String FooterNEWSMenuImageON = "res/drawable/neu_on.png";
    public static String FooterNEWSMenuImageOFF = "res/drawable/neu_off.png";
    public static String FooterVIDEOMenuImageON = "res/drawable/video_on.png";
    public static String FooterVIDEOMenuImageOFF = "res/drawable/video_off.png";
    public static String FooterANGEBOTEMenuImageON = "res/drawable/shop_on.png"; 
    public static String FooterANGEBOTEMenuImageOFF = "res/drawable/shop_off.png";  
    public static String zumshop= "res/drawable/zumshop_off.jpg";
    public static String zumshopon="res/drawable/zumshop_on.jpg";
    public static String MenuStoreImageOn="res/drawable/Parf_events_button_on.png";
    public static String MenuStoreImageOff="res/drawable/parfumerie_events_button.png";
    public static String MenuNeuImageOn ="res/drawable/neuheiten_button_on.png";
    public static String MenuNeuImageOff ="res/drawable/neuheiten_button.png";
    public static String MenuHauttyptestImageOn ="res/drawable/hauttyptest_button_on.png";
    public static String MenuHauttyptestImageOff ="res/drawable/hauttyptest_button.png";
    public static String MenuTopTenImageOn ="res/drawable/top10_button_on.png";
    public static String MenuTopTenImageOff ="res/drawable/top10_button.png";
    public static String MenuTVImageOn ="res/drawable/Looks_TV_button_on.png";
    public static String MenuTVImageOff ="res/drawable/looks_spots_button.png";
    public static String MenuShopImageOn ="res/drawable/Shop_kachel_on.png";
    public static String MenuShopImageOff ="res/drawable/shop_button.png";
    public static String HandSet="res/drawable/phoneIcon@2x.png";
    public static String DouglasLogo="res/drawable/icon.png";
    //header Menu Buttons
    public static String HeaderMenuImageON = "res/drawable/menub_on.png";
    public static String HeaderMenuImageOFF = "res/drawable/menub_off.png";
    public static String HeaderMenuBackImageON = "res/drawable/menu_2_on.png";
    public static String HeaderMenuBackImageOFF = "res/drawable/menu_2_off.png";
    public static String HeaderNeuImageON = "res/drawable/neuheiten_on.png";
    public static String HeaderNeuImageOFF = "res/drawable/neuheiten_off.png";
    public static String HeaderTopBackImageON = "res/drawable/top10_on.png";
    public static String HeaderTopBackImageOFF = "res/drawable/top10_off.png";
    
    public static String HeaderSuchBackImageON = "res/drawable/suchergebnis_on.png";
    public static String HeaderSuchBackImageOFF = "res/drawable/suchergebnis_off.png";
    public static String HeaderEventsBackImageON = "res/drawable/events_btn_on.png";
    public static String HeaderEventsBackImageOFF = "res/drawable/events_btn_on.png";
    public static String HeaderAbbrechenImageON = "res/drawable/abbrechen_on.png";
    public static String HeaderAbbrechenImageOFF = "res/drawable/abbrechen_off.png";
    public static String HeaderLupeImageON = "res/drawable/lupe_on.png";
    public static String HeaderLupeImageOFF = "res/drawable/lupe_off.png";
    public static String HeaderOkImageON = "res/drawable/OK_on.png";
    public static String HeaderOkImageOFF = "res/drawable/OK_off.png";
    public static String HeaderSuchenImageON = "res/drawable/suchen_on.png";
    public static String HeaderSuchenImageOFF = "res/drawable/suchen_off.png";
   
    public static String HeaderNeueSucheImageON = "res/drawable/neue_suche_on.png";
    public static String HeaderNeueSucheImageOFF = "res/drawable/neue_suche_off.png";
    //Prev Next Menu
    public static String prevImage = "res/drawable/arrow_left.png";
    public static String nextImage = "res/drawable/arrow_right.png";
    
    //Tab Names
    public static String DamenTab = "res/drawable/top10_damen_off.png";
    public static String HerrenTab = "res/drawable/top10_herren_off.png";
    public static String DamenTabOn = "res/drawable/top10_damen_on.png";
    public static String HerrenTabOn = "res/drawable/top10_herren_on.png";
    public static String MakeupTab = "res/drawable/make-up_vid_off.png";
    public static String TVSpotTab = "res/drawable/tv-spots_off.png";
    public static String MakeupTabOn = "res/drawable/make-up_vid_on.png";
    public static String TVSpotTabOn = "res/drawable/tv-spots_on.png";
    public static String ParfumerieTab = "res/drawable/parfumerien_off.png";
    public static String ParfumerieTabOn = "res/drawable/parfumerien_on.png";
    public static String EventsTab = "res/drawable/events_off.png";
    public static String EventsTabOn = "res/drawable/events_on.png";
    public static String InfoTabOn = "res/drawable/info_on.png";
    public static String EventTab = "res/drawable/event_off.png";
    public static String InfoTab = "res/drawable/info_off.png";
    public static String EventTabOn = "res/drawable/event_on.png";
    public static String BeautyServicesTab = "res/drawable/beauty_services_off.png";
    public static String BeautyServicesTabOn = "res/drawable/beauty_services_on.png";
    public static String ParMitTab = "res/drawable/parf_mit_event_off.png";
    public static String ParMitTabOn = "res/drawable/parf_mit_event_on.png";
    public static String ZuruckOn = "res/drawable/zuruck_off.png";
    public static String ZuruckOff = "res/drawable/zuruck_on.png";
    //Beauty Services Image
    public static String ic="res/drawable/chk.jpeg";
    public static String service="res/drawable/beauty_services.jpg";
    
    
    
    //URL's
    public static final String BASE_URL = "http://douglasapp.solidground.de/";
    public static String TopTenDamenURL = "content/top10w_de.db.xml";
    public static String TopTenHerrenURL = "content/top10m_de.db.xml";
    public static String MakeupURL = "content/videosM_de.db.xml";
    public static String TVSpotURL = "content/videosT_de.db.xml";
    public static String AngeboteURL = "content/offers_de.db.xml";
    public static String NeuheitenURL = "content/news_de.db.xml";
    public static String PerfumerieURL="getStoresFromCoords.php5?offset=0";
  

    //Common Image URLS
    public static String DouglasLogoSmall = "res/drawable/D_bb480x44.png";
    public static String MenuBackground = "res/drawable/D_text_header.png";
    public static String MenuSearch = "res/drawable/lupe_44x44_shadow@2x.png";
    public static String BackgroundBright="res/drawable/bg_bright.png";


    //Additional Menu Tags
    
    
    //Menu Title
    public static String HomeMenu = "Menü";
    public static String TopTen = "Top 10";
    public static String TopTenDamen = "Top 10 Damen";
    public static String TopTenHerren = "Top 10 Herren";
    public static String Videos = "Videos";
    public static String Neu = "Neuheiten";
    public static String Suchergebnis="Suchergebnis";
    
    //Static Screen Number
    public static final int NeuheitenScreenNumber = 2;
    public static final int TopTenScreenNumber = 3;
    public static final int MaternusScreenNumber = 4;
    public static final int MaternusEventScreenNumber = 5; 
    public static final int PerfumerieScreenNumber=6;
    //Static Strings
    public static final String PLATZ = "Platz";
    public static final String MINUTETEXT = " min ";
    public static final String EUROSIGN = " € ";
    public static final String DISTANCE = " m ";
    public static final String KMDISTANCE = " km ";
    public static final String Hypen ="-";
    public static final String Bitte="*Bitte vereinbaren Sie zuvor einen Termin:";
    public static final String Rufen="Rufen Sie uns an unter";
    public static final String BeautyServices="Unser Douglas Team bietet Ihnen vor Ort folgende Beauty Services:";
    public static final String Douglas="Douglas";
    public static final String Offnungszeiten="Öffnungszeiten";
    public static final String Parkmöglichkeiten ="Parkmöglichkeiten";
    public static final String Route="Route berechnen";
    public static final String ThrLabel="Ihr Weg zu uns  Distanz:";
    public static final String Hauttyptest="Hauttyptest";
    public static final String ZUMSHOP="Shop";
    public static final String Event="Event";
    //Error Messages
    public static final String parseError="ES tut uns leid,z.Z.sind keine Daten verfügbar";
    public static final String gpsError="Warten Sie bitte, während die Standortbestimmung geladen wird. Ihr Endgerät muss hierzu Javascript unterstützen.";
    public DouglasConstants() {    
    
    }
} 

