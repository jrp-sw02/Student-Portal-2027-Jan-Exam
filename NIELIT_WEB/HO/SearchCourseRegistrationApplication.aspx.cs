using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_SearchCourseRegistrationApplication : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
            if (!Page.IsPostBack)
            {
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                FillCourse();
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlregtype, typeof(enmRegistrationType), new ListItem("--Select One--", "0"));
                lblError.Visible = false;
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "Please Select Filter Criteria to View Application Records";
                    lblError.Visible = true;
                }
                if (Request.QueryString["courseid"] != null || Request.QueryString["examyear"] != null || Request.QueryString["examID"] != null || Request.QueryString["regtype"] != null)
                {
                    ddlCourseName.SelectedValue = Request.QueryString["courseid"];
                    ddlCourseName_SelectedIndexChanged(ddlCourseName.SelectedValue, EventArgs.Empty);
                    ddlExamYear.SelectedValue = Request.QueryString["examyear"];
                    ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                    ddlExamName.SelectedValue = Request.QueryString["examID"];
                    ddlregtype.SelectedValue = Request.QueryString["regtype"];
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Registration Applications:-" + ddlCourseName.SelectedItem.Text + "-" + "(" + ddlExamName.SelectedItem.Text + ")", "HO/SearchCourseRegistrationApplication.aspx?courseid=" + ddlCourseName.SelectedValue + "&examID=" + ddlExamName.SelectedValue + "&examyear=" + ddlExamYear.SelectedValue + "&regtype=" + ddlregtype.SelectedValue, ""));
                    BreadCrumb1.Render();
                    BindGridView();
                }
                else
                {
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Registration Applications", "HO/SearchCourseRegistrationApplication.aspx", ""));
                    BreadCrumb1.Render();
                }
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillCourse()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseTypeId = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var Courses = from p in context.Courses
                              where p.CourseTypeID == CourseTypeId
                              orderby p.ID
                              select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    Courses = Courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, Courses, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int couID = Convert.ToInt32(ddlCourseName.SelectedValue);
        ddlExamYear.Items.Clear();
        FillExamYears(couID);
        ListItem lst = new ListItem("--Select One--", "0");
        if (ddlCourseName.SelectedValue == "0")
        {
            ddlExamYear.Items.Add(lst);
        }

        ddlExamName.Items.Clear();
        ddlExamName.Items.Add(lst);
    }
    protected void FillExamYears(int couID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0)
                {
                    //var ExamYear = (from p in context.Exams
                    //                join q in context.CourseRegistrationApplications
                    //                on p.ID equals q.ApplicableExamID
                    //                where (p.CourseID == couID)                                    
                    //                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();                   
                    var ExamYear = context.Exams.Where(E => E.CourseID == couID).Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(6);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 couID = Convert.ToInt32(ddlCourseName.SelectedValue);
        Int32 exmYear = Convert.ToInt32(ddlExamYear.SelectedValue);
        ddlExamName.Items.Clear();
        FillExamNames(couID, exmYear);
        ListItem lst = new ListItem("--Select One--", "0");
        if (ddlExamYear.SelectedValue == "0")
        {
            ddlExamName.Items.Add(lst);
        }
    }
    protected void FillExamNames(Int32 couID, Int32 exmYear)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && exmYear != 0)
                {
                    var examName = (from p in context.Exams
                                    join q in context.CourseRegistrationApplications
                                    on p.ID equals q.ApplicableExamID
                                    where (p.CourseID == couID && p.ExamYear == exmYear)
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            Response.Redirect("SearchCourseRegistrationApplication.aspx", true);
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if (ddlCourseName.SelectedValue != "0" || ddlExamName.SelectedValue != "0" || ddlExamYear.SelectedValue != "0" || ddlregtype.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Course Registration Applications:-" + ddlCourseName.SelectedItem.Text + "-" + "(" + ddlExamName.SelectedItem.Text + ")", "HO/SearchCourseRegistrationApplication.aspx?courseid=" + ddlCourseName.SelectedValue + "&examID=" + ddlExamName.SelectedValue + "&examyear=" + ddlExamYear.SelectedValue + "&regtype=" + ddlregtype.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
                BindGridView();
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
            ddlCourseName.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlregtype.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("SearchCourseRegistrationApplication.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
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
            BreadCrumb1.Render();
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
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
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
    protected void BindGridView()
    {
        try
        {
            lblError.Visible = false;
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int32 courseID = 0;
            Int32 examYear = 0;
            Int32 examName = 0;
            Int32 regtype = 0;
            if (ddlCourseName.SelectedValue != "0")
            {
                courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["courseid"]))
                    courseID = Convert.ToInt32(Request.QueryString["courseid"]);
            }
            if (ddlExamYear.SelectedValue != "0")
            {
                examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examyear"]))
                    examYear = Convert.ToInt32(Request.QueryString["examyear"]);
            }
            if (ddlExamName.SelectedValue != "0")
            {
                examName = Convert.ToInt32(ddlExamName.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examID"]))
                    examName = Convert.ToInt32(Request.QueryString["examID"]);
            }
            if (ddlregtype.SelectedValue != "0")
            {
                regtype = Convert.ToInt32(ddlregtype.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["regtype"]))
                    regtype = Convert.ToInt32(Request.QueryString["regtype"]);
            }
            Int32 apptypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
            ucSearchBar.AutoCompleteContextKey = ddlCourseName.SelectedValue + "," + ddlExamName.SelectedValue + "," + ddlregtype.SelectedValue;
            upbreadsearch.Update();
            if (courseID != 0 && examYear != 0 && examName != 0 && regtype != 0)
            {
                var application = (from p in context.CourseRegistrationApplications
                                   where p.ApplicableExamID == examName && p.RegistrationTypeID == regtype && p.FinalSubmitted == true && p.CourseID == courseID
                                   select new
                                   {
                                       Id = p.ID,
                                       Appno = p.Number,
                                       Appdate = p.ApplicationDate,
                                       apptypeID = apptypeID,
                                       Name = p.Salutation + p.Name,
                                       fathername = !string.IsNullOrEmpty(p.FatherName) ? "Mr. " + p.FatherName : "NA",
                                       key = p.ApplicationStatusID,
                                       examID = p.ApplicableExamID
                                   });
                if (!String.IsNullOrEmpty(searchString))
                {
                    application = application.Where(s => s.Appno.ToUpper().Contains(searchString)
                                           || s.Name.ToUpper().Contains(searchString));
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "Id":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.Id);
                            else
                                application = application.OrderBy(s => s.Id);
                            break;
                        case "Appno":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.Appno);
                            else
                                application = application.OrderBy(s => s.Appno);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.Name);
                            else
                                application = application.OrderBy(s => s.Name);
                            break;
                        case "fathername":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.fathername);
                            else
                                application = application.OrderBy(s => s.fathername);
                            break;
                        case "Appdate":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.Appdate);
                            else
                                application = application.OrderBy(s => s.Appdate);
                            break;
                        default:
                            application = application.OrderBy(s => s.Appno);
                            break;
                    }
                }
                PagingBar1.Visible = true;
                PagingBar1.Bind(application, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
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
                Response.Redirect("SearchCourseRegistrationApplication.aspx", true);
            }
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
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int32 examid = 0;
            Int32 courseid = 0;
            Int32 regtype = 0;
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                courseid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                examid = Convert.ToInt32(keys[1]);
            if (!String.IsNullOrEmpty(keys[2]))
                regtype = Convert.ToInt32(keys[2]);
            string searchString = prefixText.Trim().ToUpper();
            var applications = from s in context.CourseRegistrationApplications
                               where s.ApplicableExamID == examid && s.RegistrationTypeID == regtype && s.CourseID == courseid
                               select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.OrderBy(s => s.Name);

            var applications1 = from s in context.CourseRegistrationApplications
                                where s.ApplicableExamID == examid && s.RegistrationTypeID == regtype && s.CourseID == courseid
                                select new { Name = s.Number };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications1 = applications1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.Union(applications1).Take(count);
            foreach (var user in applications)
            {
                items.Add(user.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
}