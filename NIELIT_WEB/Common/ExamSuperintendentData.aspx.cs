using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.OleDb;
using System.Text;
using System.Data.Objects;

public partial class Common_ExamSuperintendentData :BasePage
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
            if (loginUserType == UserType.RegionalCenter)
                btnMode.Visible = true;
            else
                btnMode.Visible = false;
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCategories();
                    FillRegionalCenter();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilterCourses();
                    if (Request.QueryString["ExamID"] != null && Request.QueryString["ExamYear"]!=null && Request.QueryString["ExamCycleID"]!=null && Request.QueryString["RegCentreID"]!=null && Request.QueryString["CourseID"]!=null)
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
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Superintendent Data:- " + "(" + ddlflExam.SelectedItem.Text + ")", "Common/ExamSuperintendentData.aspx?ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue + "&CourseID=" + ddlflCourse.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Superintendent Data", "Common/ExamSuperintendentData.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria to View Exam Superintendent Data";
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

                Int32 CourseType2 = Convert.ToInt32(enmCourseType.IRDACat);
                var Category = from p in context.CourseCategories
                               where p.ID == CourseType || p.ID == CourseType2
                               orderby (p.Name) descending
                               select new { ValueField = p.ID, TextField = p.Name };


                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillRegionalCenter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var rc = (from r in context.RegionalCenters
                          select new { ValueField = r.ID, TextField = r.Name }).Distinct();
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
                var rc = (from s in context.RegionalCenters
                          select new { ValueField = s.ID, TextField = s.Name }).Distinct();
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

                var ExamName = (from c in context.Exams
                                where c.ExamYear == ExamYear && c.CourseID == CourseID && c.ExaminationCycleID == ExamCycleID
                                && c.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && c.DateOfPublishingOfTimeTable != null
                                select new { ValueField = c.ID, TextField = c.Name }).Distinct();
                ExamName = ExamName.OrderByDescending(s => s.ValueField);
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
                                orderby (p.Name)
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                                orderby (p.Name)
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamYear, ExamYear, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            ShowAlert(ex.Message);
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
            ShowAlert(ex.Message);
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
            ShowAlert(ex.Message);
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
            ShowAlert(ex.Message);
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
                var ExamName = (from p in context.CertificateExamApplications
                                join c in context.Exams on
                                    p.ExamID equals c.ID
                                where p.Exam.ExamYear == ExamYear && p.CourseID == CourseID && p.Exam.ExaminationCycleID == ExamCycleID && p.RollNumber != null
                                select new { ValueField = p.Exam.ID, TextField = p.Exam.Name }).Distinct().ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExam, ExamName, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            //btnupload.Text = "Update";
            btnupload.Visible = false;
            lblHeading.Text = "Exam Superintendent Data";
            tbls2.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examsuppID = Convert.ToInt32(Request.QueryString["Key"]);
                var examsuperitendent = (from p in context.ExamSuperintendents
                                         join d in context.Exams
                                             on p.ExamID equals d.ID
                                         join r in context.RegionalCenters
                                             on p.RegionalCentreID equals r.ID
                                         where p.ID == examsuppID
                                         select new
                                         {
                                             examname =d.Name,
                                             regname= r.Name,
                                             uploadfilename= p.UploadFile.Name,
                                             examcyclename= d.ExaminationCycle.Name
                                         }).FirstOrDefault();

                //lblup.Visible = false;
                flUpload.Visible = false;
                //btnSave.Visible = false;
                //Response.Write(tblprint.InnerHtml);
                //lblCourse.Text = ExamResult.Course.Code + "-" + ExamResult.CourseCategory.Code;
                lblExamDetail.Text = examsuperitendent.examname + "-" + examsuperitendent.examcyclename;
                lblRegName.Text = GetInitCap(examsuperitendent.regname);
                lblfname.Text = GetInitCap(examsuperitendent.uploadfilename);
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Superitendent Data:-Detail","",""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        //finally
        //{
        //  context.Dispose();
        //}
    }
    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 CourseID = 0;
                Int64 ExamID = 0;
                Int64 ExamYear = 0;
                Int64 ExamCycleID = 0;
                Int64 regcentreID = 0;
                lblError.Visible = false;
                if (!String.IsNullOrEmpty(Request.QueryString["ExamID"]))//main problem here
                {
                    ExamID = Convert.ToInt64(Request.QueryString["ExamID"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["RegCentreID"]))//main problem here
                {
                    regcentreID = Convert.ToInt64(Request.QueryString["RegCentreID"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["ExamCycleID"]))//main problem here
                {
                    ExamCycleID = Convert.ToInt64(Request.QueryString["ExamCycleID"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["examyear"]))//main problem here
                {
                    ExamYear = Convert.ToInt64(Request.QueryString["examyear"]);
                }
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();

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
                if (regcentreID != 0 && ExamID != 0)
                {
                    var ExamResults = from s in context.ExamSuperintendents
                                      join d in context.Exams
                                          on s.ExamID equals d.ID
                                      where s.RegionalCentreID == regcentreID
                                      select new
                                      {
                                          ID = s.ID,
                                          examid = s.ExamID,
                                          examname = d.Name,
                                          filename = s.UploadFile.Name,
                                          examyear = d.ExamYear,
                                          ExamCycleID = d.ExaminationCycleID,
                                          regcentreid = s.RegionalCentreID,
                                          uploadeddate = s.CreatedOn
                                      };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        ExamResults = ExamResults.Where(s => s.filename.ToUpper().Contains(searchString));
                    }
                    if (ExamID != 0)
                    {
                        ExamResults = ExamResults.Where(s => s.examid == ExamID);
                    }
                    if (ExamCycleID != 0)
                    {
                        ExamResults = ExamResults.Where(s => s.ExamCycleID == ExamCycleID);
                    }
                    if (ExamYear != 0)
                    {
                        ExamResults = ExamResults.Where(s => s.examyear == ExamYear);
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
                            case "examname":
                                if (sortOrder == "DESC")
                                    ExamResults = ExamResults.OrderByDescending(s => s.examname);
                                else
                                    ExamResults = ExamResults.OrderBy(s => s.examname);
                                break;
                            case "filename":
                                if (sortOrder == "DESC")
                                    ExamResults = ExamResults.OrderByDescending(s => s.filename);
                                else
                                    ExamResults = ExamResults.OrderBy(s => s.filename);
                                break;
                            case "uploadeddate":
                                if (sortOrder == "DESC")
                                    ExamResults = ExamResults.OrderByDescending(s => s.uploadeddate);
                                else
                                    ExamResults = ExamResults.OrderBy(s => s.uploadeddate);
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
                    else
                    {
                        lblError.Visible = false;
                        lblError.Text = "";
                    }
                }
                else
                {
                    Response.Redirect("ExamSuperintendentData.aspx", true);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                //tblNavLinks.Visible = true;
                divprint.Visible = false;
                FillCategories();
                FillRegionalCenter();
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                //btnSave.Visible = false;
                //Change the heading text as required
                lblHeading.Text = "Exam Superintendent Data";
                //Updating Breadcrumb
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Exam Superintendent Data", "", ""));
            }
            else
            {
                //BreadCrumb1.RemoveLastBreadCrumbItem();
                if (Request.QueryString["ExamID"] != null)
                {
                    Response.Redirect("ExamSuperintendentData.aspx?ExamID=" + Request.QueryString["ExamID"] + "&RegCentreID=" + Request.QueryString["RegCentreID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&examyear=" + Request.QueryString["examyear"] + "&CourseID=" + Request.QueryString["CourseID"], true);
                }
                else
                    Response.Redirect("ExamSuperintendentData.aspx", true);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Exam Superintendent Data:- "+ "(" + ddlflExam.SelectedItem.Text + ")", "Common/ExamSuperintendentData.aspx?ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue +"&CourseID="+ddlflCourse.SelectedValue, ""));
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
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + "&CourseID=" + ddlflCourse.SelectedValue);

                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + "&CourseID=" + ddlflCourse.SelectedValue);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            var ExamResult = from s in context.ExamSuperintendents
                             select new { Name = s.UploadFile.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                ExamResult = ExamResult.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            ExamResult = ExamResult.OrderBy(s => s.Name).Distinct();
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
        try
        {
            BreadCrumb1.Render();
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
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (Request.QueryString["ExamID"] != null)
            {
                Response.Redirect("ExamSuperintendentData.aspx?ExamID=" + Request.QueryString["ExamID"] + "&RegCentreID=" + Request.QueryString["RegCentreID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&examyear=" + Request.QueryString["examyear"] + "&CourseID=" + Request.QueryString["CourseID"], true);
            }
            else
                Response.Redirect("ExamSuperintendentData.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlExamCycle.Items.Clear();
            ddlExamYear.Items.Clear();
            ddlExamName.Items.Clear();
            ddlExamYear.Items.Insert(0, "--Select One--");
            ddlExamName.Items.Insert(0, "--Select One--");
            ddlRc.Items.Clear();
            ddlRc.Items.Insert(0, "--Select One--");
            FillExamCycle();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlExamYear.Items.Clear();
            ddlExamName.Items.Clear();
            ddlExamName.Items.Insert(0, "--Select One--");
            ddlRc.Items.Clear();
            ddlRc.Items.Insert(0, "--Select One--");
            FillExamYear();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlflCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlflExamCycle.Items.Clear();
            ddlflExamYear.Items.Clear();
            ddlflExam.Items.Clear();
            ddlflExamYear.Items.Insert(0, "--Select One--");
            ddlflExam.Items.Insert(0, "--Select One--");
            ddlRegionalCentre.Items.Clear();
            ddlRegionalCentre.Items.Insert(0, "--Select One--");
            FillFilterExamCycle();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlExamName.Items.Clear();
            ddlRc.Items.Clear();
            ddlRc.Items.Insert(0, "--Select One--");
            FillExamName();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlflExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlflExamYear.Items.Clear();
            ddlflExam.Items.Clear();
            ddlflExam.Items.Insert(0, "--Select One--");
            ddlRegionalCentre.Items.Clear();
            ddlRegionalCentre.Items.Insert(0, "--Select One--");
            FillFilterExamYear();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlflExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlflExam.Items.Clear();
            ddlRegionalCentre.Items.Clear();
            ddlRegionalCentre.Items.Insert(0, "--Select One--");
            FillFilterExam();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlRc.Items.Clear();
            FillRegionalCenter();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlflExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlRegionalCentre.Items.Clear();
            FillFilterRegionalCenter();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 regcentreID = Convert.ToInt32(ddlRc.SelectedValue);
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
             string regCentreCode="";
             string examname = "";

            using (EConnectContext context = new EConnectContext())
            {
                 regCentreCode = context.RegionalCenters.Find(regcentreID).Code;
                 examname = context.Exams.Find(examID).Name;
            }
            string filename = "Exam_Sup_" + regCentreCode + "-" + examname;
            divValidateData.Visible = true;
            //btnSave.Visible = true;
            BreadCrumb1.Render();

            StringBuilder sb = new StringBuilder();
            string[] arr = new string[10];
            //string appIdList = "";
            //string appIdList1 = "";
            string appIdList2 = "";
            //string appIdList3 = "";
            string applistUpdated = "";
            arr[0] = "is not correct.Please Correct the data and Upload again.";
            arr[1] = "Result Data File is not related to selected Regional Centre";
            arr[2] = "Data Already Uploaded";
            arr[3] = "does not exist";

            string filepath = Server.MapPath("../UploadedFiles");
            flUpload.SaveAs(filepath + "/" + flUpload.FileName);
            string path = (filepath + "/" + flUpload.FileName);
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
            //OleDbCommand command = new OleDbCommand("select distinct(cent_allot) as centrecode, EXAM_COM_DATE from [MS Access;Database=" + path + "].[Cons_EC_ES]", connection);
            //December_2024
            OleDbCommand command = new OleDbCommand("select distinct(cent_allot) as centrecode, EXAM_COM_DATE from [MS Access;Database=@path].[Cons_EC_ES]", connection);
            command.Parameters.AddWithValue("@path", path);
            OleDbDataReader dr = command.ExecuteReader();
            int TotalRecords = 0;
            int ValidateRecords = 0;
            int NotValidateRecords = 0;
            UploadedFile objFile;
            ExamSuperintendent examsuperitendent;
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    while (dr.Read())
                    {
                        TotalRecords = TotalRecords + 1;
                        string centrecode = (dr["centrecode"].ToString()).ToUpper().Trim();
                        //DateTime examcomdate = Convert.ToDateTime(dr["EXAM_COM_DATE"]);
                        if (context.CertificateExamApplications.Any(s => s.ExamCentreName.ToUpper().Trim() == centrecode))
                        {

                            ValidateRecords = ValidateRecords + 1;
                            objFile = new EConnect.NIELIT.UploadedFile();
                            objFile.Name = filename;
                            objFile.OriginalName = flUpload.FileName.ToString();
                            objFile.Extension = System.IO.Path.GetExtension(flUpload.FileName).ToLower();
                            objFile.BlobFile = flUpload.FileBytes;
                            objFile.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(objFile);
                            context.SaveChanges();
                            
                            examsuperitendent = new ExamSuperintendent();
                            examsuperitendent.RegionalCentreID = regcentreID;
                            examsuperitendent.ExamID = examID;
                            examsuperitendent.UploaddedFileID = objFile.ID;
                            examsuperitendent.CreatedBy = Convert.ToInt32(Session["UserID"]);
                            examsuperitendent.CreatedOn = DateTime.Now;
                            context.ExamSuperintendents.Add(examsuperitendent);
                            context.SaveChanges();
                            applistUpdated += centrecode.ToString() + ",";

                        }
                        else
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                            appIdList2 += centrecode.ToString() + ",";

                        }
                    }
                };
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblNotValidate.Text = NotValidateRecords.ToString();
                if (ValidateRecords != 0)
                    lblUpdated.Text = "Exam Superitendent data for  Exam centre code : " + applistUpdated.TrimEnd(',').ToString() + " are  uploaded successfully";
                lblNotValidate.Text = NotValidateRecords.ToString();
                if (appIdList2 != "")
                    lblFailed.Text = " Exam Superitendent data  for Exam Centre code " + appIdList2.TrimEnd(',').ToString() + ":" + arr[0].ToString();
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
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (hfActionID.Value != "")
            {
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "ViewFile")
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Int32 examsupdataID= Convert.ToInt32(recordID);
                        var examsuperitendentdata = (from r in context.ExamSuperintendents
                                                     where r.ID == examsupdataID
                                                     select r).FirstOrDefault();
                        if (examsuperitendentdata != null)
                        {
                            Response.Redirect("../Handlers/UploadedFileHandler.ashx?ID=" + examsuperitendentdata.UploaddedFileID,true);
                        }
                    };
                    BindGridView();
                    //ShowAlert("Password reset successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        //finally { context.Dispose(); }
    }
}