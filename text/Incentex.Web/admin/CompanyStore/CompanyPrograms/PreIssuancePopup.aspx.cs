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

public partial class admin_CompanyStore_CompanyPrograms_PreIssuancePopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SetInitialRow();
        }
    }
    private void SetInitialRow()
    {
       
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dr = dt.NewRow();
            dr["Column1"] = string.Empty;
            dt.Rows.Add(dr);
            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;

            Gridview1.DataSource = dt;
            Gridview1.DataBind();
        
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;
        
        for(int k=0;k<3;k++)
        {
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                   
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");

                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["Column1"] = box1.Text;


                        rowIndex++;
                    }
                    
                    //add new row to DataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    //Store the current data to ViewState
                    ViewState["CurrentTable"] = dtCurrentTable;

                    //Rebind the Grid with the current data
                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                }
            }
           
            else
            {
                Response.Write("ViewState is null");
            }
        }
            //Set Previous Data on Postbacks
            SetPreviousData();
      
    }
    private void SetPreviousData()
    {
        
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");

                        box1.Text = dt.Rows[i]["Column1"].ToString();

                        rowIndex++;

                    }
                }
            }
        }
   
    
    protected void ButtonAdd_Click1(object sender, EventArgs e)
    {
        
        //AddNewRowToGrid();
        
    }
}
