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

public partial class DashBoard1_TrainingPartners : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if(!Page.IsPostBack )
        refreshdata();
    }

    public void refreshdata()
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {


            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "getTrainingPartners";
            cmd.CommandType = CommandType.StoredProcedure;

            using (cmd)
            {
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                GridView1.DataSource = ds;
                GridView1.DataBind();
              //  divNielitCentreGraph.Visible = false;
            }
        }
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
        string Id = GridView1.SelectedValue.ToString();

        if (Id == "N")
        {

            getNIELITCentres ();
                GridView1.Visible = false;
                btnBack.Visible = true;
              //  divNielitCentreGraph.Visible = true;
               // Label2.Text = "Training Partners/NIELIT Centres & Extension Centres";
            
        }
        if (Id == "A")
        {
            Response.Redirect("NielitCentreInst.aspx");
        }
    }
    protected void getNIELITCentres()
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        // Count of NIELIT Centres
        parameters vPara = new parameters();
        vPara.count = 0;
        
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getStateNIELITCentresForCourse", vPara);
        gvpartnerlist.DataSource = ds;
        gvpartnerlist.DataBind();
        
    }
    protected void gvpartnerlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CheckCourses")
        {
            string ID = e.CommandArgument.ToString();
            //Get the data value and bind into textbox..
            if (ID != null)
            {
                //string querystring = EncryptDecryptD.Encrypt("NIELITCentreCourses.aspx?pInstituteID=" + ID);
                
                //Response.Redirect(querystring);

                Response.Redirect("NIELITCentreCourses.aspx?pInstituteID=" + ID);
            }
              //  Response.Redirect("NIELITCentreCourses.aspx?pInstituteID=" + ID);

        }

    }
        protected void gvpartnerlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
            gvpartnerlist.PageIndex = e.NewPageIndex;
            getNIELITCentres();
            GridView1.Visible = false;
            btnBack.Visible = true;
            gvpartnerlist.DataBind();

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
    protected void gvpartnerlist_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string querystring = EncryptDecryptD.Encrypt("NIELITCentreCourses.aspx?pInstituteID=" + gvpartnerlist.SelectedValue);

        Response.Redirect(querystring);
      //  Response.Redirect("NIELITCentreCourses.aspx?pInstituteID=" + gvpartnerlist.SelectedValue);
    }
   protected void btnBack_Click(object sender, EventArgs e)
   {
       Response.Redirect("TrainingPartners.aspx");
   }
   protected void OnRowDataBound1(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
   {
       if (e.Row.RowType == DataControlRowType.DataRow)
       {
           e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvpartnerlist, "Select$" + e.Row.RowIndex);
           e.Row.ToolTip = "Click to select this row.";
       }
   }
}