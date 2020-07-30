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
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
public partial class MyAccount_CompanyProgram_MyAnniversaryCredits : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            GetCompanyEmployeeAnniversaryCreditDetails();
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }
            ((Label)Master.FindControl("lblPageHeading")).Text = "My Store Credits"; 
        }
    }

    public void GetCompanyEmployeeAnniversaryCreditDetails()
    {
        AnniversaryProgramRepository objCEProgram = new AnniversaryProgramRepository();
        SelectAnniversaryCreditProgramPerEmployeeResult objCE = objCEProgram.GetCompanyEmployeeAnniversaryCreditDetails(IncentexGlobal.CurrentMember.UserInfoID);
        if (objCE != null)
        {
            trLastAnnCreditAmount.Visible = false;
            //Strating Credit Table//
            if (objCE.StratingCreditAmount.ToString() != "")
            {
                startingcredittable.Visible = true;
                hfStartingCreditCurrentBalance.Value = objCE.StartingCreditAmountMain.ToString();
                hfStartingCreditUpdatedDate.Value = Convert.ToDateTime(objCE.StratingCreditUpdatedDate).ToShortDateString();
                hfStartingCreditAmount.Value = objCE.StratingCreditAmount.ToString();

                if (objCE.StartingCreditExpireDate != null)
                {
                    if (((DateTime)objCE.StartingCreditExpireDate).Year.ToString() != "0001")
                    {
                        hfStatrtingCreditExpirationDate.Value = Convert.ToDateTime(objCE.StartingCreditExpireDate).ToShortDateString();
                    }
                    else
                    {
                        hfStatrtingCreditExpirationDate.Value = "---";
                    }
                }
            }
            else
            {
                startingcredittable.Visible = false;
            }
            //End//

            //Anniversary Credits
            hfAnniversaryStartingBalance.Value = objCE.AnniversaryCreditBalance.ToString();
            hfDollarCreditToApply.Value = objCE.AmountFromProgram.ToString();
            hfNewHiredDate.Value = Convert.ToDateTime(objCE.NewHiredDate).ToShortDateString();
            //hfNextCreditToBeApplyDate.Value = Convert.ToDateTime(objCE.IssueCreditOn).ToShortDateString();
            if (objCE.NextCreditAmountToApplyDate != null)
            {
                hfNextCreditToBeApplyDate.Value = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).ToShortDateString();
            }
            else
            {
                hfNextCreditToBeApplyDate.Value = null;
            }
            
            if (objCE.CreditExpireAfter != null)
            {
                if (((DateTime)objCE.CreditExpireAfter).Year.ToString() != "9999")
                {
                    //hfeNextCreditExpirationDate.Value = Convert.ToDateTime(objCE.CreditExpireAfter).ToShortDateString();
                    if (objCE.NextCreditAmountToExpireDate != null)
                    {
                        hfeNextCreditExpirationDate.Value = Convert.ToDateTime(objCE.NextCreditAmountToExpireDate).ToShortDateString();
                    }
                    else
                    {
                        hfeNextCreditExpirationDate.Value = "---";
                    }
                }
                else
                {
                    hfeNextCreditExpirationDate.Value = "---";
                }
            }
            
            //End Anniversary Credit

            //Extra Div//
            hfCreditBalance.Value = objCE.CurrentBalance.ToString();
            hfDateHiredDate.Value = Convert.ToDateTime(objCE.HirerdDate).ToShortDateString();
            hfCreditAfterExpiry.Value = (objCE.CurrentBalance - objCE.AmountFromProgram).ToString();
            //End//


        }

        //Bind Anniversary Order Hisotry
       AnniversaryProgramRepository o = new AnniversaryProgramRepository();
       List<SelectAnniversaryOrderPerEmployeeResult> objList = new List<SelectAnniversaryOrderPerEmployeeResult>();
       objList = o.GetAnniversaryOrderHistoryByEmployee(IncentexGlobal.CurrentMember.UserInfoID);
       if (objList.Count > 0)
       {
           gvEmployeeOrderAnniversary.DataSource = objList;
           gvEmployeeOrderAnniversary.DataBind();
           lblmsg.Visible = false;
           lblmsg.Text = string.Empty;
       }
       else
       {
           lblmsg.Visible = true;
           lblmsg.Text = "No orders with anniversary credits used!!";
       }
           
    }
    protected void gvEmployeeOrderAnniversary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("OrderDetail"))
        {
            Response.Redirect("OrderDetail.aspx?id="+e.CommandArgument.ToString());
            
        }
    }
    protected void gvEmployeeOrderAnniversary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((Label)e.Row.FindControl("lblCreditUsed")).Text != null || ((Label)e.Row.FindControl("lblCreditUsed")).Text != "")
            {
                if (((Label)e.Row.FindControl("lblCreditUsed")).Text == "Previous")
                {
                    ((Label)e.Row.FindControl("lblCreditUsed")).Text = "Starting Credits";
                }
                else if (((Label)e.Row.FindControl("lblCreditUsed")).Text == "Anniversary")
                {
                    ((Label)e.Row.FindControl("lblCreditUsed")).Text = "Anniversary Credit";
                }
                else
                {
                    ((Label)e.Row.FindControl("lblCreditUsed")).Text = "---";
                }
            }
            else
            {
                ((Label)e.Row.FindControl("lblCreditUsed")).Text = "---";
            }

        }
    }
    protected void lnkTransactionLog_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewLedgerDetails.aspx");
    }

}
