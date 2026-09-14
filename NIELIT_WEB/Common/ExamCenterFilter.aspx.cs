using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;

public partial class ExamCenterFilter : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 applicationTypeCourseRegistration = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
    Int32 applicationTypeCourseExam = Convert.ToInt32(enmApplicationType.CourseExamApplication);
    Int32 applicationTypeCertificateExam = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
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
            hf1.Value = Convert.ToInt32(enmApplicationType.CourseExamApplication).ToString();
            if (!IsPostBack)
            {
                BindCourseCategory();
                //FillState();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Centre Report", "#", ""));
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
                var courses = from s in context.CourseCategories
                              select new { ValueField = s.ID, TextField = s.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

            }
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
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);

            if (ddlCourseCategry.SelectedValue == "0")
            {
                ListItem lst = new ListItem("--All--", "0");
                ListItem lst1 = new ListItem("--Select One--", "0");
                ddlState.Items.Clear();
                ddlState.Items.Add(lst1);
                ddlExamCenter.Items.Clear();
                ddlExamCenter.Items.Add(lst1);
                ddlExamVenue.Items.Clear();
                ddlExamVenue.Items.Add(lst);
                lblExamVenue.Visible = false;
                ddlExamVenue.Visible = false;
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
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                               && p.ID != applicationTypeCourseRegistration
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            if (ddlCourseName.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ListItem lst2 = new ListItem("--Select One--", "0");
                ddlState.Items.Clear();
                ddlState.Items.Add(lst2);
                ddlExamCenter.Items.Clear();
                ddlExamCenter.Items.Add(lst2);
                ddlExamVenue.Items.Clear();
                ddlExamVenue.Items.Add(lst1);
                lblExamVenue.Visible = false;
                ddlExamVenue.Visible = false;
            }
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

                    if (Convert.ToInt32(ddlAppType.SelectedValue) == applicationTypeCourseExam)
                    {
                        lblExamVenue.Visible = false;
                        ddlExamVenue.Visible = false;
                    }
                    else
                    {
                        lblExamVenue.Visible = false;
                        ddlExamVenue.Visible = false;
                    }
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
            int applicationTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0 && ExamCycleId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    if (applicationTypeId == applicationTypeCertificateExam)
                    {
                        var courses = (from s in context.Exams                                       
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                       select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                        courses = courses.OrderByDescending(s => s.ValueField);
                        courses = courses.OrderByDescending(s => s.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                        ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                    }
                    else if (applicationTypeId == applicationTypeCourseExam)
                    {
                        var courses = (from s in context.Exams
                                       join c in context.CourseExamApplications
                                           on s.ID equals c.ExamID
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                       select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                        ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                    }
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
            int applicationTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0 && examYear > 0 && ExamCycleId > 0)
                {
                    if (applicationTypeId == applicationTypeCertificateExam)
                    {
                        var courses = (from s in context.Exams                                       
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                       && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                        courses = courses.OrderByDescending(s => s.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                    else if (applicationTypeId == applicationTypeCourseExam)
                    {
                        var courses = (from s in context.Exams
                                       join c in context.CourseExamApplications
                                           on s.ID equals c.ExamID
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int stateId = Convert.ToInt32(ddlState.SelectedValue);
            //ddlDistrict.Items.Clear();
            //fillDistrict(id2);
            fillExamCenters(stateId);
            if (ddlState.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--Select One--", "0");
                ddlExamCenter.Items.Clear();
                ddlExamCenter.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void fillExamCenters(int stateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 couCatID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                ListItem lstAll = new ListItem("--All--", "99999999");
                if (stateID != 0 && couCatID != 0 && courseID != 0 && examID != 0)
                {
                    if (couCatID == 2)
                    {
                        //var examCenters = from s in context.ExamCenters
                        //                  where s.CourseCategoryID == couCatID
                        //                  && s.StateID == stateID && s.IsEnabled == true
                        //                  orderby s.Name
                        //                  select new { ValueField = s.ID, TextField = s.Name };
                        //EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCenter, examCenters, lst);


                        //var examCenters = from s in context.ExamCenters
                        //                  where s.CourseCategoryID == couCatID
                        //                  && s.StateID == stateID && s.IsEnabled == true
                        //                  orderby s.Name
                        //                  select new { ValueField = s.ID, TextField = s.Name };
                        //EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCenter, examCenters, lst);

                        var examCenters = from p in context.CertificateExamApplications
                                        join q in context.ExamCenters on p.ExamCentreName.Trim().Substring(0,3).Trim() equals q.Code.Trim()
                                        where p.CourseID == courseID
                                        && q.CourseCategoryID == couCatID
                                        && q.StateID == stateID
                                        && p.ExamID == examID
                                        select new { ValueField = q.ID, TextField = q.Name };

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCenter, examCenters.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lstAll);


                    }
                    else if (couCatID == 1)
                    {
                        var examCenters = from s in context.ExamCenters
                                          join t in context.ExamWiseExamCenters
                                          on s.ID equals t.ExamCenterID
                                          where s.CourseCategoryID == couCatID
                                          && s.StateID == stateID && t.ExamID == examID
                                          select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCenter, examCenters, lst);
                    }
                }
            };
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
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlState.SelectedValue = "0";
            ddlExamCenter.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int exmCenterId = Convert.ToInt32(ddlExamCenter.SelectedValue);
            //ddlDistrict.Items.Clear();
            //fillDistrict(id2);
            fillExamCenterVenue(exmCenterId);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void fillExamCenterVenue(int exmCenterId)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 stateID = Convert.ToInt32(ddlState.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                if (exmCenterId != 0)
                {
                    var examVenue = (from s in context.ExamVenues
                                     orderby (s.Name)
                                     where s.ExamCentreID == exmCenterId
                                           && s.StateID == stateID
                                     select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamVenue, examVenue, lst);
                }
            };
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
            using (EConnectContext context = new EConnectContext())
            {
                Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 stid = Convert.ToInt32(enmLocationType.State);
                ListItem lst = new ListItem("--Select One--", "0");
                if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {

                    var fillState = from cr in context.CourseExamApplications
                                    join lo in context.ExamCenters on cr.AllottedExamCentreID equals lo.ID
                                    orderby lo.State.Name
                                    where cr.ExamID == ExamID && cr.CourseID == CourseID
                                    select new { ValueField = lo.StateID, TextField = lo.State.Name };



                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, fillState.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst);
                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var fillState = from cr in context.CertificateExamApplications                                          
                                    join lo in context.ExamCenters on cr.ExamCentreName.ToUpper().Trim() equals lo.Code.ToUpper().Trim()
                                    orderby lo.State.Name
                                    where cr.ExamID == ExamID && cr.CourseID == CourseID || cr.ExamCentreName.Contains(lo.Code.ToUpper().Trim())
                                    select new { ValueField = lo.StateID, TextField = lo.State.Name };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, fillState.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst);
                }
            };
            ddlState.SelectedValue = "0";
            ddlState_SelectedIndexChanged(ddlState.SelectedValue, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}