using System;
using System.Xml.Serialization;

/// <summary>
/// Summary description for SAPItems
/// </summary>
public class InsertItemType
{
    public InsertItemType()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [XmlElementAttribute(IsNullable = false)]
    public String ItemCode { get; set; }

    [XmlElementAttribute(IsNullable = false)]
    public String ItemDescription { get; set; }

    [XmlElementAttribute(IsNullable = false)]
    public String Status { get; set; }
}

/// <summary>
/// Summary description for SAPItems
/// </summary>
public class UpdateItemType
{
    public UpdateItemType()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [XmlElementAttribute(IsNullable = false)]
    public String PreviousItemCode { get; set; }

    [XmlElementAttribute(IsNullable = false)]
    public String CurrentItemCode { get; set; }

    [XmlElementAttribute(IsNullable = false)]
    public String ItemDescription { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String WorldLinkItemID { get; set; }
    
}

/// <summary>
/// Response Class Definition for the Insert Item Response to SAP
/// </summary>
public class InsertItemResponse
{
    public InsertItemResponse()
    {
        //
        // TODO: Add constructor logic here
        //
        this.WorldLinkItemID = "0";
        this.CreatedOn = null;
        this.LogStatus = null;
        this.LogMessage = null;
    }

    public String WorldLinkItemID { get; set; }

   
    [XmlElementAttribute(IsNullable = true)]
    public String CreatedOn { get; set; }


    [XmlElementAttribute(IsNullable = true)]
    public String LogStatus { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String LogMessage { get; set; }
}

/// <summary>
/// Response Class Definition for the Update Item Response to SAP
/// </summary>
public class UpdateItemResponse
{
    public UpdateItemResponse()
    {
        //
        // TODO: Add constructor logic here
        //
        this.WorldLinkItemID = "0";
        this.CreatedOn = null;
        this.LogStatus = null;
        this.LogMessage = null;
    }

    public String WorldLinkItemID { get; set; }


    [XmlElementAttribute(IsNullable = true)]
    public String CreatedOn { get; set; }


    [XmlElementAttribute(IsNullable = true)]
    public String LogStatus { get; set; }

    [XmlElementAttribute(IsNullable = true)]
    public String LogMessage { get; set; }
}
