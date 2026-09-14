using EConnect;
using EConnect.DAL;
using EConnect.URM;

using System;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using static System.Runtime.CompilerServices.RuntimeHelpers;

public partial class HO_FacultyMas : BasePage
{
    string strMessage = string.Empty;
    int currentRoleId = 0;
    Int32 loginUserNo = 0;
    UserType loginUserType;
    Int64 entityID = 0;

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

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
           

            if (!Page.IsPostBack)
            {
                
                BindCentres();
                BindStates();
                BindDistricts(0);
                BindGridView();
                BindFilterCentres();
                BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem("Faculty Data", "HO/FacultyMas.aspx", ""));
                // BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem()


                
                if (!string.IsNullOrWhiteSpace(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    lblHeading.Text = "Faculty Data";
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());

                if (Session["FlashMessage"] != null)
                {
                    ShowAlert(Session["FlashMessage"].ToString());
                    Session.Remove("FlashMessage"); // clear after showing
                }

            }
            BreadCrumb2.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
            lblHeading.Text = "Edit Faculty Data";

            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("Get_FacultyById_for_update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
               
                cmd.Parameters.AddWithValue("@FacultyID", Convert.ToInt32(Request.QueryString["Key"]));
               

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (ddlCentre.Items.FindByValue(dr["CentreID"].ToString()) != null)
                        ddlCentre.SelectedValue = dr["CentreID"].ToString();

                    txtFacultyName.Text = dr["FacultyName"].ToString();
                    ddlGender.Text = dr["Gender"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    txtMobile.Text = dr["Mobile"].ToString();
                    txtAddress1.Text = dr["Address1"].ToString();
                    txtAddress2.Text = dr["Address2"].ToString();
                    if (ddlState.Items.FindByValue(dr["FacultyState"].ToString()) != null)
                    {
                        ddlState.SelectedValue = dr["FacultyState"].ToString();
                        BindDistricts(Convert.ToInt32(dr["FacultyState"]));
                    }
                    if (ddlDistrict.Items.FindByValue(dr["FacultyDistrict"].ToString()) != null)
                        ddlDistrict.SelectedValue = dr["FacultyDistrict"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtPin.Text = dr["PinCode"].ToString();
                    if (ddlFacultyType.Items.FindByValue(dr["FacultyType"].ToString()) != null)
                        ddlFacultyType.SelectedValue = dr["FacultyType"].ToString();
                    // ddlFacultyType.SelectedValue = dr["FacultyType"].ToString();
                    txtPrevCompany.Text = dr["PreviousCompany"].ToString();
                    txtExperience.Text = dr["ExperienceYears"].ToString();
                    txtQualification.Text = dr["Qualification"].ToString();
                    txtSkills.Text = dr["Skills"].ToString();
                    txtFacultyCode.Text = dr["FacultyCode"].ToString();
                    txtEffectiveFrom.Text = dr["EffectiveFrom"].ToString();
                    txtEffectiveTo.Text = dr["EffectiveTo"].ToString();
                }
                BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem(txtFacultyName.Text, "#", ""));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

   
    protected void BindGridView()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"] != null ? ViewState["SortOrder"].ToString() : "ASC";
            string sortField = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "ID";

            if (string.IsNullOrEmpty(sortField)) sortField = "ID";
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "ASC";

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("Faculty_data_display", con))
            {
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EntityID", entityID);
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            // Apply Centre filter
            if (!string.IsNullOrEmpty(ddlFilterCentre.SelectedValue) && ddlFilterCentre.SelectedValue != "0")
            {
                var dvCentre = dt.DefaultView;
                long centreId = Convert.ToInt64(ddlFilterCentre.SelectedValue);
                dvCentre.RowFilter = "CentreID = " + centreId;
                dt = dvCentre.ToTable();
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                var dv = dt.DefaultView;
                dv.RowFilter = "facultyName LIKE '%" + searchString + "%' " +
                               "OR facultyEmail LIKE '%" + searchString + "%' " +
                               "OR FacultyHighestQualification LIKE '%" + searchString + "%'";
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

            // Bind only paged data
            PagingBar1.Bind(dt, ref gvMain);  // Pass full data for pager counts
            gvMain.DataSource = dtPaged;      // Show only page slice
            gvMain.DataBind();

            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{

            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            BreadCrumb2.Render();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Faculty Form";
            //Updating Breadcrumb
            //Educa1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
           
            BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem("New Faculty Entry", "#", ""));
        }
        //else if (btnMode.ViewMode == ToggleView.Mode.List)  
        //{
        //    btnMode.ViewMode = ToggleView.Mode.List;
        //    mltvTab.ActiveViewIndex = 1;
        //    pnlFilter.Visible = false;
        //    ucSearchBar.Visible = false;

        //    lblHeading.Text = "Edit Faculty Data";

        //    // Breadcrumb for EDIT Faculty
        //    BreadCrumb2.AddNewBreadCrumbItem(
        //        new BreadCrumbItem("Edit Faculty Record", "#", "")
        //    );
        //}
        else
        {
            Response.Redirect("FacultyMas.aspx", true);
        }
    }

    protected void ddlFilterCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        PagingBar1.CurrentPageIndex = 0;
        gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        BindGridView();
    }

    private bool IsValidForm()
    {
        if (ddlCentre.SelectedIndex <= 0)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Centre Name is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtFacultyName.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Faculty Name is required.";
            return false;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(txtFacultyName.Text, @"^[A-Za-z\s]+$"))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Faculty Name must contain only letters.";
            return false;
        }
        if (ddlGender.SelectedIndex <= 0)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Gender is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Email is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtMobile.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Mobile Number is required.";
            return false;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(txtPin.Text, @"^\d{6}$"))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Pin Code must be exactly 6 digits.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtAddress1.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Address Line-1 is required.";
            return false;
        }
        if (ddlState.SelectedIndex <= 0)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "State is required.";
            return false;
        }
        if (ddlDistrict.SelectedIndex <= 0)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "District is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtCity.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "City Name is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtPin.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Pin Code is required.";
            return false;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(txtPin.Text, @"^\d{6}$"))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Pin Code must be exactly 6 digits.";
            return false;
        }
        if (ddlFacultyType.SelectedIndex <= 0)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Faculty Type is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtExperience.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Experience is required.";
            return false;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(txtExperience.Text, @"^\d{1,2}$"))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Experience must be a Valid Number.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtEffectiveFrom.Text))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Effective From Date is required.";
            return false;
        }
        DateTime effDate;
        if (!DateTime.TryParse(txtEffectiveFrom.Text, out effDate))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Effective From must be a valid date.";
            return false;
        }

        lblErrorMsg.Text = ""; // clear previous errors
        lblErrorMsg.Visible = false;
        return true;
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidForm())
                return; // stop if validation fails

            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            Int64 enterBy = Convert.ToInt64(Session["UserID"]);


            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("Fill_Faculty_Mas", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Insert or Update mode
                if (string.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 10).Value = "Insert";
                }
                else
                {
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 10).Value = "Update";
                    cmd.Parameters.Add("@FacultyID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["Key"]);
                }

                
                cmd.Parameters.Add("@CentreID", SqlDbType.Int).Value = Convert.ToInt32(ddlCentre.SelectedValue);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = txtFacultyName.Text.Trim();
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 10).Value = ddlGender.SelectedValue;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txtEmail.Text.Trim();
                cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar, 20).Value = txtMobile.Text.Trim();
                cmd.Parameters.Add("@AddressLine1", SqlDbType.NVarChar, 200).Value = txtAddress1.Text.Trim();
                cmd.Parameters.Add("@AddressLine2", SqlDbType.NVarChar, 200).Value = txtAddress2.Text.Trim();
                cmd.Parameters.Add("@FacultyState", SqlDbType.Int).Value = Convert.ToInt32(ddlState.SelectedValue);
                cmd.Parameters.Add("@FacultyDistrict", SqlDbType.Int).Value = Convert.ToInt32(ddlDistrict.SelectedValue);
                cmd.Parameters.Add("@CityName", SqlDbType.NVarChar, 100).Value = txtCity.Text.Trim();
                cmd.Parameters.Add("@PinCode", SqlDbType.NVarChar, 20).Value = txtPin.Text.Trim();
                cmd.Parameters.Add("@FacultyType", SqlDbType.NVarChar, 50).Value = ddlFacultyType.SelectedValue;
                cmd.Parameters.Add("@PreviousCompany", SqlDbType.NVarChar, 100).Value = txtPrevCompany.Text.Trim();
                cmd.Parameters.Add("@ExperienceYears", SqlDbType.NVarChar, 50).Value = txtExperience.Text.Trim();
                cmd.Parameters.Add("@HighestQualification", SqlDbType.NVarChar, 100).Value = txtQualification.Text.Trim();
                cmd.Parameters.Add("@Skills", SqlDbType.NVarChar, 500).Value = txtSkills.Text.Trim();
                cmd.Parameters.Add("@FacultyCode", SqlDbType.NVarChar, 50).Value = txtFacultyCode.Text.Trim();
                cmd.Parameters.Add("@enterBy", SqlDbType.BigInt).Value = enterBy;


                // EffectiveFrom
                DateTime effFrom;
                if (DateTime.TryParse(txtEffectiveFrom.Text.Trim(), out effFrom))
                    cmd.Parameters.Add("@EffectiveFrom", SqlDbType.Date).Value = effFrom;
                else
                    cmd.Parameters.Add("@EffectiveFrom", SqlDbType.Date).Value = DBNull.Value;

                // EffectiveTo
                if (string.IsNullOrEmpty(txtEffectiveTo.Text.Trim()))
                {
                    cmd.Parameters.Add("@EffectiveTo", SqlDbType.Date).Value = DBNull.Value;
                }
                else
                {
                    DateTime effTo;
                    if (DateTime.TryParse(txtEffectiveTo.Text.Trim(), out effTo))
                        cmd.Parameters.Add("@EffectiveTo", SqlDbType.Date).Value = effTo;
                    else
                        cmd.Parameters.Add("@EffectiveTo", SqlDbType.Date).Value = DBNull.Value;
                }

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
                ? "New faculty record saved."
                : "Faculty record updated.";

            Session["FlashMessage"] = msg;
            Response.Redirect("FacultyMas.aspx", true);

            //Response.Redirect("FacultyDataEntry.aspx?msg=" + Server.UrlEncode(msg), true);
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
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

    

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlFilterCentre.SelectedValue = "0";  // Reset centre
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();   // Refresh after reset
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    //protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    try
    //    {
    //        ViewState["SortField"] = e.SortExpression;
    //        if (ViewState["SortOrder"].ToString() == "DESC")
    //            ViewState["SortOrder"] = "ASC";
    //        else
    //            ViewState["SortOrder"] = "DESC";
    //        BindGridView();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

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


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("FacultyMas.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    
    private void BindCentres()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("fillCentresforFaculty", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EntityID", entityID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ddlCentre.DataSource = dr;
                ddlCentre.DataTextField = "centre";  
                ddlCentre.DataValueField = "ID";    
                ddlCentre.DataBind();
                ddlCentre.Items.Insert(0, new ListItem("-- Select Centre --", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading centres: " + ex.Message, true);
        }
    }

    private void BindFilterCentres()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))


            using (SqlCommand cmd = new SqlCommand("fillCentresforFaculty", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EntityID", entityID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ddlFilterCentre.DataSource = dr;
                ddlFilterCentre.DataTextField = "centre";  
                ddlFilterCentre.DataValueField = "ID";
                ddlFilterCentre.DataBind();
                ddlFilterCentre.Items.Insert(0, new ListItem("-- All Centres --", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading centres: " + ex.Message, true);
        }
    }


    private void BindStates()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("fillStatesforFaculty", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ddlState.DataSource = dr;
                ddlState.DataTextField = "Name";
                ddlState.DataValueField = "ID";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("-- Select State --", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading states: " + ex.Message, true);
        }

    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        int stateID = Convert.ToInt32(ddlState.SelectedValue);
        BindDistricts(stateID);
    }


    private void BindDistricts(int stateID)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("StatesPerDistrict", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StateID", stateID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlDistrict.DataSource = dr;
                ddlDistrict.DataTextField = "Name";
                ddlDistrict.DataValueField = "ID";
                ddlDistrict.DataBind();

                ddlDistrict.Items.Insert(0, new ListItem("-- Select District --", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading districts: " + ex.Message, true);
        }
    }


}