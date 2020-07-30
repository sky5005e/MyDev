using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_StoreView : PageBase
{
    #region Data Members
    Int64 StoreID
    {
        get
        {
            if (ViewState["StoreID"] == null)
            {
                ViewState["StoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["StoreID"]);
        }
        set
        {
            ViewState["StoreID"] = value;
        }
    }
    CatogeryRepository objCatogeryRepository = new CatogeryRepository();
    StoreCategoryLocationRepository objStoreCategoryLocationRepository = new StoreCategoryLocationRepository();
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString["Id"]!=null)
            {   
                StoreID = Convert.ToInt64(Request.QueryString["Id"]);

                if (this.StoreID == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.StoreID);
                }

                if (this.StoreID > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                //Assign Page Header and return URL 
                ((Label)Master.FindControl("lblPageHeading")).Text = "Store View Management";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
                Session["ManageID"] = 5;

                menuControl.PopulateMenu(12, 0, StoreID, 0, false);

                lblStore.Text = new CompanyStoreRepository().GetBYStoreId((Int32)StoreID).FirstOrDefault().CompanyName;

                //Bind values function
                BindValues();

                //Function gets called when user comes in edit mode
                DisplayData();
            }
        }
    }
    
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            //Delete previous settings for this store
            List<StoreCategoryLocation> objStoreCategoryLocationList = objStoreCategoryLocationRepository.GetCategoryLocationByStoreID(StoreID);

            foreach (StoreCategoryLocation item in objStoreCategoryLocationList)
            {
                objStoreCategoryLocationRepository.Delete(item);
                objStoreCategoryLocationRepository.SubmitChanges();
            }

            //Set new settings
            if (ddlLocationA.SelectedIndex != 0)
            {
                StoreCategoryLocation objStoreCategoryLocation = new StoreCategoryLocation();
                objStoreCategoryLocation.StoreID = StoreID;
                objStoreCategoryLocation.Location = "A";
                objStoreCategoryLocation.CategoryID = Convert.ToInt64(ddlLocationA.SelectedValue);
                objStoreCategoryLocationRepository.Insert(objStoreCategoryLocation);
                objStoreCategoryLocationRepository.SubmitChanges();
            }
            if (ddlLocationB.SelectedIndex != 0)
            {
                StoreCategoryLocation objStoreCategoryLocation = new StoreCategoryLocation();
                objStoreCategoryLocation.StoreID = StoreID;
                objStoreCategoryLocation.Location = "B";
                objStoreCategoryLocation.CategoryID = Convert.ToInt64(ddlLocationB.SelectedValue);
                objStoreCategoryLocationRepository.Insert(objStoreCategoryLocation);
                objStoreCategoryLocationRepository.SubmitChanges();
            }
            if (ddlLocationC.SelectedIndex != 0)
            {
                StoreCategoryLocation objStoreCategoryLocation = new StoreCategoryLocation();
                objStoreCategoryLocation.StoreID = StoreID;
                objStoreCategoryLocation.Location = "C";
                objStoreCategoryLocation.CategoryID = Convert.ToInt64(ddlLocationC.SelectedValue);
                objStoreCategoryLocationRepository.Insert(objStoreCategoryLocation);
                objStoreCategoryLocationRepository.SubmitChanges();
            }
            if (ddlLocationD.SelectedIndex != 0)
            {
                StoreCategoryLocation objStoreCategoryLocation = new StoreCategoryLocation();
                objStoreCategoryLocation.StoreID = StoreID;
                objStoreCategoryLocation.Location = "D";
                objStoreCategoryLocation.CategoryID = Convert.ToInt64(ddlLocationD.SelectedValue);
                objStoreCategoryLocationRepository.Insert(objStoreCategoryLocation);
                objStoreCategoryLocationRepository.SubmitChanges();
            }
            lblMsg.Text = "Data inserted successfully..";
        }
        catch {lblMsg.Text = "Data is not successfully inserted. please try again later..."; }

    }
    #endregion

    #region Methods
    protected void BindValues()
    {
        List<Category> objCategory = objCatogeryRepository.GetAllCategory();

        Common.BindDDL(ddlLocationA, objCategory, "CategoryName", "CategoryID", "-select category-");
        Common.BindDDL(ddlLocationB, objCategory, "CategoryName", "CategoryID", "-select category-");
        Common.BindDDL(ddlLocationC, objCategory, "CategoryName", "CategoryID", "-select category-");
        Common.BindDDL(ddlLocationD, objCategory, "CategoryName", "CategoryID", "-select category-");
    }

    protected void DisplayData()
    {
        List<StoreCategoryLocationRepository.StoreCategoryLocationResults> objStoreCategoryLocationResults = objStoreCategoryLocationRepository.GetLocationByStoreID(StoreID);

        foreach (StoreCategoryLocationRepository.StoreCategoryLocationResults item in objStoreCategoryLocationResults)
        {
            if (item.Location == "A")
                ddlLocationA.SelectedValue = item.CategoryID.ToString();
            else if (item.Location == "B")
                ddlLocationB.SelectedValue = item.CategoryID.ToString();
            else if (item.Location == "C")
                ddlLocationC.SelectedValue = item.CategoryID.ToString();
            else if (item.Location == "D")
                ddlLocationD.SelectedValue = item.CategoryID.ToString();
        }
    }
    #endregion
}
