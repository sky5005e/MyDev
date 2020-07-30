<%@ Page Language="C#" %>
<%@ Import Namespace="commonlib.Common" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Incentex.DAL" %>
<%@ Import Namespace="Incentex.DAL.SqlRepository" %>
<%@ Import Namespace="Incentex.DAL.Common" %>


<%
    if (Request["ctl00$ContentPlaceHolder1$txtCompanyCode"] != null)
    {
        try
        {
            string strCompanyCode = Request["ctl00$ContentPlaceHolder1$txtCompanyCode"];
            Int64 intCompanyID = Convert.ToInt64(strCompanyCode.Substring(0, strCompanyCode.IndexOf('-')));
            Int64 intDepartmentID = Convert.ToInt64(strCompanyCode.Substring((strCompanyCode.IndexOf('-') + 1), strCompanyCode.IndexOf('-', (strCompanyCode.IndexOf('-') + 1)) - (strCompanyCode.IndexOf('-') + 1)));
            Int64 intWorkgroupID = Convert.ToInt64(strCompanyCode.Substring((strCompanyCode.IndexOf('-', (strCompanyCode.IndexOf('-') + 1)) + 1), (strCompanyCode.Split('-').Length - 1) > 2 ? (strCompanyCode.LastIndexOf('-') - 1) - strCompanyCode.IndexOf('-', (strCompanyCode.IndexOf('-') + 1)) : strCompanyCode.Length - (strCompanyCode.LastIndexOf('-') + 1)));

            if (new CompanyRepository().GetById(intCompanyID) != null && new LookupRepository().GetById(intDepartmentID) != null && new LookupRepository().GetById(intDepartmentID).iLookupCode == "Department " && new LookupRepository().GetById(intWorkgroupID) != null && new LookupRepository().GetById(intWorkgroupID).iLookupCode == "Workgroup ")
                Response.Write("true");
            else
                Response.Write("false");
        }
        catch { Response.Write("false"); }
    }
%>
