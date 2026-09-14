using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Admin_CourseRegStudentStatus : BasePage
{
    String strMessage = string.Empty;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    List<SqlParameter> paramList = new List<SqlParameter>();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError.Visible = false;

        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("~/Index.aspx");

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/acc_reg_info.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ApplID"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BindGridView();
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    #region--Events---
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged1(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridViewMain();
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BatchItems.aspx?BatchID=" + Request.QueryString["BatchID"].ToString() + "&Status=" + Request.QueryString["Status"]), true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
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
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
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
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
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
            PagingBar1.CurrentPageIndex = 0;
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected string returnDataName(Int64 vCourseID)
    {
        //START -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  NIELITMIS 
        Int32 courseCatId = 0;
        string checkDbExistsData = "NA";
        if (vCourseID != 0)
        {
            // string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id=" + vCourseID;
            //  Int32 courseId = GetCourseCategoryID(Query1);

            // string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;
            //November_2024
            string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id= @vCourseID ";
            paramList.Add(new SqlParameter("@vCourseID", vCourseID));

            Int32 courseId = GetCourseCategoryID(Query1);

            //string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;

            //November_2024
            paramList.Clear();
            string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=@courseId ";
            paramList.Add(new SqlParameter("@courseId", courseId));
            courseCatId = GetCourseCategoryID(Query2);
            checkDbExistsData = "NIELITMIS";

            if (courseCatId == 0)
            {
                checkDbExistsData = "NIELIT";
            }
        }
        return (checkDbExistsData);
        //END -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  
    }

    public int GetCourseCategoryID(string myQuery)
    {
        Int32 result = 0;
        try
        {
            // string result = "0";          
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand cmd = new SqlCommand(myQuery, conn);
            //November_2024
            cmd.Parameters.AddRange(paramList.ToArray());

            conn.Open();
            var CourseCategoryId = cmd.ExecuteScalar();
            if (CourseCategoryId != null)
            {
                result = Convert.ToInt32(CourseCategoryId.ToString());
            }
            conn.Close();
            return result;
        }
        catch (Exception exx)
        {
            return result;
        }
    }
    protected void gbbatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl1 = (HyperLink)e.Row.Cells[1].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);

                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);


                HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);

                HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);

                //HyperLink hl6= (HyperLink)e.Row.Cells[6].Controls[0];
                //hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();


                //Added_UP_Project_For_Project_And_Batch_Add_10_12_2024_Start added by amit


                Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                //Int64 ApplNo = Convert.ToInt64(Request.QueryString["ApplID"]);
                string ApplNo = hl1.Text.ToString();
                Int32 ExamID = Convert.ToInt32(Request.QueryString["Examid"]);
                entityID = Convert.ToInt64(Session["EntityID"]);

                using (NIELITMISContext context = new NIELITMISContext())
                {

                    ListItem lst = new ListItem("--Select One--", "0");
                    var Proc = (from t in context.NielitCentreStudent
                                join k in context.NielitCentreBatchs on t.batch_ID equals k.ID
                                join x in context.NielitCentreCourses on t.CourseID equals x.ID
                                //join d in context.NielitCourseDurations on k.courseID equals d.ID
                                where t.CourseID == CourseID
                                && k.IsActive
                                && t.InstituteID == entityID
                                orderby (k.Name)
                                select new { ValueField = k.ID, TextField = k.Name });

                                if (Proc.Any())
                                {
                                    // Find the DropDownList control in the current row
                                    DropDownList ddlBatchOptions = (DropDownList)e.Row.FindControl("ddlBatchId");
                                    //e.Row.Cells[8].Visible = true;
                                    ddlBatchOptions.Visible = true;
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchOptions, Proc, lst);
                                }
                                else
                                {
                                    DropDownList ddlBatchOptions = (DropDownList)e.Row.FindControl("ddlBatchId");
                                    //e.Row.Cells[8].Visible = false;
                                    ddlBatchOptions.Visible = false;
                                }

                    DropDownList ddlBatchId = (DropDownList)e.Row.FindControl("ddlBatchId");

                    string ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (var command = new SqlCommand("sp_GetBatch", con))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@CourseID", CourseID.ToString());
                            //command.Parameters.AddWithValue("@ApplTypeID", ApplTypeID.ToString());
                            command.Parameters.AddWithValue("@applicantType", applicantType.ToString());
                            command.Parameters.AddWithValue("@ExamID", ExamID.ToString());
                            command.Parameters.AddWithValue("@InstituteID", entityID.ToString());
                            command.Parameters.AddWithValue("@ApplNo", ApplNo.ToString());
                            //command.Parameters.AddWithValue("@ApplNo", Convert.ToInt64(Request.QueryString["ApplID"]));

                            con.Open();
                            
                            int batchId = Convert.ToInt32(command.ExecuteScalar());
                            con.Close();

                            if (batchId != 0)
                            {
                                ddlBatchId.SelectedValue = batchId.ToString();
                                ddlBatchId.Enabled = false;
                            }

                        }
                    }
                    //PROJECT FILL  populating in drop down of project
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var Proc1 = (from t in context.NielitProjectss
                                 join k in context.NielitProjCoursess on t.ID equals k.projID
                                 join x in context.projectMainCentres on t.ID equals x.projectID
                                 //join d in context.NielitCourseDurations on k.courseID equals d.ID
                                 where k.courseID == CourseID
                                 && x.centreID == entityID
                                 && k.IsActive
                                 && t.projectTodate >= DateTime.Today
                                 orderby (t.ProjectName)
                                 select new { ValueField = t.ID, TextField = t.ProjectName })

                               .Union(from t in context.NielitProjectss
                                      join k in context.NielitProjCoursess on t.ID equals k.projID
                                      join x in context.projectSubCentres on t.ID equals x.projectID
                                      join i in context.AffInstitutes on x.centreID equals i.ID 
                                      //join d in context.NielitCourseDurations on k.courseID equals d.ID
                                      where k.courseID == CourseID
                                          && i.instituteID == entityID
                                          && k.IsActive
                                          && t.projectTodate >= DateTime.Today
                                      orderby (t.ProjectName)
                                      select new { ValueField = t.ID, TextField = t.ProjectName });


   
                    // added by amit start
                    bool excludeUPProject = false;

                    String applnum = Convert.ToString(ApplNo);

                    using (var econtext = new EConnectContext())
                    {
                        excludeUPProject = econtext.CourseRegistrationApplications
                            .Any(cra => cra.Number == applnum && cra.projectID == null);
                    }
                    

                    if (excludeUPProject)
                    {
                        Proc1 = Proc1.Where(p => p.ValueField != 10023 && p.ValueField != 36);
                    }

                    // added by amit end
                    if (Proc1.Any())
                    {
                        // Find the DropDownList control in the current row
                        DropDownList ddlProjectId = (DropDownList)e.Row.FindControl("ddlProjectId");
                        ddlProjectId.Visible = true;
                        //e.Row.Cells[9].Visible = true;
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectId, Proc1, lst1);                        
                    }
                    else
                    {
                        DropDownList ddlProjectId = (DropDownList)e.Row.FindControl("ddlProjectId");
                        ddlProjectId.Visible = false;
                        //e.Row.Cells[9].Visible = false;
                    }

                    // Fetch project for the Data filled on Registration page
                    // 
                    ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (var command = new SqlCommand("sp_GetProject", con))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@CourseID", CourseID.ToString());
                            //command.Parameters.AddWithValue("@ApplTypeID", ApplTypeID.ToString());
                            command.Parameters.AddWithValue("@applicantType", applicantType.ToString());
                            command.Parameters.AddWithValue("@ExamID", ExamID.ToString());
                            command.Parameters.AddWithValue("@InstituteID", entityID.ToString());
                            command.Parameters.AddWithValue("@ApplNo", ApplNo.ToString());
                            //command.Parameters.AddWithValue("@ApplNo", Convert.ToInt64(Request.QueryString["ApplID"]));

                            con.Open();
                            //int projectID = Convert.ToInt32(command.ExecuteScalar());

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Fetch values from the columns
                                    long projectID = reader.GetInt64(reader.GetOrdinal("ID")); // Replace column names as per your SP
                                    bool isVerified = reader.GetBoolean(reader.GetOrdinal("Is_Verified_By_Institute"));

                                    // Use the values as needed
                                    DropDownList ddlProjectId = (DropDownList)e.Row.FindControl("ddlProjectId");
                                    ddlProjectId.SelectedValue = projectID.ToString();
                                    ddlProjectId.Enabled = false;
                                    //ddlProjectId.Enabled = !isVerified; // Example: Disable dropdown based on verification status
                                    //ddlBatchId.Enabled = !isVerified;
                                }
                                //else
                                //{
                                //    // Handle case where no rows are returned
                                //    throw new Exception("No data found for the given parameters.");
                                //}
                            }



                            con.Close();

                            //if (projectID != 0)
                            //{
                            //    DropDownList ddlProjectId = (DropDownList)e.Row.FindControl("ddlProjectId");
                            //    ddlProjectId.SelectedValue = projectID.ToString();
                            //    ddlProjectId.Enabled = false;
                            //}

                        }
                    }

                    //if (ddlOptions.SelectedValue =="")
                    //    BindProject(e);

                    //Added_UP_Project_For_Project_And_Batch_Add_10_12_2024_End

                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gbbatch_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int16 verifiedCount = 0;
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 ExamID = Convert.ToInt32(Request.QueryString["Examid"]);
            //For institue * cases
            Int32 Applstatusid1 = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);

            #region Courese Registration---------------
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                List<Int64> applist = new List<Int64>();
                int coursCatgId = 0;
                Int64 applID = 0;
                Int32 ApplicationStatusId = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute);

                using (EConnectContext context = new EConnectContext())
                {
                    coursCatgId = context.Courses.Find(courseID).CourseCategory.ID;
                    Int32 paidStatusID = Convert.ToInt32(enmPaymentStatus.Pending);

                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.IsVerifiedByInstitute = true;
                                    appl.DateOfVerificationByInstitute = DateTime.Now;

                                    //Added_UP_Project_And_Batch_06_12_2024_Start

                                    DropDownList drBatchId = (DropDownList)gbbatch.Rows[i].FindControl("ddlBatchId");
                                    if (drBatchId.Visible == true)
                                    {
                                        if (Convert.ToInt32(drBatchId.SelectedItem.Value) != 0)
                                            appl.batchID = Convert.ToInt64(drBatchId.SelectedItem.Value);
                                        //else
                                        //{
                                        //    ShowAlert("Please Select Batch", true);
                                        //    return;
                                        //}
                                    }

                                    DropDownList drProjectId = (DropDownList)gbbatch.Rows[i].FindControl("ddlProjectId");

                                    // added by amit 
                                    if (drProjectId.Visible == true && drProjectId.Enabled == true)
                                    {
                                        if (Convert.ToInt32(drProjectId.SelectedItem.Value) != 0)
                                            appl.projectID = Convert.ToInt32(drProjectId.SelectedItem.Value);
                                        //else
                                        //{
                                        //    ShowAlert("Please Select Project", true);
                                        //    return;
                                        //}
                                    }
                                    // added by end
                                    // 
                                    //Added_UP_Project_And_Batch_06_12_2024_End



                                    if (appl.FeeAmount == 0 && appl.CourseCategoryID == 6 && appl.PaymentStatusID == 2)
                                    {
                                        appl.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        applist.Add(appl.ID);
                                    }
                                    else
                                        appl.ApplicationStatusID = ApplicationStatusId;

                                    //For_History_Of_UP_Project_Batch_12_12_2024_Start
                                    //if (((drBatchId.Visible == true) || (drProjectId.Visible == true)) && ((drBatchId.Enabled == true) || (drProjectId.Enabled == true)))
                                    //{
                                    //    using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
                                    //    {
                                    //        using (SqlCommand cmd = new SqlCommand("sp_CourseRegProjectBatch_History", Conn))
                                    //        {
                                    //            Conn.Open();
                                    //            cmd.CommandType = CommandType.StoredProcedure;
                                    //            cmd.Parameters.AddWithValue("@enterBy", Convert.ToInt64(Session["EntityID"]));
                                    //            cmd.Parameters.AddWithValue("@enterDate", DateTime.Now);
                                    //            cmd.Parameters.AddWithValue("@id", applID);

                                    //            cmd.ExecuteNonQuery();
                                    //        }
                                    //    }
                                    //}
                                    //For_History_Of_UP_Project_Batch_12_12_2024_End

                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };
                ///////--------------VIVEK-----------Add here for STC registration number generation----------------------

                if (coursCatgId == 6 && applist.Count > 0) // for STC
                {
                    STCregistration.STCregistrationNumberAllocation2(applist, courseID, coursCatgId, ExamID);
                }
                ///////-----------------------------------------------------------------

                ShowAlert(verifiedCount.ToString() + " application(s) has been verified.", true);
                BindGridView();
            }
            #endregion

            #region Certificate Exam Application----------
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                if (!CheckCutoffDate(ExamID))
                {
                    ShowAlert("You cannot verify the application, because Last Date is over..... ");
                    return;
                }
                else
                {
                    if (gbbatch.Rows.Count > 0)
                    {
                        for (int i = 0; i < gbbatch.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                            TextBox txtF = (TextBox)gbbatch.Rows[i].FindControl("txtFyear");
                            DropDownList drF = (DropDownList)gbbatch.Rows[i].FindControl("ddlFmonth");

                            TextBox txtT = (TextBox)gbbatch.Rows[i].FindControl("txtTyear");
                            DropDownList drT = (DropDownList)gbbatch.Rows[i].FindControl("ddlTmonth");

                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {

                                    if (drF.SelectedValue == "0")
                                    {
                                        ShowAlert("Please select course duration from month", true);
                                        drF.Focus();
                                        return;
                                    }
                                    else if (txtF.Text == "")
                                    {
                                        ShowAlert("Please enter course duration from year", true);
                                        txtF.Focus();
                                        return;
                                    }
                                    else if (drT.SelectedValue == "0")
                                    {
                                        ShowAlert("Please select course duration to month", true);
                                        drT.Focus();
                                        return;
                                    }
                                    else if (txtT.Text == "")
                                    {
                                        ShowAlert("Please enter course duration to year", true);
                                        txtT.Focus();
                                        return;
                                    }
                                    DateTime dtFrom = new DateTime(Convert.ToInt32(txtF.Text), Convert.ToInt32(drF.SelectedValue), 1);
                                    DateTime dtTo = new DateTime(Convert.ToInt32(txtT.Text), Convert.ToInt32(drT.SelectedValue), 1);
                                    if (dtFrom > DateTime.Now)
                                    {
                                        ShowAlert("Course duration From can not be greater than current date", true);
                                        txtF.Focus();
                                        return;
                                    }
                                    else if (dtTo < dtFrom)
                                    {
                                        ShowAlert("Course duration To can not be less than course duration From", true);
                                        txtT.Focus();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    Int64 applID = 0;
                    Int32 ApplicationStatusId = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute);

                    using (EConnectContext context = new EConnectContext())
                    {
                        Int32 paidStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                        Int32 instituteID = (int)entityID;
                        for (int i = 0; i < gbbatch.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                            TextBox txtF = (TextBox)gbbatch.Rows[i].FindControl("txtFyear");
                            DropDownList drF = (DropDownList)gbbatch.Rows[i].FindControl("ddlFmonth");

                            TextBox txtT = (TextBox)gbbatch.Rows[i].FindControl("txtTyear");
                            DropDownList drT = (DropDownList)gbbatch.Rows[i].FindControl("ddlTmonth");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    verifiedCount += 1;
                                    applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);

                                    CertificateExamApplication appl = context.CertificateExamApplications.Find(applID);
                                    if (appl != null)
                                    {
                                        appl.CourseDurationFrom = drF.SelectedItem.Text + "-" + txtF.Text;
                                        appl.CourseDurationTo = drT.SelectedItem.Text + "-" + txtT.Text;
                                        appl.IsVerifiedByInstitute = true;
                                        appl.DateOfVerificationByInstitute = DateTime.Now;
                                        if (appl.IsExempted == true && appl.FeeAmount == 0)
                                            appl.ApplicationStatusID = Applstatusid1;
                                        else
                                            appl.ApplicationStatusID = ApplicationStatusId;
                                        if (appl.FinalSubmitted == false)
                                        {
                                            appl.FinalSubmitted = true;
                                            appl.FinalSubmissionDate = DateTime.Now;
                                        }
                                        context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                            }
                        }
                        context.SaveChanges();
                    };

                    ShowAlert(verifiedCount.ToString() + " application(s) verified successfully.", true);
                    BindGridView();
                }
            }
            #endregion

            #region Course Exam---------------
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                if (!CheckCutoffDate(ExamID))
                {
                    ShowAlert("You cannot Verify Applications, because Last Date is over..... ");
                    return;
                }
                if (gbbatch.Rows.Count > 0)
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        TextBox txtF = (TextBox)gbbatch.Rows[i].FindControl("txtFyear");
                        DropDownList drF = (DropDownList)gbbatch.Rows[i].FindControl("ddlFmonth");

                        TextBox txtT = (TextBox)gbbatch.Rows[i].FindControl("txtTyear");
                        DropDownList drT = (DropDownList)gbbatch.Rows[i].FindControl("ddlTmonth");

                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                if (drF.SelectedValue == "0")
                                {
                                    ShowAlert("Please select course duration from month", true);
                                    drF.Focus();
                                    return;
                                }
                                else if (txtF.Text == "")
                                {
                                    ShowAlert("Please enter course duration from year", true);
                                    txtF.Focus();
                                    return;
                                }
                                else if (drT.SelectedValue == "0")
                                {
                                    ShowAlert("Please select course duration to month", true);
                                    drT.Focus();
                                    return;
                                }
                                else if (txtT.Text == "")
                                {
                                    ShowAlert("Please enter course duration to year", true);
                                    txtT.Focus();
                                    return;
                                }
                                DateTime dtFrom = new DateTime(Convert.ToInt32(txtF.Text), Convert.ToInt32(drF.SelectedValue), 1);
                                DateTime dtTo = new DateTime(Convert.ToInt32(txtT.Text), Convert.ToInt32(drT.SelectedValue), 1);
                                if (dtTo <= dtFrom)
                                {
                                    ShowAlert("Course duration To can not be less than or equal  to course duration From", true);
                                    txtT.Focus();
                                    return;
                                }
                            }
                        }
                    }
                }
                Int64 applID = 0;
                Int32 ApplicationStatusId = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute);

                using (EConnectContext context = new EConnectContext())
                {
                    Int32 paidStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    Int32 instituteID = (int)entityID;
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        TextBox txtF = (TextBox)gbbatch.Rows[i].FindControl("txtFyear");
                        DropDownList drF = (DropDownList)gbbatch.Rows[i].FindControl("ddlFmonth");

                        TextBox txtT = (TextBox)gbbatch.Rows[i].FindControl("txtTyear");
                        DropDownList drT = (DropDownList)gbbatch.Rows[i].FindControl("ddlTmonth");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.CourseDurationFrom = drF.SelectedItem.Text + "-" + txtF.Text;
                                    appl.CourseDurationTo = drT.SelectedItem.Text + "-" + txtT.Text;
                                    appl.IsVerifiedByInstitute = true;
                                    appl.DateOfVerificationByInstitute = DateTime.Now;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };

                ShowAlert(verifiedCount.ToString() + " application(s) verified successfully.", true);
                BindGridView();
            }
            #endregion
			#region Project Exam---------------
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
            {

                //commented on  05-04-2024
                //if (!CheckCutoffDate(ExamID))
                //{
                //    ShowAlert("You cannot Verify Applications, because Last Date is over..... ");
                //    return;
                //}
                //commented on  05-04-2024
                if (gbbatch.Rows.Count > 0)
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        TextBox txtF = (TextBox)gbbatch.Rows[i].FindControl("txtFyear");
                        DropDownList drF = (DropDownList)gbbatch.Rows[i].FindControl("ddlFmonth");

                        TextBox txtT = (TextBox)gbbatch.Rows[i].FindControl("txtTyear");
                        DropDownList drT = (DropDownList)gbbatch.Rows[i].FindControl("ddlTmonth");

                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                if (drF.SelectedValue == "0")
                                {
                                    ShowAlert("Please select course duration from month", true);
                                    drF.Focus();
                                    return;
                                }
                                else if (txtF.Text == "")
                                {
                                    ShowAlert("Please enter course duration from year", true);
                                    txtF.Focus();
                                    return;
                                }
                                else if (drT.SelectedValue == "0")
                                {
                                    ShowAlert("Please select course duration to month", true);
                                    drT.Focus();
                                    return;
                                }
                                else if (txtT.Text == "")
                                {
                                    ShowAlert("Please enter course duration to year", true);
                                    txtT.Focus();
                                    return;
                                }
                                DateTime dtFrom = new DateTime(Convert.ToInt32(txtF.Text), Convert.ToInt32(drF.SelectedValue), 1);
                                DateTime dtTo = new DateTime(Convert.ToInt32(txtT.Text), Convert.ToInt32(drT.SelectedValue), 1);
                                if (dtTo <= dtFrom)
                                {
                                    ShowAlert("Course duration To can not be less than or equal  to course duration From", true);
                                    txtT.Focus();
                                    return;
                                }
                            }
                        }
                    }
                }
                Int64 applID = 0;
                Int32 ApplicationStatusId = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute);

                using (EConnectContext context = new EConnectContext())
                {
                    Int32 paidStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    Int32 instituteID = (int)entityID;
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        TextBox txtF = (TextBox)gbbatch.Rows[i].FindControl("txtFyear");
                        DropDownList drF = (DropDownList)gbbatch.Rows[i].FindControl("ddlFmonth");

                        TextBox txtT = (TextBox)gbbatch.Rows[i].FindControl("txtTyear");
                        DropDownList drT = (DropDownList)gbbatch.Rows[i].FindControl("ddlTmonth");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                //CourseExamApplication appl = context.CourseExamApplications.Find(applID);





                                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.CourseDurationFrom = drF.SelectedItem.Text + "-" + txtF.Text;
                                    appl.CourseDurationTo = drT.SelectedItem.Text + "-" + txtT.Text;
                                    appl.IsVerifiedByInstitute = true;
                                    appl.DateOfVerificationByInstitute = DateTime.Now;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;



                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };

                ShowAlert(verifiedCount.ToString() + " application(s) verified successfully.", true);
                BindGridView();
            }
            #endregion
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnMNotVerify_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int64 applID = 0;
            Int16 verifiedCount = 0;
            Int16 ApplicationStatusId = Convert.ToInt16(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification);
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);

            #region Course Registration Application-----
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {   
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusId;

                                    //Added_UP_Project_And_Batch_06_12_2024_Start

                                    DropDownList drBatchId = (DropDownList)gbbatch.Rows[i].FindControl("ddlBatchId");
                                    if (drBatchId.Visible == true)
                                    {
                                        if (Convert.ToInt32(drBatchId.SelectedItem.Value) != 0)
                                            appl.batchID = Convert.ToInt64(drBatchId.SelectedItem.Value);
                                        else
                                        {
                                            ShowAlert("Please Select Batch", true);
                                            return;
                                        }
                                    }

                                    DropDownList drProjectId = (DropDownList)gbbatch.Rows[i].FindControl("ddlProjectId");
                                    // added by amit start
                                    if (drProjectId.Visible == true && drProjectId.Enabled == true)
                                    {
                                        if (Convert.ToInt32(drProjectId.SelectedItem.Value) != 0)
                                            appl.projectID = Convert.ToInt32(drProjectId.SelectedItem.Value);
                                        else
                                        {
                                            ShowAlert("Please Select Project", true);
                                            return;
                                        }
                                    }
                                    // added by amit end

                                    //Added_UP_Project_And_Batch_06_12_2024_End

                                    ////Added_02_09_2024
                                    //appl.projectID = Convert.ToInt32(drProjectId.SelectedItem.Value);


                                    if (CommonFunctions.IsDemandNoteCancellable(appl.DemandNoteID.HasValue ? appl.DemandNoteID.Value : 0))
                                    {
                                        appl.DemandNoteID = null;
                                    }



                                    //For_History_Of_UP_Project_Batch_12_12_2024_Start
                                    //if(((drBatchId.Visible == true)||(drProjectId.Visible == true))&& ((drBatchId.Enabled == true) || (drProjectId.Enabled == true)))
                                    //{
                                    //    using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
                                    //    {
                                    //        using (SqlCommand cmd = new SqlCommand("sp_CourseRegProjectBatch_History", Conn))
                                    //        {
                                    //            Conn.Open();
                                    //            cmd.CommandType = CommandType.StoredProcedure;
                                    //            cmd.Parameters.AddWithValue("@createdBy", Convert.ToInt64(Session["EntityID"]));
                                    //            cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                                    //            cmd.Parameters.AddWithValue("@updatedBy", Convert.ToInt64(Session["EntityID"]));
                                    //            cmd.Parameters.AddWithValue("@updatedDate", DateTime.Now);
                                    //            cmd.Parameters.AddWithValue("@id", applID);

                                    //            cmd.ExecuteNonQuery();
                                    //        }
                                    //    }
                                    //}
                                    //For_History_Of_UP_Project_Batch_12_12_2024_End

                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };
            }
            #endregion

            #region Certificate Exam Application-----
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CertificateExamApplication appl = context.CertificateExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.CourseDurationFrom = null;
                                    appl.CourseDurationTo = null;
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    if (CommonFunctions.IsDemandNoteCancellable(appl.DemandNoteID.HasValue ? appl.DemandNoteID.Value : 0))
                                    {
                                        appl.DemandNoteID = null;
                                    }
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };
            }
            #endregion

            #region Course Exam----------
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.CourseDurationFrom = null;
                                    appl.CourseDurationTo = null;
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    if (CommonFunctions.IsDemandNoteCancellable(appl.DemandNoteID.HasValue ? appl.DemandNoteID.Value : 0))
                                    {
                                        appl.DemandNoteID = null;
                                    }
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };
            }
            #endregion
			#region Project Exam----------
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                                if (appl != null)
                                {
                                    appl.CourseDurationFrom = null;
                                    appl.CourseDurationTo = null;
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    if (CommonFunctions.IsDemandNoteCancellable(appl.DemandNoteID.HasValue ? appl.DemandNoteID.Value : 0))
                                    {
                                        appl.DemandNoteID = null;
                                    }
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                };
            }
            #endregion

            ShowAlert(verifiedCount.ToString() + " applications marked as Not Verified (sent to pending for verfication)", true);
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCPayment_Click(object sender, EventArgs e)
    {
        try
        {
            Int16 FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre = Convert.ToInt16(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
            Int16 FeePaidByInstituteButApplicationPendingToDispatchToNIELIT = Convert.ToInt16(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
            Int16 FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication = Convert.ToInt16(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 CourseCategoryID = Convert.ToInt32(Request.QueryString["CourseCategoryID"]);
            String ServiceID = Request.QueryString["ServiceID"];

            // Added by Amit start
            Int32 Paid = Convert.ToInt32(enmPaymentStatus.Paid);

            Int32 appCount = 0;
            List<Int64> applist = new List<Int64>();

            for (int i = 0; i < gbbatch.Rows.Count; i++)
            {
                CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                if (cbx != null)
                {
                    if (cbx.Checked)
                    { applist.Add(Convert.ToInt64(gbbatch.DataKeys[i].Values[0])); }
                }
            }
            applist.TrimExcess();
            appCount = applist.Count();

            if (appCount > 0)
            {
                if (!CheckDuplicateNew(ApplTypeID, applist))
                {
                    ShowAlert("Some of the applications have Demand Note-Id or final-submitted. Please check it...");
                    return;
                }

                DemandNote demandNote;
                using (EConnectContext context = new EConnectContext())
                {

                    demandNote = new DemandNote();
                    demandNote.ApplicationDate = DateTime.Now;
                    demandNote.ApplicationTypeID = ApplTypeID;
                    demandNote.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    demandNote.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Multiple);
                    demandNote.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                    demandNote.FeeTypeID = (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication)) ? Convert.ToInt32(enmFeeType.RegistrationFee) : Convert.ToInt32(enmFeeType.ExaminationFee);
                    demandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    demandNote.Amount = 0;
                    demandNote.CourseID = CourseID;
                    demandNote.CourseCategoryID = CourseCategoryID;
                    demandNote.ServiceID = ServiceID;
                    context.DemandNotes.Add(demandNote);
                    context.SaveChanges();

                    #region Coures Registration---------------
                    if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            IQueryable<CourseRegistrationApplication> tempAppl = context.CourseRegistrationApplications.Where(s => applist.Contains(s.ID));
                            try
                            {
                                foreach (CourseRegistrationApplication appl in tempAppl)
                                {
                                    //              Added_For_UP_Project_on    13 Dec 2024
                                    Int32? projectid = (from m in context.CourseRegistrationApplications where m.ID == appl.ID && m.CourseID == appl.CourseID select (Int32?)m.projectID).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(projectid.ToString()))
                                    {

                                        var feeAmount = (from c in context.CourseRegistrationApplications
                                                         join f in context.FeeDetails on c.projectID equals f.projectid
                                                         where c.ID == appl.ID  
                                                         select (decimal?)f.FeeAmount).FirstOrDefault();    //Changed_As_Return_0_As_Default 
                                        if (feeAmount == 0)
                                        {
                                            appl.ApplicationStatusID = FeePaidByInstituteButApplicationPendingToDispatchToNIELIT;
                                            appl.PaymentStatusID = Paid;
                                            appl.DemandNoteID = demandNote.ID;

                                            //demandNote.Amount += appl.FeeAmount.Value;
                                            demandNote.Amount += feeAmount.Value;                       //Changed_As_Fee_For_Specific_Project
                                            demandNote.FeeTypeID = appl.FeeTypeID.Value;
                                        }
                                        else
                                        {
                                            appl.ApplicationStatusID = FeePaidByInstituteButApplicationPendingToDispatchToNIELIT;
                                            appl.DemandNoteID = demandNote.ID;

                                            demandNote.Amount += appl.FeeAmount.Value;
                                            demandNote.FeeTypeID = appl.FeeTypeID.Value;
                                        }
                                    }
                                    else
                                    {
                                        appl.ApplicationStatusID = FeePaidByInstituteButApplicationPendingToDispatchToNIELIT;
                                        appl.DemandNoteID = demandNote.ID;
                                        
                                        demandNote.Amount += appl.FeeAmount.Value;
                                        demandNote.FeeTypeID = appl.FeeTypeID.Value;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Some error occurred. Please try again.");
                                return;
                            }
                            scope.Complete();
                        };
                    }
                    #endregion

                    #region Certificate Exam Application----------
                    else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                IQueryable<CertificateExamApplication> tempAppl = context.CertificateExamApplications.Where(s => applist.Contains(s.ID));

                                foreach (CertificateExamApplication appl in tempAppl)
                                {
                                    appl.ApplicationStatusID = FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre;
                                    appl.DemandNoteID = demandNote.ID;

                                    demandNote.Amount += appl.FeeAmount.Value;
                                    demandNote.FeeTypeID = appl.FeeTypeID.Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Some error occurred. Please try again.");
                                return;
                            }
                            scope.Complete();
                        };
                    }
                    #endregion

                    #region Course Exam---------------
                    else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            IQueryable<CourseExamApplication> tempAppl = context.CourseExamApplications.Where(s => applist.Contains(s.ID));
                            try
                            {
                                foreach (CourseExamApplication appl in tempAppl)
                                {
                                    appl.ApplicationStatusID = FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication;
                                    appl.DemandNoteID = demandNote.ID;

                                    demandNote.Amount += appl.FeeAmount;
                                    demandNote.FeeTypeID = appl.FeeTypeID.Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Some error occurred. Please try again.");
                                return;
                            }
                            scope.Complete();
                        };
                    }
                    #endregion
					 #region Project Exam---------------
                    else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            IQueryable<CourseProjectApplication> tempAppl = context.CourseProjectApplications.Where(s => applist.Contains(s.ID));
                            try
                            {
                                foreach (CourseProjectApplication appl in tempAppl)
                                {
                                    appl.ApplicationStatusID = FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication;
                                    appl.DemandNoteID = demandNote.ID;

                                    demandNote.Amount += appl.FeeAmount;
                                    demandNote.FeeTypeID = appl.FeeTypeID.Value;


                                    //if (appl.enmApplicantType == enmApplicantType.Institute && appl.enmPaymentSource == enmPaymentSource.Institute)
                                    //{                                     
                                     
                                    //    var lastDateI = (from r in context.RegistrationDetails
                                    //                     where r.CourseID == 1213 &&
                                    //                     (from p in context.CourseProjectApplications
                                    //                      where p.RegistrationNumber == r.RegistrationNo &&
                                    //                      p.CourseID == r.CourseID && p.DemandNoteID == appl.DemandNoteID
                                    //                      select 1).Any()
                                    //                     select r.ValidUptoDate).Min();

                                    //    appl.DemandNoteValiditydateupto = lastDateI;

                                    //}


                                }
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Some error occurred. Please try again.");
                                return;
                            }
                            scope.Complete();
                        };
                    }
                    #endregion

                    context.SaveChanges();
                };
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmConfirm.aspx?TypeID=" + ApplTypeID.ToString() + "&Appid=0&DemandID=" + demandNote.ID.ToString()));
            }
            else { BindGridView(); }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnSubmits_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int64 demandNoteID = 0;
            Int16 dispatchCount = 0;
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                if (!CheckForwardDate())
                {
                    ShowAlert("You cannot Dispatch Applications to NIELIT Centre, because Last Date to Dispatch Applications to NIELIT Centre is over..... ");
                    return;
                }
            }
            Int16 ApplicationDispatchedByTheInstituteToRegionalCentre = Convert.ToInt16(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
            Int16 ApplicationDispatchedByTheInstituteToNIELIT = Convert.ToInt16(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
            Int16 ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplicationStatus = Convert.ToInt16(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
            using (EConnectContext context = new EConnectContext())
            {
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chkchild");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            demandNoteID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            {
                                var application = (from c in context.CourseRegistrationApplications
                                                   where c.DemandNoteID == demandNoteID
                                                   select c).ToList();
                                if (application.Count() > 0)
                                {
                                    foreach (var app in application)
                                    {
                                        dispatchCount += 1;
                                        app.ApplicationStatusID = ApplicationDispatchedByTheInstituteToNIELIT;
                                    }
                                }
                            }
                            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                            {
                                var application = (from c in context.CertificateExamApplications
                                                   where c.DemandNoteID == demandNoteID
                                                   select c).ToList();
                                if (application.Count() > 0)
                                {
                                    foreach (var app in application)
                                    {
                                        dispatchCount += 1;
                                        app.ApplicationStatusID = ApplicationDispatchedByTheInstituteToRegionalCentre;
                                    }
                                    //context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                var application = (from c in context.CourseExamApplications
                                                   where c.DemandNoteID == demandNoteID
                                                   select c).ToList();
                                if (application.Count() > 0)
                                {
                                    foreach (var app in application)
                                    {
                                        dispatchCount += 1;
                                        app.ApplicationStatusID = ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplicationStatus;
                                        app.FormForwardedByInstituteOn = DateTime.Now;
                                    }
                                }
                            }
							 else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                            {
                                var application = (from c in context.CourseProjectApplications
                                                   where c.DemandNoteID == demandNoteID
                                                   select c).ToList();
                                if (application.Count() > 0)
                                {
                                    foreach (var app in application)
                                    {
                                        dispatchCount += 1;
                                        app.ApplicationStatusID = ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplicationStatus;
                                        app.FormForwardedByInstituteOn = DateTime.Now;
                                    }
                                }
                            }
                        }
                    }
                }
                context.SaveChanges();
            };
            ShowAlert(dispatchCount.ToString() + " applications have been sent to concerned centres", true);
            BindGridViewMain();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            //String emailaddress = "";
            Int64 applID = 0;
            List<StudentList> rejList = new List<StudentList>();
            Int16 verifiedCount = 0;
            Int16 ApplicationStatusIdcertificate = Convert.ToInt16(enmCertificateExamApplicationStatus.ApplicationRejectedbyInstitute);
            Int16 ApplicationStatusIdcourseexam = Convert.ToInt16(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute);
            Int16 ApplicationStatusIdcoursereg = Convert.ToInt16(enmCourseApplicationStatus.ApplicationRejectedbyInstitute);
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);

            #region Course Registration Application--
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {

                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);

                                //Added_UP_Project_02_09_2024
                                DropDownList drProjectId = (DropDownList)gbbatch.Rows[i].FindControl("ddlProjectId");

                                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                if (appl != null)
                                {
                                    StudentList st = new StudentList();
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusIdcoursereg;

                                    //Added_UP_Project_02_09_2024
                                    appl.projectID = Convert.ToInt32(drProjectId.SelectedItem.Value);



                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                    st.AppDate = appl.ApplicationDate;
                                    st.AppName = appl.Name;
                                    st.AppNo = appl.Number;
                                    st.CourseCode = appl.Course.Code;
                                    st.InstituteName = appl.Institute.Name;
                                    st.Salutation = appl.Salutation;
                                    st.Email = appl.EmailAddress;
                                    rejList.Add(st);
                                }
                            }
                        }
                    }
                    context.SaveChanges();

                    for (int i = 0; i < rejList.Count(); i++)
                    {
                        string msg = "Dear " + GetInitCap(rejList[i].Salutation + " " + rejList[i].AppName) + ",<br/><br/>" + " Your online application number:- " + rejList[i].AppNo + " dated " + rejList[i].AppDate.ToString("dd-MMM-yyyy") + " for registration in " + "<b style='color:red'>'" + rejList[i].CourseCode + "'</b>" + " level has been rejected by your Institute <b style='color:red'> " + rejList[i].InstituteName + " </b> " +
                             " due to some deficiencies found in your application form. Kindly contact your Institute to process your application form.";
                        try
                        {
                            //sending Email 
                            if (rejList[i].Email.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("NIELIT: Online Registration Application Rejected For O/A/B/C Level ", msg.ToString(), rejList[i].Email);
                                mail.Send();
                            }
                        }
                        catch (Exception ex)
                        {
                            // throw ex;
                        }
                    }
                };
            }
            #endregion

            #region Certificate Exam Application--
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CertificateExamApplication appl = context.CertificateExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    StudentList st = new StudentList();
                                    appl.CourseDurationFrom = null;
                                    appl.CourseDurationTo = null;
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusIdcertificate;
                                    appl.FinalSubmissionDate = null;
                                    appl.FinalSubmitted = false;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                    st.AppDate = appl.ApplicationDate;
                                    st.AppName = appl.Name;
                                    st.AppNo = appl.Number;
                                    st.CourseCode = appl.Course.Code;
                                    st.InstituteName = appl.Institute.Name;
                                    st.Salutation = appl.Salutation;
                                    st.Email = appl.EmailAddress;
                                    rejList.Add(st);

                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    for (int i = 0; i < rejList.Count(); i++)
                    {
                        string msg = "Dear " + GetInitCap(rejList[i].Salutation + " " + rejList[i].AppName) + ",<br/><br/>" + " Your online application number:- " + rejList[i].AppNo + " dated " + rejList[i].AppDate.ToString("dd-MMM-yyyy") + " for " + "<b style='color:red'>'" + rejList[i].CourseCode + "'</b>" + " examination has been rejected by your Institute <b style='color:red'> " + rejList[i].InstituteName + " </b> " +
                             " due to some deficiencies found in your application form. Kindly contact your Institute to process your application form.";
                        try
                        {
                            //sending Email 
                            if (rejList[i].Email.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("NIELIT: Online Examination Application Rejected For BCC/CCC ", msg.ToString(), rejList[i].Email);
                                mail.Send();
                            }
                        }
                        catch (Exception ex)
                        {
                            // throw ex;
                        }
                    }
                };
            }
            #endregion

            #region Course Exam Application--
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    StudentList st = new StudentList();
                                    appl.CourseDurationFrom = null;
                                    appl.CourseDurationTo = null;
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusIdcourseexam;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                    st.AppDate = appl.ApplicationDate;
                                    st.AppName = appl.Candidate.Name;
                                    st.AppNo = appl.Number;
                                    st.CourseCode = appl.Course.Code;
                                    st.InstituteName = appl.Institute.Name;
                                    st.Salutation = appl.Candidate.Salutation;
                                    st.Email = appl.Candidate.ContactDetails.FirstOrDefault().EmailAddress;
                                    rejList.Add(st);
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    for (int i = 0; i < rejList.Count(); i++)
                    {
                        string msg = "Dear " + GetInitCap(rejList[i].Salutation + " " + rejList[i].AppName) + ",<br/><br/>" + " Your online application number:- " + rejList[i].AppNo + " dated " + rejList[i].AppDate.ToString("dd-MMM-yyyy") + " for " + "<b style='color:red'>'" + rejList[i].CourseCode + "'</b>" + " level examination has been rejected by your Institute <b style='color:red'> " + rejList[i].InstituteName + " </b> " +
                             " due to some deficiencies found in your application form. Kindly contact your Institute to process your application form.";
                        try
                        {
                            //sending Email 
                            if (rejList[i].Email.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("NIELIT: Online Examination Application Rejected For O/A/B/C Level ", msg.ToString(), rejList[i].Email);
                                mail.Send();
                            }
                        }
                        catch (Exception ex)
                        {
                            // throw ex;
                        }
                    }
                };
            }
            #endregion
			
			 #region Course Project Application--
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                                applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                                if (appl != null)
                                {
                                    StudentList st = new StudentList();
                                    appl.CourseDurationFrom = null;
                                    appl.CourseDurationTo = null;
                                    appl.IsVerifiedByInstitute = false;
                                    appl.DateOfVerificationByInstitute = null;
                                    appl.ApplicationStatusID = ApplicationStatusIdcourseexam;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                    st.AppDate = appl.ApplicationDate;
                                    st.AppName = appl.Candidate.Name;
                                    st.AppNo = appl.Number;
                                    st.CourseCode = appl.Course.Code;
                                    st.InstituteName = appl.Institute.Name;
                                    st.Salutation = appl.Candidate.Salutation;
                                    st.Email = appl.Candidate.ContactDetails.FirstOrDefault().EmailAddress;
                                    rejList.Add(st);
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    for (int i = 0; i < rejList.Count(); i++)
                    {
                        string msg = "Dear " + GetInitCap(rejList[i].Salutation + " " + rejList[i].AppName) + ",<br/><br/>" + " Your online application number:- " + rejList[i].AppNo + " dated " + rejList[i].AppDate.ToString("dd-MMM-yyyy") + " for " + "<b style='color:red'>'" + rejList[i].CourseCode + "'</b>" + " level examination has been rejected by your Institute <b style='color:red'> " + rejList[i].InstituteName + " </b> " +
                             " due to some deficiencies found in your application form. Kindly contact your Institute to process your application form.";
                        try
                        {
                            //sending Email 
                            if (rejList[i].Email.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("NIELIT: Online Examination Application Rejected For O/A/B/C Level ", msg.ToString(), rejList[i].Email);
                                mail.Send();
                            }
                        }
                        catch (Exception ex)
                        {
                            // throw ex;
                        }
                    }
                };
            }
            #endregion

            ShowAlert(verifiedCount.ToString() + " applications marked as rejected by institute", true);
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
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                CheckBox headerchk = (CheckBox)gvMain.HeaderRow.FindControl("chkheader");
                CheckBox childchk = (CheckBox)e.Row.FindControl("chkchild");
                childchk.Attributes.Add("onclick", "javascript:Selectchildcheckboxes('" + headerchk.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnShowAppl_Click(object sender, EventArgs e)
    {
        BindGridView();
        gvMain.Visible = false;
        PagingBar2.Visible = false;
        gbbatch.Visible = true;
        PagingBar1.Visible = true;
        btnShowAppl.Visible = false;
        btnSubmits.Visible = false;
    }
    protected void btnShowDemandNote_Click(object sender, EventArgs e)
    {
        btnShowDemandNote.Visible = false;
        btnShowAppl.Visible = true;
        btnSubmits.Visible = true;
        gvMain.Visible = true;
        PagingBar2.Visible = true;
        gbbatch.Visible = false;
        PagingBar1.Visible = false;
        lblErrorMsg.Visible = false;
        BindGridViewMain();
    }
    #endregion------Events-------------------------

    #region--Private Methods---
    private bool CheckDuplicateNew(int ApplTypeID, List<long> applist)
    {
        try
        {
            #region Course Registration Application
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    IQueryable<CourseRegistrationApplication> tempAppl = context.CourseRegistrationApplications.Where(s => applist.Contains(s.ID));
                    foreach (CourseRegistrationApplication appl in tempAppl)
                    {
                        if (appl.FinalSubmitted == false)
                        {
                            ShowAlert("AppLication No- " + appl.Number + "is not final-Submitted");
                            return false;
                        }
                        else if (appl.DemandNoteID.HasValue)
                        {
                            //Changed 6 June 2020
                            // ShowAlert("AppLication No- " + appl.Number + "has Demand_Note_Id");
                            ShowAlert("AppLication No- " + appl.Number + "has already Demand_Note_Id " + appl.DemandNoteID.Value.ToString());
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Certificate Exam Application
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    IQueryable<CertificateExamApplication> tempAppl = context.CertificateExamApplications.Where(s => applist.Contains(s.ID));
                    foreach (CertificateExamApplication appl in tempAppl)
                    {
                        if (appl.FinalSubmitted == false)
                        {
                            ShowAlert("AppLication No- " + appl.Number + "is not final-Submitted");
                            return false;
                        }
                        else if (appl.DemandNoteID.HasValue)
                        {
                            //Changed 6 June 2020
                           // ShowAlert("AppLication No- " + appl.Number + "has Demand_Note_Id");
                            ShowAlert("AppLication No- " + appl.Number + "has already Demand_Note_Id " + appl.DemandNoteID.Value.ToString());
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Course Exam Application
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    IQueryable<CourseExamApplication> tempAppl = context.CourseExamApplications.Where(s => applist.Contains(s.ID));

                    foreach (CourseExamApplication appl in tempAppl)
                    {
                        if (appl.FinalSubmitted == false)
                        {
                            ShowAlert("AppLication No- " + appl.Number + "is not final-Submitted");
                            return false;
                        }
                        else if (appl.DemandNoteID.HasValue)
                        {
                            //Changed 6 June 2020
                            // ShowAlert("AppLication No- " + appl.Number + "has Demand_Note_Id");
                            ShowAlert("AppLication No- " + appl.Number + "has already Demand_Note_Id " + appl.DemandNoteID.Value.ToString());
                            return false;
                        }
                    }
                }
            }
            #endregion

            return true;
        }
        catch (Exception ex)
        { return false; }
    }

    protected void BindGridViewMain()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 DemandNodeTypeID = Convert.ToInt32(enmDemandNoteType.Multiple);
                Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                Int32 PaidButNotVerified = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                Int32 Paid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 statusID = 0;
                Int32 statusID1 = 0;
                Int32 statusID2 = 0;
                //for course exam application institute verification
                Int32 ApplicationVerifiedByInstitute = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute);
                if (Request.QueryString["Status"].ToString().Contains(","))
                {
                    String[] status = Request.QueryString["Status"].ToString().Split(',');
                    statusID1 = Convert.ToInt32(status[0].ToString());
                    statusID2 = Convert.ToInt32(status[1].ToString());
                }
                else
                    statusID = Convert.ToInt32(Request.QueryString["Status"]);

                #region Certificate Exam Application-----
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var DemadNote = (from s in context.DemandNotes
                                     join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID
                                     where c.InstituteID == entityID &&
                                           c.ApplicantTypeID == applicantType && c.CourseID == CourseID && c.ApplicationStatusID == statusID
                                           && (c.PaymentStatusID == Paid || c.PaymentStatusID == PaidButNotVerified)
                                     select new
                                     {
                                         ID = s.ID,
                                         DemandNo = s.ID,
                                         DemandDate = s.ApplicationDate,
                                         applCount = (from r in context.CertificateExamApplications
                                                      where r.DemandNoteID == s.ID
                                                      select r).Count(),
                                         PaymentMode = s.PaymentMode.Name,
                                         PaymentModeID = s.PaymentModeID,
                                         PaymentStatus = s.PaymentStatus.Name,
                                         PaymentStatusID = s.PaymentStatusID,
                                         Amount = s.Amount
                                     }).Distinct();

                    if (DemadNote.Count() > 0)
                    {
                        PagingBar2.Bind(DemadNote, ref gvMain);
                        //uPnlGridSummery.Update();

                        btnSubmits.Visible = true;
                        btnShowAppl.Visible = true;
                        gvMain.Visible = true;
                        PagingBar2.Visible = true;
                        gbbatch.Visible = false;
                        PagingBar1.Visible = false;
                    }
                    else
                    {
                        //if (gvMain.Rows.Count <= 0)
                        //{
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                        btnSubmits.Visible = false;
                        gvMain.Visible = false;
                        PagingBar2.Visible = false;
                        //}
                    }
                }
                #endregion

                #region Course Registration Application-----
                else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    var DemadNote = (from s in context.DemandNotes
                                     join c in context.CourseRegistrationApplications on s.ID equals c.DemandNoteID
                                     where c.InstituteID == entityID &&
                                           c.ApplicantTypeID == applicantType && c.CourseID == CourseID &&
                                             (c.ApplicationStatusID == statusID1 || c.ApplicationStatusID == statusID2)
                                           && (c.PaymentStatusID == Paid || c.PaymentStatusID == PaidButNotVerified)
                                     select new
                                     {
                                         ID = s.ID,
                                         DemandNo = s.ID,
                                         DemandDate = s.ApplicationDate,
                                         applCount = (from r in context.CourseRegistrationApplications
                                                      where r.DemandNoteID == s.ID
                                                      select r).Count(),
                                         PaymentMode = s.PaymentMode.Name,
                                         PaymentModeID = s.PaymentModeID,
                                         PaymentStatus = s.PaymentStatus.Name,
                                         PaymentStatusID = s.PaymentStatusID,
                                         Amount = s.Amount
                                     }).Distinct();
                    if (DemadNote.Count() > 0)
                    {
                        PagingBar2.Bind(DemadNote, ref gvMain);
                        //uPnlGridSummery.Update();

                        btnSubmits.Visible = true;
                        btnShowAppl.Visible = true;
                        gvMain.Visible = true;
                        PagingBar2.Visible = true;
                        gbbatch.Visible = false;
                        PagingBar1.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                        btnSubmits.Visible = false;
                        gvMain.Visible = false;
                        PagingBar2.Visible = false;
                    }
                }
                #endregion

                #region Course Exam Application-----
                else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    var DemadNote = (from s in context.DemandNotes
                                     join c in context.CourseExamApplications on s.ID equals c.DemandNoteID
                                     where c.InstituteID == entityID &&
                                           c.ApplicantTypeID == applicantType && c.CourseID == CourseID &&
                                           (c.ApplicationStatusID == statusID1 || c.ApplicationStatusID == statusID2)
                                           && (c.PaymentStatusID == Paid || c.PaymentStatusID == PaidButNotVerified)
                                     select new
                                     {
                                         ID = s.ID,
                                         DemandNo = s.ID,
                                         DemandDate = s.ApplicationDate,
                                         applCount = (from r in context.CourseExamApplications
                                                      where r.DemandNoteID == s.ID
                                                      select r).Count(),
                                         PaymentMode = s.PaymentMode.Name,
                                         PaymentModeID = s.PaymentModeID,
                                         PaymentStatus = s.PaymentStatus.Name,
                                         PaymentStatusID = s.PaymentStatusID,
                                         Amount = s.Amount
                                     }).Distinct();
                    if (DemadNote.Count() > 0)
                    {
                        PagingBar2.Bind(DemadNote, ref gvMain);
                        //uPnlGridSummery.Update();

                        btnSubmits.Visible = true;
                        btnShowAppl.Visible = true;
                        gvMain.Visible = true;
                        PagingBar2.Visible = true;
                        gbbatch.Visible = false;
                        PagingBar1.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                        btnSubmits.Visible = false;
                        gvMain.Visible = false;
                        PagingBar2.Visible = false;
                    }
                }
                #endregion
				   #region Course Project Application-----
                else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                {
                    var DemadNote = (from s in context.DemandNotes
                                     join c in context.CourseProjectApplications on s.ID equals c.DemandNoteID
                                     where c.InstituteID == entityID &&
                                           c.ApplicantTypeID == applicantType && c.CourseID == CourseID &&
                                           (c.ApplicationStatusID == statusID1 || c.ApplicationStatusID == statusID2)
                                           && (c.PaymentStatusID == Paid || c.PaymentStatusID == PaidButNotVerified)
                                     select new
                                     {
                                         ID = s.ID,
                                         DemandNo = s.ID,
                                         DemandDate = s.ApplicationDate,
                                         applCount = (from r in context.CourseExamApplications
                                                      where r.DemandNoteID == s.ID
                                                      select r).Count(),
                                         PaymentMode = s.PaymentMode.Name,
                                         PaymentModeID = s.PaymentModeID,
                                         PaymentStatus = s.PaymentStatus.Name,
                                         PaymentStatusID = s.PaymentStatusID,
                                         Amount = s.Amount
                                     }).Distinct();
                    if (DemadNote.Count() > 0)
                    {
                        PagingBar2.Bind(DemadNote, ref gvMain);
                        //uPnlGridSummery.Update();

                        btnSubmits.Visible = true;
                        btnShowAppl.Visible = true;
                        gvMain.Visible = true;
                        PagingBar2.Visible = true;
                        gbbatch.Visible = false;
                        PagingBar1.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                        btnSubmits.Visible = false;
                        gvMain.Visible = false;
                        PagingBar2.Visible = false;
                    }
                }
                #endregion
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected Boolean CheckDuplicate(int ApplTypeID)
    {
        try
        {
            //Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);

            #region Course Registration Application---
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                Int64 applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                if (appl != null)
                                {
                                    if (appl.DemandNoteID.HasValue)
                                        return false;
                                    if (appl.FinalSubmitted == false)
                                        return false;
                                }
                            }
                        }
                    }
                };
            }
            #endregion

            #region Certificate Exam Application--
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                Int64 applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CertificateExamApplication appl = context.CertificateExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    if (appl.DemandNoteID.HasValue)
                                        return false;
                                    //if (appl.FinalSubmitted == false)
                                    //    return false;
                                }
                            }
                        }
                    }
                };
            }
            #endregion

            #region Course Exam Application--
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < gbbatch.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                Int64 applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                if (appl != null)
                                {
                                    if (appl.DemandNoteID.HasValue)
                                        return false;
                                    if (appl.FinalSubmitted == false)
                                        return false;
                                }
                            }
                        }
                    }
                };
            }
            #endregion

            #region Comment
            //if (cbx != null)
            //{
            //    if (cbx.Checked)
            //    {
            //        Int64 applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
            //        using (EConnectContext context = new EConnectContext())
            //        {
            //            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            //            {
            //                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
            //                if (appl != null)
            //                    if (appl.DemandNoteID.HasValue)
            //                        return false;
            //            }
            //            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            //            {
            //                CertificateExamApplication appl = context.CertificateExamApplications.Find(applID);
            //                if (appl != null)
            //                    if (appl.DemandNoteID.HasValue)
            //                        return false;
            //            }
            //            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            //            {
            //                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
            //                if (appl != null)
            //                    if (appl.DemandNoteID.HasValue)
            //                        return false;
            //            }
            //        };
            //    }
            //}
            //} 
            #endregion

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    protected Boolean CheckForwardDate()
    {
        try
        {
            Int64 demandNoteID = 0;
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            for (int i = 0; i < gvMain.Rows.Count; i++)
            {
                CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chkchild");
                if (cbx != null)
                {
                    if (cbx.Checked)
                    {
                        demandNoteID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                        using (EConnectContext context = new EConnectContext())
                        {
                            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                var payee = (from p in context.CourseExamApplications
                                             where p.DemandNoteID == demandNoteID
                                             select new
                                             {
                                                 appl = p,
                                             }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                                    if (payee.appl.LateFeeImposed)
                                    {
                                        submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                    }
                                    DateTime lastDate = (from c in context.CutOffDates
                                                         where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                         select c.EfferctiveDate).FirstOrDefault();
                                    Int32 NeftExtensionPeriod = payee.appl.Exam.FeeSubmissioInstituteExtPeriod;

                                    if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                    {
                                        return false;
                                    }
                                }
                            }

                        };
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    protected Boolean CheckCutoffDate(int ExamID)
    {
        try
        {
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                Int32 NeftExtensionPeriod = context.Exams.Find(ExamID).FeeSubmissioInstituteExtPeriod;

                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    if (ExamID > 0)
                    {
                        DateTime lastDate = (from c in context.CutOffDates
                                             where c.ActivityID == submisssionDateActivityID && c.ExamID == ExamID && c.ApplicantTypeID == applicantType
                                             select c.EfferctiveDate).FirstOrDefault();
                        if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                        { return false; }
                    }
                    else { return false; }
                }
                else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    if (ExamID > 0)
                    {
                        DateTime lastDate = (from c in context.CutOffDates
                                             where c.ActivityID == submisssionDateActivityID && c.ExamID == ExamID && c.ApplicantTypeID == applicantType
                                             select c.EfferctiveDate).FirstOrDefault();
                        if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                        { return false; }
                    }
                    else { return false; }
                }
            };
            return true;
        }
        catch (Exception ex)
        { return false; }
    }
    protected Boolean CheckFinalSubmission(int ApplTypeID)
    {
        try
        {
            for (int i = 0; i < gbbatch.Rows.Count; i++)
            {
                CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                //Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                if (cbx != null)
                {
                    if (cbx.Checked)
                    {
                        Int64 applID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                        using (EConnectContext context = new EConnectContext())
                        {
                            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            {
                                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                if (appl != null)
                                    if (appl.FinalSubmitted == false)
                                        return false;
                            }
                            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                            {
                                CertificateExamApplication appl = context.CertificateExamApplications.Find(applID);
                                if (appl != null)
                                    if (appl.FinalSubmitted == false)
                                        return false;
                            }
                            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                if (appl != null)
                                    if (appl.FinalSubmitted == false)
                                        return false;
                            }
                        };
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    protected void ShowEditMode()
    {
        EConnectContext context = new EConnectContext();
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnMode.Visible = false;
            lblHeading.Text = "Application Status";
            //Updating breadscrumb
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            Int32 statusID = Convert.ToInt32(Request.QueryString["Status"]);
            if (statusID == 1)
            {
                btnVerify.Visible = true;
            }
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                Int64 ApplNo = Convert.ToInt64(Request.QueryString["ApplID"]);
                var cr = context.CourseRegistrationApplications.Find(ApplNo);
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));
            }
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                Int64 ApplNo = Convert.ToInt64(Request.QueryString["ApplID"]);
                var cr = context.CertificateExamApplications.Find(ApplNo);
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));
            }
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                Int64 ApplNo = Convert.ToInt64(Request.QueryString["ApplID"]);
                var cr = context.CourseExamApplications.Find(ApplNo);
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Candidate.Name, "", ""));
            }
			 if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
            {
                Int64 ApplNo = Convert.ToInt64(Request.QueryString["ApplID"]);
                var cr = context.CourseProjectApplications.Find(ApplNo);
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Candidate.Name, "", ""));
            }

            CourseApplication1.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            CourseApplication1.ApplicationId = Convert.ToInt64(Request.QueryString["ApplID"]);
            CourseApplication1.Visible = true;
            CourseApplication1.Bind();
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            Int32 statusID = 0;
            Int32 statusID1 = 0;
            Int32 statusID2 = 0;
            //for course exam application institute verification
            Int32 ApplicationVerifiedByInstitute = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute);
            if (Request.QueryString["Status"].ToString().Contains(","))
            {
                String[] status = Request.QueryString["Status"].ToString().Split(',');
                statusID1 = Convert.ToInt32(status[0].ToString());
                statusID2 = Convert.ToInt32(status[1].ToString());
            }
            else
                statusID = Convert.ToInt32(Request.QueryString["Status"]);

            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 ExamID = Convert.ToInt32(Request.QueryString["Examid"]);
            Int32 sts = Convert.ToInt32(enmPaymentStatus.Pending);
            Int32 PaidButNotVerified = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            using (EConnectContext context = new EConnectContext())
            {
                #region Course Registration---------------
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    if (statusID != 0)
                    {
                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)statusID;
                        var fillCourseRegistrationData = from s in context.CourseRegistrationApplications
                                                         where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID && s.ApplicableExamID == ExamID &&
                                                         s.FinalSubmitted == true && s.ApplicationStatusID == statusID
                                                         orderby s.ID
                                                         select new
                                                         {
                                                             ID = s.ID,
                                                             Appno = s.Number,
                                                             Name = s.Name.ToUpper(),
                                                             FatherName = s.FatherName.ToUpper(),
                                                             MotherName = s.MotherName.ToUpper(),
                                                             Appdate = s.ApplicationDate,
                                                             IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                             PaymentStatusID = s.PaymentStatusID,
                                                             CourseID = s.CourseID,
                                                             CourseCategoryID = s.CourseCategoryID,
                                                             ServiceID = s.Course.RegistrationServiceID,
                                                             ApplTypeID = ApplTypeID,
                                                             ApplicationStatusID = s.ApplicationStatusID,
                                                             FinalSubmitted = s.FinalSubmitted,
                                                             PaymentStatus = s.PaymentStatus.Name,
                                                             PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,

                                                             //Added_UP_Project_03_09_2024
                                                             ProjectId = s.projectID == null ? "Not Selected" : s.projectID.ToString(),


                                                             //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                             PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                             DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0
                                                         };
                        if (applStatus == enmCourseApplicationStatus.AppliedButPendingForInstituteVerification)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[6].Visible = false;
                            btnVerify.Visible = true;
                            // btnReject.Visible = true;
                        }
                        else if (applStatus == enmCourseApplicationStatus.ApplicationRejectedbyInstitute)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[6].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = true;
                            btnVerify.Visible = true;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                        }
                        else if (applStatus == enmCourseApplicationStatus.ApplicationVerifiedByInstitute)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[6].Visible = false;
                            //
                            btnVerify.Visible = false;
                            btnCPayment.Visible = true;
                            btnMNotVerify.Visible = true;
                        }
                        else if (applStatus == enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                        }
                        else if (applStatus == enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)
                        {
                            Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                            if (Request.QueryString["st"] != null)
                            {
                                fillCourseRegistrationData = fillCourseRegistrationData.Where(a => a.PaymentStatusID == sts);
                                gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                                btnSubmits.Visible = false;
                                lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                                lblError.Visible = true;
                                btnShowDemandNote.Visible = false;
                                lblErrorMsg.Visible = false;
                            }
                            else
                                fillCourseRegistrationData = fillCourseRegistrationData.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));
                        }

                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.ID);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.ID);
                                    break;
                                case "Appno":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.Appno);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.Appno);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.Name);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.Name);
                                    break;
                                case "FatherName":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.FatherName);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.FatherName);
                                    break;
                                case "MotherName":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.MotherName);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.MotherName);
                                    break;

                                case "Appdate":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.Appdate);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.Appdate);
                                    break;
                                default:
                                    fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(fillCourseRegistrationData, ref gbbatch);
                        Course cs = context.Courses.Find(CourseID);
                        if ((applStatus == enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (Request.QueryString["st"] != null))
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + " Fee Pending to be Paid by the Institute ", "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                        else
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                    }
                    else if (statusID1 != 0 && statusID2 != 0)
                    {
                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)statusID1;
                        enmCourseApplicationStatus applStatus1 = (enmCourseApplicationStatus)statusID2;

                        var fillCourseRegistrationData = from s in context.CourseRegistrationApplications
                                                         where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID && s.ApplicableExamID == ExamID &&
                                                         s.FinalSubmitted == true && (s.ApplicationStatusID == statusID1 || s.ApplicationStatusID == statusID2)
                                                         orderby s.ID
                                                         select new
                                                         {
                                                             ID = s.ID,
                                                             Appno = s.Number,
                                                             Name = s.Name.ToUpper(),
                                                             FatherName = s.FatherName.ToUpper(),
                                                             MotherName = s.MotherName.ToUpper(),
                                                             Appdate = s.ApplicationDate,
                                                             IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                             PaymentStatusID = s.PaymentStatusID,
                                                             CourseID = s.CourseID,
                                                             CourseCategoryID = s.CourseCategoryID,
                                                             ServiceID = s.Course.RegistrationServiceID,
                                                             ApplTypeID = ApplTypeID,
                                                             ApplicationStatusID = s.ApplicationStatusID,
                                                             FinalSubmitted = s.FinalSubmitted,
                                                             PaymentStatus = s.PaymentStatus.Name,
                                                             PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,

                                                             //Added_UP_Project_03_09_2024
                                                             ProjectId = s.projectID == null ? "Not Selected" : s.projectID.ToString(),


                                                             //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                             PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                             DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0
                                                         };

                        if (applStatus == enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT || applStatus1 == enmCourseApplicationStatus.ApplicationVerifiedByInstitute)
                        {
                            Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[8].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            //btnSubmits.Visible = true;
                            btnShowDemandNote.Visible = true;
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.Text = "Please click Show Demand Note for dispatching the applications.";
                            if (Request.QueryString["st"] != null)
                            {
                                fillCourseRegistrationData = fillCourseRegistrationData.Where(a => a.PaymentStatusID == sts);
                                gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                                btnSubmits.Visible = false;
                                lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                                lblError.Visible = true;
                                btnShowDemandNote.Visible = false;
                                lblErrorMsg.Visible = false;
                            }
                            else
                                fillCourseRegistrationData = fillCourseRegistrationData.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));

                        }
                        if (applStatus == enmCourseApplicationStatus.AppliedButPendingForInstituteVerification || applStatus1 == enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[6].Visible = false;
                            btnVerify.Visible = true;
                        }
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.ID);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.ID);
                                    break;
                                case "Appno":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.Appno);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.Appno);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.Name);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.Name);
                                    break;
                                case "FatherName":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.FatherName);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.FatherName);
                                    break;
                                case "MotherName":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.MotherName);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.MotherName);
                                    break;

                                case "Appdate":
                                    if (sortOrder == "DESC")
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderByDescending(s => s.Appdate);
                                    else
                                        fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.Appdate);
                                    break;
                                default:
                                    fillCourseRegistrationData = fillCourseRegistrationData.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(fillCourseRegistrationData, ref gbbatch);
                        Course cs = context.Courses.Find(CourseID);
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));

                    }
                }
                #endregion

                #region Certificate Exam-------------
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    enmCertificateExamApplicationStatus applStatus = (enmCertificateExamApplicationStatus)statusID;

                    var fillCertificationData = from s in context.CertificateExamApplications.Include("DemandNotes")
                                                where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID && s.ExamID == ExamID &&
                                                s.ApplicationStatusID == statusID
                                                orderby s.ID
                                                select new
                                                {
                                                    ID = s.ID,
                                                    Appno = s.Number,
                                                    Name = s.Name,
                                                    FatherName = s.FatherName,
                                                    MotherName = s.MotherName,
                                                    Appdate = s.ApplicationDate,
                                                    IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                    PaymentStatusID = s.PaymentStatusID,
                                                    CourseID = s.CourseID,
                                                    CourseCategoryID = s.CourseCategoryID,
                                                    ServiceID = s.Course.ExaminationServiceID,
                                                    ApplTypeID = ApplTypeID,
                                                    ApplicationStatusID = s.ApplicationStatusID,
                                                    FinalSubmitted = s.FinalSubmitted,
                                                    PaymentStatus = s.PaymentStatus.Name,
                                                    PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,
                                                    //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                    PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                    DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0,

                                                };

                    if (applStatus == enmCertificateExamApplicationStatus.ApplicationRejectedbyInstitute)
                        fillCertificationData = fillCertificationData.Where(a => a.FinalSubmitted == false);
                    else
                        fillCertificationData = fillCertificationData.Where(a => a.FinalSubmitted == true);

                    if (applStatus == enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification)
                    {
                        gbbatch.Columns[7].Visible = true;
                        gbbatch.Columns[6].Visible = false;
                        btnVerify.Visible = true;
                        //  btnReject.Visible = true;
                    }
                    else if (applStatus == enmCertificateExamApplicationStatus.ApplicationRejectedbyInstitute)
                    {

                        gbbatch.Columns[7].Visible = true;
                        gbbatch.Columns[6].Visible = false;
                        gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = true;
                        btnVerify.Visible = true;
                        btnCPayment.Visible = false;
                        btnMNotVerify.Visible = false;
                        btnReject.Visible = false;
                        btnSubmits.Visible = false;
                    }
                    else if (applStatus == enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute)
                    {
                        gbbatch.Columns[7].Visible = false;
                        gbbatch.Columns[6].Visible = false;
                        //
                        btnVerify.Visible = false;
                        btnCPayment.Visible = true;
                        btnMNotVerify.Visible = true;
                    }
                    else if (applStatus == enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)
                    {
                        Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                        gbbatch.Columns[7].Visible = false;
                        gbbatch.Columns[8].Visible = false;
                        btnVerify.Visible = false;
                        btnCPayment.Visible = false;
                        btnMNotVerify.Visible = false;
                        btnReject.Visible = false;
                        btnShowDemandNote.Visible = true;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Please click Show Demand Note for dispatching the applications.";
                        //btnSubmits.Visible = true;
                        if (Request.QueryString["st"] != null)
                        {
                            fillCertificationData = fillCertificationData.Where(a => a.PaymentStatusID == sts);
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                            btnSubmits.Visible = false;
                            lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                            lblError.Visible = true;
                            btnShowDemandNote.Visible = false;
                            lblErrorMsg.Visible = false;
                        }
                        else
                            fillCertificationData = fillCertificationData.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));
                    }
                    else if (applStatus == enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre)
                    {
                        gbbatch.Columns[7].Visible = false;
                        gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                        btnVerify.Visible = false;
                        btnCPayment.Visible = false;
                        btnMNotVerify.Visible = false;
                        btnReject.Visible = false;
                        btnSubmits.Visible = false;
                        btnShowDemandNote.Visible = false;
                    }


                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    fillCertificationData = fillCertificationData.OrderByDescending(s => s.ID);
                                else
                                    fillCertificationData = fillCertificationData.OrderBy(s => s.ID);
                                break;
                            case "Appno":
                                if (sortOrder == "DESC")
                                    fillCertificationData = fillCertificationData.OrderByDescending(s => s.Appno);
                                else
                                    fillCertificationData = fillCertificationData.OrderBy(s => s.Appno);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    fillCertificationData = fillCertificationData.OrderByDescending(s => s.Name);
                                else
                                    fillCertificationData = fillCertificationData.OrderBy(s => s.Name);
                                break;
                            case "FatherName":
                                if (sortOrder == "DESC")
                                    fillCertificationData = fillCertificationData.OrderByDescending(s => s.FatherName);
                                else
                                    fillCertificationData = fillCertificationData.OrderBy(s => s.FatherName);
                                break;
                            case "MotherName":
                                if (sortOrder == "DESC")
                                    fillCertificationData = fillCertificationData.OrderByDescending(s => s.MotherName);
                                else
                                    fillCertificationData = fillCertificationData.OrderBy(s => s.MotherName);
                                break;

                            case "Appdate":
                                if (sortOrder == "DESC")
                                    fillCertificationData = fillCertificationData.OrderByDescending(s => s.Appdate);
                                else
                                    fillCertificationData = fillCertificationData.OrderBy(s => s.Appdate);
                                break;
                            default:
                                fillCertificationData = fillCertificationData.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    PagingBar1.Bind(fillCertificationData, ref gbbatch);
                    //uPnlGrid.Update();
                    //uPnlNavigation.Update();
                    Course cs = context.Courses.Find(CourseID);

                    if ((applStatus == enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre) && (Request.QueryString["st"] != null))
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + " Fee Pending to be Paid by the Institute ", "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                    else
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                }
                #endregion

                #region Course Exam------------------
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {

                    if (statusID != 0)
                    {
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID;
                        var fillCourseExamApplication = from s in context.CourseExamApplications
                                                        where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID && s.ExamID == ExamID &&
                                                        s.FinalSubmitted == true && (s.ApplicationStatusID == statusID)
                                                        orderby s.ID
                                                        select new
                                                        {
                                                            ID = s.ID,
                                                            Appno = s.Number,
                                                            Name = s.Candidate.Name.ToUpper(),
                                                            FatherName = s.Candidate.FatherName.ToUpper(),
                                                            MotherName = s.Candidate.MotherName.ToUpper(),
                                                            Appdate = s.ApplicationDate,
                                                            IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                            PaymentStatusID = s.PaymentStatusID,
                                                            CourseID = s.CourseID,
                                                            CourseCategoryID = s.CourseCategoryID,
                                                            ServiceID = s.Course.ExaminationServiceID,
                                                            ApplTypeID = ApplTypeID,
                                                            ApplicationStatusID = s.ApplicationStatusID,
                                                            FinalSubmitted = s.FinalSubmitted,
                                                            PaymentStatus = s.PaymentStatus.Name,
                                                            PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,
                                                            //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                            PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                            DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0
                                                        };

                        if (applStatus == enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)
                        {
                            gbbatch.Columns[7].Visible = true;
                            gbbatch.Columns[6].Visible = false;
                            btnVerify.Visible = true;
                            // btnReject.Visible = true;
                        }
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)
                        {
                            gbbatch.Columns[7].Visible = true;
                            gbbatch.Columns[6].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = true;
                            btnVerify.Visible = true;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                        }
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[6].Visible = false;
                            //
                            btnVerify.Visible = false;
                            btnCPayment.Visible = true;
                            btnMNotVerify.Visible = true;
                            fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                        }
                        else if (applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)
                        {
                            Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[8].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = true;
                            if (Request.QueryString["st"] != null)
                            {
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                                gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                                btnSubmits.Visible = false;
                                lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                                lblError.Visible = true;
                                lblErrorMsg.Visible = false;
                            }
                            else
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));
                            //fillCourseExamApplication = fillCourseExamApplication.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified || (a.PaymentStatusID == PaidButNotVerified && a.ApplicationStatusID == ApplicationVerifiedByInstitute)));



                        }
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                        }

                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.ID);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                                case "Appno":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appno);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appno);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Name);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Name);
                                    break;
                                case "FatherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.FatherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.FatherName);
                                    break;
                                case "MotherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.MotherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.MotherName);
                                    break;

                                case "Appdate":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appdate);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appdate);
                                    break;
                                default:
                                    fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(fillCourseExamApplication, ref gbbatch);
                        //uPnlGrid.Update();
                        //uPnlNavigation.Update();
                        Course cs = context.Courses.Find(CourseID);
                        if ((applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (Request.QueryString["st"] != null))
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + " Fee Pending to be Paid by the Institute ", "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                        else
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                    }
                    else if (statusID1 != 0 && statusID2 != 0)
                    {
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID1;
                        enmCourseExamApplicationStatus applStatus1 = (enmCourseExamApplicationStatus)statusID2;

                        var fillCourseExamApplication = from s in context.CourseExamApplications
                                                        where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID && s.ExamID == ExamID &&
                                                        s.FinalSubmitted == true && (s.ApplicationStatusID == statusID1 || s.ApplicationStatusID == statusID2)
                                                        orderby s.ID
                                                        select new
                                                        {
                                                            ID = s.ID,
                                                            Appno = s.Number,
                                                            Name = s.Candidate.Name.ToUpper(),
                                                            FatherName = s.Candidate.FatherName.ToUpper(),
                                                            MotherName = s.Candidate.MotherName.ToUpper(),
                                                            Appdate = s.ApplicationDate,
                                                            IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                            PaymentStatusID = s.PaymentStatusID,
                                                            CourseID = s.CourseID,
                                                            CourseCategoryID = s.CourseCategoryID,
                                                            ServiceID = s.Course.ExaminationServiceID,
                                                            ApplTypeID = ApplTypeID,
                                                            ApplicationStatusID = s.ApplicationStatusID,
                                                            FinalSubmitted = s.FinalSubmitted,
                                                            PaymentStatus = s.PaymentStatus.Name,
                                                            PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,
                                                            PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                            //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                            DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0
                                                        };


                        if (applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT && applStatus1 == enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                        {
                            Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[8].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnShowDemandNote.Visible = true;
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.Text = "Please click Show Demand Note for dispatching the applications.";
                            //btnSubmits.Visible = true;
                            if (Request.QueryString["st"] != null)
                            {
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                                gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                                btnSubmits.Visible = false;
                                lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                                lblError.Visible = true;
                                btnShowDemandNote.Visible = false;
                                lblErrorMsg.Visible = false;
                            }
                            else
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));
                        }
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.ID);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                                case "Appno":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appno);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appno);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Name);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Name);
                                    break;
                                case "FatherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.FatherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.FatherName);
                                    break;
                                case "MotherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.MotherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.MotherName);
                                    break;

                                case "Appdate":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appdate);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appdate);
                                    break;
                                default:
                                    fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(fillCourseExamApplication, ref gbbatch);
                        //uPnlGrid.Update();
                        //uPnlNavigation.Update();
                        Course cs = context.Courses.Find(CourseID);
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                    }
                }
                #endregion
				#region Project Exam------------------
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                {

                    if (statusID != 0)
                    {
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID;
                        var fillCourseExamApplication = from s in context.CourseProjectApplications
                                                        where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID   && s.ExamID == ExamID &&
                                                        s.FinalSubmitted == true && (s.ApplicationStatusID == statusID)
                                                        orderby s.ID
                                                        select new
                                                        {
                                                            ID = s.ID,
                                                            Appno = s.Number,
                                                            Name = s.Candidate.Name.ToUpper(),
                                                            FatherName = s.Candidate.FatherName.ToUpper(),
                                                            MotherName = s.Candidate.MotherName.ToUpper(),
                                                            Appdate = s.ApplicationDate,
                                                            IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                            PaymentStatusID = s.PaymentStatusID,
                                                            CourseID = s.CourseID,
                                                            CourseCategoryID = s.CourseCategoryID,
                                                            ServiceID = s.Course.ProjectServiceID,
                                                            ApplTypeID = ApplTypeID,
                                                            ApplicationStatusID = s.ApplicationStatusID,
                                                            FinalSubmitted = s.FinalSubmitted,
                                                            PaymentStatus = s.PaymentStatus.Name,
                                                            PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,
                                                            //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                            PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                            DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0
                                                        };

                        if (applStatus == enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)
                        {
                            gbbatch.Columns[7].Visible = true;
                            gbbatch.Columns[6].Visible = false;
                            btnVerify.Visible = true;
                            // btnReject.Visible = true;
                        }
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)
                        {
                            gbbatch.Columns[7].Visible = true;
                            gbbatch.Columns[6].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = true;
                            btnVerify.Visible = true;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                        }
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[6].Visible = false;
                            //
                            btnVerify.Visible = false;
                            btnCPayment.Visible = true;
                            btnMNotVerify.Visible = true;
                            fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                        }
                        else if (applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)
                        {
                            Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[8].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = true;
                            if (Request.QueryString["st"] != null)
                            {
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                                gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                                btnSubmits.Visible = false;
                                lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                                lblError.Visible = true;
                                lblErrorMsg.Visible = false;
                            }
                            else
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));
                            //fillCourseExamApplication = fillCourseExamApplication.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified || (a.PaymentStatusID == PaidButNotVerified && a.ApplicationStatusID == ApplicationVerifiedByInstitute)));



                        }
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)
                        {
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnSubmits.Visible = false;
                        }

                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.ID);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                                case "Appno":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appno);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appno);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Name);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Name);
                                    break;
                                case "FatherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.FatherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.FatherName);
                                    break;
                                case "MotherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.MotherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.MotherName);
                                    break;

                                case "Appdate":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appdate);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appdate);
                                    break;
                                default:
                                    fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(fillCourseExamApplication, ref gbbatch);
                        //uPnlGrid.Update();
                        //uPnlNavigation.Update();
                        Course cs = context.Courses.Find(CourseID);
                        if ((applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (Request.QueryString["st"] != null))
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + " Fee Pending to be Paid by the Institute ", "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                        else
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                    }
                    else if (statusID1 != 0 && statusID2 != 0)
                    {
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID1;
                        enmCourseExamApplicationStatus applStatus1 = (enmCourseExamApplicationStatus)statusID2;

                        var fillCourseExamApplication = from s in context.CourseProjectApplications
                                                        where s.ApplicantTypeID == applicantType && s.CourseID == CourseID && s.InstituteID == entityID && s.ExamID == ExamID &&
                                                        s.FinalSubmitted == true && (s.ApplicationStatusID == statusID1 || s.ApplicationStatusID == statusID2)
                                                        orderby s.ID
                                                        select new
                                                        {
                                                            ID = s.ID,
                                                            Appno = s.Number,
                                                            Name = s.Candidate.Name.ToUpper(),
                                                            FatherName = s.Candidate.FatherName.ToUpper(),
                                                            MotherName = s.Candidate.MotherName.ToUpper(),
                                                            Appdate = s.ApplicationDate,
                                                            IsVerifiedByInstitute = s.IsVerifiedByInstitute,
                                                            PaymentStatusID = s.PaymentStatusID,
                                                            CourseID = s.CourseID,
                                                            CourseCategoryID = s.CourseCategoryID,
                                                            ServiceID = s.Course.ExaminationServiceID,
                                                            ApplTypeID = ApplTypeID,
                                                            ApplicationStatusID = s.ApplicationStatusID,
                                                            FinalSubmitted = s.FinalSubmitted,
                                                            PaymentStatus = s.PaymentStatus.Name,
                                                            PaymentMode = s.DemandNote.PaymentMode.Name == null ? "Not Selected" : s.DemandNote.PaymentMode.Name,
                                                            PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                                            //PaymentDetails = s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                                            DemandNoteNo = s.DemandNote != null ? s.DemandNote.ID : 0
                                                        };


                        if (applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT && applStatus1 == enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                        {
                            Int32 paidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                            gbbatch.Columns[7].Visible = false;
                            gbbatch.Columns[8].Visible = false;
                            btnVerify.Visible = false;
                            btnCPayment.Visible = false;
                            btnMNotVerify.Visible = false;
                            btnReject.Visible = false;
                            btnShowDemandNote.Visible = true;
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.Text = "Please click Show Demand Note for dispatching the applications.";
                            //btnSubmits.Visible = true;
                            if (Request.QueryString["st"] != null)
                            {
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                                gbbatch.Columns[gbbatch.Columns.Count - 1].Visible = false;
                                btnSubmits.Visible = false;
                                lblError.Text = "Please go to nearest CSC-SPV to pay the demand note shown below or use Pending Demand Note interface.";
                                lblError.Visible = true;
                                btnShowDemandNote.Visible = false;
                                lblErrorMsg.Visible = false;
                            }
                            else
                                fillCourseExamApplication = fillCourseExamApplication.Where(a => (a.PaymentStatusID == paidStatus || a.PaymentStatusID == PaidButNotVerified));
                        }
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.ID);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                                case "Appno":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appno);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appno);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Name);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Name);
                                    break;
                                case "FatherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.FatherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.FatherName);
                                    break;
                                case "MotherName":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.MotherName);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.MotherName);
                                    break;

                                case "Appdate":
                                    if (sortOrder == "DESC")
                                        fillCourseExamApplication = fillCourseExamApplication.OrderByDescending(s => s.Appdate);
                                    else
                                        fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.Appdate);
                                    break;
                                default:
                                    fillCourseExamApplication = fillCourseExamApplication.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(fillCourseExamApplication, ref gbbatch);
                        //uPnlGrid.Update();
                        //uPnlNavigation.Update();
                        Course cs = context.Courses.Find(CourseID);
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Code + ":" + applStatus.ToString(), "Admin/CourseRegStudentStatus.aspx?" + Request.QueryString.ToString(), ""));
                    }
                }
                #endregion

                if (gbbatch.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    btnVerify.Visible = false;
                    btnCPayment.Visible = false;
                    btnMNotVerify.Visible = false;
                    btnReject.Visible = false;
                    btnSubmits.Visible = false;
                    btnShowDemandNote.Visible = false;
                    lblErrorMsg.Visible = false;
                }
            };
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
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            //string searchString = prefixText.Trim().ToUpper();

            //var ApplicationNo = from s in context.CourseRegistrationApplications
            //                 select new { ID = s.Candidate.ID };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    ApplicationNo = ApplicationNo.Where(s => s.ID.ToString().Contains(searchString));
            //}
            //ApplicationNo = ApplicationNo.OrderBy(s => s.ID);
            //foreach (var c in ApplicationNo)
            //{
            //    items.Add(c.ID.ToString());
            //}
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    #endregion-------Private Methods---------------
}

public class StudentList
{
    public String AppNo
    {
        get;
        set;
    }
    public DateTime AppDate
    {
        get;
        set;
    }
    public String CourseCode
    {
        get;
        set;
    }
    public String Email
    {
        get;
        set;
    }
    public String InstituteName
    {
        get;
        set;
    }
    public String AppName
    {
        get;
        set;
    }
    public String Salutation
    {
        get;
        set;
    }
}
