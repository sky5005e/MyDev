/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;

public class Neuheiten {
    private String resultset;
    private String row;
    private String brand;
    private String product;
    private String short_desc;
    private String long_desc;
    private String category;
    private String shopurl;
    private String pictureurl;

    public Neuheiten() {
    }

    public Neuheiten(String brand, String product, String short_desc, String long_desc, String category, String shopurl, String pictureurl) {
        this.brand = brand;
        this.product = product;
        this.short_desc = short_desc;
        this.long_desc = long_desc;
        this.category = category;
        this.shopurl=shopurl;
        this.pictureurl = pictureurl;
    }

    public String getBrand() {
        return brand;
    }

    public String getCategory() {
        return category;
    }

    public String getLong_desc() {
        return long_desc;
    }
    public String getShopurl(){
        return shopurl;
    }
    public String getPictureurl() {
        return pictureurl;
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

    public String getShort_desc() {
        return short_desc;
    }

    public void setBrand(String brand) {
        this.brand = brand;
    }

    public void setCategory(String category) {
        this.category = category;
    }
     public void setShopurl(String shopurl) {
        this.shopurl = shopurl;
    }
    public void setLong_desc(String long_desc) {
        this.long_desc = long_desc;
    }

    public void setPictureurl(String pictureurl) {
        this.pictureurl = pictureurl;
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

    public void setShort_desc(String short_desc) {
        this.short_desc = short_desc;
    }
}
