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
using Incentex.DA;
using Incentex.BE;

public partial class admin_proofstatus : PageBase
{
    LookupDA obj = new LookupDA();
    DataSet dsDuplicate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Proof Status";
            BindProof();
        }

    }
    private void BindProof()
    {

        PriorityBE objPriorityBe = new PriorityBE();
        PriorityDA objPriorityDa = new PriorityDA();
        objPriorityBe.SOperation = "selectall";
        DataSet ds = objPriorityDa.Priority(objPriorityBe);
        dtLst.DataSource = ds;
        dtLst.DataBind();
    }
    protected void dtLst_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string path = "../icons/" + ((HiddenField)e.Item.FindControl("hf")).Value;
            ((HtmlImage)e.Item.FindControl("imgBtn")).Src = path;
        }
    }
    protected void dtLst_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Common objcommon = new Common();
        PriorityBE objPBe = new PriorityBE();
        PriorityDA objDa = new PriorityDA();
        switch (e.CommandName)
        {
            case "deletevalue":
                try
                {

                    objPBe.iPriorityId = Convert.ToInt64(e.CommandArgument.ToString());
                    objPBe.SOperation = "deletepriority";
                    DataSet ds = objDa.Priority(objPBe);

                    //deleted icon from folder


                    objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hf")).Value, IncentexGlobal.dropdownIconPath);
                }
                catch (Exception ex)
                {
                }
                break;

            case "editvalue":
                try
                {

                    objPBe.iPriorityId = Convert.ToInt64(e.CommandArgument.ToString());
                    objPBe.SOperation = "selectbyid";
                    DataSet ds = objDa.Priority(objPBe);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            icondiv.Visible = true;
                            btnSubmit.Text = "Edit";
                            Session["id"] = e.CommandArgument.ToString();
                            Session["iconvalue"] = ds.Tables[0].Rows[0]["sPriorityIcon"].ToString();
                            txtPriorityName.Text = ds.Tables[0].Rows[0]["sPriorityName"].ToString();
                            imgEdit.Src = "../icons/" + Session["iconvalue"];
                            // flFile.Value = ds.Tables[0].Rows[0]["sPriorityIcon"].ToString();
                        }
                    }
                    modal.Show();

                }
                catch (Exception ex)
                {
                }
                break;
            default:
                break;

        }
        BindProof();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PriorityBE objPriorityBe = new PriorityBE();
        PriorityDA objPriorityDa = new PriorityDA();
        Common objcommon = new Common();
        if (btnSubmit.Text == "Add")
        {
            objPriorityBe.SOperation = "addpriority";
            objPriorityBe.sPriorityName = txtPriorityName.Text;
            objPriorityBe.sPriorityIcon = flFile.Value;
            string sFilePath = Server.MapPath("../icons/") + flFile.Value;

            dsDuplicate = obj.CheckDuplication(0, txtPriorityName.Text, btnSubmit.Text,null);
            if (dsDuplicate.Tables[0].Rows.Count > 0)
            {
                if (dsDuplicate.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                   string me= "Not add";
                }
                else
                {
                    Request.Files[0].SaveAs(sFilePath);
                    objPriorityDa.Priority(objPriorityBe);
                    modal.Hide();
                    string me = " add";
                }
            }
            
            
        }
        else
        {
            objPriorityBe.SOperation = "editpriority";
            objPriorityBe.iPriorityId = Convert.ToInt64(Session["id"].ToString());
            objPriorityBe.sPriorityName = txtPriorityName.Text;
            if (flFile.Value != "")
            {
                objPriorityBe.sPriorityIcon = flFile.Value;
            }
            else
            {
                objPriorityBe.sPriorityIcon = Session["iconvalue"].ToString();

            }

            objPriorityDa.Priority(objPriorityBe);

            //save new icon
            string sFilePath = Server.MapPath("../icons/") + objPriorityBe.sPriorityIcon;

            dsDuplicate = obj.CheckDuplication(Convert.ToInt32(Session["id"].ToString()), txtPriorityName.Text, btnSubmit.Text,null);
            if (dsDuplicate.Tables[0].Rows.Count > 0)
            {
                if (dsDuplicate.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    string me = "Not updated";
                }
                else
                {
                    Request.Files[0].SaveAs(sFilePath);

                    //delete old icon
                    objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);

                    string me = " updated";
                }
            }


           
            //



        }
        clearpopup();
        BindProof();
    }

    private void clearpopup()
    {
        txtPriorityName.Text = string.Empty;
        
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        modal.X = 350;
        modal.Y = 150;
        btnSubmit.Text = "Add";
        txtPriorityName.Text = string.Empty;
        icondiv.Visible = false;
        modal.Controls.Clear();
        modal.Show();
    }
}
