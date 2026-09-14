using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Common_RegionalDataUpload :BasePage 
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
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
          
                if (!IsPostBack)
                {
                    BindCourseCategory();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Regional Centre Data Upload Report", "#", ""));
                }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.CourseCategories
                              join c in context.Courses
                                  on s.ID equals c.CourseCategoryID
                              where c.CourseTypeID == courseTypeCertificateExam
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                ListItem lst1 = new ListItem("--All--", "0");
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
            throw ex;
        }

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            };
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
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
                                          where p.CourseTypeID == cr.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            ddlAppType.SelectedValue = "0";
            ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
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
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);

                };
            }
            else
            {
                ddlExamCycle.Items.Clear();
                ddlExamCycle.Items.Insert(0, lst);
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
            }
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
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    var courses = (from s in context.Exams                                   
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId                                   
                                   select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                    ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                }
                else
                {
                    ddlExamYear.Items.Clear();
                    ddlExamYear.Items.Insert(0, lst);
                    ddlExamYear.SelectedValue = "0";
                    ddlExamYear_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    var courses = (from s in context.Exams                                   
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                   && s.DateOfPublishingOfTimeTable != null
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                   
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                  
                }
            };
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
            ddlExamYear.SelectedValue = "0";
            Lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}