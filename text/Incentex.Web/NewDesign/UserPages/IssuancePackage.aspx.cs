using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class UserPages_IssuancePackage : PageBase
{
    #region Page Properties

    Boolean IsFirstSubCatIDSet
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFirstSubCatIDSet"]);
        }
        set
        {
            ViewState["IsFirstSubCatIDSet"] = value;
        }
    }

    #endregion

    #region Page Fields

    List<GetUserProductCategoryAccessResult> lstCategories = new List<GetUserProductCategoryAccessResult>();

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindUniformAccessRepeater();
                BindIssuancePolicies();
                if (Request.QueryString.Count > 0 && !String.IsNullOrEmpty(Request.QueryString["pid"]))
                {
                    List<UniformIssuancePolicyItem> lstPoliItems = new UniformIssuancePolicyItemRepository().GetByUniformIssuancePolicyID(Convert.ToInt64(Request.QueryString["pid"]));
                    lstPoliItems = lstPoliItems.Where(le => le.AssociationIssuanceType != null && le.AssociationIssuanceType != 392 && le.AssociationIssuancePolicyNote != null && le.AssociationIssuancePolicyNote.Trim() != String.Empty).ToList();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Control Events

    protected void rptUniformAccess_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserProductCategoryAccessResult objDataItem = (GetUserProductCategoryAccessResult)e.Item.DataItem;

                Repeater rptSubItemsUniformAccess = (Repeater)e.Item.FindControl("rptSubItemsUniformAccess");

                rptSubItemsUniformAccess.DataSource = lstCategories.Where(le => le.CategoryID == objDataItem.CategoryID).ToList();
                rptSubItemsUniformAccess.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void repIssuancePolicies_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetIssuancePoliciesByUserInfoIDResult objItem = (GetIssuancePoliciesByUserInfoIDResult)e.Item.DataItem;

                if (objItem != null)
                {
                    HyperLink hlIssuancePolicy = (HyperLink)e.Item.FindControl("hlIssuancePolicy");
                    Label lblPolicyName = (Label)e.Item.FindControl("lblPolicyName");
                    Image imgIsOrdered = (Image)e.Item.FindControl("imgIsOrdered");

                    lblPolicyName.Text = objItem.IssuanceProgramName;

                    if (objItem.IsOrdered.Substring(0, 1) == "1")
                    {
                        imgIsOrdered.ImageUrl = "../StaticContents/img/new-label.png";
                        imgIsOrdered.ToolTip = "Already Ordered";
                        hlIssuancePolicy.Attributes.Add("onclick", "GeneralAlertMsg('You have already placed an order using this policy.')");
                        hlIssuancePolicy.NavigateUrl = "";
                    }
                    //else if (objItem.IsOrdered.Substring(2, 1) == "1")
                    //{
                    //    imgIsOrdered.ImageUrl = "../NewDesign/img/new-label.png";
                    //    imgIsOrdered.ToolTip = "Already Ordered";
                    //    hlIssuancePolicy.Attributes.Add("onclick", "GeneralAlertMsg('You have already placed an order using this policy group.')");
                    //    hlIssuancePolicy.NavigateUrl = "";
                    //}
                    else
                    {
                        imgIsOrdered.ImageUrl = "../StaticContents/img/normal-label.png";
                        imgIsOrdered.ToolTip = "Not Ordered Yet";
                        hlIssuancePolicy.NavigateUrl = "IssuancePackage.aspx?pid=" + objItem.UniformIssuancePolicyID;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private void BindUniformAccessRepeater()
    {
        try
        {
            lstCategories = new ProductItemDetailsRepository().GetUserProductCategoryAccess(IncentexGlobal.CurrentMember.UserInfoID);

            // To get Distinct value from Generic List using LINQ
            // Create an Equality Comprarer Intance
            IEqualityComparer<GetUserProductCategoryAccessResult> customComparer = new Common.PropertyComparer<GetUserProductCategoryAccessResult>("CategoryName");
            IEnumerable<GetUserProductCategoryAccessResult> distinctList = lstCategories.Distinct(customComparer);

            rptUniformAccess.DataSource = distinctList;
            rptUniformAccess.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindIssuancePolicies()
    {
        try
        {
            List<GetIssuancePoliciesByUserInfoIDResult> lstPolicies = new UniformIssuancePolicyRepository().GetIssuancePolicyByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            repIssuancePolicies.DataSource = lstPolicies;
            repIssuancePolicies.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}