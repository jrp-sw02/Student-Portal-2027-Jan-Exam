using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.Objects;

public partial class Common_RegionalCentrePaymentReport : BasePage
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Regional Centre Payment Report", "#", ""));
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
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            ddlAppType.Items.Clear();
            if (courseCatId != 0)
            {
                BindApplicationType(courseCatId);
            }
            else
            {
                ddlAppType.Items.Add(new ListItem("--Select One--", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindApplicationType(Int32 ccatid)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseTypeID = context.Courses.Where(a => a.CourseCategoryID == ccatid).FirstOrDefault().CourseTypeID;
                ListItem lst = new ListItem("--Select One--", "0");
                var application = from s in context.ApplicationTypes
                                  where s.CourseTypeID == courseTypeID
                                  select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, application, lst);

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
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                if (AppTypeID > 0)
                {
                    if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        var courses = from s in context.Exams
                                      join c in context.CourseRegistrationApplications
                                       on s.ID equals c.ApplicableExamID
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.ID,
                                          TextField = s.Name
                                      };

                        courses = courses.Distinct().OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                    else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        var courses = from s in context.Exams
                                      join c in context.CourseExamApplications
                                       on s.ID equals c.ExamID
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.ID,
                                          TextField = s.Name
                                      };

                        courses = courses.Distinct().OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                    else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        var courses = from s in context.Exams
                                      where s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.ID,
                                          TextField = s.Name + "(" + s.Course.Code + ")"
                                      };

                        courses = courses.OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                }

            };

            if (ddlAppType.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--Select One--", "0");
                ddlExamName.Items.Clear();
                ddlExamName.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            //ddlExamCycle.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            //ddlExamYear.SelectedValue = "0";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}