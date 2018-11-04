/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;


public class TopTen {
private String name;
private String resultset;
private String row;
private String gender;
private String rank;
private String brand;
private String product;
private String description;
private String category;
private String shopurl;
private String pictureurl;


    public TopTen() {
    }

    public TopTen(String name, String gender, String rank, String brand, String product, String description, String category, String shopurl, String pictureurl) {
        this.name = name;
        this.gender = gender;
        this.rank = rank;
        this.brand = brand;
        this.product = product;
        this.description = description;
        this.category = category;
        this.shopurl=shopurl; 
        this.pictureurl = pictureurl;
    }

    

    public String getBrand() {
        return brand;
    }

    public void setBrand(String brand) {
        this.brand = brand;
    }

    public String getCategory() {
        return category;
    }

    public void setCategory(String category) {
        this.category = category;
    }

    public String getDescription() {
        return description;
    }

    public String getGender() {
        return gender;
    }

    public String getName() {
        return name;
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

    public String getRank() {
        return rank;
    }

    public String getResultset() {
        return resultset;
    }

    public String getRow() {
        return row;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setGender(String gender) {
        this.gender = gender;
    }

    public void setName(String name) {
        this.name = name;
    }
    public void setShopurl(String shopurl) {
        this.shopurl = shopurl;
    }
    public void setPictureurl(String pictureurl) {
        this.pictureurl = pictureurl;
    }

    public void setProduct(String product) {
        this.product = product;
    }

    public void setRank(String rank) {
        this.rank = rank;
    }

    public void setResultset(String resultset) {
        this.resultset = resultset;
    }

    public void setRow(String row) {
        this.row = row;
    }
    

}
