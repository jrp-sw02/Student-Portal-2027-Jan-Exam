using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Common_DuplicateRecordsFilter : BasePage
{
    String strMessage = string.Empty;   
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Common/DuplicateRecordsFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                FillCategories();
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Duplicate Candidate Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindApplicationType(Int32 courseid)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                Int32 ccatid = (from c in context.Courses
                               where c.ID == courseid
                               select c.CourseTypeID).FirstOrDefault();

                var application = from s in context.ApplicationTypes
                                  where s.CourseTypeID == ccatid && s.ID == 1
                                  select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, application, lst);

            };
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
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 CourseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                var Category = from p in context.CourseCategories
                               join c in context.Courses
                                   on p.ID equals c.CourseCategoryID
                               where c.CourseTypeID == CourseTypeID
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 ccatID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        ddlcoursename.Items.Clear();
        BindCourseName(ccatID);
        if (ddlCourseCategry.SelectedValue == "0")
        {
            ListItem lst1 = new ListItem("--Select One--", "0");
            ddlAppType.Items.Clear();
            ddlAppType.Items.Add(lst1);
            ddlexamname.Items.Clear();
            ddlexamname.Items.Add(lst1);
            ddlbatchnumber.Items.Clear();
            ddlbatchnumber.Items.Add(lst1);
        }
    }
    protected void BindCourseName(Int32 ccatID)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == ccatID
                              orderby s.DisplayOrder ascending
                              select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )" };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursename, courses, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindBatchNumber(Int32 examid, Int32 appTypeID, Int32 courseID)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var batches = from s in context.Batchs
                              where s.ApplicationTypeID == appTypeID && s.ExamID == examid && s.CourseID == courseID
                              orderby s.CourseID ascending
                              select new { ValueField = s.ID, TextField = s.Number };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchnumber, batches, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindExamName(Int32 appTypeID, Int32 courseID)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                ListItem lst1 = new ListItem("--Select One--", "0");
                if (courseID > 0 && appTypeID > 0)
                {
                    if (appTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        var exams = (from s in context.Exams
                                     join c in context.CourseRegistrationApplications
                                     on s.ID equals c.ApplicableExamID
                                     where s.CourseID == courseID
                                     select new { ValueField = s.ID, TextField = s.Name }).Distinct();


                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, exams, lst);
                    }
                }
                else
                {
                    ddlexamname.Items.Clear();
                    ddlexamname.Items.Insert(0, lst1);
                }
                ddlexamname.SelectedValue = "0";
                ddlexamname_SelectedIndexChanged(ddlexamname, EventArgs.Empty);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReset_Click1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlAppType.SelectedValue = "0";
            ddlCourseCategry.SelectedValue = "0";
            ddlbatchnumber.SelectedValue = "0";
            ddlcoursename.SelectedValue = "0";
            ddlexamname.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlexamname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlbatchnumber.Items.Clear();
            Int32 appTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 examid = Convert.ToInt32(ddlexamname.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlcoursename.SelectedValue);
            BindBatchNumber(examid, appTypeID, courseID);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 appTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlcoursename.SelectedValue);
            BindExamName(appTypeID, courseID);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcoursename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 courseID = Convert.ToInt32(ddlcoursename.SelectedValue);
            ddlAppType.Items.Clear();
            BindApplicationType(courseID);
            if (ddlcoursename.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--Select One--", "0");
                ddlexamname.Items.Clear();
                ddlexamname.Items.Add(lst1);
                ddlbatchnumber.Items.Clear();
                ddlbatchnumber.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}