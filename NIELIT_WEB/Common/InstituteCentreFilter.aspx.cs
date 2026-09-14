using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public partial class Common_InstituteCentreFilter : BasePage
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
                Response.Redirect("~/Index.aspx");
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Institute Performance Report", "#", ""));
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
               
               
                //Added 9 jan 2019
                if (courseCatId == 2 && Rdsearchby.SelectedValue == "1") // Consolidated report for all courses in it literacy
                {
                    DataTable dt = new DataTable();
                    dt.Columns .Add ("--All--",typeof(string));
                    dt.Columns .Add ("-1",typeof(string));
                    dt.Rows.Add("-1", "--All--");
                    foreach (var item in courses)
                    {
                        dt.Rows.Add(item.ValueField, item.TextField);
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName,dt,lst);
                    //ListItem lst1 = new ListItem("--All--", "-1");
                    //ddlCourseName.Items.Add(lst1);
                }
                else
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
                if (Rdsearchby.SelectedValue == "0")
                {
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                }
                else if (Rdsearchby.SelectedValue != "0" && CourseId != 0)
                {
                    if ((AppTypeId == 0 || AppTypeId == 1) && CourseId != -1)
                    { btnReset_Click(sender, e); throw new Exception("Date Wise report facility is not available for Course-Registration and Course-Exam Application"); }

                    //Added 11 Jan 2019

                    if (CourseId == -1)
                    {
                        ListItem lst2 = new ListItem("--Select One--", "0");
                        var institute = (from s in context.Institutes
                                         join a in context.AccreditationDetails on s.ID equals a.InstituteID
                                         //join c in context.CertificateExamApplications on a.InstituteID equals c.InstituteID
                                         where a.CourseCategoryID == 2
                                         //&& c.CourseID == CourseId
                                         orderby a.InstituteID
                                         select new { ValueField = s.ID, TextField = a.AccreditationNumber + " - " + s.Name + "(" + s.CityName + ")" });
                        //, RegionalCenterID = c.RegionalCenterID
                        institute = institute.Distinct();

                        //if (loginUserType == UserType.RegionalCenter)
                        //    institute = institute.Where(s => s.RegionalCenterID == entityID);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, institute.Distinct(), lst2);
                    }
                    else
                    {
                        ListItem lst2 = new ListItem("--Select One--", "0");
                        var institute = (from s in context.Institutes
                                         join a in context.AccreditationDetails on s.ID equals a.InstituteID
                                         //join c in context.CertificateExamApplications on a.InstituteID equals c.InstituteID
                                         where a.CourseID == CourseId
                                         //&& c.CourseID == CourseId
                                         orderby a.InstituteID
                                         select new { ValueField = s.ID, TextField = a.AccreditationNumber + " - " + s.Name + "(" + s.CityName + ")" });
                        //, RegionalCenterID = c.RegionalCenterID


                        //if (loginUserType == UserType.RegionalCenter)
                        //    institute = institute.Where(s => s.RegionalCenterID == entityID);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, institute.Distinct(), lst2);
                    }
                }
                //else
                //{
                //    ddlExamCycle.SelectedValue = "0";
                //    ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                //}
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
                //For all courses Added 10 Jan 2019
                if (cid == -1)
                {
                    Int32 ccId = Convert.ToInt32(ddlCourseCategry.SelectedValue);

                    var ApplicationList = (from p in context.ApplicationTypes
                                          join q in context.Courses on p.CourseTypeID equals q.CourseTypeID
                                          where q.CourseCategoryID == ccId
                                          select new { ValueField = p.ID, TextField = p.Name }).Distinct ();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                    
                    if (Rdsearchby.SelectedValue != "0") // Date Wise
                    { BindInstituteLIst(); }
                }

                else
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
                        ddlAppType.SelectedValue = "0";
                        ddlAppType_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                    }
                    if (Rdsearchby.SelectedValue != "0") // Date Wise
                    { BindInstituteLIst(); }
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
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            Int32 currentyear = Convert.ToInt32(DateTime.Now.Year) - 2;
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
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
                                       p.DateOfPublishingOfResult <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && p.ExaminationCycleID == ExamCycleId
                                       orderby p.ExamYear descending, p.ExamMonth descending
                                       select new { ValueField = p.ID, TextField = p.Name };

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examname, lst);
                    }
                    else
                    {
                        ddlExamName.SelectedValue = "0";
                        ddlExamName_SelectedIndexChanged(ddlinstitutename, EventArgs.Empty);
                    }
                }
                else
                {
                    ddlExamName.SelectedValue = "0";
                    ddlExamName_SelectedIndexChanged(ddlinstitutename, EventArgs.Empty);
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
            ddlinstitutename.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int examid = Convert.ToInt32(ddlExamName.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0 && examid > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                    {
                        Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                        if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {

                            var institute = (from s in context.Institutes
                                             join a in context.AccreditationDetails on s.ID equals a.InstituteID
                                             join c in context.CourseExamApplicationDetails
                                                 on s.ID equals c.InstituteID
                                             where c.CourseID == courseId && c.ExamID == examid && a.CourseID == c.CourseID
                                             select new { ValueField = s.ID, TextField = a.AccreditationNumber + " - " + s.Name + "(" + s.AddressLine1 + "," + s.CityName + ")" });
                            institute = institute.Distinct();
                            institute = institute.OrderBy(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, institute, lst);
                        }
                        //Added 16 Jan 2019
                        if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication ))
                        {

                            var institute = (from s in context.Institutes
                                             join a in context.AccreditationDetails on s.ID equals a.InstituteID
                                             where a.CourseID ==courseId  
                                             select new { ValueField = s.ID, TextField = a.AccreditationNumber + " - " + s.Name + "(" + s.AddressLine1 + "," + s.CityName + ")" });
                            institute = institute.Distinct();
                            institute = institute.OrderBy(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, institute, lst);
                        }
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {

                        var institute = (from s in context.Institutes
                                         join a in context.AccreditationDetails on s.ID equals a.InstituteID
                                         join c in context.CertificateExamApplications on a.InstituteID equals c.InstituteID
                                         where a.CourseID == courseId  && c.ExamID == examid && c.ResultGradeID != null
                                         orderby c.InstituteID
                                         select new { ValueField = s.ID, TextField = a.AccreditationNumber + " - " + s.Name + "(" + s.CityName + ")", RegionalCenterID = c.RegionalCenterID });

                        if (loginUserType == UserType.RegionalCenter)
                            institute = institute.Where(s => s.RegionalCenterID == entityID);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, institute.Distinct(), lst);

                    }
                    if (loginUserType == UserType.Institute)
                    {
                        ddlinstitutename.SelectedValue = entityID.ToString();
                        ddlinstitutename.Enabled = false;
                    }
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlinstitutename.Items.Clear();
                    ddlinstitutename.Items.Insert(0, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Rdsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlinstitutename.SelectedValue = "0";
            if (Rdsearchby.SelectedValue != "0") //Date Wise
            {
                DateWiseTable.Visible = true;
                ExamWiseTable.Visible = false;
            }
            else // Exam Wise
            {
                ExamWiseTable.Visible = true;
                DateWiseTable.Visible = false;
            }            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            if (Isvalid())
            {
                var instiituteId = ddlinstitutename.SelectedValue;
                var CourseId = ddlCourseName.SelectedValue;
                var TypeId = ddlAppType.SelectedValue;
                int ExamId = 0;
                if (Rdsearchby.SelectedValue != "0")
                {
                    var DateFrom = txtDateFrom.Text.ToString();
                    var DateTo = txtDateto.Text.ToString();
                    Response.Redirect("InstituteReport.aspx?instituteId=" + instiituteId + "&ExamId=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&CourseId=" + CourseId + "&TypeId=" + TypeId, false);
                }
                if (Rdsearchby.SelectedValue == "0")
                {
                    ExamId = Convert.ToInt32(ddlExamName.SelectedValue);
                    Response.Redirect("InstituteReport.aspx?instituteId=" + instiituteId + "&ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId, false);
                    //Response.Write("<script>window.open('InstituteReport.aspx?instituteId=" + instiituteId + "&ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId +"','_blank');</script>");

                    //Response.Write("<script>");
                    //Response.Write("window.open('InstituteReport.aspx?instituteId=" + instiituteId + "&ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId +" ','_blank')");
                    //Response.Write("</script>");
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool Isvalid()
    {
        try
        {
            if (Rdsearchby.SelectedValue != "0")
            {
                if (String.IsNullOrWhiteSpace(txtDateFrom.Text))
                    throw new Exception("Please enter from date");
                if (String.IsNullOrWhiteSpace(txtDateto.Text))
                    throw new Exception("Please enter to data");
                if (!IsDate(txtDateFrom.Text))
                    throw new Exception("Invalid From Date");
                if (!IsDate(txtDateto.Text))
                    throw new Exception("Invalid ToDate");
            }
            if (Rdsearchby.SelectedValue == "0")
            {
                if (ddlExamCycle.SelectedValue == "0")
                    throw new Exception("Please select Exam Cycle");
                if (ddlExamName.SelectedValue == "0")
                    throw new Exception("Please select Exam Name");
            }
            if (ddlinstitutename.SelectedValue == "0")
                throw new Exception("Please select an Institute");

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindInstituteLIst()
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            
            //Added for All courses in digital literacy 10 Jan 2019
            Int32 ccId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            if (courseId == -1)
            {
                ListItem lst = new ListItem("--Select One--", "0");
                using (EConnectContext context = new EConnectContext())
                {

                    var institute = context.Institutes.Join(context.AccreditationDetails.Where(s => s.CourseCategoryID == ccId),
                                                        I => I.ID, A => A.InstituteID,
                                                        (I, A) => new { I.ID, I.Name, I.AddressLine1, I.CityName, A.CourseID, A.AccreditationNumber  });

                    //var CertificateExamInstitute = institute.Join(context.CertificateExamApplications.Where(s => s.CourseID == courseId && s.ResultGradeID != null),
                    //                                       I => I.ID, C => C.InstituteID,
                    //                                       (I, C) => new { ValueField = I.ID, TextField = I.AccreditationNumber + " - " + I.Name + "(" + I.CityName + ")", RegionalCenterID = C.RegionalCenterID });
                    //Modified 10 Jan 2019

                    IEnumerable<int> inq = (from c in context.Courses
                              where c.CourseCategoryID == 2
                              select c.ID );

                   int[] x = inq.ToArray();

                     var CertificateExamInstitute = institute.Join(context.CertificateExamApplications.Where(s => x.Contains(s.CourseID) && s.ResultGradeID.HasValue),
                                                  I => I.ID, C => C.InstituteID,
                                                   (I, C) => new { ValueField = I.ID, TextField = I.AccreditationNumber + " - " + I.Name + "(" + I.CityName + ")", RegionalCenterID = C.RegionalCenterID });
                   
                    CertificateExamInstitute = CertificateExamInstitute.Distinct().OrderBy(s => s.ValueField);
                    if (loginUserType == UserType.RegionalCenter)
                    { CertificateExamInstitute = CertificateExamInstitute.Where(s => s.RegionalCenterID == entityID); }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, CertificateExamInstitute, lst);

                    if (loginUserType == UserType.Institute)
                    {
                        ddlinstitutename.SelectedValue = entityID.ToString();
                        ddlinstitutename.Enabled = false;
                    }


                };
            }
            else
            {
                //


                ListItem lst = new ListItem("--Select One--", "0");
                using (EConnectContext context = new EConnectContext())
                {
                    if (courseId > 0)
                    {
                        Course currentcourse = context.Courses.Find(courseId);
                        var institute = context.Institutes.Join(context.AccreditationDetails.Where(s => s.CourseID == courseId),
                                                            I => I.ID, A => A.InstituteID,
                                                            (I, A) => new { I.ID, I.Name, I.AddressLine1, I.CityName, A.CourseID, A.AccreditationNumber });
                        if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                        {
                            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                            if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                var CourseExamInstitute = institute.Join(context.CourseExamApplicationDetails.Where(s => s.CourseID == courseId),
                                                          I => I.ID, C => C.InstituteID,
                                                          (I, C) => new { ValueField = I.ID, TextField = I.AccreditationNumber + " - " + I.Name + "(" + I.AddressLine1 + "," + I.CityName + ")" });
                                CourseExamInstitute = CourseExamInstitute.Distinct().OrderBy(s => s.ValueField);
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, CourseExamInstitute, lst);
                            }
                        }
                        else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                        {
                            var CertificateExamInstitute = institute.Join(context.CertificateExamApplications.Where(s => s.CourseID == courseId && s.ResultGradeID != null),
                                                           I => I.ID, C => C.InstituteID,
                                                           (I, C) => new { ValueField = I.ID, TextField = I.AccreditationNumber + " - " + I.Name + "(" + I.CityName + ")", RegionalCenterID = C.RegionalCenterID });
                            CertificateExamInstitute = CertificateExamInstitute.Distinct().OrderBy(s => s.ValueField);
                            if (loginUserType == UserType.RegionalCenter)
                            { CertificateExamInstitute = CertificateExamInstitute.Where(s => s.RegionalCenterID == entityID); }
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitutename, CertificateExamInstitute, lst);
                        }
                        if (loginUserType == UserType.Institute)
                        {
                            ddlinstitutename.SelectedValue = entityID.ToString();
                            ddlinstitutename.Enabled = false;
                        }
                    }
                    else
                    {
                        ddlExamName.Items.Clear();
                        ddlExamName.Items.Insert(0, lst);
                        ddlinstitutename.Items.Clear();
                        ddlinstitutename.Items.Insert(0, lst);
                    }
                };
            }
        }
        catch (Exception ex){ ShowAlert(ex.Message, true);}
    }
}