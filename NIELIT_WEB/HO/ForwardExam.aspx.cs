using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.SqlClient;

public partial class HO_Default : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    string sql;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/ForwardExam.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                FillFilterCourseCategory();
                FillRemarks();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "Please Select Filter Criteria to View Application Records";
                    lblError.Visible = true;
                    NewExam.Visible = false;
                }
                if (Request.QueryString["courseid"] != null || Request.QueryString["examID"] != null || Request.QueryString["examcycle"] != null || Request.QueryString["examyear"] != null || Request.QueryString["categoryid"] != null || Request.QueryString["regionalcentre"] != null || Request.QueryString["examcentre"] != null)
                {
                    ddlcoursecategory.SelectedValue = Request.QueryString["categoryid"];
                    ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory.SelectedValue, EventArgs.Empty);
                    ddlcourse.SelectedValue = Request.QueryString["courseid"];
                    ddlcourse_SelectedIndexChanged(ddlcourse.SelectedValue, EventArgs.Empty);
                    ddlexamcycle.SelectedValue = Request.QueryString["examcycle"];
                    ddlexamcycle_SelectedIndexChanged(ddlexamcycle.SelectedValue, EventArgs.Empty);
                    ddlexamyear.SelectedValue = Request.QueryString["examyear"];
                    ddlexamyear_SelectedIndexChanged(ddlexamyear.SelectedValue, EventArgs.Empty);
                    ddlexamname.SelectedValue = Request.QueryString["examID"];
                    ddlexamname_SelectedIndexChanged(ddlexamname.SelectedValue, EventArgs.Empty);
                    ddlexamcenter.SelectedValue = Request.QueryString["examcentre"];
                    ddlRc.SelectedValue = Request.QueryString["regionalcentre"];
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Forward Applications:-" + ddlcourse.SelectedItem.Text + "-" + ddlexamname.SelectedItem.Text, "HO/ForwardExam.aspx?courseid=" + ddlcourse.SelectedValue + "&examID=" + ddlexamname.SelectedValue + "&examcycle=" + ddlexamcycle.SelectedValue + "&examyear=" + ddlexamyear.SelectedValue + "&categoryid=" + ddlcoursecategory.SelectedValue + "&regionalcentre=" + ddlRc.SelectedValue + "&examcentre=" + ddlexamcenter.SelectedValue, ""));
                    BreadCrumb1.Render();
                    if (Request.QueryString["updated"] != null)
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('" + Request.QueryString["updated"].ToString() + " applications forwarded to " + ddlNewExam.SelectedItem.Text + " Exam successfully');", true);
                    BindGridView();
                }
                else
                {
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Forward Applications", "HO/ForwardExam.aspx", ""));
                    BreadCrumb1.Render();

                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    // added by amit 18-02-26 start
    protected bool isDuplicateApaar(Int32 examID, Int64 appID, out string duplicateApaar)
    {
        using (EConnectContext context = new EConnectContext())
        {
            var appl = context.CertificateExamApplications.Find(appID);

            var duplicate = context.CertificateExamApplications
                .Where(c => c.ExamID == examID
                         && c.apaarID == appl.apaarID
                         && c.ID != appID)
                .Select(c => c.Number)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(duplicate))
            {
                duplicateApaar = duplicate;
                return true;   // duplicate exists
            }

            duplicateApaar = "";
            return false;      // no duplicate
        }
    }

    // added by amit 18-02-26 end


    #region Private Methods----
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if (ddlcourse.SelectedValue != "0" || ddlexamname.SelectedValue != "0" || ddlexamcycle.SelectedValue != "0" || ddlexamyear.SelectedValue != "0" || ddlcoursecategory.SelectedValue != "0" || ddlRc.SelectedValue != "0" || ddlexamcenter.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Forward Applications:-" + ddlcourse.SelectedItem.Text + "-" + ddlexamname.SelectedItem.Text, "HO/ForwardExam.aspx?courseid=" + ddlcourse.SelectedValue + "&examID=" + ddlexamname.SelectedValue + "&examcycle=" + ddlexamcycle.SelectedValue + "&examyear=" + ddlexamyear.SelectedValue + "&categoryid=" + ddlcoursecategory.SelectedValue + "&regionalcentre=" + ddlRc.SelectedValue + "&examcentre=" + ddlexamcenter.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
                BindGridView();
            }
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlcourse.SelectedValue = "0";
            ddlexamname.SelectedValue = "0";
            ddlexamcycle.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("ForwardExam.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillFilterCourse(Int32 ccatid)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 coutrsetypeid = Convert.ToInt32(enmCourseType.CertificationExam);
                // ListItem lst = new ListItem("--Select One--", "0");

                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = from p in context.Courses
                              where p.CourseTypeID == coutrsetypeid && p.CourseCategoryID == ccatid
                              orderby p.DisplayOrder
                              select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, courses, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 coutrsetypeid = Convert.ToInt32(enmCourseType.CertificationExam);
                //   ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = (from p in context.Courses
                               join c in context.CourseCategories
                                   on p.CourseCategoryID equals c.ID
                               where p.CourseTypeID == coutrsetypeid
                               orderby p.DisplayOrder
                               select new { ValueField = c.ID, TextField = c.Name }).Distinct();

                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                //ListItem lst1 = new ListItem("--All--", "0");
                System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                if (loginUserType == UserType.RegionalCenter)
                {
                    ddlRc.SelectedValue = entityID.ToString();
                    ddlRc.Enabled = false;
                }
                //var paymentstatus = from s in context.PaymentStatuss
                //                    select new { ValueField = s.ID, TextField = s.Name };
                //ListItem lst2 = new ListItem("--Select One--", "0");
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlPayment, paymentstatus.OrderBy(c => c.TextField), lst2);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, courses, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillRemarks()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var remarks = (from p in context.Remarks
                               orderby p.DisplayOrder
                               select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRemarks, remarks, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindExamCycle(int CourseId)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                // ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = (from s in context.ExaminationCycles
                               join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                               where s.CourseID == CourseId
                               select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                //var courses = (from s in context.ExaminationCycles
                //               join c in context.CertificateExamApplications
                //                   on s.ID equals c.Exam.ExaminationCycleID
                //               where s.CourseID == CourseId
                //               select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamcycle, courses, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void BindExaminationCycle(int CourseId)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                // ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = (from s in context.ExaminationCycles
                               join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                               where s.CourseID == CourseId
                               select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                //var courses = (from s in context.ExaminationCycles
                //               join c in context.CertificateExamApplications
                //                   on s.ID equals c.Exam.ExaminationCycleID
                //               where s.CourseID == CourseId
                //               select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExaminationCycle, courses, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExaminationCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindNewExam(Convert.ToInt32(ddlcourse.SelectedValue), Convert.ToInt32(ddlExaminationCycle.SelectedValue));
        }
        catch
        {
            ShowAlert("Error Occurred", true);

        }

    }

    protected void BindExamname(Int32 cid, Int32 examcycleid, Int32 examyear)
    {

        try
        {
            //ListItem lst = new ListItem("--Select One--", "0");
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var courses = (from s in context.Exams
                                   where s.CourseID == cid && s.ExaminationCycleID == examcycleid && s.ExamYear == examyear
                                   && s.DateOfPublishingOfTimeTable.HasValue
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, courses, lst);
                }
                else
                {
                    ddlexamname.Items.Insert(0, lst);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void BindExamYear(Int32 cid, Int32 examcycleid)
    {

        try
        {
            //ListItem lst = new ListItem("--Select One--", "0");
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var examyear = (from s in context.Exams
                                    where s.CourseID == cid && s.ExaminationCycleID == examcycleid
                                    select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    examyear = examyear.OrderByDescending(s => s.TextField).Take(4);

                    //var examyear = (from s in context.Exams
                    //                join c in context.CertificateExamApplications
                    //                 on s.ID equals c.ExamID
                    //                where s.CourseID == cid && s.ExaminationCycleID == examcycleid
                    //                orderby (s.ExamYear) descending
                    //                select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamyear, examyear, lst);
                }
                else
                {
                    ddlexamname.Items.Insert(0, lst);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examid = 0;
                Int32 courseid = 0;
                Int32 ccatid = 0;
                Int32 examyear = 0;
                Int32 examcycleid = 0;
                Int32 regcenterid = 0;
                Int32 examcentre1id = 0;
                Int32 paymentStatus = Convert.ToInt32(enmPaymentStatus.Pending);

                if (ddlcoursecategory.SelectedValue != "0")
                {
                    ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["categoryid"]))
                        ccatid = Convert.ToInt32(Request.QueryString["categoryid"]);
                }
                if (ddlcourse.SelectedValue != "0")
                {
                    courseid = Convert.ToInt32(ddlcourse.SelectedValue);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["courseid"]))
                        courseid = Convert.ToInt32(Request.QueryString["courseid"]);
                }
                if (ddlexamyear.SelectedValue != "0")
                {
                    examyear = Convert.ToInt32(ddlexamyear.SelectedValue);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["examyear"]))
                        examyear = Convert.ToInt32(Request.QueryString["examyear"]);
                }
                if (ddlexamname.SelectedValue != "0")
                {
                    examid = Convert.ToInt32(ddlexamname.SelectedValue);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["examID"]))
                        examid = Convert.ToInt32(Request.QueryString["examID"]);
                }
                if (ddlexamcycle.SelectedValue != "0")
                {
                    examcycleid = Convert.ToInt32(ddlexamcycle.SelectedValue);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["examcycle"]))
                        examcycleid = Convert.ToInt32(Request.QueryString["examcycle"]);
                }
                if (ddlRc.SelectedValue != "0")
                {
                    regcenterid = Convert.ToInt32(ddlRc.SelectedValue);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["regionalcentre"]))
                        regcenterid = Convert.ToInt32(Request.QueryString["regionalcentre"]);
                }
                if (ddlexamcenter.SelectedValue != "0")
                {
                    if (ddlexamcenter.SelectedValue == "-- All --")
                    {
                        examcentre1id = 1;
                    }
                    else
                    {
                        examcentre1id = Convert.ToInt32(ddlexamcenter.SelectedValue);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["examcentre"]))
                        examcentre1id = Convert.ToInt32(Request.QueryString["examcentre"]);
                }

                Int32 ResultVersionID = context.Exams.Where(s => s.ID == examid).FirstOrDefault().ResultGradeVersionID.Value;
                Int32 AbsentResultGradeID = context.ResultGrades.Where(s => s.CourseCategoryID == ccatid && s.VersionID == ResultVersionID && s.Code.ToUpper() == "ABS").FirstOrDefault().ID;
                Int32 ExamCancelledGradeID = context.ResultGrades.Where(s => s.CourseCategoryID == ccatid && s.VersionID == ResultVersionID && s.Code.ToUpper() == "@").FirstOrDefault().ID;
                if (courseid != 0 && examid != 0 && ccatid != 0 && regcenterid != 0 && examcentre1id != 0)
                {

                    if (examcentre1id == 1)
                    {
                        var data = from p in context.CertificateExamApplications
                                   where p.CourseID == courseid &&
                                                   p.CourseCategoryID == ccatid &&
                                                   p.Exam.ExamYear == examyear &&
                                                   p.Exam.ID == examid &&
                                                   p.Exam.ExaminationCycleID == examcycleid &&
                                                   //p.ExamCenter1ID == examcentre1id &&
                                                   p.RegionalCenterID == regcenterid &&
                                                   //(p.ResultGradeID == null || p.ResultGradeID == AbsentResultGradeID || p.ResultGradeID == ExamCancelledGradeID) &&
                                                   ((p.ResultGradeID == null && p.RollNumber == null) || (p.RollNumber != null && p.ResultGradeID == ExamCancelledGradeID)) &&
                                                   p.PaymentStatusID > paymentStatus &&
                                                   p.ApplicationStatusID == 17 &&
                                                   p.FinalSubmitted == true
                                   select new
                                   {
                                       ID = p.ID,
                                       AppNo = p.Number,
                                       RollNumber = p.RollNumber != null ? p.RollNumber : "N/A",
                                       ResultGrade = p.ResultGradeID.HasValue ? p.ResultGrade.Code : "N/A",
                                       AppDate = p.ApplicationDate,
                                       CandidateName = p.Name,
                                       DOB = p.DateOfBirth,
                                       FatherName = p.FatherName
                                       //Payment = p.DemandNote.NEFTTransaction == null ? (p.DemandNote.DemandDraftTransaction == null ? (p.DemandNote.OnlineTransaction == null ? "RcptNo:" + p.DemandNote.CSCTransaction.ResponseTransactionNumber : "RcptNo:" + p.DemandNote.OnlineTransaction.ReferenceNumber) : "DDNo: " + p.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "TransactionNo:" + p.DemandNote.NEFTTransaction.TransactionNumber
                                   };
                        data = data.OrderBy(s => s.RollNumber);
                        PagingBar1.Bind(data, ref gvMain);

                    }
                    else
                    {

                        var data = from p in context.CertificateExamApplications
                                   where p.CourseID == courseid &&
                                                   p.CourseCategoryID == ccatid &&
                                                   p.Exam.ExamYear == examyear &&
                                                   p.Exam.ID == examid &&
                                                   p.Exam.ExaminationCycleID == examcycleid && p.ExamCenter1ID == examcentre1id &&
                                                   p.RegionalCenterID == regcenterid &&
                                                   //(p.ResultGradeID == null || p.ResultGradeID == AbsentResultGradeID || p.ResultGradeID == ExamCancelledGradeID) &&
                                                   ((p.ResultGradeID == null && p.RollNumber == null) || (p.RollNumber != null && p.ResultGradeID == ExamCancelledGradeID)) &&
                                                   p.PaymentStatusID > paymentStatus &&
                                                   p.ApplicationStatusID == 17 &&
                                                   p.FinalSubmitted == true
                                   select new
                                   {
                                       ID = p.ID,
                                       AppNo = p.Number,
                                       RollNumber = p.RollNumber != null ? p.RollNumber : "N/A",
                                       ResultGrade = p.ResultGradeID.HasValue ? p.ResultGrade.Code : "N/A",
                                       AppDate = p.ApplicationDate,
                                       CandidateName = p.Name,
                                       DOB = p.DateOfBirth,
                                       FatherName = p.FatherName
                                       //Payment = p.DemandNote.NEFTTransaction == null ? (p.DemandNote.DemandDraftTransaction == null ? (p.DemandNote.OnlineTransaction == null ? "RcptNo:" + p.DemandNote.CSCTransaction.ResponseTransactionNumber : "RcptNo:" + p.DemandNote.OnlineTransaction.ReferenceNumber) : "DDNo: " + p.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "TransactionNo:" + p.DemandNote.NEFTTransaction.TransactionNumber
                                   };
                        data = data.OrderBy(s => s.RollNumber);
                        PagingBar1.Bind(data, ref gvMain);
                    }
                }


                else if (courseid != 0 && examid != 0 && ccatid != 0 && examcentre1id != 0)
                {
                    var data = from p in context.CertificateExamApplications
                               where p.CourseID == courseid &&
                                               p.CourseCategoryID == ccatid &&
                                               p.Exam.ExamYear == examyear &&
                                               p.Exam.ID == examid &&
                                               p.Exam.ExaminationCycleID == examcycleid && p.ExamCenter1ID == examcentre1id &&
                                              // (p.ResultGradeID == null || p.ResultGradeID == AbsentResultGradeID) &&
                                              (p.ResultGradeID == null && p.ResultGradeID == ExamCancelledGradeID) &&
                                               p.PaymentStatusID > paymentStatus &&
                                               p.ApplicationStatusID == 17 &&
                                               p.FinalSubmitted == true
                               select new
                               {
                                   ID = p.ID,
                                   AppNo = p.Number,
                                   AppDate = p.ApplicationDate,
                                   CandidateName = p.Name,
                                   DOB = p.DateOfBirth,
                                   FatherName = p.FatherName,
                                   RollNumber = p.RollNumber != null ? p.RollNumber : "N/A",
                                   ResultGrade = p.ResultGradeID.HasValue ? p.ResultGrade.Code : "N/A"
                                   //Payment = p.DemandNote.NEFTTransaction == null ? (p.DemandNote.DemandDraftTransaction == null ? (p.DemandNote.OnlineTransaction == null ? "Rcpt No:" + p.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + p.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + p.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + p.DemandNote.NEFTTransaction.TransactionNumber
                               };
                    PagingBar1.Bind(data, ref gvMain);
                }

                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    NewExam.Visible = false;
                }
                else
                {
                    lblError.Visible = false;
                    lblError.Text = "";
                    NewExam.Visible = true;
                    //BindNewExam(examid, courseid, examcycleid, examyear);

                    BindExaminationCycle(courseid);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindNewExam(Int32 courseid, Int32 examcycleid)
    {
        try
        {

            //ListItem lst = new ListItem("--Select One--", "0");
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            Int32 lastDateofformfilling = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm); //2
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(courseid);
                if (cr != null)
                {
                    var Exams = (from s in context.Exams
                                 where s.CourseID == courseid && s.ExaminationCycleID == examcycleid && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                 && s.ExamStartDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                 select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    Exams = Exams.OrderByDescending(s => s.ValueField).Take(3); //Take for latest 2 exam
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewExam, Exams, lst);
                }
                else
                {
                    ddlexamname.Items.Insert(0, lst);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void SentBulkMail(List<StudentList> rejList, Int32 UserID)
    {
        for (int i = 0; i < rejList.Count(); i++)
        {
            string msg = "Dear " + GetInitCap(rejList[i].Salutation + " " + rejList[i].AppName) + ",<br/><br/>" + " Your application with Application No :- <b> " + rejList[i].AppNo + " </b> for <b>" + rejList[i].CourseCode + "</b>" + " has been forwarded from <b> " + rejList[i].PreviousExamName + " </b> to <b> " + rejList[i].ExamName + " </b> " +
                    " Exam by NIELIT on " + DateTime.Now.ToString("dd-MMM-yyyy") + " because of the reason :- <b>" + GetInitCap(rejList[i].Reason) + "</b>.";
            try
            {
                //sending Email 
                if (rejList[i].Email.Trim().Length > 0)
                {
                    EConnect.NIELIT.Email mail = new Email("Forwarded to Exam:NIELIT", msg.ToString(), rejList[i].Email);
                    mail.SendInThread(UserID);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    protected void SentBulkSMS(List<StudentList> rejList, Int32 UserID)
    {
        for (int i = 0; i < rejList.Count(); i++)
        {
            string mobilemsg = "Dear " + GetInitCap(rejList[i].Salutation + " " + rejList[i].AppName) + " your application has been forwarded from " + rejList[i].PreviousExamName + " to " + rejList[i].ExamName +
                               " Exam by NIELIT because of the reason :-" + GetInitCap(rejList[i].Reason);
            try
            {
                //sending SMS 
                if (rejList[i].Mobileno != null && rejList[i].Mobileno != 0)
                {
                    EConnect.NIELIT.SMS sms = new SMS(mobilemsg, rejList[i].Mobileno.ToString(), "1307161053028719307", SmsServiceType.BulkSMS, false);
                    int sentMessageCount;
                    sms.sendSingleSMS(out sentMessageCount);
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindExamCenter(Int32 courseid, Int32 examid)
    {

        try
        {
            //ListItem lst = new ListItem("--Select One--", "0");
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(courseid);
                if (cr != null)
                {
                    var courses = (from c in context.CertificateExamApplications
                                   where c.CourseID == courseid && c.ExamID == examid && c.ExamCenter1ID != 0
                                   select new
                                   {
                                       ValueField = c.ExamCenter1ID,
                                       TextField = c.ExamCenter1.Name + "(" + c.ExamCenter1.Code + ")"
                                   }).Distinct().OrderBy(t => t.TextField);


                    if (ddlRc.SelectedValue != "0")
                    {
                        Int32 regid = Convert.ToInt32(ddlRc.SelectedValue);
                        courses = (from c in context.CertificateExamApplications
                                   where c.CourseID == courseid && c.ExamID == examid && c.ExamCenter1ID != 0 && c.RegionalCenterID == regid
                                   select new { ValueField = c.ExamCenter1ID, TextField = c.ExamCenter1.Name + "(" + c.ExamCenter1.Code + ")" }).Distinct().OrderBy(t => t.TextField);
                    }

                    ddlexamcenter.Items.Insert(1, "-- All --");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamcenter, courses, lst);
                    ddlexamcenter.Items.Insert(1, "-- All --");
                    //ddlexamcenter.Items.Insert(0, lst);
                }
                else
                {
                    ddlexamcenter.Items.Insert(0, lst);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool isDuplicate(Int32 examID, Int64 appID, out String DuplicateApplNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                CertificateExamApplication appl = context.CertificateExamApplications.Find(appID);
                String app = "";

                if (!string.IsNullOrEmpty(appl.GuardianName))
                {
                    app = (from c in context.CertificateExamApplications
                           where c.ExamID == examID && c.Name.ToUpper() == appl.Name.ToUpper() && c.ApplicationStatusID == 17 &&
                            c.GuardianName.ToUpper() == appl.GuardianName.ToUpper() &&
                            c.Gender == appl.Gender && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(appl.DateOfBirth)
                           select c.Number).FirstOrDefault();
                }
                else
                {
                    app = (from c in context.CertificateExamApplications
                           where c.ExamID == examID && c.Name.ToUpper() == appl.Name.ToUpper() && c.ApplicationStatusID == 17 &&
                           c.FatherName.ToUpper() == appl.FatherName.ToUpper() && c.MotherName.ToUpper() == appl.MotherName.ToUpper() &&
                           c.Gender == appl.Gender && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(appl.DateOfBirth)
                           select c.Number).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(app))
                { DuplicateApplNumber = app; return true; }
                else
                { DuplicateApplNumber = ""; return false; }
                ;
            }
        }
        catch (Exception ex)
        { throw ex; }
    }
    #endregion

    #region Events----
    protected void btnNewExam_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            int count = 0;
            int Duplicatecount = 0;
            StringBuilder DuplicateAppl = new StringBuilder();
            int ApaarCount = 0;
            StringBuilder ApaarNull = new StringBuilder();
            //added by amit start 18-2-26
            int dupApaarCnt = 0;
            StringBuilder dupApaarNo = new StringBuilder();
            //added by amit end 18-2-26
            String PreviousExamName = ddlexamname.SelectedItem.Text;
            String Remarks = ddlRemarks.SelectedItem.Text;
            int examID = Convert.ToInt32(ddlNewExam.SelectedValue);

            List<StudentList> rejList = new List<StudentList>();
            using (EConnectContext context = new EConnectContext())
            {
                string examName = context.Exams.Find(examID).Name;
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                    string ResultStatus = gvMain.Rows[i].Cells[7].Text;
                    string RollNumber = gvMain.Rows[i].Cells[2].Text;
                    if (cbx != null && ((ResultStatus != "N/A" && ResultStatus != "ABS") || (RollNumber == "N/A" && ResultStatus == "N/A")))
                    {
                        if (cbx.Checked && ddlNewExam.SelectedValue != "0" && ddlRemarks.SelectedValue != "0")
                        {
                            string DuplicateApplNumber = "";
                            string duplicateApaar = "";  //added by amit start 18-2-26
                            Int64 ID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                            CertificateExamApplication appl = context.CertificateExamApplications.Find(ID);
                            //                            if (appl.apaarID.Trim().Length == 0 || appl.apaarID == null)
                            if (appl.apaarID == null || string.IsNullOrWhiteSpace(appl.apaarID))
                            {
                                ApaarCount = ApaarCount + 1;
                                ApaarNull.Append(appl.Number + ", ");
                                continue;
                            }
                            //added by amit start 18-2-26
                            if (isDuplicateApaar(examID, ID, out duplicateApaar))
                            {
                                dupApaarCnt = dupApaarCnt + 1;
                                dupApaarNo.Append("Appl No: " + duplicateApaar + ", ");
                                continue;
                            }
                            //added by amit end 18-2-26

                            if (!isDuplicate(examID, ID, out DuplicateApplNumber))
                            {
                                count = count + 1;
                                //sql = "INSERT INTO Certificate_Exam_Application_History(Certificate_Exam_Application_ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Photo_File_Name,Photo,Signature_File_Name,Signature,Left_Thumb_File_Name,Left_Thumb,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,Created_On,Created_By,Remarks_ID,New_Exam_ID)" +
                                //    "(select ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Photo_File_Name,Photo,Signature_File_Name,Signature,Left_Thumb_File_Name,Left_Thumb,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,'" + DateTime.Now + "','" + Convert.ToInt32(Session["UserID"]) + "', '" + Convert.ToInt32(ddlRemarks.SelectedValue) + "', '" + Convert.ToInt32(ddlNewExam.SelectedValue) + "' from Certificate_Exam_Application Where ID=" + ID + ")";
                                //context.Database.ExecuteSqlCommand(sql);
                                //December_2024
                                SqlParameter[] param1 = { new SqlParameter("@currentDate", DateTime.Now),
                                                            new SqlParameter("@userID", Convert.ToInt32(Session["UserID"])),
                                                                new SqlParameter("@remarks", Convert.ToInt32(ddlRemarks.SelectedValue)),
                                                                    new SqlParameter("@newExam", Convert.ToInt32(ddlNewExam.SelectedValue)),
                                                                        new SqlParameter("@id", ID)
                                                            };
                                sql = "INSERT INTO Certificate_Exam_Application_History(Certificate_Exam_Application_ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Photo_File_Name,Photo,Signature_File_Name,Signature,Left_Thumb_File_Name,Left_Thumb,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,Created_On,Created_By,Remarks_ID,New_Exam_ID)" +
                                    "(select ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Photo_File_Name,Photo,Signature_File_Name,Signature,Left_Thumb_File_Name,Left_Thumb,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,@currentDate,@userID, @remarks, @newExam from Certificate_Exam_Application Where ID=@id)";

                                context.Database.ExecuteSqlCommand(sql, param1);

                                // context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Exam_ID=" + ddlNewExam.SelectedValue + ", Roll_Number = NULL , Exam_Centre_Name = null,	Exam_Centre_Address=null,	Date_of_Exam = null,	Exam_Batch_Number = null,	Reporting_Time = null,	Updated_On = null,	Updated_By = null,  Result_Grade_ID = NULL, Date = '" + DateTime.Now + "' Where ID=" + ID);
                                //December_2024
                                SqlParameter[] param2 = { new SqlParameter("@newExam", ddlNewExam.SelectedValue),
                                                            new SqlParameter("@currentDate", DateTime.Now),
                                                               new SqlParameter("@id", ID)
                                                            };
                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Exam_ID=@newExam, Roll_Number = NULL , Exam_Centre_Name = null,	Exam_Centre_Address=null,	Date_of_Exam = null,	Exam_Batch_Number = null,	Reporting_Time = null,	Updated_On = null,	Updated_By = null,  Result_Grade_ID = NULL, Date = @currentDate Where ID=@id ", param2);


                                if (appl != null)
                                {
                                    StudentList st = new StudentList();
                                    st.Salutation = appl.Salutation;
                                    st.AppDate = appl.ApplicationDate;
                                    st.AppName = appl.Name;
                                    st.AppNo = appl.Number;
                                    st.CourseCode = appl.Course.Name;
                                    st.Email = appl.EmailAddress;
                                    st.ExamName = examName;
                                    st.PreviousExamName = PreviousExamName;
                                    st.Reason = Remarks;
                                    st.Mobileno = appl.MobileNumber;
                                    rejList.Add(st);
                                }
                            }
                            else
                            {
                                Duplicatecount = Duplicatecount + 1;
                                DuplicateAppl.Append(appl.Number + " against " + DuplicateApplNumber + ", ");
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
            ;
            if (count > 0)
            {
                Thread thread = new Thread(() => SentBulkMail(rejList, Convert.ToInt32(Session["UserID"])));
                thread.Start();

                Thread thread1 = new Thread(() => SentBulkSMS(rejList, Convert.ToInt32(Session["UserID"])));
                thread1.Start();
            }
            if (ApaarCount > 0)
            {
                lblError.Text = ApaarCount.ToString() + " Null Apaar Applications are not forwarded.Please update by contacting registration section These are : " + ApaarNull.ToString();
                lblError.Visible = true;
            }
            //added by amit start 18-2-26
            if (dupApaarCnt > 0)
            {
                lblError.Text += dupApaarCnt.ToString() + " Duplicate Apaar Applications are not forwarded " + dupApaarNo.ToString();
                lblError.Visible = true;
            }
            //added by amit end 18-2-26

            if (Duplicatecount > 0)
            {
                lblError.Text += "<br/>" + Duplicatecount.ToString() + " Duplicate Applications are not forwarded. These are : " + DuplicateAppl.ToString();
                lblError.Visible = true;
            }
            if (count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No application is forwarded.');", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('" + count.ToString() + " applications forwarded to " + ddlNewExam.SelectedItem.Text + " Exam successfully');", true);
            ddlRemarks.SelectedValue = "0";
            BindGridView();
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                Label lb0 = (Label)e.Row.Cells[2].FindControl("lblApplicationDate");
                lb0.Text = Convert.ToDateTime(lb0.Text).ToString("dd-MMM-yyyy");
                Label lb1 = (Label)e.Row.Cells[4].FindControl("lblDob");
                lb1.Text = Convert.ToDateTime(lb1.Text).ToString("dd-MMM-yyyy");

                e.Row.Cells[4].Text = GetInitCap(e.Row.Cells[4].Text);
                e.Row.Cells[6].Text = GetInitCap(e.Row.Cells[6].Text);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlexamname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlcourse.SelectedValue);
            Int32 examid = Convert.ToInt32(ddlexamname.SelectedValue);
            BindExamCenter(courseid, examid);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 ccategoryid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        FillFilterCourse(ccategoryid);
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 cid = Convert.ToInt32(ddlcourse.SelectedValue);
        BindExamCycle(cid);
    }
    protected void ddlexamyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlcourse.SelectedValue);
        Int32 examcycleid = Convert.ToInt32(ddlexamcycle.SelectedValue);
        Int32 examyear = Convert.ToInt32(ddlexamyear.SelectedValue);
        BindExamname(courseid, examcycleid, examyear);
    }
    protected void ddlexamcycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlcourse.SelectedValue);
        Int32 examcycleid = Convert.ToInt32(ddlexamcycle.SelectedValue);
        BindExamYear(courseid, examcycleid);

    }
    #endregion

    public class StudentList
    {
        public String AppNo
        {
            get;
            set;
        }
        public DateTime AppDate
        {
            get;
            set;
        }
        public String CourseCode
        {
            get;
            set;
        }
        public String Email
        {
            get;
            set;
        }
        public String ExamName
        {
            get;
            set;
        }
        public String AppName
        {
            get;
            set;
        }
        public String Salutation
        {
            get;
            set;
        }
        public String PreviousExamName
        {
            get;
            set;
        }
        public String Reason
        {
            get;
            set;
        }
        public Int64 Mobileno
        {
            get;
            set;
        }
    }


}