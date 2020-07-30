<%@ Page Language="C#" %>
<%@ Import Namespace="Incentex.DAL" %>
<%@ Import Namespace="Incentex.DAL.SqlRepository" %>

<%
    if (Request["ctl00$ContentPlaceHolder1$txtEmail"] != null)
    {   
        UserInformationRepository objUserRepo = new UserInformationRepository();
        if (objUserRepo.CheckEmailExistence(Request["ctl00$ContentPlaceHolder1$txtEmail"].ToString(), 0))
        {
            if (Request["IsFromSignUp"] != null)
            {
                RegistrationRepository objRegiRepo = new RegistrationRepository();
                if (objRegiRepo.CheckEmailExistence(Request["ctl00$ContentPlaceHolder1$txtEmail"].ToString(), 0))
                    Response.Write("true");
                else            
                    Response.Write("1");
            }
        }
        else
        {
            Response.Write("false");
        }
    }    
%>
