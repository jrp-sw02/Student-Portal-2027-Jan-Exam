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

public partial class DashBoard1_NSQFCounts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "GridViewBind()", true);
        //}
        
        refreshdata();
    }

    public void refreshdata()
    {
        parameters vPara = new parameters();
        DataSet ds = new DataSet();
        vPara.count = 1;
        vPara.pID = 1;

        ds = utility.executeProcedure("[getCandidatesNSQF]", vPara);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView1.DataSource = ds;
            GridView1.DataBind();
            hdnSelected.Value = "0";
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
        //if (GridView1.SelectedRow.RowIndex != null)
        //{
            int index = GridView1.SelectedRow.RowIndex;
            string Id = GridView1.SelectedValue.ToString();
            hdnSelected.Value = Convert.ToString(GridView1.SelectedDataKey.Value);

            if (Id == "NIELIT Centres")
            {
                lblCenter.Text = "1";
                getNSQFNIELITCentres(1);
                gvpartnerlist.Caption = "<center><b>NSQF CANDIDATES NIELIT CENTRES</b></centre>";
            }
            else
            {
                lblCenter.Text = "2";
                getNSQFNIELITCentres(2);
                gvpartnerlist.Caption = "NSQF CANDIDATES ACCREDITED INSTITUTES";
            }
            GridView1.Visible = false;
            tblCat.Visible = true;
            btnBack.Visible = true;
            lblGrand.Text = "0";

    }
    protected void getNSQFNIELITCentres(int id)
    {

        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        // Count of NIELIT Centres
        parameters vPara = new parameters();
        vPara.count = 1;
        vPara.pID = id;
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getStateNSQFCounts", vPara);
        gvpartnerlist.DataSource = ds;
        gvpartnerlist.DataBind();
        
    }
    protected void gvpartnerlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        

    }
    protected void gvpartnerlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
            gvpartnerlist.PageIndex = e.NewPageIndex;
            if (gvpartnerlist.Caption.Contains("NIELIT CENTRES"))
                getNSQFNIELITCentres(1);
            if(gvpartnerlist .Caption .Contains("ACCREDITED"))
                    getNSQFNIELITCentres(2);
            GridView1.Visible = false;
            tblCat.Visible = true;
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
        int index1 = gvpartnerlist.SelectedRow.RowIndex;
        string Id1 = gvpartnerlist.SelectedValue.ToString();
        hdnSelected.Value = "STATE";

        getNSQFNIELITCentresStates(1);

        GridView2.Caption = "<center><b>NSQF CANDIDATES NIELIT CENTRES</b></centre>";

        GridView1.Visible = false;
        gvpartnerlist.Visible = false;
        tblCat.Visible = false;
        btnBack.Visible = true;

    }
   protected void btnBack_Click(object sender, EventArgs e)
   {
       lblGrand.Text = "1";
       Response.Redirect("NSQFCounts.aspx");
   }
   protected void OnRowDataBound1(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
   {
       //if (e.Row.RowType == DataControlRowType.DataRow)
       //{
       //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvpartnerlist, "Select$" + e.Row.RowIndex);
       //    e.Row.ToolTip = "Click to select this row.";
       //}
       e.Row.ToolTip = "Click to select this row.";
   }


   protected void chk_sc_CheckedChanged(object sender, EventArgs e)
   {
       hdnSelected.Value = "NIELIT Centres";
       if (chk_sc.Checked == true)
       {
           gvpartnerlist.Columns[3].Visible = true;
           gvpartnerlist.Columns[8].Visible = true;
       }
       else
       {
           gvpartnerlist.Columns[3].Visible = false;
           gvpartnerlist.Columns[8].Visible = false;
       }
       btnBack.Visible = true;
   }

   protected void chk_st_CheckedChanged(object sender, EventArgs e)
   {
       hdnSelected.Value = "NIELIT Centres";
       if (chk_st.Checked == true)
       {
           gvpartnerlist.Columns[4].Visible = true;
           gvpartnerlist.Columns[9].Visible = true;
       }
       else
       {
           gvpartnerlist.Columns[4].Visible = false;
           gvpartnerlist.Columns[9].Visible = false;
       }
       btnBack.Visible = true;
   }

   protected void chk_OBC_CheckedChanged(object sender, EventArgs e)
   {
       hdnSelected.Value = "NIELIT Centres";
       if (chk_OBC.Checked == true)
       {
           gvpartnerlist.Columns[5].Visible = true;
           gvpartnerlist.Columns[10].Visible = true;
       }
       else
       {
           gvpartnerlist.Columns[5].Visible = false;
           gvpartnerlist.Columns[10].Visible = false;
       }
   }

   protected void chk_PH_CheckedChanged(object sender, EventArgs e)
   {
       hdnSelected.Value = "NIELIT Centres";
       if (chk_PH.Checked == true)
       {
           gvpartnerlist.Columns[6].Visible = true;
           gvpartnerlist.Columns[11].Visible = true;
       }
       else
       {
           gvpartnerlist.Columns[6].Visible = false;
           gvpartnerlist.Columns[11].Visible = false;
       }
       btnBack.Visible = true;
   }


   protected void getNSQFNIELITCentresStates(int id)
   {
       string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

       // Count of NIELIT Centres
       parameters vPara = new parameters();
       vPara.count = 0;

       DataSet ds = new DataSet();
       ds = utility.executeProcedure("getCandEmergingTrends", vPara);

       GridView2.DataSource = ds;
       GridView2.DataBind();

   }


}