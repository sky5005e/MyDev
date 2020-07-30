//using System;
//using System.CodeDom.Compiler;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Xml.Schema;
//using System.Xml.Serialization;

//namespace Incentex_API
//{
//    [Serializable]
//    [DesignerCategory("code")]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DebuggerStepThrough]
//    public class UpdateSalesOrder
//    {
//        public UpdateSalesOrder()
//        {

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderType SalesOrder { get; set; }
//    }

//    [Serializable]
//    [DesignerCategory("code")]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DebuggerStepThrough]
//    public class SalesOrderType
//    {
//        public SalesOrderType()
//        { 

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBO BO { get; set; }
//    }

//    [Serializable]
//    [DebuggerStepThrough]
//    [DesignerCategory("code")]
//    [XmlType(AnonymousType = true)]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    public class SalesOrderTypeBO
//    {
//        public SalesOrderTypeBO()
//        { 

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBOAddressExtension AddressExtension { get; set; }
//        [XmlArray(Form = XmlSchemaForm.Unqualified)]
//        [XmlArrayItem("row", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
//        public SalesOrderTypeBORow[] Document_Lines { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBODocuments Documents { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBODocumentsAdditionalExpenses DocumentsAdditionalExpenses { get; set; }
//    }

//    [Serializable]
//    [DebuggerStepThrough]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [XmlType(AnonymousType = true)]
//    [DesignerCategory("code")]
//    public class SalesOrderTypeBODocuments
//    {
//        public SalesOrderTypeBODocuments()
//        { 

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBODocumentsRow row { get; set; }
//    }

//    [Serializable]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DebuggerStepThrough]
//    [DesignerCategory("code")]
//    [XmlType(AnonymousType = true)]
//    public class SalesOrderTypeBODocumentsRow
//    {
//        public SalesOrderTypeBODocumentsRow()
//        { 

//        }

//        public string CardCode { get; set; }
//        public string CntctCode { get; set; }
//        public string CreateDate { get; set; }
//        public string DiscSum { get; set; }
//        public string DocDate { get; set; }
//        public string DocStatus { get; set; }
//        public string DocTotal { get; set; }
//        public string NumAtCard { get; set; }
//        public string U_AnnivCred { get; set; }
//        public string U_BillToContact { get; set; }
//        public string U_BP_Cost_Ctr { get; set; }
//        public string U_CredCard { get; set; }
//        public string U_EmployeeNo { get; set; }
//        public string U_IssuancePackage { get; set; }
//        public string U_PayrollDeduct { get; set; }
//        public string U_Station { get; set; }
//        public string U_StoreID { get; set; }
//        public string U_U_WL_Tax { get; set; }
//        public string U_WL_OrderNo { get; set; }
//        public string UpdateDate { get; set; }
//    }

//    [Serializable]
//    [DebuggerStepThrough]
//    [DesignerCategory("code")]
//    [XmlType(AnonymousType = true)]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    public class SalesOrderTypeBORow
//    {
//        public SalesOrderTypeBORow()
//        { 

//        }

//        public string DelivrdQty { get; set; }
//        public string Dscription { get; set; }
//        public string ItemCode { get; set; }
//        public string OpenQty { get; set; }
//        public string Price { get; set; }
//        public string Quantity { get; set; }
//        public string U_BP_GL_Code { get; set; }
//        public string U_WorkgroupID { get; set; }
//    }

//    [Serializable]
//    [DebuggerStepThrough]
//    [XmlType(AnonymousType = true)]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DesignerCategory("code")]
//    public class SalesOrderTypeBOAddressExtension
//    {
//        public SalesOrderTypeBOAddressExtension()
//        { 

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBOAddressExtensionRow row { get; set; }
//    }

//    [Serializable]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DesignerCategory("code")]
//    [XmlType(AnonymousType = true)]
//    [DebuggerStepThrough]
//    public class SalesOrderTypeBOAddressExtensionRow
//    {
//        public SalesOrderTypeBOAddressExtensionRow()
//        { 

//        }

//        public string Address { get; set; }
//        public string Address2 { get; set; }
//        public string CityS { get; set; }
//        public string CountryS { get; set; }
//        public string CountyS { get; set; }
//        public string StateS { get; set; }
//        public string U_BillToCode { get; set; }
//        public string ZipCodeS { get; set; }
//    }

//    [Serializable]
//    [DesignerCategory("code")]
//    [DebuggerStepThrough]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [XmlType(AnonymousType = true)]
//    public class SalesOrderTypeBODocumentsAdditionalExpenses
//    {
//        public SalesOrderTypeBODocumentsAdditionalExpenses()
//        { 

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public SalesOrderTypeBODocumentsAdditionalExpensesRow row { get; set; }
//    }

//    [Serializable]
//    [XmlType(AnonymousType = true)]
//    [DesignerCategory("code")]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DebuggerStepThrough]
//    public class SalesOrderTypeBODocumentsAdditionalExpensesRow
//    {
//        public SalesOrderTypeBODocumentsAdditionalExpensesRow()
//        { 

//        }

//        public string ExpenseCode { get; set; }
//        public string LineTotal { get; set; }
//        public string Remarks { get; set; }
//    }

//    [Serializable]
//    [GeneratedCode("System.Xml", "2.0.50727.4927")]
//    [DesignerCategory("code")]
//    [DebuggerStepThrough]
//    public class OrderConfirmType
//    {
//        public OrderConfirmType()
//        {

//        }

//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public string CreatedOn { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public string LogMessage { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public string LogStatus { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public string OrderStatus { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public string SAPOrderID { get; set; }
//        [XmlElement(Form = XmlSchemaForm.Unqualified)]
//        public string WordlinkOrderNumber { get; set; }
//    }
//}