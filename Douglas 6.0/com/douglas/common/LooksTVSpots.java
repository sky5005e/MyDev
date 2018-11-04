/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;

public class LooksTVSpots {

    private String resultset;
    private String row;
    private String title;
    private String description;
    private String previewpictureurl;
    private String videourl;
    private String vlength;
    
    public LooksTVSpots() {
    }

    public LooksTVSpots( String title, String description, String previewpictureurl, String videourl, String vlength) {
  
        this.title = title;
        this.description = description;
        this.previewpictureurl = previewpictureurl;
        this.videourl = videourl;
        this.vlength = vlength;
    }

    public String getDescription() {
        return description;
    }

    public String getPreviewpictureurl() {
        return previewpictureurl;
    }

    public String getResultset() {
        return resultset;
    }

    public String getRow() {
        return row;
    }

    public String getTitle() {
        return title;
    }

    public String getVideourl() {
        return videourl;
    }

    public String getVlength() {
        return vlength;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setPreviewpictureurl(String previewpictureurl) {
        this.previewpictureurl = previewpictureurl;
    }

    public void setResultset(String resultset) {
        this.resultset = resultset;
    }

    public void setRow(String row) {
        this.row = row;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public void setVideourl(String videourl) {
        this.videourl = videourl;
    }

    public void setVlength(String vlength) {
        this.vlength = vlength;
    }

}
