using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SAP_API
{
    [Serializable]
    [GeneratedCode("System.Xml", "2.0.50727.4927")]
    [DesignerCategory("code")]
    [DebuggerStepThrough]
    public class OrderShipmentType
    {
        public OrderShipmentType()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public OrderShipmentTypeBO BO { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [GeneratedCode("System.Xml", "2.0.50727.4927")]
    [DebuggerStepThrough]
    [XmlType(AnonymousType = true)]
    public class OrderShipmentTypeBO
    {
        public OrderShipmentTypeBO()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public OrderShipmentTypeBODocuments Documents { get; set; }

        [XmlArrayItem("row", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public OrderShipmentTypeBORow[] Document_Lines { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [GeneratedCode("System.Xml", "2.0.50727.4927")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OrderShipmentTypeBORow
    {
        public OrderShipmentTypeBORow()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String ItemCode { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String ShippedQuantity { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String TrackingNumber { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String NoOfBoxes { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String ShippedDateTime { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String CarrierName { get; set; }
    }

    [Serializable]
    [GeneratedCode("System.Xml", "2.0.50727.4927")]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [DebuggerStepThrough]
    public class OrderShipmentTypeBODocuments
    {
        public OrderShipmentTypeBODocuments()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public OrderShipmentTypeBODocumentsRow row { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [GeneratedCode("System.Xml", "2.0.50727.4927")]
    public class OrderShipmentTypeBODocumentsRow
    {
        public OrderShipmentTypeBODocumentsRow()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public String U_WL_OrderNo { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("System.Xml", "2.0.50727.4927")]
    public class OrderShipmentResponse
    {
        public OrderShipmentResponse()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CreatedOn { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string LogMessage { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string LogStatus { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SAPOrderID { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public object WorldLinkOrderNumber { get; set; }
    }
}