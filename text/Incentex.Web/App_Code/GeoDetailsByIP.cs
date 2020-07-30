using System;

/// <summary>
/// Summary description for GeoDetailsByIP
/// </summary>
public class GeoDetailsByIP
{
    public GeoDetailsByIP()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public String statusCode { get; set; }
    public String statusMessage { get; set; }
    public String ipAddress { get; set; }
    public String countryCode { get; set; }
    public String countryName { get; set; }
    public String regionName { get; set; }
    public String cityName { get; set; }
    public String zipCode { get; set; }
    public Double latitude { get; set; }
    public Double longitude { get; set; }
    public String timeZone { get; set; }
}