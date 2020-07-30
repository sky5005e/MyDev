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
using System.IO;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;


public partial class MyAccount_IssuancePackage : System.Web.UI.Page
{

    #region Property

    Int64 UniformIssuancePolicyID
    {
        get
        {
            if (ViewState["UniformIssuancePolicyID"] == null)
            {
                ViewState["UniformIssuancePolicyID"] = 0;
            }
            return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]);
        }
        set
        {
            ViewState["UniformIssuancePolicyID"] = value;
        }
    }

    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    UniformIssuancePolicyItemRepository objUniformIssuancePolicyItemRepository = new UniformIssuancePolicyItemRepository();

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("UniformIssuancePolicyID"));

            UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);

            if (objUniformIssuancePolicy != null)
            {
                ((Label)Master.FindControl("lblPageHeading")).Text =  objUniformIssuancePolicy.IssuanceProgramName + " - Issuance Package " ;
            }

            DisplayData();
        }
    }


    #region Methods

    void DisplayData()
    {
        rep1.DataBind();
    }

    #endregion

    #region events


    protected void lnkPreOrder_Click(object sender, EventArgs e)
    {
        Response.Redirect("IssuancePackagePreorder.aspx",false);
        
    }

    protected void rep1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem
            )
        {

            UniformIssuancePolicyItem objUniformIssuancePolicyItem = e.Item.DataItem as UniformIssuancePolicyItem;

            StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();

            List<StoreProductImage> objList = objStoreProductImageRepository.GetStoreProductImageByCompany( Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId) , Convert.ToInt64(objUniformIssuancePolicyItem.MasterItemId));


            //imgPhoto.ImageUrl = "";
            Image imgPhoto = e.Item.FindControl("imgPhoto") as Image;
            imgPhoto.ImageUrl = "";

            imgPhoto.Visible = false;
            if (objList.Count > 0)
            {

                StoreProductImage obj = (from i in objList
                                         where i.ProductImageActive == 1
                                         select i).FirstOrDefault();
                if (obj != null)
                {

                    string FilePath = "~/UploadedImages/ProductImages/" + obj.ProductImage;

                    if (File.Exists(Server.MapPath(FilePath)))
                    {
                        //imgPhoto.ImageUrl = FilePath;
                        imgPhoto.ImageUrl = FilePath;
                        imgPhoto.Visible = true;
                        //lnkItemImg.HRef = FilePath;
                        //lnkItemImg.HRef  = "../../../UploadedImages/ProductImages/" + objList[0].ProductImage;
                    }
                }
            }
        }
   
    }

    protected void rep1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        
    }

    protected void rep1_DataBinding(object sender, EventArgs e)
    {
        List<UniformIssuancePolicyItem> objList = objUniformIssuancePolicyItemRepository.GetByUniformIssuancePolicyID(this.UniformIssuancePolicyID);

        rep1.DataSource = objList;
    }


    #endregion

}
