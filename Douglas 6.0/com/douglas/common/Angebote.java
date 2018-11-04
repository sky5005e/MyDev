/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;

public class Angebote {

    private String resultset;
    private String row;
    private String brand;
    private String product;
    private String description;
    private String price;
    private String pictureurl;

    public Angebote() {
    }

    public Angebote(String brand, String product, String description, String price, String pictureurl) {
        this.brand = brand;
        this.product = product;
        this.description = description;
        this.price = price;
        this.pictureurl = pictureurl;
    }

    public String getBrand() {
        return brand;
    }

    public String getDescription() {
        return description;
    }

    public String getPictureurl() {
        return pictureurl;
    }

    public String getPrice() {
        return price;
    }

    public String getProduct() {
        return product;
    }

    public String getResultset() {
        return resultset;
    }

    public String getRow() {
        return row;
    }

    public void setBrand(String brand) {
        this.brand = brand;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setPictureurl(String pictureurl) {
        this.pictureurl = pictureurl;
    }

    public void setPrice(String price) {
        this.price = price;
    }

    public void setProduct(String product) {
        this.product = product;
    }

    public void setResultset(String resultset) {
        this.resultset = resultset;
    }

    public void setRow(String row) {
        this.row = row;
    }
    
}
