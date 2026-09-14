using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Admin_FeeReconciliation : BasePage
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Fee Reconcilliation Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }

    }

    #region-------Private Methods----------------

    private void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.CourseCategories
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses, lst);
            };
        }
        catch (Exception ex)
        { throw ex; }

    }

    #endregion--------------------------------

    #region-------Events-------------------------

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
                                       where s.CourseID == courseId && s.DateOfPublishingOfTimeTable.HasValue 
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct() ;
                        courses = courses.OrderByDescending(s => s.ValueField).Take(12); //Take for last 12 Months
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
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    #endregion--------------------------------
}