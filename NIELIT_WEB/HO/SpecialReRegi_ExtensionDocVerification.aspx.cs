using DocumentFormat.OpenXml.Office.Word;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_SpecialReRegi_ExtensionDocVerification : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
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
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    //FillFilterQualificationLevel();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Special Re Registration/Extension Status", "/HO/SpecialReRegi_ExtensionDocVerification.aspx", ""));
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());

                FillFilters();
                btnExportExcel.Visible=true;
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void FillFilters()
    {
        try
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["EConnectContext"]
                .ConnectionString;

            string sql = @"
            SELECT id, name
            FROM nielit.dbo.applicant_type;

            SELECT id, name
            FROM nielit.dbo.course
            WHERE id IN (1, 2, 3, 4);

            SELECT id, name
            FROM Registration_Type
            WHERE id IN (7, 8);";

            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(ds);
            }

            // Applicant Type
            BindFilter(
                ddlfilterapplicanttype,
                ds.Tables[0],
                "name",
                "id"
            );

            // Course
            BindFilter(
                ddlfilterCourseLevel,
                ds.Tables[1],
                "name",
                "id"
            );

            // Registration Type
            BindFilter(
                ddlfilterRegistrationType,
                ds.Tables[2],
                "name",
                "id"
            );
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private void BindFilter(
        DropDownList ddl,
        DataTable dt,
        string textField,
        string valueField)
    {
        ddl.DataTextField = textField;
        ddl.DataValueField = valueField;
        ddl.DataSource = dt;
        ddl.DataBind();

        ddl.Items.Insert(0, new ListItem("-- All --", "-99"));
    }



    protected void ShowEditMode()
    {
        try
        {
            if (pnlFilter != null) pnlFilter.Visible = false;
            if (ucSearchBar != null) ucSearchBar.Visible = false;
            LoadVerificationDetails();
            LoadVerifiedDetails();
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
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            string sortOrder = ViewState["SortOrder"] != null ? ViewState["SortOrder"].ToString() : "ASC";
            string sortField = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "Name";

            // default sorting safeguard
            if (string.IsNullOrEmpty(sortField)) sortField = "Name";
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "ASC";

            // Read filter values (use -99 for "All" as per procedure defaults)
            int courseID = -99;
            if (ddlfilterCourseLevel != null && ddlfilterCourseLevel.SelectedValue != "0" && !string.IsNullOrEmpty(ddlfilterCourseLevel.SelectedValue))
                courseID = Convert.ToInt32(ddlfilterCourseLevel.SelectedValue);

            int splRegnType = -99;
            // If you have a dropdown for Special Registration Type, use it here
            if (ddlfilterRegistrationType != null && ddlfilterRegistrationType.SelectedValue != "0")
                splRegnType = Convert.ToInt32(ddlfilterRegistrationType.SelectedValue);

            int applicantType = -99;
            // If you have a dropdown for Applicant Type, use it here
            if (ddlfilterapplicanttype != null && ddlfilterapplicanttype.SelectedValue != "0")
                applicantType = Convert.ToInt32(ddlfilterapplicanttype.SelectedValue);

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("getdataforreregistrationcases", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseID", courseID);
                cmd.Parameters.AddWithValue("@SplRegntype", splRegnType);
                cmd.Parameters.AddWithValue("@applicant_type", applicantType);

                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            // Optional client-side search (if ucSearchBar is present)
            string searchString = string.Empty;
            if (ucSearchBar != null)
                searchString = ucSearchBar.SearchText.Trim().ToUpper();

            if (!string.IsNullOrEmpty(searchString) && dt.Rows.Count > 0)
            {
                DataView dvSearch = dt.DefaultView;
                dvSearch.RowFilter = string.Format(
                    "Convert([Name], 'System.String') LIKE '%{0}%' OR Convert([Regn No], 'System.String') LIKE '%{0}%'",
                    searchString.Replace("'", "''"));
                dt = dvSearch.ToTable();
            }

            // Apply sorting in memory
            if (dt.Rows.Count > 0)
            {
                string sortExpression = sortField + " " + sortOrder;
                try
                {
                    dt.DefaultView.Sort = sortExpression;
                    dt = dt.DefaultView.ToTable();
                }
                catch
                {
                    // fallback if column name does not match
                    dt.DefaultView.Sort = "Name ASC";
                    dt = dt.DefaultView.ToTable();
                }
            }


            PagingBar1.Bind(dt, ref gvMain);


            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void BindGridView()
    //{
    //    try
    //    {

    //        string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

    //        //Int64 instituteId = Convert.ToInt64(hfAccreID.Value);
    //        //lblError.Visible = false;
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
    //            string sortOrder = ViewState["SortOrder"].ToString();
    //            string sortField = ViewState["SortField"].ToString();
    //            Int32 QualifID =0;

    //            if (ddlfilterCourseLevel.SelectedValue != "0")
    //                QualifID = Convert.ToInt32(ddlfilterCourseLevel.SelectedValue);


    //            var EducationQuali = from s in context.EducationalQualifications
    //                                 orderby s.DisplayOrder
    //                            select new
    //                            {
    //                                ID = s.ID,
    //                                Name = s.Name,
    //                                QualificationLevel = s.QualificationLevel.Name,
    //                                QualificationLevelID=s.QualificationLevelID,
    //                                Code=s.Code

    //                            };

    //            if (!String.IsNullOrEmpty(searchString))
    //            {
    //                EducationQuali = EducationQuali.Where(s => s.Name.ToUpper().Contains(searchString)
    //                                                       || s.Code.ToUpper().Contains(searchString));
    //            }
    //            //if (Activestatus != "")
    //            //{
    //            //    ExamVenue = ExamVenue.Where(s => s.IsActive == Activestatus);
    //            //}
    //            if (QualifID != 0)
    //            {
    //                EducationQuali = EducationQuali.Where(s => s.QualificationLevelID == QualifID);
    //            }
    //            if (!string.IsNullOrEmpty(sortOrder))
    //            {
    //                switch (sortField)
    //                {
    //                    case "ID":
    //                        if (sortOrder == "DESC")
    //                            EducationQuali = EducationQuali.OrderByDescending(s => s.ID);
    //                        else
    //                            EducationQuali = EducationQuali.OrderBy(s => s.ID);
    //                        break;
    //                    case "Name":
    //                        if (sortOrder == "DESC")
    //                            EducationQuali = EducationQuali.OrderByDescending(s => s.Name);
    //                        else
    //                            EducationQuali = EducationQuali.OrderBy(s => s.Name);
    //                        break;
    //                    case "QualificationLevel":
    //                        if (sortOrder == "DESC")
    //                            EducationQuali = EducationQuali.OrderByDescending(s => s.QualificationLevel);
    //                        else
    //                            EducationQuali = EducationQuali.OrderBy(s => s.QualificationLevel);
    //                        break;
    //                    case "Code":
    //                        if (sortOrder == "DESC")
    //                            EducationQuali = EducationQuali.OrderByDescending(s => s.Code);
    //                        else
    //                            EducationQuali = EducationQuali.OrderBy(s => s.Code);
    //                        break;
    //                    default:
    //                        EducationQuali = EducationQuali.OrderBy(s => s.ID);
    //                        break;
    //                }
    //            }
    //            PagingBar1.Bind(EducationQuali, ref gvMain);
    //            uPnlGrid.Update();
    //            uPnlNavigation.Update();
    //            //if (gvMain.Rows.Count <= 0)
    //            //{
    //            //    lblError.Text = "No Record Found";
    //            //    lblError.Visible = true;
    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
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
    //protected void ToggleViewMode_Changed(object sender, EventArgs e)
    //{
    //    if (btnMode.ViewMode == ToggleView.Mode.New)
    //    {
    //        //if (!UserManager.HasRight(currentRoleId, enmRight.New))
    //        //{
    //        //    BreadCrumb1.Render();
    //        //    ShowAlert("Sorry! You don't have rights to add new record.",true);
    //        //    return;
    //        //}
    //        //FillQualificationLevel();
    //        btnMode.ViewMode = ToggleView.Mode.List;
    //        mltvTab.ActiveViewIndex = 1;
    //        pnlFilter.Visible = false;
    //        ucSearchBar.Visible = false;
    //        //Change the heading text as required
    //        lblHeading.Text = "Education Qualification";
    //        //Updating Breadcrumb
    //        //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
    //        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Education Qualification", "#", ""));
    //    }
    //    else
    //    {
    //        Response.Redirect("EducationQualification.aspx", true);
    //    }
    //}
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        //try
        //{
        //    BreadCrumb1.Render();
        //    using (EConnectContext context = new EConnectContext())
        //    {
        //        EducationalQualification objEducationQualification;
        //        string qualfname = txtEdu.Text;
        //        string code = txtCode.Text;
        //        Int32 displayOrder = Convert.ToInt32(txtDisplayOrder.Text);
        //        Int32 qualificationlevelID = Convert.ToInt32(ddlQualificationLevel.SelectedValue);
        //        if (String.IsNullOrEmpty(Request.QueryString["Key"]))
        //        {
        //            if(context.EducationalQualifications.Any(s => s.Name.ToUpper() == qualfname.ToUpper()))
        //            {
        //                throw new Exception("This educational qualification name already exists.");
        //            }
        //            else if(context.EducationalQualifications.Any(s => s.Code.ToUpper() ==code.ToUpper()))
        //            {
        //                throw new Exception("This educational qualification code already exists.");
        //            }
        //            else if(context.EducationalQualifications.Any(s => s.DisplayOrder ==displayOrder))
        //            {
        //                throw new Exception("This educational qualification display order already exists.");
        //            }
        //            else
        //            {
        //                objEducationQualification = new EConnect.EducationalQualification();
        //                objEducationQualification.QualificationLevelID = Convert.ToInt32(ddlQualificationLevel.SelectedValue);
        //                objEducationQualification.Name = Convert.ToString(txtEdu.Text.Trim());
        //                objEducationQualification.Code = txtCode.Text.ToString().Trim();
        //                objEducationQualification.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
        //                context.EducationalQualifications.Add(objEducationQualification);
        //                context.SaveChanges();
        //                strMessage = "New record saved";

        //            }
        //        }
        //        else
        //        {
        //            Int32 keyID = Convert.ToInt32(Request.QueryString["Key"]);
        //            if (!(context.EducationalQualifications.Any(s => s.Name.ToUpper() == qualfname.ToUpper() && s.Code.ToUpper() == code.ToUpper() && s.ID!=keyID)))
        //            {
        //                objEducationQualification = context.EducationalQualifications.Find(Convert.ToInt32(Request.QueryString["key"]));
        //                objEducationQualification.QualificationLevelID = Convert.ToInt32(ddlQualificationLevel.SelectedValue);
        //                objEducationQualification.Name = Convert.ToString(txtEdu.Text.Trim());
        //                objEducationQualification.Code = txtCode.Text.ToString().ToUpper().Trim();
        //                //objEducationQualification.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
        //                context.SaveChanges();
        //                strMessage = "Record updated";

        //            }
        //            else
        //            {
        //                throw new Exception("This Educational Qualification Already Exists.");
        //            }
        //        }
        //        Response.Redirect("EducationQualification.aspx?msg="+strMessage);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message,true);
        //}
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
            //ddlflstates.SelectedValue = "0";
            //ddlficoursecategory.SelectedValue = "0";
            //ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
            //ddlflexcentretype.SelectedValue = "0";
            ddlfilterCourseLevel.SelectedValue = "0";
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                //HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //h2.NavigateUrl =EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl);
                //HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //h3.NavigateUrl =EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl);
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h4.NavigateUrl);
                //HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                //h5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h5.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;

            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();

            var EduQuali = from s in context.EducationalQualifications
                           select new { Name = s.Name };


            if (!String.IsNullOrEmpty(searchString))
            {
                EduQuali = EduQuali.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            EduQuali = EduQuali.OrderBy(s => s.Name);

            var EduQuali1 = from s in context.EducationalQualifications
                            select new { Name = s.Code };


            if (!String.IsNullOrEmpty(searchString))
            {
                EduQuali1 = EduQuali1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            EduQuali1 = EduQuali1.OrderBy(s => s.Name);
            EduQuali = EduQuali.Union(EduQuali1).Take(count);
            foreach (var c in EduQuali)
            {
                items.Add(c.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("EducationQualification.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }


    // VIEW 2 
    //private void LoadVerifiedDetails()
    //{
    //    try
    //    {
    //        if (Request.QueryString["Key"] == null)
    //            return;

    //        DataTable dt = new DataTable();
    //        string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

    //        int statusVerified = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationVerified);
    //        int statusRejected = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationRejected);
    //        int statusHold = Convert.ToInt32(enmCandidateUpdateRequestStatus.KeptinAbeyance);

    //        string sql = @"
    //            SELECT 
    //                ID,
    //                candidate_id,
    //                Doc_Type,
    //                Request_Status_Id,
    //                Verified_On,
    //                remarks
    //            FROM MercyCase_UploadedDocs 
    //            WHERE candidate_id IN (
    //                SELECT Candidate_ID 
    //                FROM Course_Registration_Application 
    //                WHERE Registered_Course_Registration_No = @RegnNo 
    //                  AND registration_type_id IN (7, 8)
    //            )
    //            AND Request_Status_Id IN (@StatusVerified, @StatusRejected, @StatusHold)";

    //        using (SqlConnection con = new SqlConnection(connStr))
    //        using (SqlCommand cmd = new SqlCommand(sql, con))
    //        {
    //            cmd.CommandType = CommandType.Text;
    //            cmd.Parameters.AddWithValue("@RegnNo", Convert.ToInt64(Request.QueryString["Key"]));
    //            cmd.Parameters.AddWithValue("@StatusVerified", statusVerified);
    //            cmd.Parameters.AddWithValue("@StatusRejected", statusRejected);
    //            cmd.Parameters.AddWithValue("@StatusHold", statusHold);

    //            SqlDataAdapter da = new SqlDataAdapter(cmd);
    //            da.Fill(dt);
    //        }

    //        if (!dt.Columns.Contains("VerificationStatus"))
    //            dt.Columns.Add("VerificationStatus", typeof(string));

    //        foreach (DataRow row in dt.Rows)
    //        {
    //            int stId = 0;
    //            if (row["Request_Status_Id"] != DBNull.Value)
    //                stId = Convert.ToInt32(row["Request_Status_Id"]);

    //            if (stId == statusVerified)
    //                row["VerificationStatus"] = "Verified";
    //            else if (stId == statusRejected)
    //                row["VerificationStatus"] = "Rejected";
    //            else if (stId == statusHold)
    //                row["VerificationStatus"] = "On Hold";
    //            else
    //                row["VerificationStatus"] = "Pending";
    //        }

    //        gvVerifiedDocuments.DataSource = dt;
    //        gvVerifiedDocuments.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    private const int MercyStatusPending = 1;
    private const int MercyStatusVerified = 2;
    private const int MercyStatusRejected = 3;
    private const int MercyStatusHold = 4;

    private void LoadVerifiedDetails()
    {
        try
        {
            if (Request.QueryString["Key"] == null)
                return;

            DataTable dt = new DataTable();

            string connStr = ConfigurationManager
                .ConnectionStrings["EConnectContext"]
                .ConnectionString;

            string sql = @"
            SELECT 
                ID,
                candidate_id,
                Doc_Type,
                Request_Status_Id,
                Verified_On,
                Remarks
            FROM MercyCase_UploadedDocs 
            WHERE candidate_id IN
            (
                SELECT Candidate_ID 
                FROM Course_Registration_Application 
                WHERE Candidate_ID = @Candidate_ID 
                  AND registration_type_id IN (7, 8)
            )
            AND Request_Status_Id IN
            (
                @StatusVerified,
                @StatusRejected                
            )";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue(
                    "@Candidate_ID",
                    Convert.ToInt64(Request.QueryString["Key"])
                );

                cmd.Parameters.AddWithValue(
                    "@StatusVerified",
                    MercyStatusVerified
                );

                cmd.Parameters.AddWithValue(
                    "@StatusRejected",
                    MercyStatusRejected
                );               

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            if (!dt.Columns.Contains("VerificationStatus"))
                dt.Columns.Add("VerificationStatus", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                int statusId = 0;

                if (row["Request_Status_Id"] != DBNull.Value)
                    statusId = Convert.ToInt32(row["Request_Status_Id"]);

                if (statusId == MercyStatusVerified)
                {
                    row["VerificationStatus"] = "Verified";
                }
                else if (statusId == MercyStatusRejected)
                {
                    row["VerificationStatus"] = "Rejected";
                }               
            }

            gvVerifiedDocuments.DataSource = dt;
            gvVerifiedDocuments.DataBind();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        { 
            long entityID = Session["EntityID"] != null ? Convert.ToInt64(Session["EntityID"]) : 0;
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
          //  GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
            if (e.CommandName == "Verify" || e.CommandName == "Reject" || e.CommandName == "Hold")
            {
                long documentId = Convert.ToInt64(e.CommandArgument);
                // Get the row on which button was clicked
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                // Get Remark textbox from that row
                TextBox txtRemark = (TextBox)row.FindControl("txtRemark");
              //  GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
             //   TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

               // string remark = txtRemark.Text.Trim();              

                string remark = txtRemark != null
                    ? txtRemark.Text.Trim()
                    : string.Empty;
                if (string.IsNullOrWhiteSpace(remark))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                        "alert('Please enter remark.');", true);
                    return;
                }

                int statusId = 0;

                if (e.CommandName == "Verify")
                {
                    statusId = 2;
                }
                else if (e.CommandName == "Reject")
                {
                    statusId = 3;
                }
                else if (e.CommandName == "Hold")
                {
                    statusId = 4;
                }

                UpdateDocumentStatus(documentId, statusId, remark, entityID);

                Button btn = (Button)e.CommandSource;

                if (e.CommandName == "Verify")
                {
                    btn.Text = "Verified";                   
                }
                else if (e.CommandName == "Reject")
                {
                    btn.Text = "Rejected";                   
                }
                else if (e.CommandName == "Hold")
                {
                    btn.Text = "On Hold";                   
                }

                LoadVerifiedDetails();
                SetSendApprovalVisibility();
                //  BindDocuments();
            }

            //if (e.CommandName == "Verify")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);

            //    using (SqlConnection con = new SqlConnection(connStr))
            //    using (SqlCommand cmd = new SqlCommand(@"
            //        UPDATE MercyCase_UploadedDocs
            //        SET docsVerifiedOn = GETDATE(),
            //            VerifiedBy = @EntityID,
            //            Request_Status_Id = @StatusID,
            //            remarks = @Remarks
            //        WHERE ID = @ID", con))
            //    {
            //        cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
            //        cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
            //        cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusVerified;
            //        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = "Document Accepted";

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //    }

            //    LoadVerifiedDetails();
            //    LoadVerificationDetails();
            //}

            //if (e.CommandName == "Reject")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);

            //    GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
            //    TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

            //    string remark = txtRemark != null ? txtRemark.Text.Trim() : string.Empty;

            //    if (string.IsNullOrWhiteSpace(remark))
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "msg",
            //            "alert('Please enter rejection remark.');", true);
            //        return;
            //    }

            //    using (SqlConnection con = new SqlConnection(connStr))
            //    using (SqlCommand cmd = new SqlCommand(@"
            //        UPDATE MercyCase_UploadedDocs
            //        SET docsVerifiedOn = GETDATE(),
            //            VerifiedBy = @EntityID,
            //            Request_Status_Id = @StatusID,
            //            remarks = @Remarks
            //        WHERE ID = @ID", con))
            //    {
            //        cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
            //        cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
            //        cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusRejected;
            //        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = remark;

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //    }

            //    long candidateID = ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0;
            //    long regnNo = !string.IsNullOrEmpty(Request.QueryString["Key"]) ? Convert.ToInt64(Request.QueryString["Key"]) : 0;
            //    //if (candidateID > 0 && regnNo > 0)
            //    //{
            //    //    SendRegistrationUpdationRejectionMail(candidateID, regnNo);
            //    //}

            //    LoadVerifiedDetails();
            //    LoadVerificationDetails();
            //}

            //if (e.CommandName == "Hold")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);

            //    GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
            //    TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

            //    string remark = txtRemark != null ? txtRemark.Text.Trim() : string.Empty;

            //    if (string.IsNullOrWhiteSpace(remark))
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "msg",
            //            "alert('Please enter hold remark.');", true);
            //        return;
            //    }

            //    using (SqlConnection con = new SqlConnection(connStr))
            //    using (SqlCommand cmd = new SqlCommand(@"
            //        UPDATE MercyCase_UploadedDocs
            //        SET docsVerifiedOn = GETDATE(),
            //            Verified_By = @EntityID,
            //            Request_Status_Id = @StatusID,
            //            remarks = @Remarks
            //        WHERE ID = @ID", con))
            //    {
            //        cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
            //        cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
            //        cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusHold;
            //        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = remark;

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //    }

            //    long candidateID = ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0;
            //    long regnNo = !string.IsNullOrEmpty(Request.QueryString["Key"]) ? Convert.ToInt64(Request.QueryString["Key"]) : 0;
            //    //if (candidateID > 0 && regnNo > 0)
            //    //{
            //    //    SendRegistrationUpdationRejectionMail(candidateID, regnNo);
            //    //}

            //    LoadVerifiedDetails();
            //    LoadVerificationDetails();
            //}

            SetFinalSubmitVisibility();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private void UpdateDocumentStatus(long documentId, int statusId, string remark, long entityID)
    {
         DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_UpdateMercyDocumentStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", documentId);
                cmd.Parameters.AddWithValue("@Request_Status_Id", statusId);
                cmd.Parameters.AddWithValue("@Remarks", remark);
                cmd.Parameters.AddWithValue("@Verified_By", entityID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        gvVerifiedDocuments.DataSource = dt;
        gvVerifiedDocuments.DataBind();
    }

    //protected void btnSendApproval_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        long candidateId = ViewState["CandidateID"] != null
    //            ? Convert.ToInt64(ViewState["CandidateID"])
    //            : 0;

    //        if (candidateId <= 0)
    //        {
    //            ShowAlert("Candidate information not found.", true);
    //            return;
    //        }

    //        // Double-check before allowing approval
    //        if (!AreAllDocumentsVerified(candidateId))
    //        {
    //            ShowAlert("Please verify all documents before sending for approval.", true);
    //            return;
    //        }

    //        Response.Redirect(
    //            "MercyCasePreview.aspx?CandidateID=" + candidateId,
    //            false);

    //        Context.ApplicationInstance.CompleteRequest();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    protected void btnSendApproval_Click(object sender, EventArgs e)
    {
        try
        {
            long candidateId = ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0;
            int courseId = ViewState["courseId"] != null ? Convert.ToInt32(ViewState["courseId"]) : 0;

            if (candidateId <= 0)
            {
                ShowAlert("Candidate information not found.", true);
                return;
            }

            // Double-check from database
            if (!AreAllDocumentsVerified(candidateId))
            {
                ShowAlert(
                    "All documents must be verified before sending for approval.",
                    true);

                return;
            }
            // Get logged-in user ID
            long loggedInUserId = Convert.ToInt64(Session["EntityID"]);

            // Save Send For Approval status
            pnlApprovalDetails.Visible = true;
            SaveSpecialExtensionRequest(candidateId, loggedInUserId);
            btnFinalSubmit.Visible = true;
            btnSendApproval.Visible = false;
            btnExportExcel.Visible = false;
            //  ExportMercyCaseToExcel(candidateId, courseId);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
         long candidateId = ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0;
            int courseId = ViewState["courseId"] != null ? Convert.ToInt32(ViewState["courseId"]) : 0;
        ExportMercyCaseToExcel(candidateId, courseId);
    }

    private void SaveSpecialExtensionRequest(long candidateId, long loggedInUserId)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_SaveSpecialExtensionRequest", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Candidate_ID", candidateId);
                cmd.Parameters.AddWithValue("@LoggedInUserId", loggedInUserId);
                con.Open();
                cmd.ExecuteNonQuery();               
            }            
        }
        catch
        {
            throw;
        }
    }

    private void ExportMercyCaseToExcel(long candidateId, int courseId)
    {
        string connStr = ConfigurationManager
            .ConnectionStrings["EConnectContext"]
            .ConnectionString;

        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand("sp_get_MercyData_Preview", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = candidateId;

            cmd.Parameters.Add("@Course_ID", SqlDbType.Int).Value = courseId;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        if (dt.Rows.Count == 0)
        {
            ShowAlert("No Mercy Case data found for this candidate.", true);
            return;
        }

        Response.Clear();
        Response.Buffer = true;

        Response.AddHeader(
            "content-disposition",
            "attachment;filename=SpecialRegistrationExtension_" + candidateId + ".xls");

        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        {
            GridView gvExcel = new GridView();

            gvExcel.DataSource = dt;
            gvExcel.DataBind();

            gvExcel.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            //  Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    //private bool SaveExtensionRequest(long candidateId, int courseId)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            int sendForApprovalStatusId = context.SpecialReg_ExtensionRequestStatus
    //                .Where(x => x.StatusName == "Send For Approval")
    //                .Select(x => x.Status_Id)
    //                .FirstOrDefault();

    //            if (sendForApprovalStatusId <= 0)
    //                return false;

    //            long loggedInUserId = Convert.ToInt64(Session["EntityID"]);

    //            var request = context.SpecialReg_ExtensionRequest
    //                .FirstOrDefault(x =>
    //                    x.Candidate_Id == candidateId &&
    //                    x.Request_Status_Id == sendForApprovalStatusId);

    //            if (request == null)
    //            {
    //                request = new SpecialReg_ExtensionRequest
    //                {
    //                    Candidate_Id = candidateId,
    //                    Request_Status_Id = sendForApprovalStatusId,
    //                    Created_By = loggedInUserId,
    //                    Created_On = DateTime.Now
    //                };

    //                context.SpecialReg_ExtensionRequest.Add(request);
    //            }
    //            else
    //            {
    //                request.Modified_By = loggedInUserId;
    //                request.Modified_On = DateTime.Now;
    //            }

    //            context.SaveChanges();

    //            return true;
    //        }
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

    private void BindVerifiedDocuments(long candidateId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMercyVerifiedDocuments", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@candidate_id", candidateId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvVerifiedDocuments.DataSource = dt;
                        gvVerifiedDocuments.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }

    //protected string GetVerifyButtonText(object status)
    //{
    //    if (status == null || status == DBNull.Value)
    //        return "Verify";

    //    int statusId = Convert.ToInt32(status);

    //    if (statusId == 2)
    //        return "Verified";

    //    return "Verify";
    //}

    //protected string GetRejectButtonText(object status)
    //{
    //    if (status == null || status == DBNull.Value)
    //        return "Reject";

    //    int statusId = Convert.ToInt32(status);

    //    if (statusId == 3)
    //        return "Rejected";

    //    return "Reject";
    //}

    protected string GetDocumentButtonText(object status, string buttonType)
    {
        if (status == null || status == DBNull.Value)
            return buttonType;

        int statusId = Convert.ToInt32(status);

        if (buttonType == "Verify" && statusId == 2)
            return "Verified";

        if (buttonType == "Reject" && statusId == 3)
            return "Rejected";

        //if (buttonType == "Hold" && statusId == 4)
        //    return "On Hold";

        return buttonType;
    }

    private void LoadVerificationDetails()
    {
        try
        {
            if (Request.QueryString["Key"] == null)
                return;

            //btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            btnFinalSubmit.Text = "Registration Process";
            lblHeading.Text = "Document Verification for Special Re Registration / Extension Form";

            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            string sql = @"
                            SELECT 
                                d.[ID],
                                d.[candidate_id],
                                a.[Course_ID],
                                d.[Original_File_Name],
                                d.[Extension],
                                d.[Uploaded_File],
                                d.[Uploaded_On],
                                d.[Doc_Type],
                                d.[Request_Status_Id],
                                d.[remarks]
                            FROM MercyCase_UploadedDocs d
                            INNER JOIN Course_Registration_Application a
                                ON d.candidate_id = a.Candidate_ID
                            WHERE d.candidate_id = @Candidate_ID
                              AND a.registration_type_id IN (7,8)";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Candidate_ID", Convert.ToInt64(Request.QueryString["Key"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                ViewState["CandidateID"] = dt.Rows[0]["candidate_id"];
                ViewState["courseId"] = dt.Rows[0]["Course_ID"];

                int firstStatusId = 0;
                if (dt.Columns.Contains("Request_Status_Id") && dt.Rows[0]["Request_Status_Id"] != DBNull.Value)
                    firstStatusId = Convert.ToInt32(dt.Rows[0]["Request_Status_Id"]);
                else if (dt.Columns.Contains("requestStatusID") && dt.Rows[0]["requestStatusID"] != DBNull.Value)
                    firstStatusId = Convert.ToInt32(dt.Rows[0]["requestStatusID"]);

                if (firstStatusId == (int)enmCandidateUpdateRequestStatus.KeptinAbeyance)
                {
                    ShowAlert("This record has been kept on hold.");
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "This record has been kept on hold.";
                }

                if (!dt.Columns.Contains("docsVerified")) dt.Columns.Add("docsVerified", typeof(bool));
                if (!dt.Columns.Contains("docsRejected")) dt.Columns.Add("docsRejected", typeof(bool));
                if (!dt.Columns.Contains("docsHold")) dt.Columns.Add("docsHold", typeof(bool));
                if (!dt.Columns.Contains("Remark")) dt.Columns.Add("Remark", typeof(string));
                if (!dt.Columns.Contains("requestStatusID")) dt.Columns.Add("requestStatusID", typeof(int));

                int statusVerified = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationVerified);
                int statusRejected = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationRejected);
                int statusHold = Convert.ToInt32(enmCandidateUpdateRequestStatus.KeptinAbeyance);

                foreach (DataRow row in dt.Rows)
                {
                    int statusId = 0;
                    if (dt.Columns.Contains("Request_Status_Id") && row["Request_Status_Id"] != DBNull.Value)
                        statusId = Convert.ToInt32(row["Request_Status_Id"]);
                    else if (row["requestStatusID"] != DBNull.Value)
                        statusId = Convert.ToInt32(row["requestStatusID"]);

                    row["requestStatusID"] = statusId;
                    row["docsVerified"] = (statusId == statusVerified);
                    row["docsRejected"] = (statusId == statusRejected);
                    row["docsHold"] = (statusId == statusHold);

                    if (dt.Columns.Contains("remarks") && row["remarks"] != DBNull.Value)
                        row["Remark"] = row["remarks"].ToString();
                }

                gvDocuments.DataSource = dt;
                gvDocuments.DataBind();
            }
            else
            {
                gvDocuments.DataSource = null;
                gvDocuments.DataBind();
            }

            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Candidate ID - " + Convert.ToString(Request.QueryString["Key"]), "#", ""));

            //  SetFinalSubmitVisibility();
            SetSendApprovalVisibility();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    // Send For Approval

    private bool AreAllDocumentsVerified(long candidateId)
    {
        string connStr = ConfigurationManager
            .ConnectionStrings["EConnectContext"]
            .ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT 
            COUNT(*) AS TotalDocuments,
            SUM(CASE WHEN Request_Status_Id = 2 THEN 1 ELSE 0 END) AS VerifiedDocuments
        FROM MercyCase_UploadedDocs
        WHERE candidate_id = @CandidateID
    ", con))
        {
            cmd.Parameters.Add("@CandidateID", SqlDbType.BigInt).Value = candidateId;

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    int totalDocuments = Convert.ToInt32(dr["TotalDocuments"]);
                    int verifiedDocuments = Convert.ToInt32(dr["VerifiedDocuments"]);

                    return totalDocuments > 0 &&
                           totalDocuments == verifiedDocuments;
                }
            }
        }

        return false;
    }

    private void SetSendApprovalVisibility()
    {
        long candidateId = ViewState["CandidateID"] != null
            ? Convert.ToInt64(ViewState["CandidateID"])
            : 0;

        if (candidateId <= 0)
        {
            btnSendApproval.Visible = false;
            return;
        }

        btnSendApproval.Visible = AreAllDocumentsVerified(candidateId);
    }

    private void SendRegistrationUpdationRejectionMail(long candidateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                bool allowSendingEmail = false;

                var evnt = context.NotificationEvent.Find(
                    Convert.ToInt32(enmNotificationEvents.AfterSendingtheSpecialRegistrationorSpecialExtensionforapprovalasVerified));

                if (evnt != null && evnt.SendEmail)
                    allowSendingEmail = true;

                if (!allowSendingEmail)
                    return;
                // Get candidate email and rejected document details
                var rejectedDocument = (
                    from c in context.Candidates
                    join cd in context.CandidateContactDetails
                        on c.ID equals cd.CandidateID
                    join d in context.MercyCase_UploadedDocs
                        on c.ID equals d.Candidate_Id
                    join s in context.MercyDocumentStatus
                        on d.Request_Status_Id equals s.ID
                    where c.ID == candidateID
                          && s.StatusName == "Rejected"
                    select new
                    {
                        Name = c.Salutation + " " + c.Name,
                        Email = cd.EmailAddress,
                        DocumentName = d.Original_File_Name,
                        DocumentType = d.Doc_Type,
                        Remark = d.Remarks,
                        RejectedOn = d.Uploaded_On
                    }).FirstOrDefault();

                if (rejectedDocument != null &&
                    !string.IsNullOrWhiteSpace(rejectedDocument.Email))
                {
                    string msg =
                        "Dear " + GetInitCap(rejectedDocument.Name) + ",<br/><br/>" +
                        "One of the documents submitted as part of your Special Registration / Special Extension request has been rejected." +
                        "<br/><br/>" +
                        "<b>Document :</b> " + rejectedDocument.DocumentName +
                        "<br/><b>Document Type :</b> " + rejectedDocument.DocumentType +
                        "<br/><b>Remark :</b> " + rejectedDocument.Remark +
                        "<br/><br/>" +
                        "Kindly review the remark, make the necessary corrections, and re-upload the document." +
                        "<br/><br/>" +
                        "Regards,<br/>NIELIT";

                    EConnect.NIELIT.Email mail =
                        new EConnect.NIELIT.Email(
                            "Special Registration / Extension - Document Rejected : NIELIT",
                            msg,
                            rejectedDocument.Email);

                    mail.Send();
                }

                //var candidate = (from c in context.Candidates
                //                 join cd in context.CandidateContactDetails
                //                    on c.ID equals cd.CandidateID
                //                 join r in context.candidateUpdateRequest
                //                    on c.ID equals r.candID
                //                 join u in context.UpdateRegnMasters
                //                 on r.updateFieldID equals u.ID
                //                 where c.ID == candidateID
                //                       && r.requestNo == requestNumber && r.requestFinalised == true
                //                 select new
                //                 {
                //                     Name = c.Salutation + " " + c.Name,
                //                     Email = cd.EmailAddress,
                //                     RequestNo = r.requestNo,
                //                     FieldName = u.fieldName,
                //                     Remark = r.remarks,
                //                     RequestDate = r.enterdate
                //                 }).FirstOrDefault();

                //if (candidate != null && !string.IsNullOrWhiteSpace(candidate.Email))
                //{
                //    string msg =
                //        "Dear " + GetInitCap(candidate.Name) + ",<br/><br/>" +
                //        "One of the documents submitted as part of your Registration Updation Request has been rejected." +
                //        "<br/><br/>" +
                //        "<b>Request Number :</b> " + candidate.RequestNo +
                //        "<br/><b>Rejected Field :</b> " + candidate.FieldName +
                //        "<br/><b>Remark :</b> " + candidate.Remark +
                //        "<br/><br/>Kindly review the remark, make the necessary corrections, and submit the request again." +
                //        "<br/><br/>Regards,<br/>NIELIT";

                //    EConnect.NIELIT.Email mail =
                //        new EConnect.NIELIT.Email(
                //            "Registration Updation Request - Document Rejected : NIELIT",
                //            msg,
                //            candidate.Email);

                //    mail.Send();
                //}
            }
        }
        catch
        {
            // Ignore email failures.
        }
    }

    private void SetFinalSubmitVisibility()
    {
        try
        {
            if (Request.QueryString["Key"] == null)
            {
                btnFinalSubmit.Visible = false;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(*)
                FROM MercyCase_UploadedDocs
                WHERE candidate_id IN (
                    SELECT Candidate_ID 
                    FROM Course_Registration_Application 
                    WHERE Registered_Course_Registration_No = @RegnNo 
                      AND registration_type_id IN (7,8)
                )
                AND (Request_Status_Id IS NULL OR Request_Status_Id = 0)", con))
            {
                cmd.Parameters.AddWithValue("@RegnNo", Convert.ToInt64(Request.QueryString["Key"]));

                con.Open();

                int pending = Convert.ToInt32(cmd.ExecuteScalar());
                if (pending == 0)
                    btnFinalSubmit.Visible = true;
                else
                    btnFinalSubmit.Visible = false;
            }
        }
        catch
        {
            btnFinalSubmit.Visible = false;
        }
    }

    protected void btnFinalSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("updationRequestProcess", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RequestNo", Convert.ToInt64(Request.QueryString["Key"]));
                cmd.Parameters.AddWithValue("@candidateID", ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0);

                con.Open();

                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (result == 1)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Rejected",
                    "alert('Entire request has been rejected.');window.location='SpecialReRegi_ExtensionDocVerification.aspx';",
                    true);

                return;
            }

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Success",
                "alert('Candidate details updated successfully.');window.location='SpecialReRegi_ExtensionDocVerification.aspx';",
                true);

            LoadVerifiedDetails();
            LoadVerificationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void gvDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        object statusObj = DataBinder.Eval(e.Row.DataItem, "Request_Status_Id");

        if (statusObj == null || statusObj == DBNull.Value)
            return;

        int statusId = Convert.ToInt32(statusObj);

        Button btnVerify = (Button)e.Row.FindControl("btnVerify");

        Button btnReject = (Button)e.Row.FindControl("btnReject");

        Button btnHold = (Button)e.Row.FindControl("btnHold");
        // Default: all buttons enabled
        if (btnVerify != null)
            btnVerify.Enabled = true;

        if (btnReject != null)
            btnReject.Enabled = true;

        if (btnHold != null)
            btnHold.Enabled = true;

        // If document is Verified
        if (statusId == 2)
        {
            if (btnVerify != null)
            {
                btnVerify.Text = "Verified";
                btnVerify.Enabled = false;
            }

            if (btnReject != null)
                btnReject.Enabled = false;

            if (btnHold != null)
                btnHold.Enabled = false;
        }
        // If document is Rejected
        else if (statusId == 3)
        {
            if (btnVerify != null)
                btnVerify.Enabled = false;

            if (btnReject != null)
            {
                btnReject.Text = "Rejected";
                btnReject.Enabled = false;
            }

            if (btnHold != null)
                btnHold.Enabled = false;
        }
        // If document is On Hold
        else if (statusId == 4)
        {
            if (btnVerify != null)
                btnVerify.Enabled = false;

            if (btnReject != null)
                btnReject.Enabled = false;

            if (btnHold != null)
            {
                btnHold.Text = "On Hold";
                btnHold.Enabled = false;
            }
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("SpecialReRegi_ExtensionDocVerification.aspx");
    }


    protected void btnShowCand_Click(object sender, EventArgs e)
    {
        try
        {
            long candidateId = 0;
            int courseId = 0;
            int applicantTypeId = 0;
            string appliedAs = "D";
            string examFee = "";
            string examCycle = "";

            // Get Candidate ID
            if (ViewState["CandidateID"] != null &&
                long.TryParse(ViewState["CandidateID"].ToString(), out candidateId) &&
                candidateId > 0)
            {
                // Candidate ID obtained from ViewState
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                long.TryParse(Request.QueryString["Key"], out candidateId);
            }

            // Get Course ID
            if (ViewState["courseId"] != null)
            {
                int.TryParse(ViewState["courseId"].ToString(), out courseId);
            }

            if (candidateId <= 0)
            {
                ShowAlert("Candidate information not found.");
                return;
            }

            string keyID = Request.QueryString["SplRegType"];
            int registrationTypeId = (keyID == "SR") ? 7 : 8;

            string connStr = ConfigurationManager
                .ConnectionStrings["EConnectContext"]
                .ConnectionString;

            const string sql = @"
          SELECT TOP 1
                cra.Course_ID,
                Applicant_Type_ID,
                Fee_Amt, e.Name as Exam_Cycle
            FROM Course_Registration_Application cra
            left join Exam e on e.id = cra.Exam_ID
            WHERE Candidate_ID = @CandidateID
              AND registration_type_id = @RegistrationTypeId";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@CandidateID", SqlDbType.BigInt).Value = candidateId;
                cmd.Parameters.Add("@RegistrationTypeId", SqlDbType.Int).Value = registrationTypeId;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (dr.Read())
                    {
                        if (dr["Fee_Amt"] != DBNull.Value)
                        {
                            examFee = dr["Fee_Amt"].ToString();
                        }

                        if (dr["Exam_Cycle"] != DBNull.Value)
                        {
                            examCycle = dr["Exam_Cycle"].ToString();
                        }

                        if (courseId == 0 && dr["Course_ID"] != DBNull.Value)
                        {
                            courseId = Convert.ToInt32(dr["Course_ID"]);
                        }

                        if (dr["Applicant_Type_ID"] != DBNull.Value)
                        {
                            applicantTypeId = Convert.ToInt32(dr["Applicant_Type_ID"]);

                            if (applicantTypeId == (int)enmApplicantType.Institute)
                            {
                                appliedAs = "I";
                            }
                        }
                    }
                }
            }

            string targetUrl;

            if (keyID == "SR")
            {
                targetUrl = string.Format(
                    "~/CAND/SpecialReRegistrationPreview.aspx" +
                    "?CandidateId={0}" +
                    "&CourseId={1}" +
                    "&applicant_type_id={2}" +
                    "&appliedAs={3}" +
                    "&ExamFee={4}" +
                    "&ExamCycle={5}" +
                    "&Mode=View",
                    candidateId,
                    courseId,
                    applicantTypeId,
                    appliedAs,
                    examFee,
                    examCycle);
            }
            else
            {
                targetUrl = string.Format(
                    "~/CAND/SpecialExtensionPreview.aspx" +
                    "?CandidateId={0}" +
                    "&CourseId={1}" +
                    "&applicant_type_id={2}" +
                    "&appliedAs={3}" +
                    "&ExamFee={4}" +
                    "&ExamCycle={5}" +
                    "&Mode=View",
                    candidateId,
                    courseId,
                    applicantTypeId,
                    appliedAs,
                    examFee,
                    examCycle);
            }

            string resolvedUrl = ResolveUrl(targetUrl);

            string script = string.Format(
                "window.open('{0}', '_blank');",
                resolvedUrl);

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "OpenPreview",
                script,
                true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    // For rejection
    //private bool IsAnyDocumentRejected(long candidateId)
    //{
    //    using (EConnectContext context = new EConnectContext())
    //    {
    //        return context.MercyCase_UploadedDocs.Any(x =>
    //            x.Candidate_Id == candidateId &&
    //            x.Request_Status_Id == 3 &&
    //            (x.whetherDiscarded == null || x.whetherDiscarded == false));
    //    }
    //}

    //private void BindVerifiedRejectedDocuments(long candidateId)
    //{      

    //    btnRejectRequest.Visible = IsAnyDocumentRejected(candidateId);
    //}
}