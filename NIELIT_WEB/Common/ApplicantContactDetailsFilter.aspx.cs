using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Common_ApplicantContactDetailsFilter : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
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
            if (!IsPostBack)
            {
                BindCourseCategory();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Applicant Contact Details Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }

    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (loginUserType == UserType.RegionalCenter)
                {

                    var courses = from s in context.CourseCategories
                                  join c in context.Courses
                                      on s.ID equals c.CourseCategoryID
                                  where c.CourseTypeID == courseTypeCertificateExam
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);
                }
                else
                {
                    var courses = from s in context.CourseCategories
                                  select new { ValueField = s.ID, TextField = s.Name };
                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                        courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                    }

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses, lst);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseCatId > 0)
                {
                    var courses = from s in context.Courses
                                  where s.CourseCategoryID == courseCatId
                                  select new { ValueField = s.ID, TextField = s.Name };

                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    courses = courses.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                }
                else
                {
                    ddlCourseName.Items.Clear();
                    ddlCourseName.Items.Insert(0, lst);
                }
                ddlCourseName.SelectedValue = "0";
                ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
            };

            if (ddlCourseCategry.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlmoduletype.Items.Clear();
                ddlmoduletype.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
                ddlAppType.SelectedValue = "0";
                ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
            };
            if (ddlCourseName.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlmoduletype.Items.Clear();
                ddlmoduletype.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ListItem lst = new ListItem("--All--", "0");
            ListItem lst2 = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                if (AppTypeId > 0 && CourseId > 0)
                {

                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                }
                else
                {
                    ddlExamCycle.Items.Clear();
                    ddlExamCycle.Items.Insert(0, lst2);
                }
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                if (ddlAppType.SelectedValue == "0")
                {
                    ListItem lst12 = new ListItem("--All--", "0");
                    ddlmoduletype.Items.Clear();
                    ddlmoduletype.Items.Add(lst12);
                }
                if (ddlAppType.SelectedValue == "3")
                {
                    ListItem lst11 = new ListItem("All", "0");
                    ListItem lst22 = new ListItem("Theory", "1");
                    ListItem lst33 = new ListItem("Practical", "2");
                    ddlmoduletype.Enabled = true;
                    ddlmoduletype.Items.Clear();
                    ddlmoduletype.Items.Add(lst11);
                    ddlmoduletype.Items.Add(lst22);
                    ddlmoduletype.Items.Add(lst33);
                }
                else
                {
                    ddlmoduletype.Enabled = false;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0 && ExamCycleId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                    {
                        Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                        if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        {
                            var courses = (from s in context.Exams
                                           join c in context.CourseRegistrationApplications
                                               on s.ID equals c.ApplicableExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        }
                        else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            var courses = (from s in context.Exams
                                           join c in context.CourseExamApplications
                                               on s.ID equals c.ExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        }
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {
                        var courses = (from s in context.Exams                                       
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                       select new { ValueField = s.ID, TextField = s.Name });
                        courses = courses.OrderByDescending(s => s.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                }
                ddlExamName.SelectedValue = "0";
            }
            if (ddlExamCycle.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlmoduletype.Items.Clear();
                ddlmoduletype.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlmoduletype.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}