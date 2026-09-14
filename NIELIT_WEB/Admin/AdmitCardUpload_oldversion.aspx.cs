using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class AdmitCardUpload : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;

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
                    FillCategories();
                    ShowEditMode();

                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilterCourses();
                    if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
                    {
                        ddlflCourse.SelectedValue = Request.QueryString["CourseID"];
                        ddlflCourse_SelectedIndexChanged(ddlflCourse, EventArgs.Empty);
                        ddlflExamCycle.SelectedValue = Request.QueryString["ExamCycleID"];
                        ddlflExamCycle_SelectedIndexChanged(ddlflExamCycle, EventArgs.Empty);
                        ddlflExamYear.SelectedValue = Request.QueryString["ExamYear"];
                        ddlflExamYear_SelectedIndexChanged(ddlflExamYear, EventArgs.Empty);
                        ddlflExam.SelectedValue = Request.QueryString["ExamID"];
                        ddlflExam_SelectedIndexChanged(ddlflExam.SelectedValue, EventArgs.Empty);
                        ddlRegionalCentre.SelectedValue = Request.QueryString["RegCentreID"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Admit Card Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/AdmitCardUpload.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Admit Card Data", "Admin/AdmitCardUpload.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria For View Result";
                        lblError.Visible = true;
                    }
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

    private void UnsendSMS(Int64 ExamID, Int64 regcentreID)
    {
        using (EConnectContext context = new EConnectContext())
        {
            int appCount = context.CertificateExamApplications.
                           Where(s => s.ExamID == ExamID && s.RegionalCenterID == regcentreID && s.SMSSent == false && s.RollNumber != null).Count();
            SMSLbl.Text = "SMS to be sent : " + appCount;
            if (appCount > 0)
            { SendSMSBtn.Enabled = true; }
            else { SendSMSBtn.Enabled = false; }
        }
    }

    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                //var Category = from p in context.CourseCategories
                // where p.ID == CourseType
                //orderby (p.Name)
                //select new { ValueField = p.ID, TextField = p.Name };

                //Int32 CourseType2 = Convert.ToInt32(enmCourseType.IRDACat);
                //var Category = from p in context.CourseCategories
                //               where p.ID == CourseType || p.ID == CourseType2
                //               orderby (p.Name) descending
                //               select new { ValueField = p.ID, TextField = p.Name };
                var Category = (from s in context.CourseCategories
                                join c in context.Courses on s.ID equals c.CourseCategoryID
                                where c.CourseTypeID == CourseType
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillRegionalCenter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var rc = (from s in context.RegionalCenters
                          select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);

                ListItem lst1 = new ListItem("--Select One--", "0");
                if (rc != null)
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        ddlRc.SelectedValue = entityID.ToString();
                        ddlRc.Enabled = false;
                    }
                }
                else
                {
                    ddlRc.Items.Add(lst1);
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterRegionalCenter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var rc = (from s in context.RegionalCenters
                          select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                Int32 examID = Convert.ToInt32(ddlflExam.SelectedValue);

                ListItem lst1 = new ListItem("--Select One--", "0");
                if (rc != null)
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, rc.OrderBy(c => c.TextField), lst1);
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        ddlRegionalCentre.SelectedValue = entityID.ToString();
                        ddlRegionalCentre.Enabled = false;
                    }
                }
                else
                {
                    ddlRegionalCentre.Items.Add(lst1);
                }
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillExamYear()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamYear = (from p in context.Exams
                                where p.ExaminationCycleID == ExamCycleID
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(3);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterExamYear()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlflExamCycle.SelectedValue);
                var ExamYear = (from p in context.Exams
                                where p.ExaminationCycleID == ExamCycleID
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(3);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamYear, ExamYear, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillExamCycle()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                int CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                var ExamCycleList = from p in context.ExaminationCycles
                                    where p.CourseID == CourseID
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, ExamCycleList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterExamCycle()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                int CourseID = Convert.ToInt32(ddlflCourse.SelectedValue);
                var ExamCycleList = from p in context.ExaminationCycles
                                    where p.CourseID == CourseID
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamCycle, ExamCycleList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id && p.CourseTypeID == CourseType
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseTypeID = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseTypeID == CourseTypeID
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflCourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillExamName()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamName = from p in context.Exams
                               where p.ExamYear == ExamYear && p.CourseID == CourseID && p.ExaminationCycleID == ExamCycleID
                               && p.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                               select new { ValueField = p.ID, TextField = p.Name };
                ExamName = ExamName.OrderByDescending(s => s.ValueField);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterExam()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamYear = Convert.ToInt32(ddlflExamYear.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlflCourse.SelectedValue);
                Int32 examcycleid = Convert.ToInt32(ddlflExamCycle.SelectedValue);
                //var ExamName = from p in context.CertificateExamApplications
                //               join c in context.Exams on
                //                   p.ExamID equals c.ID
                //               where p.Exam.ExamYear == ExamYear && p.CourseID == CourseID && p.Exam.ExaminationCycleID == examcycleid
                //               select new { ValueField = p.Exam.ID, TextField = p.Exam.Name };
                var ExamName = from p in context.Exams
                               where p.ExamYear == ExamYear && p.CourseID == CourseID && p.ExaminationCycleID == examcycleid
                               && p.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                               select new { ValueField = p.ID, TextField = p.Name };
                ExamName = ExamName.OrderByDescending(s => s.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExam, ExamName, lst);
            };
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
            btnValidate.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Upload Admit Card Data";
            tblNavLinks.Visible = true;
            //imgSampleDoc.Visible = false;
            tbls2.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 applicationID = Convert.ToInt32(Request.QueryString["Key"]);
                var application = (from p in context.CertificateExamApplications
                                   where p.ID == applicationID
                                   select p).FirstOrDefault();
                //var examCentreName = (from s in context.ExamCenters
                //                      where s.Code == application.ExamCentreName
                //                      select s).FirstOrDefault();

                //ddlcoursecategory.SelectedValue = application.CourseCategoryID.ToString();
                //ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);
                //ddlcourse.SelectedValue = application.CourseID.ToString();
                //ddlcourse_SelectedIndexChanged(ddlcourse, EventArgs.Empty);
                //ddlExamCycle.SelectedValue = application.Exam.ExaminationCycleID.ToString();
                //ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                //ddlExamName.SelectedValue = application.ExamID.ToString();
                //ddlcoursecategory.Enabled = false;
                //ddlcourse.Enabled = false;
                //ddlExamCycle.Enabled = false;
                //ddlExamName.Enabled = false;
                //lblup.Visible = false;
                flUpload.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                //Response.Write(tblprint.InnerHtml);
                lblAppNumber.Text = application.Number.ToString();
                lblAppDate.Text = application.ApplicationDate.ToString("dd-MMM-yyyy");
                lblCourse.Text = application.Course.Code + "-" + application.CourseCategory.Code;
                lblExamDetail.Text = application.Exam.Name + "-" + application.Exam.ExaminationCycle.Name;
                lblRegName.Text = GetInitCap(application.RegionalCenter.Name);
                lblRollno.Text = application.RollNumber.ToString();
                lblCandidate.Text = application.Salutation + " " + GetInitCap(application.Name);
                if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    if (!String.IsNullOrEmpty(application.FatherName))
                        lblFather.Text = "Mr. " + GetInitCap(application.FatherName);
                    else
                        lblFather.Text = "NA";
                    if (!String.IsNullOrEmpty(application.MotherName))
                        lblMother.Text = "Mrs. " + GetInitCap(application.MotherName);
                    else
                        lblMother.Text = "NA";
                }
                else
                {
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    LblGuardianName.Text = GetInitCap(application.GuardianName);
                }

                lblExamCentreName.Text = application.ExamCentreName.ToString();
                //lblExamCentreName.Text = examCentreName.Name != null ?  GetInitCap(examCentreName.Name.ToString()): application.ExamCentreName.ToString();
                lblAddress.Text = application.ExamCentreAddress.ToString();
                lblBatchNo.Text = application.ExamBatchNumber.ToString();
                lblReportTime.Text = application.ReportingTime.ToString();
                lblDateofExam.Text = application.DateOfExam.Value.ToString("dd-MMM-yyyy");
                Txtexamcentrename.Text = lblExamCentreName.Text;
                Txtexamcentreaddress.Text = lblAddress.Text;
                Txtbatchno.Text = lblBatchNo.Text;
                Txtreptime.Text = lblReportTime.Text;
                Txtexamdate.Text = lblDateofExam.Text;
                Txtrollno.Text = lblRollno.Text;
                // hlAppDetail.Attributes.Add("Onclick", "return showForm('../CAND/CertificatePreview.aspx?Appid=' " + ApplicationID + " ');");
                hlAppDetail.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?Appid=" + applicationID.ToString()) + "');");
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(application.RollNumber + "-" + "(" + application.Name + ")", "Admin/AdmitCardUpload.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;

                //checking the edit option
                Int32 filterexamid = application.ExamID;
                var exams = context.Exams.Find(filterexamid);
                if (exams != null)
                {
                    if (exams.DateOfPublishingOfRollNumber.HasValue)
                        trchangedetail.Visible = false;
                    else
                        trchangedetail.Visible = true;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            Int32 cid = 0;
            lblError.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                cid = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int64 CourseID = 0;
            Int64 ExamID = 0;
            Int64 ExamYear = 0;
            Int64 ExamCycleID = 0;
            Int64 regcentreID = 0;
            if (ddlRegionalCentre.SelectedValue != "0")
                regcentreID = Convert.ToInt64(ddlRegionalCentre.SelectedValue);
            if (ddlflCourse.SelectedValue != "0")
                CourseID = Convert.ToInt64(ddlflCourse.SelectedValue);
            if (ddlflExamCycle.SelectedValue != "0")
                ExamCycleID = Convert.ToInt64(ddlflExamCycle.SelectedValue);
            if (ddlflExamYear.SelectedValue != "0")
                ExamYear = Convert.ToInt64(ddlflExamYear.SelectedValue);
            if (ddlflExam.SelectedValue != "0")
                ExamID = Convert.ToInt64(ddlflExam.SelectedValue);
            ucSearchBar.AutoCompleteContextKey = ddlflCourse.SelectedValue + "," + ddlflExam.SelectedValue + "," + ddlRegionalCentre.SelectedValue;
            upbreadsearch.Update();
            var ExamResults = from s in context.CertificateExamApplications
                              where s.RollNumber != null && s.RegionalCenterID == regcentreID && s.ExamID == ExamID
                              select new
                              {
                                  ID = s.ID,
                                  appNo = s.Number,
                                  appDate = s.ApplicationDate,
                                  CourseID = s.CourseID,
                                  ExamCycleID = s.Exam.ExaminationCycleID,
                                  ExamYear = s.Exam.ExamYear,
                                  ExamID = s.ExamID,
                                  Rollno = s.RollNumber,
                                  examName = s.Exam.Name,
                                  regCentreID = s.RegionalCenterID,
                                  Name = s.Name,
                                  Father = (s.FatherName != null && s.MotherName != null) ? s.FatherName : s.GuardianName,
                                  Result = s.ResultGrade.Code,
                                  ExamDate = s.Exam.ExamStartDate
                              };

            if (!String.IsNullOrEmpty(searchString))
            {
                ExamResults = ExamResults.Where(s => s.Rollno.ToUpper().Contains(searchString));
            }
            if (ExamID != 0)
            {
                ExamResults = ExamResults.Where(s => s.ExamID == ExamID);
            }
            else
            {
                return;
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            ExamResults = ExamResults.OrderByDescending(s => s.ID);
                        else
                            ExamResults = ExamResults.OrderBy(s => s.ID);
                        break;
                    case "appNo":
                        if (sortOrder == "DESC")
                            ExamResults = ExamResults.OrderByDescending(s => s.appNo);
                        else
                            ExamResults = ExamResults.OrderBy(s => s.appNo);
                        break;
                    case "appDate":
                        if (sortOrder == "DESC")
                            ExamResults = ExamResults.OrderByDescending(s => s.appDate);
                        else
                            ExamResults = ExamResults.OrderBy(s => s.appDate);
                        break;
                    case "Rollno":
                        if (sortOrder == "DESC")
                            ExamResults = ExamResults.OrderByDescending(s => s.Rollno);
                        else
                            ExamResults = ExamResults.OrderBy(s => s.Rollno);
                        break;
                    case "Father":
                        if (sortOrder == "DESC")
                            ExamResults = ExamResults.OrderByDescending(s => s.Father);
                        else
                            ExamResults = ExamResults.OrderBy(s => s.Father);
                        break;

                    case "Name":
                        if (sortOrder == "DESC")
                            ExamResults = ExamResults.OrderByDescending(s => s.Name);
                        else
                            ExamResults = ExamResults.OrderBy(s => s.Name);
                        break;
                    default:
                        ExamResults = ExamResults.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(ExamResults, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No Record Found";
                lblError.Visible = true;
            }
            UnsendSMS(ExamID, regcentreID);
            UpdatePanelSMS.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool CheckDuplicateRollNumber(DataTable UploadDataSheet, int CourseCategoryID, int CourseID, int ExamID, int regCentreID)
    {
        //Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        //Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
        //Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
        //Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
        //Int32 regCentreID = Convert.ToInt32(ddlRc.SelectedValue);
        StringBuilder ErrorMsg = new StringBuilder();
        string RcCode = "", CourseCode = "", ExamYear = "", ExamMonth = "", RollNumberPattern = "";
        //int RcId = 0;
        //string accessConnectionString = "";
        //if (ext.ToUpper() == ".MDB")
        //{ //accessConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;", path);
        //    accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        //}
        //else if (ext.ToUpper() == ".ACCDB")
        //    accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        //OleDbConnection connection = new OleDbConnection();
        //connection.ConnectionString = accessConnectionString;
        //connection.Open();
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                RcCode = context.RegionalCenters.Find(regCentreID).Code;
                CourseCode = context.Courses.Find(CourseID).Code;
                var exam = context.Exams.Find(ExamID);
                ExamMonth = exam.ExamMonth.ToString();
                if (ExamMonth.Length == 1)
                { ExamMonth = "0" + ExamMonth; }
                ExamYear = exam.ExamYear.ToString().Substring(2, 2);
            }
            //--RollNumberPattern----RcCode ExamYear ExamMonth NumberSeries------------
            if (CourseCode == "CCC")
                RollNumberPattern = RcCode + ExamYear + ExamMonth;
            else if (CourseCode == "BCC")
                RollNumberPattern = RcCode + "B" + ExamYear + ExamMonth;
            else if (CourseCode == "CCCP")
                RollNumberPattern = RcCode + "C" + ExamYear + ExamMonth;
            else if (CourseCode == "ECC")
                RollNumberPattern = RcCode + "EC" + ExamYear + ExamMonth;
            else if (CourseCode == "POS-EXAM")
                RollNumberPattern = RcCode + "I" + ExamYear + ExamMonth;
            else
            { ValidPatternLbl.Text = "Not Defined"; return true; }
            ValidPatternLbl.Text = RollNumberPattern + "XXXXXX";
            //-------------------------------------------------------------------------

            //[MS Access;Database=" + Access + "].[Exam_Database]", connection);
            //OleDbCommand command = new OleDbCommand("select Rollno from [MS Access;Database=" + path + "].[Exam_Database]", connection);
            //OleDbDataAdapter dtadpt = new OleDbDataAdapter(command);
            //DataTable UploadDataSheet = new DataTable();
            //dtadpt.Fill(UploadDataSheet);

            //------------------CHECK 1----------------------------------------------
            var InvalidRollNumberPattern = from tbl in UploadDataSheet.AsEnumerable()
                                           let Roll_Number = tbl.Field<string>("Rollno")
                                           where !Roll_Number.StartsWith(RollNumberPattern)
                                           select Roll_Number;
            if (InvalidRollNumberPattern.Count() > 0)
            {
                ErrorMsg.Append("<b>InValid Roll Number Pattern:</b></br>  ");
                foreach (string InvalidRolls in InvalidRollNumberPattern)
                {
                    ErrorMsg.Append(InvalidRolls + ", ");
                }
                InvalidNumberLbl.Text = ErrorMsg.ToString();
                return true;
            }
            //------------------END CHECK 1-------------------------------------------

            //------------------CHECK 2-----------------------------------------------
            IEnumerable<string> DuplicatesRollInUploadedSheet = UploadDataSheet.AsEnumerable().GroupBy(r => r["Rollno"], (key, g) => new { Rollno = key.ToString(), Count = g.Count() }).Where(s => s.Count > 1).Select(p => p.Rollno);
            if (DuplicatesRollInUploadedSheet.Count() > 0)
            {
                ErrorMsg.Append("<b>Duplicate Roll Numbers in your Uploaded File: </b></br>  ");
                foreach (string DRolls in DuplicatesRollInUploadedSheet)
                {
                    ErrorMsg.Append(DRolls + ", ");
                }
                InvalidNumberLbl.Text = ErrorMsg.ToString();
                return true;
            }
            //----------------- END CHECK 2--------------------------------------------

            //------------------CHECK 3------------------------------------------------
            string RollNumberPatternLike = RollNumberPattern + "%";
            DataTable RollNumberTable = EConnect.Utils.Data.DbUtility.GetDataTable("SELECT Roll_Number 'Rollno' FROM Certificate_Exam_Application where Regional_Center_ID = '" + regCentreID + "' and Roll_Number is not null and Roll_Number like '" + RollNumberPatternLike + "' ", con, null, CommandType.Text, true);
            if (RollNumberTable.Rows.Count > 0)
            {
                IEnumerable<DataRow> RollInDatabase = RollNumberTable.AsEnumerable();
                IEnumerable<DataRow> RollInUploadedSheet = UploadDataSheet.AsEnumerable();
                IEnumerable<DataRow> RollInIntersection = RollInDatabase.Intersect(RollInUploadedSheet, DataRowComparer.Default);
                if (RollInIntersection.Count() > 0)
                {
                    ErrorMsg.Append("<b>Duplicate Roll Numbers exists in database: </b></br>  ");
                    foreach (DataRow CommonRolls in RollInIntersection)
                    {
                        ErrorMsg.Append(CommonRolls.Field<string>("Rollno") + ", ");
                    }
                    InvalidNumberLbl.Text = ErrorMsg.ToString();
                    return true;
                }
            }
            //------------------END CHECK 3--------------------------------------------
            return false;
        }
        catch (Exception ex) { throw ex; }
        finally
        {
            con.Close();
            //connection.Close();
            //connection.Dispose();
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            if (ddlflCourse.SelectedValue != "0" && ddlflExam.SelectedValue != "0")
            {
                BindGridView();
            }

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
            //tblNavLinks.Visible = true;
            divprint.Visible = false;
            FillCategories();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Upload Admit Card Data";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Admit Cards Details", "", ""));
        }
        else
        {
            //BreadCrumb1.RemoveLastBreadCrumbItem();
            if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
            {
                Response.Redirect("AdmitCardUpload.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&RegCentreID=" + Request.QueryString["RegCentreID"], true);
                //Response.Redirect("AdmitCardUpload.aspx", true);
            }
            else
            {
                Response.Redirect("AdmitCardUpload.aspx", true);
            }
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
            if (ddlflCourse.SelectedValue != "0" && ddlflExam.SelectedValue != "0")
            {
                //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Certificate Exam Result: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/ResultImports.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue, ""));
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Upload Admit Card Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/AdmitCardUpload.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
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
            ddlflCourse.SelectedValue = "0";
            ddlflExamCycle.SelectedValue = "0";
            ddlflExamYear.SelectedValue = "0";
            ddlflExam.SelectedValue = "0";
            ddlRegionalCentre.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
                HyperLink hl1 = (HyperLink)e.Row.Cells[1].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue);

                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue);

                HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue);

                HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue);

                HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int32 courseid = 0;
            Int32 examid = 0;
            Int32 regcentreid = 0;
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                courseid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                examid = Convert.ToInt32(keys[1]);
            if (!String.IsNullOrEmpty(keys[2]))
                regcentreid = Convert.ToInt32(keys[2]);
            string searchString = prefixText.Trim().ToUpper();
            var ExamResult = from s in context.CertificateExamApplications
                             where s.CourseID == courseid && s.ExamID == examid && s.RegionalCenterID == regcentreid
                             select new { Name = s.RollNumber };
            if (!String.IsNullOrEmpty(searchString))
            {
                ExamResult = ExamResult.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            ExamResult = ExamResult.OrderBy(s => s.Name).Distinct().Take(count);
            foreach (var c in ExamResult)
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
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
        ddlExamCycle.Items.Clear();
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlRc.Items.Clear();
        ddlExamCycle.Items.Insert(0, "--Select One--");
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Insert(0, "--Select One--");
        FillCourses();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        //Response.Write(flUpload.FileName);
        string filepath = Server.MapPath("../UploadedFiles");
        flUpload.SaveAs(filepath + "/" + flUpload.FileName);
        string path = (filepath + "/" + flUpload.FileName);
        flpath.Value = path;
        string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        string accessConnectionString = "";
        //string sExcelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilePath + ";Extended Properties=" + "\"Excel 8.0;HDR=YES;\"";
        if (ext.ToUpper() == ".MDB")
            accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        else if (ext.ToUpper() == ".ACCDB")
            accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;HDR=Yes'", path);
        else
        {
            ShowAlert("Please Choose .MDB/.ACCDB Extension Database", true);
            return;
        }
        OleDbConnection connection = new OleDbConnection();
        connection.ConnectionString = accessConnectionString;
        connection.Open();
        //[MS Access;Database=" + Access + "].[Exam_Database]", connection);
        OleDbCommand command = new OleDbCommand("select * from [MS Access;Database=" + path + "].[Exam_Database]", connection);
        OleDbDataReader dr = command.ExecuteReader();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 regCentreID = Convert.ToInt32(ddlRc.SelectedValue);
                //CertificateExamResult rs;
                CertificateExamApplication rs;
                ResultGrade rg = new ResultGrade();
                RegionalCenter rc = new RegionalCenter();
                while (dr.Read())
                {
                    if (dr["Rollno"] != null && dr["cent_allot"] != null && dr["cent_add"] != null && dr["examDate"] != null && dr["batch"] != null && dr["rep_time"] != null)
                    {

                        Int32 applicationID = Convert.ToInt32(dr["Application_ID"].ToString());
                        rs = new CertificateExamApplication();
                        rs = context.CertificateExamApplications.Where(s => s.ID == applicationID).FirstOrDefault();
                        if (rs != null)
                        {
                            if (rs.RollNumber == null && rs.RegionalCenterID == regCentreID)
                            {
                                rs.RollNumber = dr["Rollno"].ToString();
                                rs.ExamCentreName = dr["cent_allot"].ToString();
                                rs.ExamCentreAddress = dr["cent_add"].ToString();
                                rs.DateOfExam = Convert.ToDateTime(dr["examDate"]);
                                rs.ExamBatchNumber = dr["batch"].ToString();
                                rs.ReportingTime = dr["rep_time"].ToString();
                                rs.UpdatedOn = DateTime.Now;
                                rs.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                context.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                ShowAlert("Data Already Uploaded", true);
                            }
                        }
                    }

                }
                context.SaveChanges();
                string message = "Admit Card Data Uploaded Successfully";
                ShowAlert(message, true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            dr.Close();
            dr.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            System.IO.File.Delete(path);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.RemoveLastBreadCrumbItem();
        if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
        {
            Response.Redirect("AdmitCardUpload.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&RegCentreID=" + Request.QueryString["RegCentreID"], true);
        }
        else
        {
            Response.Redirect("AdmitCardUpload.aspx", true);
        }
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamCycle.Items.Clear();
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlRc.Items.Clear();
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Insert(0, "--Select One--");
        FillExamCycle();
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Clear();
        ddlRc.Items.Insert(0, "--Select One--");
        FillExamYear();
    }
    protected void ddlflCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExamCycle.Items.Clear();
        ddlflExamYear.Items.Clear();
        ddlflExam.Items.Clear();
        ddlflExamYear.Items.Insert(0, "--Select One--");
        ddlflExam.Items.Insert(0, "--Select One--");
        ddlRegionalCentre.Items.Clear();
        ddlRegionalCentre.Items.Insert(0, "--Select One--");
        FillFilterExamCycle();
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamName.Items.Clear();
        ddlRc.Items.Clear();
        ddlRc.Items.Insert(0, "--Select One--");
        FillExamName();
    }
    protected void ddlflExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExamYear.Items.Clear();
        ddlflExam.Items.Clear();
        ddlflExam.Items.Insert(0, "--Select One--");
        ddlRegionalCentre.Items.Clear();
        ddlRegionalCentre.Items.Insert(0, "--Select One--");
        FillFilterExamYear();
    }
    protected void ddlflExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExam.Items.Clear();
        ddlRegionalCentre.Items.Clear();
        ddlRegionalCentre.Items.Insert(0, "--Select One--");
        FillFilterExam();
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRc.Items.Clear();
        FillRegionalCenter();
    }
    protected void ddlflExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRegionalCentre.Items.Clear();
        FillFilterRegionalCenter();
    }
    protected void Lnkcentrechange_Click(object sender, EventArgs e)
    {
        try
        {
            Lnkcentrechange.Visible = false;
            divchangedata.Visible = true;
            trchangedetail.Visible = false;
            divdata.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Bcancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divchangedata.Visible = false;
            trchangedetail.Visible = true;
            Lnkcentrechange.Visible = true;
            divdata.Visible = false;
            ShowEditMode();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            StringBuilder instcompleteaddress = new StringBuilder();
            Int32 applicationID = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 examid = Convert.ToInt32(Request.QueryString["ExamID"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["CourseID"]);
            String batchnumber = Txtbatchno.Text.Trim();
            String reportingtime = Txtreptime.Text.Trim();
            String examcentrename = Txtexamcentrename.Text.Trim();
            String examcentreaddress = Txtexamcentreaddress.Text.Trim();
            DateTime examdate = Convert.ToDateTime(Txtexamdate.Text);
            String rollnumber = Txtrollno.Text.Trim();
            if (String.IsNullOrEmpty(Txtexamcentrename.Text))
            {
                ShowAlert("Please Enter Exam Centre Name.", true);
                return;
            }
            else if (String.IsNullOrEmpty(Txtexamcentreaddress.Text))
            {
                ShowAlert("Please Enter Exam Venue Name.", true);
                return;
            }
            else if (String.IsNullOrEmpty(Txtexamdate.Text))
            {
                ShowAlert("Please Enter Exam Date.", true);
                return;
            }
            else if (!IsDate(Txtexamdate.Text))
            {
                ShowAlert("Invalid Exam Date.", true);
                return;
            }
            else if (String.IsNullOrEmpty(Txtreptime.Text))
            {
                ShowAlert("Please Enter EXAM Time.", true);
                return;
            }
            else if (String.IsNullOrEmpty(Txtbatchno.Text))
            {
                ShowAlert("Please Enter Batch Number.", true);
                return;
            }
            else if (String.IsNullOrEmpty(Txtrollno.Text))
            {
                ShowAlert("Please Enter roll number.", true);
                return;
            }
            using (EConnectContext context = new EConnectContext())
            {
                var cexam = context.CertificateExamApplications.Where(s => s.ID == applicationID && s.CourseID == courseid && s.ExamID == examid).FirstOrDefault();
                cexam.ExamCentreName = examcentrename;
                cexam.ExamCentreAddress = examcentreaddress;
                cexam.DateOfExam = examdate;
                cexam.ExamBatchNumber = batchnumber;
                cexam.ReportingTime = reportingtime;
                if (Txtrollno.Text.Substring(0, 2).ToUpper().Trim() != cexam.RegionalCenter.Code.ToUpper().Trim())
                {
                    ShowAlert("Incorrect Roll number.It should start with " + cexam.RegionalCenter.Code.ToUpper());
                    return;
                }
                else if (context.CertificateExamApplications.Any(s => s.RollNumber == rollnumber && s.ID != cexam.ID && s.ExamID == cexam.ExamID))
                {
                    ShowAlert("This Roll Number for the candidate already exists.");
                    return;
                }
                else
                {
                    cexam.RollNumber = rollnumber;
                }
                context.Entry(cexam).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            };
            ShowAlert("Admit Card details of the candidate changed successfully.");
            ShowEditMode();
            trchangedetail.Visible = true;
            Lnkcentrechange.Visible = true;
            divchangedata.Visible = false;
            divdata.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void SentBulkSMS(List<StudentList> candidateList, Int32 UserID)
    {
        StringBuilder mobilemsg = new StringBuilder();

        for (int i = 0; i < candidateList.Count(); i++)
        {
            mobilemsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + "," + " Admit Card for the " + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " Exam has been uploaded by NIELIT. Please check your E-mail regarding this.");
            try
            {
                //sending SMS 
                if (candidateList[i].MobileNo != 0)
                {
                    EConnect.NIELIT.SMS sms = new SMS(mobilemsg.ToString(), candidateList[i].MobileNo.ToString(), "1307161052928807635", SmsServiceType.SignleSMS, false);
                    int sentMessageCount;
                    sms.sendSingleSMS(out sentMessageCount);

                    //EConnect.NIELIT.SMS message = new SMS(mobilemsg, candidateList[i].MobileNo.ToString(), SmsServiceType.SignleSMS);
                    //int sentMessageCount;
                    //message.Send(out sentMessageCount);
                }
            }
            catch (Exception){ }
            mobilemsg.Clear();
        }
    }
    protected void SentBulkEmail(List<StudentList> candidateList, Int32 UserID)
    {
        StringBuilder emailmsg = new StringBuilder();

        for (int i = 0; i < candidateList.Count(); i++)
        {
            emailmsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + ",<br/><br/>" + " Admit Card for the <b>" + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " </b> Exam has been uploaded by NIELIT. You can download your admit card from <b> NIELIT Website (https://student.nielit.gov.in) </b> with your Application-Number (<b>" + candidateList[i].Appno + "</b>)");
            try
            {
                //sending Email 
                if (candidateList[i].Email.Trim().Length > 0)
                {
                    EConnect.NIELIT.Email mail1 = new Email("Admit Card Notification:NIELIT", emailmsg.ToString(), candidateList[i].Email);
                    mail1.Send();
                }
            }
            catch (Exception ex)
            {

            }
            emailmsg.Clear();
        }
    }

    protected void btnValidate_Click(object sender, EventArgs e)
    {
        EConnectContext context = new EConnectContext();
        Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
        Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
        Int32 regCentreID = Convert.ToInt32(ddlRc.SelectedValue);
        Int64 applicationID = 0;
        Int32 TotalRecords = 0, ValidateRecords = 0, NotValidateRecords = 0;
        List<StudentList> candidateList = new List<StudentList>();
        List<Int64> ValidatedAppRecords = new List<Int64>();
        StringBuilder sb = new StringBuilder();
        String appIdList = "", appIdList1 = "", appIdList2 = "", appIdList3 = "", applistUpdated = "";
        String[] arr = new String[10];
        arr[0] = "Uploaded Admit Card Data is not correct.Please Correct the data and Upload again";
        arr[1] = "Admit Card Data File is not related to selected Regional Centre";
        arr[2] = "Data Already Uploaded"; arr[3] = "does not exist";
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        divValidateData.Visible = true;
        btnValidate.Visible = false;
        BreadCrumb1.Render();

        String filepath = Server.MapPath("../UploadedFiles");
        flUpload.SaveAs(filepath + "/" + flUpload.FileName);
        String path = (filepath + "/" + flUpload.FileName);
        String ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        String accessConnectionString = "";
        if (ext.ToUpper() == ".MDB")
        { //accessConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;", path);
            accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        }
        else if (ext.ToUpper() == ".ACCDB")
        { //accessConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;", path);
            accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        }
        else { ShowAlert("Please Choose .MDB/.ACCDB Extension Database", true); return; }


        OleDbConnection connection = new OleDbConnection();
        connection.ConnectionString = accessConnectionString;
        connection.Open();
        //[MS Access;Database=" + Access + "].[Exam_Database]", connection);
        OleDbCommand command = new OleDbCommand("select Application_ID, Rollno, cent_allot, cent_add, examDate, batch, rep_time from [MS Access;Database=" + path + "].[Exam_Database]", connection);
        OleDbDataAdapter dtadpt = new OleDbDataAdapter(command);
        DataTable UploadDataSheet = new DataTable();
        dtadpt.Fill(UploadDataSheet);
        TotalRecords = UploadDataSheet.Rows.Count;

        try
        {
            if (!CheckDuplicateRollNumber(UploadDataSheet, CourseCategoryID, CourseID, ExamID, regCentreID))
            {
                for (int i = 0; i < TotalRecords; i++)
                {
                    applicationID = Convert.ToInt64(UploadDataSheet.Rows[i]["Application_ID"]);

                    if (!String.IsNullOrEmpty(UploadDataSheet.Rows[i]["Rollno"].ToString()) && !String.IsNullOrEmpty(UploadDataSheet.Rows[i]["cent_allot"].ToString()) && !String.IsNullOrEmpty(UploadDataSheet.Rows[i]["cent_add"].ToString()) && !String.IsNullOrEmpty(UploadDataSheet.Rows[i]["examDate"].ToString()) && !String.IsNullOrEmpty(UploadDataSheet.Rows[i]["batch"].ToString()) && !String.IsNullOrEmpty(UploadDataSheet.Rows[i]["rep_time"].ToString()))
                    {
                        SqlCommand scCommand = new SqlCommand("updateRollNumbers", new SqlConnection(con.ConnectionString));
                        scCommand.CommandType = CommandType.StoredProcedure;
                        scCommand.Parameters.Add("@ApplID", SqlDbType.BigInt).Value = applicationID;
                        scCommand.Parameters.Add("@RegionalCenterID", SqlDbType.Int).Value = regCentreID;
                        scCommand.Parameters.Add("@RollNumber ", SqlDbType.VarChar, 15).Value = UploadDataSheet.Rows[i]["Rollno"].ToString();
                        scCommand.Parameters.Add("@ExamCentreName", SqlDbType.VarChar, 200).Value = UploadDataSheet.Rows[i]["cent_allot"].ToString();
                        scCommand.Parameters.Add("@ExamCentreAddress", SqlDbType.VarChar, 300).Value = UploadDataSheet.Rows[i]["cent_add"].ToString();
                        scCommand.Parameters.Add("@DateOfExam", SqlDbType.DateTime).Value = Convert.ToDateTime(UploadDataSheet.Rows[i]["examDate"]);
                        scCommand.Parameters.Add("@ExamBatchNumber", SqlDbType.VarChar, 50).Value = UploadDataSheet.Rows[i]["batch"].ToString();
                        scCommand.Parameters.Add("@ReportingTime", SqlDbType.VarChar, 20).Value = UploadDataSheet.Rows[i]["rep_time"].ToString();
                        scCommand.Parameters.Add("@UpdatedByID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"]);

                        try
                        {
                            if (scCommand.Connection.State == ConnectionState.Closed)
                            {
                                scCommand.Connection.Open();
                            }
                            var result = scCommand.ExecuteScalar();

                            #region Output Result
                            if (result.ToString() == "-1")
                            {
                                ValidateRecords = ValidateRecords + 1;
                                ValidatedAppRecords.Add(applicationID);
                                applistUpdated += applicationID.ToString() + ",";
                                if (ValidateRecords % 10 == 0 && ValidateRecords > 0)
                                    applistUpdated += WebUtility.HtmlDecode("<br/>");
                            }
                            else if (result.ToString() == "1")
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList3 += applicationID.ToString() + ",";
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList3 += WebUtility.HtmlDecode("<br/>");
                            }
                            else if (result.ToString() == "2")
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList1 += applicationID.ToString() + ",";
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList1 += WebUtility.HtmlDecode("<br/>");
                            }
                            else if (result.ToString() == "3")
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList2 += applicationID.ToString() + ",";
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList2 += WebUtility.HtmlDecode("<br/>");
                            }
                            else
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList += applicationID.ToString() + ",";
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList += WebUtility.HtmlDecode("<br/>");
                            } 
                            #endregion
                        }
                        catch (Exception)
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                            appIdList += applicationID.ToString() + ",";
                            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                appIdList += WebUtility.HtmlDecode("<br/>");
                        }
                        finally { scCommand.Connection.Close(); }
                    }
                    else
                    {
                        NotValidateRecords = NotValidateRecords + 1;
                        appIdList += applicationID.ToString() + ",";
                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                            appIdList += WebUtility.HtmlDecode("<br/>");
                    }
                }
                
                // sending sms and email
                candidateList = context.CertificateExamApplications.Where(s => ValidatedAppRecords.Contains(s.ID))
                                .Select(s => new StudentList { AppName = s.Name, Email = s.EmailAddress, Salutation = s.Salutation, MobileNo = s.MobileNumber, Appno = s.Number, ExamName = s.Exam.Name, CourseName = s.Course.Name }).ToList();           
                SentBulkEmail(candidateList, Convert.ToInt32(Session["UserID"]));
                //----------------------------------------------------------------

                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                if (ValidateRecords != 0)
                    lblUpdated.Text = "Admit Card Data  for Application ID : " + applistUpdated.TrimEnd(',').ToString() + " are  updated successfully";
                lblNotValidate.Text = NotValidateRecords.ToString();
                if (appIdList != "")
                    lblFailed.Text = "<b>" + arr[0].ToString() + " For Application ID:- </b> <br/>" + appIdList.TrimEnd(',').ToString();
                if (appIdList1 != "")
                    lblFailed.Text += "<br/> <b>" + arr[1].ToString() + " For Application ID:- </b><br/> " + appIdList1.TrimEnd(',').ToString();
                if (appIdList2 != "")
                    lblFailed.Text += "<br/> <b>" + arr[2].ToString() + " For Application ID:- </b> <br/> " + appIdList2.TrimEnd(',').ToString();
                if (appIdList3 != "")
                    lblFailed.Text += "<br/> <b>" + arr[3].ToString() + " For Application ID:- </b> <br/> " + appIdList2.TrimEnd(',').ToString();
            }
            else { throw new Exception("Incorrect or Duplicate Roll Numbers are not allowed"); }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            command.Dispose();
            connection.Close();
            con.Close();
            connection.Dispose();
            System.IO.File.Delete(path);
            context.Dispose();
        }
    }
    protected void SendSMSBtn_Click(object sender, EventArgs e)
    {
        Int64 regcentreID = 0; Int64 ExamID = 0;
        if (ddlRegionalCentre.SelectedValue != "0")
            regcentreID = Convert.ToInt64(ddlRegionalCentre.SelectedValue);
        if (ddlflExam.SelectedValue != "0")
            ExamID = Convert.ToInt64(ddlflExam.SelectedValue);

        using (EConnectContext context = new EConnectContext())
        {
            List<StudentList> candidateList = context.CertificateExamApplications.
                                              Where(s => s.ExamID == ExamID && s.RegionalCenterID == regcentreID && s.SMSSent == false && s.RollNumber != null).
                                              Select(x => new StudentList
                                                {
                                                    AppName = x.Name,
                                                    Email = x.EmailAddress,
                                                    Salutation = x.Salutation,
                                                    MobileNo = x.MobileNumber,
                                                    Appno = x.Number,
                                                    ExamName = x.Exam.Name,
                                                    CourseName = x.Course.Name
                                                }).Take(5000).ToList();

            for (int i = 0; i < candidateList.Count(); i++)
            {
                try
                {
                    string Appno = candidateList[i].Appno;
                    StringBuilder mobilemsg = new StringBuilder();
                    mobilemsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + "," + " Admit Card for the " + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " Exam has been uploaded by NIELIT. Please check your E-mail regarding this.");

                    EConnect.NIELIT.SMS message = new SMS(mobilemsg.ToString(), candidateList[i].MobileNo.ToString(), "1307161052928807635", SmsServiceType.SignleSMS, false);
                    int sentMessageCount;
                    message.sendSingleSMS(out sentMessageCount);
                    mobilemsg.Clear();
                    if (sentMessageCount == 1)
                    {
                        try
                        {
                            var app = context.CertificateExamApplications.Where(p => p.Number == Appno).FirstOrDefault();
                            app.SMSSent = true;
                            context.Entry(app).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        catch (Exception ex) { throw ex; }
                    }
                }
                catch (Exception ex) { throw ex; }
            }
            UnsendSMS(ExamID, regcentreID);
            UpdatePanelSMS.Update();
        }
    }
    public class StudentList
    {
        public Int64 MobileNo
        {
            get;
            set;
        }
        public String Email
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
        public String ExamName
        {
            get;
            set;
        }
        public String CourseName
        {
            get;
            set;
        }
        public String Appno
        {
            get;
            set;
        }
    }
}