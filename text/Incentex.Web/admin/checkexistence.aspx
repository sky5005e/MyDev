<%@ Page Language="C#" Title="Checkexistence.aspx" %>

<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="Incentex.DAL" %>
<%@ Import Namespace="Incentex.DAL.SqlRepository" %>
<%@ Import Namespace="Incentex.DAL.Common" %>


<%
    LookupBE objLookupBe = new LookupBE();
    LookupDA objLookupda = new LookupDA();
    System.Data.DataSet ds = new System.Data.DataSet();
    if (Request.Params["action"] != null)
    {
        if (Request.Params["action"] == "lookupnameexistence")
        {
            if (Request.Params["button"] == "Add")
            {
                objLookupBe.iLookupCode = Session["iLookupCode"].ToString();
                objLookupBe.sLookupName = Request.Params["ctl00$ContentPlaceHolder1$txtPriorityName"];
                objLookupBe.SOperation = "checkexistenceforlookupname";
                ds = objLookupda.LookUp(objLookupBe);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        Response.Write("false");
                    else
                        Response.Write("true");
                }
            }
            else
            {
                Response.Write("true");
            }


        }
        else if (Request.Params["action"] == "emailexistence")
        {
            UserInformation objUserExistence = new UserInformation();
            UserInformationRepository objRe = new UserInformationRepository();
            if (objRe.CheckEmailExistence(Request.Params["ctl00$ContentPlaceHolder1$txtLoginEmail"], 0))
                Response.Write("true");
            else
                Response.Write("false");
        }
    }
%>