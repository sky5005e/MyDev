using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomDropDown: System.Web.UI.UserControl
{
    #region Custom Properties

    public Boolean IsEditing
    {
        get
        {
            return Convert.ToBoolean(hdnIsEditing.Value);
        }

        set
        {
            hdnIsEditing.Value = Convert.ToString(value);
        }
    }

    public String Module
    {
        get
        {
            return Convert.ToString(hdnModule.Value);
        }

        set
        {
            hdnModule.Value = Convert.ToString(value);
        }
    }

    #endregion

    #region DropDownList Properties Exposed

    public Object DataSource
    {
        get
        {
            return ddlDropDown.DataSource;
        }

        set
        {
            ddlDropDown.DataSource = value;
        }
    }

    public String DataTextField
    {
        get
        {
            return ddlDropDown.DataTextField;
        }

        set
        {
            ddlDropDown.DataTextField = value;
        }
    }

    public String DataValueField
    {
        get
        {
            return ddlDropDown.DataValueField;
        }

        set
        {
            ddlDropDown.DataValueField = value;
        }
    }

    public ListItemCollection Items
    {
        get
        {
            return ddlDropDown.Items;
        }
    }

    public Int32 SelectedIndex
    {
        get
        {
            return ddlDropDown.SelectedIndex;
        }

        set
        {
            ddlDropDown.SelectedIndex = value;
        }
    }

    public ListItem SelectedItem
    {
        get
        {
            return ddlDropDown.SelectedItem;
        }
    }

    public String SelectedValue
    {
        get
        {
            return ddlDropDown.SelectedValue;
        }

        set
        {
            ddlDropDown.SelectedValue = value;
        }
    }

    public String DropDownCssClass
    {
        get
        {
            return ddlDropDown.CssClass;
        }

        set
        {
            ddlDropDown.CssClass = value;
        }
    }

    public String GroupName
    {
        get
        {
            return ddlDropDown.Attributes["data-Group"];
        }

        set
        {
            ddlDropDown.Attributes.Add("data-Group", value);
        }
    }

    public String DefaultOptionText
    {
        get
        {
            return ddlDropDown.Attributes["DefaultOptionText"];
        }

        set
        {
            ddlDropDown.Attributes.Add("DefaultOptionText", value);
        }
    }

    public String ApplyToAll
    {
        get
        {
            return txtTextBox.Attributes["ApplyToAll"];
        }

        set
        {
            txtTextBox.Attributes.Add("ApplyToAll", value);
        }
    }

    #endregion

    #region TextBox Properties Exposed

    public Int32 MaxLength
    {
        get
        {
            return txtTextBox.MaxLength;
        }

        set
        {
            txtTextBox.MaxLength = value;
        }
    }

    public String Text
    {
        get
        {
            return txtTextBox.Text.Trim();
        }

        set
        {
            txtTextBox.Text = value.Trim();
        }
    }

    public String TextBoxCssClass
    {
        get
        {
            return txtTextBox.CssClass;
        }

        set
        {
            txtTextBox.CssClass = value;
        }
    }
    #endregion

    #region HiddenField Properties Exposed

    public String Value
    {
        get
        {
            return hdnValueToAddNewOption.Value;
        }

        set
        {
            hdnValueToAddNewOption.Value = value;
        }
    }

    public String ParentSpanClassToRemove
    {
        get
        {
            return hdnParentSpanClassToRemove.Value;
        }

        set
        {
            hdnParentSpanClassToRemove.Value = value;
        }
    }

    #endregion

    #region EditableDropDown Events

    public delegate void SaveNewOptionAttemptedEventHandler(Object sender, EventArgs e);

    public event SaveNewOptionAttemptedEventHandler SaveNewOptionAttempted;

    protected virtual void OnSaveNewOptionAttempted(EventArgs e)
    {
        SaveNewOptionAttemptedEventHandler handler = SaveNewOptionAttempted;
        if (handler != null)
        {
            handler(this, e);
        }
    }

    public delegate void SelectedIndexChangedHandler(Object sender, EventArgs e);

    public event SelectedIndexChangedHandler SelectedIndexChanged;

    protected virtual void OnSelectedIndexChanged(EventArgs e)
    {
        SelectedIndexChangedHandler handler = SelectedIndexChanged;
        if (handler != null)
        {
            if (ddlDropDown.SelectedItem.Value == hdnValueToAddNewOption.Value)
            {
                btnButton_Click(null, new EventArgs() { });
            }
            handler(this, e);
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void DataBind()
    {
        base.DataBind();
        ddlDropDown.DataBind();
    }

    protected void btnButton_Click(object sender, EventArgs e)
    {
        if (IsEditing)
        {
            //&& !String.IsNullOrEmpty(txtTextBox.Text.Trim())
            OnSaveNewOptionAttempted(EventArgs.Empty);
        }
        else
        {
            ddlDropDown.ClearSelection();
            ddlDropDown.Items[0].Selected = true;
        }
        ToggleView();
    }

    protected void ddlDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnSelectedIndexChanged(EventArgs.Empty);
    }

    protected void ToggleView()
    {
        IsEditing = !IsEditing;
        ddlDropDown.Visible = !IsEditing;
        txtTextBox.Visible = IsEditing;
        txtTextBox.Text = "";
    }
}
