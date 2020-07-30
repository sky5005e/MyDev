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
using Incentex.BE;
using Incentex.DA;

public partial class admin_UserControl_MenuUserControl : System.Web.UI.UserControl
{
    UserManagementMenuBE objBE = new UserManagementMenuBE();
    UserManagementMenuDA objDA = new UserManagementMenuDA();
    DataSet ds = new DataSet();
    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

 
    /// <summary>
    /// BindMainMenu()
    /// Nagmani Kumar 18/09/2010
    /// Bind the Main Menu. from table INC_MenuMaster
    /// </summary>
    /// 
    private void BindMainMenu()
    {
        try
        {
            if (IncentexGlobal.ManageID != 0)
            {
                int iManageID = Convert.ToInt32(IncentexGlobal.ManageID);
                PrivateBindMainMenu(iManageID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
 
    }


    void PrivateBindMainMenu(int iManageID)
    {
        try
        {
            dvMainSecondMenu.Visible = false;
            objBE.IManageID = iManageID;
            ds = objDA.getMenuData(objBE);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int cnt = 0;
                foreach (DataRow masterRow in ds.Tables[0].Rows)
                {
                    MenuItem menuName = new MenuItem(masterRow["sMenuName"].ToString());
                    cnt++;

                    for (int i = 0; i < MainMenu.Items.Count; i++)
                    {
                        if (MainMenu.Items[i].Text == "Comapny Data")
                        {
                            MainMenu.Items[i].Selected = true;
                        }
                    }
                    if (Request.QueryString["SubId"] != null)
                        menuName.NavigateUrl = masterRow["PageUrl"].ToString() + "?id=" + Request.QueryString["id"] + "&SubId=" + Request.QueryString["SubId"];
                    else
                        menuName.NavigateUrl = masterRow["PageUrl"].ToString() + "?id=" + Request.QueryString["id"];

                    //start condition change by mayur on 20th-jun-2012
                    if (cnt > 10 && iManageID == 5)
                        MainSecondMenu.Items.Add(menuName);
                    else
                        MainMenu.Items.Add(menuName);
                    //end condition change by mayur on 20th-jun-2012

                    if (cnt != ds.Tables[0].Rows.Count && cnt != 10)
                        menuName.SeparatorImageUrl = "~/Images/pipe.gif";
                }

                //start condition add by mayur on 20th-jun-2012
                if (ds.Tables[0].Rows.Count > 10 && iManageID == 5)
                    dvMainSecondMenu.Visible = true;
                //end condition add by mayur on 20th-jun-2012

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        
    }

    protected void SubMainMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
        try
        {
          
            if (Session["ManageID"] != null)
            {
                int iManageID = Convert.ToInt32(Session["ManageID"].ToString());
                //This is for Manage Company
                if (iManageID == 1)
                {
                    string strSubMenu = e.Item.Text;
                    e.Item.Selected = true;
                    
                }
                else if (iManageID == 2)
                {
                    string strSubMenu = e.Item.Text;
                    e.Item.Selected = true;
                }
                else if (iManageID == 3)
                {
                    string strSubMenu = e.Item.Text;    
                    e.Item.Selected = true;
                }
                BindMainMenu();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void PopulateMenu(int SelectedMenuIndex, int SelectedSubMenuIndex, Int64 Id, Int64 SubId
        ,bool ShowSubMenu)
    {
        try
        {
            BindMainMenu();
            DataSet ds = new DataSet();
            UserManagementMenuBE objBE = new UserManagementMenuBE();
            UserManagementMenuDA objDA = new UserManagementMenuDA();
           
            dvSubMenu.Visible = ShowSubMenu;
            
            Menu submenu = SubMainMenu;

            dvSubSecondMenu.Visible = false;
            Menu subsecondmenu = SubSecondMainMenu;

            //Find here Main Manu control item of the Click Menu
            string strMenu;

            //start condition add by mayur on 20th-jun-2012
            if (SelectedMenuIndex > 9 && IncentexGlobal.ManageID == 5)
            {
                MainSecondMenu.Items[SelectedMenuIndex-10].Selected = true;
                strMenu = MainSecondMenu.Items[SelectedMenuIndex-10].Text;
            }
            else
            {
                MainMenu.Items[SelectedMenuIndex].Selected = true;
                strMenu =  MainMenu.Items[SelectedMenuIndex].Text;
            }
            //end condition add by mayur on 20th-jun-2012

            objBE.SMenuName = strMenu;
            objBE.IManageID = IncentexGlobal.ManageID;
            ds = objDA.getMenuSubID(objBE);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataSet dsSubmenu = new DataSet();
                //objBE.IMenuID = 3;
                
                objBE.IMenuID = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                dsSubmenu = objDA.getMenuSubData(objBE);
                Menu me = new Menu();

                int cnt = 0;
                foreach (DataRow masterRow in dsSubmenu.Tables[0].Rows)
                {
                    MenuItem menuSubName = new MenuItem(masterRow["sMenuSubName"].ToString());
                    menuSubName.Text = masterRow["sMenuSubName"].ToString();

                    cnt++;

                    //start condition change by mayur on 3rd-jan-2012
                    if (cnt <= 7)
                        submenu.Items.Add(menuSubName);
                    else
                        subsecondmenu.Items.Add(menuSubName);
                    //end condition change by mayur on 3rd-jan-2012

                    menuSubName.NavigateUrl = masterRow["PageSubURL"].ToString() + "?Id=" + Id + "&SubId=" + SubId;
                    if(cnt != dsSubmenu.Tables[0].Rows.Count && cnt != 7)
                    {
                        menuSubName.SeparatorImageUrl = "~/Images/pipe.gif";
                    }
                }

                //start condition add by mayur on 3rd-jan-2012
                if (dsSubmenu.Tables[0].Rows.Count > 7 && ShowSubMenu == true)
                    dvSubSecondMenu.Visible = true;
                //end condition add by mayur on 3rd-jan-2012

                if (dsSubmenu.Tables[0].Rows.Count > 0)
                {
                    //start condition change by mayur on 3rd-jan-2012
                    if (SelectedSubMenuIndex > 6)
                        subsecondmenu.Items[SelectedSubMenuIndex-7].Selected = true;
                    else
                        submenu.Items[SelectedSubMenuIndex].Selected = true;
                    //end condition change by mayur on 3rd-jan-2012
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}