/*
 * Store Class
 */

package com.douglas.common;


/**
 *
 * @author One Associates Technologies Pvt Ltd.
 */
public class Store {

    private String distance;
    private Shop shop;
    private Event[] events;

    public Store() {
    }

    public Store(String distance, Shop shop, Event[] events) {
        this.distance = distance;
        this.shop = shop;
        this.events = events;
    }

    public Store(String distance, Shop shop) {
        this.distance = distance;
        this.shop = shop;
    }

    public String getDistance() {
        return distance;
    }

    public void setDistance(String distance) {
        this.distance = distance;
    }

    public Event[] getEvents() {
        return events;
    }

    public void setEvents(Event[] events) {
        this.events = events;
    }

   
    public Shop getShop() {
        return shop;
    }

    public void setShop(Shop shop) {
        this.shop = shop;
    }
    

}
