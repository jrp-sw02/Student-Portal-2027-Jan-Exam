using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
//using iTextSharp.text;

public partial class BulkCertificateExamData : BasePage
{
    Int64 EntityID = 0;
    Int32 currentRoleId = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);

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
            EntityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                BindCourseCategory();
               
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Exam Data", "#", ""));
                lblcount.Text = "";
                lblcount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = (from s in context.CourseCategories
                              join c in context.Courses
                                  on s.ID equals c.CourseCategoryID
                              where c.CourseTypeID == courseTypeCertificateExam
                              select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses, lst);
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
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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

    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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
            ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                    System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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
            using (var context = new EConnectContext())
            {
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    var courses = (from s in context.Exams
                                   join c in context.CertificateExamApplications
                                       on s.ID equals c.ExamID
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                   select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
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
            using (var context = new EConnectContext())
            {
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    var courses = (from s in context.Exams
                                   join c in context.CertificateExamApplications
                                       on s.ID equals c.ExamID
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear && s.DateOfPublishingOfRollNumber != null
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
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
            lblcount.Text = "";
            lblcount.Visible = false;
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
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    var regional_center = (from s in context.RegionalCenters
                                   join c in context.CertificateExamApplications
                                       on s.ID equals c.RegionalCenterID
                                       where  
                                       c.ExamID ==examID 
                                
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRC, regional_center, lst);
                }
                else
                {
                    ddlRC.Items.Clear();
                    ddlRC.Items.Insert(0, lst);
                    ddlRC.SelectedValue = "0";
                }
            }


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString(), true);
        }

    }

    protected void ddlRC_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            int rccode = Convert.ToInt32(ddlRC.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                  
                    var Exam_Date =( from  c in context.CertificateExamApplications
                                          where c.ExamID ==examID && c.CourseID ==courseId  && c.RegionalCenterID ==rccode && c.DateOfExam != null
                                     select new { ValueField = c.DateOfExam.Value, TextField = c.DateOfExam.Value }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamDate, Exam_Date, lst);
                }
                else
                {
                    ddlExamDate.Items.Clear();
                    ddlExamDate.Items.Insert(0, lst);
                    ddlExamDate.SelectedValue = "0";
                }
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString(), true);
        }
    }

    protected void ddlExamDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            int rccode = Convert.ToInt32(ddlRC.SelectedValue);
            DateTime examdate=Convert.ToDateTime(ddlExamDate.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {

                    var Exam_Batch = (from c in context.CertificateExamApplications
                                     where c.ExamID == examID && c.CourseID == courseId && c.RegionalCenterID == rccode && c.DateOfExam ==examdate 
                                     select new { ValueField = c.ExamBatchNumber, TextField = c.ExamBatchNumber }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamBatch, Exam_Batch, lst);

                  
                }
                else
                {
                    ddlExamBatch.Items.Clear();
                    ddlExamBatch.Items.Insert(0, lst);
                    ddlExamBatch.SelectedValue = "0";
                }
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString(), true);
        }
    }

    protected void ddlExamBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            int rccode = Convert.ToInt32(ddlRC.SelectedValue);
            string batchnumber = ddlExamBatch.SelectedValue;
            DateTime examdate = Convert.ToDateTime(ddlExamDate.SelectedValue);
            ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var Exam_Center = (from c in context.CertificateExamApplications
                                  where c.ExamID == examID && c.CourseID == courseId && c.RegionalCenterID == rccode && c.DateOfExam == examdate
                                  && c.ExamBatchNumber == batchnumber orderby  c.ExamCentreName ascending 
                                  select new { ValueField = c.ExamCentreName, TextField = c.ExamCentreName }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCenter, Exam_Center, lst);

            };
        }
        catch (Exception ex) { ShowAlert(ex.Message); }

    }
}