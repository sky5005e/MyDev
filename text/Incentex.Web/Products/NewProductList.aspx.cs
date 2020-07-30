using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.BE;
using System.Web.UI.HtmlControls;

public partial class Products_NewProductList : PageBase
{
    #region Objects/Variables

    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();

    #endregion

    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "New Products List";
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

    #endregion

    #region DisplayData

    void DisplayData()
    {
        lstProductList.DataBind();

        if (lstProductList.Items.Count == 0)
            lblMsg.Visible = true;
    }

    #endregion

    #region Events
    
    protected void lstProductList_DataBinding(object sender, EventArgs e)
    {
        int employeetypeid = 0;

        List<SP_GetStoreProductResult> objList = new List<SP_GetStoreProductResult>();

        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        if (objCmpnyInfo.EmployeeTypeID != null)
        {
            employeetypeid = Convert.ToInt32(objCmpnyInfo.EmployeeTypeID);
        }
        objList = objStoreProductRepository.GetNewStoreProductItems((Int64)IncentexGlobal.CurrentMember.CompanyId, (Int64)objCmpnyInfo.WorkgroupID, (Int64)IncentexGlobal.CurrentMember.UserInfoID);


        //check here compay admin then no filter check by employeetype
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
        {
            objList = SortListingOrderByGender(objList);
        }
        else
        {

            if (employeetypeid == 0)
            {
                objList = SortListingOrderByGender(objList.Where(s => s.EmployeeTypeid == 0 || s.EmployeeTypeid == null).ToList());
            }
            else
            {
                objList = SortListingOrderByGender(objList.Where(s => s.EmployeeTypeid == 0 || s.EmployeeTypeid == null || s.EmployeeTypeid == employeetypeid).ToList());
            }
        }

        var objProducts = (from l in objList
                           select new { l.StoreProductID, l.MasterItemNo }
                           ).Distinct().ToList();

        List<StoreProductLocal> objProductList = new List<StoreProductLocal>();

        foreach (var p in objProducts)
        {
            objProductList.Add(new StoreProductLocal() { MasterItemNo = p.MasterItemNo, StoreProductID = p.StoreProductID });
        }

        
        lstProductList.DataSource = objProductList;

    }
    
    //Sorting product list based on Gender who is logged in and viewing
    public List<SP_GetStoreProductResult> SortListingOrderByGender(List<SP_GetStoreProductResult> objProductList)
    {
        List<SP_GetStoreProductResult> malelist = new List<SP_GetStoreProductResult>();
        List<SP_GetStoreProductResult> femalelist = new List<SP_GetStoreProductResult>();
        List<SP_GetStoreProductResult> unisexlist = new List<SP_GetStoreProductResult>();
        CompanyEmployee obj = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        INC_Lookup objLookup = new LookupRepository().GetById(obj.GenderID);

        foreach (SP_GetStoreProductResult a in objProductList)
        {
            if (a.GarmentTypeName == "Male")
            {
                malelist.Add(a);
            }
            else if (a.GarmentTypeName == "Female")
            {
                femalelist.Add(a);
            }
            else
            {
                unisexlist.Add(a);
            }
        }
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
        {
            if (objLookup.sLookupName == "Male")
            {
                malelist.AddRange(femalelist);
            }
            else
            {
                femalelist.AddRange(malelist);
            }
        }

        if (objLookup.sLookupName == "Male")
        {
            malelist.AddRange(unisexlist);
        }
        else
        {
            femalelist.AddRange(unisexlist);
        }

        if (objLookup.sLookupName == "Male")
        {
            return malelist;
        }
        else
        {
            return femalelist;
        }
    }

    protected void lstProductList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            StoreProductLocal obj = e.Item.DataItem as StoreProductLocal;

            if (obj.MasterItemNo != null && obj.StoreProductID != null)
            {
                List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById((int)obj.StoreProductID, (int)obj.MasterItemNo);

                if (objImageList.Count > 0)
                {
                    HtmlAnchor lnkImage = e.Item.FindControl("lnkImage") as HtmlAnchor;
                    lnkImage.HRef = "../UploadedImages/ProductImages/" + objImageList[0].LargerProductImage;

                    HtmlImage imgProduct = e.Item.FindControl("imgProduct") as HtmlImage;
                    imgProduct.Src = "../UploadedImages/ProductImages/Thumbs/" + objImageList[0].ProductImage;
                }
            }
        }


    }

    protected void lstProductList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Detail":
                Response.Redirect("NewProductDetail.aspx?MasterItemNo=" + e.CommandArgument + /*"&SubCat=" + this.PSubCategoryName +*/ "&StoreProductId=" + ((HiddenField)e.Item.FindControl("hndStoreProductImageId")).Value.ToString());
                break;
        }
    } 

    #endregion

    #region Class

    class StoreProductLocal
    {
        public Int64? StoreProductID { get; set; }

        public Int64? MasterItemNo { get; set; }
    }

    #endregion
}
