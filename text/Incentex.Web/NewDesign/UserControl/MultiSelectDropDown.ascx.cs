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

public partial class MultiSelectDropDown : System.Web.UI.UserControl
{
    public Object DataSource
    {
        get
        {
            return repMultiSelect.DataSource;
        }

        set
        {
            repMultiSelect.DataSource = value;
        }
    }

    public String DataTextField
    {
        get;
        set;
    }

    public String DataValueField
    {
        get;
        set;
    }

    public String DataCheckField
    {
        get;
        set;
    }

    public String DataIndexField
    {
        get;
        set;
    }

    public String DataSelectText
    {
        get
        {
            return lblMultiSelect.Text;
        }
        set
        {
            lblMultiSelect.Text = value;
        }
    }

    public String ToolTip
    {
        get
        {
            return lblMultiSelect.ToolTip;
        }
        set
        {
            lblMultiSelect.ToolTip = value;
        }
    }

    public RepeaterItemCollection Items
    {
        get
        {
            return repMultiSelect.Items;
        }
    }

    public new void DataBind()
    {
        repMultiSelect.DataBind();
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        
    }
}
