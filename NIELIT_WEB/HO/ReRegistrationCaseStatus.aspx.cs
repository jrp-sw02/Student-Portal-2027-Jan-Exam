using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_ReRegistrationCaseStatus : BasePage
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
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Re Registration Case Status", "/HO/ReRegistrationCaseStatus.aspx", ""));
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());

                FillFilters();
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
    private void LoadVerifiedDetails()
    {
        try
        {
            if (Request.QueryString["Key"] == null)
                return;

            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            int statusVerified = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationVerified);
            int statusRejected = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationRejected);
            int statusHold = Convert.ToInt32(enmCandidateUpdateRequestStatus.KeptinAbeyance);

            string sql = @"
                SELECT 
                    ID,
                    candidate_id,
                    Doc_Type,
                    Request_Status_Id,
                    Verified_On,
                    remarks
                FROM MercyCase_UploadedDocs 
                WHERE candidate_id IN (
                    SELECT Candidate_ID 
                    FROM Course_Registration_Application 
                    WHERE Registered_Course_Registration_No = @RegnNo 
                      AND registration_type_id IN (7, 8)
                )
                AND Request_Status_Id IN (@StatusVerified, @StatusRejected, @StatusHold)";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@RegnNo", Convert.ToInt64(Request.QueryString["Key"]));
                cmd.Parameters.AddWithValue("@StatusVerified", statusVerified);
                cmd.Parameters.AddWithValue("@StatusRejected", statusRejected);
                cmd.Parameters.AddWithValue("@StatusHold", statusHold);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (!dt.Columns.Contains("VerificationStatus"))
                dt.Columns.Add("VerificationStatus", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                int stId = 0;
                if (row["Request_Status_Id"] != DBNull.Value)
                    stId = Convert.ToInt32(row["Request_Status_Id"]);

                if (stId == statusVerified)
                    row["VerificationStatus"] = "Verified";
                else if (stId == statusRejected)
                    row["VerificationStatus"] = "Rejected";
                else if (stId == statusHold)
                    row["VerificationStatus"] = "On Hold";
                else
                    row["VerificationStatus"] = "Pending";
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
            int statusVerified = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationVerified);
            int statusRejected = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationRejected);
            int statusHold = Convert.ToInt32(enmCandidateUpdateRequestStatus.KeptinAbeyance);

            long entityID = Session["EntityID"] != null ? Convert.ToInt64(Session["EntityID"]) : 0;
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            if (e.CommandName == "Verify")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE MercyCase_UploadedDocs
                    SET docsVerifiedOn = GETDATE(),
                        VerifiedBy = @EntityID,
                        Request_Status_Id = @StatusID,
                        remarks = @Remarks
                    WHERE ID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusVerified;
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = "Document Accepted";

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadVerifiedDetails();
                LoadVerificationDetails();
            }

            if (e.CommandName == "Reject")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

                string remark = txtRemark != null ? txtRemark.Text.Trim() : string.Empty;

                if (string.IsNullOrWhiteSpace(remark))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                        "alert('Please enter rejection remark.');", true);
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE MercyCase_UploadedDocs
                    SET docsVerifiedOn = GETDATE(),
                        VerifiedBy = @EntityID,
                        Request_Status_Id = @StatusID,
                        remarks = @Remarks
                    WHERE ID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusRejected;
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = remark;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                long candidateID = ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0;
                long regnNo = !string.IsNullOrEmpty(Request.QueryString["Key"]) ? Convert.ToInt64(Request.QueryString["Key"]) : 0;
                if (candidateID > 0 && regnNo > 0)
                {
                    SendRegistrationUpdationRejectionMail(candidateID, regnNo);
                }

                LoadVerifiedDetails();
                LoadVerificationDetails();
            }

            if (e.CommandName == "Hold")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

                string remark = txtRemark != null ? txtRemark.Text.Trim() : string.Empty;

                if (string.IsNullOrWhiteSpace(remark))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                        "alert('Please enter hold remark.');", true);
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE MercyCase_UploadedDocs
                    SET docsVerifiedOn = GETDATE(),
                        Verified_By = @EntityID,
                        Request_Status_Id = @StatusID,
                        remarks = @Remarks
                    WHERE ID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusHold;
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = remark;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                long candidateID = ViewState["CandidateID"] != null ? Convert.ToInt64(ViewState["CandidateID"]) : 0;
                long regnNo = !string.IsNullOrEmpty(Request.QueryString["Key"]) ? Convert.ToInt64(Request.QueryString["Key"]) : 0;
                if (candidateID > 0 && regnNo > 0)
                {
                    SendRegistrationUpdationRejectionMail(candidateID, regnNo);
                }

                LoadVerifiedDetails();
                LoadVerificationDetails();
            }

            SetFinalSubmitVisibility();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private void LoadVerificationDetails()
    {
        try
        {
            if (Request.QueryString["Key"] == null)
                return;

            //btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            btnFinalSubmit.Text = "Final Submit";
            lblHeading.Text = "Document Verification for Spl Re Registration form";

            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            string sql = @"
                SELECT 
                    [ID],
                    [candidate_id],
                    [Original_File_Name],
                    [Extension],
                    [Uploaded_File],
                    [Uploaded_On],
                    [Doc_Type],
                    [Request_Status_Id],
                    [remarks]
                FROM MercyCase_UploadedDocs 
                WHERE candidate_id IN (
                    SELECT Candidate_ID 
                    FROM Course_Registration_Application 
                    WHERE Registered_Course_Registration_No = @RegnNo 
                      AND registration_type_id IN (7,8)
                )";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@RegnNo", Convert.ToInt64(Request.QueryString["Key"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                ViewState["CandidateID"] = dt.Rows[0]["candidate_id"];

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

            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Number - " + Convert.ToString(Request.QueryString["Key"]), "#", ""));

            SetFinalSubmitVisibility();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private void SendRegistrationUpdationRejectionMail(long candidateID, long requestNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                bool allowSendingEmail = false;

                var evnt = context.NotificationEvent.Find(
                    Convert.ToInt32(enmNotificationEvents.AftersubmittingtheRegistrationUpdationCorrectionform));

                if (evnt != null && evnt.SendEmail)
                    allowSendingEmail = true;

                if (!allowSendingEmail)
                    return;

                var candidate = (from c in context.Candidates
                                 join cd in context.CandidateContactDetails
                                    on c.ID equals cd.CandidateID
                                 join r in context.candidateUpdateRequest
                                    on c.ID equals r.candID
                                 join u in context.UpdateRegnMasters
                                 on r.updateFieldID equals u.ID
                                 where c.ID == candidateID
                                       && r.requestNo == requestNumber && r.requestFinalised == true
                                 select new
                                 {
                                     Name = c.Salutation + " " + c.Name,
                                     Email = cd.EmailAddress,
                                     RequestNo = r.requestNo,
                                     FieldName = u.fieldName,
                                     Remark = r.remarks,
                                     RequestDate = r.enterdate
                                 }).FirstOrDefault();

                if (candidate != null && !string.IsNullOrWhiteSpace(candidate.Email))
                {
                    string msg =
                        "Dear " + GetInitCap(candidate.Name) + ",<br/><br/>" +
                        "One of the documents submitted as part of your Registration Updation Request has been rejected." +
                        "<br/><br/>" +
                        "<b>Request Number :</b> " + candidate.RequestNo +
                        "<br/><b>Rejected Field :</b> " + candidate.FieldName +
                        "<br/><b>Remark :</b> " + candidate.Remark +
                        "<br/><br/>Kindly review the remark, make the necessary corrections, and submit the request again." +
                        "<br/><br/>Regards,<br/>NIELIT";

                    EConnect.NIELIT.Email mail =
                        new EConnect.NIELIT.Email(
                            "Registration Updation Request - Document Rejected : NIELIT",
                            msg,
                            candidate.Email);

                    mail.Send();
                }
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
                    "alert('Entire request has been rejected.');window.location='ReRegistrationCaseStatus.aspx';",
                    true);

                return;
            }

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Success",
                "alert('Candidate details updated successfully.');window.location='ReRegistrationCaseStatus.aspx';",
                true);

            LoadVerifiedDetails();
            LoadVerificationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}