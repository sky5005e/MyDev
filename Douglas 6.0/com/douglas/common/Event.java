/*
 * Event Class
 */

package com.douglas.common;

/**
 *
 * @author One Associates Technologies Pvt Ltd.
 */
public class Event {

    private String eventId;
    private String topic;
    private String type;
    private String appointmentRequired;
    private String schedule;
    private String scheduleShort;
    private String text;
    private String teasertText;
   // private String image;
    private String teaserImage;
    private String distance;
    private String exclusive;
    public Event() {
    }

    public Event(String eventId, String topic, String type,String appointmentRequired, String schedule, String scheduleShort, String text, String teasertText, String teaserImage,String distance,String exclusive) {
        this.eventId = eventId;
        this.topic = topic;
        this.type = type;
        this.appointmentRequired=appointmentRequired;
        this.schedule = schedule;
        this.scheduleShort = scheduleShort;
        this.text = text;
        this.teasertText = teasertText;
        this.teaserImage = teaserImage;
        this.distance=distance;
        this.exclusive=exclusive;
    }

    public Event(String eventId) {
        this.eventId = eventId;
    }

    public String getEventId() {
        return eventId;
    }

    public void setEventId(String eventId) {
        this.eventId = eventId;
    }

  
    public String getAppointment() {
        return appointmentRequired;
    }

    public void setAppointment(String appointmentRequired) {
        this.appointmentRequired = appointmentRequired;
    }

    public String getSchedule() {
        return schedule;
    }

    public void setSchedule(String schedule) {
        this.schedule = schedule;
    }

    public String getScheduleShort() {
        return scheduleShort;
    }

    public void setScheduleShort(String scheduleShort) {
        this.scheduleShort = scheduleShort;
    }

    public String getTeaserImage() {
        return teaserImage;
    }

    public void setTeaserImage(String teaserImage) {
        this.teaserImage = teaserImage;
    }

    public String getTeasertText() {
        return teasertText;
    }

    public void setTeasertText(String teasertText) {
        this.teasertText = teasertText;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public String getTopic() {
        return topic;
    }

    public void setTopic(String topic) {
        this.topic = topic;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getDistance() {
        return distance;
    }

    public void setDistance(String distance) {
        this.distance = distance;
    }
   public String getExclusive() {
        return exclusive;
    }

    public void setExclusive(String exclusive) {
        this.exclusive = exclusive;
    }



}
