/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;


import java.io.InputStream;

/**
 *
 * @author Himanshu
 */
public abstract class BaseXMLParser implements XmlParser {

    // names of the XML tags
    static final String RESULTSET = "resultset ";
    static final  String ROW = "row";
    static final String NAME = "name";
    static final  String GENDER = "gender";
    static final  String RANK = "rank";
    static final  String BRAND = "brand";
    static final  String PRODUCT = "product";
    static final  String DESCRIPTION = "description";
    static final  String CATEGORY = "category";
    static final  String PICTUREURL = "pictureurl";
    static final String STORES = "stores";
    static final String STORE = "store";
    static final String SHOP = "shop";
    static final String DISTANCE = "distance";
    static final String COMPANYNAME = "companyname";
    static final String STREETNAME = "streetname";
    static final String STREETNUMBER = "streetnumber";
    static final String POSTALCODE = "postalcode";
    static final String TOWN = "town";
    static final String TELEFON = "telefon";
    static final String ADDRESSLNG = "addresslng";
    static final String ADDRESSLAT = "addresslat";
    static final String IMAGESMALL = "imagesmall";
    static final String IMAGELARGE = "imagelarge";
    static final String REGULATOPENINGSTEXT = "regularopeningstext";
    static final String PARKINGINFO = "parkinginfo";
    static final String SHOPURLN = "shopurl";
    static final String DISTANCEE="distance";
    static final String EVENTS = "events";
    static final String EVENT = "event";
    static final String EVENTID = "eventid";
    static final String TOPIC = "topic";
    static final String TYPE = "type";
    static final String APPOINTMENTREQUIRED = "appointmentrequired";
    static final String SCHEDULE = "schedule";
    static final String SCHEDULESHORT = "scheduleshort";
    static final String TEASERIMAGE = "teaserimage";
    static final String TEXT = "text";
    static final String TEXTTEASER = "textteaser";
    static final String REPLACEMENT="100";
    static final String REPLACEMENT1="63";
    static final String TARGET="{size}";
    public static final  String PRICE = "price";
    public static final  String POUND = " €";
    public static final  String PLATZ = ". Platz";
    public static final  String SHORT_DESCRIPTION = "short_description";
    public static final  String LONG_DESCRIPTION = "long_description";
    public static final  String PREVIEWPICTUREURL = "previewpictureurl";
    public static final  String  TITLE= "title";
    public static final  String  VIDEO_LENGTH= "video_length";
    public static final  String  VIDEOURL= "videourl";
    public static final  String  MIN= " min";
    public static final String SHOPURL="shopurl";
    public static final String DCEXCLUSIVE="dcexclusive";
    final InputStream XmlInputStream;

    protected BaseXMLParser(InputStream xmlInputStream){
        try {
            this.XmlInputStream = xmlInputStream;
        } catch (Exception e) {
            throw new RuntimeException("Inputstream Error");

        }
    }

    protected InputStream getInputStream() {
        try {
            return XmlInputStream;
        } catch (Exception e) {
            throw new RuntimeException("Inputstream Error");
        }
    }
}

