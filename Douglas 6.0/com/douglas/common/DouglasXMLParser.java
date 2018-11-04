/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;

import java.io.InputStream;
import java.util.Vector;
import net.rim.device.api.xml.parsers.SAXParser;
import net.rim.device.api.xml.parsers.SAXParserFactory;
import org.xml.sax.Attributes;
import org.xml.sax.SAXException;
import org.xml.sax.helpers.DefaultHandler;
 
public class DouglasXMLParser extends BaseXMLParser
{
    
    public DouglasXMLParser(InputStream feedStream) {
        super(feedStream);
    }

    private boolean insideShopCompanyName, insideStoreDistance,insideShopStreetName,insideShopStreetNumber,insideShopTelefNo, insideShopAddressLat,insideShopImageSmall,insideShopImageLarge,insideShopRegularOpeningsText,insideShopParkingInfo,insideStoreDistances,insideEventsDistance,insideEventsEventID, insideEventsTopic, insideEventsType, insideEventSchedule, insideEventsScheduleShort, insideEventsTeaserImage, insideEventsText, insideEventsTextTeaser, insideRowName, insideRowGender, insideRowRank, insideRowBrand, insideRowProduct, insideRowDescription, insideRowCategory,insideRowShopurl, insideRowPictureurl,insideNeuheitenBrand,insideNeuheitenProduct,insideNeuheitenShortDescription,insideNeuheitenLongDescription,insideNeuheitenCategory,insideNeuheitenShopurl,insideNeuheitenPictureurl,insideAngeboteBrand,insideAngeboteProduct,insideAngeboteDescription,insideAngebotePrice,insideAngebotePictureurl, insideLooksTVSpotsTitle,insideLooksTVSpotsDescription,insideLooksTVSpotsPreviewPictureurl,insideLooksTVSpotsVideourl,insideLooksTVSpotsVideoLength,insideShopTown,insideShopPostalCode,insideEventsExclusive,insideShopAddressLng,insideAppointmentRequired ;


    public Store[] parseStores()
    {

        final Vector eventVector = new Vector();
        final Vector storeVector = new Vector();
        final Vector shopVector=new Vector();      
        try {
              
              final Store currentStore = new Store();
              final Event currentEvent = new Event();
              final Shop currentShop = new Shop();
                          
              SAXParserFactory factory = SAXParserFactory.newInstance();
              SAXParser saxParser = factory.newSAXParser();
 
              DefaultHandler handler = new DefaultHandler() {
 
                public void startElement(String uri, String localName,
                    String qName, Attributes attributes)
                    throws SAXException {
                     setStartTag(qName);
                }
 
                public void endElement(String uri, String localName,
                        String qName)
                        throws SAXException {

                        if (qName.equals(EVENT))
                        {
                            eventVector.addElement(new Event(currentEvent.getEventId(),currentEvent.getTopic(),currentEvent.getType(), currentEvent.getAppointment() ,currentEvent.getSchedule(),currentEvent.getScheduleShort(), currentEvent.getText(),currentEvent.getTeasertText(),currentEvent.getTeaserImage(),currentEvent.getDistance(),currentEvent.getExclusive()));
                        }
                        else if (qName.equals(STORE))
                        {
                            Shop _tShop = new Shop(currentShop.getCompanyName(),currentShop.getStreetName(),currentShop.getStreetNumber(),currentShop.getPostalCode(),currentShop.getTown(),currentShop.getTelefon(),currentShop.getAddresslng(),currentShop.getAddresslat(),currentShop.getImageSmall(),currentShop.getImageLarge(),currentShop.getRegularOpeningsText(),currentShop.getParkingInfo());
                            storeVector.addElement(new Store(currentStore.getDistance(),_tShop ,vectorToEventArray(eventVector)));
                            eventVector.removeAllElements();
                       }

                        setEndTag(qName);
                }
 
                public void characters(char ch[], int start, int length)
                    throws SAXException {

                    if (insideStoreDistance)
                    {
                        currentStore.setDistance(new String(ch, start, length));
                    }
                    else if (insideShopCompanyName || insideShopStreetName || insideShopStreetNumber || insideShopPostalCode || insideShopTown || insideShopTelefNo || insideShopAddressLng || insideShopAddressLat ||  insideShopImageSmall || insideShopImageLarge || insideShopRegularOpeningsText || insideShopParkingInfo)
                    {
                        setShopValue(currentShop, new String(ch, start, length));
                    }
                    else if (insideEventsEventID ||  insideEventsTopic || insideEventsType || insideAppointmentRequired || insideEventSchedule || insideEventsScheduleShort || insideEventsText || insideEventsTextTeaser || insideEventsTeaserImage || insideEventsDistance || insideEventsExclusive)
                    {
                        setEventValue(currentEvent,new String(ch, start, length));
                    }
              }};

              saxParser.parse(this.getInputStream(), handler);

            
        }
        catch (Exception e) {
              e.printStackTrace();
        }
        return vectorToStoreArray(storeVector);
 
    }

    private void setStartTag(String qName)
    {
            if (qName.equals(STORES))
            {
                        
                //Add the currentStore to Array
            }
            else if (qName.equals(COMPANYNAME))
            {
                insideShopCompanyName = true;
            }else if (qName.equals(STREETNAME))
            {
                insideShopStreetName = true;
            }
            else if (qName.equals(STREETNUMBER))
            {
                insideShopStreetNumber = true;
            }else if(qName.equals(POSTALCODE))
            {
                insideShopPostalCode=true;
            }else if(qName.equals(TOWN))
            {
                insideShopTown=true;
            }else if (qName.equals(TELEFON))
            {
                insideShopTelefNo = true;
            }else if (qName.equals(ADDRESSLAT))
            {
                insideShopAddressLat = true;
            }else if (qName.equals(ADDRESSLNG))
            {
                insideShopAddressLng = true;
            }
            else if (qName.equals(IMAGESMALL))
            {
                insideShopImageSmall = true;
            }else if (qName.equals(IMAGELARGE))
            {
                insideShopImageLarge = true;
            }else if (qName.equals(REGULATOPENINGSTEXT))
            {
                insideShopRegularOpeningsText = true;
            }else if (qName.equals(PARKINGINFO))
            {
                insideShopParkingInfo = true;
            }
            else if (qName.equals(DISTANCE))
            {
                insideStoreDistance = true;
                
            }else if(qName.equals(DISTANCEE))
            {
                insideEventsDistance=true;
            }
            else if (qName.equals(EVENTID))
            {
                insideEventsEventID = true;
            }else if(qName.equals(DCEXCLUSIVE))
            {
                insideEventsExclusive=true;
            } else if (qName.equals(TOPIC))
            {
                insideEventsTopic = true;
            }else if (qName.equals(TYPE))
            {
                insideEventsType = true;
            }else if(qName.equals(APPOINTMENTREQUIRED))
            {
                insideAppointmentRequired=true;
            }
            else if (qName.equals(SCHEDULE))
            {
                insideEventSchedule = true;
            }else if (qName.equals(SCHEDULESHORT))
            {
               insideEventsScheduleShort= true;
            }else if (qName.equals(TEASERIMAGE))
            {
                insideEventsTeaserImage = true;
            }else if (qName.equals(TEXT))
            {
                insideEventsText = true;
            }else if (qName.equals(TEXTTEASER))
            {
                     
                insideEventsTextTeaser = true;
            }else if (qName.equals(NAME))
            {
                insideRowName = true;
            }else if (qName.equals(GENDER))
            {
                insideRowGender = true;
            }else if (qName.equals(RANK))
            {
                insideRowRank = true;
            }else if (qName.equals(BRAND))
            {
                insideRowBrand = true;
            }else if (qName.equals(PRODUCT))
            {
                insideRowProduct = true;
            }else if (qName.equals(DESCRIPTION))
            {
                 insideRowDescription = true;
            }else if (qName.equals(CATEGORY))
            {
                insideRowCategory = true;
            }else if(qName.equals(SHOPURL))
            {
                insideRowShopurl=true;
                
            }
            else if (qName.equals(PICTUREURL))
            { 
                insideRowPictureurl = true;
            }
    
    }

    private void setEndTag(String qName)
    {
            if (qName.equals(STORES))
            {
                //Add the currentStore to Array
            }
            else if (qName.equals(COMPANYNAME))
            {
                insideShopCompanyName = false;
            }
            else if (qName.equals(STREETNUMBER))
            {
                insideShopStreetNumber = false;
            }
            else if (qName.equals(DISTANCE))
            {
                insideStoreDistance = false;
            }
            else if (qName.equals(EVENTID))
            {
                insideEventsEventID = false;
            }
    }

    private Shop setShopValue(Shop currentShop, String value)
    {
        if (insideShopCompanyName)
        {
            currentShop.setCompanyName(value);
            insideShopCompanyName = false;
        }else if (insideShopStreetName)
        {
            currentShop.setStreetName(value);
            insideShopStreetName = false;
        }
        else if (insideShopStreetNumber)
        {
            currentShop.setStreetNumber(value);
            insideShopStreetNumber = false;
        }else if(insideShopPostalCode)
        {
            currentShop.setPostalCode(value);
             insideShopPostalCode=false;
        }else if(insideShopTown)
        {
           currentShop.setTown(value);
           insideShopTown=false; 
        }else if (insideShopTelefNo)
        {
            currentShop.setTelefon(value);
            insideShopTelefNo = false;
        }else if (insideShopAddressLng)
        {
            currentShop.setAddresslng(value);
            insideShopAddressLng = false;
        }else if (insideShopAddressLat)
        {
            currentShop.setAddresslat(value);
            insideShopAddressLat = false;
        }else if (insideShopImageSmall)
        {
            currentShop.setImageSmall(value);
            insideShopImageSmall = false;
        }else if (insideShopImageLarge)
        {
            currentShop.setImageLarge(value);
            insideShopImageLarge = false;
        }else if (insideShopRegularOpeningsText)
        {
            currentShop.setRegularOpeningsText(value);
            insideShopRegularOpeningsText = false;
        }else if(insideShopParkingInfo)
                {
            currentShop.setParkingInfo(value);
            insideShopParkingInfo = false;
        }
        return currentShop;
        
    }

    private Event setEventValue(Event currentEvent, String value)
    {
        if (insideEventsEventID)
        {
            currentEvent.setEventId(value);
            insideEventsEventID = false;
        }else if (insideEventsTopic)
        {
            currentEvent.setTopic(value);
            insideEventsTopic = false;
        }else if (insideEventsType)
        {
            currentEvent.setType(value);
            insideEventsType = false;
        }else if(insideAppointmentRequired)
        {
           currentEvent.setAppointment(value);
           insideAppointmentRequired = false; 
        }
        else if (insideEventSchedule)
        {
            currentEvent.setSchedule(value);
            insideEventSchedule = false;
        }else if (insideEventsScheduleShort)
        {
            currentEvent.setScheduleShort(value);
            insideEventsScheduleShort = false;
        }else if (insideEventsTeaserImage)
        {
            currentEvent.setTeaserImage(value);
            insideEventsTeaserImage = false;
        }else if (insideEventsText)
        {
            currentEvent.setText(value);
            insideEventsText = false;
        }else if (insideEventsTextTeaser)
        {
            currentEvent.setTeasertText(value);
            insideEventsEventID = false;
        }else if(insideEventsDistance)
        {
            currentEvent.setDistance(value);
            insideEventsDistance=false;
        }else if(insideEventsExclusive)
        {
            currentEvent.setExclusive(value);
            insideEventsExclusive=false;
        }
        return currentEvent;

    }

    public static Event[] vectorToEventArray(Vector vector)
    {
        Event[] array =  new Event[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Event) vector.elementAt(i);
        }
        return array;
    }
    public static Shop[] vectorToShoptArray(Vector vector)
    {
        Shop[] array =  new Shop[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Shop) vector.elementAt(i);
        }
        return array;
    }
    
    public static Store[] vectorToStoreArray(Vector vector)
    {
        Store[] array =  new Store[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Store) vector.elementAt(i);
        }
        return array;
    }
/**********************************************Event******************************************************/
public Event[] parseEvents()
    {

        final Vector eventVector = new Vector();
        
              
        try {
              
              final Event currentEvent = new Event();
              
                          
              SAXParserFactory factory = SAXParserFactory.newInstance();
              SAXParser saxParser = factory.newSAXParser();
 
              DefaultHandler handler = new DefaultHandler() {
 
                public void startElement(String uri, String localName,
                    String qName, Attributes attributes)
                    throws SAXException {
                     setStartTagEvents(qName);
                }
 
                public void endElement(String uri, String localName,
                        String qName)
                        throws SAXException {

                        if (qName.equals(EVENT))
                        {
                            eventVector.addElement(new Event(currentEvent.getEventId(),currentEvent.getTopic(),currentEvent.getType(),currentEvent.getAppointment(),currentEvent.getSchedule(),currentEvent.getScheduleShort(),currentEvent.getText(),currentEvent.getTeasertText(),currentEvent.getTeaserImage(),currentEvent.getDistance(),currentEvent.getExclusive()));
                        }

                        setEndTag(qName);
                }
 
                public void characters(char ch[], int start, int length)
                    throws SAXException {

                    if (insideEventsEventID || insideEventsTopic || insideEventsType || insideEventSchedule || insideEventsScheduleShort || insideEventsText || insideEventsTextTeaser || insideEventsTeaserImage || insideEventsDistance || insideEventsExclusive)
                    {
                        setEventsValue(currentEvent,new String(ch, start, length));
                    }
                    
              }};

              saxParser.parse(this.getInputStream(), handler);

            
        }
        catch (Exception e) {
              e.printStackTrace();
        }
        return vectorToEventsArray(eventVector);
 
    }
     private void setStartTagEvents(String qName)
    {
           
            if (qName.equals(DISTANCE))
            {
                insideEventsDistance = true;
            }else if (qName.equals(EVENTID))
            {
                insideEventsEventID= true;
            }else if(qName.equals(DCEXCLUSIVE))
            {
                insideEventsExclusive = true;
            }else if (qName.equals(TOPIC))
            {
                insideEventsTopic = true;
            }else if (qName.equals(TYPE))
            {
                insideEventsType = true;
            }else if (qName.equals(SCHEDULE))
            {
                insideEventSchedule = true;
            }else if (qName.equals(SCHEDULESHORT))
            {
               insideEventsScheduleShort= true;
            }else if (qName.equals(TEASERIMAGE))
            {
                insideEventsTeaserImage = true;
            }else if (qName.equals(TEXT))
            {
                insideEventsText = true;
            }else if (qName.equals(TEXTTEASER))
            {
                     
                insideEventsTextTeaser = true;
            }
    }

  
     private Event setEventsValue(Event currentEvent, String value)
    {
        if(insideEventsDistance)
        {
           currentEvent.setDistance(value);
            insideEventsDistance=false;
        }else if (insideEventsEventID)
        {
            currentEvent.setEventId(value);
            insideEventsEventID = false;
        }else if(insideEventsExclusive)
        {
           currentEvent.setExclusive(value);
           insideEventsExclusive=false; 
        }
        else if (insideEventsTopic)
        {
            currentEvent.setTopic(value);
            insideEventsTopic = false;
        }else if (insideEventsType)
        {
            currentEvent.setType(value);
            insideEventsType = false;
        }else if (insideEventSchedule)
        {
            currentEvent.setSchedule(value);
            insideEventSchedule = false;
        }else if (insideEventsScheduleShort)
        {
            currentEvent.setScheduleShort(value);
            insideEventsScheduleShort = false;
        }else if (insideEventsTeaserImage)
        {
            currentEvent.setTeaserImage(value);
            insideEventsTeaserImage = false;
        }else if (insideEventsText)
        {
            currentEvent.setText(value);
            insideEventsText = false;
        }else if (insideEventsTextTeaser)
        {
            currentEvent.setTeasertText(value);
            insideEventsTextTeaser = false;
        }
   
        return currentEvent;

    }

    public static Event[] vectorToEventsArray(Vector vector)
    {
        Event[] array =  new Event[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Event) vector.elementAt(i);
        }
        return array;
    }

/********************************************parse top ten************************************************/
 public TopTen[] parseTopTen()
    {

        final Vector toptenVector = new Vector();
        
              
        try {
              
              final TopTen currentRow = new TopTen();
              
                          
              SAXParserFactory factory = SAXParserFactory.newInstance();
              SAXParser saxParser = factory.newSAXParser();
 
              DefaultHandler handler = new DefaultHandler() {
 
                public void startElement(String uri, String localName,
                    String qName, Attributes attributes)
                    throws SAXException {
                     setStartTag(qName);
                }
 
                public void endElement(String uri, String localName,
                        String qName)
                        throws SAXException {

                        if (qName.equals(ROW))
                        {
                            toptenVector.addElement(new TopTen(currentRow.getName(),currentRow.getGender(),currentRow.getRank(), currentRow.getBrand(), currentRow.getProduct() , currentRow.getDescription(), currentRow.getCategory() , currentRow.getShopurl(), currentRow.getPictureurl()));
                        }

                        setEndTag(qName);
                }
 
                public void characters(char ch[], int start, int length)
                    throws SAXException {

                    if (insideRowName || insideRowGender || insideRowRank || insideRowBrand || insideRowProduct || insideRowDescription || insideRowCategory || insideRowShopurl || insideRowPictureurl)
                    {
                        setRowValue(currentRow, new String(ch, start, length));
                    }
                    
              }};

              saxParser.parse(this.getInputStream(), handler);

            
        }
        catch (Exception e) {
              e.printStackTrace();
        }
        return vectorToTopTenArray(toptenVector);
 
    }
    private TopTen setRowValue(TopTen currentRow, String value)
    {
        if (insideRowName)
        {
            currentRow.setName(value);
            insideRowName= false;
        }else if (insideRowGender)
        {
            currentRow.setGender(value);
            insideRowGender= false;
        }else if (insideRowRank)
        {
            currentRow.setRank(value);
            insideRowRank = false;
        }else if (insideRowBrand)
        {
            currentRow.setBrand(value);
            insideRowBrand = false;
        }else if (insideRowProduct)
        {
            currentRow.setProduct(value);
            insideRowProduct = false;
        }else if (insideRowDescription)
        {
            currentRow.setDescription(value);
            insideRowDescription = false;
        }else if (insideRowCategory)
        {
            currentRow.setCategory(value);
            insideRowCategory = false;
        }else if(insideRowShopurl)
        {
            currentRow.setShopurl(value);
            insideRowShopurl=false;
        }
        else if (insideRowPictureurl)
        {
            currentRow.setPictureurl(value);
            insideRowPictureurl= false;
        }
        return currentRow;

    }
        public static TopTen[] vectorToTopTenArray(Vector vector)
    {
        TopTen[] array =  new TopTen[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (TopTen) vector.elementAt(i);
        }
        return array;
    }
//********************************************parse Neuheiten**********************************************

     public Neuheiten[] parseNeuheiten()
    {

        final Vector neuheitenVector = new Vector();
        
              
        try {
              
              final Neuheiten neuheitenObj = new Neuheiten();
              
                          
              SAXParserFactory factory = SAXParserFactory.newInstance();
              SAXParser saxParser = factory.newSAXParser();
 
              DefaultHandler handler = new DefaultHandler() {
 
                public void startElement(String uri, String localName,
                    String qName, Attributes attributes)
                    throws SAXException {
                     setStartTagNeuheiten(qName);
                }
 
                public void endElement(String uri, String localName,
                        String qName)
                        throws SAXException {

                        if (qName.equals(ROW))
                        {
                            neuheitenVector.addElement(new Neuheiten(neuheitenObj.getBrand(),neuheitenObj.getProduct(), neuheitenObj.getShort_desc() , neuheitenObj.getLong_desc(), neuheitenObj.getCategory(),neuheitenObj.getShopurl(),neuheitenObj.getPictureurl()));
                        }

                        setEndTagNeuheiten(qName);
                }
 
                public void characters(char ch[], int start, int length)
                    throws SAXException {

                    if (insideNeuheitenBrand || insideNeuheitenProduct || insideNeuheitenShortDescription ||insideNeuheitenLongDescription || insideNeuheitenCategory || insideNeuheitenShopurl || insideNeuheitenPictureurl)
                    {
                        setNeuheitenValue(neuheitenObj, new String(ch, start, length));
                    }
                    
              }};

              saxParser.parse(this.getInputStream(), handler);

            
        }
        catch (Exception e) {
              e.printStackTrace();
        }
        return vectorNeuheitenArray(neuheitenVector);
 
    }
        private Neuheiten setNeuheitenValue(Neuheiten neuheitenObj, String value)
    {
        if (insideNeuheitenBrand)
        {
            neuheitenObj.setBrand(value);
            insideNeuheitenBrand= false;
        }else if (insideNeuheitenProduct)
        {
            neuheitenObj.setProduct(value);
            insideNeuheitenProduct= false;
        }else if (insideNeuheitenShortDescription)
        {
            neuheitenObj.setShort_desc(value);
            insideNeuheitenShortDescription= false;
        }else if (insideNeuheitenLongDescription)
        {
            neuheitenObj.setLong_desc(value);
            insideNeuheitenLongDescription= false;
        }else if (insideNeuheitenCategory)
        {
            neuheitenObj.setCategory(value);
            insideNeuheitenCategory = false;
        }else if(insideNeuheitenShopurl)
        {
            neuheitenObj.setShopurl(value);
            insideNeuheitenShopurl=false;
        }
        else if (insideNeuheitenPictureurl)
        {
            neuheitenObj.setPictureurl(value);
            insideNeuheitenPictureurl = false;
        }
        return neuheitenObj;

    }
        public static Neuheiten[] vectorNeuheitenArray(Vector vector)
    {
        Neuheiten[] array =  new Neuheiten[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Neuheiten) vector.elementAt(i);
        }
        return array;
    }
        
        private void setStartTagNeuheiten(String qName)
    {
           
            if (qName.equals(BRAND))
            {
                insideNeuheitenBrand = true;
            }else if (qName.equals(PRODUCT))
            {
                insideNeuheitenProduct= true;
            }else if (qName.equals(SHORT_DESCRIPTION))
            {
                insideNeuheitenShortDescription = true;
            }else if (qName.equals(LONG_DESCRIPTION))
            {
                insideNeuheitenLongDescription = true;
            }else if (qName.equals(CATEGORY))
            {
                insideNeuheitenCategory = true;
            }else if(qName.equals(SHOPURL))
            {
                insideNeuheitenShopurl=true;
            }
            else if (qName.equals(PICTUREURL))
            {
                insideNeuheitenPictureurl = true;
            }
    
    }

    private void setEndTagNeuheiten(String qName)
    {
           
    }
    
    //***********************************************parse Angebote*********************************************

     public Angebote[] parseAngebote()
    {

        final Vector angeboteVector = new Vector();
        
              
        try {
              
              final Angebote angeboteObj = new Angebote();
              
                          
              SAXParserFactory factory = SAXParserFactory.newInstance();
              SAXParser saxParser = factory.newSAXParser();
 
              DefaultHandler handler = new DefaultHandler() {
 
                public void startElement(String uri, String localName,
                    String qName, Attributes attributes)
                    throws SAXException {
                     setStartTagAngebote(qName);
                }
 
                public void endElement(String uri, String localName,
                        String qName)
                        throws SAXException {

                        if (qName.equals(ROW))
                        {
                            angeboteVector.addElement(new Angebote(angeboteObj.getBrand(),angeboteObj.getProduct(), angeboteObj.getDescription() , angeboteObj.getPrice(), angeboteObj.getPictureurl()));
                        }

                        setEndTagAngebote(qName);
                }
 
                public void characters(char ch[], int start, int length)
                    throws SAXException {

                    if (insideAngeboteBrand || insideAngeboteProduct || insideAngeboteDescription ||insideAngebotePrice || insideAngebotePictureurl)
                    {
                        setAngeboteValue(angeboteObj, new String(ch, start, length));
                    }
                    
              }};

              saxParser.parse(this.getInputStream(), handler);

            
        }
        catch (Exception e) {
              e.printStackTrace();
        }
        return vectorAngeboteArray(angeboteVector);
 
    }
        private Angebote setAngeboteValue(Angebote angeboteObj, String value)
    {
        if (insideAngeboteBrand)
        {
            angeboteObj.setBrand(value);
            insideAngeboteBrand= false;
        }else if (insideAngeboteProduct)
        {
            angeboteObj.setProduct(value);
            insideAngeboteProduct= false;
        }else if (insideAngeboteDescription)
        {
            angeboteObj.setDescription(value);
            insideAngeboteDescription= false;
        }else if (insideAngebotePrice)
        {
            angeboteObj.setPrice(value);
            insideAngebotePrice= false;
        }else if (insideAngebotePictureurl)
        {
            angeboteObj.setPictureurl(value);
            insideAngebotePictureurl = false;
        }
        return angeboteObj;

    }
        public static Angebote[] vectorAngeboteArray(Vector vector)
    {
        Angebote[] array =  new Angebote[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (Angebote) vector.elementAt(i);
        }
        return array;
    }
 
       private void setStartTagAngebote(String qName)
    {
           
            if (qName.equals(BRAND))
            { 
                insideAngeboteBrand = true;
            }else if (qName.equals(PRODUCT))
            {
                insideAngeboteProduct= true;
            }else if (qName.equals(DESCRIPTION))
            {
                insideAngeboteDescription = true;
            }else if (qName.equals(PRICE))
            {
                insideAngebotePrice = true;
            }else if (qName.equals(PICTUREURL))
            {
                insideAngebotePictureurl = true;
            }
    
    }

    private void setEndTagAngebote(String qName)
    {
           
    }
//********************************************parse  LooksTVSpots************************************************

public LooksTVSpots[] parselookstvspots()
    {

        final Vector lookstvspotsVector = new Vector();
        
              
        try {
              
              final LooksTVSpots lookstvObj = new LooksTVSpots();
              
                          
              SAXParserFactory factory = SAXParserFactory.newInstance();
              SAXParser saxParser = factory.newSAXParser();
 
              DefaultHandler handler = new DefaultHandler() {
 
                public void startElement(String uri, String localName,
                    String qName, Attributes attributes)
                    throws SAXException {
                     setStartTagLooksTVSpots(qName);
                }
 
                public void endElement(String uri, String localName,
                        String qName)
                        throws SAXException {

                        if (qName.equals(ROW))
                        {
                            lookstvspotsVector.addElement(new LooksTVSpots(lookstvObj.getTitle(),lookstvObj.getDescription(),lookstvObj.getPreviewpictureurl(), lookstvObj.getVideourl(), lookstvObj.getVlength()));
                        }

                        setEndTagLooksTVSpots(qName);
                }
 
                public void characters(char ch[], int start, int length)
                    throws SAXException {

                    if (insideLooksTVSpotsTitle || insideLooksTVSpotsDescription || insideLooksTVSpotsPreviewPictureurl || insideLooksTVSpotsVideourl ||insideLooksTVSpotsVideoLength)
                    {
                        setLooksTVSpotsValue(lookstvObj, new String(ch, start, length));
                    }
                    
              }};

              saxParser.parse(this.getInputStream(), handler);

            
        }
        catch (Exception e) {
              e.printStackTrace();
        }
        return vectorLooksTVSpotsArray(lookstvspotsVector);
 
    }
        private LooksTVSpots setLooksTVSpotsValue(LooksTVSpots lookstvObj, String value)
    {
        if (insideLooksTVSpotsTitle)
        {
            lookstvObj.setTitle(value);
            insideLooksTVSpotsTitle= false;
        }else if (insideLooksTVSpotsDescription)
        {
            lookstvObj.setDescription(value);
            insideLooksTVSpotsDescription= false;
        }else if (insideLooksTVSpotsPreviewPictureurl)
        {
            lookstvObj.setPreviewpictureurl(value);
            insideLooksTVSpotsPreviewPictureurl = false;
        }else if (insideLooksTVSpotsVideourl)
        {
            lookstvObj.setVideourl(value);
            insideLooksTVSpotsVideourl= false;
        }else if (insideLooksTVSpotsVideoLength)
        {
            lookstvObj.setVlength(value);
            insideLooksTVSpotsVideoLength = false;
        }
        return lookstvObj;

    }
        public static LooksTVSpots[] vectorLooksTVSpotsArray(Vector vector)
    {
        LooksTVSpots[] array =  new LooksTVSpots[vector.size()];

        for(int i=0; i<vector.size(); i++)
        {
            array[i] = (LooksTVSpots) vector.elementAt(i);
        }
        return array;
    }
        
        private void setStartTagLooksTVSpots(String qName)
    {
           
            if (qName.equals(TITLE))
            { 
               insideLooksTVSpotsTitle= true;
            }else if (qName.equals(DESCRIPTION))
            {
                insideLooksTVSpotsDescription= true;
            }else if (qName.equals(PREVIEWPICTUREURL))
            {
                insideLooksTVSpotsPreviewPictureurl = true;
            }else if (qName.equals(VIDEOURL))
            {
                insideLooksTVSpotsVideourl = true;
            }else if (qName.equals(VIDEO_LENGTH))
            {
                insideLooksTVSpotsVideoLength = true;
            }
    
    }

    private void setEndTagLooksTVSpots(String qName)
    {
           
    }
}
