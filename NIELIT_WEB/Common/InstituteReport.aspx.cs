using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Collections;
using System.Collections.Generic;

public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int64 instituteId = 0;
    Int32 ExamId = 0;
    Int32 CourseId = 0;
    Int32 TypeId = 0;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            if (Session["RoleID"] != null)
            {
                currentRoleId = Convert.ToInt32(Session["RoleID"]);
                if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/InstituteCentreFilter.aspx"))
                {
                    Response.Write("Sorry! You don't have rights  to view this page");
                    Response.End();
                }
            }
            if(Session["UserType"]!=null)
                loginUserType = (UserType)Session["UserType"];
            if(Session["EntityID"]!=null)
                entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["instituteId"]) && !String.IsNullOrEmpty(Request.QueryString["ExamId"]) && !String.IsNullOrEmpty(Request.QueryString["CourseId"]) && !String.IsNullOrEmpty(Request.QueryString["TypeId"]))
                {
                    tbl.CssClass = "sample3";
                    tbl.CellPadding = 2;
                    tbl.CellSpacing = 1;
                    tbl.Width = Unit.Percentage(100);
                    ShowData();
                    divReportData.Controls.Add(tbl);
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                    Response.End();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader()
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "#";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            //Added 15 Jan 2019
            if (CourseId == -1)
            {
                TableHeaderCell tcCol1A = new TableHeaderCell();
                tcCol1A.Width = Unit.Percentage(1);
                tcCol1A.Text = "Course";
                tcCol1A.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol1A);
            }
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            //Modifid 15 Jan 2019
            //tcCol2.Text = "CCC No.";
            tcCol2.Text = "Institute No.";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);


            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(25);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Institute Name";
            th.Cells.Add(tcCol3);
            
            TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(15);
                tcCol4.Text = "Regional Centre";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);
            }
            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(14);
            tcCol5.Text = "Candidate Applied";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(12);
            tcCol6.Text = "Candidate Appeared";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(10);
            tcCol7.Text = "Candidate Passed";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            //deep add on 27 july 2018
            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(10);
            tcCol9.Text = "Candidate Absent";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(10);
            tcCol10.Text = "Candidate Failed";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);
            // deep end code on 27 july 2018

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(12);
            tcCol8.Text = "Pass %";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowTableHeader1()
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol0 = new TableHeaderCell();
            //tcCol0.Width = Unit.Percentage(3);
            tcCol0.Text = "#";
            th.Cells.Add(tcCol0);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.Width = Unit.Percentage(30);
            tcCol1.Text = "Name";
            tcCol1.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            //tcCol2.Width = Unit.Percentage(30);
            tcCol2.Text = "Father Name";
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            //tcCol3.Width = Unit.Percentage(30);
            tcCol3.HorizontalAlign = HorizontalAlign.Left;
            tcCol3.Text = "Mother Name";
            th.Cells.Add(tcCol3);

            //TableHeaderCell tcCol4 = new TableHeaderCell();
            //tcCol4.Width = Unit.Percentage(15);
            //tcCol4.Text = "Father Name";
            //tcCol4.HorizontalAlign = HorizontalAlign.Left;
            //th.Cells.Add(tcCol4);

            //TableHeaderCell tcCol5 = new TableHeaderCell();
            //tcCol5.Width = Unit.Percentage(15);
            //tcCol5.Text = "Mother Name";
            //tcCol5.HorizontalAlign = HorizontalAlign.Left;
            //th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol6 = new TableHeaderCell();
            //tcCol6.Width = Unit.Percentage(10);
            //tcCol6.Text = "Date of Birth";
            //tcCol6.HorizontalAlign = HorizontalAlign.Right;
            //th.Cells.Add(tcCol6);

            //TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(10);
            //tcCol7.Text = "Payment Mode";
            //tcCol7.HorizontalAlign = HorizontalAlign.Left;
            //th.Cells.Add(tcCol7);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            //tcCol8.Width = Unit.Percentage(10);
            //tcCol8.Text = "Payment status";
            //th.Cells.Add(tcCol8);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void ShowCertificateExamData(IEnumerable<CertificateExamApplication> application)
    //{
    //    try
    //    {
    //        var app = application.FirstOrDefault();
    //        ShowTableHeader();
    //        int j = 1;

    //        TableRow tr = new TableRow();
    //        tr.CssClass = "gdalternate1";

    //        TableCell tdRow = new TableCell();
    //        tdRow.Width = Unit.Percentage(1);
    //        tdRow.Text = j.ToString();
    //        tdRow.HorizontalAlign = HorizontalAlign.Right;
    //        tr.Cells.Add(tdRow);

    //        TableCell tdRow1 = new TableCell();
    //        tdRow1.Width = Unit.Percentage(5);
    //        string accno = (from a in context.AccreditationDetails
    //                        where a.InstituteID == instituteId && a.CourseID == CourseId
    //                        select a.AccreditationNumber).FirstOrDefault();
    //        if (accno != null)
    //            tdRow1.Text = accno.ToString();
    //        else
    //            tdRow1.Text = "NA";
    //        tdRow1.HorizontalAlign = HorizontalAlign.Right;
    //        tr.Cells.Add(tdRow1);


    //        TableCell tdRow3 = new TableCell();
    //        tdRow3.Width = Unit.Percentage(20);
    //        if (app.Institute.Name != null)
    //            tdRow3.Text = GetInitCap(app.Institute.Name);
    //        tdRow3.HorizontalAlign = HorizontalAlign.Left;
    //        tr.Cells.Add(tdRow3);

    //        TableCell tdRow4 = new TableCell();
    //        tdRow4.Width = Unit.Percentage(10);
    //        if (app.RegionalCenterID.HasValue)
    //            tdRow4.Text = app.RegionalCenter.Name;
    //        tdRow4.HorizontalAlign = HorizontalAlign.Left;
    //        tr.Cells.Add(tdRow4);

    //        HyperLink link = new HyperLink();
    //        link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candapplied";
    //        TableCell tdRow5 = new TableCell();
    //        tdRow5.Width = Unit.Percentage(10);
    //        if (application != null)
    //            link.Text = application.Count().ToString();
    //        else
    //            link.Text = "0";
    //        tdRow5.HorizontalAlign = HorizontalAlign.Right;
    //        tdRow5.Controls.Add(link);
    //        tr.Cells.Add(tdRow5);

    //        Int32 absent = (from r in context.ResultGrades
    //                        where r.CourseCategoryID == 2
    //                        && r.Code.ToUpper().Trim() == "ABS".Trim()
    //                        select r.ID).FirstOrDefault();
    //        HyperLink link1 = new HyperLink();
    //        link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candappeared";
    //        var total2 = application.Where(s => s.ResultGradeID.Value != absent);
    //        TableCell tdRow6 = new TableCell();
    //        tdRow6.Width = Unit.Percentage(14);
    //        if (total2 != null)
    //            link1.Text = total2.Count().ToString();
    //        else
    //            link1.Text = "0";
    //        tdRow6.HorizontalAlign = HorizontalAlign.Right;
    //        tdRow6.Controls.Add(link1);
    //        tr.Cells.Add(tdRow6);

    //        HyperLink link2 = new HyperLink();
    //        link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candpassed";
    //        var paasedcodes = (from r in context.ResultGrades
    //                           where r.CourseCategoryID == 2
    //                           && r.Description.ToUpper().Trim() == "PASS".Trim()
    //                           select r.ID).ToArray();

    //        var totalpassedstudent = application.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Distinct().Count();

    //        TableCell tdRow7 = new TableCell();
    //        tdRow7.Width = Unit.Percentage(14);
    //        if (totalpassedstudent != 0)
    //            link2.Text = totalpassedstudent.ToString();
    //        else
    //            link2.Text = "0";
    //        tdRow7.HorizontalAlign = HorizontalAlign.Right;
    //        tdRow7.Controls.Add(link2);
    //        tr.Cells.Add(tdRow7);

    //        float per = 0;
    //        if (totalpassedstudent != 0 && total2 != null)
    //        {
    //            float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
    //            float totalstudent = Convert.ToSingle(total2.Count().ToString());
    //            if (passedstudent == 0 && totalstudent == 0)
    //            {
    //                per = 0;
    //            }
    //            else
    //            {
    //                float ratio = ((passedstudent) / (totalstudent));
    //                per = ratio * 100;
    //            }
    //        }
    //        TableCell tdRow8 = new TableCell();
    //        tdRow8.Width = Unit.Percentage(14);
    //        tdRow8.Text = per.ToString("F");
    //        tdRow8.HorizontalAlign = HorizontalAlign.Right;
    //        tr.Cells.Add(tdRow8);
    //        tbl.Rows.Add(tr);
    //        j++;
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }

    //}
    //protected void ShowCourseExamData(IEnumerable<CourseExamApplicationDetail> application)
    //{
    //    try
    //    {
    //        var app = application.FirstOrDefault();
    //        ShowTableHeader();
    //        int j = 1;

    //        TableRow tr = new TableRow();
    //        tr.CssClass = "gdalternate1";

    //        TableCell tdRow = new TableCell();
    //        tdRow.Width = Unit.Percentage(1);
    //        tdRow.Text = j.ToString();
    //        tdRow.HorizontalAlign = HorizontalAlign.Right;
    //        tr.Cells.Add(tdRow);

    //        TableCell tdRow1 = new TableCell();
    //        tdRow1.Width = Unit.Percentage(5);
    //        string accno = (from a in context.AccreditationDetails
    //                        where a.InstituteID == instituteId && a.CourseID == CourseId
    //                        select a.AccreditationNumber).FirstOrDefault();
    //        if (accno != null)
    //            tdRow1.Text = accno.ToString();
    //        else
    //            tdRow1.Text = "NA";
    //        tdRow1.HorizontalAlign = HorizontalAlign.Right;
    //        tr.Cells.Add(tdRow1);


    //        TableCell tdRow3 = new TableCell();
    //        tdRow3.Width = Unit.Percentage(20);
    //        if (app.Institute.Name != null)
    //            tdRow3.Text = GetInitCap(app.Institute.Name);
    //        tdRow3.HorizontalAlign = HorizontalAlign.Left;
    //        tr.Cells.Add(tdRow3);

    //        HyperLink link = new HyperLink();
    //        link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candapplied";
    //        TableCell tdRow5 = new TableCell();
    //        tdRow5.Width = Unit.Percentage(10);
    //        var appliedCandidates = application.Select(t => t.CandidateID).Distinct().Count();
    //        if (appliedCandidates != 0)
    //            link.Text = appliedCandidates.ToString();
    //        else
    //            link.Text = "0";
    //        tdRow5.HorizontalAlign = HorizontalAlign.Right;
    //        tdRow5.Controls.Add(link);
    //        tr.Cells.Add(tdRow5);


    //        Int32 absent = (from r in context.ResultGrades
    //                        where r.CourseCategoryID == 1
    //                        && r.Code.ToUpper().Trim() == "ABS".Trim()
    //                        select r.ID).FirstOrDefault();
    //        HyperLink link1 = new HyperLink();
    //        link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candappeared";
    //        var total2 = application.Where(s => s.ResultGradeID.Value != absent).Select(t => t.CandidateID).Distinct();
    //        TableCell tdRow6 = new TableCell();
    //        tdRow6.Width = Unit.Percentage(14);
    //        if (total2 != null)
    //            link1.Text = total2.Count().ToString();
    //        else
    //            link1.Text = "0";
    //        tdRow6.HorizontalAlign = HorizontalAlign.Right;
    //        tdRow6.Controls.Add(link1);
    //        tr.Cells.Add(tdRow6);

    //        HyperLink link2 = new HyperLink();
    //        link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candpassed";
    //        var paasedcodes = (from r in context.ResultGrades
    //                           where r.CourseCategoryID == 1
    //                           && r.Description.ToUpper().Trim() == "PASS".Trim()
    //                           select r.ID).ToArray();

    //        var totalpassedstudent = application.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Select(t => t.CandidateID).Distinct().Count();
    //        //Int32 totalpassedstudent = 0;
    //        //for (int i = 0; i < paasedcodes.Length; i++)
    //        //{
    //        // var total1 = application.Where(s => s.ResultGradeID.Value == paasedcodes[i]).Select(t=>t.CandidateID).Distinct().Count();
    //        // totalpassedstudent += total1;
    //        //}

    //        TableCell tdRow7 = new TableCell();
    //        tdRow7.Width = Unit.Percentage(14);
    //        if (totalpassedstudent != 0)
    //            link2.Text = totalpassedstudent.ToString();
    //        else
    //            link2.Text = "0";
    //        tdRow7.HorizontalAlign = HorizontalAlign.Right;
    //        tdRow7.Controls.Add(link2);
    //        tr.Cells.Add(tdRow7);

    //        float per = 0;
    //        if (totalpassedstudent != 0 && total2 != null)
    //        {
    //            float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
    //            float totalstudent = Convert.ToSingle(total2.Count().ToString());
    //            if (passedstudent == 0 && totalstudent == 0)
    //            {
    //                per = 0;
    //            }
    //            else
    //            {
    //                float ratio = ((passedstudent) / (totalstudent));
    //                per = ratio * 100;
    //            }
    //        }
    //        TableCell tdRow8 = new TableCell();
    //        tdRow8.Width = Unit.Percentage(14);
    //        tdRow8.Text = per.ToString("F");
    //        tdRow8.HorizontalAlign = HorizontalAlign.Right;
    //        tr.Cells.Add(tdRow8);
    //        tbl.Rows.Add(tr);
    //        j++;
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }

    //}
    protected void ShowData()
    {
        try
        {
            context = new EConnectContext();
            CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);

            ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            DateTime? DateFrom = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime? DateTo = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DateTime NullDate = Convert.ToDateTime("1/1/0001");
            instituteId = Convert.ToInt64(Request.QueryString["instituteId"]);
            Institute institute = context.Institutes.Find(instituteId);
			
			 //Added 9 April 2019
            Int64 gtCandApplied, gtCandAppeared, gtCandPassed, gtCandFailed, gtCandAbsent,gTotal2;

            gtCandAppeared = gtCandApplied = gtCandPassed = gtCandFailed = gtCandAbsent =gTotal2 = 0;
            //
            if (CourseId == -1)
            {

                IEnumerable<int> inq = (from c in context.Courses
                                        where c.CourseCategoryID == 2
                                        select c.ID);

                int[] x = inq.ToArray();
                ShowTableHeader();

                for (int i = 0; i < x.Count(); i++)
                {
                    context = new EConnectContext();
                    CourseId = x[i];
                    var courseDetail = context.Courses.Find(CourseId);
                    int CourseCatgId = courseDetail.CourseCategoryID;
                    TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
                    
                    //Added 15 jan 2019
                    var instAccrDetails=institute.AccreditationDetails.Where(s => s.InstituteID  == instituteId && CourseId ==s.CourseID ).FirstOrDefault();
                    if (instAccrDetails == null)
                        continue;
                    //
                    //String AccrNo = institute.AccreditationDetails.Where(s => s.CourseID == CourseId).FirstOrDefault().AccreditationNumber;
                    String AccrNo = institute.AccreditationDetails.Where(s => s.InstituteID  == instituteId && CourseId ==s.CourseID ).FirstOrDefault().AccreditationNumber;
                    string strHead = "";
                    if (ExamId != 0 && institute != null)
                    {
                        Exam exam = context.Exams.Find(ExamId);

                        strHead += "</br><b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                        strHead += "</br><b>Course : </b>" + exam.Course.Name + ", <b> Exam Name : </b>" + exam.Name;
                        strHead += "<br/><b>Institute Name : </b>(" + AccrNo + ") " + institute.Name;
                    }
                    else if (ExamId == 0 && DateFrom.GetValueOrDefault() != NullDate)
                    {
                        strHead += "<br/><b>Institute Name : </b>(" + AccrNo + ") " + institute.Name;
                        //Commented 15 Jan 2019
                       // strHead += "<br/><b>Course : </b>" + courseDetail.Name;
                        strHead += "<br/><b>From : </b>" + DateFrom.GetValueOrDefault().ToString("dd-MMM-yyyy") + " <b>To : </b>" + DateTo.GetValueOrDefault().ToString("dd-MMM-yyyy");

                    }
                    else { }

                    if (CourseId != 0 && TypeId != 0)
                    {
                        #region-------Certificate-Exam-Data----------
                        if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        {
                            using (context = new EConnectContext())
                            {//Added 11 Jan 2019
                                IQueryable<MyType> application;
                                if (CourseId == -1)
                                {
                                    application = (from a in context.CertificateExamApplications.AsNoTracking()
                                                   //  where a.CourseID == CourseId && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   where a.CourseCategoryID == 2 && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   select new MyType
                                                   {
                                                       ID = a.ID,
                                                       InstituteID = a.InstituteID,
                                                       RegionalCenterID = a.RegionalCenterID,
                                                       InstituteName = a.Institute.Name,
                                                       RegionalCenterName = a.RegionalCenter.Name,
                                                       ExamId = a.ExamID,
                                                       ResultGradeID = a.ResultGradeID,
                                                       ExamDate = a.DateOfExam
                                                   });
                                }
                                else
                                {
                                    application = (from a in context.CertificateExamApplications.AsNoTracking()
                                                   //  where a.CourseID == CourseId && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   where a.CourseID == CourseId && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   select new MyType
                                                    {
                                                        ID = a.ID,
                                                        InstituteID = a.InstituteID,
                                                        RegionalCenterID = a.RegionalCenterID,
                                                        InstituteName = a.Institute.Name,
                                                        RegionalCenterName = a.RegionalCenter.Name,
                                                        ExamId = a.ExamID,
                                                        ResultGradeID = a.ResultGradeID,
                                                        ExamDate = a.DateOfExam
                                                    });
                                }
                                if (ExamId != 0)
                                { application = application.Where(s => s.ExamId == ExamId); }
                                if (DateFrom != NullDate)
                                { application = application.Where(s => s.ExamDate >= DateFrom && s.ExamDate < DateTo); }
                                if (loginUserType == UserType.Institute)
                                { application = application.Where(s => s.InstituteID == entityID); }
                                if (loginUserType == UserType.RegionalCenter)
                                { application = application.Where(s => s.RegionalCenterID == entityID); }

                                //if (application.Count() > 0)
                                //{
                                //var app = application.FirstOrDefault();
                                var resultGrade = context.ResultGrades.Where(s => s.CourseCategoryID == CourseCatgId);
                             //   ShowTableHeader();
                                int j = i+1;

                                TableRow tr = new TableRow();
                                tr.CssClass = "gdalternate1";

                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = j.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                //Added 15 Jan 2019
                               

                                TableCell tdRow1a = new TableCell();
                                tdRow1a.Width = Unit.Percentage(1);
                                tdRow1a.Text = courseDetail .Name ;
                                tdRow1a.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow1a);
                                //



                                TableCell tdRow1 = new TableCell();
                                tdRow1.Width = Unit.Percentage(5);
                                tdRow1.Text = AccrNo.ToString();
                                tdRow1.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow1);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(20);
                                tdRow3.Text = GetInitCap(institute.Name);
                                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow3);

                                if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                                {
                                    TableCell tdRow4 = new TableCell();
                                    tdRow4.Width = Unit.Percentage(10);
                                    tdRow4.Text = (application.Count() != 0) ? GetInitCap(application.FirstOrDefault().RegionalCenterName) : "NA";
                                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow4);
                                }

                                HyperLink link = new HyperLink();
                                //link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candapplied";
                                // for date range display of Applied candidates list  
                                link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candapplied";

                                TableCell tdRow5 = new TableCell();
                                tdRow5.Width = Unit.Percentage(10);
                                link.Text = (application.Count() != 0) ? application.Count().ToString() : "0";
                                tdRow5.HorizontalAlign = HorizontalAlign.Right;
                                tdRow5.Controls.Add(link);
                                tr.Cells.Add(tdRow5);

								 //Added 9 April 2019
                                gtCandApplied =gtCandApplied + ((application.Count() != 0) ? application.Count() : 0);
                                //
								
                                var absent = resultGrade.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                HyperLink link1 = new HyperLink();
                                //link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candappeared";
                                // for date range display of appeared candidates list 
                                link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candappeared";

                                int total2 = (application.Count() != 0) ? application.Where(s => !absent.Contains(s.ResultGradeID.Value)).Count() : 0;
                                TableCell tdRow6 = new TableCell();
                                tdRow6.Width = Unit.Percentage(14);
                                link1.Text = (total2 != 0) ? total2.ToString() : "0";
                                tdRow6.HorizontalAlign = HorizontalAlign.Right;
                                tdRow6.Controls.Add(link1);
                                tr.Cells.Add(tdRow6);

								//Added 9 April 2019
                                gTotal2 = gTotal2 + total2;
                                ///
								
                                HyperLink link2 = new HyperLink();
                                // link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candpassed";
                                // for date range display of Passed candidates list    
                                link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candpassed";

                                var paasedcodes = resultGrade.Where(s => s.Description.Equals("PASS", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                int totalpassedstudent = (application.Count() != 0) ? application.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Distinct().Count() : 0;
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(14);
                                link2.Text = (totalpassedstudent != 0) ? totalpassedstudent.ToString() : "0";
                                tdRow7.HorizontalAlign = HorizontalAlign.Right;
                                tdRow7.Controls.Add(link2);
                                tr.Cells.Add(tdRow7);

								 //Added 9 April 2019
                                gtCandPassed = gtCandPassed + totalpassedstudent;
                                //
								
                                //deep add code on 27 july 2018
                                HyperLink link3 = new HyperLink();
                                //link3.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candabsent";
                                // for date range display of Absent candidates list    
                                link3.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candabsent";

                                var absentcodes = resultGrade.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                int totalabsentstudent = (application.Count() != 0) ? application.Where(s => absentcodes.Contains(s.ResultGradeID.Value)).Distinct().Count() : 0;
                                TableCell tdRow9 = new TableCell();
                                tdRow9.Width = Unit.Percentage(14);
                                link3.Text = (totalabsentstudent != 0) ? totalabsentstudent.ToString() : "0";
                                tdRow9.HorizontalAlign = HorizontalAlign.Right;
                                tdRow9.Controls.Add(link3);
                                tr.Cells.Add(tdRow9);

								 //Added 9 April 2019
                                gtCandAbsent = gtCandAbsent + totalabsentstudent;
                                //
								
                                HyperLink link4 = new HyperLink();
                                // link4.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candfail";
                                // for date range display of Failed candidates list 
                                link4.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candfail";

                                var failcodes = resultGrade.Where(s => s.Description.Equals("Fail", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                int totalfailstudent = (application.Count() != 0) ? application.Where(s => failcodes.Contains(s.ResultGradeID.Value)).Distinct().Count() : 0;
                                TableCell tdRow10 = new TableCell();
                                tdRow10.Width = Unit.Percentage(14);
                                link4.Text = (totalfailstudent != 0) ? totalfailstudent.ToString() : "0";
                                tdRow10.HorizontalAlign = HorizontalAlign.Right;
                                tdRow10.Controls.Add(link4);
                                tr.Cells.Add(tdRow10);

								//Added 9 April 2019
                                gtCandFailed = gtCandFailed + totalfailstudent;
                                //
								
                                //deep end code on 27 july 2018

                                float per = (totalpassedstudent != 0 && total2 != 0) ? ((Convert.ToSingle(totalpassedstudent) * 100) / Convert.ToSingle(total2)) : 0;
                                TableCell tdRow8 = new TableCell();
                                tdRow8.Width = Unit.Percentage(14);
                                tdRow8.Text = per.ToString("F");
                                tdRow8.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow8);
                                tbl.Rows.Add(tr);
                                j++;
                                //}
                                //else
                                //{
                                //    lblError.Visible = true;
                                //    lblError.Text = "No Record Found";
                                //}
                            };
                        }
                        #endregion---------------

                       
                    }
                    LblRptSubHeader.Text = strHead;
                }
				
				//Added 9 April 2019
                TableRow trGrand = new TableRow();
                trGrand.CssClass = "gdalternate1";

                TableCell tdRowG1 = new TableCell();
                tdRowG1.Width = Unit.Percentage(1);
                tdRowG1.Text =  " ";
                tdRowG1.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG1);

                TableCell tdRowG2 = new TableCell();
                tdRowG2.Width = Unit.Percentage(1);
                tdRowG2.Text = " ";
                tdRowG2.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG2);

                TableCell tdRowG3 = new TableCell();
                tdRowG3.Width = Unit.Percentage(1);
                tdRowG3.Text = " ";
                tdRowG3.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG3);

                TableCell tdRowG4 = new TableCell();
                tdRowG4.Width = Unit.Percentage(1);
                tdRowG4.Text = "Totals";
                tdRowG4.HorizontalAlign = HorizontalAlign.Center;
                trGrand.Cells.Add(tdRowG4);

                TableCell tdRowG5 = new TableCell();
                tdRowG5.Width = Unit.Percentage(1);
                tdRowG5.Text = "----->";
                tdRowG5.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG5);

                TableCell tdRowG6 = new TableCell();
                tdRowG6.Width = Unit.Percentage(1);
                tdRowG6.Text = gtCandApplied .ToString ();
                tdRowG6.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG6);

                TableCell tdRowG7 = new TableCell();
                tdRowG7.Width = Unit.Percentage(1);
                tdRowG7.Text = gTotal2.ToString();
                tdRowG7.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG7);

                TableCell tdRowG8 = new TableCell();
                tdRowG8.Width = Unit.Percentage(1);
                tdRowG8.Text = gtCandPassed.ToString();
                tdRowG8.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG8);


                TableCell tdRowG9 = new TableCell();
                tdRowG9.Width = Unit.Percentage(1);
                tdRowG9.Text =gtCandAbsent.ToString();
                tdRowG9.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG9);

                
                TableCell tdRowG10 = new TableCell();
                tdRowG10.Width = Unit.Percentage(14);
                tdRowG10.Text = gtCandFailed.ToString();
                tdRowG10.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG10);


                float gPer = (gtCandPassed  != 0 && gTotal2  != 0) ? ((Convert.ToSingle(gtCandPassed ) * 100) / Convert.ToSingle(gTotal2 )) : 0;
                TableCell tdRowG11 = new TableCell();
                tdRowG11.Width = Unit.Percentage(14);
                tdRowG11.Text = gPer.ToString("F");
                tdRowG11.HorizontalAlign = HorizontalAlign.Right;
                trGrand.Cells.Add(tdRowG11);
                tbl.Rows.Add(trGrand);
                //
            }

            else
            {
                 var courseDetail = context.Courses.Find(CourseId);
                    int CourseCatgId = courseDetail.CourseCategoryID;
                    TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
                  //  CourseId = x[i];
                  //  Institute institute = context.Institutes.Find(instituteId);
                    String AccrNo = institute.AccreditationDetails.Where(s => s.CourseID == CourseId).FirstOrDefault().AccreditationNumber;
                    string strHead = "";
                    if (ExamId != 0 && institute != null)
                    {
                        Exam exam = context.Exams.Find(ExamId);

                        strHead += "</br><b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                        strHead += "</br><b>Course : </b>" + exam.Course.Name + ", <b> Exam Name : </b>" + exam.Name;
                        strHead += "<br/><b>Institute Name : </b>(" + AccrNo + ") " + institute.Name;
                    }
                    else if (ExamId == 0 && DateFrom.GetValueOrDefault() != NullDate)
                    {
                        strHead += "<br/><b>Institute Name : </b>(" + AccrNo + ") " + institute.Name;
                        strHead += "<br/><b>Course : </b>" + courseDetail.Name;
                        strHead += "<br/><b>From : </b>" + DateFrom.GetValueOrDefault().ToString("dd-MMM-yyyy") + " <b>To : </b>" + DateTo.GetValueOrDefault().ToString("dd-MMM-yyyy");

                    }
                    else { }

                    if (CourseId != 0 && TypeId != 0)
                    {
                        #region-------Certificate-Exam-Data----------
                        if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        {
                            using (context = new EConnectContext())
                            {//Added 11 Jan 2019
                                IQueryable<MyType> application;
                                if (CourseId == -1)
                                {
                                    application = (from a in context.CertificateExamApplications.AsNoTracking()
                                                   //  where a.CourseID == CourseId && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   where a.CourseCategoryID == 2 && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   select new MyType
                                                   {
                                                       ID = a.ID,
                                                       InstituteID = a.InstituteID,
                                                       RegionalCenterID = a.RegionalCenterID,
                                                       InstituteName = a.Institute.Name,
                                                       RegionalCenterName = a.RegionalCenter.Name,
                                                       ExamId = a.ExamID,
                                                       ResultGradeID = a.ResultGradeID,
                                                       ExamDate = a.DateOfExam
                                                   });
                                }
                                else
                                {
                                    application = (from a in context.CertificateExamApplications.AsNoTracking()
                                                   //  where a.CourseID == CourseId && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   where a.CourseID == CourseId && a.ResultGradeID != null && a.InstituteID == instituteId
                                                   select new MyType
                                                    {
                                                        ID = a.ID,
                                                        InstituteID = a.InstituteID,
                                                        RegionalCenterID = a.RegionalCenterID,
                                                        InstituteName = a.Institute.Name,
                                                        RegionalCenterName = a.RegionalCenter.Name,
                                                        ExamId = a.ExamID,
                                                        ResultGradeID = a.ResultGradeID,
                                                        ExamDate = a.DateOfExam
                                                    });
                                }
                                if (ExamId != 0)
                                { application = application.Where(s => s.ExamId == ExamId); }
                                if (DateFrom != NullDate)
                                { application = application.Where(s => s.ExamDate >= DateFrom && s.ExamDate < DateTo); }
                                if (loginUserType == UserType.Institute)
                                { application = application.Where(s => s.InstituteID == entityID); }
                                if (loginUserType == UserType.RegionalCenter)
                                { application = application.Where(s => s.RegionalCenterID == entityID); }

                                //if (application.Count() > 0)
                                //{
                                //var app = application.FirstOrDefault();
                                var resultGrade = context.ResultGrades.Where(s => s.CourseCategoryID == CourseCatgId);
                                ShowTableHeader();
                                int j = 1;

                                TableRow tr = new TableRow();
                                tr.CssClass = "gdalternate1";

                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = j.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow1 = new TableCell();
                                tdRow1.Width = Unit.Percentage(5);
                                tdRow1.Text = AccrNo.ToString();
                                tdRow1.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow1);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(20);
                                tdRow3.Text = GetInitCap(institute.Name);
                                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow3);

                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(10);
                                tdRow4.Text = (application.Count() != 0) ? GetInitCap(application.FirstOrDefault().RegionalCenterName) : "NA";
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);

                                HyperLink link = new HyperLink();
                                //link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candapplied";
                                // for date range display of Applied candidates list  
                                link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candapplied";

                                TableCell tdRow5 = new TableCell();
                                tdRow5.Width = Unit.Percentage(10);
                                link.Text = (application.Count() != 0) ? application.Count().ToString() : "0";
                                tdRow5.HorizontalAlign = HorizontalAlign.Right;
                                tdRow5.Controls.Add(link);
                                tr.Cells.Add(tdRow5);

                                var absent = resultGrade.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                HyperLink link1 = new HyperLink();
                                //link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candappeared";
                                // for date range display of appeared candidates list 
                                link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candappeared";

                                int total2 = (application.Count() != 0) ? application.Where(s => !absent.Contains(s.ResultGradeID.Value)).Count() : 0;
                                TableCell tdRow6 = new TableCell();
                                tdRow6.Width = Unit.Percentage(14);
                                link1.Text = (total2 != 0) ? total2.ToString() : "0";
                                tdRow6.HorizontalAlign = HorizontalAlign.Right;
                                tdRow6.Controls.Add(link1);
                                tr.Cells.Add(tdRow6);

                                HyperLink link2 = new HyperLink();
                                // link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candpassed";
                                // for date range display of Passed candidates list    
                                link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candpassed";

                                var paasedcodes = resultGrade.Where(s => s.Description.Equals("PASS", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                int totalpassedstudent = (application.Count() != 0) ? application.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Distinct().Count() : 0;
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(14);
                                link2.Text = (totalpassedstudent != 0) ? totalpassedstudent.ToString() : "0";
                                tdRow7.HorizontalAlign = HorizontalAlign.Right;
                                tdRow7.Controls.Add(link2);
                                tr.Cells.Add(tdRow7);

                                //deep add code on 27 july 2018
                                HyperLink link3 = new HyperLink();
                                //link3.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candabsent";
                                // for date range display of Absent candidates list    
                                link3.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candabsent";

                                var absentcodes = resultGrade.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                int totalabsentstudent = (application.Count() != 0) ? application.Where(s => absentcodes.Contains(s.ResultGradeID.Value)).Distinct().Count() : 0;
                                TableCell tdRow9 = new TableCell();
                                tdRow9.Width = Unit.Percentage(14);
                                link3.Text = (totalabsentstudent != 0) ? totalabsentstudent.ToString() : "0";
                                tdRow9.HorizontalAlign = HorizontalAlign.Right;
                                tdRow9.Controls.Add(link3);
                                tr.Cells.Add(tdRow9);

                                HyperLink link4 = new HyperLink();
                                // link4.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candfail";
                                // for date range display of Failed candidates list 
                                link4.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&courseCatId=" + CourseCatgId + "&examid=" + ExamId + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&instituteid=" + instituteId + "&src=candfail";

                                var failcodes = resultGrade.Where(s => s.Description.Equals("Fail", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                                int totalfailstudent = (application.Count() != 0) ? application.Where(s => failcodes.Contains(s.ResultGradeID.Value)).Distinct().Count() : 0;
                                TableCell tdRow10 = new TableCell();
                                tdRow10.Width = Unit.Percentage(14);
                                link4.Text = (totalfailstudent != 0) ? totalfailstudent.ToString() : "0";
                                tdRow10.HorizontalAlign = HorizontalAlign.Right;
                                tdRow10.Controls.Add(link4);
                                tr.Cells.Add(tdRow10);

                                //deep end code on 27 july 2018

                                float per = (totalpassedstudent != 0 && total2 != 0) ? ((Convert.ToSingle(totalpassedstudent) * 100) / Convert.ToSingle(total2)) : 0;
                                TableCell tdRow8 = new TableCell();
                                tdRow8.Width = Unit.Percentage(14);
                                tdRow8.Text = per.ToString("F");
                                tdRow8.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow8);
                                tbl.Rows.Add(tr);
                                j++;
                                //}
                                //else
                                //{
                                //    lblError.Visible = true;
                                //    lblError.Text = "No Record Found";
                                //}
                            };
                        }
                        #endregion---------------

                        #region--------Course-Exam-Data---------
                        else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            using (context = new EConnectContext())
                            {

                                var application = (from a in context.CourseExamApplicationDetails
                                                   where a.ExamID == ExamId && a.CourseID == CourseId && a.InstituteID == instituteId && a.ResultGradeID != null
                                                   select new
                                                   {
                                                       ID = a.ID,
                                                       InstituteID = a.InstituteID,
                                                       InstituteName = a.Institute.Name,
                                                       CandidateID = a.CandidateID,
                                                       ResultGradeID = a.ResultGradeID,
                                                   }).ToList();

                                if (loginUserType == UserType.Institute)
                                    application = application.Where(s => s.InstituteID == entityID).ToList();
                                if (application.Count() > 0)
                                {
                                    //ShowCourseExamData(application);
                                    var app = application.FirstOrDefault();
                                    ShowTableHeader();
                                    int j = 1;

                                    TableRow tr = new TableRow();
                                    tr.CssClass = "gdalternate1";

                                    TableCell tdRow = new TableCell();
                                    tdRow.Width = Unit.Percentage(1);
                                    tdRow.Text = j.ToString();
                                    tdRow.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(tdRow);

                                    TableCell tdRow1 = new TableCell();
                                    tdRow1.Width = Unit.Percentage(5);
                                    //string AccrNo = (from a in context.AccreditationDetails where a.InstituteID == instituteId && a.CourseID == CourseId select a.AccreditationNumber).FirstOrDefault();
                                    if (AccrNo != null)
                                        tdRow1.Text = AccrNo.ToString();
                                    else
                                        tdRow1.Text = "NA";
                                    tdRow1.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(tdRow1);


                                    TableCell tdRow3 = new TableCell();
                                    tdRow3.Width = Unit.Percentage(20);
                                    if (app.InstituteName != null)
                                        tdRow3.Text = GetInitCap(app.InstituteName);
                                    tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow3);

                                    HyperLink link = new HyperLink();
                                    link.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candapplied";
                                    TableCell tdRow5 = new TableCell();
                                    tdRow5.Width = Unit.Percentage(10);
                                    var appliedCandidates = application.Select(t => t.CandidateID).Distinct().Count();
                                    if (appliedCandidates != 0)
                                        link.Text = appliedCandidates.ToString();
                                    else
                                        link.Text = "0";
                                    tdRow5.HorizontalAlign = HorizontalAlign.Right;
                                    tdRow5.Controls.Add(link);
                                    tr.Cells.Add(tdRow5);


                                    Int32 absent = (from r in context.ResultGrades
                                                    where r.CourseCategoryID == 1
                                                    && r.Code.ToUpper().Trim() == "ABS".Trim()
                                                    select r.ID).FirstOrDefault();
                                    HyperLink link1 = new HyperLink();
                                    link1.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candappeared";
                                    var total2 = application.Where(s => s.ResultGradeID.Value != absent).Select(t => t.CandidateID).Distinct();
                                    TableCell tdRow6 = new TableCell();
                                    tdRow6.Width = Unit.Percentage(14);
                                    if (total2 != null)
                                        link1.Text = total2.Count().ToString();
                                    else
                                        link1.Text = "0";
                                    tdRow6.HorizontalAlign = HorizontalAlign.Right;
                                    tdRow6.Controls.Add(link1);
                                    tr.Cells.Add(tdRow6);

                                    HyperLink link2 = new HyperLink();
                                    link2.NavigateUrl = "../Common/InstituteStudentList.aspx?typeid=" + TypeId + "&courseid=" + CourseId + "&examid=" + ExamId + "&instituteid=" + instituteId + "&src=candpassed";
                                    var paasedcodes = (from r in context.ResultGrades
                                                       where r.CourseCategoryID == 1
                                                       && r.Description.ToUpper().Trim() == "PASS".Trim()
                                                       select r.ID).ToArray();

                                    var totalpassedstudent = application.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Select(t => t.CandidateID).Distinct().Count();

                                    TableCell tdRow7 = new TableCell();
                                    tdRow7.Width = Unit.Percentage(14);
                                    if (totalpassedstudent != 0)
                                        link2.Text = totalpassedstudent.ToString();
                                    else
                                        link2.Text = "0";
                                    tdRow7.HorizontalAlign = HorizontalAlign.Right;
                                    tdRow7.Controls.Add(link2);
                                    tr.Cells.Add(tdRow7);

                                    float per = 0;
                                    if (totalpassedstudent != 0 && total2 != null)
                                    {
                                        float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
                                        float totalstudent = Convert.ToSingle(total2.Count().ToString());
                                        if (passedstudent == 0 && totalstudent == 0)
                                        {
                                            per = 0;
                                        }
                                        else
                                        {
                                            float ratio = ((passedstudent) / (totalstudent));
                                            per = ratio * 100;
                                        }
                                    }
                                    TableCell tdRow8 = new TableCell();
                                    tdRow8.Width = Unit.Percentage(14);
                                    tdRow8.Text = per.ToString("F");
                                    tdRow8.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(tdRow8);
                                    tbl.Rows.Add(tr);
                                    j++;
                                }
                                else
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "No Record Found";
                                }
                            };
                        }
                        #endregion--------------
                    }
                    LblRptSubHeader.Text = strHead;
                }
    
    
    

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }

    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
}
public partial class MyType
{
    public long ID { get; set; }
    public long? InstituteID { get; set; }
    public int? RegionalCenterID { get; set; }
    public string InstituteName { get; set; }
    public string RegionalCenterName { get; set; }
    public int ExamId { get; set; }
    public int? ResultGradeID { get; set; }
    public DateTime? ExamDate { get; set; }
}