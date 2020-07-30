using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;


public partial class MyAccount_MyIssuancePolicy : PageBase
{

    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    ChangeStatusRepository objChangeStatusRepo = new ChangeStatusRepository();
    string strExpiredDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "My Company Uniform Issuance";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

            DisplayData();
        }
    }


    #region Functions

    void DisplayData()
    {
        Rep.DataBind();
    }

    #endregion

    #region Events

    DateTime? HiredDate
    {
        get { return Convert.ToDateTime(ViewState["HiredDate"]); }
        set { ViewState["HiredDate"] = value; }
    }

    protected void Rep_DataBinding(object sender, EventArgs e)
    {
        INC_Lookup obklookup = new INC_Lookup();
        LookupRepository objLookRepos = new LookupRepository();

        List<AdditionalWorkgroup> objAddWorkgroupList = new List<AdditionalWorkgroup>();
        AdditionalWorkgroupRepository objAddWorkgroupRepos = new AdditionalWorkgroupRepository();

        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        HiredDate = objCmpnyInfo.HirerdDate;
        //Check Here Status of Issuance Polciy in Special Program Active or Inactive
        int ActiveInactive = Convert.ToInt32(objCmpnyInfo.EmpIssuancePolicyStatus);
        obklookup = objLookRepos.GetById(ActiveInactive);
        if (obklookup != null)
        {
            if (obklookup.sLookupName == "Active")
            {
                obklookup = objLookRepos.GetById(objCmpnyInfo.GenderID);
                long strgendertype = obklookup.iLookupID;
                if (strgendertype != 0)
                {
                    List<UniformIssuancePolicy> objList = objUniformIssuancePolicyRepository.GetByCompanyIdandWorkgroupId((Int64)IncentexGlobal.CurrentMember.CompanyId, objCmpnyInfo.WorkgroupID, strgendertype, objCmpnyInfo.EmployeeTypeID);
                    List<UniformIssuancePolicy> objAddTotalList = new List<UniformIssuancePolicy>();
                    int addWorkgroupID = 0;
                    objAddWorkgroupList = objAddWorkgroupRepos.GetWorkgroupName((int)IncentexGlobal.CurrentMember.UserInfoID, (int)IncentexGlobal.CurrentMember.CompanyId);

                    if (objAddWorkgroupList.Count > 0)
                    {
                        foreach (var p in objAddWorkgroupList)
                        {
                            addWorkgroupID = Convert.ToInt32(p.WorkgroupID);
                            List<UniformIssuancePolicy> obj1 = objUniformIssuancePolicyRepository.GetByCompanyIdandWorkgroupId((Int64)IncentexGlobal.CurrentMember.CompanyId, addWorkgroupID, strgendertype, objCmpnyInfo.EmployeeTypeID);
                            if (obj1.Count > 0)
                            {
                                objAddTotalList.AddRange(obj1);
                            }
                        }
                    }

                    objList.AddRange(objAddTotalList);
                    //Rep.DataSource = objList;
                    Rep.DataSource = GetPublishIssuancePolicy(objList);


                }
            }
        }

        //End Update Nagmani 22-Nov-2011
    }
    private List<UniformIssuancePolicy> GetPublishIssuancePolicy(List<UniformIssuancePolicy> objList)
    {
        var listPubPlcy = objChangeStatusRepo.GetUserPublishIssuancePolicies(IncentexGlobal.CurrentMember.UserInfoID).ToList();
        if (listPubPlcy.Count > 0)
        {
            List<UniformIssuancePolicy> ResultList = (from p in listPubPlcy
                                                      join up in objList on p.UniformIssuancePolicyID equals up.UniformIssuancePolicyID
                                                      select up).ToList();
            return ResultList;
        }
        else
            return objList;
    }
    protected void Rep_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            UniformIssuancePolicy obj = e.Item.DataItem as UniformIssuancePolicy;

            LinkButton lnkPackage = e.Item.FindControl("lnkPackage") as LinkButton;
            LinkButton lnkPackage2 = e.Item.FindControl("lnkPackage2") as LinkButton;
            HiddenField hdnPlicydate = (HiddenField)e.Item.FindControl("hdnPolicydate");
            HiddenField hdnBeforeAfter = (HiddenField)e.Item.FindControl("hdnBeforeAfter");
            HtmlControl imgColor = (HtmlControl)e.Item.FindControl("imgColor");
            INC_Lookup obklookup = new INC_Lookup();
            LookupRepository objLookRepos = new LookupRepository();
            obklookup = objLookRepos.GetById(Convert.ToInt32(hdnBeforeAfter.Value));
            string ShowBeforeAfterPolicy = obklookup.sLookupName;
            lnkPackage2.Text = obj.IssuanceProgramName;
            DateTime HireDate = Convert.ToDateTime(HiredDate);
            DateTime PolicyDate = Convert.ToDateTime(obj.PolicyDate);
            if (ShowBeforeAfterPolicy == "Before")
            {
                // if (HiredDate < Convert.ToDateTime(obj.PolicyDate))
                if (HireDate < PolicyDate)
                {
                    lnkPackage2.Visible = true;
                    imgColor.Visible = true;
                }
                else
                {
                    lnkPackage2.Visible = false;
                    imgColor.Visible = false;
                }
            }
            else if (ShowBeforeAfterPolicy == "After")
            {
                //if (HiredDate > Convert.ToDateTime(obj.PolicyDate))
                if (HireDate > PolicyDate)
                {
                    lnkPackage2.Visible = true;
                    imgColor.Visible = true;
                }
                else
                {
                    lnkPackage2.Visible = false;
                    imgColor.Visible = false;
                }
            }
           
           
        }


    }

    protected void Rep_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        HiddenField hdnWorkgroupID = (HiddenField)e.Item.FindControl("hdnWorkgroupID");
        ViewState["PlicyWorkgroup"] = hdnWorkgroupID.Value;
        switch (e.CommandName)
        {
            case "Detail":
                Session["PID"] = e.CommandArgument;
                UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
                UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(e.CommandArgument));
                if(objPolicy!=null)
                {
                    if (Convert.ToBoolean(objPolicy.IsDateOfHiredTicked) && objPolicy.NumberOfMonths > 0)
                    {
                        DateTime _NewDate = Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.NumberOfMonths + objPolicy.CreditExpireNumberOfMonths));
                        strExpiredDate = Convert.ToDateTime(_NewDate).ToShortDateString();
                    }
                    else
                    {
                        strExpiredDate = Convert.ToDateTime(objPolicy.CreditExpireDate).ToShortDateString();
                    }
                }
                if (Convert.ToDateTime(strExpiredDate) < DateTime.Now)
                {
                    string myStringVariable = string.Empty;
                    myStringVariable = "Your Policy Has Expired.";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }
                else
                { 
                    if (Session["PolicyItemID"] != null)
                    {
                        Session["PolicyItemID"] = null;
                    }
                    
                    if (Session["GroupBudgetTotal"] != null)
                    {
                        Session["GroupBudgetTotal"] = null;
                    }
                    if (Session["GroupBudgetCartIDs"] != null)
                    {
                        Session["GroupBudgetCartIDs"] = null;
                    }
                    if (Session["TotalAmount"] != null)
                    {
                        Session["TotalAmount"] = null;
                    }
                    if (Session["GroupTotal"] != null)
                    {
                        Session["GroupTotal"] = null;
                    }
                    if (Session["GroupCartIDs"] != null)
                    {
                        Session["GroupCartIDs"] = null;
                    }
                    if (Session["SingleTotal"] != null)
                    {
                        Session["SingleTotal"] = null;
                    }
                    if (Session["SingleCartIDs"] != null)
                    {
                        Session["SingleCartIDs"] = null;
                    }
                    if (Session["TotalAmount"] != null)
                        Session["TotalAmount"] = null;
                    if (Session["MyIssuanceCartIDs"] != null)
                        Session["MyIssuanceCartIDs"] = null;

                    //Response.Redirect("IssuancePackagePreorder.aspx?PID=" + e.CommandArgument + "&PolicyWorkgroupID=" + ViewState["PlicyWorkgroup"], false);
                    Response.Redirect("IssuancePackagePreOrderDetails.aspx?PID=" + e.CommandArgument + "&PolicyWorkgroupID=" + ViewState["PlicyWorkgroup"], false);
                    
                }
                break;
        }
    }
    
    #endregion

}
