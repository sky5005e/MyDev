/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;

/**
 *
 * @author Himanshu
 */
public class Shop {

    private String companyName;
    private String streetName;
    private String streetNumber;
    private String postalCode;
    private String town;
    private String telefon;
    private String addresslng;
    private String addresslat;
    private String imageSmall;
    private String imageLarge;
    private String regularOpeningsText;
    private String parkingInfo;
    
    public Shop() {
    }

    public Shop( String companyName, String streetName, String streetNumber, String postalCode, String town, String telefon, String addresslng, String addresslat, String imageSmall, String imageLarge, String regularOpeningsText, String parkingInfo) {
        this.companyName = companyName;
        this.streetName = streetName;
        this.streetNumber = streetNumber;
        this.postalCode = postalCode;
        this.town = town;
        this.telefon = telefon;
        this.addresslng = addresslng;
        this.addresslat = addresslat;
        this.imageSmall = imageSmall;
        this.imageLarge = imageLarge;
        this.regularOpeningsText = regularOpeningsText;
        this.parkingInfo = parkingInfo;
    }

    public Shop(String streetName, String streetNumber, String postalCode, String town, String imageSmall) {
        this.streetName = streetName;
        this.streetNumber = streetNumber;
        this.postalCode = postalCode;
        this.town = town;
        this.imageSmall = imageSmall;
    }

    public String getAddresslat() {
        return addresslat;
    }

    public void setAddresslat(String addresslat) {
        this.addresslat = addresslat;
    }

    public String getAddresslng() {
        return addresslng;
    }

    public void setAddresslng(String addresslng) {
        this.addresslng = addresslng;
    }

    public String getCompanyName() {
        return companyName;
    }

    public void setCompanyName(String companyName) {
        this.companyName = companyName;
    }

    public String getImageLarge() {
        return imageLarge;
    }

    public void setImageLarge(String imageLarge) {
        this.imageLarge = imageLarge;
    }

    public String getImageSmall() {
        return imageSmall;
    }

    public void setImageSmall(String imageSmall) {
        this.imageSmall = imageSmall;
    }

    public String getPostalCode() {
        return postalCode;
    }

    public void setPostalCode(String postalCode) {
        this.postalCode = postalCode;
    }

    public String getRegularOpeningsText() {
        return regularOpeningsText;
    }

    public void setRegularOpeningsText(String regularOpeningsText) {
        this.regularOpeningsText = regularOpeningsText;
    }

    public String getStreetName() {
        return streetName;
    }

    public void setStreetName(String streetName) {
        this.streetName = streetName;
    }

    public String getStreetNumber() {
        return streetNumber;
    }

    public void setStreetNumber(String streetNumber) {
        this.streetNumber = streetNumber;
    }

    public String getTelefon() {
        return telefon;
    }

    public void setTelefon(String telefon) {
        this.telefon = telefon;
    }

    public String getTown() {
        return town;
    }

    public void setTown(String town) {
        this.town = town;
    }

    public String getParkingInfo() {
        return parkingInfo;
    }

    public void setParkingInfo(String parkingInfo) {
        this.parkingInfo = parkingInfo;
    }

}
