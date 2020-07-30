using System;
using System.Data;
using System.IO;
using System.Xml;

/// <summary>
/// Summary description for XMLHelper
/// </summary>
public class XmlHelper
{
    public XmlHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void WriteSalesOrderXml(String OrderNumber)
    {
        String StrRootPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["XMLFilePath"]);
        String XmlPath = System.Web.HttpContext.Current.Server.MapPath(StrRootPath + "/" + Convert.ToString(OrderNumber) + ".xml");

        XmlTextWriter objXmlTextWriter = new XmlTextWriter(XmlPath, null);
        objXmlTextWriter.Formatting = Formatting.Indented;

        //Xml Document starts here...
        objXmlTextWriter.WriteStartDocument();

        //BO is the first tag of the Xml document...
        objXmlTextWriter.WriteStartElement("BO");
        
        #region Documents Tag

        objXmlTextWriter.WriteStartElement("Documents");//Documents
        objXmlTextWriter.WriteStartElement("row");//Documents row

        #region Documents Tag Properties

        objXmlTextWriter.WriteStartElement("DocStatus");
        objXmlTextWriter.WriteString("1");
        objXmlTextWriter.WriteEndElement();//end DocStatus

        objXmlTextWriter.WriteStartElement("DocDate");
        objXmlTextWriter.WriteString("2");
        objXmlTextWriter.WriteEndElement();//end DocDate

        objXmlTextWriter.WriteStartElement("DiscSum");
        objXmlTextWriter.WriteString("3");
        objXmlTextWriter.WriteEndElement();//end DiscSum

        objXmlTextWriter.WriteStartElement("DocTotal");
        objXmlTextWriter.WriteString("4");
        objXmlTextWriter.WriteEndElement();//end DocTotal

        objXmlTextWriter.WriteStartElement("CntctCode");
        objXmlTextWriter.WriteString("5");
        objXmlTextWriter.WriteEndElement();//end CntctCode

        objXmlTextWriter.WriteStartElement("UpdateDate");
        objXmlTextWriter.WriteString("6");
        objXmlTextWriter.WriteEndElement();//end UpdateDate

        objXmlTextWriter.WriteStartElement("CreateDate");
        objXmlTextWriter.WriteString("7");
        objXmlTextWriter.WriteEndElement();//end CreateDate

        objXmlTextWriter.WriteStartElement("U_AnnivCred");
        objXmlTextWriter.WriteString("8");
        objXmlTextWriter.WriteEndElement();//end U_AnnivCred

        objXmlTextWriter.WriteStartElement("U_WL_OrderNo");
        objXmlTextWriter.WriteString("9");
        objXmlTextWriter.WriteEndElement();//end U_WL_OrderNo

        objXmlTextWriter.WriteStartElement("U_IssuancePackage");
        objXmlTextWriter.WriteString("10");
        objXmlTextWriter.WriteEndElement();//end U_IssuancePackage

        objXmlTextWriter.WriteStartElement("U_BillToContact");
        objXmlTextWriter.WriteString("11");
        objXmlTextWriter.WriteEndElement();//end U_BillToContact

        #endregion

        objXmlTextWriter.WriteEndElement();//end Documents row
        objXmlTextWriter.WriteEndElement();//end Documents

        #endregion

        #region Document_Lines

        objXmlTextWriter.WriteStartElement("Documents_Line");

        //foreach (OrderLine objLine in lstOrderLine)
        //{
        objXmlTextWriter.WriteStartElement("row");//Documents_Line row

        objXmlTextWriter.WriteStartElement("ItemCode");
        objXmlTextWriter.WriteString("12");
        objXmlTextWriter.WriteEndElement();//end ItemCode

        objXmlTextWriter.WriteStartElement("Dscription");
        objXmlTextWriter.WriteString("13");
        objXmlTextWriter.WriteEndElement();//end Dscription

        objXmlTextWriter.WriteStartElement("Quantity");
        objXmlTextWriter.WriteString("14");
        objXmlTextWriter.WriteEndElement();//end Quantity

        objXmlTextWriter.WriteStartElement("OpenQty");
        objXmlTextWriter.WriteString("15");
        objXmlTextWriter.WriteEndElement();//end OpenQty

        objXmlTextWriter.WriteStartElement("Price");
        objXmlTextWriter.WriteString("16");
        objXmlTextWriter.WriteEndElement();//end Price

        objXmlTextWriter.WriteStartElement("DelivrdQty");
        objXmlTextWriter.WriteString("17");
        objXmlTextWriter.WriteEndElement();//end DelivrdQty

        objXmlTextWriter.WriteEndElement();//end Documents_Line row            
        //}

        objXmlTextWriter.WriteEndElement();//end Documents_Line

        #endregion        

        #region AddressExtension Tag

        objXmlTextWriter.WriteStartElement("AddressExtension");
        objXmlTextWriter.WriteStartElement("row");//AddressExtension row

        objXmlTextWriter.WriteStartElement("Address");
        objXmlTextWriter.WriteString("18");
        objXmlTextWriter.WriteEndElement();//end Address

        objXmlTextWriter.WriteStartElement("Address2");
        objXmlTextWriter.WriteString("19");
        objXmlTextWriter.WriteEndElement();//end Address2

        objXmlTextWriter.WriteStartElement("CityS");
        objXmlTextWriter.WriteString("20");
        objXmlTextWriter.WriteEndElement();//end CityS

        objXmlTextWriter.WriteStartElement("ZipCodeS");
        objXmlTextWriter.WriteString("21");
        objXmlTextWriter.WriteEndElement();//end ZipCodeS

        objXmlTextWriter.WriteStartElement("StateS");
        objXmlTextWriter.WriteString("22");
        objXmlTextWriter.WriteEndElement();//end StateS

        objXmlTextWriter.WriteStartElement("CountryS");
        objXmlTextWriter.WriteString("23");
        objXmlTextWriter.WriteEndElement();//end CountryS

        objXmlTextWriter.WriteStartElement("U_BillToCode");
        objXmlTextWriter.WriteString("24");
        objXmlTextWriter.WriteEndElement();//end U_BillToCode

        objXmlTextWriter.WriteEndElement();//end AddressExtension row
        objXmlTextWriter.WriteEndElement();//end AddressExtension

        #endregion

        //Closing the BO (first) tag...
        objXmlTextWriter.WriteEndElement();

        //Closing the Document...
        objXmlTextWriter.WriteEndDocument();

        //Flushing the Xml structure into the file...
        objXmlTextWriter.Flush();

        //Closing the XmlTextWriter object after the Xml is written to the file...
        objXmlTextWriter.Close();
    }

    public void DeleteSalesOrderXML(String OrderNumber)
    {
        try
        {
            String strRootPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["XMLFilePath"]);
            String xmlPath = System.Web.HttpContext.Current.Server.MapPath(strRootPath + "/" + Convert.ToString(OrderNumber) + ".xml");

            if (File.Exists(xmlPath))
            {
                File.Delete(xmlPath);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public XmlElement ReadSalesOrderXml(String OrderNumber)
    {
        XmlElement objXmlElement = null;
        try
        {
            String strRootPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["XMLFilePath"]);
            String xmlPath = System.Web.HttpContext.Current.Server.MapPath(strRootPath + "/" + Convert.ToString(OrderNumber) + ".xml");

            if (File.Exists(xmlPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@xmlPath);
                objXmlElement = doc.DocumentElement;
                doc.RemoveAll();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return objXmlElement;
    }
}