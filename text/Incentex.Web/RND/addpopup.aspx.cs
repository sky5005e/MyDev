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

public partial class RND_addpopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
            ((HtmlImage)e.Item.FindControl("imgBtn")).Src= path;
        }
    }
    protected void dtLst_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch(e.CommandName)
        {
            case "deletevalue":
                try
                {
                    PriorityBE objPBe = new PriorityBE();
                    PriorityDA objDa = new PriorityDA();
                    objPBe.iPriorityId = Convert.ToInt64(e.CommandArgument.ToString());
                    objPBe.SOperation = "deletepriority";
                    DataSet ds = objDa.Priority(objPBe);

                    //deleted icon from folder

                    Common objcommon = new Common();
                    objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hf")).Value, IncentexGlobal.dropdownIconPath);
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
        objPriorityBe.SOperation = "addpriority";
        objPriorityBe.sPriorityName = txtPriorityName.Text;
        objPriorityBe.sPriorityIcon = flFile.Value;
        string sFilePath = Server.MapPath("../icons/") + flFile.Value; ;
        Request.Files[0].SaveAs(sFilePath);


        //objPriorityBe.sPriorityIcon;
        DataSet ds = objPriorityDa.Priority(objPriorityBe);

    }
}
