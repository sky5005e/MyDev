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
using ASP;

public partial class DropDownControl : System.Web.UI.UserControl
{


    #region Label Properties Exposed

    public String LabelText
    {
        get
        {
            return lblText.Text;
        }

        set
        {
            lblText.Text = value.Trim();
        }
    }

    public String LabelCssClass
    {
        get
        {
            return lblText.CssClass;
        }

        set
        {
            lblText.CssClass = value;
        }
    }

    public String LabelID
    {
        get
        {
            return lblText.ID;
        }

        set
        {
            lblText.ID = value.Trim();
        }
    }

    #endregion

    #region HiddenField Properties Exposed
    public Int64 FieldMasterID
    {
        get
        {
            return Convert.ToInt64(hdnFieldMasterID.Value);
        }

        set
        {
            hdnFieldMasterID.Value = Convert.ToString(value);
        }
    }

    public Int64 FieldDetailID
    {
        get
        {
            return Convert.ToInt64(hdnFieldDetailID.Value);
        }

        set
        {
            hdnFieldDetailID.Value = Convert.ToString(value);
        }
    }
    #endregion

    public String LiTagCssClassValue
    {
        get
        {
            return Convert.ToString(ViewState["LiTagCssClassValue"]);
        }

        set
        {
            ViewState["LiTagCssClassValue"] = value;
        }
    }

    public Int32 SelectedIndex
    {
        get
        {
            return ddlCustomDropDown.SelectedIndex;
        }

        set
        {
            ddlCustomDropDown.SelectedIndex = value;
        }
    }

    public ListItem SelectedItem
    {
        get
        {
            return ddlCustomDropDown.SelectedItem;
        }
    }

    public String SelectedValue
    {
        get
        {
            return ddlCustomDropDown.SelectedValue;
        }

        set
        {
            ddlCustomDropDown.SelectedValue = value;
        }
    }

    public String GroupName
    {
        get
        {
            return ddlCustomDropDown.GroupName;
        }

        set
        {
            ddlCustomDropDown.GroupName = value;
        }
    }

    public String DefaultOptionText
    {
        get
        {
            return ddlCustomDropDown.DefaultOptionText;
        }

        set
        {
            ddlCustomDropDown.DefaultOptionText =  value;
        }
    }

    public String ApplyToAll
    {
        get
        {
            return ddlCustomDropDown.ApplyToAll;
        }

        set
        {
            ddlCustomDropDown.ApplyToAll = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkRemoveControl_Click(object sender, EventArgs e)
    {
        OnClick(EventArgs.Empty);
    }

    public delegate void ClickHandler(object sender, EventArgs e);

    public event ClickHandler Click;

    protected virtual void OnClick(EventArgs e)
    {
        ClickHandler handler = Click;
        if (handler != null)
        {
            handler(this, e);
        }
    }

}
