using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class DashBoard1_NIELITCentreInst : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack )
        refreshdata();
    }

    public void refreshdata()
    {
         string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        // Count of NIELIT Centres
        parameters vPara = new parameters();
        vPara.count = 0;
    
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getNIELITACCREDITEDCentreCount", vPara);
                GridView1.DataSource = ds;
                GridView1.DataBind();



              
        btnBack.Visible = false;
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#50C878';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");               
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#50C878';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");          
            }
        }
    }

    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = GridView1.SelectedRow.RowIndex;
         int Id = Convert.ToInt32(GridView1.DataKeys[index].Values[0]);
        string name = GridView1.SelectedRow.Cells[1].Text;
        string country = GridView1.SelectedRow.Cells[2].Text;
        string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
        //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
     string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        // Count of NIELIT Centres
        parameters vPara = new parameters();
        vPara.count = 1;
        vPara.pID =Id;
    
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getNIELITACCREDITEDCentres", vPara);
               
               gvpartnerlist .DataSource =ds;
                gvpartnerlist.DataBind();
            
        
            GridView1.Visible = false;
            btnBack.Visible = true ;
            lblstatenamelist.Text = name.ToUpper()+ " " +" NIELIT ACCREDITATED INSTITUTE LIST";
        }      
       
    
    protected void gvpartnerlist_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F5F5DC ';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F5F5DC';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
        }
    }


   protected void btnBack_Click(object sender, EventArgs e)
   {      
       Response.Redirect("NIELITCentreInst.aspx");
   }
   protected void OnPaging(object sender, GridViewPageEventArgs e)
   {
       GridView1.PageIndex = e.NewPageIndex;
       GridView1.DataBind();
   }
   protected void OnPaging1(object sender, GridViewPageEventArgs e)
   {
       OnSelectedIndexChanged(sender, e);
       gvpartnerlist.PageIndex = e.NewPageIndex;
       gvpartnerlist.DataBind();
   }
}