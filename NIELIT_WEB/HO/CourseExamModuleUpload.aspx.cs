using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.OleDb;
using System.Data.SqlClient;
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
using RestSharp;

public partial class HO_CourseExamModuleUpload : BasePage
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
                        ddlmodulefilter.SelectedValue = Request.QueryString["modulefilter"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Module Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "HO/CourseExamModuleUpload.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload  Module Data", "HO/CourseExamModuleUpload.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria to View Module Data.";
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
                                where p.CourseID == CourseID && p.CourseCategoryID == CourseCategoryID && p.ExaminationCycleID == ExamCycleID && p.ExamYear >= currentyear && p.DateOfPublishingOfResult == null
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
            lblHeading.Text = "Upload Module Data";
            tblNavLinks.Visible = true;
            //imgSampleDoc.Visible = false;
            tbls2.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 applicationID = Convert.ToInt32(Request.QueryString["Key"]);
                Int32 ModuletypeID = Convert.ToInt32(enmModuleType.Practical);
                var application = (from p in context.CourseExamApplications
                                   where p.ID == applicationID
                                   select p).FirstOrDefault();

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
                lblAppNumber.Text = application.Number.ToString();
                lblAppDate.Text = application.ApplicationDate.ToString("dd-MMM-yyyy");
                lblCourse.Text = application.Course.Name + " ( " + application.CourseCategory.Code + " ) ";
                lblExamDetail.Text = application.Exam.Name;
                lblRollno.Text = application.RollNumber.HasValue ? application.RollNumber.ToString() : "NA";
                lblCandidate.Text = application.Candidate.Salutation + " " + GetInitCap(application.Candidate.Name);
                Lbregno.Text = application.RegistrationNumber.ToString();
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

                var moduledetails = (from d in context.CourseExamApplicationDetails
                                     join m in context.Modules on d.ModuleID equals m.ID
                                     where d.CourseExamApplicationID == applicationID && m.ModuleTypeID != ModuletypeID
                                     orderby m.ShortName
                                     select new
                                         {
                                             modulename = m.Name,
                                             modulesname = m.ShortName,
                                             moduleshortcode = m.ModuleSubCode
                                         }).ToList();
                if (moduledetails.Count() > 0)
                {
                    foreach (var mdetails in moduledetails)
                    {
                        lbmodule.Text += mdetails.modulesname + " :- " + mdetails.modulename + WebUtility.HtmlDecode("<br/>");
                    }
                }
                else
                {
                    lbmodule.Text = "No Module Available";
                }
                // Practical Details
                var pracmoduledetails = (from d in context.CourseExamApplicationDetails
                                         join m in context.Modules on d.ModuleID equals m.ID
                                         where d.CourseExamApplicationID == applicationID && m.ModuleTypeID == ModuletypeID
                                         orderby m.ShortName
                                         select new
                                         {
                                             modulename = m.Name,
                                             modulesname = m.ShortName,
                                             moduleshortcode = m.ModuleSubCode
                                         }).ToList();
                if (pracmoduledetails.Count() > 0)
                {
                    foreach (var pmdetails in pracmoduledetails)
                    {
                        lbpmodule.Text += pmdetails.modulesname + " :- " + pmdetails.modulename + WebUtility.HtmlDecode("<br/>");
                    }
                }
                else
                {
                    lbpmodule.Text = "Not Applied For Practical.";
                }
                //lblDateofExam.Text = application.Exam.ExamStartDate.ToString("dd-MMM-yyyy");
                hlAppDetail.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?AppID=" + applicationID.ToString() + "&candidateID=" + application.CandidateID) + "');");
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(application.RegistrationNumber + "-" + "(" + application.Candidate.Name + ")", "HO/CourseExamModuleUpload.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
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
            var Examadmitdata = from s in context.CourseExamApplications
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
            lblHeading.Text = "Upload Module Data";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Module Data Details", "", ""));
        }
        else
        {
            if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
            {
                Response.Redirect("CourseExamModuleUpload.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&modulefilter=" + Request.QueryString["modulefilter"], true);
            }
            else
                Response.Redirect("CourseExamModuleUpload.aspx", true);
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
            if (ddlflCourse.SelectedValue != "0" || ddlflExam.SelectedValue != "0" || ddlflExamYear.SelectedValue != "0" || ddlflExamYear.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Upload Module Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "HO/CourseExamModuleUpload.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&modulefilter=" + ddlmodulefilter.SelectedValue, ""));
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
            Response.Redirect("CourseExamModuleUpload.aspx");
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
        //if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
        //{
        //    Response.Redirect("CourseExamModuleUpload.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"], true);
        //}
        //else
        Response.Redirect("CourseExamModuleUpload.aspx", true);
    }
    protected void btnValidate_Click(object sender, EventArgs e)
    {
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        divValidateData.Visible = true;
        //btnSave.Visible = true;
        btnValidate.Visible = false;
        BreadCrumb1.Render();

        StringBuilder sb = new StringBuilder();
        string[] arr = new string[10];
        string appIdList = "";
        string appIdList2 = "";
        string appIdList3 = "";
        string applistUpdated = "";
        arr[0] = "uploaded data is not correct.Please Correct the data and Upload again.";
        arr[2] = "Data Already Updated";
        arr[3] = "do not exist";


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
        OleDbCommand command = new OleDbCommand("select * from [sub$]", connection);
        OleDbDataReader dr = command.ExecuteReader();
        int TotalRecords = 0;
        int Updatedrecords = 0;
        int NotValidateRecords = 0;
        Int64 RegistrationNumber = 0;
        try
        {
            Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
            Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 moduletypeID = Convert.ToInt32(enmModuleType.Practical);
            Int32 ProcessOption = Convert.ToInt32(ddlprocess.SelectedValue);
            Int32 moduleType = Convert.ToInt32(Ddlmoduletype.SelectedValue);
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    string sql2 = " delete from Temp_Table  where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + "";
                    context.Database.ExecuteSqlCommand(sql2);
                    context.SaveChanges();
                    if (moduleType == 1) // theory
                    {
                        while (dr.Read())
                        {
                            if (CommonFunctions.IsNumeric(dr[3].ToString()))//REgistration Number
                            {
                                TotalRecords = TotalRecords + 1;
                                RegistrationNumber = Convert.ToInt64(dr[3].ToString());
                            }
                            else
                            {
                                continue;
                            }
                            Int32 modulesubcode = Convert.ToInt32(dr[4].ToString()); // sub_code (module sub-code)
                            String modulesname = dr[5].ToString();                  //  alpha_sub_code (module short_name)  
                            if (!String.IsNullOrEmpty(dr[4].ToString()) && !String.IsNullOrEmpty(dr[5].ToString()) && !String.IsNullOrEmpty(dr[3].ToString()))
                            {
                                var module = context.Modules.Where(s => s.ModuleSubCode == modulesubcode && s.CourseID == CourseID && s.ShortName.ToUpper() == modulesname.ToUpper() && s.ModuleTypeID != moduletypeID).OrderBy(s => s.ID).FirstOrDefault();
                                if (module == null)
                                {
                                    NotValidateRecords = NotValidateRecords + 1;
                                    appIdList3 += modulesname.ToString() + ",";
                                    if (NotValidateRecords % 10 == 0)
                                        appIdList3 += WebUtility.HtmlDecode("<br/>");
                                }
                                else
                                {
                                    //string sqle = "insert into Temp_Table values (" + RegistrationNumber + ", " + module.ID + ", " + ExamID + ", " + CourseID + ")";
                                    //context.Database.ExecuteSqlCommand(sqle);

                                    //December_2024
                                    SqlParameter[] param1 = { new SqlParameter("@registrationNumber", RegistrationNumber),
                                                                new SqlParameter("@moduleId", module.ID),
                                                                    new SqlParameter("@examID", ExamID),
                                                                        new SqlParameter("@courseID", CourseID)
                                                                        };

                                    string sqle = "insert into Temp_Table values (@registrationNumber, @moduleId, @examID, @courseID)";
                                    context.Database.ExecuteSqlCommand(sqle, param1);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    else if (moduleType == 2) // practical
                    {
                        while (dr.Read())
                        {
                            if (CommonFunctions.IsNumeric(dr[0].ToString()))//REgistration Number
                            {
                                TotalRecords = TotalRecords + 1;
                                RegistrationNumber = Convert.ToInt64(dr[0].ToString());
                            }
                            else
                            {
                                continue;
                            }
                            Int32 revisionNo = (from p in context.Modules
                                                where p.CourseID == CourseID
                                                select p).Max(p => p.RevisionNumber);
                            String PaperCode = dr[3].ToString(); // PAPER_CODE (PAPER_CODE) 
                            if (!String.IsNullOrEmpty(dr[0].ToString()) && !String.IsNullOrEmpty(dr[3].ToString()))
                            {
                                var module = context.Modules.Where(s => s.CourseID == CourseID && s.ShortName.ToUpper() == PaperCode.ToUpper() && s.ModuleTypeID == moduletypeID && s.RevisionNumber == revisionNo).OrderBy(s => s.ID).FirstOrDefault();
                                if (module == null)
                                {
                                    NotValidateRecords = NotValidateRecords + 1;
                                    appIdList3 += PaperCode.ToString() + ",";
                                    if (NotValidateRecords % 10 == 0)
                                        appIdList3 += WebUtility.HtmlDecode("<br/>");
                                }
                                else
                                {
                                    //string sqle = "insert into Temp_Table values (" + RegistrationNumber + ", " + module.ID + ", " + ExamID + ", " + CourseID + ")";
                                    //context.Database.ExecuteSqlCommand(sqle);

                                    //December_2024
                                    SqlParameter[] param2 = { new SqlParameter("@registrationNumber", RegistrationNumber),
                                                                new SqlParameter("@moduleId", module.ID),
                                                                    new SqlParameter("@examID", ExamID),
                                                                        new SqlParameter("@courseID", CourseID)
                                                                        };
                                    string sqle = "insert into Temp_Table values (@registrationNumber, @moduleId, @examID, @courseID)";
                                    context.Database.ExecuteSqlCommand(sqle,param2);

                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                };
                scope.Complete();
            };
            if (moduleType == 1) // theory
            {
                if (ProcessOption == 1)
                {
                    //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                    con.Open();
                    con.BeginTransaction();
                    string sql = " select distinct Registration_Number from Temp_Table where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + " order by Registration_Number";
                    SqlDataReader dr1 = EConnect.Utils.Data.DbUtility.ExecuteReader(sql, con, null, CommandType.Text, true);
                    while (dr1.Read())
                    {
                        Int64 regno = Convert.ToInt64(dr1["Registration_Number"]);
                        sql = " select Module_ID from Temp_Table where Registration_Number = " + regno + " order by Module_ID ";
                        DataTable dr2 = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);

                        Int32 moduleID = 0;
                        List<Int32> listexecel = new List<Int32>();
                        String strExcel = "";
                        List<Int32> listtable = new List<Int32>();
                        for (int i = 0; i <= dr2.Rows.Count - 1; i++)
                        {
                            moduleID = Convert.ToInt32(dr2.Rows[i]["Module_ID"]);
                            listexecel.Add(moduleID);
                            strExcel += moduleID.ToString() + ",";
                            sql = " select Count(*) from Course_Exam_Application_Detail where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                                  " and Course_ID = " + CourseID + " and Module_ID = " + moduleID + "";
                            Int32 moduleCount = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller(sql, con, null, CommandType.Text, true));
                            if (moduleCount == 0)
                            {
                                listtable.Add(moduleID);
                            }
                        }
                        int j = 0;
                        int k = 0;
                        sql = " select Module_ID from Course_Exam_Application_Detail where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                              " and Course_ID = " + CourseID + " and Module_ID NOT IN (" + strExcel.Trim().Trim(',') + ") order by Module_ID ";
                        DataTable tblMOdules = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);
                        if (tblMOdules.Rows.Count > 0)
                        {
                            foreach (DataRow cexamdetail in tblMOdules.Rows)
                            {
                                j = j + 1;
                                foreach (Int32 tablemoduleid in listtable)
                                {
                                    k = k + 1;
                                    if (j == k)
                                    {
                                        Updatedrecords = Updatedrecords + 1;
                                        sql = " Update Course_Exam_Application_Detail set Module_ID = " + tablemoduleid + "  where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                                              " and Course_ID = " + CourseID + " and Module_ID = " + Convert.ToInt32(cexamdetail["Module_ID"]) + "";
                                        EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql, con, null, CommandType.Text, true);
                                        applistUpdated += regno + ":-" + cexamdetail["Module_ID"].ToString() + ",";
                                        if (Updatedrecords % 10 == 0)
                                            applistUpdated += WebUtility.HtmlDecode("<br/>");
                                    }
                                }
                                k = 0;
                            }
                        }
                        else
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                        }
                    }
                    string sql4 = " delete from Temp_Table  where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + "";
                    EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql4, con, null, CommandType.Text, true);

                    dr1.Close();
                    dr1.Dispose();
                    con.CommitTransaction();
                    con.Close();
                    lblTotalRecords.Text = TotalRecords.ToString();
                    lblupdatedRecords.Text = Updatedrecords.ToString();
                    if (Updatedrecords != 0)
                        lblUpdated.Text = "Modules Data for Registration No with Module-ID :- " + applistUpdated.TrimEnd(',').ToString() + " are  updated successfully";
                    lblNotValidate.Text = NotValidateRecords.ToString();
                    if (appIdList2 != "")
                        lblFailed.Text += "<br/> Modules Data for Module-ID:- " + appIdList2.TrimEnd(',').ToString() + ":" + arr[2].ToString();
                    if (appIdList3 != "")
                        lblFailed.Text += "<br/> Module Code:- " + appIdList3.TrimEnd(',').ToString() + " " + arr[0].ToString();
                    if (appIdList != "")
                        lblFailed.Text = "Registration No:-" + appIdList.TrimEnd(',').ToString() + ":" + arr[3].ToString();
                }
                else if (ProcessOption == 2)
                {
                    //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                    con.Open();
                    con.BeginTransaction();
                    string sql = " select distinct Registration_Number from Temp_Table where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + " order by Registration_Number";
                    SqlDataReader dr2 = EConnect.Utils.Data.DbUtility.ExecuteReader(sql, con, null, CommandType.Text, true);
                    while (dr2.Read())
                    {
                        Int64 regno = Convert.ToInt64(dr2["Registration_Number"]);
                        sql = " select t.Module_ID as Module_ID  from Course_Exam_Application_Detail t, Module m " +
                                " where t.Module_ID = m.ID and t.Exam_ID = " + ExamID + " and  m.Module_Type_ID != " + moduletypeID + " " +
                            " and t.Registration_Number = " + regno + "  and t.Course_ID = " + CourseID + " and " +
                            " t.Module_ID not in (Select Module_ID from Temp_Table c " +
                            "  where c.Course_ID = t.Course_ID and c.Exam_ID = t.Exam_ID and c.Registration_Number = t.Registration_Number) ";
                        DataTable tblMOdules1 = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);
                        if (tblMOdules1.Rows.Count > 0)
                        {
                            foreach (DataRow cexamdetail in tblMOdules1.Rows)
                            {
                                Updatedrecords = Updatedrecords + 1;
                                sql = " Update Course_Exam_Application_Detail set Is_Canceled = 'true', Result_Grade_ID = '7', Canceled_On = '" + DateTime.Now + "', Cancel_Remarks = 'Previously Appeared(Wrongly Applied)'  where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                                        " and Course_ID = " + CourseID + " and Module_ID = " + Convert.ToInt32(cexamdetail["Module_ID"]) + "";
                                EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql, con, null, CommandType.Text, true);
                                applistUpdated += regno + ":-" + cexamdetail["Module_ID"].ToString() + ",";
                                if (Updatedrecords % 10 == 0)
                                    applistUpdated += WebUtility.HtmlDecode("<br/>");
                            }
                        }
                        else
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                        }
                    }
                    string sql4 = " delete from Temp_Table  where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + "";
                    EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql4, con, null, CommandType.Text, true);

                    dr2.Close();
                    dr2.Dispose();
                    con.CommitTransaction();
                    con.Close();
                    lblTotalRecords.Text = TotalRecords.ToString();
                    lblupdatedRecords.Text = Updatedrecords.ToString();
                    if (Updatedrecords != 0)
                        lblUpdated.Text = "Modules Data for Registration No with Module-ID :- " + applistUpdated.TrimEnd(',').ToString() + " are  cancelled successfully";
                    lblNotValidate.Text = NotValidateRecords.ToString();
                    if (appIdList2 != "")
                        lblFailed.Text += "<br/> Modules Data for Module-ID:- " + appIdList2.TrimEnd(',').ToString() + ":" + arr[2].ToString();
                    if (appIdList3 != "")
                        lblFailed.Text += "<br/> Module Code:- " + appIdList3.TrimEnd(',').ToString() + " " + arr[0].ToString();
                    if (appIdList != "")
                        lblFailed.Text = "Registration No:-" + appIdList.TrimEnd(',').ToString() + ":" + arr[3].ToString();
                }
            }
            else if (moduleType == 2) // practical
            {
                if (ProcessOption == 1)
                {
                    //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                    con.Open();
                    con.BeginTransaction();
                    string sql = " select distinct Registration_Number from Temp_Table where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + " order by Registration_Number";
                    SqlDataReader dr1 = EConnect.Utils.Data.DbUtility.ExecuteReader(sql, con, null, CommandType.Text, true);
                    while (dr1.Read())
                    {
                        Int64 regno = Convert.ToInt64(dr1["Registration_Number"]);
                        sql = " select Module_ID from Temp_Table where Registration_Number = " + regno + " order by Module_ID ";
                        DataTable dr2 = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);

                        Int32 moduleID = 0;
                        List<Int32> listexecel = new List<Int32>();
                        String strExcel = "";
                        List<Int32> listtable = new List<Int32>();
                        for (int i = 0; i <= dr2.Rows.Count - 1; i++)
                        {
                            moduleID = Convert.ToInt32(dr2.Rows[i]["Module_ID"]);
                            listexecel.Add(moduleID);
                            strExcel += moduleID.ToString() + ",";
                            sql = " select Count(*) from Course_Exam_Application_Detail where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                                  " and Course_ID = " + CourseID + " and Module_ID = " + moduleID + "";
                            Int32 moduleCount = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller(sql, con, null, CommandType.Text, true));
                            if (moduleCount == 0)
                            {
                                listtable.Add(moduleID);
                            }
                        }
                        int j = 0;
                        int k = 0;
                        sql = " select Module_ID from Course_Exam_Application_Detail where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                              " and Course_ID = " + CourseID + " and Module_ID NOT IN (" + strExcel.Trim().Trim(',') + ") order by Module_ID ";
                        DataTable tblMOdules = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);
                        if (tblMOdules.Rows.Count > 0)
                        {
                            foreach (DataRow cexamdetail in tblMOdules.Rows)
                            {
                                j = j + 1;
                                foreach (Int32 tablemoduleid in listtable)
                                {
                                    k = k + 1;
                                    if (j == k)
                                    {
                                        Updatedrecords = Updatedrecords + 1;
                                        sql = " Update Course_Exam_Application_Detail set Module_ID = " + tablemoduleid + "  where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                                              " and Course_ID = " + CourseID + " and Module_ID = " + Convert.ToInt32(cexamdetail["Module_ID"]) + "";
                                        EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql, con, null, CommandType.Text, true);
                                        applistUpdated += regno + ":-" + cexamdetail["Module_ID"].ToString() + ",";
                                        if (Updatedrecords % 10 == 0)
                                            applistUpdated += WebUtility.HtmlDecode("<br/>");
                                    }
                                }
                                k = 0;
                            }
                        }
                        else
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                        }
                    }
                    string sql4 = " delete from Temp_Table  where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + "";
                    EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql4, con, null, CommandType.Text, true);

                    dr1.Close();
                    dr1.Dispose();
                    con.CommitTransaction();
                    con.Close();
                    lblTotalRecords.Text = TotalRecords.ToString();
                    lblupdatedRecords.Text = Updatedrecords.ToString();
                    if (Updatedrecords != 0)
                        lblUpdated.Text = "Modules Data for Registration No with Module-ID :- " + applistUpdated.TrimEnd(',').ToString() + " are  updated successfully";
                    lblNotValidate.Text = NotValidateRecords.ToString();
                    if (appIdList2 != "")
                        lblFailed.Text += "<br/> Modules Data for Module-ID:- " + appIdList2.TrimEnd(',').ToString() + ":" + arr[2].ToString();
                    if (appIdList3 != "")
                        lblFailed.Text += "<br/> Module Code:- " + appIdList3.TrimEnd(',').ToString() + " " + arr[0].ToString();
                    if (appIdList != "")
                        lblFailed.Text = "Registration No:-" + appIdList.TrimEnd(',').ToString() + ":" + arr[3].ToString();
                }
                else if (ProcessOption == 2)
                {
                    //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                    con.Open();
                    con.BeginTransaction();
                    string sql = " select distinct Registration_Number from Temp_Table where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + " order by Registration_Number";
                    SqlDataReader dr2 = EConnect.Utils.Data.DbUtility.ExecuteReader(sql, con, null, CommandType.Text, true);
                    while (dr2.Read())
                    {
                        Int64 regno = Convert.ToInt64(dr2["Registration_Number"]);
                        sql = " select t.Module_ID as Module_ID  from Course_Exam_Application_Detail t, Module m " +
                                " where t.Module_ID = m.ID and t.Exam_ID = " + ExamID + " and  m.Module_Type_ID = " + moduletypeID + " " +
                            " and t.Registration_Number = " + regno + "  and t.Course_ID = " + CourseID + " and " +
                            " t.Module_ID not in (Select Module_ID from Temp_Table c " +
                            "  where c.Course_ID = t.Course_ID and c.Exam_ID = t.Exam_ID and c.Registration_Number = t.Registration_Number) ";
                        DataTable tblMOdules1 = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);
                        if (tblMOdules1.Rows.Count > 0)
                        {
                            foreach (DataRow cexamdetail in tblMOdules1.Rows)
                            {
                                Updatedrecords = Updatedrecords + 1;
                                sql = " Update Course_Exam_Application_Detail set Is_Canceled = 'true', Result_Grade_ID = '7', Canceled_On = '" + DateTime.Now + "', Cancel_Remarks = 'Previously Appeared(Wrongly Applied)'  where Registration_Number = " + regno + " and Exam_ID = " + ExamID +
                                        " and Course_ID = " + CourseID + " and Module_ID = " + Convert.ToInt32(cexamdetail["Module_ID"]) + "";
                                EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql, con, null, CommandType.Text, true);
                                applistUpdated += regno + ":-" + cexamdetail["Module_ID"].ToString() + ",";
                                if (Updatedrecords % 10 == 0)
                                    applistUpdated += WebUtility.HtmlDecode("<br/>");
                            }
                        }
                        else
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                        }
                    }
                    string sql4 = " delete from Temp_Table  where Course_ID = " + CourseID + " and Exam_ID = " + ExamID + "";
                    EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sql4, con, null, CommandType.Text, true);

                    dr2.Close();
                    dr2.Dispose();
                    con.CommitTransaction();
                    con.Close();
                    lblTotalRecords.Text = TotalRecords.ToString();
                    lblupdatedRecords.Text = Updatedrecords.ToString();
                    if (Updatedrecords != 0)
                        lblUpdated.Text = "Modules Data for Registration No with Module-ID :- " + applistUpdated.TrimEnd(',').ToString() + " are  cancelled successfully";
                    lblNotValidate.Text = NotValidateRecords.ToString();
                    if (appIdList2 != "")
                        lblFailed.Text += "<br/> Modules Data for Module-ID:- " + appIdList2.TrimEnd(',').ToString() + ":" + arr[2].ToString();
                    if (appIdList3 != "")
                        lblFailed.Text += "<br/> Module Code:- " + appIdList3.TrimEnd(',').ToString() + " " + arr[0].ToString();
                    if (appIdList != "")
                        lblFailed.Text = "Registration No:-" + appIdList.TrimEnd(',').ToString() + ":" + arr[3].ToString();
                }
            }
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
            con.Close();
            System.IO.File.Delete(path);
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
        OleDbCommand command = new OleDbCommand("select * from [sub$]", connection);
        OleDbDataReader dr = command.ExecuteReader();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                CourseExamApplicationDetail rs;
                Module module;
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
                    Int32 modulesubcode = Convert.ToInt32(dr[4].ToString()); // sub_code (module sub-code)
                    String modulesname = dr[5].ToString();                  //  alpha_sub_code (module short_name)  
                    if (!String.IsNullOrEmpty(dr[3].ToString()) && !String.IsNullOrEmpty(dr[4].ToString()) && !String.IsNullOrEmpty(dr[5].ToString()))
                    {
                        rs = new CourseExamApplicationDetail();
                        module = new Module();
                        module = context.Modules.Where(s => s.ShortName == modulesname.ToUpper() && s.ModuleSubCode == modulesubcode && s.CourseID == CourseID).FirstOrDefault();
                        rs = context.CourseExamApplicationDetails.Where(s => s.RegistrationNumber == RegistrationNumber && s.ExamID == ExamID && s.CourseID == CourseID && s.ModuleID == module.ID).FirstOrDefault();
                        if (rs != null)
                        {
                            if (rs.ModuleID != module.ID)
                            {
                                rs.ModuleID = module.ID;
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
                string message = "Module Data Uploaded Successfully";
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
}