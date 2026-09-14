
using DocumentFormat.OpenXml.Spreadsheet;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_TargetDetails : BasePage
{
    String strMessage = string.Empty;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.QueryString.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {

            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");

            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                    BindGridView();
                    FillFilterCourses();
                    FillFilterCentres();
                    FillFilterFrequency();

                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Target Details", "Admin/TargetDetails.aspx", ""));
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());

                fillTargets();
                fillProjects();
                fillUnits();

                ddlproject.ToolTip = ddltarget.SelectedItem.Text;
            }

            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillFilterCourses()
    {
        try
        {
            ListItem lst1 = new ListItem("-Select One-", "0");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("getcoursesforfilter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        ddlcoursefilter.DataSource = ds.Tables[0];
                        ddlcoursefilter.DataTextField = "CourseName";
                        ddlcoursefilter.DataValueField = "ID";
                        ddlcoursefilter.DataBind();
                        ddlcoursefilter.Items.Insert(0, new ListItem("--All--", "0"));
                        //default is selected on adding
                        ddlcoursefilter.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCentres()
    {
        try
        {
            ListItem lst1 = new ListItem("--All--", "0");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                conn.Open();


                string sql = @"select distinct nc.name as CentreName, nc.Id as CentreID from projectMainCentre p
	                           inner join NielitCentres nc on p.centreID = nc.id";

                using (var cmd = new SqlCommand(sql, conn))
                {

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        ddlcentrefilter.DataSource = ds.Tables[0];
                        ddlcentrefilter.DataTextField = "CentreName";
                        ddlcentrefilter.DataValueField = "CentreID";

                        ddlcentrefilter.DataBind();

                        ddlcentrefilter.Items.Insert(0, lst1);
                        //default is selected on adding
                        ddlcentrefilter.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterFrequency()
    {
        try
        {
            ListItem lst1 = new ListItem("-Select One-", "0");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                conn.Open();

                string sql = @"select * from nielitmis.dbo.frequency";

                using (var cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.Clear();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        ddlfreqfilter.DataSource = ds.Tables[0];
                        ddlfreqfilter.DataTextField = "Name";
                        ddlfreqfilter.DataValueField = "ID";

                        ddlfreqfilter.DataBind();

                        ddlfreqfilter.Items.Insert(0, new ListItem("--All--", "0"));

                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillCourse()
    {
        try
        {
                ListItem lst1 = new ListItem("-Select One-", "0");
                string ConnectionString1 = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

                using (SqlConnection con = new SqlConnection(ConnectionString1))
                {
                    using (var command = new SqlCommand("[GetCourses_ByCentre_ByProject]", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@entityId", ddlCentre.SelectedValue);
                        command.Parameters.AddWithValue("@projectid", ddlproject.SelectedItem.Value);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, dt, lst1);

                            ddlCourse.DataTextField = "CourseName";   // e.g. centre name column
                            ddlCourse.DataValueField = "ID";
                            ddlCourse.DataBind();
                            ddlCourse.Items.Insert(0, lst1);
                        }
                    }
                }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 
    protected void fillTargets()
    {
        try
        {
            ListItem lst1 = new ListItem("-Select One-", "0");

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                string sql = @"select targetName, ID from targetMas";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();

                        da.Fill(ds);

                        // EConnect.Utils.Common.ControlUtility.BindListObject(ddltarget, ds.Tables[0], lst1);
                        ddltarget.DataSource = ds.Tables[0];
                        ddltarget.DataTextField = "targetName";
                        ddltarget.DataValueField = "ID";
                        ddltarget.DataBind();
                        ddltarget.Items.Insert(0, lst1);

                    }
                }

            }
        }
        catch (Exception ex)
        {

        }
    }   
    protected void ddlproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCentres();
        //  fillCourse();
    }
    protected void ddlcentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCourse();
    }
    protected void ddltarget_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string connstring = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

            string sql = "SELECT fileUploadRequired FROM targetMas WHERE ID = @targetID";

            using (SqlConnection con = new SqlConnection(connstring))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@targetID", ddltarget.SelectedValue);
                con.Open();

                object result = cmd.ExecuteScalar();

                int flag = (result != DBNull.Value && result != null)
                            ? Convert.ToInt32(result)
                            : 0;

                bool show = flag == 1;

                // new case
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    //edit case
                    lblfluploaded.Visible = show;
                }
                else
                {
                    // new case
                    flupload.Visible = show;
                    fluploadlbl.Visible = show;
                }




            }
        }
        catch
        {
            ShowAlert("Error loading file upload option.");
            flupload.Visible = false;
            fluploadlbl.Visible = false;
        }
    }
    protected void fillProjects()
    {
        ListItem lst1 = new ListItem("-Select One-", "0");
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("fillprojectsforcentre", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@entityid", entityID);

                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlproject, dt, lst1);
                }
            }

        }

    }   
    protected void fillCentres()
    {
        ListItem lst1 = new ListItem("-Select One-", "0");
        // institute login , they will only see the centre name
        using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand("[fillCentresByProject]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectid", ddlproject.SelectedValue);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                // EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentre, dt, lst1);
                ddlCentre.DataSource = dt;
                ddlCentre.DataTextField = "centre";   // e.g. centre name column
                ddlCentre.DataValueField = "ID";
                ddlCentre.DataBind();
                ddlCentre.Items.Insert(0, lst1);
                //ddlCentre.SelectedIndex = 0;
                //ddlCentre.Enabled = false;

            }

        }
    }   
    protected void fillUnits()
    {
        try
        {
            ListItem lst1 = new ListItem("-Select One-", "0");
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                string sql = @"select unitName, ID from units order by id";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        ddlunit.DataSource = ds.Tables[0];
                        ddlunit.DataTextField = "unitName";
                        ddlunit.DataValueField = "ID";
                        ddlunit.DataBind();
                        ddlunit.Items.Insert(0, lst1);
                        //ddlunit.SelectedIndex = 0;
                    }
                }

            }
        }
        catch (Exception ex)
        {

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
            lblHeading.Text = "Edit Target";

            int TargetQID = Convert.ToInt32(Request.QueryString["Key"]);
            string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_GetTargetDetailsById", con))
            {  
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TargetQID", TargetQID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (dt.Rows.Count == 0)
                throw new Exception("Target record not found.");

            DataRow dr = dt.Rows[0];

            // return if effective to date is present.
            if (dr["EffectiveToDate"] != DBNull.Value)
            {
                string strMessage = "Effective To Date is Present. No modifications are allowed.";
                Response.Redirect("TargetDetails.aspx?msg=" + strMessage);
            }


            // Fill dropdowns first
            fillTargets();
            fillProjects();
            fillUnits();

            ddltarget.SelectedValue = dr["targetID"].ToString();
            ddltarget_SelectedIndexChanged(null, null);

            //flupload.Visible = false;
            //lblfluploaded.Visible = true;
            
                //fileupload.Visible = false;
                //lblfluploaded.Visible = true;
                //lblfltext.Visible = false;
           
            ddlunit.SelectedValue = dr["valueUnitId"].ToString();
            ddlproject.SelectedValue = dr["projectId"].ToString();
            ddlproject_SelectedIndexChanged(null, null);
            ddlCentre.SelectedValue = dr["centreID"].ToString();
            ddlcentre_SelectedIndexChanged(null, null);
            ddlCourse.SelectedValue = dr["courseID"].ToString();


            // Fill textboxes
            txteffrom.Text = Convert.ToDateTime(dr["EffectiveFromDate"]).ToString("dd-MMM-yyyy");
            txtvaluebudget.Text = dr["value"].ToString();

           
            //txteffto.Text = Convert.ToDateTime(dr["EffectiveToDate"]).ToString("yyyy-MM-dd");
           

            // UPDATE mode
            ddltarget.Enabled = false;
            ddlproject.Enabled = false;
            ddlCentre.Enabled = false;
            ddlCourse.Enabled = false;
            txteffrom.Enabled = false;
            txtvaluebudget.Enabled = false;
            ddlunit.Enabled = false;

            // Breadcrumb
            string pageTitle = dr["targetName"].ToString();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(pageTitle, "#", ""));

            // ViewState["LastModifiedOn"] = DateTime.Now;
        }
        catch (Exception ex)
        {
            // Log or display error appropriately
            throw ex;
        }
    }
    protected void BindGridView()
    {
        try
        {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString() ?? "ASC";
                string sortField = ViewState["SortField"].ToString() ?? "ID";
                int Course_Id_filter = 0;
                int centre_id_filter = 0;

                DataTable dt = new DataTable();

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("getTargets", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // cmd.Parameters.AddWithValue("@entityid", entityID);  // if required

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                //  Convert DataTable to LINQ queryable form
                var targetRecords = dt.AsEnumerable();

                // Apply Search 
                if (!string.IsNullOrEmpty(searchString))
                {
                    targetRecords = targetRecords.Where(row =>
                        row["targetName"].ToString().ToUpper().Contains(searchString) ||
                        row["ID"].ToString().ToUpper().Contains(searchString) ||
                        row["projectName"].ToString().ToUpper().Contains(searchString) ||
                        row["CourseName"].ToString().ToUpper().Contains(searchString)
                        );
                }

                // Apply Course Filter
                if (ddlcoursefilter.SelectedValue != "0")
                    Course_Id_filter = Convert.ToInt32(ddlcoursefilter.SelectedValue);

                if (Course_Id_filter != 0)
                {
                    targetRecords = targetRecords.Where(row =>
                        Convert.ToInt32(row["courseID"]) == Course_Id_filter);
                }

                // Centre namne filter

                if (ddlcentrefilter.SelectedValue != "0")
                    centre_id_filter = Convert.ToInt32(ddlcentrefilter.SelectedValue);

                if (centre_id_filter != 0)
                {
                    targetRecords = targetRecords.Where(row =>
                        Convert.ToInt32(row["CentreID"]) == centre_id_filter);
                }

                // Frequency Filter
                if(ddlfreqfilter.SelectedValue != "0")
                    targetRecords = targetRecords.Where(row =>
                       Convert.ToString(row["frequency"]) == Convert.ToString(ddlfreqfilter.SelectedItem.Text.Trim()));

                //Effective From Date Filter
                if (!string.IsNullOrWhiteSpace(txtfilterefffrom.Text))
                {
                    DateTime filterDate, rowDate;
                    if (DateTime.TryParse(txtfilterefffrom.Text, out filterDate))
                    {
                        targetRecords = targetRecords.Where(row =>
                        {
                            if (row["efd"] == null || row["efd"] == DBNull.Value)
                                return false;

                            return DateTime.TryParse(row["efd"].ToString(), out rowDate) &&
                                   rowDate.Date >= filterDate.Date;
                        });
                    }
                }

                // Effective To Date Filter
                if (!string.IsNullOrWhiteSpace(txtfiltereffto.Text))
                {
                    DateTime filterDate, rowDate;
                    if (DateTime.TryParse(txtfiltereffto.Text, out filterDate))
                    {
                        targetRecords = targetRecords.Where(row =>
                        {
                            if (row["etd"] == null || row["etd"] == DBNull.Value)
                                return false;

                            return DateTime.TryParse(row["etd"].ToString(), out rowDate) &&
                                   rowDate.Date <= filterDate.Date;
                        });
                    }
                }


                //if (!string.IsNullOrWhiteSpace(txteffrom.Text) && !string.IsNullOrWhiteSpace(txteffto.Text))
                //{
                //    DateTime effFrom, effTo;
                //    if (DateTime.TryParse(txteffrom.Text, out effFrom) && DateTime.TryParse(txteffto.Text, out effTo))
                //    {
                //        if (effFrom > effTo)
                //        {
                //            lblerror.Visible = true;
                //            lblerror.Text = "Effective From date must be less than or equal to Effective To date.";
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        lblerror.Visible = true;
                //        lblerror.Text = "Enter valid dates in both Effective From and Effective To fields.";
                //        return false;
                //    }
                //}

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    // Sorting Logic
                    switch (sortField)
                    {
                        case "ID":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => Convert.ToInt32(r["ID"]))
                                : targetRecords.OrderBy(r => Convert.ToInt32(r["ID"]));
                            break;

                        case "Name":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => r["targetName"].ToString())
                                : targetRecords.OrderBy(r => r["targetName"].ToString());
                            break;
                                
                        case "Frequency":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => r["frequency"].ToString())
                                : targetRecords.OrderBy(r => r["frequency"].ToString());
                            break;

                        case "projectName":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => r["projectName"].ToString())
                                : targetRecords.OrderBy(r => r["projectName"].ToString());
                            break;

                        case "CourseName":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => r["CourseName"].ToString())
                                : targetRecords.OrderBy(r => r["CourseName"].ToString());
                            break;

                        case "CentreName":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => r["CentreName"].ToString())
                                : targetRecords.OrderBy(r => r["CentreName"].ToString());
                            break;

                        case "value":
                            targetRecords = (sortOrder == "DESC")
                                ? targetRecords.OrderByDescending(r => r["value"].ToString())
                                : targetRecords.OrderBy(r => r["value"].ToString());
                            break;

                        default:
                            targetRecords = targetRecords.OrderBy(r => Convert.ToInt32(r["ID"]));
                            break;
                    }
                }

                //Convert filtered records back to DataTable
                DataTable targetRecords_dt = (targetRecords.Any()) ? targetRecords.CopyToDataTable() : dt.Clone();

                // Bind to GridView
                PagingBar1.Bind(targetRecords_dt, ref gvMain);

                uPnlGrid.Update();
                uPnlNavigation.Update();
            
        }
        catch (Exception ex)
        {
            throw; 
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //added by amit
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.",true);
            //    return;
            //}
            //FillQualificationLevel();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Add New Target";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Add New Target", "#", ""));
        }
        else
        {
            Response.Redirect("TargetDetails.aspx", true);
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
    private bool ValidateDropdown(DropDownList ddl, string fieldName)
    {
        if (!isSelected(ddl))
        {
            lblerror.Text = " Please select " + fieldName;
            lblerror.Visible = true;
            return false;
        }
        return true;
    }
    protected bool isValidForm()
    {

        if (!ValidateDropdown(ddltarget, "Target")) return false;
        if (!ValidateDropdown(ddlproject, "Project")) return false;
        if (!ValidateDropdown(ddlCentre, "Centre")) return false;
        if (!ValidateDropdown(ddlCourse, "Course")) return false;


        if (string.IsNullOrWhiteSpace(txtvaluebudget.Text))
        {
            lblerror.Visible = true;
            lblerror.Text = lblvalue.Text + " cannot be left blank.";
            return false;
        }

        Decimal value;
     
        if (!Decimal.TryParse(txtvaluebudget.Text, out value))
        {
            lblerror.Visible = true;
            lblerror.Text = lblvalue.Text + " must be a valid number.";
            return false;
        }
        const decimal MAX_INTEGER_VALUE = 100000000M; 
        if (value >= MAX_INTEGER_VALUE)
        {
            lblerror.Visible = true;
            lblerror.Text = lblvalue.Text + " exceeds the maximum allowed value (Max 8 whole digits).";
            return false;
        }


        if (!ValidateDropdown(ddlunit, "Unit")) return false;

        if (string.IsNullOrWhiteSpace(txteffrom.Text))
        {
            lblerror.Visible = true;
            lblerror.Text = lbleffrom.Text + " cannot be left blank";
            return false;
        }


        if (flupload.Visible == true)
        {
            if (!fileupload.HasFile)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please upload the PDF file before proceeding.";
                return false;
            }

            string fileExt = Path.GetExtension(fileupload.FileName).ToLowerInvariant();

            if (fileExt != ".pdf")
            {
                lblerror.Visible = true;
                lblerror.Text = "Only PDF files are allowed. Please upload a .pdf file.";
                return false;
            }
        }

        if (!string.IsNullOrWhiteSpace(txteffrom.Text) && !string.IsNullOrWhiteSpace(txteffto.Text))
        {
            DateTime effFrom, effTo;
            if (DateTime.TryParse(txteffrom.Text, out effFrom) && DateTime.TryParse(txteffto.Text, out effTo))
            {
                if (effFrom > effTo)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Effective From date must be less than or equal to Effective To date.";
                    return false;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Enter valid dates in both Effective From and Effective To fields.";
                return false;
            }

        }
        return true;
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            if (!isValidForm())
            {
                return;
            }

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();

                Int64 ID = Convert.ToInt64(Request.QueryString["Key"]);
                Int64 targetId = Convert.ToInt64(ddltarget.SelectedValue);
                Int64 projectId = Convert.ToInt64(ddlproject.SelectedValue);
                Int64 centreId = Convert.ToInt64(ddlCentre.SelectedValue);
                Int64 courseId = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 userId = Convert.ToInt64(Session["UserID"]);

                DateTime efd = Convert.ToDateTime(txteffrom.Text);
                DateTime? etd = null;

                if (!string.IsNullOrWhiteSpace(txteffto.Text))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(txteffto.Text, out parsedDate))
                        etd = parsedDate;
                }

                Decimal value = Convert.ToDecimal(txtvaluebudget.Text);
                Int32 value_unit_id = Convert.ToInt32(ddlunit.SelectedValue);

                // file upload
                byte[] fileBytes = null;
                if (flupload.Visible)
                {
                        const long MAX_SIZE = 10 * 1024 * 1024;
                        if (fileupload.PostedFile.ContentLength > MAX_SIZE)
                        {
                            ShowAlert("File too large (max 10 MB).");
                            return;
                        }
               

                        using (var ms = new MemoryStream())
                        {
                            fileupload.PostedFile.InputStream.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                }
     
                string strMessage = string.Empty;
                string action = string.Empty;

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    // INSERT mode
                    action = "insert";
                }
                else
                {
                    // UPDATE mode
                    action = "update";
                }

                using (var command = new SqlCommand("save_update_record_targetmanagementsystem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@action_id", action);
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@targetId", targetId);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@centreId", centreId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@efd", efd);
                    command.Parameters.AddWithValue("@etd", etd);
                    command.Parameters.AddWithValue("@userid", userId);
                    command.Parameters.AddWithValue("@valueUnitId", value_unit_id);
                    command.Parameters.AddWithValue("@fileupload", flupload.Visible?fileBytes:null);
                    //added new 
                    command.Parameters.Add("@resulttext", SqlDbType.Int).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();


                    int resultCode = 0;
                    if (command.Parameters["@resulttext"].Value != DBNull.Value)
                        resultCode = Convert.ToInt32(command.Parameters["@resulttext"].Value);

                    switch (resultCode)
                    {

                        case -1:
                            ShowAlert("Error Occurred");
                            return;
                        case 5:
                            ShowAlert("Target Overlap: This Target–Project–Centre–Course combination already exists for another timeline.");
                            return;

                        case 10:
                            ShowAlert("Target–Project–Centre–Course Combination Already Exists ! First Close Existing Target, then Try Again");
                            return;

                        default:
                            // no error, normal flow continues
                            break;
                    }



                    strMessage = (action == "insert")
                        ? "New record saved successfully."
                        : "Record updated successfully.";
                }

                Response.Redirect("TargetDetails.aspx?msg=" + strMessage, true);
            }


        }
        catch (SqlException ex)
        {
            ShowAlert(ex.Message, true);
        }
        catch (Exception ex)
        {
            ShowAlert("Error: " + ex.Message, true);
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
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
            //ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
            ddlcoursefilter.SelectedValue = "0";
            ddlcentrefilter.SelectedValue = "0";
            ddlfreqfilter.SelectedValue = "0";
            txtfilterefffrom.Text = "";
            txtfiltereffto.Text = "";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
            if (ViewState["SortOrder"].ToString() == "DESC")
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] GetSearchText(string prefixText, int count)
    {
        if (count <= 0) count = 10;
        string search = (prefixText ?? "").Trim().ToUpper();

        string connStr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

        var items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        using (var conn = new SqlConnection(connStr))
        using (var cmd = new SqlCommand("getTargets", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = ( reader["targetName"] as string ?? "").Trim();
                    //string code = (reader["projectName"]  as string ?? "").Trim();

                    // Match logic
                    if (string.IsNullOrEmpty(search) ||
                        name.ToUpper().Contains(search) 
                        // || code.ToUpper().Contains(search))
                        )
                    {
                        if (!string.IsNullOrEmpty(name))
                            items.Add(name);
                        //if (!string.IsNullOrEmpty(code))
                        //    items.Add(code);
                    }

                    // Stop early if we already have enough
                    if (items.Count >= count)
                        break;
                }
            }
        }

        return items.Take(count).ToArray();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("TargetDetails.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    /*Backend Validations Start */
    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidDob(TextBox txtBox)
    {
        try
        {
            DateTime today = DateTime.Now;
            DateTime inputDate = Convert.ToDateTime(txtBox.Text);

            if (inputDate > today.AddYears(-10))
            {
                return false;  // too young
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /* Backend Validations End */

}