using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;

public partial class Common_InstituteResultFilter : BasePage
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Institute Result Report", "#", ""));
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
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                var InstCategory = context.AccreditationDetails.Where(a => a.InstituteID == entityID).Select(k => k.CourseCategoryID).Distinct();
                Category = Category.Where(a => InstCategory.Contains(a.ValueField));

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
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
            ListItem lst = new ListItem("--All--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {

                Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                ListItem lst1 = new ListItem("--Select One--", "0");
                if (CourseId > 0 && AppTypeId > 0)
                {
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                }
                else
                {
                    //ddlExamCycle.Items.Clear();
                    //ddlExamCycle.Items.Insert(0, lst);
                    ddlExamCycle.SelectedValue = "0";
                    ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                }
                //ddlExamCycle.SelectedValue = "0";
                //ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
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
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID && p.ID != 1
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    //ddlAppType.Items.Clear();
                    //ddlAppType.Items.Insert(0, lst);
                    ddlAppType.SelectedValue = "0";
                    ddlAppType_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                }
            };
            //ddlAppType.SelectedValue = "0";
            //ddlAppType_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
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
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            Int32 currentyear = Convert.ToInt32(DateTime.Now.Year) - 3;
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                ListItem lst1 = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                    {
                        Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                        if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            var examname = from p in context.Exams
                                           where p.CourseID == courseId && p.DateOfPublishingOfResult != null &&
                                           p.DateOfPublishingOfResult <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && p.ExamYear > currentyear && p.ExaminationCycleID == ExamCycleId
                                           orderby p.ExamMonth, p.Name
                                           select new { ValueField = p.ID, TextField = p.Name };

                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examname, lst);
                        }
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {
                        var examname = from p in context.Exams
                                       where p.CourseID == courseId && p.DateOfPublishingOfResult != null &&
                                       p.DateOfPublishingOfResult <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && p.ExamYear > currentyear && p.ExaminationCycleID == ExamCycleId
                                       orderby p.ExamMonth
                                       select new { ValueField = p.ID, TextField = p.Name };
                        examname = examname.OrderByDescending(s => s.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examname, lst);
                    }
                }
                else
                {
                    ddlExamName.SelectedValue = "0";
                }
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
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}