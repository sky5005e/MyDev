/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package com.douglas.common;

/**
 *
 * @author Himanshu
 */


public interface XmlParser {
    Store[] parseStores();
    Event[] parseEvents();
    TopTen[] parseTopTen();
    Angebote[] parseAngebote();
    Neuheiten[] parseNeuheiten();
    LooksTVSpots[] parselookstvspots();
     
}
