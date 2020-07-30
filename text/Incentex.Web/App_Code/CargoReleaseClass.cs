using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;

/// <summary>
/// Summary description for CargoReleaseClass
/// </summary>
public class CargoReleaseClass
{

    public CargoReleaseClass(string XML)
	{
		Document = Create(XML);
		if(Document!=null)
			TraverseItemsTree(Document, ref Items, "CargoRelease/Items/Item");
	}
    private const string xmlschema = "xmlns=\"http://www.magaya.com/XMLSchema/V1\"";

    private static XPathNavigator Create(string xml)
    {
        if (String.IsNullOrEmpty(xml))
            return null;

        xml = xml.Replace(xmlschema, "");

        try
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader text = new XmlTextReader(stringReader);
            XPathDocument reader = new XPathDocument(text);
            return reader.CreateNavigator();
        }
        catch
        {
            return null;
        }
    }
    private static void TraverseItemsTree(XPathNavigator root, ref List<Item> ItemsTree, string xpath)
    {
        foreach (XPathNavigator itemNode in root.Select(xpath))
        {
            Item aux = new Item();
            aux.CargoReleaseNumber = itemNode.SelectSingleNode("CargoReleaseNumber") != null ? itemNode.SelectSingleNode("CargoReleaseNumber").Value : null;
            aux.PartNumber = itemNode.SelectSingleNode("PartNumber") != null ? itemNode.SelectSingleNode("PartNumber").Value : null;
            aux.PieceQuantity = itemNode.SelectSingleNode("PieceQuantity") != null ? itemNode.SelectSingleNode("PieceQuantity").Value : null;
            aux.Pieces = itemNode.SelectSingleNode("ItemDefinition/Pieces") != null ? itemNode.SelectSingleNode("ItemDefinition/Pieces").Value : null;
            aux.SalesOrderNumber = itemNode.SelectSingleNode("SalesOrderNumber") != null ? itemNode.SelectSingleNode("SalesOrderNumber").Value : null;
           
            List<Item> internalnodes = new List<Item>();

            if ((itemNode.Select("ContainedItems/Item") != null) && (itemNode.Select("ContainedItems/Item").Count > 0))
            {
                TraverseItemsTree(itemNode, ref internalnodes, "ContainedItems/Item");
                aux.ContainedItems.AddRange(internalnodes);
            }

            ItemsTree.Add(aux);
        }
    }

    private XPathNavigator Document = null;
    public List<Item> Items = new List<Item>();

    public string CreatedOn
    {
        get
        {
            return Document.SelectSingleNode("CargoRelease/CreatedOn") != null ? Document.SelectSingleNode("CargoRelease/CreatedOn").Value : null;
        }
    }

    public string Number
    {
        get
        {
            return Document.SelectSingleNode("CargoRelease/Number") != null ? Document.SelectSingleNode("CargoRelease/Number").Value : null;
        }
    }

    public string ReleasedToType
    {
        get
        {
            return Document.SelectSingleNode("CargoRelease/ReleasedTo/Type") != null ? Document.SelectSingleNode("CargoRelease/ReleasedTo/Type").Value : null;
        }
    }

    public string ReleasedToName
    {
        get
        {
            return Document.SelectSingleNode("CargoRelease/ReleasedTo/Name") != null ? Document.SelectSingleNode("CargoRelease/ReleasedTo/Name").Value : null;
        }
    }

    public string CarrierName
    {
        get
        {
            return Document.SelectSingleNode("CargoRelease/CarrierName") != null ? Document.SelectSingleNode("CargoRelease/CarrierName").Value : null;
        }
    }

    public string CarrierTrackingNumber
    {
        get
        {
            return Document.SelectSingleNode("CargoRelease/CarrierTrackingNumber") != null ? Document.SelectSingleNode("CargoRelease/CarrierTrackingNumber").Value : null;
        }
    }
}
