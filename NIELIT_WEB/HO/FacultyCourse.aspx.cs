using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Globalization;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


public partial class HO_FacultyCourse : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;


    protected void Page_Load(object sender, EventArgs e)
    { 
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}

            // loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            

            if (!Page.IsPostBack)
            {
                
                BindGridView();
                BindFilterCourse();
                BindFaculty();
                BindCourse();
                BindModule(0);
                
                BreadCrumb3.AddNewBreadCrumbItem(new BreadCrumbItem("Faculty Course Data", "HO/FacultyCourse.aspx", ""));

                

                if (!string.IsNullOrWhiteSpace(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    lblHeading.Text = "Faculty Course";
                }
             

                if (Session["FlashMessage"] != null)
                {
                    ShowAlert(Session["FlashMessage"].ToString());
                    Session.Remove("FlashMessage"); // clear after showing
                }
            }

            BreadCrumb3.Render();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    private void BindFaculty()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("GetFacultyList_ByCentres", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EntityID", entityID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ddlFaculty.DataSource = dr;
                ddlFaculty.DataTextField = "FacultyDisplay";
                ddlFaculty.DataValueField = "FacultyId";
                ddlFaculty.DataBind();
            }
            ddlFaculty.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert("Error while binding faculty: " + ex.Message, true);
        }
    }


    private void BindCourse()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("[GetCourses_ByCentre]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@EntityID", entityID);

                con.Open();
                ddlCourse.DataSource = cmd.ExecuteReader();
                ddlCourse.DataTextField = "CourseName";
                ddlCourse.DataValueField = "CourseId";
                ddlCourse.DataBind();
            }
            ddlCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert("Error while binding course: " + ex.Message, true);
        }
    }


    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            long courseId = Convert.ToInt64(ddlCourse.SelectedValue);
            BindModule(courseId);
        }
        catch (Exception ex)
        {
            ShowAlert("Error while changing course selection: " + ex.Message, true);
        }
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Set serial number considering paging
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) +
                    (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

                // Action image binding
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                if (imgAction != null)
                    imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                // Checkbox binding
                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                if (chk != null)
                    chk.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{

            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            //BreadCrumb2.Render();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Faculty Course";
            //Updating Breadcrumb
            //Educa1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));

            BreadCrumb3.AddNewBreadCrumbItem(new BreadCrumbItem("New Faculty course", "#", ""));
        }

        else
        {
            Response.Redirect("FacultyCourse.aspx", true);
        }
    }

    protected void  BindGridView()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"] != null ? ViewState["SortOrder"].ToString() : "ASC";
            string sortField = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "ID";

            // default sorting safeguard
            if (string.IsNullOrEmpty(sortField)) sortField = "ID";
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "ASC";

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("Faculty_Course_display", con)) // Using stored procedure
            {
                cmd.CommandType = CommandType.StoredProcedure;

                //Pass EntityID from session (matches procedure parameter)
                cmd.Parameters.AddWithValue("@EntityID", Convert.ToInt64(Session["EntityID"]));

                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }


            if (!string.IsNullOrEmpty(ddlFilterCourse.SelectedValue) && ddlFilterCourse.SelectedValue != "0")

            {
                var dvCentre = dt.DefaultView;

                // Convert SelectedValue to long (BIGINT in SQL maps to C# long)
                long courseId = Convert.ToInt64(ddlFilterCourse.SelectedValue);
                dvCentre.RowFilter = "CourseId = " + courseId;

                dt = dvCentre.ToTable();
            }

            // Apply search filter in memory
            if (!string.IsNullOrEmpty(searchString))
            {
                var dv = dt.DefaultView;
                dv.RowFilter = "FacultyDisplay LIKE '%" + searchString + "%'";
                dt = dv.ToTable();
            }

            // Apply sorting in memory
            string sortExpression = sortField + " " + sortOrder;
            dt.DefaultView.Sort = sortExpression;
            dt = dt.DefaultView.ToTable();

            // Add Serial Number Column if missing
            if (!dt.Columns.Contains("SNo"))
            {
                dt.Columns.Add("SNo", typeof(int));
            }

            int pageSize = PagingBar1.CurrentPageSize;
            int pageIndex = PagingBar1.CurrentPageIndex;

            DataTable dtPaged;

            if (pageSize == 0) // "All" selected
            {
                dtPaged = dt.Copy(); // take all rows
                for (int i = 0; i < dtPaged.Rows.Count; i++)
                {
                    dtPaged.Rows[i]["SNo"] = i + 1;
                }
            }
            else
            {
                int startRow = pageIndex * pageSize;
                int endRow = startRow + pageSize;

                dtPaged = dt.Clone();
                for (int i = startRow; i < endRow && i < dt.Rows.Count; i++)
                {
                    DataRow newRow = dtPaged.NewRow();
                    newRow.ItemArray = dt.Rows[i].ItemArray;
                    newRow["SNo"] = i + 1;
                    dtPaged.Rows.Add(newRow);
                }
            }
            // Add Serial Number Column (#) dynamically
            //if (!dt.Columns.Contains("SNo"))
            //{
            //    dt.Columns.Add("SNo", typeof(int));
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["SNo"] = i + 1;
            //}

            PagingBar1.Bind(dt, ref gvMain);
            // Bind to GridView
            gvMain.DataSource = dtPaged;
            gvMain.DataBind();

            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Edit Faculty Course Mapping";

            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("Get_FacultyCourse_for_update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(Request.QueryString["Key"]));

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    // Preselect Faculty
                    if (ddlFaculty.Items.FindByValue(dr["FacultyId"].ToString()) != null)
                        ddlFaculty.SelectedValue = dr["FacultyId"].ToString();

                    // Preselect Course
                    if
                       (ddlCourse.Items.FindByValue(dr["CourseId"].ToString()) != null)
                    {
                        ddlCourse.SelectedValue = dr["CourseId"].ToString();
                        BindModule(Convert.ToInt32(dr["CourseId"]));
                    }

                        // Preselect Module
                        if (ddlModule.Items.FindByValue(dr["ModuleId"].ToString()) != null)
                        ddlModule.SelectedValue = dr["ModuleId"].ToString();

                    // Dates
                    txtEffectiveFrom.Text = Convert.ToDateTime(dr["EffectiveFrom"]).ToString("yyyy-MM-dd");
                    txtEffectiveTo.Text = Convert.ToDateTime(dr["EffectiveTo"]).ToString("yyyy-MM-dd");
                }

                // Breadcrumb should display FacultyName

                BreadCrumb3.AddNewBreadCrumbItem(
      new BreadCrumbItem(ddlFaculty.SelectedItem.Text, "#", "")
  );
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlFilterCourse.SelectedValue = "0";  // Reset course
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();   // Refresh after reset
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;

            // Default to ASC if SortOrder is null
            if (ViewState["SortOrder"] == null || ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";

            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);


        }
    }
    private void BindFilterCourse()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))


            using (SqlCommand cmd = new SqlCommand("GetCourses_ByCentre", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ddlFilterCourse.DataSource = dr;
                ddlFilterCourse.DataTextField = "CourseName";
                ddlFilterCourse.DataValueField = "CourseId";
                ddlFilterCourse.DataBind();
                ddlFilterCourse.Items.Insert(0, new ListItem("-- All Courses --", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading courses: " + ex.Message, true);
        }
    }

    private void BindModule(long courseId)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("GetModules_ByCourse", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                con.Open();
                ddlModule.DataSource = cmd.ExecuteReader();
                ddlModule.DataTextField = "Name";
                ddlModule.DataValueField = "ID";
                ddlModule.DataBind();
            }
            ddlModule.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert("Error while binding modules: " + ex.Message, true);
        }
    }

    protected void ddlFilterCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            long courseId = Convert.ToInt64(ddlFilterCourse.SelectedValue);
            BindModule(courseId);  // Assuming you want to refresh modules when course changes
            BindGridView();        // Rebind grid data based on new filter
        }
        catch (Exception ex)
        {
            ShowAlert("Error: " + ex.Message, true);
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            Int64 enterBy = Convert.ToInt64(Session["UserID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("Fill_Faculty_Course", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (string.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 10).Value = "Insert";
                }
                else
                {
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 10).Value = "Update";
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["Key"]);
                }
                cmd.Parameters.AddWithValue("@FacultyId", ddlFaculty.SelectedValue);
                cmd.Parameters.AddWithValue("@CourseId", ddlCourse.SelectedValue);
                cmd.Parameters.AddWithValue("@ModuleId", ddlModule.SelectedValue);
                cmd.Parameters.AddWithValue("@EffectiveFrom", txtEffectiveFrom.Text);
                cmd.Parameters.AddWithValue("@EffectiveTo", txtEffectiveTo.Text);
                cmd.Parameters.Add("@enterBy", SqlDbType.BigInt).Value = enterBy;

                con.Open();
              //int affected = 
                    cmd.ExecuteNonQuery();

                //if (affected <= 0)
                //{
                //    lblErrorMsg.Visible = true;
                //    lblErrorMsg.Text = "No rows were inserted/updated. Please verify the data and try again.";
                //    return;
                //}
            }

            string msg = string.IsNullOrEmpty(Request.QueryString["Key"])
                ? "New Faculty-Course record saved."
                : "Faculty-Course record updated.";

            Session["FlashMessage"] = msg;
            Response.Redirect("FacultyCourse.aspx", true);
            

        }
        catch (Exception ex)
        {
            ShowAlert("Error while saving: " + ex.Message, true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("FacultyCourse.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert("Error while cancelling: " + ex.Message, true);
        }
    }

}

