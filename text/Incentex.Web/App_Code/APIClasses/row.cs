using System;

namespace DocumentsRow
{
    /// <summary>
    /// Summary description for documents row
    /// </summary>
    [System.Xml.Serialization.XmlRoot(Namespace = "DocumentsRow")]
    public class row
    {
        public row()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public String DocStatus { get; set; }
        public String DocDate { get; set; }
        public Decimal DiscSum { get; set; }
        public Decimal DocTotal { get; set; }
        public String CntctCode { get; set; }
        public String UpdateDate { get; set; }
        public String CreateDate { get; set; }
        public String U_AnnivCred { get; set; }
        public String U_WL_OrderNo { get; set; }
        public String U_IssuancePackage { get; set; }
        public String U_BillToContact { get; set; }
    }
}

namespace Document_LineRow
{
    /// <summary>
    /// Summary description for Document_Line row
    /// </summary>
    [System.Xml.Serialization.XmlRoot(Namespace = "Document_LineRow")]
    public class row
    {
        public row()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public String ItemCode { get; set; }
        public String Dscription { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 OpenQty { get; set; }
        public Decimal Price { get; set; }
        public Int32 DelivrdQty { get; set; }
    }
}

namespace AddressExtensionRow
{
    /// <summary>
    /// Summary description for AddressExtension row
    /// </summary>
    [System.Xml.Serialization.XmlRoot(Namespace = "AddressExtensionRow")]
    public class row
    {
        public row()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public String Address { get; set; }
        public String Address2 { get; set; }
        public String CityS { get; set; }
        public String ZipCodeS { get; set; }
        public String CountyS { get; set; }
        public String StateS { get; set; }
        public String CountryS { get; set; }
        public String U_BillToCode { get; set; }
    }
}