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


public partial class CourseExamDataupload_Revised : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
               // Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCategories();
                    bindState();
                    ShowEditMode();
                    Int32 examid = Convert.ToInt32(Request.QueryString["ExamID"]);
                    Int32 courseid = Convert.ToInt32(Request.QueryString["CourseID"]);
                    BindExamCentre(examid);
                    BindInstituteName(courseid);
                    ddlinstitutename.Items.Insert(1, "Others");
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
                        ddlmodulefilter.SelectedValue = Request.QueryString["modulefilter"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Revised Admit Card Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "HO/CourseExamDataUpload_Revised.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Revised Admit Card Data", "HO/CourseExamDataUpload_Revised.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria to View Admit Card Records";
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
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               join c in context.Courses on p.ID equals c.CourseCategoryID
                               where c.CourseTypeID == CourseType
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                Category = Category.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
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
                var ExamName = (from p in context.CourseExamApplications
                                join c in context.Exams on
                                p.ExamID equals c.ID
                                where p.Exam.ExamYear == ExamYear && p.CourseID == CourseID && c.DateOfPublishingOfResult == null && c.ExaminationCycleID == ExamCycleID
                                orderby (c.Name) ascending
                                select new { ValueField = p.Exam.ID, TextField = p.Exam.Name }).Distinct().ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                Int32 currentyear = Convert.ToInt32(DateTime.Now.Year);
                var examYear = (from p in context.Exams
                                join q in context.CourseExamApplications
                                on p.ID equals q.ExamID
                                where (p.CourseID == CourseID && p.CourseCategoryID == CourseCategoryID && p.ExaminationCycleID == ExamCycleID && (p.ExamYear >= currentyear || p.ExamYear< currentyear) && p.DateOfPublishingOfResult == null)
                                orderby (p.ExamYear) descending
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examYear, lst);
            };
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
                Int32 CourseID = Convert.ToInt32(ddlflCourse.SelectedValue);
                Int32 currentyear = Convert.ToInt32(DateTime.Now.Year);
                var examYear = (from p in context.Exams
                                join q in context.CourseExamApplications
                                on p.ID equals q.ExamID
                                where (p.CourseID == CourseID && p.ExaminationCycleID == ExamCycleID && p.ExamYear >= currentyear)
                                orderby (p.ExamYear) descending
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamYear, examYear, lst);
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
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindExamCentre(Int32 ExamID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var examcentre = from p in context.ExamCenters
                                 join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                 where s.ExamID == ExamID
                                 orderby p.Name
                                 select new
                                 {
                                     ValueField = p.ID,
                                     TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                     examCentreTypeID = s.ExamCentreTypeID
                                 };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamcentre, examcentre.Distinct(), new ListItem("--Select Exam Centre--", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindInstituteName(Int32 courseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var institute = from s in context.CourseExamApplications
                                where s.CourseID == courseID && s.PracticalInstituteName != null
                                select new
                                {
                                    ValueField = s.PracticalInstituteName + "|" + s.PracticalInstituteAddress.Replace("<br/>", "!"),
                                    TextField = (s.PracticalInstituteName + " ( " + s.PracticalInstituteAddress + " ) ").ToLower().Replace("<br/>", "")
                                };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, institute.Distinct().OrderBy(t => t.TextField), new ListItem("--Select Institute Name --", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseTypeID == CourseTypeID
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }


                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflCourse, CourseList, lst);
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
    protected void FillFilterExam()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamYear = Convert.ToInt32(ddlflExamYear.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlflCourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlflExamCycle.SelectedValue);
                var ExamName = (from p in context.CourseExamApplications
                                join c in context.Exams on
                                    p.ExamID equals c.ID
                                where p.Exam.ExamYear == ExamYear && p.CourseID == CourseID && c.ExaminationCycleID == ExamCycleID
                                select new { ValueField = p.Exam.ID, TextField = p.Exam.Name }).Distinct().ToList();
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
            lblHeading.Text = "Upload Revised Admit Card Data";
            tblNavLinks.Visible = true;
            //imgSampleDoc.Visible = false;
            tbls2.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 applicationID = Convert.ToInt32(Request.QueryString["Key"]);
                var application = (from p in context.CourseExamApplications
                                   where p.ID == applicationID
                                   select p).FirstOrDefault();
                var examCentreName = (from s in context.ExamCenters
                                      where s.ID == application.AllottedExamCentreID
                                      select s).FirstOrDefault();
                flUpload.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                lblAppNumber.Text = application.Number.ToString();
                lblAppDate.Text = application.ApplicationDate.ToString("dd-MMM-yyyy");
                lblCourse.Text = application.Course.Name + " ( " + application.CourseCategory.Code + " ) ";
                lblExamDetail.Text = application.Exam.Name;
                lblRollno.Text = application.RollNumber.HasValue ? application.RollNumber.ToString() : "NA";
                Txtrollno.Text = application.RollNumber.HasValue ? application.RollNumber.ToString() : "NA";
                if (application.AllottedExamCentreID.HasValue && application.AllottedExamVenueID.HasValue)
                {
                    ddlexamcentre.SelectedValue = application.AllottedExamCentreID.ToString();
                    Int32 examcentreid = application.AllottedExamCentreID.Value;
                    Int32 courseid = application.CourseID;
                    BindExamVenue(examcentreid, courseid);
                    ddlexamvenue.SelectedValue = application.AllottedExamVenueID.ToString();
                }
                lblCandidate.Text = application.Candidate.Salutation + " " + GetInitCap(application.Candidate.Name);
                if (string.IsNullOrEmpty(application.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(application.Candidate.GuardianName))
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    lblFather.Text = "Mr. " + GetInitCap(application.Candidate.FatherName);
                    lblMother.Text = "Mrs. " + GetInitCap(application.Candidate.MotherName);
                }
                else
                {
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    LblGuardianName.Text = GetInitCap(application.Candidate.GuardianName);
                }
                // Admit Card Details.
                if (application.NumberOfTheoryModulesApplied != 0 && application.RollNumber != null)
                {
                    trtheory.Visible = true;
                    trtheory1.Visible = true;
                    trtheory2.Visible = true;
                    trtheory3.Visible = true;
                    trtheory4.Visible = true;
                    trtheory5.Visible = true;
                    lblExamCentreName.Text = examCentreName.Name.ToString() + " ( " + examCentreName.Code + " ) ";
                    var examcentreaddress = (from c in context.ExamVenues
                                             join d in context.ExamCenters
                                                 on c.ExamCentreID equals d.ID
                                             where c.ID == application.AllottedExamVenueID && d.ID == application.AllottedExamCentreID
                                             select new
                                             {
                                                 venuename = c.Name,
                                                 addline1 = c.AddressLine1,
                                                 addline2 = c.AddressLine2,
                                                 city = c.City,
                                                 state = c.State.Name,
                                                 pincode = c.PinCode
                                             }).FirstOrDefault();
                    if (examcentreaddress != null)
                    {
                        lblexamvenuecode.Text = examcentreaddress.venuename;
                        lblAddress.Text = examcentreaddress.venuename + WebUtility.HtmlDecode("<br/>");
                        lblAddress.Text += examcentreaddress.addline1 + WebUtility.HtmlDecode("<br/>");
                        lblAddress.Text += string.IsNullOrEmpty(examcentreaddress.addline2) == false && !string.IsNullOrWhiteSpace(examcentreaddress.addline2) ? examcentreaddress.addline2 + WebUtility.HtmlDecode("<br/>") : "";
                        lblAddress.Text += examcentreaddress.city + WebUtility.HtmlDecode("<br/>");
                        lblAddress.Text += GetInitCap(examcentreaddress.state) + WebUtility.HtmlDecode(", ");
                        lblAddress.Text += examcentreaddress.pincode.HasValue ? "Pincode:-" + examcentreaddress.pincode.Value.ToString() : "NA";
                    }
                }
                else
                {
                    trtheory.Visible = false;
                    trtheory1.Visible = false;
                    trtheory2.Visible = false;
                    trtheory3.Visible = false;
                    trtheory4.Visible = false;
                    trtheory5.Visible = false;
                    rblOption.Items.FindByValue("T").Enabled = false;
                }
                // Practical Admit Card Details.
                if (application.NumberOfPracticalModulesApplied != 0 && application.PracticalOfficeRefNumber != null)
                {
                    trprac.Visible = true;
                    trprac1.Visible = true;
                    trprac2.Visible = true;
                    trprac4.Visible = true;
                    Int32 practical = Convert.ToInt32(enmModuleType.Practical);
                    lbpracticalinsname.Text = application.PracticalInstituteName;
                    Lbpracinstaddress.Text = application.PracticalInstituteAddress;
                    // selecting institutename
                    ddlinstitutename.SelectedValue = application.PracticalInstituteName + "|" + application.PracticalInstituteAddress.Replace("<br/>", "!");
                    Txtpracofficerefno.Text = application.PracticalOfficeRefNumber;
                    var examdate = (from c in context.CourseExamApplications
                                    join f in context.CourseExamApplicationDetails
                                    on c.ID equals f.CourseExamApplicationID
                                    where f.Module.ModuleTypeID == practical && f.CourseExamApplicationID == applicationID && f.CourseID == application.CourseID && f.ExamID == application.ExamID
                                    orderby f.PracticalExamDate ascending, f.PracticalExamReportingTime descending
                                    select new
                                    {
                                        modulename = c.Course.Code + " / " + f.Module.ShortName,
                                        //practicaldate = f.PracticalExamDate,
                                        batch = f.PracticalExamBatchNumber,
                                        reptime = f.PracticalExamReportingTime
                                    }).ToList();
                    gvpracexam.DataSource = examdate;
                    gvpracexam.DataBind();
                }
                else
                {
                    trprac.Visible = false;
                    trprac1.Visible = false;
                    trprac2.Visible = false;
                    trprac4.Visible = false;
                    rblOption.Items.FindByValue("P").Enabled = false;
                }
                //lblDateofExam.Text = application.Exam.ExamStartDate.ToString("dd-MMM-yyyy");
                hlAppDetail.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?AppID=" + applicationID.ToString() + "&candidateID=" + application.CandidateID) + "');");
                hldownloadadmitcard.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseAdmitCard_Ver5.aspx?id=" + application.CourseID.ToString() + "&Appid=" + application.ID) + "');");
                hldownloadpracticaladmitcard.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CoursePracticalAdmitCard_Ver2.aspx?id=" + application.CourseID.ToString() + "&Appid=" + application.ID) + "');");
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(application.RegistrationNumber + "-" + "(" + application.Candidate.Name + ")", "HO/CourseExamDataUpload_Revised.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
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
            Int32 modulefilter = Convert.ToInt32(ddlmodulefilter.SelectedValue);
            if (ddlflCourse.SelectedValue != "0")
                CourseID = Convert.ToInt64(ddlflCourse.SelectedValue);
            if (ddlflExamCycle.SelectedValue != "0")
                ExamCycleID = Convert.ToInt64(ddlflExamCycle.SelectedValue);
            if (ddlflExamYear.SelectedValue != "0")
                ExamYear = Convert.ToInt64(ddlflExamYear.SelectedValue);
            if (ddlflExam.SelectedValue != "0")
                ExamID = Convert.ToInt64(ddlflExam.SelectedValue);
            ucSearchBar.AutoCompleteContextKey = ddlflCourse.SelectedValue + "," + ddlflExam.SelectedValue;
            upbreadsearch.Update();
            var Examadmitdata = from s in context.CourseExamApplications.AsNoTracking()
                                select new
                                {
                                    ID = s.ID,
                                    appNo = s.Number,
                                    appDate = s.ApplicationDate,
                                    CourseID = s.CourseID,
                                    ExamCycleID = s.Exam.ExaminationCycleID,
                                    ExamYear = s.Exam.ExamYear,
                                    ExamID = s.ExamID,
                                    Rollno = SqlFunctions.StringConvert((double)s.RollNumber),
                                    examName = s.Exam.Name,
                                    Name = s.Candidate.Name,
                                    Father = (s.Candidate.FatherName != null && s.Candidate.MotherName != null) ? s.Candidate.FatherName : s.Candidate.GuardianName,
                                    ExamDate = s.Exam.ExamStartDate,
                                    RegNo = SqlFunctions.StringConvert((double)s.RegistrationNumber),
                                    Profficerefno = s.PracticalOfficeRefNumber
                                };
            if (!String.IsNullOrEmpty(searchString))
            {
                Examadmitdata = Examadmitdata.Where(s => s.Name.ToUpper().Contains(searchString) || s.RegNo.ToUpper().Contains(searchString) || s.Rollno.ToUpper().Contains(searchString));
            }

            if (CourseID != 0 && ExamCycleID != 0 && ExamYear != 0 && ExamID != 0)
            {
                Examadmitdata = Examadmitdata.Where(s => s.CourseID == CourseID && s.ExamCycleID == ExamCycleID && s.ExamYear == ExamYear && s.ExamID == ExamID);
            }
            if (modulefilter == 0)
            {
                Examadmitdata = Examadmitdata.Where(s => s.Rollno != null || s.Profficerefno != null);
            }
            else if (modulefilter == 1)
            {
                Examadmitdata = Examadmitdata.Where(s => s.Rollno != null && s.Profficerefno == null);
            }
            else if (modulefilter == 2)
            {
                Examadmitdata = Examadmitdata.Where(s => s.Profficerefno != null && s.Rollno == null);
            }
            else if (modulefilter == 3)
            {
                Examadmitdata = Examadmitdata.Where(s => s.Rollno != null && s.Profficerefno != null);
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
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.ID);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.ID);
                        break;
                    case "appNo":
                        if (sortOrder == "DESC")
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.appNo);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.appNo);
                        break;
                    case "appDate":
                        if (sortOrder == "DESC")
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.appDate);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.appDate);
                        break;
                    case "Rollno":
                        if (sortOrder == "DESC")
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.Rollno);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.Rollno);
                        break;
                    case "RegNo":
                        if (sortOrder == "DESC")
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.RegNo);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.RegNo);
                        break;
                    case "Father":
                        if (sortOrder == "DESC")
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.Father);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.Father);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            Examadmitdata = Examadmitdata.OrderByDescending(s => s.Name);
                        else
                            Examadmitdata = Examadmitdata.OrderBy(s => s.Name);
                        break;
                    default:
                        Examadmitdata = Examadmitdata.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(Examadmitdata, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No Record Found";
                lblError.Visible = true;
            }

            //BreadCrumb1.Render();
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
            lblHeading.Text = "Upload Revised Admit Card Data";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Admit Cards Details", "", ""));
        }
        else
        {
            if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
            {
                Response.Redirect("CourseExamDataUpload_Revised.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&modulefilter=" + Request.QueryString["modulefilter"], true);
            }
            else
                Response.Redirect("CourseExamDataUpload_Revised.aspx", true);
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
            if (ddlflCourse.SelectedValue != "0" || ddlflExam.SelectedValue != "0" || ddlflExamYear.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Upload Revised Admit Card Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "HO/CourseExamDataUpload_Revised.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue, ""));
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
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("CourseExamDataupload.aspx");
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
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue);

                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue);

                HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue);

                HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue);

                HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue);

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
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                courseid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                examid = Convert.ToInt32(keys[1]);
            string searchString = prefixText.Trim().ToUpper();
            var applications = from s in context.CourseExamApplications
                               where (s.RollNumber != null || s.PracticalOfficeRefNumber != null) && s.CourseID == courseid && s.ExamID == examid
                               select new { Name = s.Candidate.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.OrderBy(s => s.Name).Distinct();

            var applications2 = from s in context.CourseExamApplications
                                where (s.RollNumber != null || s.PracticalOfficeRefNumber != null) && s.CourseID == courseid && s.ExamID == examid
                                select new { Name = SqlFunctions.StringConvert((double)s.RollNumber).Replace(" ", "") };

            if (!String.IsNullOrEmpty(searchString))
            {
                applications2 = applications2.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            var applications3 = from s in context.CourseExamApplications
                                where (s.RollNumber != null || s.PracticalOfficeRefNumber != null) && s.CourseID == courseid && s.ExamID == examid
                                select new { Name = SqlFunctions.StringConvert((double)s.RegistrationNumber).Replace(" ", "") };

            if (!String.IsNullOrEmpty(searchString))
            {
                applications3 = applications3.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            applications = applications.Union(applications2).Union(applications3).Take(count);
            foreach (var c in applications)
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
        ddlExamCycle.Items.Insert(0, "--Select One--");
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        FillCourses();
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamCycle.Items.Clear();
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        FillExamCycle();
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlExamName.Items.Insert(0, "--Select One--");
        FillExamYear();
    }
    protected void ddlflCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExamCycle.Items.Clear();
        ddlflExamYear.Items.Clear();
        ddlflExam.Items.Clear();
        ddlflExamYear.Items.Insert(0, "--Select One--");
        ddlflExam.Items.Insert(0, "--Select One--");
        FillFilterExamCycle();
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamName.Items.Clear();
        FillExamName();
    }
    protected void ddlflExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExamYear.Items.Clear();
        ddlflExam.Items.Clear();
        ddlflExam.Items.Insert(0, "--Select One--");
        FillFilterExamYear();
    }
    protected void ddlflExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExam.Items.Clear();
        FillFilterExam();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.RemoveLastBreadCrumbItem();       
        Response.Redirect("CourseExamDataUpload_Revised.aspx", true);
    }

    protected void btnValidate_Click(object sender, EventArgs e)
    {
        divValidateData.Visible = true;
        btnValidate.Visible = false;
        BreadCrumb1.Render();
        string[] arr = new string[10];
        StringBuilder appIdList = new StringBuilder();
        StringBuilder appIdList2 = new StringBuilder();
        StringBuilder appIdList3 = new StringBuilder();
        StringBuilder applistUpdated = new StringBuilder();
        StringBuilder instaddress = new StringBuilder();
        arr[0] = "Uploaded Admit Card Data is not correct.Please Correct the data and Upload again.";
        arr[2] = "Data Already Uploaded.";
        arr[3] = "Invalid Registration/Exam Centre/Exam Venue Data.";
        arr[4] = "Uploaded Practical Admit Card Data is not correct.Please Correct the data and Upload again.";
        arr[5] = "Invalid Registration/Institute Name.";

        try
        {


            //command = new OleDbCommand("select * from [cand$]", connection);
            //dr = command.ExecuteReader();
            int TotalRecords = 0;
            int ValidateRecords = 0;
            int NotValidateRecords = 0;
            int alreadyUploadedRecords = 0;
            int incompletedRecords = 0;
            int uniqueregno = 0;
            Int64 RegistrationNumber = 0;
            Int64 OldRegistrationNumber = 0;
            Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
            Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 ModuleType = Convert.ToInt32(Ddlmoduletype.SelectedValue);

            #region Theory
            if (ModuleType == 1)
            {
                string filepath = Server.MapPath("../UploadedFiles");
                string fileName = "fl" + DateTime.Now.ToString("ddMMyyyyhhhhss") + "_" + flUpload.FileName;
                flUpload.SaveAs(filepath + "/" + fileName);
                string path = (filepath + "/" + fileName);
                string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
                string excelConnectionString = "";
                OleDbCommand command;
                OleDbConnection connection = new OleDbConnection();
                OleDbDataReader dr;

                if (ext.ToUpper() == ".XLS")
                    excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", path);
                else if (ext.ToUpper() == ".XLSX")
                    excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.15.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", path);
                else
                {
                    ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
                    return;
                }
                connection.ConnectionString = excelConnectionString;
                connection.Open();
                command = new OleDbCommand("select * from [cand$]", connection);
                dr = command.ExecuteReader();
                using (EConnectContext context = new EConnectContext())
                {
                    String ExamCentreCode = "";
                    String ExamvenueCode = "";
                    Int32 blankRowsCount = 0;
                    while (dr.Read())
                    {
                        if (CommonFunctions.IsNumeric(dr[3].ToString()))
                        {
                            TotalRecords = TotalRecords + 1;
                            RegistrationNumber = Convert.ToInt64(dr[3].ToString());
                        }
                        else
                        {
                            blankRowsCount++;
                            if (blankRowsCount < 3)
                                continue;
                            else
                                break;
                        }
                        ExamCentreCode = dr[7].ToString(); // c_code (center code)
                        ExamvenueCode = dr[8].ToString(); // loc_alloted(location code)
                        if (!String.IsNullOrEmpty(dr[4].ToString()) && !String.IsNullOrEmpty(dr[7].ToString()) && !String.IsNullOrEmpty(dr[8].ToString()) && !String.IsNullOrEmpty(dr[6].ToString()))
                        {
                            CourseExamApplication rs = context.CourseExamApplications.Where(s => s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID && s.CourseID == CourseID).FirstOrDefault();
                            ExamCenter examcentre = context.ExamCenters.Where(s => s.Code.ToUpper() == ExamCentreCode.ToUpper()).FirstOrDefault();
                            //ExamVenue examvenue = context.ExamVenues.Where(s => s.Code.ToUpper() == ExamvenueCode.ToUpper() && s.CourseID == CourseID ).FirstOrDefault();
                            var examvenue = (from m in context.ExamVenues where m.Code.ToUpper() == ExamvenueCode.ToUpper() && m.CourseID == CourseID && m.ExamCentreID == examcentre.ID select new { Id = m.ID }).FirstOrDefault();
                            //ExamVenue examvenue = context.ExamVenues.Where(s => s.Code.ToUpper() == ExamvenueCode.ToUpper()).FirstOrDefault();
                            if (rs != null && examcentre != null && examvenue != null && CourseID != 1)
                            {
                                if (rs.RollNumber.HasValue == false)
                                {
                                    ValidateRecords = ValidateRecords + 1;
                                    rs.RollNumber = Convert.ToInt64(dr[4]);
                                    rs.AllottedExamCentreID = examcentre.ID;
                                    rs.AllottedExamVenueID = examvenue.Id;
                                    rs.AllottedExamStateID = examcentre.StateID;

                                    context.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                                    string sql = "Update Course_Exam_Application_Detail set Roll_Number = " + rs.RollNumber + ", Exam_Centre_ID = " + rs.AllottedExamCentreID + " , Exam_Venue_ID = " + rs.AllottedExamVenueID + " where Registration_Number = " + RegistrationNumber + " and Exam_ID=" + ExamID + " and Course_ID = " + CourseID + " and Module_ID in ( Select ID from module where Course_ID = " + CourseID + " and Module_Type_ID!=3 and Id!=938 and Id!=939 and Id!=940 and Id!=941)";
                                    context.Database.ExecuteSqlCommand(sql);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    alreadyUploadedRecords = alreadyUploadedRecords + 1;
                                    appIdList2.Append(RegistrationNumber.ToString() + ",");
                                    if (alreadyUploadedRecords % 10 == 0 && alreadyUploadedRecords > 0)
                                        appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                                }




                            }
                            else
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList3.Append(RegistrationNumber.ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                            }
                        }
                        else
                        {
                            incompletedRecords = NotValidateRecords + 1;
                            appIdList.Append(RegistrationNumber.ToString() + ",");
                            if (incompletedRecords % 10 == 0 && incompletedRecords > 0)
                                appIdList.Append(WebUtility.HtmlDecode("<br/>"));

                        }
                        ExamCentreCode = "";
                        ExamvenueCode = "";
                    }
                    dr.Close();
                    dr.Dispose();
                    command.Dispose();
                };
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                if (ValidateRecords > 0)
                    lblUpdated.Text = "Admit Card Data for " + ValidateRecords.ToString() + " candidates are updated successfully";
                lblNotValidate.Text = NotValidateRecords.ToString();
                if (appIdList2.Length > 0)
                    lblFailed.Text += "<BR>" + alreadyUploadedRecords.ToString() + " (Registration No:- " + appIdList2.ToString().TrimEnd(',').ToString() + ":-" + arr[2].ToString() + ")";
                if (appIdList3.Length > 0)
                    lblFailed.Text += "<BR>" + NotValidateRecords.ToString() + " ( Registration No:- " + appIdList3.ToString().TrimEnd(',').ToString() + ":- " + arr[3].ToString() + ")";
                if (appIdList.Length > 0)
                    lblFailed.Text = "<BR>" + incompletedRecords.ToString() + " (Registration No:- " + appIdList.ToString().TrimEnd(',').ToString() + ":-" + arr[0].ToString() + ")";
            }
            #endregion


            #region OnLine Theory
            if (ModuleType == 2)
            {
                //command = new OleDbCommand("select * from [cand$]", connection);
                //dr = command.ExecuteReader();
                //Int32 blankRowsCount = 0;
                //while (dr.Read())
                //{
                //    if (CommonFunctions.IsNumeric(dr[0].ToString()))
                //    {
                //        TotalRecords = TotalRecords + 1;
                //        RegistrationNumber = Convert.ToInt64(dr[0].ToString());
                //    }
                //    else
                //    {
                //        blankRowsCount++;
                //        if (blankRowsCount < 3)
                //            continue;
                //        else
                //            break;
                //    }

                //    if (!String.IsNullOrEmpty(dr[4].ToString()) && !String.IsNullOrEmpty(dr[7].ToString()) && !String.IsNullOrEmpty(dr[8].ToString()) && !String.IsNullOrEmpty(dr[6].ToString()))
                //    {
                //        CourseExamApplication rs = context.CourseExamApplications.Where(s => s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID && s.CourseID == CourseID).FirstOrDefault();


                //                ValidateRecords = ValidateRecords + 1;

                //                if (string.IsNullOrEmpty(rs.RollNumber.ToString()))
                //                {
                //                    //rs.RollNumber = Convert.ToInt64(dr[1]);
                //                    //// rs.OnlineVenueCode = dr[6].ToString();
                //                    //// rs.OnlineVenueName = dr[7].ToString();
                //                    //// rs.OnlineVenueAddress = dr[8].ToString();
                //                    //context.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                //                    string sql1 = "update Course_Exam_Application set Roll_Number=" + Convert.ToInt64(dr[1]) + " where Registration_Number = " + RegistrationNumber + " and Exam_ID=" + ExamID + " and Course_ID = " + CourseID + "";
                //                    try
                //                    {
                //                        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                //                        using (SqlConnection Conn = new SqlConnection(constr))
                //                        {
                //                            using (SqlCommand cmd = new SqlCommand(sql1, Conn))
                //                            {
                //                                Conn.Open();
                //                               // cmd.CommandType = CommandType.StoredProcedure;
                //                                cmd.ExecuteNonQuery();
                //                            }
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ShowAlert(ex.Message, true);
                //                    }

                //                }


                //                string dt = dr[11].ToString();

                //                DateTime dt2 = new DateTime();
                //                dt2 = Convert.ToDateTime(dt);
                //                var dt3 = dt2.ToString("yyyy-MM-dd");
                //                //string sql = "Update Course_Exam_Application_Detail set Roll_Number = " + Convert.ToInt64(dr[1]) + ", Online_Exam_Login_ID = " + dr[5].ToString() + " , Pr_Batch_No = " + dr[10].ToString() + ", Pr_Date = " + Convert.ToDateTime(dr[11]) + ", Pr_Rept_Time = " + dr[12].ToString() + " where Registration_Number = " + RegistrationNumber + " and Exam_ID=" + ExamID + " and Course_ID = " + CourseID + " and Module_ID in ( Select ID from module where Course_ID = " + CourseID + " and Module_Type_ID!=3)";
                //                string sql = "Update Course_Exam_Application_Detail set ";
                //                if (CourseID != 2)
                //                    sql += "Roll_Number = " + Convert.ToInt64(dr[1]) + ",";
                //                sql += "Online_Exam_Login_ID = '" + dr[5].ToString() + "' , Venue_Code = '" + dr[6].ToString() + "' , Venue_Name = '" + dr[7].ToString() + "', Venue_City_Name = '" + dr[8].ToString() + "', Venue_Address ='" + dr[9].ToString() + "', Pr_Batch_No = '" + dr[13].ToString() + "', Pr_Date = '" + dt3 + "', Pr_Rept_Time = '" + dr[12].ToString() + "' where Registration_Number = " + RegistrationNumber + " and Exam_ID=" + ExamID + " and Course_ID = " + CourseID + " and Module_ID =" + Convert.ToInt16(dr[4]) + " and result_grade_id is null";
                //                //context.Database.ExecuteSqlCommand(sql);
                //                //context.SaveChanges();

                //                try
                //                {
                //                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                //                    using (SqlConnection Conn = new SqlConnection(constr))
                //                    {
                //                        using (SqlCommand cmd = new SqlCommand(sql, Conn))
                //                        {
                //                            Conn.Open();
                //                            //cmd.CommandType = CommandType.StoredProcedure;
                //                            cmd.ExecuteNonQuery();
                //                        }
                //                    }
                //                }
                //                catch (Exception ex)
                //                {
                //                    ShowAlert(ex.Message, true);
                //                }


                //    }
                //    else
                //    {
                //        incompletedRecords = NotValidateRecords + 1;
                //        appIdList.Append(RegistrationNumber.ToString() + ",");
                //        if (incompletedRecords % 10 == 0 && incompletedRecords > 0)
                //            appIdList.Append(WebUtility.HtmlDecode("<br/>"));

                //    }

                //}
                //dr.Close();
                //dr.Dispose();
                //command.Dispose();

                try
                {
                    string filepath = Server.MapPath("../UploadedFiles");
                    string fileName = "fl" + DateTime.Now.ToString("ddMMyyyyhhhhss") + "_" + flUpload.FileName;
                    flUpload.SaveAs(filepath + "/" + fileName);
                    string _path = (filepath + "/" + fileName);
                    string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
                    string excelConnectionString = "";
                    if (ext.ToUpper() == ".XLS")
                        excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", _path);
                    else if (ext.ToUpper() == ".XLSX")
                        excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", _path);
                    else
                    {
                        ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
                        return;
                    }
                    //  ExcelConn(_path);
                    //string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", _path);
                    OleDbConnection Econ = new OleDbConnection(excelConnectionString);
                    string Query = string.Format("Select [reg_no], [roll_no], [Course_ID], [Exam_ID], [Module_ID], [Module_Type], [loginid], [venue_code], [venue_name], [venue_city_name], [venue_address], [batch_no], [exam_date], [rep_time], [start_time] FROM [{0}]", "cand$");
                    //string Query = string.Format("Select [reg no], [roll no] FROM [{0}]", "cand$");
                    //string Query = string.Format("Select * FROM [{0}]", "cand$");

                    OleDbCommand Ecom = new OleDbCommand(Query, Econ);
                    Econ.Open();

                    DataSet ds = new DataSet();
                    OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
                    oda.Fill(ds);
                    Econ.Close();
                    DataTable Exceldt = ds.Tables[0];

                    //for (int i = Exceldt.Rows.Count - 1; i >= 0; i--)
                    //{
                    //    if (Exceldt.Rows[i]["Employee Name"] == DBNull.Value || Exceldt.Rows[i]["Email"] == DBNull.Value)
                    //    {
                    //        Exceldt.Rows[i].Delete();
                    //    }
                    //}
                    Exceldt.AcceptChanges();

                    //creating object of SqlBulkCopy
                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                    SqlBulkCopy objbulk = new SqlBulkCopy(constr);
                    //assigning Destination table name
                    objbulk.DestinationTableName = "Course_Exam_Schedule_Temp";
                    //Mapping Table column

                    objbulk.ColumnMappings.Add("[reg_no]".Trim(), "Registration_Number");
                    objbulk.ColumnMappings.Add("[roll_no]".Trim(), "Roll_Number");
                    objbulk.ColumnMappings.Add("[Course_ID]".Trim(), "Course_ID");
                    objbulk.ColumnMappings.Add("[Exam_ID]".Trim(), "Exam_ID");

                    objbulk.ColumnMappings.Add("[Module_ID]".Trim(), "Module_ID");
                    objbulk.ColumnMappings.Add("[Module_Type]".Trim(), "Module_Type");
                    objbulk.ColumnMappings.Add("[loginid]".Trim(), "Online_Exam_Login_ID");
                    objbulk.ColumnMappings.Add("[venue_code]".Trim(), "Venue_Code");

                    objbulk.ColumnMappings.Add("[venue_name]".Trim(), "Venue_Name");
                    objbulk.ColumnMappings.Add("[venue_city_name]".Trim(), "Venue_City_Name");
                    objbulk.ColumnMappings.Add("[venue_address]".Trim(), "Venue_Address");
                    objbulk.ColumnMappings.Add("[batch_no]".Trim(), "Batch_No");
                    objbulk.ColumnMappings.Add("[exam_date]".Trim(), "Exam_Date");
                    objbulk.ColumnMappings.Add("[rep_time]".Trim(), "Rept_Time");
                    objbulk.ColumnMappings.Add("[start_time]".Trim(), "Start_Time");

                    //inserting Datatable Records to DataBase
                    SqlConnection sqlConnection = new SqlConnection(constr);
                    string sqlQuery = "Delete from Course_Exam_Schedule_Temp";
                    SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                    string sqlQuery1 = "Course_Exam_Admit_Card_Upload";
                    SqlCommand cmd1 = new SqlCommand(sqlQuery1, sqlConnection);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 8000;
                    cmd1.Parameters.Add("@PUser_Id", Convert.ToInt64(Session["UserID"]));
                    cmd1.Parameters.Add("@PIs_Revised", Convert.ToInt16("1"));
                    cmd1.Parameters.Add("@Invalid_record_count", SqlDbType.Int, 20);
                    cmd1.Parameters["@Invalid_record_count"].Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add("@Invalid_module_id", SqlDbType.VarChar, 3000);
                    cmd1.Parameters["@Invalid_module_id"].Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    objbulk.WriteToServer(Exceldt);
                    cmd1.ExecuteNonQuery();
                    lblNotValidate.Text = cmd1.Parameters["@Invalid_record_count"].Value.ToString();
                    lblFailed.Text = cmd1.Parameters["@Invalid_module_id"].Value.ToString();
                    sqlConnection.Close();
                    TotalRecords = ds.Tables[0].Rows.Count;
                    ValidateRecords = (ds.Tables[0].Rows.Count - Convert.ToInt32(cmd1.Parameters["@Invalid_record_count"].Value));
                    //MessageBox.Show("Data has been Imported successfully.", "Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowAlert("Data has been Imported successfully.");

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(string.Format("Data has not been Imported due to :{0}", ex.Message), "Not Imported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ShowAlert("Data has not been Imported due to " + ex.Message + " Not Imported");
                    //pb1.Visible = false;
                    // txtpath.Text = "";
                    //btBrowse.Enabled = true;
                    //label1.Visible = false;
                }


                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                if (ValidateRecords > 0)
                    lblUpdated.Text = "Admit Card Data  for " + ValidateRecords.ToString() + " candidates are updated successfully";
                //--lblNotValidate.Text = NotValidateRecords.ToString();
                if (appIdList2.Length > 0)
                    lblFailed.Text += "<BR>" + alreadyUploadedRecords.ToString() + " (Registration No:- " + appIdList2.ToString().TrimEnd(',').ToString() + ":-" + arr[2].ToString() + ")";
                if (appIdList3.Length > 0)
                    lblFailed.Text += "<BR>" + NotValidateRecords.ToString() + " ( Registration No:- " + appIdList3.ToString().TrimEnd(',').ToString() + ":- " + arr[3].ToString() + ")";
                if (appIdList.Length > 0)
                    lblFailed.Text = "<BR>" + incompletedRecords.ToString() + " (Registration No:- " + appIdList.ToString().TrimEnd(',').ToString() + ":-" + arr[0].ToString() + ")";
            }
            #endregion

            #region Practicle
            else if (ModuleType == 3)
            {
                //using (EConnectContext context = new EConnectContext())
                //{                    
                //    Int32 blankRowsCount = 0;
                //    Int32 practical = Convert.ToInt32(enmModuleType.Practical);
                //    Int32 revisionNo = (from p in context.Modules where p.CourseID == CourseID select p).Max(p => p.RevisionNumber);

                //    while (dr.Read())
                //    {
                //        if (CommonFunctions.IsNumeric(dr[0].ToString()))
                //        {
                //            if (OldRegistrationNumber != Convert.ToInt64(dr[0].ToString()))
                //            {
                //                uniqueregno = uniqueregno + 1;
                //            }
                //            TotalRecords = TotalRecords + 1;
                //            RegistrationNumber = Convert.ToInt64(dr[0].ToString());
                //            OldRegistrationNumber = RegistrationNumber;
                //        }
                //        else
                //        {
                //            blankRowsCount++;
                //            if (blankRowsCount < 3)
                //                continue;
                //            else
                //                break;
                //        }
                //        String PaperCode = dr[3].ToString(); 

                //        if (!String.IsNullOrEmpty(dr[3].ToString()) && !String.IsNullOrEmpty(dr[17].ToString()) && !String.IsNullOrEmpty(dr[18].ToString()) && !String.IsNullOrEmpty(dr[19].ToString()))
                //        {    


                //            instaddress.Clear();
                //            Module module = context.Modules.Where(s => s.CourseID == CourseID && s.ShortName.ToUpper() == PaperCode.ToUpper() && s.ModuleTypeID == practical && s.RevisionNumber == revisionNo ).FirstOrDefault();
                //            // added due to both rev4 and rev5 is applicable in  O and A level
                //          // Module module1 = context.Modules.Where(s => s.CourseID == CourseID && s.ShortName.ToUpper() == PaperCode.ToUpper() && s.ModuleTypeID == practical && s.RevisionNumber == 4).FirstOrDefault();

                //           // CourseExamApplicationDetail cexamdetail = context.CourseExamApplicationDetails.Where(s => s.CourseID == CourseID && (s.ModuleID == module.ID || s.ModuleID == module1.ID )&& s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID).FirstOrDefault();
                //           CourseExamApplicationDetail cexamdetail = context.CourseExamApplicationDetails.Where(s => s.CourseID == CourseID && s.ModuleID == module.ID  && s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID).FirstOrDefault();
                //            //if (cexamdetail != null && (module != null || module1 != null))
                //                if (cexamdetail != null && module != null )
                //            {
                //                if (cexamdetail.PracticalExamBatchNumber == null)
                //                {
                //                    ValidateRecords = ValidateRecords + 1;
                //                    cexamdetail.PracticalExamBatchNumber = dr[17].ToString();
                //                    string[] date = dr[18].ToString().Substring(0, 10).Split('/');
                //                    DateTime pracdate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                //                    cexamdetail.PracticalExamDate = pracdate;
                //                    cexamdetail.PracticalExamReportingTime = dr[19].ToString();
                //                    cexamdetail.CourseExamApplication.PracticalOfficeRefNumber = dr[10].ToString();
                //                    cexamdetail.CourseExamApplication.PracticalInstituteName = dr[11].ToString();
                //                    instaddress.Append(dr[12].ToString() + WebUtility.HtmlDecode("<br/>") + dr[14].ToString());
                //                    if (!String.IsNullOrEmpty(dr[13].ToString()))
                //                    {
                //                        instaddress.Append(WebUtility.HtmlDecode("<br/>") + " Dist:- " + dr[13].ToString() + WebUtility.HtmlDecode(", ") + dr[15].ToString() + ", PinCode:- " + Convert.ToInt64(dr[16].ToString()));
                //                    }
                //                    else
                //                    {
                //                        instaddress.Append(WebUtility.HtmlDecode(", ") + dr[15].ToString() + ", PinCode:- " + Convert.ToInt64(dr[16].ToString()));
                //                    }
                //                    cexamdetail.CourseExamApplication.PracticalInstituteAddress = instaddress.ToString();
                //                    context.Entry(cexamdetail).State = System.Data.Entity.EntityState.Modified;

                //                }
                //                else
                //                {
                //                    alreadyUploadedRecords = alreadyUploadedRecords + 1;
                //                    appIdList2.Append(RegistrationNumber.ToString() + ",");
                //                    if (alreadyUploadedRecords % 10 == 0 && alreadyUploadedRecords > 0)
                //                        appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                //                }
                //            }
                //            else
                //            {
                //                NotValidateRecords = NotValidateRecords + 1;
                //                appIdList3.Append(RegistrationNumber.ToString() + ",");
                //                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                //                    appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                //            }
                //        }
                //        else
                //        {
                //            incompletedRecords = NotValidateRecords + 1;
                //            appIdList.Append(RegistrationNumber.ToString() + ",");
                //            if (incompletedRecords % 10 == 0 && incompletedRecords > 0)
                //                appIdList.Append(WebUtility.HtmlDecode("<br/>"));
                //        }
                //    }
                //    context.SaveChanges();
                //    dr.Close();
                //    dr.Dispose();
                //    command.Dispose();                  
                //};
                //lblTotalRecords.Text = TotalRecords.ToString();
                //lblValidateRecords.Text = ValidateRecords.ToString();
                //if (ValidateRecords > 0)
                //    lblUpdated.Text = "Practical Admit Card Data  of " + uniqueregno.ToString() + " candidates are  updated successfully";
                //lblNotValidate.Text = NotValidateRecords.ToString();
                //if (appIdList2.Length > 0)
                //    lblFailed.Text += "<BR>" + alreadyUploadedRecords.ToString() + " (Registration No:- " + appIdList2.ToString().TrimEnd(',').ToString() + ":-" + arr[2].ToString() + ")";
                //if (appIdList3.Length > 0)
                //    lblFailed.Text += "<BR>" + NotValidateRecords.ToString() + " ( Registration No:- " + appIdList3.ToString().TrimEnd(',').ToString() + ":- " + arr[5].ToString() + ")";
                //if (appIdList.Length > 0)
                //    lblFailed.Text = "<BR>" + incompletedRecords.ToString() + " (Registration No:- " + appIdList.ToString().TrimEnd(',').ToString() + ":-" + arr[4].ToString() + ")";


                //using (EConnectContext context = new EConnectContext())
                //{

                //    Int32 blankRowsCount = 0;
                //    while (dr.Read())
                //    {
                //        if (CommonFunctions.IsNumeric(dr[0].ToString()))
                //        {
                //            TotalRecords = TotalRecords + 1;
                //            RegistrationNumber = Convert.ToInt64(dr[0].ToString());
                //        }
                //        else
                //        {
                //            blankRowsCount++;
                //            if (blankRowsCount < 3)
                //                continue;
                //            else
                //                break;
                //        }

                //        if (!String.IsNullOrEmpty(dr[4].ToString()) && !String.IsNullOrEmpty(dr[7].ToString()) && !String.IsNullOrEmpty(dr[8].ToString()) && !String.IsNullOrEmpty(dr[6].ToString()))
                //        {
                //            CourseExamApplication rs = context.CourseExamApplications.Where(s => s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID && s.CourseID == CourseID).FirstOrDefault();


                //            ValidateRecords = ValidateRecords + 1;
                //            if (rs.RollNumber.HasValue == false)
                //            {
                //                rs.RollNumber = Convert.ToInt64(dr[1]);
                //                // rs.OnlineVenueCode = dr[6].ToString();
                //                // rs.OnlineVenueName = dr[7].ToString();
                //                // rs.OnlineVenueAddress = dr[8].ToString();
                //                context.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                //            }

                //            //string[] date = dr[10].ToString().Substring(0, 10).Split('/');
                //            //DateTime pracdate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                //            string dt = dr[11].ToString();
                //            //DateTime prdt = DateTime.ParseExact(dt,"yyyy-mm-dd",CultureInfo.CurrentCulture);
                //            DateTime dt2 = new DateTime();
                //            dt2 = Convert.ToDateTime(dt);
                //            var dt3 = dt2.ToString("yyyy-MM-dd");
                //            //string sql = "Update Course_Exam_Application_Detail set Roll_Number = " + Convert.ToInt64(dr[1]) + ", Online_Exam_Login_ID = " + dr[5].ToString() + " , Pr_Batch_No = " + dr[10].ToString() + ", Pr_Date = " + Convert.ToDateTime(dr[11]) + ", Pr_Rept_Time = " + dr[12].ToString() + " where Registration_Number = " + RegistrationNumber + " and Exam_ID=" + ExamID + " and Course_ID = " + CourseID + " and Module_ID in ( Select ID from module where Course_ID = " + CourseID + " and Module_Type_ID!=3)";
                //            string sql = "Update Course_Exam_Application_Detail set Roll_Number = " + Convert.ToInt64(dr[1]) + ", Online_Exam_Login_ID = '" + dr[5].ToString() + "' , Venue_Code = '" + dr[6].ToString() + "' , Venue_Name= '" + dr[7].ToString() + "', Venue_City_Name= '" + dr[8].ToString() + "', Venue_Address='" + dr[9].ToString() + "', Pr_Batch_No = '" + dr[13].ToString() + "', Pr_Date = '" + dt3 + "', Pr_Rept_Time = '" + dr[12].ToString() + "' where Registration_Number = " + RegistrationNumber + " and Exam_ID=" + ExamID + " and Course_ID = " + CourseID + " and Module_ID =" + Convert.ToInt16(dr[4]) + " and result_grade_id is null";
                //            context.Database.ExecuteSqlCommand(sql);
                //            context.SaveChanges();
                //            //}
                //            //else
                //            //{
                //            //    alreadyUploadedRecords = alreadyUploadedRecords + 1;
                //            //    appIdList2.Append(RegistrationNumber.ToString() + ",");
                //            //    if (alreadyUploadedRecords % 10 == 0 && alreadyUploadedRecords > 0)
                //            //        appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                //            //}

                //        }
                //        else
                //        {
                //            incompletedRecords = NotValidateRecords + 1;
                //            appIdList.Append(RegistrationNumber.ToString() + ",");
                //            if (incompletedRecords % 10 == 0 && incompletedRecords > 0)
                //                appIdList.Append(WebUtility.HtmlDecode("<br/>"));

                //        }

                //    }
                //    dr.Close();
                //    dr.Dispose();
                //    command.Dispose();
                //};
                try
                {
                    string filepath = Server.MapPath("../UploadedFiles");
                    string fileName = "fl" + DateTime.Now.ToString("ddMMyyyyhhhhss") + "_" + flUpload.FileName;
                    flUpload.SaveAs(filepath + "/" + fileName);
                    string _path = (filepath + "/" + fileName);
                    string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
                    string excelConnectionString = "";
                    if (ext.ToUpper() == ".XLS")
                        excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", _path);
                    else if (ext.ToUpper() == ".XLSX")
                        excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", _path);
                    else
                    {
                        ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
                        return;
                    }
                    //  ExcelConn(_path);
                    //string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", _path);
                    OleDbConnection Econ = new OleDbConnection(excelConnectionString);
                    string Query = string.Format("Select [reg_no], [roll_no], [Course_ID], [Exam_ID], [Module_ID], [Module_Type], [loginid], [venue_code], [venue_name], [venue_city_name], [venue_address], [batch_no], [exam_date], [rep_time], [start_time] FROM [{0}]", "cand$");
                    //string Query = string.Format("Select [reg no], [roll no] FROM [{0}]", "cand$");
                    //string Query = string.Format("Select * FROM [{0}]", "cand$");

                    OleDbCommand Ecom = new OleDbCommand(Query, Econ);
                    Econ.Open();

                    DataSet ds = new DataSet();
                    OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
                    oda.Fill(ds);
                    Econ.Close();
                    DataTable Exceldt = ds.Tables[0];

                    //for (int i = Exceldt.Rows.Count - 1; i >= 0; i--)
                    //{
                    //    if (Exceldt.Rows[i]["Employee Name"] == DBNull.Value || Exceldt.Rows[i]["Email"] == DBNull.Value)
                    //    {
                    //        Exceldt.Rows[i].Delete();
                    //    }
                    //}
                    Exceldt.AcceptChanges();

                    //creating object of SqlBulkCopy
                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                    SqlBulkCopy objbulk = new SqlBulkCopy(constr);
                    //assigning Destination table name
                    objbulk.DestinationTableName = "Course_Exam_Schedule_Temp";
                    //Mapping Table column

                    objbulk.ColumnMappings.Add("[reg_no]".Trim(), "Registration_Number");
                    objbulk.ColumnMappings.Add("[roll_no]".Trim(), "Roll_Number");
                    objbulk.ColumnMappings.Add("[Course_ID]".Trim(), "Course_ID");
                    objbulk.ColumnMappings.Add("[Exam_ID]".Trim(), "Exam_ID");

                    objbulk.ColumnMappings.Add("[Module_ID]".Trim(), "Module_ID");
                    objbulk.ColumnMappings.Add("[Module_Type]".Trim(), "Module_Type");
                    objbulk.ColumnMappings.Add("[loginid]".Trim(), "Online_Exam_Login_ID");
                    objbulk.ColumnMappings.Add("[venue_code]".Trim(), "Venue_Code");

                    objbulk.ColumnMappings.Add("[venue_name]".Trim(), "Venue_Name");
                    objbulk.ColumnMappings.Add("[venue_city_name]".Trim(), "Venue_City_Name");
                    objbulk.ColumnMappings.Add("[venue_address]".Trim(), "Venue_Address");
                    objbulk.ColumnMappings.Add("[batch_no]".Trim(), "Batch_No");
                    objbulk.ColumnMappings.Add("[exam_date]".Trim(), "Exam_Date");
                    objbulk.ColumnMappings.Add("[rep_time]".Trim(), "Rept_Time");
                    objbulk.ColumnMappings.Add("[start_time]".Trim(), "Start_Time");

                    //inserting Datatable Records to DataBase
                    SqlConnection sqlConnection = new SqlConnection(constr);
                    string sqlQuery = "Delete from Course_Exam_Schedule_Temp";
                    SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                    //sqlConnection.ConnectionString = "server = VSBS01; database = dbHRVeniteck; User ID = sa; Password = veniteck@2016"; //Connection Details
                    string sqlQuery1 = "Course_Exam_Admit_Card_Upload";
                    SqlCommand cmd1 = new SqlCommand(sqlQuery1, sqlConnection);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 8000;
                    cmd1.Parameters.Add("@PUser_Id", Convert.ToInt64(Session["UserID"]));
                    cmd1.Parameters.Add("@PIs_Revised", Convert.ToInt16("1"));
                    cmd1.Parameters.Add("@Invalid_record_count", SqlDbType.Int, 20);
                    cmd1.Parameters["@Invalid_record_count"].Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add("@Invalid_module_id", SqlDbType.VarChar, 3000);
                    cmd1.Parameters["@Invalid_module_id"].Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    objbulk.WriteToServer(Exceldt);
                    cmd1.ExecuteNonQuery();
                    lblNotValidate.Text = cmd1.Parameters["@Invalid_record_count"].Value.ToString();
                    lblFailed.Text = cmd1.Parameters["@Invalid_module_id"].Value.ToString();
                    sqlConnection.Close();
                    TotalRecords = ds.Tables[0].Rows.Count;
                    ValidateRecords = (ds.Tables[0].Rows.Count - Convert.ToInt32(cmd1.Parameters["@Invalid_record_count"].Value));
                    //MessageBox.Show("Data has been Imported successfully.", "Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowAlert("Data has been Imported successfully.");

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(string.Format("Data has not been Imported due to :{0}", ex.Message), "Not Imported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ShowAlert("Data has not been Imported due to " + ex.Message + " Not Imported");
                    //pb1.Visible = false;
                    // txtpath.Text = "";
                    //btBrowse.Enabled = true;
                    //label1.Visible = false;
                }
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                if (ValidateRecords > 0)
                    lblUpdated.Text = "Admit Card Data  for " + ValidateRecords.ToString() + " candidates are updated successfully";
                //--lblNotValidate.Text = NotValidateRecords.ToString();
                if (appIdList2.Length > 0)
                    lblFailed.Text += "<BR>" + alreadyUploadedRecords.ToString() + " (Registration No:- " + appIdList2.ToString().TrimEnd(',').ToString() + ":-" + arr[2].ToString() + ")";
                if (appIdList3.Length > 0)
                    lblFailed.Text += "<BR>" + NotValidateRecords.ToString() + " ( Registration No:- " + appIdList3.ToString().TrimEnd(',').ToString() + ":- " + arr[3].ToString() + ")";
                if (appIdList.Length > 0)
                    lblFailed.Text = "<BR>" + incompletedRecords.ToString() + " (Registration No:- " + appIdList.ToString().TrimEnd(',').ToString() + ":-" + arr[0].ToString() + ")";
            }
            #endregion
        }
        catch (Exception ex)
        {
            ShowAlert(ex.ToString(), true);
        }
        finally
        {
            //connection.Close();
            //connection.Dispose();
            //System.IO.File.Delete(path);    
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        string filepath = Server.MapPath("../UploadedFiles");
        flUpload.SaveAs(filepath + "/" + flUpload.FileName);
        string path = (filepath + "/" + flUpload.FileName);
        string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        string excelConnectionString = "";
        if (ext.ToUpper() == ".XLS")
            excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", path);
        else if (ext.ToUpper() == ".XLSX")
            excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", path);
        else
        {
            ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
            return;
        }
        OleDbConnection connection = new OleDbConnection();
        connection.ConnectionString = excelConnectionString;
        connection.Open();
        OleDbCommand command = new OleDbCommand("select * from [cand_dtl$]", connection);
        OleDbDataReader dr = command.ExecuteReader();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                CourseExamApplication rs;
                ExamCenter examcentre;
                ExamVenue examvenue;
                Int64 RegistrationNumber = 0;
                while (dr.Read())
                {
                    if (CommonFunctions.IsNumeric(dr[3].ToString()))
                    {
                        RegistrationNumber = Convert.ToInt64(dr[3].ToString());
                    }
                    else
                    {
                        continue;
                    }
                    String ExamCentreCode = dr[7].ToString(); // c_code(center code)
                    String ExamvenueCode = dr[8].ToString(); // loc_alloted(location code)
                    if (!String.IsNullOrEmpty(dr[4].ToString()) && !String.IsNullOrEmpty(dr[7].ToString()) && !String.IsNullOrEmpty(dr[8].ToString()) && !String.IsNullOrEmpty(dr[6].ToString()))
                    {
                        rs = new CourseExamApplication();
                        examcentre = new ExamCenter();
                        examvenue = new ExamVenue();
                        rs = context.CourseExamApplications.Where(s => s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID && s.CourseID == CourseID).FirstOrDefault();
                        examcentre = context.ExamCenters.Where(s => s.Code.ToUpper() == ExamCentreCode.ToUpper()).FirstOrDefault();
                        examvenue = context.ExamVenues.Where(s => s.Code.ToUpper() == ExamvenueCode.ToUpper() && s.CourseID == CourseID).FirstOrDefault();
                        if (rs != null)
                        {
                            if (rs.RollNumber == null)
                            {
                                rs.RollNumber = Convert.ToInt64(dr[4]);
                                rs.AllottedExamCentreID = examcentre.ID;
                                rs.AllottedExamVenueID = examvenue.ID;
                                rs.AllottedExamStateID = examcentre.StateID; // need to confirm
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
    protected void Lnkcentrechange_Click(object sender, EventArgs e)
    {
        try
        {
            Lnkcentrechange.Visible = false;
            divchangedata.Visible = true;
            tblTheoryData.Visible = false;
            tblPracticalData.Visible = false;
            BtnradioCancel.Visible = true;
            rblOption.ClearSelection();
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
            String instname = "";
            String instaddress = "";
            String batchno = "";
            //DateTime examdate;
            String repttime = "";
            Int32 prac = Convert.ToInt32(enmModuleType.Practical);
            if (rblOption.SelectedValue == "T")
            {
                if (ddlexamcentre.SelectedValue == "0")
                {
                    ShowAlert("Please Select Exam Centre Name.", true);
                    return;
                }
                else if (ddlexamvenue.SelectedValue == "0")
                {
                    ShowAlert("Please Select Exam Venue Name.", true);
                    return;
                }
                else if (Txtrollno.Text == "")
                {
                    ShowAlert("Please Enter Roll Number.", true);
                    return;
                }
                else if (!IsNumeric(Txtrollno.Text))
                {
                    ShowAlert("Invalid roll number.", true);
                    return;
                }
                Int32 AllotedExamcentreId = Convert.ToInt32(ddlexamcentre.SelectedValue);
                Int32 AllotedExamvenueId = Convert.ToInt32(ddlexamvenue.SelectedValue);
                Int64 rollnumber = Convert.ToInt64(Txtrollno.Text.Trim());
                using (EConnectContext context = new EConnectContext())
                {
                    var cexam = context.CourseExamApplications.Where(s => s.ID == applicationID && s.CourseID == courseid && s.ExamID == examid).FirstOrDefault();
                    cexam.AllottedExamCentreID = AllotedExamcentreId;
                    var state = context.ExamCenters.Where(s => s.ID == AllotedExamcentreId).FirstOrDefault();
                    cexam.AllottedExamStateID = state.StateID;
                    cexam.AllottedExamVenueID = AllotedExamvenueId;
                    if (context.CourseExamApplications.Any(s => s.RollNumber == rollnumber && s.ID != cexam.ID && s.ExamID == examid))
                    {
                        ShowAlert("This Roll Number for the candidate already exists.");
                        return;
                    }
                    else
                    {
                        cexam.RollNumber = rollnumber;
                    }
                    cexam.AdmitCardChangedOn = DateTime.Now;
                    cexam.AdmitCardChangedBy = Convert.ToInt32(Session["UserID"]);
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
            if (rblOption.SelectedValue == "P")
            {
                if (ddlinstitutename.SelectedValue.Trim().ToUpper() != "OTHERS")
                {
                    if (ddlinstitutename.SelectedValue == "0")
                    {
                        ShowAlert("Please Select Institute Name.", true);
                        return;
                    }
                    else if (Txtpracofficerefno.Text == "")
                    {
                        ShowAlert("Please Enter Practical Office Reference Number.", true);
                        return;
                    }
                    String[] keys = ddlinstitutename.SelectedValue.Split('|');
                    if (!String.IsNullOrEmpty(keys[0]))
                        instname = Convert.ToString(keys[0]);
                    if (!String.IsNullOrEmpty(keys[1]))
                        instaddress = Convert.ToString(keys[1]).Replace("!", "<br/>");
                    String practofficerefno = Convert.ToString(Txtpracofficerefno.Text.Trim());
                    if (IsValidRecord())
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            using (EConnectContext context = new EConnectContext())
                            {
                                var cexam = context.CourseExamApplications.Where(s => s.ID == applicationID && s.CourseID == courseid && s.ExamID == examid).FirstOrDefault();
                                cexam.PracticalInstituteName = instname;
                                cexam.PracticalInstituteAddress = instaddress;
                                cexam.PracticalOfficeRefNumber = practofficerefno;
                                context.Entry(cexam).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                Int32 i = 0;
                                var cedetail = context.CourseExamApplicationDetails.Where(s => s.CourseExamApplicationID == cexam.ID && s.ExamID == cexam.ExamID && s.CourseID == cexam.CourseID && s.Module.ModuleTypeID == prac).OrderBy(s => s.PracticalExamDate).ThenByDescending(s => s.PracticalExamReportingTime).ToList();
                                if (cedetail.Count() > 0)
                                {
                                    foreach (var courseexamdetail in cedetail)
                                    {
                                        // Updating Data in Course Exam Application details
                                        //for (int i = 0; i < gvpracexam.Rows.Count; i++)
                                        //{
                                        batchno = ((TextBox)gvpracexam.Rows[i].FindControl("txtbatchno")).Text;
                                        //examdate = Convert.ToDateTime(((TextBox)gvpracexam.Rows[i].FindControl("txtpracexamdate")).Text);
                                        repttime = ((TextBox)gvpracexam.Rows[i].FindControl("txtreptime")).Text;
                                        courseexamdetail.PracticalExamBatchNumber = batchno.ToUpper();
                                        //courseexamdetail.PracticalExamDate = examdate;
                                        courseexamdetail.PracticalExamReportingTime = repttime;
                                        context.Entry(courseexamdetail).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                        //}
                                        i++;
                                    }
                                }
                                scope.Complete();
                                ShowAlert("Practical Admit Card details of the candidate changed successfully.");
                            };
                        };
                        ShowEditMode();
                        trchangedetail.Visible = true;
                        Lnkcentrechange.Visible = true;
                        divchangedata.Visible = false;
                        divdata.Visible = false;
                    }
                }
                else if (ddlinstitutename.SelectedValue.Trim().ToUpper() == "OTHERS")
                {

                    if (Txtinstname.Text == "")
                    {
                        ShowAlert("Please Enter Institute Name.", true);
                        return;
                    }
                    else if (Txtinstadd.Text == "")
                    {
                        ShowAlert("Please Enter Institute Address.", true);
                        return;
                    }
                    else if (TxtinstCity.Text == "")
                    {
                        ShowAlert("Please Enter Institute City.", true);
                        return;
                    }
                    else if (ddlstate.SelectedValue == "0")
                    {
                        ShowAlert("Please Select State Name.", true);
                        return;
                    }
                    else if (ddldistrict.SelectedValue == "0")
                    {
                        ShowAlert("Please Select District Name.", true);
                        return;
                    }
                    else if (Txtinstpincode.Text == "")
                    {
                        ShowAlert("Please Enter PinCode.", true);
                        return;
                    }
                    else if (!IsNumeric(Txtinstpincode.Text))
                    {
                        ShowAlert("Invalid PinCode.", true);
                        return;
                    }
                    else if (Txtpracofficerefno.Text == "")
                    {
                        ShowAlert("Please Enter Practical Office Reference Number.", true);
                        return;
                    }
                    string inst1name = Txtinstname.Text.Trim().ToUpper();
                    string inst1city = TxtinstCity.Text.Trim().ToUpper();
                    string inst1add = Txtinstadd.Text.ToUpper();
                    string statename = ddlstate.SelectedItem.Text.ToUpper();
                    string districtname = ddldistrict.SelectedItem.Text.ToUpper();
                    string profficeno = Txtpracofficerefno.Text.Trim().ToUpper();
                    Int64 pincode = Convert.ToInt64(Txtinstpincode.Text.Trim());
                    instcompleteaddress.Append(inst1add.ToString() + WebUtility.HtmlDecode("<br/>") + inst1city.ToString());
                    instcompleteaddress.Append(WebUtility.HtmlDecode("<br/>") + " Dist:- " + districtname.ToString() + WebUtility.HtmlDecode(", ") + statename.ToString() + ", PinCode:- " + pincode.ToString());
                    if (IsValidRecord())
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            using (EConnectContext context = new EConnectContext())
                            {
                                var cexam = context.CourseExamApplications.Where(s => s.ID == applicationID && s.CourseID == courseid && s.ExamID == examid).FirstOrDefault();
                                cexam.PracticalInstituteName = inst1name;
                                cexam.PracticalInstituteAddress = instcompleteaddress.ToString();
                                cexam.PracticalOfficeRefNumber = profficeno;
                                context.Entry(cexam).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                Int32 i = 0;
                                var cedetail = context.CourseExamApplicationDetails.Where(s => s.CourseExamApplicationID == cexam.ID && s.ExamID == cexam.ExamID && s.CourseID == cexam.CourseID && s.Module.ModuleTypeID == prac).OrderBy(s => s.PracticalExamDate).ThenByDescending(s => s.PracticalExamReportingTime).ToList();
                                if (cedetail.Count() > 0)
                                {
                                    foreach (var courseexamdetail in cedetail)
                                    {
                                        // Updating Data in Course Exam Application details
                                        //for (int i = 0; i < gvpracexam.Rows.Count; i++)
                                        //{
                                        batchno = ((TextBox)gvpracexam.Rows[i].FindControl("txtbatchno")).Text;
                                        //examdate = Convert.ToDateTime(((TextBox)gvpracexam.Rows[i].FindControl("txtpracexamdate")).Text);
                                        repttime = ((TextBox)gvpracexam.Rows[i].FindControl("txtreptime")).Text;
                                        courseexamdetail.PracticalExamBatchNumber = batchno.ToUpper();
                                        //courseexamdetail.PracticalExamDate = examdate;
                                        courseexamdetail.PracticalExamReportingTime = repttime;
                                        context.Entry(courseexamdetail).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                        //}
                                        i++;
                                    }
                                }
                                scope.Complete();
                                ShowAlert("Practical Admit Card details of the candidate changed successfully.");
                            };
                        };
                        BindInstituteName(courseid);
                        ddlinstitutename.Items.Insert(1, "Others");
                        ShowEditMode();
                        trchangedetail.Visible = true;
                        Lnkcentrechange.Visible = true;
                        divchangedata.Visible = false;
                        divdata.Visible = false;
                        trinstitute1.Visible = false;
                        trinstitute2.Visible = false;
                        trinstitute3.Visible = false;
                        trinstitute4.Visible = false;
                        trinstitute5.Visible = false;
                        trinstitute6.Visible = false;
                        trinstitutehead.Visible = false;
                    }
                }
            }
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
            trinstitute1.Visible = false;
            trinstitute2.Visible = false;
            trinstitute3.Visible = false;
            trinstitute4.Visible = false;
            trinstitute5.Visible = false;
            trinstitute6.Visible = false;
            trinstitutehead.Visible = false;
            rblOption.ClearSelection();
            ShowEditMode();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlexamcentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 examCentreID = Convert.ToInt32(ddlexamcentre.SelectedValue);
            Int32 courseid = Convert.ToInt32(Request.QueryString["CourseID"]);
            using (EConnectContext context = new EConnectContext())
            {
                var examvenue = from r in context.ExamVenues
                                where r.ExamCentreID == examCentreID && r.CourseID == courseid
                                orderby r.Name
                                select new
                                {
                                    ValueField = r.ID,
                                    TextField = r.Name + "," + r.AddressLine1
                                };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamvenue, examvenue.Distinct(), new ListItem("--Select Exam Venue--", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindExamVenue(Int32 Examcentreid, Int32 courseID)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                var examvenue = from r in context.ExamVenues
                                where r.ExamCentreID == Examcentreid && r.CourseID == courseID
                                orderby r.Name
                                select new
                                {
                                    ValueField = r.ID,
                                    TextField = r.Name + "," + r.AddressLine1
                                };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamvenue, examvenue.Distinct(), new ListItem("--Select Exam Venue--", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvpracexam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected Boolean IsValidRecord()
    {
        try
        {
            if (gvpracexam.Rows.Count > 0)
            {
                for (int i = 0; i < gvpracexam.Rows.Count; i++)
                {
                    TextBox txtbatchno = (TextBox)gvpracexam.Rows[i].FindControl("txtbatchno");
                    //TextBox txtexamdate = (TextBox)gvpracexam.Rows[i].FindControl("txtpracexamdate");
                    TextBox txtreptime = (TextBox)gvpracexam.Rows[i].FindControl("txtreptime");
                    if (txtbatchno.Text == "")
                    {
                        ShowAlert("Please enter Batch No.", true);
                        txtbatchno.Focus();
                        return false;
                    }
                    //else if (txtexamdate.Text == "")
                    //{
                    //    ShowAlert("Please enter Practical Exam Date", true);
                    //    txtexamdate.Focus();
                    //    return false;
                    //}
                    //else if (!IsDate(txtexamdate.Text))
                    //{
                    //    ShowAlert("Invalid Practical Exam Date", true);
                    //    txtexamdate.Focus();
                    //    return false;
                    //}
                    else if (txtreptime.Text == "")
                    {
                        ShowAlert("Please enter Time-Slot.", true);
                        txtreptime.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rblOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divchangedata.Visible = true;
            trchangedetail.Visible = false;
            divdata.Visible = true;
            BtnradioCancel.Visible = false;
            if (rblOption.SelectedValue == "T")
            {
                tblTheoryData.Visible = true;
                tblPracticalData.Visible = false;
            }
            else
            {
                tblTheoryData.Visible = false;
                tblPracticalData.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlinstitutename_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            String instname = "";
            String instaddress = "";
            Txtpracofficerefno.Text = "";
            if (ddlinstitutename.SelectedValue.Trim().ToUpper() != "OTHERS")
            {
                trinstitute1.Visible = false;
                trinstitute2.Visible = false;
                trinstitute3.Visible = false;
                trinstitute4.Visible = false;
                trinstitute5.Visible = false;
                trinstitute6.Visible = false;
                trinstitutehead.Visible = false;
                if (ddlinstitutename.SelectedValue != "0")
                {
                    String[] keys = ddlinstitutename.SelectedValue.Split('|');
                    if (!String.IsNullOrEmpty(keys[0]))
                        instname = Convert.ToString(keys[0]);
                    if (!String.IsNullOrEmpty(keys[1]))
                        instaddress = Convert.ToString(keys[1]);
                    using (EConnectContext context = new EConnectContext())
                    {
                        var profficerefno = (from r in context.CourseExamApplications
                                             where r.PracticalInstituteName.Trim().ToUpper() == instname.Trim().ToUpper()
                                             select r).FirstOrDefault();
                        if (profficerefno != null)
                        {
                            Txtpracofficerefno.Text = profficerefno.PracticalOfficeRefNumber;
                        }
                    };
                }
            }
            else if (ddlinstitutename.SelectedValue.Trim().ToUpper() == "OTHERS")
            {
                trinstitute1.Visible = true;
                trinstitute2.Visible = true;
                trinstitute3.Visible = true;
                trinstitute4.Visible = true;
                trinstitute5.Visible = true;
                trinstitute6.Visible = true;
                trinstitutehead.Visible = true;
                Txtpracofficerefno.Text = "";
                Txtinstname.Text = "";
                Txtinstadd.Text = "";
                TxtinstCity.Text = "";
                Txtinstpincode.Text = "";
                ddlstate.SelectedValue = "0";
                ddldistrict.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnradioCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divchangedata.Visible = false;
            trchangedetail.Visible = true;
            Lnkcentrechange.Visible = true;
            divdata.Visible = false;
            rblOption.ClearSelection();
            ShowEditMode();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public void bindState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var state = (from s in context.Locations
                             orderby (s.Name)
                             where s.LocationTypeID == 2 && s.ParentLocationID == 1
                             select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlstate, state, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            int stateID = Convert.ToInt32(ddlstate.SelectedValue);
            ddldistrict.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district = from s in context.Locations
                               orderby (s.Name)
                               where s.LocationTypeID == locationTypeID && s.ParentLocationID == stateID
                               select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddldistrict, district.ToList(), lst);
                if (ddldistrict.Items.Count == 0)
                    ddldistrict.Items.Add(lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}