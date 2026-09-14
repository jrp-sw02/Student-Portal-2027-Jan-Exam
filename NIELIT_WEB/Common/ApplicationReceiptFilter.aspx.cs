using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using System.Data.Objects;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class HO_ApplicationReceiptFilter : BasePage
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
                Ddlinstitutes.Enabled = false;
                BindCourseCategory();
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlGender, typeof(EConnect.Gender), new ListItem("--All--", "0"));
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCastCategory, typeof(EConnect.enmCastCategory), new ListItem("--All--", "0"));
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                OccupationFill();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application Receipt Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }

    #region Private Methods--
    protected void bindinst()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                var institutes = from c in context.Institutes
                                 where c.ID == entityID
                                 select new
                                 {
                                     ValueField = c.ID,
                                     TextField = c.Name
                                 };
                if (institutes.Count() > 0)
                {
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddlinstitutes, institutes.Distinct(), lst);
                    Ddlinstitutes.SelectedValue = entityID.ToString();
                    Ddlinstitutes.Enabled = false;
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void binRegCentre()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                var RegCentre = from c in context.RegionalCenters
                                where c.ID == entityID
                                select new
                                {
                                    ValueField = c.ID,
                                    TextField = c.Name
                                };
                if (RegCentre.Count() > 0)
                {
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, RegCentre.Distinct(), lst);
                    ddlRegionalCentre.SelectedValue = entityID.ToString();
                    ddlRegionalCentre.Enabled = false;
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void OccupationFill()
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var Occ = from p in context.Occupations
                          select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlOccupation, Occ, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
                                  select new { ValueField = s.ID, TextField = s.Name  };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);
                }
                else
                {
                    var courses = from s in context.CourseCategories
                                  select new { ValueField = s.ID, TextField = s.Name  };


                    if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                        courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses, lst);
                }
                if (loginUserType == UserType.Institute)
                {
                    //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                    ddlapplicationtype.SelectedValue = "2";
                    ddlapplicationtype.Enabled = false;
                    bindinst();
                }
                else if (loginUserType == UserType.RegionalCenter)
                {
                    binRegCentre();
                    ddlRegionalCentre.SelectedValue = entityID.ToString();
                    ddlRegionalCentre.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void BindInstitutes(Int32 ApplicationTypeID, Int32 courseID, Int32 examid)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {

                    var institutes = from c in context.CourseExamApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     where c.ExamID == examid && a.CourseID == courseID
                                     select new
                                     {
                                         ValueField = i.ID,
                                         TextField = i.Name + "(" + a.AccreditationNumber + ")"
                                     };
                    if (institutes.Count() > 0)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(Ddlinstitutes, institutes.Distinct(), lst);
                    }
                    else
                    {
                        ddlapplicationtype.SelectedValue = "1";
                        ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                    }
                }
                else if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {

                    var institutes = from c in context.CourseRegistrationApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     where a.CourseID == courseID && c.ApplicableExamID == examid
                                     select new
                                     {
                                         ValueField = i.ID,
                                         TextField = i.Name + "(" + a.AccreditationNumber + ")"
                                     };
                    if (institutes.Count() > 0)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(Ddlinstitutes, institutes.Distinct(), lst);
                    }
                    else
                    {
                        ddlapplicationtype.SelectedValue = "1";
                        ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                    }
                }
                else if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var institutes = from c in context.CertificateExamApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     where a.CourseID == courseID && c.ExamID == examid
                                     select new
                                     {
                                         ValueField = i.ID,
                                         TextField = i.Name + "(" + a.AccreditationNumber + ")"
                                     };

                    if (institutes.Count() > 0)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(Ddlinstitutes, institutes.Distinct(), lst);
                    }
                    else
                    {
                        ddlapplicationtype.SelectedValue = "1";
                        ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                    }
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    #endregion

    #region Events--
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
                    //var courses = from s in context.Courses
                    //              where s.CourseCategoryID == courseCatId
                    //               && s.ShowOnWeb
                    //              orderby s.Name
                    //              //Modified 19 Feb 2019
                    //              select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )" };
                    //Added by Deep on 12 May 2022 for Short term course(6) view all courses start
                    if (courseCatId == 6)
                    {
                        ddlCourseName.BackColor =System.Drawing.Color.LightYellow;
                        var courses = from s in context.Courses
                                      where s.CourseCategoryID == courseCatId                                     
                                      orderby s.Name
                                      select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )" };

                        if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                        {
                            var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                            courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                        }
                        courses = courses.Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                        if (loginUserType == UserType.Institute)
                        {
                            //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                            ddlapplicationtype.SelectedValue = "2";
                            ddlapplicationtype.Enabled = false;
                            bindinst();
                        }

                    }
                    else
                    {
                        ddlCourseName.BackColor = System.Drawing.Color.White;
                        var courses = from s in context.Courses
                                      where s.CourseCategoryID == courseCatId
                                       && s.ShowOnWeb
                                      orderby s.Name
                                      select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )" };

                        if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                        {
                            var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                            courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                        }
                        courses = courses.Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                        if (loginUserType == UserType.Institute)
                        {
                            //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                            ddlapplicationtype.SelectedValue = "2";
                            ddlapplicationtype.Enabled = false;
                            bindinst();
                        }
                    }

                    //Added by Deep on 12 May 2022 for Short term course(6) view all courses End
                    
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
                ddlExamCentre1.Items.Clear();
                ddlExamCentre1.Items.Add(lst1);
                ddlExamCentre2.Items.Clear();
                ddlExamCentre2.Items.Add(lst1);
                ddlCandidateState.Items.Clear();
                ddlCandidateState.Items.Add(lst1);
                if (loginUserType != UserType.RegionalCenter)
                {
                    ddlRegionalCentre.Items.Clear();
                    ddlRegionalCentre.Items.Add(lst1);
                }
                ddlGender.SelectedValue = "0";
                ddlCastCategory.SelectedValue = "0";
                ddlOccupation.SelectedValue = "0";
                ddlExamState1.Items.Clear();
                ddlExamState1.Items.Add(lst1);
                ddlExamState2.Items.Clear();
                ddlExamState2.Items.Add(lst1);
                if (loginUserType != UserType.Institute)
                {
                    ddlapplicationtype.SelectedValue = "0";
                    ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                }
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
                    var AppStatus = from p in context.ApplicationStatusMappings
                                    where p.ApplicationTypeID == AppTypeId
                                    orderby p.ApplicationStatusID
                                    select new
                                    {
                                        ValueField = p.ApplicationStatus.ID,
                                        TextField = p.ApplicationStatus.Name,
                                        p.IsRelatedToHeadOffice,
                                        p.IsRelatedToRegionalCentre,
                                        p.IsRelatedToAccreditedCentre
                                    };
                    if (loginUserType == UserType.HeadOffice)
                        AppStatus = AppStatus.Where(p => p.IsRelatedToHeadOffice == true);
                    else if (loginUserType == UserType.RegionalCenter)
                        AppStatus = AppStatus.Where(p => p.IsRelatedToRegionalCentre == true);
                    else if (loginUserType == UserType.Institute)
                        AppStatus = AppStatus.Where(p => p.IsRelatedToAccreditedCentre == true);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlStatus, AppStatus, lst);


                    //Added new status to break fee paid by institute status into 2 status and assigned value by default to 100
                    ListItem lsinst = new ListItem("Fee Pending to be Paid by Institute", "100");
                    ddlStatus.Items.Add(lsinst);


                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }
                }
                else
                {
                    ddlStatus.Items.Clear();
                    ddlStatus.Items.Insert(0, lst);
                    ddlExamCycle.Items.Clear();
                    ddlExamCycle.Items.Insert(0, lst2);
                }
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                if (ddlAppType.SelectedValue == "0")
                {
                    ListItem lst12 = new ListItem("--All--", "0");
                    ddlExamCentre1.Items.Clear();
                    ddlExamCentre1.Items.Add(lst12);
                    ddlExamCentre2.Items.Clear();
                    ddlExamCentre2.Items.Add(lst12);
                    ddlCandidateState.Items.Clear();
                    ddlCandidateState.Items.Add(lst12);
                    if (loginUserType != UserType.RegionalCenter)
                    {
                        ddlRegionalCentre.Items.Clear();
                        ddlRegionalCentre.Items.Add(lst12);
                    }
                    ddlGender.SelectedValue = "0";
                    ddlCastCategory.SelectedValue = "0";
                    ddlOccupation.SelectedValue = "0";
                    ddlExamState1.Items.Clear();
                    ddlExamState1.Items.Add(lst12);
                    ddlExamState2.Items.Clear();
                    ddlExamState2.Items.Add(lst12);
                    if (loginUserType != UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "0";
                        ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                    }
                }

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
                    if (loginUserType == UserType.Institute)
                    {
                        //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
                ddlAppType.SelectedValue = "0";
                ddlAppType_SelectedIndexChanged(ddlapplicationtype, EventArgs.Empty);
            };
            if (ddlCourseName.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlExamCentre1.Items.Clear();
                ddlExamCentre1.Items.Add(lst1);
                ddlExamCentre2.Items.Clear();
                ddlExamCentre2.Items.Add(lst1);
                ddlCandidateState.Items.Clear();
                ddlCandidateState.Items.Add(lst1);
                if (loginUserType != UserType.RegionalCenter)
                {
                    ddlRegionalCentre.Items.Clear();
                    ddlRegionalCentre.Items.Add(lst1);
                }
                ddlGender.SelectedValue = "0";
                ddlCastCategory.SelectedValue = "0";
                ddlOccupation.SelectedValue = "0";
                ddlExamState1.Items.Clear();
                ddlExamState1.Items.Add(lst1);
                ddlExamState2.Items.Clear();
                ddlExamState2.Items.Add(lst1);
                if (loginUserType != UserType.Institute)
                {
                    ddlapplicationtype.SelectedValue = "0";
                    ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                }
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
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);

                            if (loginUserType == UserType.Institute)
                            {
                                ddlapplicationtype.SelectedValue = "2";
                                ddlapplicationtype.Enabled = false;
                                bindinst();
                            }

                        }
                        else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            var courses = (from s in context.Exams
                                           join c in context.CourseExamApplications
                                               on s.ID equals c.ExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                            if (loginUserType == UserType.Institute)
                            {
                                ddlapplicationtype.SelectedValue = "2";
                                ddlapplicationtype.Enabled = false;
                                bindinst();
                            }

                        }
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {
                        if (loginUserType == UserType.RegionalCenter)
                        {
                            var courses = (from s in context.Exams
                                           //join c in context.CertificateExamApplications
                                           //on s.ID equals c.ExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                           //&& c.FinalSubmitted == true && c.RegionalCenterID == entityID
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        }
                        else
                        {

                            var courses = (from s in context.Exams
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        }

                        if (loginUserType == UserType.Institute)
                        {
                            ddlapplicationtype.SelectedValue = "2";
                            ddlapplicationtype.Enabled = false;
                            bindinst();
                        }


                    }
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                }
                ddlExamName.SelectedValue = "0";
                ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
            }
            if (ddlExamCycle.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlExamCentre1.Items.Clear();
                ddlExamCentre1.Items.Add(lst1);
                ddlExamCentre2.Items.Clear();
                ddlExamCentre2.Items.Add(lst1);
                ddlCandidateState.Items.Clear();
                ddlCandidateState.Items.Add(lst1);
                if (loginUserType != UserType.RegionalCenter)
                {
                    ddlRegionalCentre.Items.Clear();
                    ddlRegionalCentre.Items.Add(lst1);
                }
                ddlGender.SelectedValue = "0";
                ddlCastCategory.SelectedValue = "0";
                ddlOccupation.SelectedValue = "0";
                ddlExamState1.Items.Clear();
                ddlExamState1.Items.Add(lst1);
                ddlExamState2.Items.Clear();
                ddlExamState2.Items.Add(lst1);
                if (loginUserType != UserType.Institute)
                {
                    ddlapplicationtype.SelectedValue = "0";
                    ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
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
            ddlStatus.SelectedValue = "0";
            ddlapplicationtype.SelectedValue = "0";
            //trinstitute.Visible = false;
            // trinstitute1.Visible = false;
            Ddlinstitutes.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlapplicationtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ListItem lst1 = new ListItem("--All--", "0");
            if (Convert.ToInt32(ddlapplicationtype.SelectedValue) == Convert.ToInt32(enmApplicantType.Direct))
            {
                Ddlinstitutes.Enabled = false;
                Ddlinstitutes.Items.Clear();
                Ddlinstitutes.Items.Add(lst1);
            }
            else if (Convert.ToInt32(ddlapplicationtype.SelectedValue) == Convert.ToInt32(enmApplicantType.Institute))
            {
                Ddlinstitutes.Enabled = true;
                Int32 ApplicationTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 examid = Convert.ToInt32(ddlExamName.SelectedValue);
                BindInstitutes(ApplicationTypeID, courseID, examid);
            }
            else
            {
                Ddlinstitutes.Items.Clear();
                Ddlinstitutes.Items.Add(lst1);
                Ddlinstitutes.Enabled = false;
            }

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
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 stid = Convert.ToInt32(enmLocationType.State);
            using (EConnectContext context = new EConnectContext())
            {
                if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    var capp = from cr in context.CourseRegistrationApplications
                               join lo in context.Locations on cr.CorStateID equals lo.ID
                               where cr.ApplicableExamID == ExamID && cr.CourseID == CourseID && lo.LocationTypeID == stid && cr.FinalSubmitted == true
                               orderby lo.Name
                               select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst = new ListItem("--All--", "0");

                    ddlExamCentre1.Enabled = false;
                    ddlExamCentre2.Enabled = false;
                    ddlExamState1.Enabled = false;
                    ddlExamState2.Enabled = false;
                    ddlRegionalCentre.Enabled = false;
                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCandidateState, capp.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst);
                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    Int32 correspondenceAddress = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    var capp = from cr in context.CourseExamApplications
                               join ad in context.Addresses
                                   on cr.CandidateID equals ad.CandidateID
                               join lo in context.Locations on ad.StateID equals lo.ID
                               where cr.ExamID == ExamID && cr.CourseID == CourseID && lo.LocationTypeID == stid && cr.FinalSubmitted == true
                               && ad.AddressTypeID == correspondenceAddress
                               orderby lo.Name
                               select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst = new ListItem("--All--", "0");

                    ddlExamCentre1.Enabled = true;
                    ddlExamCentre2.Enabled = true;
                    ddlExamState1.Enabled = true;
                    ddlExamState2.Enabled = true;
                    ddlRegionalCentre.Enabled = false;
                    var capp1 = from cr in context.CourseExamApplications
                                join lo in context.ExamCenters on cr.ExamCenter1ID equals lo.ID
                                orderby lo.Name
                                where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst1 = new ListItem("--All--", "0");

                    var capp2 = from cr in context.CourseExamApplications
                                join lo in context.ExamCenters on cr.ExamCenter2ID equals lo.ID
                                orderby lo.Name
                                where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst2 = new ListItem("--All--", "0");

                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCandidateState, capp.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCentre1, capp1.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst1);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCentre2, capp2.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst2);
                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var capp = from cr in context.CertificateExamApplications
                               join lo in context.Locations on cr.CorStateID equals lo.ID
                               where cr.ExamID == ExamID && cr.CourseID == CourseID && lo.LocationTypeID == stid && cr.FinalSubmitted == true
                               orderby lo.Name
                               select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst = new ListItem("--All--", "0");

                    ddlExamCentre1.Enabled = true;
                    ddlExamCentre2.Enabled = true;
                    ddlExamState1.Enabled = true;
                    ddlExamState2.Enabled = true;
                    ddlRegionalCentre.Enabled = true;
                    var capp1 = from cr in context.CertificateExamApplications
                                join lo in context.ExamCenters on cr.ExamCenter1ID equals lo.ID
                                orderby lo.Name
                                where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst1 = new ListItem("--All--", "0");

                    var capp2 = from cr in context.CertificateExamApplications
                                join lo in context.ExamCenters on cr.ExamCenter2ID equals lo.ID
                                orderby lo.Name
                                where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst2 = new ListItem("--All--", "0");

                    var RegCentre = from cr in context.CertificateExamApplications
                                    join lo in context.RegionalCenters on cr.RegionalCenterID equals lo.ID
                                    where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                    orderby lo.Name
                                    select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst3 = new ListItem("--All--", "0");
                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }
                    else if (loginUserType == UserType.RegionalCenter)
                    {
                        binRegCentre();
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCandidateState, capp.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCentre1, capp1.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst1);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCentre2, capp2.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst2);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, RegCentre.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst3);
                }
            };
            ddlExamCentre1.SelectedValue = "0";
            ddlExamCentre1_SelectedIndexChanged(ddlExamCentre1, EventArgs.Empty);
            ddlExamCentre2.SelectedValue = "0";
            ddlExamCentre2_SelectedIndexChanged(ddlExamCentre2, EventArgs.Empty);
            ddlCandidateState.SelectedValue = "0";
            if (loginUserType != UserType.Institute)
            {
                ddlapplicationtype.SelectedValue = "0";
                ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCentre1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 ExamCentreID1 = Convert.ToInt32(ddlExamCentre1.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {

                if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    var ExState1 = from cr in context.ExamCenters
                                   where cr.ID == ExamCentreID1
                                   select new { ValueField = cr.StateID, TextField = cr.State.Name };
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamState1, ExState1.Distinct(), lst);

                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var ExState1 = from cr in context.ExamCenters
                                   where cr.ID == ExamCentreID1
                                   select new { ValueField = cr.StateID, TextField = cr.State.Name };
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamState1, ExState1.Distinct(), lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCentre2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 ExamCentreID2 = Convert.ToInt32(ddlExamCentre2.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {

                if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    var ExState2 = from cr in context.ExamCenters
                                   where cr.ID == ExamCentreID2
                                   select new { ValueField = cr.StateID, TextField = cr.State.Name };
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamState2, ExState2.Distinct(), lst);

                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var ExState2 = from cr in context.ExamCenters
                                   where cr.ID == ExamCentreID2
                                   select new { ValueField = cr.StateID, TextField = cr.State.Name };
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamState2, ExState2.Distinct(), lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    } 
    #endregion

}