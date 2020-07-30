using System;
using System.Web.UI.WebControls;

public partial class TextBoxControl : System.Web.UI.UserControl
{
   

    #region Custom Properties

    public Boolean IsEditing
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsEditing"]);
        }

        set
        {
            ViewState["IsEditing"] = value;
        }
    }

    #endregion

    #region Li Tag Properties Exposed

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

    #endregion

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
    
    #region TextBox Properties Exposed

    public Int32 MaxLength
    {
        get
        {
            return txtSpecification.MaxLength;
        }

        set
        {
            txtSpecification.MaxLength = value;
        }
    }

    public String Text
    {
        get
        {
            return txtSpecification.Text.Trim();
        }

        set
        {
            txtSpecification.Text = value.Trim();
        }
    }

    public String TextBoxID
    {
        get
        {
            return txtSpecification.ID;
        }

        set
        {
            txtSpecification.ID = value.Trim();
        }
    }

    public String TextBoxCssClass
    {
        get
        {
            return txtSpecification.CssClass;
        }

        set
        {
            txtSpecification.CssClass = value;
        }
    }

    public String GroupName
    {
        get
        {
            return txtSpecification.Attributes["data-Group"];
        }

        set
        {
            txtSpecification.Attributes.Add("data-Group", value);
        }
    }

    public String ApplyToAll
    {
        get
        {
            return txtSpecification.Attributes["ApplyToAll"];
        }

        set
        {
            txtSpecification.Attributes.Add("ApplyToAll", value);
        }
    }

    #endregion

    #region LinkButton Properties Exposed

    public String LinkButtonCssClass
    {
        get
        {
            return lnkbtnRemoveClick.CssClass;
        }

        set
        {
            lnkbtnRemoveClick.CssClass = value;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        //txtSpecification.Text = Request.Form[txtSpecification.UniqueID];
        //lblText.Text = Request.Form[lblText.UniqueID];
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
