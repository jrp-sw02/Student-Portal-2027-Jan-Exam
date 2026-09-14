using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;
using Ionic.Zip;
using System.Data.Objects;
using System.Text;
using System.Web.UI;


public partial class downloadImagesAdmin : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        lblNoRecord.Text = "";
        lblNoRecord.Visible = false;
        txtRegnoFrom.Attributes.Add("onkeyup", "OnTextKeyUp(this);");   
        //txtRegnoTo.Attributes.Add("onfocusout", "isCheckInputNumber(this);");
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
            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
                if (!IsPostBack)
                {
                    Ddlinstitutes.Enabled = false;
                    BindCourseCategory();
                    FillCourseType();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Candidate Photographs", "#", ""));
                    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--Both--", "0"));
                    lblNoRecord.Text = "";
                    lblNoRecord.Visible = false;
                }
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Candidate Photographs", "#", ""));
                btnView.Visible = false;
                btnReset.Visible = false;
                Lblerror.Text = "You can not download candidate photographs.";
                Lblerror.Visible = true;
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
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, CourseList, lst);
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void FillCourseType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.Course.CourseTypeID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
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
    protected void ddlctype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            //Int32 coursetypeID = Convert.ToInt32(ddlctype.SelectedValue);           
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
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseCatId > 0)
                {
                    var courses = from s in context.Courses
                                  where s.CourseCategoryID == courseCatId
                                  select new { ValueField = s.ID, TextField = s.Name +" ("+s.Code +")" };

                    if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
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
            lblNoRecord.Text = "";
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID && ( p.Code == "LR" || p.Code == "CE" || p.Code == "LE") // add code comment by deep on 20 Nov 2019
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
            lblNoRecord.Text = "";
            ddlNameFormat.Enabled = true;
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
                if (AppTypeId == 1)
                {
                    //List<string[]> l = new List<string[]> { new string[] { "1", "Application Number" }, new string[] { "3", "Registration Number" } }; (Org)
                    List<string[]> l = new List<string[]> { new string[] { "3", "Registration Number" }, new string[] { "1", "Application Number" }, new string[] { "2", "Roll Number" } };
                    var listitems = (from obj in l
                                     select new
                                     {
                                         ValueField = obj[0],
                                         TextField = obj[1]
                                     }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlNameFormat, listitems, lst);

                }
                else if (AppTypeId == 2)
                {
                    List<string[]> l = new List<string[]> { new string[] { "1", "Application Number" }, new string[] { "2", "Roll Number" } };

                    var listitems = (from obj in l
                                     select new
                                     {
                                         ValueField = obj[0],
                                         TextField = obj[1]
                                     }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlNameFormat, listitems, lst);

                }
                else if (AppTypeId == 3)
                {
                     //List<string[]> l = new List<string[]> { new string[] { "1", "Application Number" }, new string[] { "3", "Registration Number" } }; (Org)
                    List<string[]> l = new List<string[]> { new string[] { "3", "Registration Number" }, new string[] { "1", "Application Number" }, new string[] { "2", "Roll Number" } };
                    var listitems = (from obj in l
                                     select new
                                     {
                                         ValueField = obj[0],
                                         TextField = obj[1]
                                     }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlNameFormat, listitems, lst);

                }
                else
                {
                    ddlNameFormat.Items.Clear();
                    ddlNameFormat.Items.Insert(0, lst2);
                }
                ddlNameFormat.SelectedValue = "0";
                ddlNameFormat_SelectedIndexChanged(ddlNameFormat, EventArgs.Empty);

                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                Label7.Text = "";
                Label10.Text = "";

               
            };
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
            lblNoRecord.Text = "";
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

                    if (examid == 0)
                    {
                         institutes = from c in context.CourseRegistrationApplications
                                         join i in context.Institutes
                                             on c.InstituteID equals i.ID
                                         join a in context.AccreditationDetails
                                             on i.ID equals a.InstituteID
                                         where a.CourseID == courseID 
                                         select new
                                         {
                                             ValueField = i.ID,
                                             TextField = i.Name + "(" + a.AccreditationNumber + ")"
                                         };
    
                    }
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
  
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            //ddlNameFormat.Enabled = true;
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
                                           select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();                                      
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);

                            //List<string[]> l = new List<string[]> { new string[] { "1", "Application Number" }, new string[] { "3", "Registration Number" } };

                            //var listitems = (from obj in l
                            //                 select new
                            //                 {
                            //                     ValueField = obj[0],
                            //                     TextField = obj[1]
                            //                 }).Distinct();
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlNameFormat, listitems, lst);
                        }
                        else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication)) // add code comment by deep on 20 Nov 2019
                        {
                            var courses = (from s in context.Exams
                                           join c in context.CourseExamApplications
                                               on s.ID equals c.ExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                           select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                        }// add code END comment by deep on 20 Nov 2019
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {                      
                            var courses = (from s in context.Exams
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                           select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);

                            //List<string[]> l = new List<string[]> { new string[] { "1", "Application Number" }, new string[] { "2", "Roll Number" } };

                            //var listitems = (from obj in l
                            //                 select new
                            //                 {
                            //                     ValueField = obj[0],
                            //                     TextField = obj[1]
                            //                 }).Distinct();
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlNameFormat, listitems, lst);
                    }
                }
                else
                {
                    ddlExamYear.Items.Clear();
                    ddlExamYear.Items.Insert(0, lst);
                }
                ddlExamYear.SelectedValue = "0";
                ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
            }          

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlNameFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            int selectionRecord = Convert.ToInt32(ddlNameFormat.SelectedValue);
            if (selectionRecord == 1)
            {
                Label7.Text = "Application Number From";
                Label10.Text = "Application Number To";
                txtRegnoFrom.Enabled = true;
                txtRegnoTo.Enabled = true;
            }
            else if (selectionRecord == 2)
            {
                Label7.Text = "Roll Number From";
                Label10.Text = "Roll Number To";
                txtRegnoFrom.Enabled = true;
                txtRegnoTo.Enabled = true;
            }
            else if (selectionRecord == 3)
            {
                Label7.Text = "Registration Number From";
                Label10.Text = "Registration Number To";
                txtRegnoFrom.Enabled = true;
                txtRegnoTo.Enabled = true;
            }
            else 
            {
                Label7.Text = "";
                Label10.Text = "";
                txtRegnoFrom.Text = "";
                txtRegnoTo.Text = "";
                txtRegnoFrom.Enabled = false;
                txtRegnoTo.Enabled = false;
                txtRegnoFrom.Text = "";
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    //protected void txtRegnoFrom_TextChanged(object sender, EventArgs e)
    //{
    //   String RangetxtFrom=txtRegnoFrom.Text;
    //    String RangetxtTo=txtRegnoTo.Text;

    //    if (String.IsNullOrEmpty(RangetxtTo))
    //    {
    //        txtRegnoTo.Text = RangetxtFrom;
    //    }


    //}
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
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
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                               select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                                             courses = courses.OrderByDescending(s => s.ValueField);
                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                                      
                        }
                        else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            var courses = (from s in context.Exams
                                           join c in context.CourseExamApplications
                                               on s.ID equals c.ExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                            courses = courses.OrderByDescending(s => s.ValueField);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);

                        }
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {
                        var courses = (from s in context.Exams
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                        courses = courses.OrderByDescending(s => s.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    }
                }        
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                   ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
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
            ddlExamYear.SelectedValue = "0";
             Ddlinstitutes.SelectedValue = "0";
             ddlNameFormat.SelectedValue = "0";
             Label7.Text = "";
             Label10.Text = "";
             txtRegnoFrom.Text = "";
             txtRegnoTo.Text = "";
             lblNoRecord.Text = "";
            //chkbatchlist.Items.Clear();
            //chkbatchlist.DataBind();
            divbatches.Style.Remove("overflow");
            divbatches.Style.Remove("width");
            divbatches.Style.Remove("height");        
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
            BreadCrumb1.Render(); 
            ddlapplicationtype.SelectedIndex = 0;            
            ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype, EventArgs.Empty);
            ddlNameFormat.SelectedIndex = 0;
            txtRegnoFrom.Enabled = false;
            txtRegnoTo.Enabled = false;
            lblNoRecord.Text = "";
           // ddlNameFormat.Enabled = false;           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
  
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
            List<Int32> Batches = new List<Int32>();
            //foreach (ListItem chk in chkbatchlist.Items)
            //{
            //    if (chk.Selected)
            //        Batches.Add(Convert.ToInt32(chk.Value));
            //}
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var applCount = (from p in context.BatchItems
                                 where Batches.Contains(p.BatchID)
                                 select p).Count();               
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {           
            Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
            
            using (EConnectContext context = new EConnectContext())
            {
                var photoCount = 0;
                lblNoRecord.Text = "";
                Int32 applTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue); 
                Int32 CandidateTypeID=Convert.ToInt32(ddlapplicationtype.SelectedValue);
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);              
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);              
                Int32 institueId = Convert.ToInt32(Ddlinstitutes.SelectedValue);
                String rangefrom= txtRegnoFrom.Text.Trim();
                String rangeto = txtRegnoTo.Text.Trim();
                string datetimeF = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string dirnamedate = datetimeF.Replace("/", "").Replace(":","").Replace(" ","_");
                String directoryName = ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "") + "_" + context.Courses.Where(s=>s.ID == courseID).FirstOrDefault().Code.ToUpper();
                String Course_Code = context.Courses.Where(s => s.ID == courseID).FirstOrDefault().Code.ToUpper().Replace("/","_");
                if (Ddlinstitutes.SelectedValue != "0")
                    if (ddlExamName.SelectedValue != "0")
                        directoryName = Ddlinstitutes.SelectedItem.Text.Replace(",", "_").Replace("/", "_") + "_" + directoryName.Replace("/", "_") + "_" + dirnamedate;
                    else
                       // directoryName = Ddlinstitutes.SelectedItem.Text.Replace(",", "_").Replace("/", "_") + "_" + ddlCourseName.SelectedItem.Text + "_" + dirnamedate;
                        directoryName = Ddlinstitutes.SelectedItem.Text.Replace(",", "_").Replace("/", "_") + "_" + Course_Code + "_" + dirnamedate;
                else
                    //directoryName = "NIELIT_" + ddlCourseName.SelectedItem.Text + "_" + dirnamedate; 
                    directoryName = "NIELIT_" + Course_Code + "_" + dirnamedate;
                directoryName = directoryName.Replace("-", "").Replace("/","_");
                string directoryPath = Server.MapPath("~/Download/" + directoryName);

                if (!Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName));
                }

                string photoName = "";
                if (applTypeID == 2) // For Certificate Exam Application photo downloads start
                {
                    if (examID != 0) // exam cycle and year and name  SELECTED by user 
                    {
                        var photos = context.CertificateExamApplications.Where(a => a.ExamID == examID && a.CourseID == courseID
                                  && a.FinalSubmitted == true); //ExamId wise
                        if (CandidateTypeID == 2) // Institute Type Candidates
                        {
                            if (Ddlinstitutes.SelectedValue != "0") //  institute id not null
                            {
                                if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range  roll number or application number
                                {
                                    photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == CandidateTypeID);
                                }
                                else // given range roll number or application number
                                {
                                    if (ddlNameFormat.SelectedValue == "2")
                                    {
                                        photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber != null &&
                                            ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                                    }
                                    else
                                    {
                                        photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.Number != null &&
                                              ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                                    }
                                }
                            }
                            else //All Institute  
                            {
                                if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                                {
                                    photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID);
                                }
                                else // given range roll number or application number
                                {
                                    if (ddlNameFormat.SelectedValue == "2")
                                    {
                                        photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber != null &&
                                            ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                                    }
                                    else
                                    {
                                        photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.Number != null &&
                                              ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                                    }
                                }
                            }
                        }
                        else if (CandidateTypeID == 1) // For 1=Direct Candidates
                        {
                            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                            {
                                photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID);
                            }
                            else // given range roll number or application number
                            {
                                if (ddlNameFormat.SelectedValue == "2")
                                {
                                    photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber != null &&
                                        ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                                }
                                else
                                {
                                    photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber != null && s.Number != null &&
                                        ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                                }
                            }
                        }
                        else if (CandidateTypeID == 0) // For Both 2=Institute and 1=Direct Candidates
                        {
                            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                            {
                                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)));
                            }
                            else // given range roll number or application number
                            {
                                if (ddlNameFormat.SelectedValue == "2")
                                {
                                    photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.RollNumber != null &&
                                                 ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                                }
                                else
                                {
                                    photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number != null &&
                                            ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                                }
                            }
                        }
                         photoCount = photos.Count();
                        if (photoCount == 0)
                        {
                            Directory.Delete(directoryPath);
                        }
                        else
                        {
                            foreach (var photo in photos)
                            {
                                if (ddlNameFormat.SelectedValue == "1")
                                    photoName = photo.Number;
                                else if (ddlNameFormat.SelectedValue == "2")
                                    photoName = photo.RollNumber;
                                else
                                    photoName = photo.Number;
                                FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpg"));
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(photo.Photo);
                                bw.Flush();
                                fs.Flush();
                                bw.Close();
                                fs.Close();
                            }
                        }
                    }
                    //else // exam cycle and year and name not selected by user
                    //{
                    //    var photos = context.CertificateExamApplications.Where(a => a.CourseID == courseID && a.FinalSubmitted == true);
                    //    if (CandidateTypeID == 2) // Institute Type Candidates
                    //    {
                    //        if (Ddlinstitutes.SelectedValue != "0") //  institute id not null
                    //        {
                    //            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range  roll number or application number
                    //            {
                    //                photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == CandidateTypeID);
                    //            }
                    //            else // given range roll number or application number
                    //            {
                    //                if (ddlNameFormat.SelectedValue == "2")
                    //                {
                    //                    photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == CandidateTypeID & s.RollNumber!=null &&
                    //                           ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //                }
                    //                else
                    //                {
                    //                    photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == CandidateTypeID && s.Number!=null &&
                    //                          ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                    //                }
                    //                //photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == CandidateTypeID &&
                    //                //    ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0) || (s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //            }
                    //        }
                    //        else //All Institute  
                    //        {
                    //            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                    //            {
                    //                photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID);
                    //            }
                    //            else // given range roll number or application number
                    //            {
                    //                if (ddlNameFormat.SelectedValue == "2")
                    //                {
                    //                    photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber!=null &&
                    //                             ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //                }
                    //                else
                    //                {
                    //                    photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.Number!=null &&
                    //                                  ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                    //                }
                    //                //photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID &&
                    //                //    ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0) || (s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //            }
                    //        }
                    //    }
                    //    else if (CandidateTypeID == 1) // For 1=Direct Candidates
                    //    {
                    //        if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                    //        {
                    //            photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID);
                    //        }
                    //        else // given range roll number or application number
                    //        {
                    //            if (ddlNameFormat.SelectedValue == "2")
                    //            {
                    //                photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber!=null &&
                    //                    ( (s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //            }
                    //            else
                    //            {
                    //                photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.Number!=null &&
                    //                         ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                    //            }
                    //            //photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID &&
                    //            //    ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0) || (s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //        }
                    //    }
                    //    else if (CandidateTypeID == 0) // For 2=Institute and 1=Direct Both Candidates and 0= Both Direct and Institute
                    //    {
                    //        if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                    //        {
                    //            photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)));
                    //        }
                    //        else // given range roll number or application number
                    //        {
                    //            if (ddlNameFormat.SelectedValue == "2")
                    //            {
                    //                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.RollNumber!=null &&
                    //                    ((s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //            }
                    //            else
                    //            {
                    //                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number!=null &&
                    //                    ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0)));
                    //            }
                    //            //photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) &&
                    //            //    ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0) || (s.RollNumber.CompareTo(rangefrom) >= 0 && s.RollNumber.CompareTo(rangeto) <= 0)));
                    //        }
                    //    }
                    //    foreach (var photo in photos)
                    //    {
                    //        if (ddlNameFormat.SelectedValue == "1")
                    //            photoName = photo.Number;
                    //        else if (ddlNameFormat.SelectedValue == "2")
                    //            photoName = photo.RollNumber;
                    //        else
                    //            photoName = photo.Number;
                    //        FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpeg"));
                    //        BinaryWriter bw = new BinaryWriter(fs);
                    //        bw.Write(photo.Photo);
                    //        bw.Flush();
                    //        fs.Flush();
                    //        bw.Close();
                    //        fs.Close();
                    //    }
                    //}
                } // For Certificate Exam Application photo downloads END

                else if (applTypeID == 1) //For Registration Application Details photo download // add code comment by deep on 20 Nov 2019 //For Registration Application Details photo download
                {
                    if (examID != 0) // exam cycle and year and name  SELECTED by user 
                    {
                        var photos = (from a in context.CourseRegistrationApplications.AsNoTracking()
                                      join b in context.RegistrationDetails.AsNoTracking() on a.CandidateID equals b.CandidateID
                                      where a.ApplicableExamID == examID && a.CourseID == courseID && a.CourseID == b.CourseID && a.FinalSubmitted == true
                                      select new { a.Photo, a.Number, b.RegistrationNo, a.InstituteID, a.ApplicantTypeID }).ToList();
                        if (CandidateTypeID == 2) // Institute Type Candidates
                        {
                            if (Ddlinstitutes.SelectedValue != "0") //  institute id not null
                            {
                                if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range  roll number or application number
                                {
                                    photos = photos.Where(s => s.InstituteID == institueId && s.Number != null && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                                }
                                else // given range roll number or application number
                                {
                                    if (ddlNameFormat.SelectedValue == "3")
                                    {
                                        photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                               ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                    }
                                    else
                                        photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                           ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();

                                }
                            }
                            else //All Institute  
                            {
                                if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                                {
                                    //photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber != null);
                                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null).ToList();
                                }
                                else // given range roll number or application number
                                {
                                    if (ddlNameFormat.SelectedValue == "3")
                                    {
                                        photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                               ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                    }
                                    else
                                        photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                           ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                                }
                            }
                        }
                        else if (CandidateTypeID == 1) // For 1=Direct Candidates
                        {
                            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                            {
                                photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.Number != null).ToList();
                            }
                            else // given range roll number or application number
                            {
                                if (ddlNameFormat.SelectedValue == "3")
                                {
                                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && s.Number != null && s.RegistrationNo != null &&
                                           ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                }
                                else
                                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && s.Number != null &&
                                       ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                            }
                        }
                        else if (CandidateTypeID == 0) // For Both 2=Institute and 1=Direct Candidates
                        {
                            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                            {
                                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number != null && s.RegistrationNo != null).ToList();
                            }
                            else // given range roll number or application number
                            {
                                if (ddlNameFormat.SelectedValue == "3")
                                {
                                    photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) &&
                                           ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                }
                                else
                                    photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number != null &&
                                       ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                            }
                        }
                         photoCount = photos.Count();
                        if (photoCount == 0)
                        {
                            Directory.Delete(directoryPath);
                        }
                        else
                        {
                            foreach (var photo in photos)
                            {
                                if (ddlNameFormat.SelectedValue == "1")
                                {
                                    photoName = photo.Number;
                                    photoName = photoName.Replace("/", "_");
                                }
                                else if (ddlNameFormat.SelectedValue == "3")
                                {
                                    photoName = (Convert.ToString(photo.RegistrationNo));
                                }
                                else
                                {
                                    photoName = photo.Number;
                                    photoName = photoName.Replace("/", "_");
                                     // photoName = (Convert.ToString(photo.));
                                }
                                FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpg"));
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(photo.Photo);
                                bw.Flush();
                                fs.Flush();
                                bw.Close();
                                fs.Close();
                            }
                        }
                    }
                    //else // exam cycle and year and name not selected by user
                    //{
                    //    var photos = (from a in context.CourseRegistrationApplications.AsNoTracking()
                    //                  join b in context.CourseExamApplications.AsNoTracking() on a.CandidateID equals b.CandidateID
                    //                  where  b.CourseID == a.CourseID && b.CourseID == courseID && b.FinalSubmitted == true
                    //                  select new { a.Photo, a.Number, b.RegistrationNumber, a.InstituteID, a.ApplicantTypeID }).ToList();
                    //    if (CandidateTypeID == 2) // Institute Type Candidates
                    //    {
                    //        if (Ddlinstitutes.SelectedValue != "0") //  institute id not null
                    //        {
                    //            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range  roll number or application number
                    //            {
                    //                photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                    //            }
                    //            else // given range roll number or application number
                    //            {
                    //                if (ddlNameFormat.SelectedValue == "3")
                    //                {
                    //                    photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) &&
                    //                           ((s.RegistrationNumber >= Convert.ToInt32(rangefrom) && s.RegistrationNumber <= Convert.ToInt32(rangeto)))).ToList();
                    //                }
                    //                else
                    //                    photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                    //                       ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                    //            }
                    //        }
                    //        else //All Institute  
                    //        {
                    //            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                    //            {                                    
                    //                photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                    //            }
                    //            else // given range roll number or application number
                    //            {
                    //                if (ddlNameFormat.SelectedValue == "3")
                    //                {
                    //                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) &&
                    //                           ((s.RegistrationNumber >= Convert.ToInt32(rangefrom) && s.RegistrationNumber <= Convert.ToInt32(rangeto)))).ToList();
                    //                }
                    //                else
                    //                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                    //                       ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                    //            }
                    //        }
                    //    }
                    //    else if (CandidateTypeID == 1) // For 1=Direct Candidates
                    //    {
                    //        if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                    //        {
                    //            photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID).ToList();
                    //        }
                    //        else // given range roll number or application number
                    //        {
                    //            if (ddlNameFormat.SelectedValue == "3") // 3= Registration number
                    //            {
                    //                photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) &&
                    //                       ((s.RegistrationNumber >= Convert.ToInt32(rangefrom) && s.RegistrationNumber <= Convert.ToInt32(rangeto)))).ToList();
                    //            }
                    //            else
                    //                photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && s.Number != null &&
                    //                   ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                    //        }
                    //    }
                    //    else if (CandidateTypeID == 0) // For Both 2=Institute and 1=Direct Candidates
                    //    {
                    //        if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                    //        {
                    //            photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2))).ToList();
                    //        }
                    //        else // given range roll number or application number
                    //        {
                    //            if (ddlNameFormat.SelectedValue == "3")
                    //            {
                    //                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) &&
                    //                       ((s.RegistrationNumber >= Convert.ToInt32(rangefrom) && s.RegistrationNumber <= Convert.ToInt32(rangeto)))).ToList();
                    //            }
                    //            else
                    //                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number != null &&
                    //                   ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                    //        }
                    //    }

                    //    foreach (var photo in photos)
                    //    {
                    //        if (ddlNameFormat.SelectedValue == "1")
                    //            photoName = photo.Number;
                    //        else if (ddlNameFormat.SelectedValue == "3")
                    //            photoName = (Convert.ToString(photo.RegistrationNumber));
                    //        else
                    //            photoName = photo.Number;
                    //        FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpeg"));
                    //        BinaryWriter bw = new BinaryWriter(fs);
                    //        bw.Write(photo.Photo);
                    //        bw.Flush();
                    //        fs.Flush();
                    //        bw.Close();
                    //        fs.Close();
                    //    }
                    //}
                }//For Course Exam Application photo downloads END
				      // add code comment by deep on 20 Nov 2019 
                else // course exam apllication select on dropdown list and implement course registration and course exam application table join for data fetch
                {
                    if (examID != 0) // exam cycle and year and name  SELECTED by user 
                    {
                        var photos = (from a in context.CourseRegistrationApplications.AsNoTracking()
                                      join b in context.RegistrationDetails.AsNoTracking() on a.CandidateID equals b.CandidateID
                                      join c in context.CourseExamApplications.AsNoTracking() on b.CandidateID equals c.CandidateID
                                      where c.ExamID == examID && a.CourseID == courseID && a.CourseID == b.CourseID && a.FinalSubmitted == true
                                      && c.FinalSubmitted==true && c.PaymentStatusID==2 &&c.ApplicationStatusID == 8
                                      select new { a.Photo, a.Number, b.RegistrationNo, a.InstituteID , a.ApplicantTypeID , c.NSQFRollNo }).ToList();
                        if (CandidateTypeID == 2) // Institute Type Candidates
                        {
                            if (Ddlinstitutes.SelectedValue != "0") //  institute id not null
                            {
                                if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range  roll number or application number
                                {
                                    photos = photos.Where(s => s.InstituteID == institueId && s.Number != null && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                                }
                                else // given range roll number or application number
                                {
                                    if (ddlNameFormat.SelectedValue == "3")
                                    {
                                        photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                               ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                    }
                                    else
                                        photos = photos.Where(s => s.InstituteID == institueId && s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                           ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                                }
                            }
                            else //All Institute  
                            {
                                if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                                {
                                    //photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.RollNumber != null);
                                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null).ToList();
                                }
                                else // given range roll number or application number
                                {
                                    if (ddlNameFormat.SelectedValue == "3")
                                    {
                                        photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                               ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                    }
                                    else
                                        photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && s.Number != null &&
                                           ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                                }
                            }
                        }
                        else if (CandidateTypeID == 1) // For 1=Direct Candidates
                        {
                            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                            {
                                photos = photos.Where(s => s.ApplicantTypeID == CandidateTypeID && s.Number != null).ToList();
                            }
                            else // given range roll number or application number
                            {
                                if (ddlNameFormat.SelectedValue == "3")
                                {
                                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && s.Number != null && s.RegistrationNo != null &&
                                           ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                }
                                else
                                    photos = photos.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && s.Number != null &&
                                       ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                            }
                        }
                        else if (CandidateTypeID == 0) // For Both 2=Institute and 1=Direct Candidates
                        {
                            if (String.IsNullOrEmpty(rangefrom) || String.IsNullOrEmpty(rangeto)) // not given range roll number or application number
                            {
                                photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number != null && s.RegistrationNo != null).ToList();
                            }
                            else // given range roll number or application number
                            {
                                if (ddlNameFormat.SelectedValue == "3")
                                {
                                    photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) &&
                                           ((s.RegistrationNo >= Convert.ToInt32(rangefrom) && s.RegistrationNo <= Convert.ToInt32(rangeto)))).ToList();
                                }
                                else
                                    photos = photos.Where(s => ((s.ApplicantTypeID == 1) || (s.ApplicantTypeID == 2)) && s.Number != null &&
                                       ((s.Number.CompareTo(rangefrom) >= 0 && s.Number.CompareTo(rangeto) <= 0))).ToList();
                            }
                        }
                        photoCount = photos.Count();
                        if (photoCount == 0)
                        {
                            Directory.Delete(directoryPath);
                        }
                        else
                        {
                            foreach (var photo in photos)
                            {
                                if (ddlNameFormat.SelectedValue == "1")
                                {
                                    photoName = photo.Number;
                                    photoName = photoName.Replace("/", "_");
                                }
                                else if (ddlNameFormat.SelectedValue == "3")
                                {
                                    photoName = (Convert.ToString(photo.RegistrationNo));
                                }
                                else
                                {
                                    //photoName = photo.Number;
                                    //photoName = photoName.Replace("/", "_");
                                    photoName = (Convert.ToString(photo.NSQFRollNo));
                                }
                                FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + ".jpg"));
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(photo.Photo);
                                bw.Flush();
                                fs.Flush();
                                bw.Close();
                                fs.Close();
                            }
                        }
                    }
                }// add code END comment by deep on 20 Nov 2019
                if (photoCount != 0)
                {
                    using (ZipFile zipFile = new ZipFile())
                    {
                        zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                        Response.Clear();
                        zipFile.CompressionMethod = CompressionMethod.None;
                        zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + directoryName + ".zip");
                        zipFile.Save(Response.OutputStream);
                    };
                }
                else
                {
                    lblNoRecord.Text = "No Record Found";
                    lblNoRecord.Visible = true;
                }                
            };
        }
        catch (Exception ex)
        {
            Response.Write(ex.InnerException.ToString() + "<br/>" + ex.Message.ToString() + "<br/> " + ex.InnerException.InnerException.Message.ToString() + "<br/>" + ex.InnerException.Message.ToString());
        }
    }
    protected void Ddlinstitutes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (var context = new EConnectContext())
            {
                //chkbatchlist.Items.Clear();
                //chkbatchlist.DataBind();
                if (Ddlinstitutes.SelectedIndex == 1)
                {
                    Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
                    Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                    Int32 completed = Convert.ToInt32(enmBatchStatus.Completed);
                    ListItem lst = new ListItem("--Select One--", "0");
                    var batches = from b in context.Batchs
                                  where b.ApplicationTypeID == applType && b.ExamID == examID && b.StatusID == completed
                                  select new { ValueField = b.ID, TextField = b.Number };
                    //if (ddlRc.SelectedValue != "0")
                    //{
                    //    Int32 regionalCentreID = Convert.ToInt32(ddlRc.SelectedValue);
                    //    batches = batches.Where(b => b.RegionalCenterID == regionalCentreID);
                    //}
                    //EConnect.Utils.Common.ControlUtility.BindListObject(chkbatchlist, batches.ToList());
                    //if (chkbatchlist.Items.Count <= 0)
                    //{
                    //    ShowAlert("Batches does not exist in this category.", true);
                    //    Ddlinstitutes.SelectedIndex = 0;
                    //    return;
                    //}
                }
                else
                {
                    divbatches.Style.Remove("overflow");
                    divbatches.Style.Remove("width");
                    divbatches.Style.Remove("height");
                }
                //chkbatchlist.Visible = (chkbatchlist.Items.Count > 0);
                //if (chkbatchlist.Items.Count > 6)
                //{
                //    divbatches.Style.Add("overflow", "scroll");
                //    divbatches.Style.Add("width", "750px");
                //    divbatches.Style.Add("height", "90px");
                //    chkbatchlist.RepeatColumns = 10;
                //}
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}