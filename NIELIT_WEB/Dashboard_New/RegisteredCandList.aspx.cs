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
using System.Text;

public partial class DashBoard1_RegisteredCandList : BasePage
{

    string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.refreshdata();
            BindFromYear();
            BindToYear();
          //  GridView1.RowDataBound += OnRowDataBound;
          //  OnRowDataBound(sender, e);

        }
       // refreshdata();      
    }
   
    private void BindFromYear()
    {

        try
        {
            string query = "WITH YearsCTE AS ( SELECT 1990 AS Year UNION ALL SELECT Year + 1 FROM YearsCTE WHERE Year < YEAR(GETDATE()))SELECT Year FROM YearsCTE OPTION (MAXRECURSION 0);";

            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    ddlFromYear.DataSource = dt;
                    ddlFromYear.DataTextField = "Year"; // Displayed text
                    ddlFromYear.DataValueField = "Year";  // Value associated with the item
                    ddlFromYear.DataBind();

                    ddlFromYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--", "0"));

                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private void BindToYear()
    {

        try
        {
            string query = "WITH YearsCTE AS ( SELECT 1990 AS Year UNION ALL SELECT Year + 1 FROM YearsCTE WHERE Year < YEAR(GETDATE()))SELECT Year FROM YearsCTE OPTION (MAXRECURSION 0);";

            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    ddlToYear.DataSource = dt;
                    ddlToYear.DataTextField = "Year"; // Displayed text
                    ddlToYear.DataValueField = "Year";  // Value associated with the item
                    ddlToYear.DataBind();

                    ddlToYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--", "0"));

                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void refreshdata()
    {

        Int32? gender = Convert.ToInt32(ddlGender.SelectedValue) ;  
        Int32? fromYear = Convert.ToInt32(ddlFromYear.SelectedValue);
        Int32? toYear = Convert.ToInt32(ddlToYear.SelectedValue);
        Int32? fromMonth = Convert.ToInt32(ddlFromMonth.SelectedValue);
        Int32? toMonth = Convert.ToInt32(ddlToMonth.SelectedValue);
      
        using (SqlConnection con = new SqlConnection(CS))
        {            
            using (SqlCommand cmd = new SqlCommand("DashboardRecordsRegCert", con))
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                if (gender == 1 || gender == 2 || gender == 3)
                {
                    cmd.Parameters.AddWithValue("@pGender", gender);
                }
                if (fromYear != 0 && toYear != 0 && fromMonth != 0 && toMonth != 0)
                {

                    cmd.Parameters.AddWithValue("@pYearFrom", fromYear);
                    cmd.Parameters.AddWithValue("@pYearTo", toYear);

                    cmd.Parameters.AddWithValue("@pmonthFrom", fromMonth);
                    cmd.Parameters.AddWithValue("@pmonthTo", toMonth);
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                GridView1.DataSource = ds;
                GridView1.DataBind();
                tblCat.Visible = true;
                hdnSelected.Value = "0";
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
               // e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#50C878';");
             //   e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");


            }
            else
            {
              //  e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#50C878';");
              //  e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }



          

            //int lastRowIndex = GridView1.Rows.Count - 1;
            //if (e.Row.RowIndex == lastRowIndex)
            //{
            //    Label lblRowNumber = (Label)e.Row.FindControl("lblRowNumber");
            //    if (lblRowNumber != null)
            //    {
            //        lblRowNumber.Visible = false;
            //    }
            //}
        }
    }


    //protected void GridView1_DataBound(object sender, EventArgs e)
    //{       
    //    int lastRowIndex = GridView1.Rows.Count - 1;
    //    if (lastRowIndex >= 0)
    //    {
    //        Label lblRowNumber = (Label)GridView1.Rows[lastRowIndex].FindControl("lblRowNumber");
    //        if (lblRowNumber != null)
    //        {
    //            lblRowNumber.Visible = false;
    //        }
    //    }

       
    //}

    protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#50C878';");
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
            else
            {
                //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#50C878';");
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
        }
    } 
    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
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


           // int lastRowIndex = 33;
           //   int lastRowIndex = GridView1.Rows.Count - 2;// row.DataView.Count
            //int lastRowIndex = GridView1.Rows.Count - 1;
            //ShowAlert("index" + lastRowIndex.ToString());
            //if (e.Row.RowIndex == lastRowIndex)
            //{
            //    // Disable click event for the last row
            //    e.Row.Attributes.Remove("onclick");
            //    e.Row.BackColor = System.Drawing.Color.Firebrick;
            //    // e.Row.Attributes["onclick"] = "";
            //}




            if (e.Row.Cells[1].Text == "GRAND TOTAL")
            {
                //e.Row.Attributes.Remove("onclick");
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F82605");
                e.Row.ForeColor = System.Drawing.Color.Beige;
                e.Row.Font.Bold = true; 
            }
           

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
    protected void OnRowDataBound1(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView3, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";

            if (e.Row.Cells[1].Text == "GRAND TOTAL")
            {
                //e.Row.Attributes.Remove("onclick");
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F82605");
                e.Row.ForeColor = System.Drawing.Color.Beige;
                e.Row.Font.Bold = true;
            }
           
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
        //currenttxtValue.Value = "STATE";
        hdnStateID.Value = Id.ToString();
        HdnfSc.Value = chk_sc.Checked ? "Y" : "";
        HdnfST.Value = chk_st.Checked ? "Y" : "";
        HdnfOBC.Value = chk_OBC.Checked ? "Y" : "";
        HdnfPH.Value = chk_PH.Checked ? "Y" : "";
        string name = GridView1.SelectedRow.Cells[1].Text;
        string country = GridView1.SelectedRow.Cells[2].Text;




        Int32? gender = Convert.ToInt32(ddlGender.SelectedValue);
        Int32? fromYear = Convert.ToInt32(ddlFromYear.SelectedValue);
        Int32? toYear = Convert.ToInt32(ddlToYear.SelectedValue);

        Int32? fromMonth = Convert.ToInt32(ddlFromMonth.SelectedValue);
        Int32? toMonth = Convert.ToInt32(ddlToMonth.SelectedValue);

        char? pSC = !string.IsNullOrEmpty(HdnfSc.Value) ? Convert.ToChar(HdnfSc.Value) : (char?)null;
        char? pST = !string.IsNullOrEmpty(HdnfST.Value) ? Convert.ToChar(HdnfST.Value) : (char?)null;
        char? pOBC = !string.IsNullOrEmpty(HdnfOBC.Value) ? Convert.ToChar(HdnfOBC.Value) : (char?)null;
        char?  pPH = !string.IsNullOrEmpty(HdnfPH.Value) ? Convert.ToChar(HdnfPH.Value) : (char?)null;


        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
          
            using (SqlCommand cmd = new SqlCommand("getStateWiseNSQFCounts_grid", con))
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pStateID", Id);
                cmd.Parameters.AddWithValue("@pSC", pSC);
                cmd.Parameters.AddWithValue("@pST", pST);
                cmd.Parameters.AddWithValue("@pOBC", pOBC);
                cmd.Parameters.AddWithValue("@pPH", pPH);
                if (gender == 1 || gender == 2 || gender == 3)
                {
                    cmd.Parameters.AddWithValue("@pGender", gender);
                }
                if (fromYear != 0 && toYear != 0 && fromMonth !=0 && toMonth != 0)
                {
                    cmd.Parameters.AddWithValue("@pYearFrom", fromYear);
                    cmd.Parameters.AddWithValue("@pYearTo", toYear);

                    cmd.Parameters.AddWithValue("@pmonthFrom", fromMonth);
                    cmd.Parameters.AddWithValue("@pmonthTo", toMonth);
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                GridView3.DataSource = ds;
                GridView3.DataBind();
                                
            }
            GridView1.Visible = false;
            btnBack.Visible = true;

            chk_sc.Enabled = false;
            chk_st.Enabled = false;
            chk_OBC.Enabled = false;
            chk_PH.Enabled = false;
            GridView3.Caption = "<center><b> "+ name.ToUpper() + " " + " CANDIDATES COURSE CATEGORY-WISE DETAILS </b></centre>";
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
    protected void chk_sc_CheckedChanged(object sender, EventArgs e)
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        //OnSelectedIndexChanged(sender, e);
        //GridView3_SelectedIndexChanged(sender, e);

        if (chk_sc.Checked == true)
        {
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[9].Visible = true;

     
        }
        else
        {
            GridView1.Columns[3].Visible = false;
            GridView1.Columns[9].Visible = false;
        }
             
    }   
    protected void chk_st_CheckedChanged(object sender, EventArgs e)
    {

        //OnSelectedIndexChanged(sender, e);
        //GridView3_SelectedIndexChanged(sender, e);

        if (chk_st.Checked == true)
        {
            GridView1.Columns[4].Visible = true;
            GridView1.Columns[10].Visible = true;
        }
        else
        {
            GridView1.Columns[4].Visible = false;
            GridView1.Columns[10].Visible = false;
        }
    }
    protected void chk_OBC_CheckedChanged(object sender, EventArgs e)
    {

        //OnSelectedIndexChanged(sender, e);
        //GridView3_SelectedIndexChanged(sender, e);

        if (chk_OBC.Checked == true)
        {
            GridView1.Columns[5].Visible = true;
            GridView1.Columns[11].Visible = true;
        }
        else
        {
            GridView1.Columns[5].Visible = false;
            GridView1.Columns[11].Visible = false;
        }
    }
    protected void chk_PH_CheckedChanged(object sender, EventArgs e)
    {
        //OnSelectedIndexChanged(sender, e);
        //GridView3_SelectedIndexChanged(sender, e);

        if (chk_PH.Checked == true)
        {
            GridView1.Columns[6].Visible = true;
            GridView1.Columns[12].Visible = true;
        }
        else
        {
            GridView1.Columns[6].Visible = false;
            GridView1.Columns[12].Visible = false;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Dashboard_New/DashBoard.aspx");
    }
    protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
    {

        int index = GridView3.SelectedRow.RowIndex;
        int courseCategoryId = Convert.ToInt32(GridView3.DataKeys[index].Values["Course_Category_id"]);     

        int Id = Convert.ToInt32(GridView1.DataKeys[GridView1.SelectedRow.RowIndex].Values["id"]);

        hdnSelected.Value = "CATEGORY";
       // currenttxtValue.Value = "CATEGORY";
        hndCourseID.Value = courseCategoryId.ToString();


        hdnStateID.Value = Id.ToString();
        HdnfSc.Value = chk_sc.Checked ? "Y" : "";
        HdnfST.Value = chk_st.Checked ? "Y" : "";
        HdnfOBC.Value = chk_OBC.Checked ? "Y" : "";
        HdnfPH.Value = chk_PH.Checked ? "Y" : "";

        Int32? gender = Convert.ToInt32(ddlGender.SelectedValue);
        Int32? fromYear = Convert.ToInt32(ddlFromYear.SelectedValue);
        Int32? toYear = Convert.ToInt32(ddlToYear.SelectedValue);
        Int32? fromMonth = Convert.ToInt32(ddlFromMonth.SelectedValue);
        Int32? toMonth = Convert.ToInt32(ddlToMonth.SelectedValue);



        char? pSC = !string.IsNullOrEmpty(HdnfSc.Value) ? Convert.ToChar(HdnfSc.Value) : (char?)null;
        char? pST = !string.IsNullOrEmpty(HdnfST.Value) ? Convert.ToChar(HdnfST.Value) : (char?)null;
        char? pOBC = !string.IsNullOrEmpty(HdnfOBC.Value) ? Convert.ToChar(HdnfOBC.Value) : (char?)null;
        char? pPH = !string.IsNullOrEmpty(HdnfPH.Value) ? Convert.ToChar(HdnfPH.Value) : (char?)null;

        string name = GridView1.SelectedRow.Cells[1].Text;
        string country = GridView1.SelectedRow.Cells[2].Text;

        string grandTotal = GridView1.SelectedRow.Cells[1].Text;
        if (grandTotal == "GRAND TOTAL")
        {
            GridView1.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#F82605");
            GridView1.SelectedRow.ForeColor = System.Drawing.Color.Beige;
            GridView1.SelectedRow.Font.Bold = true; 
        }
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {            
            using (SqlCommand cmd = new SqlCommand("[getStateWiseCourseCategoryWiseNSQFCounts_grid]", con))
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pStateID", Id);
                cmd.Parameters.AddWithValue("@pCourseCategoryID", courseCategoryId);
                cmd.Parameters.AddWithValue("@pSC", pSC);
                cmd.Parameters.AddWithValue("@pST", pST);
                cmd.Parameters.AddWithValue("@pOBC", pOBC);
                cmd.Parameters.AddWithValue("@pPH", pPH);
                if (gender == 1 || gender == 2 || gender == 3)
                {
                    cmd.Parameters.AddWithValue("@pGender", gender);
                }
                if (fromYear != 0 && toYear != 0 && fromMonth != 0 && toMonth != 0)
                {
                    cmd.Parameters.AddWithValue("@pYearFrom", fromYear);
                    cmd.Parameters.AddWithValue("@pYearTo", toYear);
                    cmd.Parameters.AddWithValue("@pmonthFrom", fromMonth);
                    cmd.Parameters.AddWithValue("@pmonthTo", toMonth);
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                GridView2.DataSource = ds;
                GridView2.DataBind();


            }

            GridView1.Visible = false;
            GridView3.Visible = false;
            btnBack.Visible = true;
        
            chk_sc.Enabled = false;
            chk_st.Enabled = false;
            chk_OBC.Enabled = false;
            chk_PH.Enabled = false;
            GridView2.Caption = "<center><b> " + name.ToUpper() + " " + " CANDIDATES COURSE-WISE DETAILS</b></centre>";
        }


    }
    protected void btnApplyFilters_Click(object sender, EventArgs e)
    {
        if (hdnSelected.Value != "STATE" && hdnSelected.Value != "CATEGORY")
        {
            refreshdata();
        }
         if (hdnSelected.Value == "STATE")
        {
            OnSelectedIndexChanged(sender, e);
        }
         if (hdnSelected.Value == "CATEGORY")
         {
             GridView3_SelectedIndexChanged(sender, e);
         }
    }  
}