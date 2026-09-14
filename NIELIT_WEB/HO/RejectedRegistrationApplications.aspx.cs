using System;
using System.Collections.Generic;
using System.Data.SqlClient;                                                    //November_2024
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Common;

public partial class RejectedRegistrationApplications : BasePage
{
    String strMessage = string.Empty;    
    Table tbl = new Table();
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                lblError.Visible = false;

                if (!String.IsNullOrEmpty(Request.QueryString["batchItemID"]))
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        var batch = context.BatchItems.Find(Convert.ToInt32(Request.QueryString["batchItemID"]));
                        int BatchId = batch.BatchID;
                        CourseRegistrationApplication cr = new CourseRegistrationApplication();
                        cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                        int courseId = cr.CourseID;
                        int CandidateTypeId = cr.ApplicantTypeID;
                        Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Kept In Abeyance Applications", "/HO/RejectedRegistrationApplications.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ApplicationStatusID=" + ApplicationStatusId, ""));
                        ShowEditMode();
                    };
                }
                else
                {
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Kept In Abeyance Applications", "#", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    btnMode.Visible = false;
                    bindCourse();
                    bindCandidateType();
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
    public void bindCourse()
    {
        try
        {      
            using (var context = new EConnectContext())
            {
                Int32 courseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                ddlCourse.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseTypeID
                              select new { ValueField = s.ID, TextField = s.Name +" ("+s.Code +")"};
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, courses, lst);
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void BindDefiencyCodes()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                var code = from s in context.DeficiencyCodes
                           orderby s.ID
                           select new { ValueField = s.ID, TextField = s.Description };

                EConnect.Utils.Common.ControlUtility.BindListObject(chklistCodes, code);
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void bindCandidateType()
    {
        try
        {          
            using (var context = new EConnectContext())
            {
                EnumUtility.BindListObject(ref ddlCandidateType, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--Select One--", "0"));
            }
        }
        catch (Exception ex){ ShowAlert(ex.Message);}
    }
    public void bindBatchNumber(int CourseId, int CandidateTypeId)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ApplicationStatus = 0;
                
                if (ddlApplStatus.SelectedValue == "K")
                {
                    ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                    var courses = (from b in context.Batchs
                                   join bi in context.BatchItems on b.ID equals bi.BatchID
                                   where (b.CourseID == CourseId && b.ApplicantTypeID == CandidateTypeId
                                          && bi.StatusID == ApplicationStatus && bi.IsKeptInAbeyance == true)
                                   select new { ValueField = b.ID, TextField = b.Number }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchNumber, courses, lst);
                }
                else if (ddlApplStatus.SelectedValue == "R")
                {
                    ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason);
                    var courses = (from b in context.Batchs
                                   join bi in context.BatchItems on b.ID equals bi.BatchID
                                   where (b.CourseID == CourseId && b.ApplicantTypeID == CandidateTypeId
                                          && bi.StatusID == ApplicationStatus && bi.IsRejected == true)
                                   select new { ValueField = b.ID, TextField = b.Number }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchNumber, courses, lst);
                }
                else if (ddlApplStatus.SelectedValue == "V")
                {
                    ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                    var courses = (from b in context.Batchs
                                   join bi in context.BatchItems on b.ID equals bi.BatchID
                                   where (b.CourseID == CourseId && b.ApplicantTypeID == CandidateTypeId
                                          && bi.StatusID == ApplicationStatus && bi.IsKeptInAbeyance == true)
                                   select new { ValueField = b.ID, TextField = b.Number }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchNumber, courses, lst);
                }               
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message);}
    }
    protected void ShowEditMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 id = Convert.ToInt64(Request.QueryString["batchItemID"]);
                BatchItem obj = context.BatchItems.Find(id);

                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 2;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "Candidate Detail";
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now;

                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(obj.CourseRegistrationApplicationID);

                if (!String.IsNullOrEmpty(Request.QueryString["ApplTypeID"]))
                {
                    candidateDetail.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(obj.CourseRegistrationApplication.Name + "(" + obj.CourseRegistrationApplicationID.ToString() + ")", "/HO/RejectedRegistrationApplications.aspx?batchItemID=" + Request.QueryString["batchItemID"].ToString(), ""));
                }
                else
                {
                    candidateDetail.ApplicationTypeID = cr.ApplicantTypeID;
                    BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem(obj.CourseRegistrationApplication.Name + "(" + obj.CourseRegistrationApplicationID.ToString() + ")", "/HO/RejectedRegistrationApplications.aspx?batchItemID=" + Request.QueryString["batchItemID"].ToString(), ""));
                }
                candidateDetail.BatchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);
                candidateDetail.Bind();

                if (cr.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason)) // Rejected Application
                {
                    divDefiencyDetail.Visible = false;
                    BtnSave.Visible = false;
                }
                else if (cr.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance)) // Kept in Abeyance Application
                {
                    divDefiencyDetail.Visible = true;
                    BtnSave.Visible = true;
                    BindDefiencyCodes();
                    txtDescription.Text = obj.KeptInAbeyanceReason;
                    var deficiencyCodes = (from s in context.BatchItemDeficiencyDetails
                                           where (s.BatchItemID == id && s.IsFullFilled == false)
                                           orderby s.ID
                                           select new
                                           {
                                               ID = s.ID,
                                               defiencyID = s.DeficiencyID
                                           }).ToList();
                    if (deficiencyCodes.Count() > 0)
                    {
                        btnVerify.Visible = true;
                        btnReject.Visible = true;
                        trRejectedReason1.Visible = true;
                        trRejectedReason2.Visible = true;
                        BtnSave.Text = "Update";
                        foreach (var code in deficiencyCodes)
                        {
                            foreach (ListItem item in chklistCodes.Items)
                            {
                                if (code.defiencyID == Convert.ToInt64(item.Value))
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                }
                else if (cr.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing))// Application Verified after kept in abeyance full filled
                {
                    divDefiencyDetail.Visible = false;
                    BtnSave.Visible = false;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView()
    {
        try
        {
            lblError.Visible = false;           
            //this is the sample code how to bind the grid control                                 
            int courseId = 0;
            int BatchId = 0;
            int CandidateTypeId = 0;
            Int32 ApplicationStatus = 0;
            Int32 applStatusID = 0;

            if (Convert.ToInt32(ddlCourse.SelectedValue) != 0 && Convert.ToInt32(ddlBatchNumber.SelectedValue) != 0 && Convert.ToInt32(ddlCandidateType.SelectedValue) != 0)
            {
                courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                BatchId = Convert.ToInt32(ddlBatchNumber.SelectedValue);
                CandidateTypeId = Convert.ToInt32(ddlCandidateType.SelectedValue);
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Kept In Abeyance Applications", "#", ""));
            }
            else
            {
                courseId = Convert.ToInt32(Request.QueryString["courseId"]);
                BatchId = Convert.ToInt32(Request.QueryString["BatchId"]);
                CandidateTypeId = Convert.ToInt32(Request.QueryString["CandidateTypeId"]);
                applStatusID = Convert.ToInt32(Request.QueryString["ApplicationStatusID"]);
                if (applStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing))
                    ddlApplStatus.SelectedValue = "V";
                else if (applStatusID == Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance))
                    ddlApplStatus.SelectedValue = "K";
                else if (applStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason))
                    ddlApplStatus.SelectedValue = "R";
                bindCourse();
                bindCandidateType();
                bindBatchNumber(courseId, CandidateTypeId);
                ddlCourse.SelectedValue = Convert.ToString(Request.QueryString["courseId"]);
                ddlBatchNumber.SelectedValue = Convert.ToString(Request.QueryString["BatchId"]);
                ddlCandidateType.SelectedValue = Convert.ToString(Request.QueryString["CandidateTypeId"]);
            }
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            Int32 ApplicationRejectedWithReason = 0;
            Int32 KeptInAbeyance = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
            if (courseId != 0 && CandidateTypeId != 0 && BatchId != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (ddlApplStatus.SelectedValue == "V")
                        ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                    else if (ddlApplStatus.SelectedValue == "K")
                        ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                    else if (ddlApplStatus.SelectedValue == "R")
                        ApplicationRejectedWithReason = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason);

                    Int32 applTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                    var registration = from s in context.CourseRegistrationApplications
                                       join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
                                       where ((s.ApplicationStatusID == ApplicationStatus && b.IsKeptInAbeyance == true)
                                       || s.ApplicationStatusID == ApplicationRejectedWithReason && b.IsRejected == true)
                                       && s.CourseID == courseId && s.ApplicantTypeID == CandidateTypeId
                                       && b.BatchID == BatchId
                                       select new
                                       {
                                           AppNo = s.Number,
                                           AppDate = s.ApplicationDate,
                                           CandidateName = s.Name,
                                           CandidateType = s.ApplicantType.Name,
                                           Course = s.Course.Name,
                                           DateOfReg = DateTime.Now,
                                           RegNo = "",
                                           commencedFromDate = "",
                                           CourseId = s.CourseID,
                                           CandidateTypeId = s.ApplicantTypeID,
                                           ID = b.ID,
                                           BatchId = b.Batch.ID,
                                           ApplTypeID = applTypeID
                                       };

                    registration = registration.OrderBy(s => s.AppNo);
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "AppNo":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.AppNo);
                                else
                                    registration = registration.OrderBy(s => s.AppNo);
                                break;
                            case "AppDate":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.AppDate);
                                else
                                    registration = registration.OrderBy(s => s.AppDate);
                                break;
                            case "CandidateName":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.CandidateName);
                                else
                                    registration = registration.OrderBy(s => s.CandidateName);
                                break;
                            case "CandidateType":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.CandidateType);
                                else
                                    registration = registration.OrderBy(s => s.CandidateType);
                                break;

                            case "Course":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.Course);
                                else
                                    registration = registration.OrderBy(s => s.Course);
                                break;
                            case "DateOfReg":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.DateOfReg);
                                else
                                    registration = registration.OrderBy(s => s.DateOfReg);
                                break;

                            default:
                                registration = registration.OrderBy(s => s.CandidateName);
                                break;
                        }
                    }
                    PagingBar1.Bind(registration, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvMain.Rows.Count > 0)
                    {
                        lblError.Visible = false;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Sorry ! No Record Found ";
                    }
                };
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Sorry ! No Record.Please filter data to show records.";
            }
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
            using (EConnectContext context = new EConnectContext())
            {
                var batch = context.BatchItems.Find(Convert.ToInt32(Request.QueryString["batchItemID"]));
                int BatchId = batch.BatchID;
                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                int courseId = cr.CourseID;
                int CandidateTypeId = cr.ApplicantTypeID;
                Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                Response.Redirect("~/HO/RejectedRegistrationApplications.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ApplicationStatusID=" + ApplicationStatusId);
            };
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
            //Response.Redirect("~/HO/RegistrationProcess.aspx");
            ddlBatchNumber.SelectedValue = "0";
            ddlCourse.SelectedValue = "0";
            ddlCandidateType.SelectedValue = "0";
            lblError.Text = "Sorry ! No Record.Please filter data to show records.";
            //BindGridView();
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
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;
                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = hl.NavigateUrl;
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

        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                Regex regex = new Regex(@"^[0-9]+$");
                if (count <= 0)
                    count = 10;
                List<String> items = new List<String>();
                string searchString = prefixText.Trim().ToUpper();
                int AppStatusId = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                var reg = from s in context.CourseRegistrationApplications
                          where s.ApplicationStatusID == AppStatusId
                          select new { AppId = s.ID, AppName = s.Name };
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (regex.IsMatch(searchString))
                    {
                        long SearchStr = Convert.ToInt64(searchString);
                        reg = reg.Where(s => s.AppId == SearchStr);

                    }
                    else
                        reg = reg.Where(s => s.AppName.ToUpper().Contains(searchString));
                }
                foreach (var user in reg)
                {

                    if (regex.IsMatch(searchString))
                        items.Add(user.AppId.ToString());
                    else
                        items.Add(user.AppName);

                }
                return items.ToArray();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
            int CandidateTypeId = Convert.ToInt32(ddlCandidateType.SelectedValue);

            bindBatchNumber(CourseId, CandidateTypeId);
        }
        catch (Exception ex)
        {

            ShowAlert(ex.Message);
        }
    }
    protected void ddlBatchNumber_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean isNotSelectedCurrentModule = false;
            Boolean allowSendingEmail = false;
            Boolean allowSendingSms = false;
            StringBuilder defficiencydetails = new StringBuilder();
            String emailaddress = "";
            Int64 mobileno = 0;
            string mobileMsg = "";
            StringBuilder msg = new StringBuilder();
            Int32 batchItemID = 0;
            Int32 count = 0;
            foreach (ListItem item in chklistCodes.Items)
            {
                if (item.Selected == true)
                {
                    isNotSelectedCurrentModule = true;
                }
            }
            if (isNotSelectedCurrentModule == false)
            {
                throw new Exception("Please Select at least one Deficiency Code");
            }
            //if (txtDescription.Text == "")
            //{
            //    throw new Exception("Deficiency Description can not be left blank");
            //}

            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (BtnSave.Text == "Save")
                    {
                        batchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);
                        BatchItem batchItem = context.BatchItems.Find(batchItemID);
                        batchItem.KeptInAbeyanceReason = txtDescription.Text;
                        batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                        batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                        context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;

                        foreach (ListItem item in chklistCodes.Items)
                        {
                            if (item.Selected == true)
                            {
                                BatchItemDeficiencyDetail objDefiency = new BatchItemDeficiencyDetail();
                                objDefiency.BatchItemID = batchItemID;
                                objDefiency.DeficiencyID = Convert.ToInt32(item.Value);
                                objDefiency.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                objDefiency.CreatedOn = Convert.ToDateTime(DateTime.Now);
                                objDefiency.IsFullFilled = false;
                                context.BatchItemDeficiencyDetails.Add(objDefiency);
                            }
                        }
                        strMessage = "Record Saved";

                    }
                    else
                    {
                        batchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);

                        //November_2024
                        SqlParameter[] param1 = { new SqlParameter("@batchItemID", batchItemID) };

                        //context.Database.ExecuteSqlCommand("delete from Batch_Item_Deficiency_Detail  where Batch_Item_ID='" + batchItemID + "'");
                        //November_2024
                        context.Database.ExecuteSqlCommand("delete from Batch_Item_Deficiency_Detail  where Batch_Item_ID=@batchItemID", param1);                    


                        BatchItem batchItem = context.BatchItems.Find(batchItemID);
                        batchItem.KeptInAbeyanceReason = txtDescription.Text;
                        batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                        batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                        context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;

                        foreach (ListItem item in chklistCodes.Items)
                        {
                            if (item.Selected == true)
                            {
                                BatchItemDeficiencyDetail objDefiency = new BatchItemDeficiencyDetail();
                                objDefiency.BatchItemID = batchItemID;
                                objDefiency.DeficiencyID = Convert.ToInt32(item.Value);
                                objDefiency.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                objDefiency.CreatedOn = Convert.ToDateTime(DateTime.Now);
                                objDefiency.IsFullFilled = false;
                                context.BatchItemDeficiencyDetails.Add(objDefiency);
                            }
                        }
                        strMessage = "Record Updated";
                    }
                    context.SaveChanges();
                };
                scope.Complete();
            };
            using (EConnectContext context = new EConnectContext())
            {
                var batch = context.BatchItems.Find(batchItemID);
                int BatchId = batch.BatchID;
                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                int courseId = cr.CourseID;
                int CandidateTypeId = cr.ApplicantTypeID;
                Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                // sending sms and email
                var defficiency = (from a in context.BatchItemDeficiencyDetails
                                   join d in context.DeficiencyCodes
                                       on a.DeficiencyID equals d.ID
                                   where a.BatchItemID == batchItemID && a.IsFullFilled == false
                                   select new
                                   {
                                       defficiencydetail = d.Description,
                                   }).ToList();
                if (defficiency.Count() > 0)
                {
                    foreach (var deffdesc in defficiency)
                    {
                        count = count + 1;
                        defficiencydetails.Append(count + ". " + deffdesc.defficiencydetail + "<br/>");
                    }
                    mobileno = cr.MobileNumber;
                    emailaddress = cr.EmailAddress;
                    var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterMarkingtheCourseRegistrationApplicationasKeptinAbeyance));
                    if (evnt.SendEmail == true)
                        allowSendingEmail = true;
                    if (evnt.SendSms == true)
                        allowSendingSms = true;
                    //mobile message
                    //mobileMsg = "Your application with application number:- " + cr.Number + " has been received by NIELIT,but your application has been kept in abeyance due to some deficiency found in your application.So please check your e-mail or check your application status to know the deficiency details.";
					 mobileMsg = "Your application with application number:- " + cr.Number + " has been received by NIELIT,but your application has been kept in abeyance due to some deficiency found in your application.So please check your e-mail or check your application status to know the deficiency details.-NIELIT";
                    //email message
                    msg.Append("Dear " + GetInitCap(cr.Salutation + " " + cr.Name) + ",<br/><br/>" + " Your online application number:- " + cr.Number + " dated " + cr.ApplicationDate.ToString("dd-MMM-yyyy") + " for registration in " + "<b style='color:red'>'" + cr.Course.Code + "'</b>" + " level has been received by NIELIT and <b style='color:red'> under </b> Kept in Abeyance " +
                    " due to following deficiencies found in your application form :- " + "<br/><br/> " + defficiencydetails.ToString());
                    if (!string.IsNullOrEmpty(batch.KeptInAbeyanceReason) && !string.IsNullOrWhiteSpace(batch.KeptInAbeyanceReason))
                    {
                        msg.Append("<br/>Remarks:- <br/>" + GetInitCap(batch.KeptInAbeyanceReason.ToString()));
                        msg.Append("<br/><br/> Please fulfill the deficiencies found in your application and resend your application form back to the NIELIT alongwith the suitable documents in " +
                                  " support of this within the 15 days of the receive of this e-mail, failing which your application will be rejected without prior notice. <br/><br/>" +
                                  " <b style='color:red'> Thanks & Regards <br/> NIELIT<br/>[Registration Section]</b>");
                    }
                    else
                    {
                        msg.Append("<br/></br/> Please fulfill the deficiencies found in your application and resend your application form back to the NIELIT alongwith the suitable documents in " +
                                   " support of this within the 15 days of the receive of this e-mail, failing which your application will be rejected without prior notice. <br/><br/>" +
                                   " <b style='color:red'> Thanks & Regards <br/> NIELIT<br/>[Registration Section]</b>");
                    }

                    if (allowSendingEmail == true)
                    {
                        try
                        {
                            //sending Email 
                            if (emailaddress.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("NIELIT: Status of O/A/B/C Level Online Registration Application", msg.ToString(), emailaddress);
                                mail.Send();
                            }
                        }
                        catch (Exception) {  }
                    }
                    //sending Mobile Message
                    if (allowSendingSms == true)
                    {
                        try
                        {
                            //Sending Mobile Message
                            if (mobileno != 0)
                            {
                                EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileno.ToString(),"1307160931607212852", SmsServiceType.SignleSMS);
                                int sentMessageCount;
                                message.sendSingleSMS(out sentMessageCount);
                            }
                        }
                        catch (Exception) { }
                    }
                }
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RejectedRegistrationApplications.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ApplicationStatusID=" + ApplicationStatusId + "&msg=" + strMessage), true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["batchItemID"]))
        {
            using (EConnectContext context = new EConnectContext())
            {
                var batch = context.BatchItems.Find(Convert.ToInt32(Request.QueryString["batchItemID"]));
                int BatchId = batch.BatchID;
                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                int courseId = cr.CourseID;
                int CandidateTypeId = cr.ApplicantTypeID;
                Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RejectedRegistrationApplications.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ApplicationStatusID=" + ApplicationStatusId), true);
            };
        }
        else
        {
            Response.Redirect("RejectedRegistrationApplications.aspx", true);
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 batchItemID = 0;
            Int32 batchID = 0;
            Boolean allowSendingEmail = false;
            Boolean allowSendingSms = false;
            String emailaddress = "";
            Int64 mobileno = 0;
            string mobileMsg = "";
            string msg = "";
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    batchItemID = Convert.ToInt64(Request.QueryString["batchItemID"]);
                    BatchItem batchItem = context.BatchItems.Find(batchItemID);
                    Int32 PaidButNotVerified = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                    if (batchItem != null)
                    {
                        batchID = Convert.ToInt32(batchItem.BatchID);
                        Batch batch = context.Batchs.Find(batchID);
                        enmApplicationType applicationType = batch.enmApplicationType;
                        if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        {
                            if (batchItem.CourseRegistrationApplication.PaymentStatusID == PaidButNotVerified)
                            {
                                throw new Exception("Your Payment details are not yet verified. " +
                                                    " Please Verify Payment Details through Payment Reconciliation form in Accounts menu.");
                            }
                            else
                            {
                                Int32 ApplicationVerifiedByNIELITAndForwardedToRegistrationWing = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                                batchItem.StatusID = ApplicationVerifiedByNIELITAndForwardedToRegistrationWing;
                                batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationVerifiedByNIELITAndForwardedToRegistrationWing;
                                context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;

                                var deficiencyCodes = (from s in context.BatchItemDeficiencyDetails
                                                       where (s.BatchItemID == batchItemID && s.IsFullFilled == false)
                                                       orderby s.ID
                                                       select new
                                                       {
                                                           ID = s.ID,
                                                           defiencyID = s.DeficiencyID
                                                       }).ToList();
                                if (deficiencyCodes.Count() > 0)
                                {
                                    foreach (var code in deficiencyCodes)
                                    {
                                        BatchItemDeficiencyDetail objDefiency = context.BatchItemDeficiencyDetails.Find(Convert.ToInt64(code.ID));
                                        objDefiency.IsFullFilled = true;
                                        objDefiency.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                        objDefiency.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                        context.Entry(objDefiency).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                };
                scope.Complete();
            };
            using (EConnectContext context = new EConnectContext())
            {
                var batch = context.BatchItems.Find(batchItemID);
                int BatchId = batch.BatchID;
                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                int courseId = cr.CourseID;
                int CandidateTypeId = cr.ApplicantTypeID;
                Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                strMessage = "Application Verified Successfully";
                mobileno = cr.MobileNumber;
                emailaddress = cr.EmailAddress;
                var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterMarkingtheCourseRegistrationApplicationasVerified));
                if (evnt.SendEmail == true)
                    allowSendingEmail = true;
                if (evnt.SendSms == true)
                    allowSendingSms = true;
                //mobile message
               // mobileMsg = " Your application with  application number:- " + cr.Number + " has been received by NIELIT and has been successfully verified.";
			    mobileMsg = "Your application with  application number:- " + cr.Number + "  has been received by NIELIT and has been successfully verified.-NIELIT";
                //email message
                msg = " Dear " + GetInitCap(cr.Salutation + " " + cr.Name) + ",<br/><br/>" + " Your online application number:-  " + cr.Number + " dated " + cr.ApplicationDate.ToString("dd-MMM-yyyy") + " for registration in " + "<b style='color:red'>'" + cr.Course.Code + "'</b>" + " level has been successfully received by NIELIT " +
                      " and your application has been successfully verified by NIELIT by resolving the deficiencies found in your application form.<br/><br/>" +
                      " <b style='color:red'> Thanks & Regards <br/> NIELIT<br/>[Registration Section]</b>";
                if (allowSendingEmail == true)
                {
                    try
                    {
                        //sending Email 
                        if (emailaddress.Trim().Length > 0)
                        {
                            EConnect.NIELIT.Email mail = new Email("NIELIT: Status of O/A/B/C Level Online Registration Application", msg, emailaddress);
                            mail.Send();
                        }
                    }
                    catch (Exception) { }
                }
                //sending Mobile Message
                if (allowSendingSms == true)
                {
                    try
                    {
                        //Sending Mobile Message
                        if (mobileno != 0)
                        {
                            EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileno.ToString(),"1307161053039898101", SmsServiceType.SignleSMS);
                            int sentMessageCount;
                            message.sendSingleSMS(out sentMessageCount);
                        }
                    }
                    catch (Exception) { }
                }
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RejectedRegistrationApplications.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ApplicationStatusID=" + ApplicationStatusId + "&msg=" + strMessage), true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlApplStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
            int CandidateTypeId = Convert.ToInt32(ddlCandidateType.SelectedValue);

            bindBatchNumber(CourseId, CandidateTypeId);
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 batchItemID = 0;
            Int32 batchID = 0;
            Int32 count = 0;
            Boolean allowSendingEmail = false;
            Boolean allowSendingSms = false;
            StringBuilder defficiencydetails = new StringBuilder();
            String emailaddress = "";
            Int64 mobileno = 0;
            string mobileMsg = "";
            string msg = "";
            if (txtReason.Text == "")
            {
                throw new Exception("Rejected Reason cannot be left blank");
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    batchItemID = Convert.ToInt64(Request.QueryString["batchItemID"]);
                    BatchItem batchItem = context.BatchItems.Find(batchItemID);
                    if (batchItem != null)
                    {
                        batchID = Convert.ToInt32(batchItem.BatchID);
                        Batch batch = context.Batchs.Find(batchID);
                        enmApplicationType applicationType = batch.enmApplicationType;
                        if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        {
                            Int32 ApplicationRejectedWithReason = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason);
                            batchItem.StatusID = ApplicationRejectedWithReason;
                            batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                            batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                            batchItem.RejectedByID = Convert.ToInt32(Session["UserID"]);
                            batchItem.RejectedOn = Convert.ToDateTime(DateTime.Now);
                            batchItem.RejectedReason = txtReason.Text;
                            batchItem.IsRejected = true;
                            batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationRejectedWithReason;
                            context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                };
                scope.Complete();
            };
            using (EConnectContext context = new EConnectContext())
            {
                var batch = context.BatchItems.Find(batchItemID);
                int BatchId = batch.BatchID;
                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                int courseId = cr.CourseID;
                int CandidateTypeId = cr.ApplicantTypeID;
                Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                strMessage = "Application Rejected Successfully";
                var defficiency = (from a in context.BatchItemDeficiencyDetails
                                   join d in context.DeficiencyCodes
                                   on a.DeficiencyID equals d.ID
                                   where a.BatchItemID == batchItemID && a.IsFullFilled == false
                                   select new
                                   {
                                       defficiencydetail = d.Description,
                                   }).ToList();
                if (defficiency.Count() > 0)
                {
                    foreach (var deffdesc in defficiency)
                    {
                        count = count + 1;
                        defficiencydetails.Append(count + ". " + deffdesc.defficiencydetail + "<br/>");
                    }
                    mobileno = cr.MobileNumber;
                    emailaddress = cr.EmailAddress;
                    var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterMarkingtheCourseRegistrationApplicationasRejectedWithReason));
                    if (evnt.SendEmail == true)
                        allowSendingEmail = true;
                    if (evnt.SendSms == true)
                        allowSendingSms = true;
                    //mobile message
                    //mobileMsg = " Your application with application number:- " + cr.Number + " has been successfully received by NIELIT but your application has been rejected by NIELIT due to some reasons.So please check your e-mail regarding this.";
					mobileMsg = " Your application with application number:- " + cr.Number + " has been successfully received by NIELIT but your application has been rejected by NIELIT due to some reasons.So please check your e-mail regarding this.-NIELIT";
                    //email message
                    msg = " Dear " + GetInitCap(cr.Salutation + " " + cr.Name) + ",<br/><br/>" + " Your online application number:- " + cr.Number + " dated " + cr.ApplicationDate.ToString("dd-MMM-yyyy") + " for registration in " + "<b style='color:red'>'" + cr.Course.Code + "'</b>" + " level has been received by NIELIT " +
                          ", but your application has been rejected by NIELIT because of the following reason:- " + GetInitCap(batch.RejectedReason.ToString()) + " Your deficiency details are as followed :- <br/> " + defficiencydetails.ToString() + "<br/> Please contact the Registration Wing, NIELIT for more details.<br/><br/>" +
                          "<b style='color:red'> Thanks & Regards <br/> NIELIT<br/>[Registration Section]</b>";
                    if (allowSendingEmail == true)
                    {
                        try
                        {
                            //sending Email 
                            if (emailaddress.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("NIELIT: Status of O/A/B/C Level Online Registration Application", msg, emailaddress);
                                mail.Send();
                            }
                        }
                        catch (Exception) { }
                    }
                    //sending Mobile Message
                    if (allowSendingSms == true)
                    {
                        try
                        {
                            //Sending Mobile Message
                            if (mobileno != 0)
                            {
                                EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileno.ToString(),"1307160931607212852", SmsServiceType.SignleSMS);
                                int sentMessageCount;
                                message.sendSingleSMS(out sentMessageCount);
                            }
                        }
                        catch (Exception) { }
                    }
                }
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RejectedRegistrationApplications.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ApplicationStatusID=" + ApplicationStatusId + "&msg=" + strMessage), true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}