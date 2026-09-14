using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class hoCoursesRegStatus : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError.Visible = false;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            txtbatchno.Enabled = false;
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    PopulateDataForNewEditMode();
                    ShowEditMode();
                }
                else
                {
                    PopulateDataListMode();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Batches", "HO/hoCoursesRegStatus.aspx", ""));
                }
                // BindState();
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
    protected void PopulateDataForNewEditMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Course Category
                ListItem lst = new ListItem("--Select One--", "0");



                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category, lst);

                //Populating Applicant Type
                var CourseList = from p in context.ApplicantTypes
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlApplicantType, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PopulateDataListMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Course Category
                ListItem lst = new ListItem("--All--", "0");
                var Category = (from p in context.CourseCategories
                                orderby (p.Name)
                                select new { ValueField = p.ID, TextField = p.Name }).Distinct();

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategoryFilter, Category, lst);
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlStatusFilter, typeof(enmBatchStatus), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PupulateCourses(Int32 CourseCategoryID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {

                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == CourseCategoryID
                                 //Added 17 Jan 2019
				//Updated 13 Dec 2022 from p.ShowOnWeb
                                 && p.IsActive
                                 select new { ValueField = p.ID, TextField = p.Name+" ("+p.Code +")" };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PupulateApplicationType(Int32 CourseID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {

                Course course = context.Courses.Find(CourseID);
                if (course != null)
                {
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        if (course.CourseTypeID == Convert.ToInt32(enmCourseType.CertificationExam))
                        {
                            var ApplicationList = from p in context.ApplicationTypes
                                                  where p.CourseTypeID == course.CourseTypeID
                                                  select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddl, ApplicationList, lst);
                        }
                        else
                            ddl.Items.Add(lst);
                    }
                    else if (loginUserType == UserType.HeadOffice)
                    {
                        if (course.CourseTypeID == Convert.ToInt32(enmCourseType.CertificationCourse))
                        {
                            var ApplicationList = from p in context.ApplicationTypes
                                                  where p.CourseTypeID == course.CourseTypeID
                                                  select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddl, ApplicationList, lst);
                        }
                        else
                            ddl.Items.Add(lst);
                    }
                    else
                        ddl.Items.Add(lst);
                }
                else
                {
                    ddl.Items.Add(lst);
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
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 courseCategoryID = 0;
                Int32 courseID = 0;
                Int32 ExTypeID = 0;
                Int32 statusID = 0;
                Int32 ExamID = 0;
                Int32 loginUserID = Convert.ToInt32(Session["UserID"]); ;
                if (ddlCourseCategoryFilter.SelectedValue != "0")
                    courseCategoryID = Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue);
                if (ddlCourseFilter.SelectedValue != "0")
                    courseID = Convert.ToInt32(ddlCourseFilter.SelectedValue);
                if (ddlApplicationTypeFilter.SelectedValue != "0")
                    ExTypeID = Convert.ToInt32(ddlApplicationTypeFilter.SelectedValue);
                if (ddlStatusFilter.SelectedValue != "0")
                    statusID = Convert.ToInt32(ddlStatusFilter.SelectedValue);
                if (ddlExamNameFilter.SelectedValue != "0")
                    ExamID = Convert.ToInt32(ddlExamNameFilter.SelectedValue);
                var fillBatch = from s in context.Batchs
                                select new
                                {
                                    CreatedBy = s.CreatedByID,
                                    RegionalCenterID = s.RegionalCenterID,
                                    ID = s.ID,
                                    Number = s.Number,
                                    CourseCategoryID = s.CourseCategoryID,
                                    CourseID = s.CourseID,
                                    Course = s.CourseCategory.Code + "-" + s.Course.Code,
                                    ApplicationType = s.ApplicationType.Name,
                                    ApplicationTypeID = s.ApplicationTypeID,
                                    StatusID = s.StatusID,
                                    CreatedOn = s.CreatedOn,
                                    examId = s.ExamID,
                                    ExamName = s.Exam.Name
                                };
                fillBatch = fillBatch.OrderByDescending(s => s.ID);
                if (loginUserType == UserType.RegionalCenter)
                {
                    fillBatch = fillBatch.Where(s => s.RegionalCenterID == entityID);
                }
                else
                {
                    fillBatch = fillBatch.Where(s => s.RegionalCenterID == null);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    fillBatch = fillBatch.Where(s => s.Number.ToUpper().Contains(searchString));
                }

                if (courseCategoryID != 0)
                {
                    fillBatch = fillBatch.Where(s => s.CourseCategoryID == courseCategoryID);
                }
                if (courseID != 0)
                {
                    fillBatch = fillBatch.Where(s => s.CourseID == courseID);
                }
                if (statusID != 0)
                {
                    fillBatch = fillBatch.Where(s => s.StatusID == statusID);
                }
                if (ExamID != 0)
                {
                    fillBatch = fillBatch.Where(s => s.examId == ExamID);
                }
                if (ExTypeID != 0)
                {
                    fillBatch = fillBatch.Where(s => s.ApplicationTypeID == ExTypeID);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ApplicationType":
                            if (sortOrder == "DESC")
                                fillBatch = fillBatch.OrderByDescending(s => s.ApplicationType);
                            else
                                fillBatch = fillBatch.OrderBy(s => s.ApplicationType);
                            break;
                        case " ExamName":
                            if (sortOrder == "DESC")
                                fillBatch = fillBatch.OrderByDescending(s => s.ExamName);
                            else
                                fillBatch = fillBatch.OrderBy(s => s.ExamName);
                            break;
                        case "Number":
                            if (sortOrder == "DESC")
                                fillBatch = fillBatch.OrderByDescending(s => s.Number);
                            else
                                fillBatch = fillBatch.OrderBy(s => s.Number);
                            break;
                        case "Course":
                            if (sortOrder == "DESC")
                                fillBatch = fillBatch.OrderByDescending(s => s.Course);
                            else
                                fillBatch = fillBatch.OrderBy(s => s.Course);
                            break;
                        case "CreatedOn":
                            if (sortOrder == "DESC")
                                fillBatch = fillBatch.OrderByDescending(s => s.CreatedOn);
                            else
                                fillBatch = fillBatch.OrderBy(s => s.CreatedOn);
                            break;
                        case "StatusID":
                            if (sortOrder == "DESC")
                                fillBatch = fillBatch.OrderByDescending(s => s.StatusID);
                            else
                                fillBatch = fillBatch.OrderBy(s => s.StatusID);
                            break;
                        default:
                            fillBatch = fillBatch.OrderBy(s => s.ID);
                            break;
                    }
                }

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    fillBatch = fillBatch.Where(a => roleCourses.Contains(a.CourseCategoryID));
                }

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    fillBatch = fillBatch.Where(a => roleCourses.Contains(a.CourseID));
                }

                PagingBar1.Bind(fillBatch, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            //context.Dispose();
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
            lblHeading.Text = "Batch Details";
            tblNavLinks.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            using (EConnectContext context = new EConnectContext())
            {
                Int32 applTypeID = 0;
                Int32 BatchId = Convert.ToInt32(Request.QueryString["Key"]);
                Int32 batchStatusID = (from p in context.Batchs
                                       where p.ID == BatchId
                                       select p.StatusID).FirstOrDefault();
                string batchNumber = "";
                if (batchStatusID == Convert.ToInt32(enmBatchStatus.Created))
                {
                    Batch batch = context.Batchs.Find(BatchId);
                    hfCcategory.Value = batch.CourseCategoryID.ToString();
                    ddlCourseCategry.SelectedValue = batch.CourseCategoryID.ToString();
                    ddlCourseCategry_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
                    ddlCourseName.SelectedValue = batch.CourseID.ToString();
                    ddlCourseName_SelectedIndexChanged(ddlApplicationType, EventArgs.Empty);
                    ddlApplicationType.SelectedValue = batch.ApplicationTypeID.ToString();
                    ddlrequesttype_SelectedIndexChanged(ddlApplicationType, EventArgs.Empty);
                    ddlExamName.SelectedValue = batch.ExamID.ToString();
                    ddlApplicantType.SelectedValue = batch.ApplicantTypeID.ToString();
                    txtbatchno.Text = batch.Number.ToString();
                    txtbatchdate.Text = batch.CreatedOn.ToString("dd-MMM-yyyy");
                    batchNumber = batch.Number;
                    applTypeID = batch.ApplicationTypeID;
                }
                else
                {
                    var batch = (from p in context.Batchs
                                 where p.ID == BatchId
                                 select new
                                 {
                                     BatchNumber = p.Number,
                                     BatchDate = p.CreatedOn,
                                     ApplicantType = p.ApplicantType.Name,
                                     CourseName = p.CourseCategory.Code + "-" + p.Course.Code,
                                     ApplicationType = p.ApplicationType.Name,
                                     applTypeID = p.ApplicationTypeID,
                                     batchStatusID = p.StatusID
                                 }).FirstOrDefault();
                    lblcourse.Text = batch.CourseName;
                    lblbthtype.Text = batch.ApplicationType;
                    lblbno.Text = batch.BatchNumber.ToString();
                    lblHeading.Text += ": " + batch.BatchNumber.ToString();
                    bthdate.Text = batch.BatchDate.ToString("dd-MMM-yyyy");
                    lblApplicantType.Text = batch.ApplicantType;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    tblEditInfo.Visible = false;
                    divViewInfo.Visible = true;
                    btnscan.Visible = false;
                    batchNumber = batch.BatchNumber;
                    applTypeID = batch.applTypeID;
                    //Retreiving Application count application status vise
                    //Total Applications in current batch
                    var AppCount = (from a in context.BatchItems
                                    where a.BatchID == BatchId
                                    select a).Count();
                    hktotal.Text = AppCount.ToString();
                    hlkreceived.CommandArgument = "0";

                    Int32 PendingPaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    Int32 DemandDraftPaymentMode = Convert.ToInt32(enmPaymentMode.DemandDraft);
                    Int32 PaymentVerificationPending = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                    Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                    Int32 ApplicationReceivedByRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                    Int32 ApplicationFoundDublicate = Convert.ToInt32(enmCourseApplicationStatus.ApplicationFoundDublicate);
                    Int32 KeptInAbeyance = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                    Int32 ApplicationRejectedWithReason = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason);
                    Int32 ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
                    Int32 ApplicationVerifiedByNIELITAndForwardedToRegistrationWing = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                    Int32 ApplicationVerifiedByNIELITAndForwardedToExaminationWing = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing);

                    //ApplicationReceivedByNIELIT/RegionalCenter
                    if ((enmApplicationType)applTypeID == enmApplicationType.CourseRegistrationApplication)
                    {
                        var receivedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                             b.StatusID == ApplicationReceivedByNIELIT
                                             select b).Count();
                        hlkreceived.Text = receivedCount.ToString();
                        hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByNIELIT).ToString();
                        lnkNewScan.CommandArgument = hlkreceived.CommandArgument;
                    }
                    else if ((enmApplicationType)applTypeID == enmApplicationType.MercyCaseRegistration)   // Mercy Case
                    {
                        var receivedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                             b.StatusID == ApplicationReceivedByNIELIT
                                             select b).Count();
                        hlkreceived.Text = receivedCount.ToString();
                        hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByNIELIT).ToString();
                        lnkNewScan.CommandArgument = hlkreceived.CommandArgument;
                    }
                    else if ((enmApplicationType)applTypeID == enmApplicationType.CertificateExamApplication)
                    {
                        var receivedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                             b.StatusID == ApplicationReceivedByRegionalCentre
                                             select b).Count();
                        hlkreceived.Text = receivedCount.ToString();
                        hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByRegionalCentre).ToString();
                        lnkNewScan.CommandArgument = hlkreceived.CommandArgument;
                    }
                    else if ((enmApplicationType)applTypeID == enmApplicationType.CourseExamApplication)
                    {
                        var receivedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                             b.StatusID == ApplicationReceivedByNIELIT
                                             select b).Count();
                        hlkreceived.Text = receivedCount.ToString();
                        hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByNIELIT).ToString();
                        lnkNewScan.CommandArgument = hlkreceived.CommandArgument;
                    }

                    //ApplicationRejectedWithReason
                    var rejectedcount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                         b.StatusID == ApplicationRejectedWithReason
                                         select b).Count();
                    hlkreject.Text = rejectedcount.ToString();
                    hlkreject.CommandArgument = Convert.ToInt16(ApplicationRejectedWithReason).ToString();

                    //KeptInAbeyance
                    var keptInAbeyancecount = (from b in context.BatchItems
                                               where b.BatchID == BatchId &&
                                               b.StatusID == KeptInAbeyance
                                               select b).Count();
                    hlKeptInAbeyance.Text = keptInAbeyancecount.ToString();
                    hlKeptInAbeyance.CommandArgument = Convert.ToInt16(KeptInAbeyance).ToString();

                    //DemandDraftVerificationPending
                    var demandVerificationPendingCount = (from b in context.BatchItems
                                                          where b.BatchID == BatchId &&
                                                         b.StatusID == PaymentVerificationPending
                                                          select b).Count();
                    hlkpendingDDVerify.Text = demandVerificationPendingCount.ToString();
                    hlkpendingDDVerify.CommandArgument = Convert.ToInt16(PaymentVerificationPending).ToString();

                    //ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing
                    if ((enmApplicationType)applTypeID == enmApplicationType.CourseRegistrationApplication)
                    {
                        var verifiedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                            b.StatusID == ApplicationVerifiedByNIELITAndForwardedToRegistrationWing
                                             select b).Count();
                        hlkprocess.Text = verifiedCount.ToString();
                        hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByNIELITAndForwardedToRegistrationWing).ToString();
                    }
                  else if ((enmApplicationType)applTypeID == enmApplicationType.MercyCaseRegistration)   // Mercy Case
                    {
                        var verifiedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                            b.StatusID == ApplicationVerifiedByNIELITAndForwardedToRegistrationWing
                                             select b).Count();
                        hlkprocess.Text = verifiedCount.ToString();
                        hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByNIELITAndForwardedToRegistrationWing).ToString();
                    }
                    else if ((enmApplicationType)applTypeID == enmApplicationType.CertificateExamApplication)
                    {
                        var verifiedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                            b.StatusID == ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing
                                             select b).Count();
                        hlkprocess.Text = verifiedCount.ToString();
                        hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing).ToString();
                    }
                    else if ((enmApplicationType)applTypeID == enmApplicationType.CourseExamApplication)
                    {
                        var verifiedCount = (from b in context.BatchItems
                                             where b.BatchID == BatchId &&
                                            b.StatusID == ApplicationVerifiedByNIELITAndForwardedToExaminationWing
                                             select b).Count();
                        hlkprocess.Text = verifiedCount.ToString();
                        hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByNIELITAndForwardedToExaminationWing).ToString();
                    }
                    //ApplicationFoundDublicate
                    var duplicateCount = (from b in context.BatchItems
                                          where b.BatchID == BatchId &&
                                         b.StatusID == ApplicationFoundDublicate
                                          select b).Count();
                    hlkduplicate.Text = duplicateCount.ToString();
                    hlkduplicate.CommandArgument = Convert.ToInt16(ApplicationFoundDublicate).ToString();
                    Int32 verified;
                    verified = Convert.ToInt32(hktotal.Text) - (Convert.ToInt32(hlkreject.Text) + Convert.ToInt32(hlKeptInAbeyance.Text));
                    Int32 Complete = Convert.ToInt32(enmBatchStatus.Completed);
                    if (Convert.ToInt32(hlkprocess.Text) == verified && batch.batchStatusID != Complete && Convert.ToInt32(hktotal.Text) != 0 && Convert.ToInt32(hlkprocess.Text) != 0)
                        BtnMrkAsVerified.Visible = true;
                    if (batch.batchStatusID == Complete)
                        lnkNewScan.Visible = false;
                }

                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batchNumber, "HO/hoCoursesRegStatus.aspx?" + Request.QueryString.ToString(), ""));
            };
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
            txtbatchdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnscan.Visible = false;
            //Change the heading text as required
            PopulateDataForNewEditMode();
            lblHeading.Text = "New Batch";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Batch", "#", ""));

        }
        else
        {
            Response.Redirect("hoCoursesRegStatus.aspx", true);
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            Batch batch;
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                        if (loginUserType == UserType.RegionalCenter)
                        {
                            if (!context.Exams.Find(examID).IsBatchProcessable)
                            {
                                ShowAlert("Batch processing not required for the selected exam.", true);
                                ddlApplicantType.SelectedIndex = ddlApplicationType.SelectedIndex = ddlCourseCategry.SelectedIndex = ddlCourseName.SelectedIndex = ddlExamName.SelectedIndex = 0;
                                txtbatchno.Text = "";
                                txtbatchdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                                return;
                            }
                        }

                        batch = new EConnect.NIELIT.Batch();
                        batch.Number = txtbatchno.Text.ToUpper().ToString();
                        batch.ApplicationTypeID = Convert.ToInt32(ddlApplicationType.SelectedValue);
                        batch.CourseCategoryID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                        batch.CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                        batch.StatusID = Convert.ToInt32(enmBatchStatus.Created);
                        batch.CreatedOn = Convert.ToDateTime(txtbatchdate.Text);
                        batch.CreatedByID = Convert.ToInt32(Session["UserID"]);
                        if (loginUserType == UserType.RegionalCenter)
                        {
                            batch.RegionalCenterID = (int)entityID;
                        }
                        batch.ApplicantTypeID = Convert.ToInt32(ddlApplicantType.SelectedValue);
                        batch.ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                        context.Batchs.Add(batch);
                        context.SaveChanges();
                        batch.Number += batch.ID.ToString();
                        context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        strMessage = "New record saved.";
                    }
                    else
                    {
                        batch = context.Batchs.Find(Convert.ToInt16(Request.QueryString["key"]));
                        batch.Number = txtbatchno.Text.ToUpper().ToString();
                        batch.ApplicationTypeID = Convert.ToInt32(ddlApplicationType.SelectedValue);
                        batch.ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                        batch.CourseCategoryID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                        batch.ApplicantTypeID = Convert.ToInt32(ddlApplicantType.SelectedValue);
                        batch.CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                        batch.StatusID = Convert.ToInt32(enmBatchStatus.Created);
                        batch.CreatedOn = Convert.ToDateTime(txtbatchdate.Text);
                        batch.CreatedByID = Convert.ToInt32(Session["UserID"]);
                        context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        strMessage = "Record updated.";
                    }
                };
                scope.Complete();
            };
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("hoCoursesRegStatus.aspx?Key=" + batch.ID.ToString()));
            }
            else
                Response.Redirect("hoCoursesRegStatus.aspx");
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
            ddlCourseCategoryFilter.SelectedValue = "0";
            PupulateCourses(Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue), ddlCourseFilter, new ListItem("--All--", "0"));
            ddlApplicationTypeFilter.Items.Clear();
            ListItem lst = new ListItem("--All--", "0");
            ddlApplicationTypeFilter.Items.Add(lst);
            ddlStatusFilter.SelectedValue = "0";
            ddlExamNameFilter.SelectedValue = "0";
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
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
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

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var fillbatch = from s in context.Batchs
                            //            where s.CourseTypeID == courseType
                            select new { Number = s.Number };
            if (!String.IsNullOrEmpty(searchString))
            {
                fillbatch = fillbatch.Where(s => s.Number.ToUpper().Contains(searchString));
            }
            fillbatch = fillbatch.OrderBy(s => s.Number);

            //var ExamCode = from c in context.ExamCenters
            //               //            where s.CourseTypeID == courseType
            //               select new { Code = c.Code };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    ExamCode = ExamCode.Where(c => c.Code.ToUpper().Contains(searchString));
            //}

            //ExamCenter =
            //Examcentre = Examcentre.Union(ExamCode).Take(count);
            foreach (var course in fillbatch)
            {
                items.Add(course.Number);
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
        Response.Redirect("hoCoursesRegStatus.aspx", true);
    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PupulateCourses(Convert.ToInt32(ddlCourseCategry.SelectedValue), ddlCourseName, new ListItem("--Select One--", "0"));
            ddlApplicationType.Items.Clear();
            ListItem lst = new ListItem("--Select One--", "0");
            ddlApplicationType.Items.Add(lst);
            ddlExamName.Items.Clear();
            ddlExamName.Items.Add(lst);
            GenerateBatchNumber();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PupulateCourses(Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue), ddlCourseFilter, new ListItem("--All--", "0"));
            ddlApplicationTypeFilter.Items.Clear();
            ListItem lst = new ListItem("--All--", "0");
            ddlApplicationTypeFilter.Items.Add(lst);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCourseFilter.SelectedValue != "0")
                PupulateApplicationType(Convert.ToInt32(ddlCourseFilter.SelectedValue), ddlApplicationTypeFilter, new ListItem("--All--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void hlkreceived_Click(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application Received", "HO/hoCoursesRegStatus.aspx?" + Request.QueryString.ToString(), ""));
            LinkButton btn = (LinkButton)sender;
            //Response.Redirect("hoCoursesRegStatus.aspx?key=" + Request.QueryString["key"] + "&Status=" + btn.CommandArgument);
            //  Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BatchItems.aspx?BatchID=" + Request.QueryString["key"] + "&Status=" + btn.CommandArgument));
            string url = EConnect.Utils.Security.QuertStringModule.Encrypt(
      "BatchItems.aspx?BatchID=" + Request.QueryString["key"] +
      "&Status=" + btn.CommandArgument);

            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PupulateApplicationType(Convert.ToInt32(ddlCourseName.SelectedValue), ddlApplicationType, new ListItem("--Select One--", "0"));
            GenerateBatchNumber();
            ListItem lst = new ListItem("--Select One--", "0");
            ddlExamName.Items.Clear();
            ddlExamName.Items.Add(lst);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnscan_Click(object sender, EventArgs e)
    {
        try
        {
            String status = "10";
            using (EConnectContext context = new EConnectContext())
            {
                Batch batch;
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    batch = context.Batchs.Find(Convert.ToInt16(Request.QueryString["key"]));
                    batch.StatusID = Convert.ToInt32(enmBatchStatus.UnderProcessing);
                    context.SaveChanges();

                    if (batch.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                    {
                        status = Convert.ToInt16(enmCourseApplicationStatus.ApplicationReceivedByNIELIT).ToString();
                    }
                    else if (batch.enmApplicationType == enmApplicationType.CertificateExamApplication)
                    {
                        status = Convert.ToInt16(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre).ToString();
                    }
                    if (batch.enmApplicationType == enmApplicationType.MercyCaseRegistration)
                    {
                        status = Convert.ToInt16(enmCourseApplicationStatus.ApplicationReceivedByNIELIT).ToString();
                    }
                }
            };
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BatchItems.aspx?BatchID=" + Request.QueryString["key"] + "&Status=" + status));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void GenerateBatchNumber()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = 0;
                Int32 courseCategoryID = 0;
                Int32 requestType = 0;
                Int32 applicantType = 0;
                Int32 examid = Convert.ToInt32(ddlExamName.SelectedValue);
                StringBuilder batchNumber = new StringBuilder();
                if (loginUserType == UserType.RegionalCenter)
                    batchNumber.Append("RC-" + context.RegionalCenters.Find(entityID).Code.ToUpper() + "/");
                else
                    batchNumber.Append("HO/");

                if (ddlCourseCategry.SelectedValue != "0")
                {
                    courseCategoryID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                    batchNumber.Append(context.CourseCategories.Find(courseCategoryID).Code.ToUpper() + "/");
                }
                if (ddlCourseName.SelectedValue != "0")
                {
                    courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                    batchNumber.Append(context.Courses.Find(courseID).Code.ToUpper() + "/");
                }
                if (ddlApplicationType.SelectedValue != "0")
                {
                    requestType = Convert.ToInt32(ddlApplicationType.SelectedValue);
                    batchNumber.Append(context.ApplicationTypes.Find(requestType).Code.ToUpper() + "/");
                }
                if (ddlApplicantType.SelectedValue != "0")
                {
                    applicantType = Convert.ToInt32(ddlApplicantType.SelectedValue);
                    batchNumber.Append(context.ApplicantTypes.Find(applicantType).Code.ToUpper() + "/");
                }
                if (Request.QueryString["Key"] != null)
                {
                    batchNumber.Append(Request.QueryString["Key"].ToString());
                }
                txtbatchno.Text = batchNumber.ToString();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString(), true);
        }
    }
    protected void ddlApplicantType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GenerateBatchNumber();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    protected void ddlrequesttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GenerateBatchNumber();
            ddlExamName.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 couID = Convert.ToInt32(ddlCourseName.SelectedValue);
                if (Convert.ToInt32(ddlApplicationType.SelectedValue) == (Int32)enmApplicationType.CourseRegistrationApplication)
                {
                    var ExamName = (from c in context.Exams
                                    join p in context.CourseRegistrationApplications
                                    on c.ID equals p.ApplicableExamID
                                    where (p.BatchItemID == null
                                          && p.CourseID == couID
				//Added for uniqueness
                                          &&c.CourseID ==p.CourseID 
				)
                                    select new { ValueField = c.ID, TextField = c.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName, new ListItem("--Select One--", "0"));

                }
                else if (Convert.ToInt32(ddlApplicationType.SelectedValue) == (Int32)enmApplicationType.MercyCaseRegistration)   // Mercy case
                {
                    var ExamName = (from c in context.Exams
                                    join p in context.CourseRegistrationApplications
                                    on c.ID equals p.ApplicableExamID
                                    where (p.BatchItemID == null
                                          && p.CourseID == couID
                                         
                                          && c.CourseID == p.CourseID
                )
                                    select new { ValueField = c.ID, TextField = c.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName, new ListItem("--Select One--", "0"));

                }
                else if (Convert.ToInt32(ddlApplicationType.SelectedValue) == (Int32)enmApplicationType.CourseExamApplication)
                {
                    var ExamName = (from c in context.Exams
                                    join p in context.CourseExamApplications
                                    on c.ID equals p.ExamID
                                    where (p.BatchItemID == null
                                          && p.CourseID == couID)
                                    select new { ValueField = c.ID, TextField = c.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName, new ListItem("--Select One--", "0"));
                }
                else if (Convert.ToInt32(ddlApplicationType.SelectedValue) == (Int32)enmApplicationType.CertificateExamApplication)
                {
                    var ExamName = (from c in context.Exams
                                    join p in context.CertificateExamApplications
                                    on c.ID equals p.ExamID
                                    where (p.BatchItemID == null
                                          && p.CourseID == couID) && p.FinalSubmitted == true
                                    select new { ValueField = c.ID, TextField = c.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName, new ListItem("--Select One--", "0"));
                }
            };
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    protected void BtnMrkAsVerified_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    Int32 batchID = Convert.ToInt32(Request.QueryString["Key"]);
                    var batch = context.Batchs.Find(batchID);
                    Int32 Complete = Convert.ToInt32(enmBatchStatus.Completed);
                    batch.StatusID = Complete;
                    context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                ShowAlert("Successfully marked this batch as complete!");
                BtnMrkAsVerified.Visible = false;
                lnkNewScan.Visible = false;
                //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BatchItems.aspx?BatchID=" + Request.QueryString["key"] + "&Status=" + status));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlApplicationTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlStatusFilter, typeof(enmBatchStatus), lst);               
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 couCatID = Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue);
                Int32 couID = Convert.ToInt32(ddlCourseFilter.SelectedValue);
                Int32 applicationTypeId = Convert.ToInt32(ddlApplicationTypeFilter.SelectedValue);
                ListItem lst = new ListItem("--All--", "0");
                if (couCatID != 0 && couID != 0 && applicationTypeId != 0)
                {
                    var examName = (from b in context.Batchs
                                    where (b.CourseCategoryID == couCatID && b.CourseID == couID
                                           && b.ApplicationTypeID == applicationTypeId)
                                    select new { ValueField = b.ExamID, TextField = b.Exam.Name }).Distinct();
                    if (examName != null)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamNameFilter, examName, lst);
                    }
                    else
                    {
                        ddlExamNameFilter.Items.Add(lst);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

}