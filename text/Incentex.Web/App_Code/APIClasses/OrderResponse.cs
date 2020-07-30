using System;
using System.Xml.Serialization;

/// <summary>
/// Summary description for OrderResponse
/// </summary>
public class OrderResponse
{
    public OrderResponse()
    {
        //
        // TODO: Add constructor logic here
        //
        this.SAPOrderID = 0;
        this.WorldlinkOrderNumber = null;
        this.CreatedOn = null;        
        this.LogStatus = null;
        this.LogMessage = null;
    }
    
    public Int64 SAPOrderID { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String WorldlinkOrderNumber { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String CreatedOn { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String LogStatus { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String LogMessage { get; set; }
}