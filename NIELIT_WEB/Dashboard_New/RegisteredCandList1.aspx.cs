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

public partial class DashBoard1_RegisteredCandList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.refreshdata();
        }
        //refreshdata();
    }


    protected void BindGridView()
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
            using (SqlCommand cmd = new SqlCommand("Nielit_Reg_CandList_all", con))
            {
                cmd.CommandTimeout = 0;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                GridView1.DataSource = ds;
                GridView1.DataBind();
                // tblCat.Visible = true;
                //hdnSelected.Value = "0";
            }
        }
        btnBack.Visible = false;
    }
    public void refreshdata()
    {
        BindGridView();
        //btnBack.Visible = false;
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
    //protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    GridView1.PageIndex = e.NewPageIndex;
    //    refreshdata(); //bindgridview will get the data source and bind it again
    //}
    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        // Rebind the GridView to display data for the new page index
       // BindGridView(); // You should replace BindGridView() with the method you use to bind data to the GridView
      //  GridView1.DataBind();
        BindGridView();
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    Label lblheadSC = (Label)e.Row.FindControl("lblheadSC");
        //    if (chk_sc.Checked == true)
        //    {
        //        if (lblheadSC != null)
        //            lblheadSC.Visible = true;
        //        //  string s = e.Row.Cells[3].Text;
        //        //  e.Row.Cells[3].Attributes.Add("style", "display:block;"); //true
        //    }
        //    else
        //    {
        //        if (lblheadSC != null)
        //            lblheadSC.Visible = false;
        //        // string s = e.Row.Cells[3].Text;
        //        // e.Row.Cells[3].Attributes.Add("style", "display:none;"); //false
        //    }

        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
        //e.Row.ToolTip = "Click to select this row.";
        //    //e.Row.Cells[0].Text = "Date";
        //    //e.Row.Cells[3].Attributes.Add("style", "display:block;");
        //    Label lblSC = (Label)e.Row.FindControl("lblSC");
           

        //    if (chk_sc.Checked == true)
        //    {
        //        if (lblSC != null )
        //            lblSC.Visible = true;
        //      //  string s = e.Row.Cells[3].Text;
        //      //  e.Row.Cells[3].Attributes.Add("style", "display:block;"); //true
        //    }
        //    else
        //    {
        //        if (lblSC != null)
        //            lblSC.Visible = false;
        //       // string s = e.Row.Cells[3].Text;
        //       // e.Row.Cells[3].Attributes.Add("style", "display:none;"); //false
        //    }

        //}
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = GridView1.SelectedRow.RowIndex;
        int Id = Convert.ToInt32(GridView1.DataKeys[index].Values[0]);
        hdnSelected.Value = "STATE";
        hdnStateID.Value = Id.ToString();

        string name = GridView1.SelectedRow.Cells[1].Text;
        string country = GridView1.SelectedRow.Cells[2].Text;
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
            string sqlquery = " select  st.Name FROM [NIELIT].[dbo].[Location] st where st.ID = " + Id + "";
                             //"[NIELIT].[dbo].[Institute] inst where Parent_ID=1 and nb.InstituteID=inst.ID and inst.State_ID=st.ID and st.ID = " + Id + "";

            using (SqlCommand cmd = new SqlCommand(sqlquery, con))
            {
                parameters vPara = new parameters();
                vPara.count = 1;
                vPara.pID = Id;
                DataSet ds = new DataSet();
                ds = utility.executeProcedure("getStateWiseNSQFCounts", vPara);
                GridView3.DataSource = ds;
                GridView3.DataBind();
            }
            GridView1.Visible = false;
            btnBack.Visible = true;
            tblCat.Visible = false;
            GridView3.Caption = "<center><b> "+ name.ToUpper() + " " + " CANDIDATES NIELIT CENTRES</b></centre>";
        }

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
        Response.Redirect("RegisteredCandList.aspx");
    }
    //protected void chk_sc_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chk_sc.Checked == true)
    //    {
    //        GridView1.Columns[3].Visible = true;
    //        GridView1.Columns[9].Visible = true;
    //    }
    //    else
    //    {
    //        GridView1.Columns[3].Visible = false;
    //        GridView1.Columns[9].Visible = false;
    //    }

    //}

    //protected void chk_st_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chk_st.Checked == true)
    //    {
    //        GridView1.Columns[4].Visible = true;
    //        GridView1.Columns[10].Visible = true;
    //    }
    //    else
    //    {
    //        GridView1.Columns[4].Visible = false;
    //        GridView1.Columns[10].Visible = false;
    //    }
    //}

    //protected void chk_OBC_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chk_OBC.Checked == true)
    //    {
    //        GridView1.Columns[5].Visible = true;
    //        GridView1.Columns[11].Visible = true;
    //    }
    //    else
    //    {
    //        GridView1.Columns[5].Visible = false;
    //        GridView1.Columns[11].Visible = false;
    //    }
    //}

    //protected void chk_PH_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chk_PH.Checked == true)
    //    {
    //        GridView1.Columns[6].Visible = true;
    //        GridView1.Columns[12].Visible = true;
    //    }
    //    else
    //    {
    //        GridView1.Columns[6].Visible = false;
    //        GridView1.Columns[12].Visible = false;
    //    }
    //}
}