<%@ Page Language="C#" %>

<%@ Import Namespace="commonlib.Common" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Incentex.DAL" %>
<%@ Import Namespace="Incentex.DAL.SqlRepository" %>
<%@ Import Namespace="Incentex.DAL.Common" %>
<%
    if (Request["ctl00$ContentPlaceHolder1$txtEmployeeId"] != null && Request["companyid"] != null)
    {
        try
        {   
            if (new CompanyEmployeeRepository().CheckEmployeeIDExistence(Request["ctl00$ContentPlaceHolder1$txtEmployeeId"].ToString(), Convert.ToInt64(Request["companyid"]), 0))
                Response.Write("true");
            else
                Response.Write("false");
        }
        catch { }
    }
%>