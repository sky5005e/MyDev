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
using System.Collections.Generic;

public partial class RND_Default2 : System.Web.UI.Page
{
     
    Int64 Count
    {
        get
        {
            if (ViewState["Count"] == null)
            {
                ViewState["Count"] = 0;
            }
            return Convert.ToInt64(ViewState["Count"]);
        }
        set
        {
            ViewState["Count"] = value;
        }
    }
    private static List<int> ListValues
    {
        get
        {

            if ((HttpContext.Current.Session["ListValues"]) == null)
                return new List<int>(50);
            else
                return ((List<int>)HttpContext.Current.Session["ListValues"]);

        }
        set
        {
            HttpContext.Current.Session["ListValues"] = value;
        }
    }

    public class TrackingNumber
    {
       public int trackingnuber { get; set; }
       public int suppliernumber { get; set; }
       public int ordernumber { get; set; }
    }
    private static List<TrackingNumber> ListValuesTracking
    {
        get
        {
            if ((HttpContext.Current.Session["ListValuesTracking"]) == null)
                return new List<TrackingNumber>();
            else
                return (List<TrackingNumber>)HttpContext.Current.Session["ListValuesTracking"];
        }
        set
        {
            HttpContext.Current.Session["ListValuesTracking"] = value;
        }
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {   
            //Session["ListVal"] = null;
            //ListValuesTracking = null;
            bindgrid();
        }
    }
    private void bindgrid()
    {
        if (ListValuesTracking.Count > 0)
        {
            grv.DataSource = ListValuesTracking;
            grv.DataBind();
        }
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        List<TrackingNumber> obj = new List<TrackingNumber>();
        TrackingNumber objnumber = new TrackingNumber();
        objnumber.trackingnuber = Convert.ToInt32(txtValue.Text);
        objnumber.ordernumber = 1;
        objnumber.suppliernumber = 1;
        if (Session["ListVal"] != null)
        {
            List<TrackingNumber> objtemp = new List<TrackingNumber>();
            
            
            objtemp = (List<TrackingNumber>)Session["ListVal"];
            objtemp.Add(objnumber);
            obj = objtemp;
            Session["ListVal"] = obj;
        }
        else
        {
            obj.Add(objnumber);
            Session["ListVal"] = obj;
        }

        ListValuesTracking = obj;
        bindgrid();
        txtValue.Text = string.Empty;
    }
 
    protected void grv_RowEditing1(object sender, GridViewEditEventArgs e)
    {
        grv.EditIndex = e.NewEditIndex;
        bindgrid();
    }
    protected void grv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grv.Rows[e.RowIndex];
        ListValuesTracking[e.RowIndex].trackingnuber = Convert.ToInt32(((TextBox)row.Cells[0].Controls[1]).Text);
        grv.EditIndex = -1;
        bindgrid();
    }
    protected void grv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ListValuesTracking.RemoveAt(e.RowIndex);
        bindgrid();
    }
    protected void grv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grv.EditIndex = -1;
        bindgrid();
    }
}
