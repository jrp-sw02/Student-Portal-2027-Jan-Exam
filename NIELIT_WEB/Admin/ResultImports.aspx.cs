using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class ResultImports : BasePage
{
    String strMessage = string.Empty;    
    UserType loginUserType;
    Int64 entityID = 0;
    Table tbl = new Table();
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
                    //
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
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Result: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/ResultImports.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Result", "Admin/ResultImports.aspx", ""));
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
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);

                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.ID == CourseType || p.ID == 8
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };


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
                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                ListItem lst1 = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                if (loginUserType == UserType.RegionalCenter)
                {
                    ddlRc.SelectedValue = entityID.ToString();
                    ddlRc.Enabled = false;
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
                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                Int32 examID = Convert.ToInt32(ddlflExam.SelectedValue);
                ListItem lst1 = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, rc.OrderBy(c => c.TextField), lst1);
                if (loginUserType == UserType.RegionalCenter)
                {
                    ddlRegionalCentre.SelectedValue = entityID.ToString();
                    ddlRegionalCentre.Enabled = false;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                               orderby (p.ExamMonth) descending
                               select new { ValueField = p.ID, TextField = p.Name };

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
                var ExamYear = (from p in context.Exams
                                where p.ExaminationCycleID == ExamCycleID                                
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct().OrderByDescending(s => s.ValueField);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
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
                Int32 CourseId = Convert.ToInt32(ddlflCourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlflExamCycle.SelectedValue);
                var ExamYear = context.Exams.Where(E => E.CourseID == CourseId && E.ExaminationCycleID == ExamCycleID).Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField);  
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
                //var ExamName = from p in context.CertificateExamApplications.AsNoTracking()
                //               join c in context.Exams on
                //                   p.ExamID equals c.ID
                //               where p.Exam.ExamYear == ExamYear && p.CourseID == CourseID && p.ResultGradeID != null && p.Exam.ExaminationCycleID == ExamCycleID
                //               select new { ValueField = p.Exam.ID, TextField = p.Exam.Name };
                var ExamName = context.Exams.Where(E => E.CourseID == CourseID && E.ExamYear == ExamYear && E.ExaminationCycleID == ExamCycleID && E.DateOfPublishingOfRollNumber != null).OrderByDescending(s => s.ExamMonth)
                                 .Select(s => new { ValueField = s.ID, TextField = s.Name }).Distinct();
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
            lblHeading.Text = "Certificate Exam Result";
            tblNavLinks.Visible = true;
            //imgSampleDoc.Visible = false;
            tbls2.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ExamResultID = Convert.ToInt32(Request.QueryString["Key"]);
                var ExamResult = (from p in context.CertificateExamApplications.AsNoTracking()
                                  where p.ID == ExamResultID
                                  select p).FirstOrDefault();
                ddlcoursecategory.SelectedValue = ExamResult.CourseCategoryID.ToString();
                ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);
                ddlcourse.SelectedValue = ExamResult.CourseID.ToString();
                ddlcourse_SelectedIndexChanged(ddlcourse, EventArgs.Empty);
                ddlExamCycle.SelectedValue = ExamResult.Exam.ExaminationCycleID.ToString();
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                ddlExamName.SelectedValue = ExamResult.ExamID.ToString();
                ddlcoursecategory.Enabled = false;
                ddlcourse.Enabled = false;
                ddlExamCycle.Enabled = false;
                ddlExamName.Enabled = false;
                //lblup.Visible = false;
                flUpload.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                //Response.Write(tblprint.InnerHtml);
                //lblCourse.Text = ExamResult.Course.Code + "-" + ExamResult.CourseCategory.Code;
                lblExamDetail.Text = ExamResult.Exam.Name + "-" + ExamResult.Exam.ExaminationCycle.Name;
                //lblRegName.Text = GetInitCap(ExamResult.RegionalCenter.Name);
                lblRollno.Text = ExamResult.RollNumber.ToString();
                lblCandidate.Text = ExamResult.Salutation + " " + ExamResult.Name.ToString();
                if (string.IsNullOrEmpty(ExamResult.GuardianName) == true && string.IsNullOrWhiteSpace(ExamResult.GuardianName))
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    if (!String.IsNullOrEmpty(ExamResult.FatherName))
                        lblFather.Text = "Mr. " + GetInitCap(ExamResult.FatherName);
                    else
                        lblFather.Text = "NA";
                    if (!String.IsNullOrEmpty(ExamResult.MotherName))
                        lblMother.Text = "Mrs. " + GetInitCap(ExamResult.MotherName);
                    else
                        lblMother.Text = "NA";
                }
                else
                {
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    LblGuardianName.Text = GetInitCap(ExamResult.GuardianName);
                    Trrollnumber.Attributes.Add("class", "gdrow1");
                    Trexamname.Attributes.Add("class", "gdalternate1");
                    TrGrade.Attributes.Add("class", "gdrow1");
                    Tresult.Attributes.Add("class", "gdalternate1");
                    Trcentrecode.Attributes.Add("class", "gdrow1");
                    Trdoe.Attributes.Add("class", "gdalternate1");
                }

                //lblAccNo.Text = ExamResult.Institute.AccreditationDetails.FirstOrDefault().AccreditationNumber.ToString();
                //lblInsName.Text = ExamResult.Institute.Name.ToString();
                lblResult.Text = ExamResult.ResultGrade.Code.ToString();
                lblDescription.Text = ExamResult.ResultGrade.Description.ToString();
                //lblPercentage.Text = ExamResult.ResultGrade.PercentageFrom.ToString() + " To " + ExamResult.ResultGrade.PercentageTo.ToString();
                lblCentreCode.Text = ExamResult.ExamCentreName.ToString();
                lblDateofExam.Text = ExamResult.DateOfExam.Value.ToString("dd-MMM-yyyy");
                // hlAppDetail.Attributes.Add("Onclick", "return showForm('../CAND/CertificatePreview.aspx?Appid=' " + ApplicationID + " ');");
                hlAppDetail.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?Appid=" + ExamResultID) + "');");
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(ExamResult.RollNumber + "-" + "(" + ExamResult.Name + ")", "Admin/ResultImports.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                ShowLegends(ExamResult.CourseCategoryID, ExamResult.ExamID);
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
            using (EConnectContext context = new EConnectContext())
            {
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
                var ExamResults = from s in context.CertificateExamApplications.AsNoTracking()
                                  where s.RollNumber != null && s.RegionalCenterID == regcentreID && s.ResultGradeID != null
                                  select new
                                  {
                                      ID = s.ID,
                                      CourseID = s.CourseID,
                                      ExamCycleID = s.Exam.ExaminationCycleID,
                                      ExamYear = s.Exam.ExamYear,
                                      ExamID = s.ExamID,
                                      Rollno = s.RollNumber,
                                      examName = s.Exam.Name,
                                      Name = s.Name,
                                      Father = (s.FatherName != null && s.MotherName != null) ? s.FatherName : s.GuardianName,
                                      Result = s.ResultGrade.Code,
                                      ExamDate = s.Exam.ExamStartDate,
                                      IsPassed = s.ResultGrade.IsPassed == true ? "Pass" : "Fail"
                                  };
                if (!String.IsNullOrEmpty(searchString))
                {
                    ExamResults = ExamResults.Where(s => s.Rollno.ToUpper().Contains(searchString));
                }

                if (CourseID != 0 && ExamCycleID != 0 && ExamYear != 0 && ExamID != 0)
                {
                    ExamResults = ExamResults.Where(s => s.CourseID == CourseID && s.ExamCycleID == ExamCycleID && s.ExamYear == ExamYear && s.ExamID == ExamID);
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
                        case "Name":
                            if (sortOrder == "DESC")
                                ExamResults = ExamResults.OrderByDescending(s => s.Name);
                            else
                                ExamResults = ExamResults.OrderBy(s => s.Name);
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
                        case "Result":
                            if (sortOrder == "DESC")
                                ExamResults = ExamResults.OrderByDescending(s => s.Result);
                            else
                                ExamResults = ExamResults.OrderBy(s => s.Result);
                            break;
                        case "examName":
                            if (sortOrder == "DESC")
                                ExamResults = ExamResults.OrderByDescending(s => s.examName);
                            else
                                ExamResults = ExamResults.OrderBy(s => s.examName);
                            break;
                        case "IsPassed":
                            if (sortOrder == "DESC")
                                ExamResults = ExamResults.OrderByDescending(s => s.IsPassed);
                            else
                                ExamResults = ExamResults.OrderBy(s => s.IsPassed);
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
            };
        }
        catch (Exception ex)
        { throw ex; }
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
            lblHeading.Text = "Certificate Exam Results";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Certificate Exam Result", "", ""));
        }
        else
        {

            if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
            {
                Response.Redirect("ResultImports.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&RegCentreID=" + Request.QueryString["RegCentreID"], true);
            }
            else
            {
                Response.Redirect("ResultImports.aspx", true);
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
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Certificate Exam Result: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/ResultImports.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue, ""));
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
        ddlExamCycle.Items.Insert(0, "--Select One--");
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Clear();
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
            using (TransactionScope scope = new TransactionScope())
            {

                using (EConnectContext context = new EConnectContext())
                {
                    Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                    Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                    Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                    Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                    Int32 regCentreID = Convert.ToInt32(ddlRc.SelectedValue);                    
                    CertificateExamApplication rs;
                    ResultGrade rg = new ResultGrade();
                    RegionalCenter rc = new RegionalCenter();
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        while (dr.Read())
                        {
                            if (dr["Result"] != null)
                            {
                                string Rollno = dr["Rollno"].ToString().ToUpper();
                                rs = new CertificateExamApplication();

                                rs = context.CertificateExamApplications.Where(s => s.RollNumber.ToUpper() == Rollno).FirstOrDefault();
                                if (rs != null)
                                {
                                    if (rs.ResultGradeID == null && rs.RegionalCenterID == regCentreID)
                                    {
                                        string ResultGrade = dr["Result"].ToString().ToUpper();
                                        rg = context.ResultGrades.Where(s => s.Code.ToUpper() == ResultGrade && s.CourseCategoryID == 2).FirstOrDefault();
                                        rs.ResultGradeID = rg.ID;
                                        rs.ResultUpdatedOn = DateTime.Now;
                                        rs.ResultUpdatedBy = Convert.ToInt32(Session["UserID"]);
                                        context.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    else
                                    {
                                        ShowAlert("Your Are Not Authorised To Upload This Data");
                                    }
                                }
                            }
                        }

                    }
                    finally { context.ChangeTracker.DetectChanges(); }
                    context.SaveChanges();
                };
                scope.Complete();
            };
            string message = "Result Uploaded Successfully";
            ShowAlert(message, true);


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
            Response.Redirect("ResultImports.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&RegCentreID=" + Request.QueryString["RegCentreID"], true);
        }
        else
        {
            Response.Redirect("ResultImports.aspx", true);
        }
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamCycle.Items.Clear();
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Clear();
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
    protected void btnValidate_Click(object sender, EventArgs e)
    {
        divValidateData.Visible = true;
        //btnSave.Visible = true;
        btnValidate.Visible = false;
        BreadCrumb1.Render();

        StringBuilder sb = new StringBuilder();
        string[] arr = new string[10];
        string appIdList = "";
        string appIdList1 = "";
        string appIdList2 = "";
        string appIdList3 = "";
        string applistUpdated = "";
        arr[0] = "Uploaded Result Data is not correct.Please Correct the data and Upload again.";
        arr[1] = "Result Data File is not related to selected Regional Centre";
        arr[2] = "Data Already Uploaded";
        arr[3] = "Record does not exist";

        string filepath = Server.MapPath("../UploadedFiles");
        flUpload.SaveAs(filepath + "/" + flUpload.FileName);
        string path = (filepath + "/" + flUpload.FileName);
        string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        string accessConnectionString = "";
        //string sExcelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilePath + ";Extended Properties=" + "\"Excel 8.0;HDR=YES;\"";
        if (ext.ToUpper() == ".MDB")
            accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        // accessConnectionString = string.Format("Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Persist Security Info=False;", path);
        else if (ext.ToUpper() == ".ACCDB")
            accessConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;HDR=Yes'", path);
            //accessConnectionString = string.Format("Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Persist Security Info=False;", path);
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
        int TotalRecords = 0;
        int ValidateRecords = 0;
        int NotValidateRecords = 0;
        List<StudentList> candidateList = new List<StudentList>();
        try
        {

            Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
            Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 regCentreID = Convert.ToInt32(ddlRc.SelectedValue);

            while (dr.Read())
            {

                TotalRecords = TotalRecords + 1;
                Int64 applicationID = Convert.ToInt64(dr["Application_ID"].ToString());
                EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                if (!String.IsNullOrEmpty(dr["Result"].ToString()) && !String.IsNullOrEmpty(dr["Rollno"].ToString()))
                {
                    //to enter e-mail and mobile number for the sms /email
                    DataTable dt = EConnect.Utils.Data.DbUtility.GetDataTable("SELECT Salutaion,Name,Mobile,Email,Roll_Number FROM Certificate_Exam_Application where ID= " + applicationID, con, null, CommandType.Text, true);
                    if (dt.Rows.Count > 0)
                    {
                        StudentList st = new StudentList();
                        st.AppName = dt.Rows[0]["Name"].ToString();
                        st.Email = dt.Rows[0]["Email"].ToString();
                        st.Salutation = dt.Rows[0]["Salutaion"].ToString();
                        st.MobileNo = Convert.ToInt64(dt.Rows[0]["Mobile"].ToString());
                        st.RollNumber = dt.Rows[0]["Roll_Number"].ToString();
                        st.ExamName = ddlExamName.SelectedItem.Text;
                        st.CourseName = ddlcourse.SelectedItem.Text;
                        candidateList.Add(st);
                    }

                    SqlCommand scCommand = new SqlCommand("updateResultBCC_CCC", new SqlConnection(con.ConnectionString));
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@AppID", SqlDbType.BigInt).Value = applicationID;
                    scCommand.Parameters.Add("@RegionalCenterID", SqlDbType.Int).Value = regCentreID;
                    scCommand.Parameters.Add("@ResultGrade ", SqlDbType.NVarChar, 10).Value = dr["Result"].ToString();
                    scCommand.Parameters.Add("@ResultUpdatedByID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"]);
                    scCommand.Parameters.Add("@ExamID", SqlDbType.Int).Value = ExamID;
                    scCommand.Parameters.Add("@RollNumber", SqlDbType.NVarChar, 100).Value = dr["Rollno"].ToString();
                    scCommand.Parameters.Add("@CourseCatID", SqlDbType.Int).Value = CourseCategoryID;
                    scCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = CourseID;
                    try
                    {
                        if (scCommand.Connection.State == ConnectionState.Closed)
                        {
                            scCommand.Connection.Open();
                        }
                        var result = scCommand.ExecuteScalar();

                        if (result.ToString() == "-1")
                        {
                            ValidateRecords = ValidateRecords + 1;
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
                    }
                    catch (Exception)
                    {
                        NotValidateRecords = NotValidateRecords + 1;
                        appIdList += applicationID.ToString() + ",";
                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                            appIdList += WebUtility.HtmlDecode("<br/>");
                    }
                    finally
                    {
                        scCommand.Connection.Close();
                    }
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
            Thread threademail = new Thread(() => SentBulkEmail(candidateList, Convert.ToInt32(Session["UserID"])));
            threademail.Start();
            Thread threadsms = new Thread(() => SentBulkSMS(candidateList, Convert.ToInt32(Session["UserID"])));
            threadsms.Start();
            lblTotalRecords.Text = TotalRecords.ToString();
            lblValidateRecords.Text = ValidateRecords.ToString();
            lblNotValidate.Text = NotValidateRecords.ToString();
            if (ValidateRecords != 0)
                lblUpdated.Text = "Result Data  for Application ID : " + applistUpdated.TrimEnd(',').ToString() + " are  updated successfully";
            lblNotValidate.Text = NotValidateRecords.ToString();
            if (appIdList != "")
                lblFailed.Text = "<b>" + arr[0].ToString() + " For Application ID:- </b> <br/>" + appIdList.TrimEnd(',').ToString();
            if (appIdList1 != "")
                lblFailed.Text += "<br/> <b>" + arr[1].ToString() + " For Application ID:- </b><br/> " + appIdList1.TrimEnd(',').ToString();
            if (appIdList2 != "")
                lblFailed.Text += "<br/> <b>" + arr[2].ToString() + " For Application ID:- </b> <br/> " + appIdList2.TrimEnd(',').ToString();
            if (appIdList3 != "")
                lblFailed.Text += "<br/> <b>" + arr[3].ToString() + " For Application ID:- </b> <br/> " + appIdList3.TrimEnd(',').ToString();
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
    protected void ShowLegends(Int32 courseCategory, Int32 ExamID)
    {
        try
        {
            tbl.Width = Unit.Percentage(100);
            tbl.CellSpacing = 0;
            tbl.BorderColor = System.Drawing.Color.Black;
            tbl.BorderWidth = Unit.Pixel(1);
            tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            //showTableheader();           
            string[] heading = { "Grade", "Marks Range (in %)", "Remarks" };
            using (EConnectContext context = new EConnectContext())
            {
                Int32[] notInGrades = { 89, 84 };
                Int32 ResultVersionID = context.Exams.Where(s => s.ID == ExamID).FirstOrDefault().ResultGradeVersionID.Value;
                var grade = (from g in context.ResultGrades
                             where g.CourseCategoryID == courseCategory && !notInGrades.Contains(g.ID) && g.VersionID == ResultVersionID
                             orderby g.Code
                             select new
                             {
                                 grade = g.Code,
                                 description = g.Description,
                                 legend1 = g.PercentageFrom,
                                 legend2 = g.PercentageTo
                             }).ToList();
                if (grade.Count() >= 0)
                {
                    int rowCounter = 0;
                    for (rowCounter = 0; rowCounter < 3; rowCounter++)
                    {
                        TableRow tr = new TableRow();
                        //if (i % 2 == 0)
                        //    tr.CssClass = "normal";
                        //else
                        //    tr.CssClass = "normal";
                        //i++;
                        tbl.Rows.Add(tr);

                        TableCell tdHeading = new TableCell();
                        tdHeading.Text = heading[rowCounter].ToString();
                        tdHeading.HorizontalAlign = HorizontalAlign.Left;
                        tdHeading.Font.Bold = true;
                        tdHeading.Width = Unit.Percentage(20);
                        tdHeading.BorderColor = System.Drawing.Color.Black;
                        tdHeading.BorderWidth = Unit.Pixel(1);
                        tdHeading.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(tdHeading);
                        foreach (var result in grade)
                        {
                            TableCell tdGrade = new TableCell();
                            tdGrade.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdGrade);
                            tdGrade.Font.Bold = false;
                            tdGrade.BorderColor = System.Drawing.Color.Black;
                            tdGrade.BorderWidth = Unit.Pixel(1);
                            if (rowCounter == 0)
                            {
                                tdGrade.Font.Bold = true;
                                tdGrade.Text = result.grade.ToUpper();
                            }
                            else if (rowCounter == 1)
                            {
                                if (result.legend1 != null && result.legend1 > 0 && result.legend2 != null && result.legend2 > 0)
                                    tdGrade.Text = result.legend1 + " to " + result.legend2;
                                else
                                    tdGrade.Text = result.grade;
                            }
                            else
                                tdGrade.Text = (result.description);
                            tdGrade.VerticalAlign = VerticalAlign.Top;
                        }
                    }
                }
                tdLegends.Controls.Add(tbl);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showTableheader()
    {

        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(20);
            tcCol.Text = "Grade";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(30);
            tcCol2.Text = "Marks(%)";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(50);
            tcCol1.Text = "Result";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void SentBulkSMS(List<StudentList> candidateList, Int32 UserID)
    {
        for (int i = 0; i < candidateList.Count(); i++)
        {
            string mobilemsg = "Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + "," + " Result for the " + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " Exam  has been declared by NIELIT. Please check your E-mail regarding this.";
            try
            {
                //sending SMS 
                if (candidateList[i].MobileNo != 0)
                {
                    EConnect.NIELIT.SMS sms = new SMS(mobilemsg, candidateList[i].MobileNo.ToString(), "1307161052934603903", SmsServiceType.BulkSMS, false);
                    int sentMessageCount;
                    sms.sendSingleSMS(out sentMessageCount);
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }
        }
    }

    protected void SentBulkEmail(List<StudentList> candidateList, Int32 UserID)
    {
        for (int i = 0; i < candidateList.Count(); i++)
        {
            string emailmsg = "Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + ",<br/><br/>" + " Result for the <b>" + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " </b> Exam  has been declared by NIELIT. You can view your result on <b> NIELIT Website (https://student.nielit.gov.in) </b> with your Roll-Number (<b>" + candidateList[i].RollNumber + "</b>)";
            try
            {
                //sending SMS 
                if (candidateList[i].Email.Trim().Length > 0)
                {
                    EConnect.NIELIT.Email mail = new Email("Result Notification:NIELIT", emailmsg, candidateList[i].Email, false);
                    mail.SendInThread(UserID);
                }
            }
            catch (Exception ex)
            {

            }
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
        public String RollNumber
        {
            get;
            set;
        }

    }
}